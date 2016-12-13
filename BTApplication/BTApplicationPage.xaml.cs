using System;
using Xamarin.Forms;
using static System.String;

namespace BTApplication
{
	public partial class BTApplicationPage : ContentPage
	{
	    private readonly IBluetoothManager _bluetoothManager;
	    private readonly Label _display;
		public StackLayout userslayout;
        public int count  =0; 

        public BTApplicationPage(IBluetoothManager bluetoothManager)
		{
		    _bluetoothManager = bluetoothManager;
		    var button = new Button()
		    {
		        Text = "Skanuj w poszukiwaniu urządzeń"
		    };

		    _display = new Label()
		    {
		        Text = "Czekanie na wyniki skanowania",
		        TextColor = Color.Maroon,
		        FontSize = 18,
		    };

            userslayout = new StackLayout()
            {

            };
      
            Content = new StackLayout
		    {
                Children =
                {
                    new Label
                    {
                        Text = "Xamarin - demo wyszukiwania urządzeń w zasięgu",
                        TextColor = Color.Green,
                        FontSize = 20
                    },
                    button,
                    _display,
                    userslayout
                }
		    };

            button.Clicked += Button_Clicked;
		}

        private void Button_Clicked(object sender, EventArgs e)
        {
            _bluetoothManager.Scan();
            _display.Text = "Skanowanie w poszukiwaniu urządzeń...";
        }
    }
}
