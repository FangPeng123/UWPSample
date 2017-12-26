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
        public ObservableCollectionView<Report> AllMySearchReport { get; set; }
        public int pageSize { get; set; }
        public static int tag = 0;
        public int searchPageSize { get; set; }
        public Vendor_Reports_EscalationThread()
        {
            this.InitializeComponent();
            this.EndDatePicker.Date = DateTime.Today;
            int date = DateTime.Today.Day;
            this.StartDatePicker.Date = DateTime.Today.AddDays(-(date - 1));
            VendorEscalationReportViewModel = new VendorEscalationReportViewModel();
            AllMyPlatform = new ProductWithSelectedItem();
            AllMyForum = new ObservableCollection<string>();
            AllMySearchReport = new ObservableCollectionView<Report>();

            this.SizeChanged += Vendor_Reports_EscalationThread_SizeChanged;
            this.Loaded += Vendor_Reports_EscalationThread_Loaded;
        }

        private void ShowQueryImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (ShowSearchPanel.Visibility == Visibility.Collapsed)
            {
                ShowSearchPanel.Visibility = Visibility.Visible;
            }
            else
            {
                ShowSearchPanel.Visibility = Visibility.Collapsed;
            }
        }

        private async void Vendor_Reports_EscalationThread_Loaded(object sender, RoutedEventArgs e)
        {

            try
            {


                VendorEscalationReportViewModel = await VendorEscalationReportViewModel.GetVendorEscalationReportViewModel();
                AllMyPlatform = VendorEscalationReportViewModel.AllPratfromList;
                AllMyForum.Add("All");
                PlatformComboBox.DataContext = AllMyPlatform;
                Product product = (Product)PlatformComboBox.SelectedValue;
                if (product.Platform.Equals("All"))
                {
                    ForumComboBox.DataContext = new ObservableCollection<string> { "All" };

                }
                else
                {
                    ObservableCollection<string> Allforum = await VendorEscalationReportViewModel.GetAllForum(product.Platform);
                    ForumComboBox.DataContext = Allforum;

                }
                ForumComboBox.SelectedIndex = 0;
                PageComboBox.SelectedIndex = 0;
                if (PlatformComboBox.DataContext == null)
                {
                    DataGrid.ItemsSource = null;
                    AllRecords.Text = "0";
                    AllPageIndex.Text = "0";
                    PageTxt.Text = "0";
                    MyProgressRing.IsActive = false;
                }
                else
                {
                    QueryButton_Click(sender, e);
                }
            }
            catch (Exception ex)
            {
                DataGrid.ItemsSource = null;
                MyProgressRing.IsActive = false;
            }

            MyProgressRing.IsActive = false;
        }

        private void Vendor_Reports_EscalationThread_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SceenSizeViewModel.ScreenWidth = this.ActualWidth;
        }

        private async void QueryButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ShowSearchPanel.Visibility = Visibility.Collapsed;
                MyProgressRing.IsActive = true;
                DataGrid.ItemsSource = null;
                DateTime startDate = DateTime.Parse(StartDatePicker.Date.ToString());
                string startDatestring = startDate.ToString("MM-dd-yyyy");
                DateTime endDate = DateTime.Parse(EndDatePicker.Date.ToString());
                DateTime enddatelast = endDate.Date.AddDays(1);
                string endDatestring = enddatelast.ToString("MM-dd-yyyy");
                Product product = (Product)PlatformComboBox.SelectedValue;
                string platform = product.Platform;
                string forum = ForumComboBox.SelectedValue.ToString();
                AllMyReport = await VendorEscalationReportViewModel.QueryAllEscalationReport(platform, forum, startDatestring, endDatestring, true);
                ComboBoxItem curItem = (ComboBoxItem)PageComboBox.SelectedItem;
                pageSize = Convert.ToInt32(curItem.Content.ToString());
                if (AllMyReport.Count == 0)
                {
                    AllRecords.Text = "0";
                    AllPageIndex.Text = "0";
                    PageTxt.Text = "0";
                    DataGrid.ItemsSource = AllMyReport;
                    MyScrollView.Height = 100;
                }

                else if (AllMyReport.Count < 10)
                {
                    DataGrid.ItemsSource = AllMyReport;
                    MyScrollView.Height = (AllMyReport.Count + 1) * 60;
                    AllRecords.Text = AllMyReport.Count.ToString();
                    AllPageIndex.Text = VendorEscalationReportViewModel.GetPageIndex(AllMyReport, pageSize).ToString();
                    PageTxt.Text = VendorEscalationReportViewModel.GetPageIndex(AllMyReport, pageSize).ToString();
                }

                else
                {
                    AllRecords.Text = AllMyReport.Count.ToString();
                    AllPageIndex.Text = VendorEscalationReportViewModel.GetPageIndex(AllMyReport, pageSize).ToString();
                    int AllPagesIndex = VendorEscalationReportViewModel.GetPageIndex(AllMyReport, pageSize);
                    PageTxt.Text = "1";
                    if (AllMyReport.Count >= 10)
                    {
                        MyScrollView.Height = 650;
                    }
                    if (AllPagesIndex == 1)
                    {
                        DataGrid.ItemsSource = AllMyReport;

                    }
                    else
                    {
                        var EscalationThreadListPage = AllMyReport.Skip(0 * pageSize).Take(pageSize).ToList();
                        DataGrid.ItemsSource = EscalationThreadListPage;

                    }
                }
                await Task.Delay(new TimeSpan(3000));
                MyProgressRing.IsActive = false;

            }
            catch (Exception ex)
            {
                DataGrid.ItemsSource = null;
                AllRecords.Text = "0";
                AllPageIndex.Text = "0";
                PageTxt.Text = "0";
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
                //ForumCombobox.DataContext = Allforum;
                //ForumCombobox.SelectedIndex = 0;
            }

        }

        private async void Search_Tapped(object sender, TappedRoutedEventArgs e)
        {

            try
            {

                if (AllMySearchReport.Count > 0)
                {
                    AllMySearchReport.Items.Clear();
                }
                DataGrid.ItemsSource = null;
                MyProgressRing.IsActive = true;
                PreviousImage.Visibility = Visibility.Collapsed;
                FirstImage.Visibility = Visibility.Collapsed;
                NextImage.Visibility = Visibility.Collapsed;
                LastImage.Visibility = Visibility.Collapsed;
                PreviousSearchImage.Visibility = Visibility.Visible;
                FirstSearchImage.Visibility = Visibility.Visible;
                NextSearchImage.Visibility = Visibility.Visible;
                LastSearchImage.Visibility = Visibility.Visible;

                DateTime startDate = DateTime.Parse(StartDatePicker.Date.ToString());
                string startDatestring = startDate.ToString("MM-dd-yyyy");
                DateTime endDate = DateTime.Parse(EndDatePicker.Date.ToString());
                DateTime enddatelast = endDate.Date.AddDays(1);
                string endDatestring = enddatelast.ToString("MM-dd-yyyy");
                Product product = (Product)PlatformComboBox.SelectedValue;
                string platform = product.Platform;
                ComboBoxItem curItem = (ComboBoxItem)PageComboBox.SelectedItem;
                searchPageSize = Convert.ToInt32(curItem.Content.ToString());
                string forum = ForumComboBox.SelectedValue.ToString();
                MyProgressRing.IsActive = true;
                try
                {
                    if (Searchtxt.Text.ToString().Contains("v-"))
                    {
                        var SearchReport = await VendorEscalationReportViewModel.QueryAllEscalationReport(platform, forum, startDatestring, endDatestring, true);
                        int i = 0;
                        if (SearchReport.Items.Count > 0)
                        {

                            foreach (var item in SearchReport)
                            {
                                if (item.Alias == Searchtxt.Text.ToString())
                                {
                                    AllMySearchReport.Items.Add(item);
                                    i = 1;
                                }

                            }

                            if (i == 1)
                            {


                                if (AllMySearchReport.Count < 10)
                                {
                                    DataGrid.ItemsSource = AllMySearchReport;
                                    MyScrollView.Height = (AllMySearchReport.Count + 1) * 60;
                                    AllRecords.Text = AllMySearchReport.Count.ToString();
                                    AllPageIndex.Text = VendorEscalationReportViewModel.GetPageIndex(AllMySearchReport, searchPageSize).ToString();
                                    PageTxt.Text = VendorEscalationReportViewModel.GetPageIndex(AllMySearchReport, searchPageSize).ToString();
                                }

                                else
                                {
                                    AllRecords.Text = AllMySearchReport.Count.ToString();
                                    AllPageIndex.Text = VendorEscalationReportViewModel.GetPageIndex(AllMySearchReport, searchPageSize).ToString();
                                    int AllPagesIndex = VendorEscalationReportViewModel.GetPageIndex(AllMySearchReport, searchPageSize);
                                    PageTxt.Text = "1";
                                    if (AllMySearchReport.Count >= 10)
                                    {
                                        MyScrollView.Height = 650;
                                    }
                                    else if (AllPagesIndex == 1)
                                    {
                                        DataGrid.ItemsSource = AllMySearchReport;

                                    }
                                    else
                                    {
                                        var SearchThreadList1Page = AllMySearchReport.Skip(0 * searchPageSize).Take(searchPageSize).ToList();
                                        DataGrid.ItemsSource = SearchThreadList1Page;

                                    }
                                }
                            }
                            else
                            {
                                DataGrid.ItemsSource = null;
                                AllRecords.Text = "0";
                                AllPageIndex.Text = "0";
                                PageTxt.Text = "0";
                                MyProgressRing.IsActive = false;
                            }
                        }

                        else
                        {
                            DataGrid.ItemsSource = null;
                            AllRecords.Text = "0";
                            AllPageIndex.Text = "0";
                            PageTxt.Text = "0";
                            MyProgressRing.IsActive = false;
                        }
                    }

                    else
                    {
                        var SearchReport = await VendorEscalationReportViewModel.QueryAllEscalationReport(platform, forum, startDatestring, endDatestring, false);
                        int i = 0;
                        if (SearchReport.Items.Count > 0)
                        {

                            foreach (var item in SearchReport)
                            {
                                if (item.Alias == Searchtxt.Text.ToString())
                                {
                                    AllMySearchReport.Items.Add(item);
                                    i = 1;
                                }

                            }

                            if (i == 1)
                            {


                                if (AllMySearchReport.Count < 10)
                                {
                                    DataGrid.ItemsSource = AllMySearchReport;
                                    MyScrollView.Height = (AllMySearchReport.Count + 1) * 60;
                                    AllRecords.Text = AllMySearchReport.Count.ToString();
                                    AllPageIndex.Text = VendorEscalationReportViewModel.GetPageIndex(AllMySearchReport, searchPageSize).ToString();
                                    PageTxt.Text = VendorEscalationReportViewModel.GetPageIndex(AllMySearchReport, searchPageSize).ToString();
                                }

                                else
                                {
                                    AllRecords.Text = AllMySearchReport.Count.ToString();
                                    AllPageIndex.Text = VendorEscalationReportViewModel.GetPageIndex(AllMySearchReport, searchPageSize).ToString();
                                    int AllPagesIndex = VendorEscalationReportViewModel.GetPageIndex(AllMySearchReport, searchPageSize);
                                    PageTxt.Text = "1";
                                    if (AllMySearchReport.Count >= 10)
                                    {
                                        MyScrollView.Height = 650;
                                    }
                                    else if (AllPagesIndex == 1)
                                    {
                                        DataGrid.ItemsSource = AllMySearchReport;

                                    }
                                    else
                                    {
                                        var SearchThreadList1Page = AllMySearchReport.Skip(0 * searchPageSize).Take(searchPageSize).ToList();
                                        DataGrid.ItemsSource = SearchThreadList1Page;

                                    }
                                }
                            }
                            else
                            {
                                DataGrid.ItemsSource = null;
                                AllRecords.Text = "0";
                                AllPageIndex.Text = "0";
                                PageTxt.Text = "0";
                                MyProgressRing.IsActive = false;
                            }
                        }

                        else
                        {
                            DataGrid.ItemsSource = null;
                            AllRecords.Text = "0";
                            AllPageIndex.Text = "0";
                            PageTxt.Text = "0";
                            MyProgressRing.IsActive = false;
                        }
                    }

                }

                catch
                {
                    DataGrid.ItemsSource = null;
                    AllRecords.Text = "0";
                    AllPageIndex.Text = "0";
                    PageTxt.Text = "0";
                    MyProgressRing.IsActive = false;
                }

            }
            catch
            {
                DataGrid.ItemsSource = null;
                AllRecords.Text = "0";
                AllPageIndex.Text = "0";
                PageTxt.Text = "0";
                MyProgressRing.IsActive = false;
            }
               MyProgressRing.IsActive = false;
           }
        

        private void FirstSearchImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (AllMySearchReport.Count == 0)
            {
                this.PageTxt.Text = "0";
                MyScrollView.Height = 100;
                DataGrid.ItemsSource = null;
            }
            else
            {
                var ReportListPage1 = AllMySearchReport.Skip(0 * searchPageSize).Take(searchPageSize).ToList();
                DataGrid.ItemsSource = ReportListPage1;
                setScrollViewheight(ReportListPage1);
                PageTxt.Text = "1";

            }
        }


        private void PreviousSearchImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (AllMySearchReport.Count == 0)
            {
                this.PageTxt.Text = "0";
                MyScrollView.Height = 100;
                DataGrid.ItemsSource = null;
            }
            else
            {
                if (Convert.ToInt32(PageTxt.Text.ToString()) == 1)
                {
                    var ReportListPage1 = AllMySearchReport.Skip(0 * searchPageSize).Take(searchPageSize).ToList();
                    DataGrid.ItemsSource = ReportListPage1;
                    setScrollViewheight(ReportListPage1);
                    PageTxt.Text = "1";

                }
                else
                {
                    var ReportListPage = AllMySearchReport.Skip((Convert.ToInt32(PageTxt.Text.ToString())) - 1 * searchPageSize).Take(searchPageSize).ToList();
                    DataGrid.ItemsSource = ReportListPage;
                    setScrollViewheight(ReportListPage);
                    PageTxt.Text = ((Convert.ToInt32(PageTxt.Text.ToString())) - 1).ToString();
                }
            }

        }

        private void NextSearchImage_Tapped(object sender, TappedRoutedEventArgs e)
        {

            if (AllMySearchReport.Count == 0)
            {
                this.PageTxt.Text = "0";
                MyScrollView.Height = 100;
                DataGrid.ItemsSource = null;
            }

            else
            {
                int AllPageIndex = VendorEscalationReportViewModel.GetPageIndex(AllMySearchReport, searchPageSize);
                int index = Convert.ToInt32(PageTxt.Text.ToString());
                index++;
                if (index < AllPageIndex)
                {
                    var ReportListPage = AllMySearchReport.Skip((index - 1) * searchPageSize).Take(searchPageSize).ToList();
                    DataGrid.ItemsSource = ReportListPage;
                    setScrollViewheight(ReportListPage);
                    PageTxt.Text = index.ToString();

                }

                else
                {
                    var ReportListPage = AllMySearchReport.Skip((AllPageIndex - 1) * searchPageSize).Take(searchPageSize).ToList();
                    DataGrid.ItemsSource = ReportListPage;
                    setScrollViewheight(ReportListPage);
                    PageTxt.Text = AllPageIndex.ToString();


                }
            }

        }

        private void LastSearchImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (AllMySearchReport.Count == 0)
            {
                this.PageTxt.Text = "0";
                MyScrollView.Height = 100;
                DataGrid.ItemsSource = null;
            }
            else
            {
                int AllPageIndex = VendorEscalationReportViewModel.GetPageIndex(AllMySearchReport, searchPageSize);
                PageTxt.Text = AllPageIndex.ToString();
                var ReportListPage = AllMySearchReport.Skip((AllPageIndex - 1) * searchPageSize).Take(searchPageSize).ToList();
                DataGrid.ItemsSource = ReportListPage;
                setScrollViewheight(ReportListPage);
                PageTxt.Text = AllPageIndex.ToString();
            }
        }

    


    private void FirstImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (AllMyReport.Count == 0)
            {
                this.PageTxt.Text = "0";
                MyScrollView.Height = 100;
                DataGrid.ItemsSource = null;
            }
            else
            {
                var ReportThreadListPage1 = AllMyReport.Skip(0 * pageSize).Take(pageSize).ToList();
                DataGrid.ItemsSource = ReportThreadListPage1;
                setScrollViewheight(ReportThreadListPage1);
                PageTxt.Text = "1";

            }
        }


        public void setScrollViewheight(List<Report> MyList)
        {
            if (MyList.Count >= 10)
            {
                MyScrollView.Height = 650;
            }
            else
            {
                MyScrollView.Height = (MyList.Count + 1) * 55;
            }
        }

        private void NextImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (AllMyReport.Count == 0)
            {
                this.PageTxt.Text = "0";
                MyScrollView.Height = 100;
                DataGrid.ItemsSource = null;
            }

            else
            {
                int AllPageIndex = VendorEscalationReportViewModel.GetPageIndex(AllMyReport, pageSize);
                int index = Convert.ToInt32(PageTxt.Text.ToString());
                index++;
                if (index < AllPageIndex)
                {
                    var ReportThreadListPage = AllMyReport.Skip((index - 1) * pageSize).Take(pageSize).ToList();
                    DataGrid.ItemsSource = ReportThreadListPage;
                    setScrollViewheight(ReportThreadListPage);
                    PageTxt.Text = index.ToString();

                }

                else
                {
                    var ReportThreadListPage = AllMyReport.Skip((AllPageIndex - 1) * pageSize).Take(pageSize).ToList();
                    DataGrid.ItemsSource = ReportThreadListPage;
                    setScrollViewheight(ReportThreadListPage);
                    int count = AllMyReport.Count;
                    PageTxt.Text = AllPageIndex.ToString();

                }
            }
        }

        private void PreviousImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (AllMyReport.Count == 0)
            {
                this.PageTxt.Text = "0";
                MyScrollView.Height = 100;
                DataGrid.ItemsSource = null;
            }
            else
            {
                if (Convert.ToInt32(PageTxt.Text.ToString()) == 1)
                {
                    var ReportThreadListPage1 = AllMyReport.Skip(0 * pageSize).Take(pageSize).ToList();
                    DataGrid.ItemsSource = ReportThreadListPage1;
                    setScrollViewheight(ReportThreadListPage1);
                    PageTxt.Text = "1";

                }
                else
                {
                    var ReportThreadListPage = AllMyReport.Skip((Convert.ToInt32(PageTxt.Text.ToString())) - 1 * pageSize).Take(pageSize).ToList();
                    DataGrid.ItemsSource = ReportThreadListPage;
                    setScrollViewheight(ReportThreadListPage);
                    PageTxt.Text = ((Convert.ToInt32(PageTxt.Text.ToString())) - 1).ToString();



                }
            }
        }


        private void LastImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (AllMyReport.Count == 0)
            {
                this.PageTxt.Text = "0";
                MyScrollView.Height = 100;
                DataGrid.ItemsSource = null;
            }
            else
            {
                int AllPageIndex = VendorEscalationReportViewModel.GetPageIndex(AllMyReport, pageSize);
                PageTxt.Text = AllPageIndex.ToString();
                var ReportThreadListPage = AllMyReport.Skip((AllPageIndex - 1) * pageSize).Take(pageSize).ToList();
                DataGrid.ItemsSource = ReportThreadListPage;
                setScrollViewheight(ReportThreadListPage);
                PageTxt.Text = AllPageIndex.ToString();
            }
        }

        private async void PlatformComboBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            Product product = (Product)PlatformComboBox.SelectedValue;
            if (product.Platform.Equals("All"))
            {
                ForumComboBox.DataContext = new ObservableCollection<string> { "All" };
                ForumComboBox.SelectedIndex = 0;
            }
            else
            {
                ObservableCollection<string> Allforum = await VendorEscalationReportViewModel.GetAllForum(product.Platform);
                ForumComboBox.DataContext = Allforum;
                ForumComboBox.SelectedIndex = 0;
            }

        }
    }

}

  
