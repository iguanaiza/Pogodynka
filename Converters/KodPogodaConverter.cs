using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pogodynka.Resources.Strings;

namespace Pogodynka.Converters
{
    public class KodPogodaConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            //weryfikacja wprowadzonych do konwertera danych
            if (value == null)
                return null;

            var code = (int)value;

            return code switch //podmiana kodu API na odpowiedni opis, ustawiony w AppResources (PL/ENG)
            {
                0 => AppResources.Weather_value_0,
                1 => AppResources.Weather_value_1,
                2 => AppResources.Weather_value_2,
                3 => AppResources.Weather_value_3,
                45 => AppResources.Weather_value_45,
                48 => AppResources.Weather_value_48,
                51 => AppResources.Weather_value_51,
                53 => AppResources.Weather_value_53,
                55 => AppResources.Weather_value_55,
                56 => AppResources.Weather_value_56,
                57 => AppResources.Weather_value_57,
                61 => AppResources.Weather_value_61,
                63 => AppResources.Weather_value_63,
                65 => AppResources.Weather_value_65,
                66 => AppResources.Weather_value_66,
                67 => AppResources.Weather_value_67,
                71 => AppResources.Weather_value_71,
                73 => AppResources.Weather_value_73,
                75 => AppResources.Weather_value_75,
                77 => AppResources.Weather_value_77,
                80 => AppResources.Weather_value_80,
                81 => AppResources.Weather_value_81,
                82 => AppResources.Weather_value_82,
                85 => AppResources.Weather_value_85,
                86 => AppResources.Weather_value_86,
                95 => AppResources.Weather_value_95,
                96 => AppResources.Weather_value_96,
                99 => AppResources.Weather_value_99,
                _ => AppResources.Weather_default
            };
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException(); //brak konwersji w druga strone
        }
    }
}