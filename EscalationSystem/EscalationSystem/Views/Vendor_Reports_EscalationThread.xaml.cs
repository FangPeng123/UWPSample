using EscalationSystem.Models;
using EscalationSystem.ViewModels;
using MyToolkit.Collections;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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
    public sealed partial class Vendor_Reports_EscalationThread : Page
    {
        public VendorEscalationReportViewModel VendorEscalationReportViewModel { get; set; }
        public ProductWithSelectedItem AllMyPlatform { get; set; }
        public ObservableCollection<string> AllMyForum { get; set; }
        public ObservableCollectionView<Report> AllMyReport { get; set; }
        public Vendor_Reports_EscalationThread()
        {
            this.InitializeComponent();
            this.EndDatePicker.Date = DateTime.Today;
            int date = DateTime.Today.Day;
            this.StartDatePicker.Date = DateTime.Today.AddDays(-(date - 1));
            VendorEscalationReportViewModel = new VendorEscalationReportViewModel();
            AllMyPlatform = new ProductWithSelectedItem();
            AllMyForum = new ObservableCollection<string>();

            this.SizeChanged += Vendor_Reports_EscalationThread_SizeChanged;
            this.Loaded += Vendor_Reports_EscalationThread_Loaded;
        }

        private async void Vendor_Reports_EscalationThread_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {


                VendorEscalationReportViewModel = await VendorEscalationReportViewModel.GetVendorEscalationReportViewModel();
                AllMyPlatform = VendorEscalationReportViewModel.AllPratfromList;
                AllMyForum.Add("All");
                PlatformComboBox.DataContext = AllMyPlatform;
                ForumCombobox.DataContext = AllMyForum;
                ForumCombobox.SelectedIndex = 0;
                ForumCombobox.DataContext = AllMyPlatform;
                QueryButton_Click(sender, e);
            }
            catch (Exception ex)
            {
                MessageDialog messageDialog = new MessageDialog(ex.Message.ToString());
                await messageDialog.ShowAsync();
                MyProgressRing.IsActive = false;
            }
        }

        private void Vendor_Reports_EscalationThread_SizeChanged(object sender, SizeChangedEventArgs e)
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
                Product product = (Product)PlatformComboBox.SelectedValue;
                string platform = product.Platform;
                string forum = ForumCombobox.SelectedValue.ToString();
                AllMyReport = await VendorEscalationReportViewModel.QueryAllEscalationReport(platform, forum, startDatestring, endDatestring);
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

        private async void PlatformComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (PlatformComboBox.SelectedIndex != 0)
            {
                
                Product product = (Product)PlatformComboBox.SelectedValue;
                ObservableCollection<string> Allforum = await VendorEscalationReportViewModel.GetAllForum(product.Platform);
                Allforum.Insert(0, "All");
                ForumCombobox.DataContext = Allforum;
                ForumCombobox.SelectedIndex = 0;
            }

        }
    }
}
