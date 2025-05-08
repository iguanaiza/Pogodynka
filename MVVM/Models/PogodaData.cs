using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pogodynka.MVVM.Models
{
    #region API data
    //dane utworzone na podstawie API (Edit->Paste Special->Paste JSON as classes), z poprawkami wg docs
    public class PogodaData
    {
        public float latitude { get; set; }
        public float longitude { get; set; }
        public float generationtime_ms { get; set; }
        public int utc_offset_seconds { get; set; }
        public string timezone { get; set; }
        public string timezone_abbreviation { get; set; }
        public float elevation { get; set; }
        public Current current { get; set; }
        public Daily daily { get; set; }
        public ObservableCollection<Daily_Single> daily_single { get; set;} = new ObservableCollection<Daily_Single>();
    }

    public class Current
    {
        public string time { get; set; }
        public int interval { get; set; }
        public float apparent_temperature { get; set; }
        public int is_day { get; set; }
        public int relative_humidity_2m { get; set; }
        public int weather_code { get; set; }
        public float pressure_msl { get; set; }
        public float wind_speed_10m { get; set; }
        public int wind_direction_10m { get; set; }
    }

    public class Daily
    {
        public string[] time { get; set; }
        public int[] weather_code { get; set; }
        public float[] apparent_temperature_min { get; set; }
        public float[] apparent_temperature_max { get; set; }
        public float[] pressure_msl_mean { get; set; }
        public int[] winddirection_10m_dominant { get; set; }
        public float[] wind_speed_10m_mean { get; set; }
        public int[] relative_humidity_2m_mean { get; set; }
    }
    #endregion

    public class Daily_Single //klasa własna do pobrania danych dla prognozy
    {
        public int is_day { get; set; } = 1; //dodatkowa opcja, dla prognozy zawsze wartość 1 - żeby animacja była 'day'
        public string time { get; set; }
        public int weather_code { get; set; }
        public float apparent_temperature_min { get; set; }
        public float apparent_temperature_max { get; set; }
        public float pressure_msl_mean { get; set; }
        public int winddirection_10m_dominant { get; set; }
        public float wind_speed_10m_mean { get; set; }
        public int relative_humidity_2m_mean { get; set; }
    }
}
