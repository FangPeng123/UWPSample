using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Escalation_UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FirstPage : Page
    {
        public FirstPage()
        {
            this.InitializeComponent();

        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var users = await User.FindAllAsync();
            var currentDomainUser = await users.FirstOrDefault().GetPropertyAsync(KnownUserProperties.DomainName);

            //Save Current User
            var currentUser = await users.FirstOrDefault().GetPropertyAsync(KnownUserProperties.DisplayName);
            Windows.Storage.ApplicationDataContainer localSettings =
    Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["currentUserSetting"] = currentUser.ToString();

            if (currentUser.Equals("Fang Peng"))
            {
                VendorButton.Visibility = Visibility.Visible;
                FTEButton.Visibility = Visibility.Visible;
            }

            else
            {
                //Check if it is the Microsoft alias
                var currentusersplit = currentDomainUser.ToString().Split('\\');
                string currentDomain = currentusersplit[0];
                int count = 0;
                foreach (var item in currentDomain.Split('.'))
                {
                    if (item.Equals("MICROSOFT"))
                    {
                        this.Frame.Navigate(typeof(MainPage));
                        count = 1;
                    }

                }

                if (count == 0)
                {
                    MessageDialog Message = new MessageDialog("You do not have the premission for this App, please log in your PC with your Microsoft Alias!!! ");
                    await Message.ShowAsync();
                }
            }
        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MainPage));        
           
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(FTEMainPage));
        }
    }
}
