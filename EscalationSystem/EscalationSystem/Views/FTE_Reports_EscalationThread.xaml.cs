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
using Windows.UI.Xaml.Navigation;
using EscalationSystem.ViewModels;
using EscalationSystem.Models;
using MyToolkit.Collections;
using System.Threading.Tasks;
using Windows.UI.Popups;
using System.Collections.ObjectModel;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace EscalationSystem.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FTE_Reports_EscalationThread : Page
    {
        public FTEEscalationReportViewModel FTEEscalationReportViewModel { get; set; }
        public ProductWithSelectedItem AllMyPlatform { get; set; }
        public ObservableCollectionView<Report> AllMyReport { get; set; }
        public FTE_Reports_EscalationThread()
        {
            this.InitializeComponent();
            this.EndDatePicker.Date = DateTime.Today;
            int date = DateTime.Today.Day;
            this.StartDatePicker.Date = DateTime.Today.AddDays(-(date - 1));
            FTEEscalationReportViewModel = new FTEEscalationReportViewModel();
            AllMyPlatform = new ProductWithSelectedItem();
            this.SizeChanged += FTE_Reports_EscalationThread_SizeChanged;
            this.Loaded += FTE_Reports_EscalationThread_Loaded;
        }

        private async void FTE_Reports_EscalationThread_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                FTEEscalationReportViewModel = await FTEEscalationReportViewModel.GetFTEEscalationReportViewModel();
                AllMyPlatform = FTEEscalationReportViewModel.AllPratfromList;
                PlatformComboBox.DataContext = AllMyPlatform;
                Product product = (Product)PlatformComboBox.SelectedValue;
                if (product.Platform.Equals("All"))
                {
                    ForumComboBox.DataContext = new ObservableCollection<string> { "All" };
                 
                }
                else
                {
                    ObservableCollection<string> Allforum = await FTEEscalationReportViewModel.GetAllForum(product.Platform);
                    ForumComboBox.DataContext = Allforum;
                  
                }
                ForumComboBox.SelectedIndex = 0;
                QueryButton_Click(sender, e);
            }
            catch (Exception ex)
            {
                DataGrid.ItemsSource = null;
                MyProgressRing.IsActive = false;
            }
        }

        private void FTE_Reports_EscalationThread_SizeChanged(object sender, SizeChangedEventArgs e)
        {         
                SceenSizeViewModel.ScreenWidth = this.ActualWidth;
        }

        private async void QueryButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MyProgressRing.IsActive = true;
                DataGrid.ItemsSource = null;
                DateTime startDate = DateTime.Parse(StartDatePicker.Date.ToString());
                string startDatestring = startDate.ToString("MM-dd-yyyy");
                DateTime endDate = DateTime.Parse(EndDatePicker.Date.ToString());
                DateTime enddatelast = endDate.Date.AddDays(1);
                string endDatestring = enddatelast.ToString("MM-dd-yyyy");
                PlatformComboBox.DataContext = AllMyPlatform;
                Product product = (Product)PlatformComboBox.SelectedValue;
                string plafrom = product.Platform;
                string forum = ForumComboBox.SelectedValue.ToString();
                AllMyReport = await FTEEscalationReportViewModel.QueryAllEscalationReport(AllMyPlatform,plafrom,forum, startDatestring, endDatestring);
                DataGrid.ItemsSource = AllMyReport;
                await Task.Delay(new TimeSpan(3));
                MyProgressRing.IsActive = false;

            }

            catch (Exception ex)
            {
                DataGrid.ItemsSource = null;
                MyProgressRing.IsActive = false;
            }
        }

        private async void PlatformComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Product product = (Product)PlatformComboBox.SelectedValue;
            if (product.Platform.Equals("All"))
            {
                ForumComboBox.DataContext = new ObservableCollection<string> { "All" };
                ForumComboBox.SelectedIndex = 0;
            }
            else
            {
                ObservableCollection<string> Allforum = await FTEEscalationReportViewModel.GetAllForum(product.Platform);
                ForumComboBox.DataContext = Allforum;
                ForumComboBox.SelectedIndex = 0;
            }
        }
    }
}