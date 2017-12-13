using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace EscalationSystem.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoginPage : Page
    {
        public LoginPage()
        {
            this.InitializeComponent();
        }

        private async void Submit_Click(object sender, RoutedEventArgs e)
        {
            if(FTERadioButton.IsChecked==false&&VendorRadioButton.IsChecked==false&&string.IsNullOrEmpty(AliasTxt.Text))
            {
                MessageDialog messageDialog = new MessageDialog("Please choose if you are the FTE or the Vendor and enter your Alias");
                await messageDialog.ShowAsync();
            }
            else if (FTERadioButton.IsChecked == false && VendorRadioButton.IsChecked == false && string.IsNullOrEmpty(AliasTxt.Text)==false)
            {
                MessageDialog messageDialog = new MessageDialog("Please choose if you are the FTE or the Vendor");
                await messageDialog.ShowAsync();
            }
            else if ((FTERadioButton.IsChecked == true || VendorRadioButton.IsChecked == true) && string.IsNullOrEmpty(AliasTxt.Text))
            {
                MessageDialog messageDialog = new MessageDialog("Please enter your alias");
                await messageDialog.ShowAsync();
            }

            else if ((FTERadioButton.IsChecked == true || VendorRadioButton.IsChecked == true) && string.IsNullOrEmpty(AliasTxt.Text)==false)
            {
                Windows.Storage.ApplicationDataContainer LocalSettings =
 Windows.Storage.ApplicationData.Current.LocalSettings;
                LocalSettings.Values["currentUserAlias"] = AliasTxt.Text.ToString();
                if(FTERadioButton.IsChecked==true)
                {
                    this.Frame.Navigate(typeof(FTE_MainPage));
                }

                else if(VendorRadioButton.IsChecked==true)
                {
                    this.Frame.Navigate(typeof(Vendor_MainPage));
                }
            }
        }
    }
}
