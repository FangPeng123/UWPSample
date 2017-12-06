using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace EscalationSystem.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FTE_MainPage : Page
    {
        public static int i = 0;
        public FTE_MainPage()
        {
            this.InitializeComponent();
            Windows.Storage.ApplicationDataContainer localSettings =
Windows.Storage.ApplicationData.Current.LocalSettings;
            Object value = localSettings.Values["currentUser"];
            UserTxt.Text = value.ToString();
            this.SizeChanged += FTE_MainPage_SizeChanged;
            Splitter.IsPaneOpen = false;
        }

        private void FTE_MainPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Window.Current.Bounds.Width < 800)
            {
                Splitter.IsPaneOpen = false;
            }
        }

        public List<FTEScenario> Scenarios
        {
            get { return this.scenarios; }
        }

        protected async override void OnNavigatedTo(NavigationEventArgs e)
        {

            // Populate the scenario list from the SampleConfiguration.cs file
            ScenarioControl.ItemsSource = scenarios;
            if (Window.Current.Bounds.Width < 640)
            {
                ScenarioControl.SelectedIndex = -1;
            }
            else
            {
                ScenarioControl.SelectedIndex = 0;
            }

            var access = await BackgroundExecutionManager.RequestAccessAsync();
            if(access==BackgroundAccessStatus.Denied)
            {
                await new MessageDialog("Denied").ShowAsync();
            }
            else
            {
                RegisterBackgroundTask();
            }

        }

        private void RegisterBackgroundTask()
        {
            try
            {

                foreach (var cur in BackgroundTaskRegistration.AllTasks)
                {
                    if (cur.Value.Name == "FTE_All_EscalationThread_Notification")
                    {
                        cur.Value.Unregister(true);
                    }
                }

                bool isregister = BackgroundTaskRegistration.AllTasks.Any(x => x.Value.Name
           == "FTE_All_EscalationThread_Notification");
                if (!isregister)
                {
                    BackgroundTaskBuilder builder = new BackgroundTaskBuilder { Name = "FTE_All_EscalationThread_Notification", TaskEntryPoint = "EscalationSystemBackgroundTask.FTE_All_EscalationThread_Notification" };
                    MaintenanceTrigger trigger = new MaintenanceTrigger(15, false);
                    builder.SetTrigger(trigger);
                    BackgroundTaskRegistration task = builder.Register();
                }

              
            }
            catch
            {

            }
           



        }


        private void ScenarioControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Clear the status block when navigating scenarios.
            try
            {
                ListBox scenarioListBox = sender as ListBox;
                FTEScenario s = scenarioListBox.SelectedItem as FTEScenario;
                if (s != null)
                {
                    ScenarioFrame.Navigate(s.ClassType);
                    if (Window.Current.Bounds.Width < 640)
                    {
                        Splitter.IsPaneOpen = false;
                    }
                }
            }
            catch
            { }
        }

        private async void Footer_Click(object sender, RoutedEventArgs e)
        {
            await Windows.System.Launcher.LaunchUriAsync(new Uri(((HyperlinkButton)sender).Tag.ToString()));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Splitter.IsPaneOpen = !Splitter.IsPaneOpen;
        }

    }

    public class FTEScenario
    {
        public string Title { get; set; }
        public Type ClassType { get; set; }
        public BitmapImage imagesource { get; set; }
    }

    public partial class FTE_MainPage : Page
    {
        List<FTEScenario> scenarios = new List<FTEScenario>
        {
            //  new FTEScenario()
            //{
            //    Title = "FTE Opend Escalation Threads",
            //    ClassType = typeof(Views.FTE_Opened_EscalationThread),
            //    imagesource=new BitmapImage(new Uri("ms-appx:///Assets/create.png"))
            //},
            new FTEScenario()
            {
                Title = "FTE All Esclatation Threads",
                ClassType = typeof(Views.FTE_All_EscalationThread),
                imagesource=new BitmapImage(new Uri("ms-appx:///Assets/angent.png"))
            },
            new FTEScenario()
            {
                Title = "FTE Escalation Thread Reports",
                ClassType = typeof(Views.FTE_Reports_EscalationThread),
                imagesource=new BitmapImage(new Uri("ms-appx:///Assets/report.png"))
            },
             new FTEScenario()
            {
                Title = "FTE Consult Threads Records",
                ClassType = typeof(Views.FTE_Consult_Page),
                imagesource=new BitmapImage(new Uri("ms-appx:///Assets/create.png"))
            },
        };
    }
}
