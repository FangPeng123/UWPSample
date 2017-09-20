using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Escalation_UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FTEMainPage : Page
    {
        public FTEMainPage()
        {
            this.InitializeComponent();
        }
        public List<FTEScenario> Scenarios
        {
            get { return this.scenarios; }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
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
        }
        private void ScenarioControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Clear the status block when navigating scenarios.
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

    public partial class FTEMainPage : Page
    {
        List<FTEScenario> scenarios = new List<FTEScenario>
        {
        
        
            new FTEScenario()
            {
                Title = "FTE Escalation Thread",
                ClassType = typeof(Escalation_Thread_FTEs),
                imagesource=new BitmapImage(new Uri("ms-appx:///Assets/FTE.png"))
            },
            new FTEScenario()
            {
                Title = "Escalation Thread Report",
                ClassType = typeof(Escalation_Thread_Reports),
                imagesource=new BitmapImage(new Uri("ms-appx:///Assets/report.png"))
            },
        };
    }
}
