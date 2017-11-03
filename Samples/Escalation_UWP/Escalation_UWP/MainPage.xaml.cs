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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Escalation_UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        public MainPage()
        {
            this.InitializeComponent();
            this.SizeChanged += MainPage_SizeChanged;
        }

        private void MainPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (Window.Current.Bounds.Width < 800)
            {
                Splitter.IsPaneOpen = false;
            }
        }

        public List<Scenario> Scenarios
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
            Scenario s = scenarioListBox.SelectedItem as Scenario;
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

    public class Scenario
    {
        public string Title { get; set; }
        public Type ClassType { get; set; }
        public BitmapImage imagesource { get; set; }
    }

    public partial class MainPage : Page
    {
        List<Scenario> scenarios = new List<Scenario>
        {
            new Scenario()
            {
                Title = "Create Escalation Thread",
                ClassType = typeof(Escalation_Thread),
                imagesource=new BitmapImage(new Uri("ms-appx:///Assets/create.png"))
            },
            new Scenario()
            {
                Title = "Angent Esclatation Thread",
                ClassType = typeof(Escalation_Thread_Agents),
                imagesource=new BitmapImage(new Uri("ms-appx:///Assets/angent.png"))
            },
            new Scenario()
            {
                Title = "Escalation Thread Report",
                ClassType = typeof(Escalation_Thread_Reports),
                imagesource=new BitmapImage(new Uri("ms-appx:///Assets/report.png"))
            },
        };
}
   


}
