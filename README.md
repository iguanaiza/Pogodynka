# About the project
Simple weather app created as one of the projects during informatics bachelor's studies (6th semester).

The project aimed to create a user-friendly weather application, enabling users to check the weather forecast at any time. The application was intended to allow users to check the current weather for a user-selected location, as well as a 5-day forecast. Weather data was fetched from an external API, which required an internet connection. The application also offered automatic retrieval of the user's current location.

The project objectives were:
* To create an intuitive mobile application for checking the weather.
* To provide the ability to check both the current weather and the forecast.
* To implement GPS support for location retrieval.
* To design a user-friendly and clear user interface.
* To adapt to the device's light/dark mode.
* To adapt the application language to the system language (EN and PL).
* To design a custom splash screen and application icon.

## Technologies
The project was developed using **.NET MAUI 9**, Visual Studio 2022, and Android Studio (Pixel 7, Android 15). Weather data is provided by the open-source Open-Meteo API.

The animations used in the application are sourced from the free LottieFiles website (lottiefiles.com). The icon and splash screen graphics were created in Adobe Illustrator, based on the visuals from the weather animations.

Additionally, the _Fody_ package was installed to handle the automatic implementation of the _INotifyPropertyChanged_ interface. _Fody_ is a tool for modifying compiled .NET code, allowing for the elimination of boilerplate code, such as the _INotifyPropertyChanged_ implementation.

To handle the .json animations, the _SkiaSharp.Extended.UI.Maui_ package was installed, which contains SkiaSharp controls for MAUI. SkiaSharp is a 2D graphics system for .NET and C# used for drawing 2D vector graphics, bitmaps, and text.

## Project documentation
**Partial project documentation** in polish is available in the file [Pogodynka_docs_PL.pdf](https://github.com/iguanaiza/Pogodynka/blob/master/Pogodynka_docs_PL.pdf). 
Full version can be sent on request for job recruitment process.
