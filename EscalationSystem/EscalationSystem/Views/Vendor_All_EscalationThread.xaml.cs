﻿using EscalationSystem.Models;
using MyToolkit.Collections;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
using Windows.Web.Http;
using EscalationSystem.ViewModels;
using System.Threading.Tasks;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace EscalationSystem.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>

    public sealed partial class Vendor_All_EscalationThread : Page
    {

        public EscalationStatusWithSelectedItem EscalatonStatusList { get; set; }
        public ProductWithSelectedItem AllMyPlatform { get; set; }
        public ObservableCollectionView<EscalationAndStatusThread> EscalationThreadList { get; set; }
        public ObservableCollectionView<EscalationAndStatusThread> EscalationThreadListPage { get; set; }
        public ObservableCollectionView<EscalationAndStatusThread> MySearchEscalationThreadList { get; set; }
        public ObservableCollectionView<EscalationAndStatusThread> MySearchEscalationThreadListPage { get; set; }
        public EscalationThread EscalationThread { get; set; }
        public VendorEscalationThreadViewModel VendorEscalationThreadViewModel { get; set; }
        public int pageSize;
        public int searchPageSize;
        public static int i = 0;
        public static int tag = 0;
        public Vendor_All_EscalationThread()
        {
            this.InitializeComponent();
            this.EndDatePicker.Date = DateTime.Today;
            int date = DateTime.Today.Day;
            this.StartDatePicker.Date = DateTime.Today.AddDays(-(date - 1));
            this.SizeChanged += Vendor_All_EscalationThread_SizeChanged;
            EscalatonStatusList = new EscalationStatusWithSelectedItem();
            AllMyPlatform = new ProductWithSelectedItem();

            EscalationThreadList = new ObservableCollectionView<EscalationAndStatusThread>();
            EscalationThreadListPage = new ObservableCollectionView<EscalationAndStatusThread>();
            MySearchEscalationThreadList = new ObservableCollectionView<EscalationAndStatusThread>();
            MySearchEscalationThreadListPage = new ObservableCollectionView<EscalationAndStatusThread>();
            VendorEscalationThreadViewModel = new VendorEscalationThreadViewModel();
            EscalationThread = new EscalationThread();
            this.Loaded += Vendor_All_EscalationThread_Loaded;
            this.DataContext = VendorEscalationThreadViewModel;

        }



        private async void Vendor_All_EscalationThread_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                VendorEscalationThreadViewModel = await VendorEscalationThreadViewModel.GetVendorEscalationThreadViewModel();
                EscalatonStatusList = VendorEscalationThreadViewModel.AllEscalationStatusList;
                StatusComboBox.DataContext = EscalatonStatusList;
                AllMyPlatform = VendorEscalationThreadViewModel.AllPratfromList;
                PlatformComboBox.DataContext = AllMyPlatform;
                PageComboBox.SelectedIndex = 0;
                if (StatusComboBox.DataContext == null || PlatformComboBox.DataContext == null)
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
                AllRecords.Text = "0";
                AllPageIndex.Text = "0";
                PageTxt.Text = "0";
                MyProgressRing.IsActive = false;
            }
        }


        private void Vendor_All_EscalationThread_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SceenSizeViewModel.ScreenWidth = this.ActualWidth;
        }

        private async void QueryButton_Click(object sender, RoutedEventArgs e)
        {
            ShowSearchPanel.Visibility = Visibility.Collapsed;
            PreviousImage.Visibility = Visibility.Visible;
            FirstImage.Visibility = Visibility.Visible;
            NextImage.Visibility = Visibility.Visible;
            LastImage.Visibility = Visibility.Visible;
            PreviousSearchImage.Visibility = Visibility.Collapsed;
            FirstSearchImage.Visibility = Visibility.Collapsed;
            NextSearchImage.Visibility = Visibility.Collapsed;
            LastSearchImage.Visibility = Visibility.Collapsed;
            try
            {
                if (tag == 0)
                {
                    MyProgressRing.IsActive = true;
                    DataGrid.ItemsSource = null;
                    AllRecords.Text = "0";
                    AllPageIndex.Text = "0";
                    PageTxt.Text = "0";
                }
                MyProgressRing.IsActive = true;
                tag = 1;
                DateTime startDate = DateTime.Parse(StartDatePicker.Date.ToString());
                string startDatestring = startDate.ToString("MM-dd-yyyy");
                DateTime endDate = DateTime.Parse(EndDatePicker.Date.ToString());
                DateTime enddatelast = endDate.Date.AddDays(1);
                string endDatestring = enddatelast.ToString("MM-dd-yyyy");
                EscalationStatus statusitem = StatusComboBox.SelectedItem as EscalationStatus;
                string status = statusitem.Status;
                Product productitem = PlatformComboBox.SelectedItem as Product;
                string plaform = productitem.Platform;
                EscalationThreadList = await VendorEscalationThreadViewModel.QueryAllEscalationAndStatusThread(AllMyPlatform, EscalatonStatusList, status,plaform, startDatestring, endDatestring);
                ComboBoxItem curItem = (ComboBoxItem)PageComboBox.SelectedItem;
                pageSize = Convert.ToInt32(curItem.Content.ToString());
                if (EscalationThreadList.Count == 0)
                {
                    AllRecords.Text = "0";
                    AllPageIndex.Text = "0";
                    PageTxt.Text = "0";
                    DataGrid.ItemsSource = EscalationThreadList;
                    MyScrollView.Height = 100;
                }

                else if (EscalationThreadList.Count < 10)
                {
                    DataGrid.ItemsSource = EscalationThreadList;
                    MyScrollView.Height = (EscalationThreadList.Count + 1) * 60;
                    AllRecords.Text = EscalationThreadList.Count.ToString();
                    AllPageIndex.Text = VendorEscalationThreadViewModel.GetPageIndex(EscalationThreadList, pageSize).ToString();
                    PageTxt.Text = VendorEscalationThreadViewModel.GetPageIndex(EscalationThreadList, pageSize).ToString();
                }

                else
                {
                    AllRecords.Text = EscalationThreadList.Count.ToString();
                    AllPageIndex.Text = VendorEscalationThreadViewModel.GetPageIndex(EscalationThreadList, pageSize).ToString();
                    int AllPagesIndex = VendorEscalationThreadViewModel.GetPageIndex(EscalationThreadList, pageSize);
                    PageTxt.Text = "1";
                    if (EscalationThreadList.Count >= 10)
                    {
                        MyScrollView.Height = 650;
                    }
                    if (AllPagesIndex == 1)
                    {
                        DataGrid.ItemsSource = EscalationThreadList;

                    }
                    else
                    {
                        var EscalationThreadListPage = EscalationThreadList.Skip(0 * pageSize).Take(pageSize).ToList();
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

        public void setScrollViewheight(List<EscalationAndStatusThread> MyList)
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
            if (EscalationThreadList.Count == 0)
            {
                this.PageTxt.Text = "0";
                MyScrollView.Height = 100;
                DataGrid.ItemsSource = null;
            }

            else
            {
                int AllPageIndex = VendorEscalationThreadViewModel.GetPageIndex(EscalationThreadList, pageSize);
                int index = Convert.ToInt32(PageTxt.Text.ToString());
                index++;
                if (index < AllPageIndex)
                {
                    var EscalationThreadListPage = EscalationThreadList.Skip((index - 1) * pageSize).Take(pageSize).ToList();
                    DataGrid.ItemsSource = EscalationThreadListPage;
                    setScrollViewheight(EscalationThreadListPage);
                    PageTxt.Text = index.ToString();
                }

                else
                {
                    var EscalationThreadListPage = EscalationThreadList.Skip((AllPageIndex - 1) * pageSize).Take(pageSize).ToList();
                    DataGrid.ItemsSource = EscalationThreadListPage;
                    setScrollViewheight(EscalationThreadListPage);
                    int count = EscalationThreadList.Count;
                    PageTxt.Text = AllPageIndex.ToString();

                }
            }
        }

        private void PreviousImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (EscalationThreadList.Count == 0)
            {
                this.PageTxt.Text = "0";
                MyScrollView.Height = 100;
                DataGrid.ItemsSource = null;
            }
            else
            {
                if (Convert.ToInt32(PageTxt.Text.ToString()) == 1)
                {
                    var EscalationThreadListPage1 = EscalationThreadList.Skip(0 * pageSize).Take(pageSize).ToList();
                    DataGrid.ItemsSource = EscalationThreadListPage1;
                    setScrollViewheight(EscalationThreadListPage1);
                    PageTxt.Text = "1";

                }
                else
                {
                    var EscalationThreadListPage = EscalationThreadList.Skip((Convert.ToInt32(PageTxt.Text.ToString())) - 1 * pageSize).Take(pageSize).ToList();
                    DataGrid.ItemsSource = EscalationThreadListPage;
                    setScrollViewheight(EscalationThreadListPage);
                    PageTxt.Text = ((Convert.ToInt32(PageTxt.Text.ToString())) - 1).ToString();

                }
            }
        }

        private void LastImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (EscalationThreadList.Count == 0)
            {
                this.PageTxt.Text = "0";
                MyScrollView.Height = 100;
                DataGrid.ItemsSource = null;
            }
            else
            {
                int AllPageIndex = VendorEscalationThreadViewModel.GetPageIndex(EscalationThreadList, pageSize);
                PageTxt.Text = AllPageIndex.ToString();
                var EscalationThreadListPage = EscalationThreadList.Skip((AllPageIndex - 1) * pageSize).Take(pageSize).ToList();
                DataGrid.ItemsSource = EscalationThreadListPage;
                setScrollViewheight(EscalationThreadListPage);
                PageTxt.Text = AllPageIndex.ToString();
            }
        }

        private void FirstImage_Tapped(object sender, TappedRoutedEventArgs e)
        {


            if (EscalationThreadList.Count == 0)
            {
                this.PageTxt.Text = "0";
                MyScrollView.Height = 100;
                DataGrid.ItemsSource = null;
            }
            else
            {
                var EscalationThreadListPage1 = EscalationThreadList.Skip(0 * pageSize).Take(pageSize).ToList();
                DataGrid.ItemsSource = EscalationThreadListPage1;
                setScrollViewheight(EscalationThreadListPage1);
                PageTxt.Text = "1";
            }
        }

        private void DataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (DataGrid.Items.Count < 10)
                {
                    MyScrollView.Height = 600;
                }
            }
            catch (Exception ex)
            {
                MyScrollView.Height = 600;
            }

        }

        private async void Search_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (MySearchEscalationThreadList.Count > 0)
            {
                MySearchEscalationThreadList.Items.Clear();
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
            ComboBoxItem curItem = (ComboBoxItem)PageComboBox.SelectedItem;
            searchPageSize = Convert.ToInt32(curItem.Content.ToString());
            try
            {
                var test = await VendorEscalationThreadViewModel.QueryAllEscalationAndStatusThread(AllMyPlatform, EscalatonStatusList, "", "", "", "");
                int i = 0;
                if (test.Items.Count > 0)
                {
        
                    foreach (var item in EscalationThreadList)
                    {
                        if (Searchtxt.Text.ToString().Contains("*"))
                        {
                            string Text = Searchtxt.Text.ToString().Trim((new Char[] { '*' }));
                            if (item.EscalationThread.ThreadId.Contains(Text))
                            {
                                MySearchEscalationThreadList.Items.Add(item);
                                i = 1;
                            }
                            else if (item.EscalationThread.Title.Contains(Text))
                            {
                                MySearchEscalationThreadList.Items.Add(item);
                                i = 1;

                            }
                        }
                        else
                        {
                            if (item.EscalationThread.ThreadId==Searchtxt.Text.ToString())
                            {
                                MySearchEscalationThreadList.Items.Add(item);
                                i = 1;
                            }
                            else if (item.EscalationThread.Title==Searchtxt.Text.ToString())
                            {
                                MySearchEscalationThreadList.Items.Add(item);
                                i = 1;

                            }

                        }

                    }
                    if (i == 1)
                    {


                        if (MySearchEscalationThreadList.Count < 10)
                        {
                            DataGrid.ItemsSource = MySearchEscalationThreadList;
                            MyScrollView.Height = (MySearchEscalationThreadList.Count + 1) * 60;
                            AllRecords.Text = MySearchEscalationThreadList.Count.ToString();
                            AllPageIndex.Text = VendorEscalationThreadViewModel.GetPageIndex(MySearchEscalationThreadList, searchPageSize).ToString();
                            PageTxt.Text = VendorEscalationThreadViewModel.GetPageIndex(MySearchEscalationThreadList, searchPageSize).ToString();
                        }

                        else
                        {
                            AllRecords.Text = MySearchEscalationThreadList.Count.ToString();
                            AllPageIndex.Text = VendorEscalationThreadViewModel.GetPageIndex(MySearchEscalationThreadList, searchPageSize).ToString();
                            int AllPagesIndex = VendorEscalationThreadViewModel.GetPageIndex(MySearchEscalationThreadList, searchPageSize);
                            PageTxt.Text = "1";
                            if (MySearchEscalationThreadList.Count >= 10)
                            {
                                MyScrollView.Height = 650;
                            }
                            if (AllPagesIndex == 1)
                            {
                                DataGrid.ItemsSource = MySearchEscalationThreadList;

                            }
                            else
                            {
                                var SearchEscalationThreadListPage = MySearchEscalationThreadList.Skip(0 * searchPageSize).Take(searchPageSize).ToList();
                                DataGrid.ItemsSource = SearchEscalationThreadListPage;

                            }
                        }
                    }
                    else
                    {
                        DateTime startDate = DateTime.Parse(StartDatePicker.Date.ToString());
                        string startDatestring = startDate.ToString("MM-dd-yyyy");
                        DateTime endDate = DateTime.Parse(EndDatePicker.Date.ToString());
                        DateTime enddatelast = endDate.Date.AddDays(1);
                        string endDatestring = enddatelast.ToString("MM-dd-yyyy");
                        EscalationStatus statusitem = StatusComboBox.SelectedItem as EscalationStatus;
                        string status = statusitem.Status;
                        Product productitem = PlatformComboBox.SelectedItem as Product;
                        string plaform = productitem.Platform;
                        var AllEscalationThreadList = await VendorEscalationThreadViewModel.QueryAllEscalationAndStatusThread(AllMyPlatform, EscalatonStatusList, status, plaform, startDatestring, endDatestring);

                        if (AllEscalationThreadList.Items.Count > 0)
                        {
                           
                            foreach (var item in AllEscalationThreadList)
                            {
                                if (item.EscalationThread.FteAlias == Searchtxt.Text.ToString())
                                {
                                    MySearchEscalationThreadList.Items.Add(item);
                                    i = 1;
                                }

                            }

                            if (i == 1)
                            {


                                if (MySearchEscalationThreadList.Count < 10)
                                {
                                    DataGrid.ItemsSource = MySearchEscalationThreadList;
                                    MyScrollView.Height = (MySearchEscalationThreadList.Count + 1) * 60;
                                    AllRecords.Text = MySearchEscalationThreadList.Count.ToString();
                                    AllPageIndex.Text = VendorEscalationThreadViewModel.GetPageIndex(MySearchEscalationThreadList, searchPageSize).ToString();
                                    PageTxt.Text = VendorEscalationThreadViewModel.GetPageIndex(MySearchEscalationThreadList, searchPageSize).ToString();
                                }

                                else
                                {
                                    AllRecords.Text = MySearchEscalationThreadList.Count.ToString();
                                    AllPageIndex.Text = VendorEscalationThreadViewModel.GetPageIndex(MySearchEscalationThreadList, searchPageSize).ToString();
                                    int AllPagesIndex = VendorEscalationThreadViewModel.GetPageIndex(MySearchEscalationThreadList, searchPageSize);
                                    PageTxt.Text = "1";
                                    if (MySearchEscalationThreadList.Count >= 10)
                                    {
                                        MyScrollView.Height = 650;
                                    }
                                    if (AllPagesIndex == 1)
                                    {
                                        DataGrid.ItemsSource = MySearchEscalationThreadList;

                                    }
                                    else
                                    {
                                        var SearchEscalationThreadList1Page = MySearchEscalationThreadList.Skip(0 * searchPageSize).Take(searchPageSize).ToList();
                                        DataGrid.ItemsSource = SearchEscalationThreadList1Page;
                                        MyProgressRing.IsActive = false;

                                    }
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
                            DataGrid.ItemsSource = null;
                            AllRecords.Text = "0";
                            AllPageIndex.Text = "0";
                            PageTxt.Text = "0";
                            MyProgressRing.IsActive = false;
                        }
                    }
                }
            }

            catch(Exception ex)
            {
                DataGrid.ItemsSource = null;
                AllRecords.Text = "0";
                AllPageIndex.Text = "0";
                PageTxt.Text = "0";
                MyProgressRing.IsActive = false;
            }
            MyProgressRing.IsActive = false;
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

        private void FirstSearchImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (MySearchEscalationThreadList.Count == 0)
            {
                this.PageTxt.Text = "0";
                MyScrollView.Height = 100;
                DataGrid.ItemsSource = null;
            }
            else
            {
                var EscalationThreadListPage1 = MySearchEscalationThreadList.Skip(0 * searchPageSize).Take(searchPageSize).ToList();
                DataGrid.ItemsSource = EscalationThreadListPage1;
                setScrollViewheight(EscalationThreadListPage1);
                PageTxt.Text = "1";

            }
        }

        private void PreviousSearchImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (MySearchEscalationThreadList.Count == 0)
            {
                this.PageTxt.Text = "0";
                MyScrollView.Height = 100;
                DataGrid.ItemsSource = null;
            }
            else
            {
                if (Convert.ToInt32(PageTxt.Text.ToString()) == 1)
                {
                    var EscalationThreadListPage1 = MySearchEscalationThreadList.Skip(0 * searchPageSize).Take(searchPageSize).ToList();
                    DataGrid.ItemsSource = EscalationThreadListPage1;
                    setScrollViewheight(EscalationThreadListPage1);
                    PageTxt.Text = "1";

                }
                else
                {
                    var EscalationThreadListPage = MySearchEscalationThreadList.Skip((Convert.ToInt32(PageTxt.Text.ToString())) - 1 * searchPageSize).Take(searchPageSize).ToList();
                    DataGrid.ItemsSource = EscalationThreadListPage;
                    setScrollViewheight(EscalationThreadListPage);
                    PageTxt.Text = ((Convert.ToInt32(PageTxt.Text.ToString())) - 1).ToString();



                }
            }
        }

        private void NextSearchImage_Tapped(object sender, TappedRoutedEventArgs e)
        {


            if (MySearchEscalationThreadList.Count == 0)
            {
                this.PageTxt.Text = "0";
                MyScrollView.Height = 100;
                DataGrid.ItemsSource = null;
            }

            else
            {
                int AllPageIndex = VendorEscalationThreadViewModel.GetPageIndex(MySearchEscalationThreadList, searchPageSize);
                int index = Convert.ToInt32(PageTxt.Text.ToString());
                index++;
                if (index < AllPageIndex)
                {
                    var EscalationThreadListPage = MySearchEscalationThreadList.Skip((index - 1) * searchPageSize).Take(searchPageSize).ToList();
                    DataGrid.ItemsSource = EscalationThreadListPage;
                    setScrollViewheight(EscalationThreadListPage);
                    PageTxt.Text = index.ToString();

                }

                else
                {
                    var EscalationThreadListPage = MySearchEscalationThreadList.Skip((AllPageIndex - 1) * searchPageSize).Take(searchPageSize).ToList();
                    DataGrid.ItemsSource = EscalationThreadListPage;
                    setScrollViewheight(EscalationThreadListPage);
                    int count = MySearchEscalationThreadList.Count;
                    PageTxt.Text = AllPageIndex.ToString();


                }
            }
        }

        private void LastSearchImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (MySearchEscalationThreadList.Count == 0)
            {
                this.PageTxt.Text = "0";
                MyScrollView.Height = 100;
                DataGrid.ItemsSource = null;
            }
            else
            {
                int AllPageIndex = VendorEscalationThreadViewModel.GetPageIndex(MySearchEscalationThreadList, searchPageSize);
                PageTxt.Text = AllPageIndex.ToString();
                var EscalationThreadListPage = MySearchEscalationThreadList.Skip((AllPageIndex - 1) * searchPageSize).Take(searchPageSize).ToList();
                DataGrid.ItemsSource = EscalationThreadListPage;
                setScrollViewheight(EscalationThreadListPage);
                PageTxt.Text = AllPageIndex.ToString();
            }
        }
    }
    
    public class URLConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            Uri Myuri = new Uri(value.ToString());
            return Myuri;
        }
        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}


