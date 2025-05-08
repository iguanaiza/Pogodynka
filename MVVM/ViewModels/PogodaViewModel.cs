using Microsoft.Maui.ApplicationModel;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.Controls;
using Pogodynka.MVVM.Models;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;
using Pogodynka.Resources.Strings;

namespace Pogodynka.MVVM.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class PogodaViewModel
    {
        #region Initial variables
        public PogodaData? PogodaData { get; set; }
        public string? PlaceName { get; set; }
        public string? PlaceNameFound { get; set; } //po znalezieniu miejscowości, podmiana wyszukiwanej nazwy na realną nazwę
        public DateTime DateNow { get; set; } = DateTime.Now;
        public bool IsVisible { get; set; } = false; //widocznosc detali - na start pusto, tylko wyszukiwarka
        public bool IsLoading { get; set; } //widoczność ActivityIndicator
        private bool isBusy; //pomocnicze aby usunąć problem z podwójnym wywołaniem searchbara (2x komunikat się pojawiał)
        private readonly HttpClient HttpClient;
        #endregion

        public PogodaViewModel()
        {
            //inicjacja klienta do api
            HttpClient = new HttpClient();
        }

        #region Get current location
        //Aktywacja tapnięcia ikony GPS
        public ICommand UseDeviceLocationCommand => new Command(async () => await GetWeatherFromDeviceLocationAsync());

        //Pobranie lokacji z GPS
        private async Task GetWeatherFromDeviceLocationAsync()
        {
            try
            {
                var status = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

                if (status != PermissionStatus.Granted)
                    status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();

                if (status == PermissionStatus.Granted)
                {
                    var location = await Geolocation.GetLastKnownLocationAsync()
                                   ?? await Geolocation.GetLocationAsync(new GeolocationRequest(GeolocationAccuracy.Medium));

                    if (location != null)
                    {
                        var placemarks = await Geocoding.Default.GetPlacemarksAsync(location);
                        var placemark = placemarks?.FirstOrDefault();

                        var autoPlaceName = placemark?.Locality ?? placemark?.SubAdminArea ?? placemark?.AdminArea;

                        if (!string.IsNullOrWhiteSpace(autoPlaceName))
                        {
                            PlaceNameFound = autoPlaceName;

                            //Wrzucenie w wyszukiwarkę i uruchomienie komendy przypisanej do wyszukiwarki
                            if (SearchCommand.CanExecute(autoPlaceName))
                                SearchCommand.Execute(autoPlaceName);
                        }
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert(AppResources.alertLocationMessage, AppResources.alertLocationMessage, "OK");
                    }
                }
                else
                {
                    await Shell.Current.DisplayAlert(AppResources.alertNoAccessTitle, AppResources.alertNoAccessMessage, "OK");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert(AppResources.errorTitle, ex.Message, "OK");
            }
        }
        #endregion

        #region Search location
        //Wyszukiwanie lokalizacji przez wyszukiwarkę
        public ICommand SearchCommand => new Command(async (searchText) =>
        {
            if (isBusy)
                return;

            isBusy = true;

            try
            {
                PlaceName = searchText?.ToString();
                var location = await GetCoordinatesAsync(PlaceName);

                if (location == null)
                {
                    await Shell.Current.DisplayAlert(AppResources.alertNotFoundTitle, AppResources.alertNotFoundMessage, "OK");
                    return;
                }

                var placemarks = await Geocoding.Default.GetPlacemarksAsync(location); //pobranie prawdziwej nazwy lokacji
                var placemark = placemarks?.FirstOrDefault();
                PlaceNameFound = placemark.Locality ?? placemark.SubAdminArea ?? placemark.AdminArea ?? PlaceName;

                await GetWeather(location);
            }
            finally
            {
                isBusy = false;
            }
        });

        //Pobranie współrzędnych znalezionej lokalizacji
        private static async Task<Location> GetCoordinatesAsync(string address)
        {
            IEnumerable<Location> locations = await Geocoding.Default.GetLocationsAsync(address);
            Location? location = locations?.FirstOrDefault();

            return location;
        }
        #endregion

        #region Get weather info
        //Pobieranie informacji z API
        private async Task GetWeather(Location location)
        {
           string latitude = location.Latitude.ToString(CultureInfo.InvariantCulture); //konwersja formatu liczbowego PL na EN
           string longitude = location.Longitude.ToString(CultureInfo.InvariantCulture); //konwersja formatu liczbowego PL na EN

           var url = $"https://api.open-meteo.com/v1/forecast?latitude={latitude}&longitude={longitude}&daily=weather_code,apparent_temperature_max,apparent_temperature_min,pressure_msl_mean,winddirection_10m_dominant,wind_speed_10m_mean,relative_humidity_2m_mean&current=apparent_temperature,is_day,relative_humidity_2m,weather_code,pressure_msl,wind_speed_10m,wind_direction_10m&timezone=auto";
           IsLoading = true; //widoczność ActivityIndicator
           var response = await HttpClient.GetAsync(url);

           if (response.IsSuccessStatusCode)
           {
               using (var responseStream = await response.Content.ReadAsStreamAsync())
               {
                   var data = await JsonSerializer.DeserializeAsync<PogodaData>(responseStream);
                   PogodaData = data;

                   for (int i = 1; i < PogodaData.daily.time.Length-1; i++) //pobieranie danych dla każdego kolejnego dnia z osobna (5 kolejnych dni, API pobiera 7 z bieżącym włącznie)
                   {
                       var daily_single = new Daily_Single
                       {
                           time = PogodaData.daily.time[i],
                           weather_code = PogodaData.daily.weather_code[i],
                           apparent_temperature_min = PogodaData.daily.apparent_temperature_min[i],
                           apparent_temperature_max = PogodaData.daily.apparent_temperature_max[i],
                           pressure_msl_mean = PogodaData.daily.pressure_msl_mean[i],
                           winddirection_10m_dominant = PogodaData.daily.winddirection_10m_dominant[i],
                           wind_speed_10m_mean = PogodaData.daily.wind_speed_10m_mean[i],
                           relative_humidity_2m_mean = PogodaData.daily.relative_humidity_2m_mean[i]
                       };

                       PogodaData.daily_single.Add(daily_single);
                   }
               }
           }

            else
            {
                await Shell.Current.DisplayAlert(AppResources.errorTitle, AppResources.errorHttpMessage, "OK");
            }

            IsLoading = false; //widoczność ActivityIndicator
            IsVisible = true; //widoczność detali
        }
        #endregion
    }
}