using EscalationSystem.Models;
using EscalationSystem.ViewModels;
using MyToolkit.Collections;
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
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.ComponentModel;
using Windows.UI;
using MyToolkit.Controls;
using Windows.Web.Http;
using Newtonsoft.Json;
using Windows.UI.Popups;
using System.Threading.Tasks;
using Windows.UI.Core;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace EscalationSystem.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class FTE_Consult_Page : Page
    {
        //public EscalationStatusWithSelectedItem EscalatonStatusList { get; set; }
        public ProductWithSelectedItem AllMyPlatform { get; set; }
        public ObservableCollectionView<ConsultThread> ConsultThreadListPage { get; set; }
        public ObservableCollectionView<ConsultThread> ConsultThreadList { get; set; }

        public ObservableCollectionView<ConsultThread> MySearchConsultThreadListPage { get; set; }
        public ObservableCollectionView<ConsultThread> MySearchConsultThreadList { get; set; }
        //public EscalationThread EscalationThread { get; set; }
        public FTEConsultThreadViewModel FTEConsultThreadViewModel { get; set; }
        public int pageSize;
        public int searchPageSize;
        public static int i = 0;
     
        public FTE_Consult_Page()
        {
            this.InitializeComponent();
            this.EndDatePicker.Date = DateTime.Today;
            int date = DateTime.Today.Day;
            this.StartDatePicker.Date = DateTime.Today.AddDays(-(date - 1));
            this.SizeChanged += FTE_Consult_Page_SizeChanged;
            AllMyPlatform = new ProductWithSelectedItem();
            ConsultThreadList= new ObservableCollectionView<ConsultThread>();
            ConsultThreadListPage = new ObservableCollectionView<ConsultThread>();
            MySearchConsultThreadList = new ObservableCollectionView<ConsultThread>();
            MySearchConsultThreadListPage = new ObservableCollectionView<ConsultThread>();
            FTEConsultThreadViewModel = new FTEConsultThreadViewModel();
            this.Loaded += FTE_Consult_Page_Loaded;
            this.DataContext = FTEConsultThreadViewModel;
        }
        private async void FTE_Consult_Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                FTEConsultThreadViewModel = await FTEConsultThreadViewModel.GetFTEConsultThreadViewModel();

                AllMyPlatform = FTEConsultThreadViewModel.AllPratfromList;
                PlatformComboBox.DataContext = AllMyPlatform;

                ObservableCollection<Product> ProductList = new ObservableCollection<Product>();
                foreach (var item in AllMyPlatform.MyProductList)
                {

                    if (item.Platform != "All")
                    {
                        ProductList.Add(item);
                    }
                }
                AllMyPlatform.MyProductList = ProductList;
                AddPanelPlatformCombox.DataContext = AllMyPlatform;
                FTEComboBox.DataContext =await FTEConsultThreadViewModel.GetFTEList();
                FTEComboBox.SelectedIndex = 0;
                PageComboBox.SelectedIndex = 0;
                if (FTEComboBox.DataContext == null || PlatformComboBox.DataContext == null)
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

        private void FTE_Consult_Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SceenSizeViewModel.ScreenWidth = this.ActualWidth;
        }

        private async void QueryButton_Click(object sender, RoutedEventArgs e)
        {
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
                MyProgressRing.IsActive = true;
                DataGrid.ItemsSource = null;
                AllRecords.Text = "0";
                AllPageIndex.Text = "0";
                PageTxt.Text = "0";
                DateTime startDate = DateTime.Parse(StartDatePicker.Date.ToString());
                string startDatestring = startDate.ToString("MM-dd-yyyy");
                DateTime endDate = DateTime.Parse(EndDatePicker.Date.ToString());
                DateTime enddatelast = endDate.Date.AddDays(1);
                string endDatestring = enddatelast.ToString("MM-dd-yyyy");
                Product product = (Product)PlatformComboBox.SelectedValue;
                var alias = FTEComboBox.SelectedValue.ToString();
                string platform = product.Platform;
                ConsultThreadList = await FTEConsultThreadViewModel.QueryAllConsultThread(alias,platform, startDatestring, endDatestring);
                ComboBoxItem curItem = (ComboBoxItem)PageComboBox.SelectedItem;
                pageSize = Convert.ToInt32(curItem.Content.ToString());
                if (ConsultThreadList.Count == 0)
                {
                    AllRecords.Text = "0";
                    AllPageIndex.Text = "0";
                    PageTxt.Text = "0";
                    DataGrid.ItemsSource = ConsultThreadList;
                    MyScrollView.Height = 100;
                }

                else if (ConsultThreadList.Count < 10)
                {
                    DataGrid.ItemsSource = ConsultThreadList;
                    MyScrollView.Height = (ConsultThreadList.Count + 1) * 55;
                    AllRecords.Text = ConsultThreadList.Count.ToString();
                    AllPageIndex.Text = FTEConsultThreadViewModel.GetPageIndex(ConsultThreadList, pageSize).ToString();
                    PageTxt.Text = FTEConsultThreadViewModel.GetPageIndex(ConsultThreadList, pageSize).ToString();
                    if (ShowSearchPanel.Visibility == Visibility.Visible && AddRecordsPanle.Visibility == Visibility.Visible)
                    {
                        MyScrollView.Height = 350;
                    }
                    else if (ShowSearchPanel.Visibility == Visibility.Visible)
                    {
                        MyScrollView.Height = 500;
                    }
                    else if (AddRecordsPanle.Visibility == Visibility.Visible)
                    {
                        MyScrollView.Height = 400;

                    }

                    else
                    {
                        MyScrollView.Height = 650;
                    }
                }

                else
                {
                    AllRecords.Text = ConsultThreadList.Count.ToString();
                    AllPageIndex.Text = FTEConsultThreadViewModel.GetPageIndex(ConsultThreadList, pageSize).ToString();
                    int AllPagesIndex = FTEConsultThreadViewModel.GetPageIndex(ConsultThreadList, pageSize);
                    PageTxt.Text = "1";
                    if (ConsultThreadList.Count >= 10)
                    {
                        if (ShowSearchPanel.Visibility == Visibility.Visible && AddRecordsPanle.Visibility == Visibility.Visible)
                        {
                            MyScrollView.Height = 350;
                        }
                        else
                        {
                            MyScrollView.Height = 500;
                        }
                    }
                    if (AllPagesIndex == 1)
                    {
                        DataGrid.ItemsSource = ConsultThreadList;

                    }
                    else
                    {
                        var ConsultThreadListPage = ConsultThreadList.Skip(0 * pageSize).Take(pageSize).ToList();
                        DataGrid.ItemsSource = ConsultThreadListPage;

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
            }
        }

        public void setScrollViewheight(List<ConsultThread> MyList)
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
            if (ConsultThreadList.Count == 0)
            {
                this.PageTxt.Text = "0";
                MyScrollView.Height = 100;
                DataGrid.ItemsSource = null;
            }

            else
            {
                int AllPageIndex = FTEConsultThreadViewModel.GetPageIndex(ConsultThreadList, pageSize);
                int index = Convert.ToInt32(PageTxt.Text.ToString());
                index++;
                if (index < AllPageIndex)
                {
                    var ConsultThreadListPage = ConsultThreadList.Skip((index - 1) * pageSize).Take(pageSize).ToList();
                    DataGrid.ItemsSource = ConsultThreadListPage;
                    setScrollViewheight(ConsultThreadListPage);
                    PageTxt.Text = index.ToString();
                    if (ShowSearchPanel.Visibility == Visibility.Visible && AddRecordsPanle.Visibility == Visibility.Visible)
                    {
                        MyScrollView.Height = 350;
                    }
                    else if (ShowSearchPanel.Visibility == Visibility.Visible)
                    {
                        MyScrollView.Height = 500;
                    }
                    else if (AddRecordsPanle.Visibility == Visibility.Visible)
                    {
                        MyScrollView.Height = 400;

                    }

                    else
                    {
                        MyScrollView.Height = 650;
                    }
                }

                else
                {
                    var ConsultThreadListPage = ConsultThreadList.Skip((AllPageIndex - 1) * pageSize).Take(pageSize).ToList();
                    DataGrid.ItemsSource = ConsultThreadListPage;
                    setScrollViewheight(ConsultThreadListPage);
                    int count = ConsultThreadList.Count;
                    PageTxt.Text = AllPageIndex.ToString();
                    if (ShowSearchPanel.Visibility == Visibility.Visible && AddRecordsPanle.Visibility == Visibility.Visible)
                    {
                        MyScrollView.Height = 350;
                    }
                    else if (ShowSearchPanel.Visibility == Visibility.Visible)
                    {
                        MyScrollView.Height = 500;
                    }
                    else if (AddRecordsPanle.Visibility == Visibility.Visible)
                    {
                        MyScrollView.Height = 400;

                    }

                    else
                    {
                        MyScrollView.Height = 650;
                    }

                }
            }
        }

        private void PreviousImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (ConsultThreadList.Count == 0)
            {
                this.PageTxt.Text = "0";
                MyScrollView.Height = 100;
                DataGrid.ItemsSource = null;
            }
            else
            {
                if (Convert.ToInt32(PageTxt.Text.ToString()) == 1)
                {
                    var ConsultThreadListPage1 = ConsultThreadList.Skip(0 * pageSize).Take(pageSize).ToList();
                    DataGrid.ItemsSource = ConsultThreadListPage1;
                    setScrollViewheight(ConsultThreadListPage1);
                    PageTxt.Text = "1";

                }
                else
                {
                    var ConsultThreadListPage = ConsultThreadList.Skip((Convert.ToInt32(PageTxt.Text.ToString())) - 1 * pageSize).Take(pageSize).ToList();
                    DataGrid.ItemsSource = ConsultThreadListPage;
                    setScrollViewheight(ConsultThreadListPage);
                    PageTxt.Text = ((Convert.ToInt32(PageTxt.Text.ToString())) - 1).ToString();

                    if (ShowSearchPanel.Visibility == Visibility.Visible && AddRecordsPanle.Visibility == Visibility.Visible)
                    {
                        MyScrollView.Height = 350;
                    }
                    else if (ShowSearchPanel.Visibility == Visibility.Visible)
                    {
                        MyScrollView.Height = 500;
                    }
                    else if (AddRecordsPanle.Visibility == Visibility.Visible)
                    {
                        MyScrollView.Height = 400;

                    }

                    else
                    {
                        MyScrollView.Height = 650;
                    }

                }
            }
        }

   
        private void LastImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (ConsultThreadList.Count == 0)
            {
                this.PageTxt.Text = "0";
                MyScrollView.Height = 100;
                DataGrid.ItemsSource = null;
            }
            else
            {
                int AllPageIndex = FTEConsultThreadViewModel.GetPageIndex(ConsultThreadList, pageSize);
                PageTxt.Text = AllPageIndex.ToString();
                var ConsultThreadListPage = ConsultThreadList.Skip((AllPageIndex - 1) * pageSize).Take(pageSize).ToList();
                DataGrid.ItemsSource = ConsultThreadListPage;
                setScrollViewheight(ConsultThreadListPage);
                PageTxt.Text = AllPageIndex.ToString();
            }
        }

        private void FirstImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (ConsultThreadList.Count == 0)
            {
                this.PageTxt.Text = "0";
                MyScrollView.Height = 100;
                DataGrid.ItemsSource = null;
            }
            else
            {
                var ConsultThreadListPage1 = ConsultThreadList.Skip(0 * pageSize).Take(pageSize).ToList();
                DataGrid.ItemsSource = ConsultThreadListPage1;
                setScrollViewheight(ConsultThreadListPage1);
                PageTxt.Text = "1";
                if (ShowSearchPanel.Visibility == Visibility.Visible && AddRecordsPanle.Visibility == Visibility.Visible)
                {
                    MyScrollView.Height = 350;
                }
                else if (ShowSearchPanel.Visibility == Visibility.Visible)
                {
                    MyScrollView.Height = 500;
                }
                else if (AddRecordsPanle.Visibility == Visibility.Visible)
                {
                    MyScrollView.Height = 400;

                }

                else
                {
                    MyScrollView.Height = 650;
                }
            }
        }

        static int count = 0;

        private void ShowAddImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            count = count + 1;
            if(count%2==0)
            {
                RotateTransform rotateTransform = new RotateTransform();
                rotateTransform.Angle = 360;
                rotateTransform.CenterX = 10;
                rotateTransform.CenterY = 10;
                ShowAddImage.RenderTransform = rotateTransform;
                AddRecordsPanle.Visibility = Visibility.Collapsed;
                MyScrollView.Height = 500;
            }
            else
            {

                RotateTransform rotateTransform = new RotateTransform();
                rotateTransform.Angle = 90;
                rotateTransform.CenterX = 10;
                rotateTransform.CenterY = 10;
                ShowAddImage.RenderTransform = rotateTransform;
                AddRecordsPanle.Visibility = Visibility.Visible;
                MyScrollView.Height = 400;
            }
           
         
        }

        private async void AddPanelPlatformCombox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ForumStackPanel.Visibility = Visibility.Visible;
            Product product = (Product)AddPanelPlatformCombox.SelectedValue;
            ObservableCollection<string> Allforum = await FTEConsultThreadViewModel.GetAllForum(product.Platform);
            AddPanelForumCombox.DataContext = Allforum;           
        }


        private async void AddConsultThread_Tapped(object sender, TappedRoutedEventArgs e)
        {
           await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(CoreDispatcherPriority.Normal,
            () =>
              {
                 AddProgressRing.IsActive = true;
              });

            try
                {
                    if (string.IsNullOrEmpty(CaseLinkTxt.Text.ToString()) == false && string.IsNullOrEmpty(VendorAliasTxt.Text.ToString()) == false && AddPanelPlatformCombox.SelectedIndex >= 0 && AddPanelForumCombox.SelectedIndex >= 0)
                    {
                        ConsultThread consultThread = new ConsultThread();
                        consultThread.Description = "N/A";
                        consultThread.Forum = AddPanelForumCombox.SelectedValue.ToString();
                        consultThread.Platform = ((Product)AddPanelPlatformCombox.SelectedValue).Platform;
                        consultThread.Reason = "N/A";
                        consultThread.SrescalationId = "N/A";
                        consultThread.Status = "N/A";
                        consultThread.Title = "N/A";
                        consultThread.ThreadId = ThreadIDTxt.Text.ToString();
                        consultThread.Url = CaseLinkTxt.Text.ToString();
                        Windows.Storage.ApplicationDataContainer LocalSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
                        string userAlias = LocalSettings.Values["currentUserAlias"].ToString();
                        consultThread.FteAlias = userAlias;
                        consultThread.VendorAlias = VendorAliasTxt.Text.ToString();
                        consultThread.EscalatedDatetime = DateTime.Now;
                        consultThread.IsManaged = false;
                        consultThread.Labor = "N/A";
                        consultThread.LastreplyDatetime = DateTime.Now;
                        consultThread.LastreplyFromOp = false;
                        consultThread.ThreadCreatedDatetime = DateTime.Now;
                        FTEConsultThreadViewModel.AddConsultThread(consultThread);
                        AddProgressRing.IsActive = false;

                    }

                    else
                    {
                        AddProgressRing.IsActive = false;
                    MessageDialog messageDialog = new MessageDialog("Please fill all the fields!!!");
                    await messageDialog.ShowAsync();

                }
                }
                catch (Exception ex)
                {
                    AddProgressRing.IsActive = false;
                    MessageDialog messageDialog = new MessageDialog(ex.Message.ToString());
                    await messageDialog.ShowAsync();
                    MyProgressRing.IsActive = false;

                }

            
        }

       
        private void ShowQueryImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            
            if (ShowSearchPanel.Visibility == Visibility.Collapsed)
            {
                ShowSearchPanel.Visibility = Visibility.Visible;
                if (AddRecordsPanle.Visibility==Visibility.Collapsed)
                {
                    MyScrollView.Height = 600;
                }
                else
                {
                    MyScrollView.Height = 350;
                }
            }
            else
            {
                ShowSearchPanel.Visibility = Visibility.Collapsed;
               
            }
        }

        private async void Search_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (MySearchConsultThreadList.Count > 0)
            {
                MySearchConsultThreadList.Items.Clear();
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
                var test = await FTEConsultThreadViewModel.QueryAllConsultThread( "", "", "", "");
                int i = 0;
                if (test.Items.Count > 0)
                {
                   
                    foreach (var item in test)
                    {
                        if (Searchtxt.Text.ToString().Contains("*"))
                        {
                            string Text = Searchtxt.Text.ToString().Trim((new Char[] { '*' }));
                            if (item.ThreadId.Contains(Text))
                            {
                                MySearchConsultThreadList.Items.Add(item);
                                i = 1;
                            }
                            else if (item.Title.Contains(Text))
                            {
                                MySearchConsultThreadList.Items.Add(item);
                                i = 1;

                            }
                        }
                        else
                        {
                            if (item.ThreadId == Searchtxt.Text.ToString())
                            {
                                MySearchConsultThreadList.Items.Add(item);
                                i = 1;
                            }
                            else if (item.Title == Searchtxt.Text.ToString())
                            {
                                MySearchConsultThreadList.Items.Add(item);
                                i = 1;

                            }

                        }

                    }
                    if (i == 1)
                    {


                        if (MySearchConsultThreadList.Count < 10)
                        {
                            DataGrid.ItemsSource = MySearchConsultThreadList;
                            MyScrollView.Height = (MySearchConsultThreadList.Count + 1) * 60;
                            AllRecords.Text = MySearchConsultThreadList.Count.ToString();
                            AllPageIndex.Text = FTEConsultThreadViewModel.GetPageIndex(MySearchConsultThreadList, searchPageSize).ToString();
                            PageTxt.Text = FTEConsultThreadViewModel.GetPageIndex(MySearchConsultThreadList, searchPageSize).ToString();

                            if(ShowSearchPanel.Visibility==Visibility.Visible&&AddRecordsPanle.Visibility==Visibility.Visible)
                            {
                                MyScrollView.Height = 350;
                            }
                            else if(ShowSearchPanel.Visibility==Visibility.Visible)
                            {
                                MyScrollView.Height = 500;
                            }
                             else if(AddRecordsPanle.Visibility==Visibility.Visible)
                            {
                                MyScrollView.Height = 400;

                            }

                            else
                            {
                                MyScrollView.Height = 650;
                            }
                        }

                        else
                        {
                            AllRecords.Text = MySearchConsultThreadList.Count.ToString();
                            AllPageIndex.Text = FTEConsultThreadViewModel.GetPageIndex(MySearchConsultThreadList, searchPageSize).ToString();
                            int AllPagesIndex = FTEConsultThreadViewModel.GetPageIndex(MySearchConsultThreadList, searchPageSize);
                            PageTxt.Text = "1";
                            if (MySearchConsultThreadList.Count >= 10)
                            {

                                if (ShowSearchPanel.Visibility == Visibility.Visible && AddRecordsPanle.Visibility == Visibility.Visible)
                                {
                                    MyScrollView.Height = 350;
                                }
                                else
                                {
                                    MyScrollView.Height = 650;
                                }
                            }
                            if (AllPagesIndex == 1)
                            {
                                DataGrid.ItemsSource = MySearchConsultThreadList;

                            }
                            else
                            {
                                var SearchConsultThreadListPage = MySearchConsultThreadList.Skip(0 * searchPageSize).Take(searchPageSize).ToList();
                                DataGrid.ItemsSource = SearchConsultThreadListPage;

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
                        Product product = (Product)PlatformComboBox.SelectedValue;
                        var alias = FTEComboBox.SelectedValue.ToString();
                        string platform = product.Platform;
                        var AllConsultThreadList = await FTEConsultThreadViewModel.QueryAllConsultThread(alias, platform, startDatestring, endDatestring);

                        if (AllConsultThreadList.Items.Count > 0)
                        {
                            
                            foreach (var item in AllConsultThreadList)
                            {
                                if (item.FteAlias == Searchtxt.Text.ToString())
                                {
                                    MySearchConsultThreadList.Items.Add(item);
                                    i = 1;
                                }

                                else if (item.VendorAlias == Searchtxt.Text.ToString())
                                {
                                    MySearchConsultThreadList.Items.Add(item);
                                    i = 1;
                                }
                            }

                            if (i == 1)
                            {


                                if (MySearchConsultThreadList.Count < 10)
                                {
                                    DataGrid.ItemsSource = MySearchConsultThreadList;
                                    MyScrollView.Height = (MySearchConsultThreadList.Count + 1) * 60;
                                    AllRecords.Text = MySearchConsultThreadList.Count.ToString();
                                    AllPageIndex.Text = FTEConsultThreadViewModel.GetPageIndex(MySearchConsultThreadList, searchPageSize).ToString();
                                    PageTxt.Text = FTEConsultThreadViewModel.GetPageIndex(MySearchConsultThreadList, searchPageSize).ToString();
                                    if (ShowSearchPanel.Visibility == Visibility.Visible && AddRecordsPanle.Visibility == Visibility.Visible)
                                    {
                                        MyScrollView.Height = 350;
                                    }
                                    else if (ShowSearchPanel.Visibility == Visibility.Visible)
                                    {
                                        MyScrollView.Height = 500;
                                    }
                                    else if (AddRecordsPanle.Visibility == Visibility.Visible)
                                    {
                                        MyScrollView.Height = 400;

                                    }

                                    else
                                    {
                                        MyScrollView.Height = 650;
                                    }
                                }

                                else
                                {
                                    AllRecords.Text = MySearchConsultThreadList.Count.ToString();
                                    AllPageIndex.Text = FTEConsultThreadViewModel.GetPageIndex(MySearchConsultThreadList, searchPageSize).ToString();
                                    int AllPagesIndex = FTEConsultThreadViewModel.GetPageIndex(MySearchConsultThreadList, searchPageSize);
                                    PageTxt.Text = "1";
                                    if (MySearchConsultThreadList.Count >= 10)
                                    {
                                        if (ShowSearchPanel.Visibility == Visibility.Visible && AddRecordsPanle.Visibility == Visibility.Visible)
                                        {
                                            MyScrollView.Height = 350;
                                        }
                                        else
                                        {
                                            MyScrollView.Height = 650;
                                        }
                                    }
                                    if (AllPagesIndex == 1)
                                    {
                                        DataGrid.ItemsSource = MySearchConsultThreadList;

                                    }
                                    else
                                    {
                                        var SearchConsultThreadList1Page = MySearchConsultThreadList.Skip(0 * searchPageSize).Take(searchPageSize).ToList();
                                        DataGrid.ItemsSource = SearchConsultThreadList1Page;

                                    }
                                }
                            }
                            else
                            {
                                DataGrid.ItemsSource = null;
                                AllRecords.Text = "0";
                                AllPageIndex.Text = "0";
                                PageTxt.Text = "0";
                            }
                        }

                        else
                        {
                            DataGrid.ItemsSource = null;
                            AllRecords.Text = "0";
                            AllPageIndex.Text = "0";
                            PageTxt.Text = "0";
                        }
                    }
                }
            }

            catch
            {
                DataGrid.ItemsSource = null;
                AllRecords.Text = "0";
                AllPageIndex.Text = "0";
                PageTxt.Text = "0";
            }

            MyProgressRing.IsActive = false;
        }

        private void FirstSearchImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (MySearchConsultThreadList.Count == 0)
            {
                this.PageTxt.Text = "0";
                MyScrollView.Height = 100;
                DataGrid.ItemsSource = null;
            }
            else
            {
                var ConsultThreadListPage1 = MySearchConsultThreadList.Skip(0 * searchPageSize).Take(searchPageSize).ToList();
                DataGrid.ItemsSource = ConsultThreadListPage1;
                setScrollViewheight(ConsultThreadListPage1);
                PageTxt.Text = "1";
                if (ShowSearchPanel.Visibility == Visibility.Visible && AddRecordsPanle.Visibility == Visibility.Visible)
                {
                    MyScrollView.Height = 350;
                }
                else if (ShowSearchPanel.Visibility == Visibility.Visible)
                {
                    MyScrollView.Height = 500;
                }
                else if (AddRecordsPanle.Visibility == Visibility.Visible)
                {
                    MyScrollView.Height = 400;

                }

                else
                {
                    MyScrollView.Height = 650;
                }
            }
        }

        private void PreviousSearchImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (MySearchConsultThreadList.Count == 0)
            {
                this.PageTxt.Text = "0";
                MyScrollView.Height = 100;
                DataGrid.ItemsSource = null;
            }
            else
            {
                if (Convert.ToInt32(PageTxt.Text.ToString()) == 1)
                {
                    var ConsultThreadListPage1 = MySearchConsultThreadList.Skip(0 * searchPageSize).Take(searchPageSize).ToList();
                    DataGrid.ItemsSource = ConsultThreadListPage1;
                    setScrollViewheight(ConsultThreadListPage1);
                    PageTxt.Text = "1";

                }
                else
                {
                    var ConsultThreadListPage = MySearchConsultThreadList.Skip((Convert.ToInt32(PageTxt.Text.ToString())) - 1 * searchPageSize).Take(searchPageSize).ToList();
                    DataGrid.ItemsSource = ConsultThreadListPage;
                    setScrollViewheight(ConsultThreadListPage);
                    PageTxt.Text = ((Convert.ToInt32(PageTxt.Text.ToString())) - 1).ToString();

                    if (ShowSearchPanel.Visibility == Visibility.Visible && AddRecordsPanle.Visibility == Visibility.Visible)
                    {
                        MyScrollView.Height = 350;
                    }
                    else if (ShowSearchPanel.Visibility == Visibility.Visible)
                    {
                        MyScrollView.Height = 500;
                    }
                    else if (AddRecordsPanle.Visibility == Visibility.Visible)
                    {
                        MyScrollView.Height = 400;

                    }

                    else
                    {
                        MyScrollView.Height = 650;
                    }

                }
            }
        }

        private void NextSearchImage_Tapped(object sender, TappedRoutedEventArgs e)
        {


            if (MySearchConsultThreadList.Count == 0)
            {
                this.PageTxt.Text = "0";
                MyScrollView.Height = 100;
                DataGrid.ItemsSource = null;
            }

            else
            {
                int AllPageIndex = FTEConsultThreadViewModel.GetPageIndex(MySearchConsultThreadList, searchPageSize);
                int index = Convert.ToInt32(PageTxt.Text.ToString());
                index++;
                if (index < AllPageIndex)
                {
                    var ConsultThreadListPage = MySearchConsultThreadList.Skip((index - 1) * searchPageSize).Take(searchPageSize).ToList();
                    DataGrid.ItemsSource = ConsultThreadListPage;
                    setScrollViewheight(ConsultThreadListPage);
                    PageTxt.Text = index.ToString();
                    if (ShowSearchPanel.Visibility == Visibility.Visible && AddRecordsPanle.Visibility == Visibility.Visible)
                    {
                        MyScrollView.Height = 350;
                    }
                    else if (ShowSearchPanel.Visibility == Visibility.Visible)
                    {
                        MyScrollView.Height = 500;
                    }
                    else if (AddRecordsPanle.Visibility == Visibility.Visible)
                    {
                        MyScrollView.Height = 400;

                    }

                    else
                    {
                        MyScrollView.Height = 650;
                    }
                }

                else
                {
                    var ConsultThreadListPage = MySearchConsultThreadList.Skip((AllPageIndex - 1) * searchPageSize).Take(searchPageSize).ToList();
                    DataGrid.ItemsSource = ConsultThreadListPage;
                    setScrollViewheight(ConsultThreadListPage);
                    int count = MySearchConsultThreadList.Count;
                    PageTxt.Text = AllPageIndex.ToString();
                    if (ShowSearchPanel.Visibility == Visibility.Visible && AddRecordsPanle.Visibility == Visibility.Visible)
                    {
                        MyScrollView.Height = 350;
                    }
                    else if (ShowSearchPanel.Visibility == Visibility.Visible)
                    {
                        MyScrollView.Height = 500;
                    }
                    else if (AddRecordsPanle.Visibility == Visibility.Visible)
                    {
                        MyScrollView.Height = 400;

                    }

                    else
                    {
                        MyScrollView.Height = 650;
                    }

                }
            }
        }

        private void LastSearchImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (MySearchConsultThreadList.Count == 0)
            {
                this.PageTxt.Text = "0";
                MyScrollView.Height = 100;
                DataGrid.ItemsSource = null;
            }
            else
            {
                int AllPageIndex = FTEConsultThreadViewModel.GetPageIndex(MySearchConsultThreadList, searchPageSize);
                PageTxt.Text = AllPageIndex.ToString();
                var ConsultThreadListPage = MySearchConsultThreadList.Skip((AllPageIndex - 1) * searchPageSize).Take(searchPageSize).ToList();
                DataGrid.ItemsSource = ConsultThreadListPage;
                setScrollViewheight(ConsultThreadListPage);
                PageTxt.Text = AllPageIndex.ToString();
            }
        }
    }
}