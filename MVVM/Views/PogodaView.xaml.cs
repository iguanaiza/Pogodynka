using Microsoft.Maui.Devices.Sensors;
using Pogodynka.MVVM.ViewModels;

namespace Pogodynka.MVVM.Views;

public partial class PogodaView : ContentPage
{
	public PogodaView()
	{
		InitializeComponent();
		BindingContext = new PogodaViewModel();
	}
}