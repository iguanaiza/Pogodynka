using Pogodynka.MVVM.Models;
using Pogodynka.Resources;
using SkiaSharp.Extended.UI.Controls;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pogodynka.Converters
{
    public class KodGrafikaConverter : IMultiValueConverter
    {
        public object? Convert(object[] values, Type targetType, object? parameter, CultureInfo culture)
        {
            //weryfikacja wprowadzonych do konwertera danych - musi pobrac najpierw kod API, potem kod dnia
            if (values.Length < 2 || values[0] is not int code || values[1] is not int isDay)
                return null;

            var lottieImageSource = new SKFileLottieImageSource();

            //przypisanie kodów API do rodzaju grafiki (po kilka na jeden dla uproszczenia)
            var rainCodes = new[] { 51, 53, 55, 61, 63, 65 };
            var rainSnowCodes = new[] { 56, 57 };
            var snowCodes = new[] { 56, 57, 66, 67, 71, 73, 75, 77, 85, 86 };
            var thunderCodes = new[] { 95, 96, 99 };
            var showerCodes = new[] { 80, 81, 82 };
            var fogCodes = new[] { 45, 48 };

            string file = code switch //podmiana kodu API na odpowiednią grafikę
            {
                0 or 1 => isDay == 1 ? "dayClear.json" : "nightClear.json",
                2 => isDay == 1 ? "dayCloudPart.json" : "nightCloudPart.json",
                3 => isDay == 1 ? "dayCloud.json" : "nightCloud.json",
                _ when fogCodes.Contains(code) => isDay == 1 ? "dayFog.json" : "nightFog.json",
                _ when rainCodes.Contains(code) => isDay == 1 ? "dayRain.json" : "nightRain.json",
                _ when rainSnowCodes.Contains(code) => isDay == 1 ? "dayRainSnow.json" : "nightRainSnow.json",
                _ when snowCodes.Contains(code) => isDay == 1 ? "daySnow.json" : "nightSnow.json",
                _ when showerCodes.Contains(code) => isDay == 1 ? "dayRainShower.json" : "nightRainShowerw.json",
                _ when thunderCodes.Contains(code) => isDay == 1 ? "dayThunder.json" : "nightThunder.json",
                _ => "dayClear.json"
            };

            lottieImageSource.File = file; //przypisanie grafiki
            return lottieImageSource;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException(); //brak konwersji w druga strone
        }
    }
}