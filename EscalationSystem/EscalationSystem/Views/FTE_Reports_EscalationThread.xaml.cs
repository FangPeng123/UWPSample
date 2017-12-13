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
                QueryButton_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageDialog messageDialog = new MessageDialog(ex.Message.ToString());
                await messageDialog.ShowAsync();
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
                string endDatestring = endDate.ToString("MM-dd-yyyy");
                AllMyReport = await FTEEscalationReportViewModel.QueryAllEscalationReport(AllMyPlatform, startDatestring, endDatestring);
                DataGrid.ItemsSource = AllMyReport;
                await Task.Delay(new TimeSpan(3));
                MyProgressRing.IsActive = false;

            }

            catch (Exception ex)
            {
                MessageDialog messageDialog = new MessageDialog(ex.Message.ToString());
                await messageDialog.ShowAsync();
                MyProgressRing.IsActive = false;
            }
        }

    }
}