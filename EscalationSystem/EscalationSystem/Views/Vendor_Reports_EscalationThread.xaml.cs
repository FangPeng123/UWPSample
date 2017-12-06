using EscalationSystem.Models;
using EscalationSystem.ViewModels;
using MyToolkit.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
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
    public sealed partial class Vendor_Reports_EscalationThread : Page
    {
        public FTEEscalationReportViewModel FTEEscalationReportViewModel { get; set; }
        public ProductWithSelectedItem AllMyPlatform { get; set; }
        public ObservableCollectionView<Report> AllMyReport { get; set; }
        public Vendor_Reports_EscalationThread()
        {
            this.InitializeComponent();
            this.EndDatePicker.Date = DateTime.Today;
            int date = DateTime.Today.Day;
            this.StartDatePicker.Date = DateTime.Today.AddDays(-(date - 1));
            FTEEscalationReportViewModel = new FTEEscalationReportViewModel();
            AllMyPlatform = new ProductWithSelectedItem();
            this.SizeChanged += Vendor_Reports_EscalationThread_SizeChanged;
            this.Loaded += Vendor_Reports_EscalationThread_Loaded;
        }

        private async void Vendor_Reports_EscalationThread_Loaded(object sender, RoutedEventArgs e)
        {
            FTEEscalationReportViewModel = await FTEEscalationReportViewModel.GetFTEEscalationReportViewModel();
            AllMyPlatform = FTEEscalationReportViewModel.AllPratfromList;
            PlatformComboBox.DataContext = AllMyPlatform;
            QueryButton_Click(sender, e);
        }

        private void Vendor_Reports_EscalationThread_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SceenSizeViewModel.ScreenWidth = this.ActualWidth;
        }

        private async void QueryButton_Click(object sender, RoutedEventArgs e)
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
    }
}
