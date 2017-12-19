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
using EscalationSystem.Views;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace EscalationSystem
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
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
            Windows.Storage.ApplicationDataContainer LocalSettings =
    Windows.Storage.ApplicationData.Current.LocalSettings;
            LocalSettings.Values["currentUser"] = currentUser.ToString();
            var currentusersplit = currentDomainUser.ToString().Split('\\');
            string currentDomain = currentusersplit[0];
            //store alias
            string userAlias = currentusersplit[1];
            LocalSettings.Values["currentUserAlias"] = userAlias.ToString();

            int count = 0;
            foreach (var item in currentDomain.Split('.'))
            {
                if (item.Equals("MICROSOFT"))
                {
                    if (userAlias.Equals("v-luyong"))
                    {
                        VendorButton.Visibility = Visibility.Visible;
                        FTEButton.Visibility = Visibility.Visible;
                        count = 1;
                        //string val = userAlias;
                        //new Views.ShowNotificationForm("Login Successful").show();
                        //SHToastNotification.ShowToastNotification("Square150x150Logo.scale-200.png", $"已复制 {val}", NotificationAudioNames.Default,true);
                    }

                    else if (userAlias.Equals("fapeng"))
                    {
                        VendorButton.Visibility = Visibility.Visible;
                        FTEButton.Visibility = Visibility.Visible;
                        count = 1;

                        

                        //string val = userAlias;
                        //new Views.ShowNotificationForm("Login Successful").show();
                        //SHToastNotification.ShowToastNotification("Square150x150Logo.scale-200.png", $"已复制 {val}", NotificationAudioNames.Default,true);
                    }
                    else if (userAlias.Equals("jierong"))
                    {
                        VendorButton.Visibility = Visibility.Visible;
                        FTEButton.Visibility = Visibility.Visible;
                        count = 1;



                        //string val = userAlias;
                        //new Views.ShowNotificationForm("Login Successful").show();
                        //SHToastNotification.ShowToastNotification("Square150x150Logo.scale-200.png", $"已复制 {val}", NotificationAudioNames.Default,true);
                    }

                    else if (userAlias.Contains("v-"))
                    {
                        this.Frame.Navigate(typeof(Vendor_MainPage));
                        count = 1;
                        //string val = userAlias;
                        //new Views.ShowNotificationForm("Login Successful").show();
                        //SHToastNotification.ShowToastNotification("Square150x150Logo.scale-200.png", $"已复制 {val}", NotificationAudioNames.Default,true);
                    }

                    else
                    {
                        this.Frame.Navigate(typeof(FTE_MainPage));
                        count = 1;
                        //string val = userAlias;
                        //new Views.ShowNotificationForm("Login Successful").show();
                        //SHToastNotification.ShowToastNotification("Square150x150Logo.scale-200.png", $"已复制 {val}", NotificationAudioNames.Default,true);
                    }
                    
                }

            }

            if (count == 0)
            {
                MessageDialog Message = new MessageDialog("You do not have the premission for this App, please log in your PC with your Microsoft Alias!!! or you will go to this Login Page ");
                await Message.ShowAsync();
                this.Frame.Navigate(typeof(LoginPage));
            }



        }
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Views.Vendor_MainPage));

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(Views.FTE_MainPage));
        }
    }
}

