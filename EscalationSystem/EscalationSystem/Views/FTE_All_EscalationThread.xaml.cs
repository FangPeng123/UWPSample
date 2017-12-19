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
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using EscalationSystem.Models;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.ComponentModel;
using EscalationSystem.ViewModels;
using MyToolkit.Collections;
using Windows.UI;
using MyToolkit.Controls;
using Windows.Web.Http;
using Newtonsoft.Json;
using Windows.UI.Popups;
using System.Threading.Tasks;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace EscalationSystem.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 

    public sealed partial class FTE_All_EscalationThread : Page
    {

        public EscalationStatusWithSelectedItem EscalatonStatusList { get; set; }
        public ProductWithSelectedItem AllMyPlatform { get; set; }
        public ObservableCollectionView<EscalationAndStatusThread> EscalationThreadList { get; set; }
        public ObservableCollectionView<EscalationAndStatusThread> EscalationThreadListPage { get; set; }
        public EscalationThread EscalationThread { get; set; }
        public FTEEscalationThreadViewModel FTEEscalationThreadViewModel{get;set;}
        public int pageSize;
        public static int i = 0;
        public static int tag = 0;
        public FTE_All_EscalationThread()
        {
            this.InitializeComponent();
            this.EndDatePicker.Date = DateTime.Today;
            int date = DateTime.Today.Day;
            this.StartDatePicker.Date = DateTime.Today.AddDays(-(date-1));
            this.SizeChanged += FTE_All_EscalationThread_SizeChanged;
            EscalatonStatusList = new EscalationStatusWithSelectedItem();
            AllMyPlatform = new ProductWithSelectedItem();
            EscalationThreadList = new ObservableCollectionView<EscalationAndStatusThread>();
            EscalationThreadListPage = new ObservableCollectionView<EscalationAndStatusThread>();
            FTEEscalationThreadViewModel = new FTEEscalationThreadViewModel();
            EscalationThread = new EscalationThread();
            this.Loaded += FTE_All_EscalationThread_Loaded;
            this.DataContext = FTEEscalationThreadViewModel;

        }

      

        private async void FTE_All_EscalationThread_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                FTEEscalationThreadViewModel = await FTEEscalationThreadViewModel.GetFTEEscalationThreadViewModel();
                EscalatonStatusList = FTEEscalationThreadViewModel.AllEscalationStatusList;
                StatusComboBox.DataContext = EscalatonStatusList;
                AllMyPlatform = FTEEscalationThreadViewModel.AllPratfromList;
                PlatformComboBox.DataContext = AllMyPlatform;
                PageComboBox.SelectedIndex = 0;
                QueryButton_Click(sender, e);
            }
            catch(Exception ex)
            {
                MessageDialog messageDialog = new MessageDialog(ex.Message.ToString());
                await messageDialog.ShowAsync();
                MyProgressRing.IsActive = false;
            }

        }


        private void FTE_All_EscalationThread_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SceenSizeViewModel.ScreenWidth = this.ActualWidth;
        }

        private async void QueryButton_Click(object sender, RoutedEventArgs e)
        {

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

                    tag = 1;
                    MyProgressRing.IsActive = true;
                    DateTime startDate = DateTime.Parse(StartDatePicker.Date.ToString());
                    string startDatestring = startDate.ToString("MM-dd-yyyy");
                    DateTime endDate = DateTime.Parse(EndDatePicker.Date.ToString());
                    DateTime enddatelast = endDate.Date.AddDays(1);
                    string endDatestring = enddatelast.ToString("MM-dd-yyyy");
                    EscalationThreadList = await FTEEscalationThreadViewModel.QueryAllEscalationAndStatusThread(AllMyPlatform, EscalatonStatusList, startDatestring, endDatestring);

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
                        AllPageIndex.Text = FTEEscalationThreadViewModel.GetPageIndex(EscalationThreadList, pageSize).ToString();
                        PageTxt.Text = FTEEscalationThreadViewModel.GetPageIndex(EscalationThreadList, pageSize).ToString();
                    }

                    else
                    {
                        AllRecords.Text = EscalationThreadList.Count.ToString();
                        AllPageIndex.Text = FTEEscalationThreadViewModel.GetPageIndex(EscalationThreadList, pageSize).ToString();
                        int AllPagesIndex = FTEEscalationThreadViewModel.GetPageIndex(EscalationThreadList, pageSize);
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
                MessageDialog messageDialog = new MessageDialog(ex.Message.ToString());
                await messageDialog.ShowAsync();
                MyProgressRing.IsActive = false;
            }
        }

        public void setScrollViewheight(List<EscalationAndStatusThread> MyList)
            {
             if(MyList.Count>=10)
               {
                MyScrollView.Height = 650;
                }
             else
              {
                MyScrollView.Height = (MyList.Count+1) * 60;
              }
            }

        private void DataGridComboBoxStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox DataGridComboBoxStatus = new ComboBox();
            DataGridComboBoxStatus = sender as ComboBox;
            EscalationStatus statusitem = DataGridComboBoxStatus.SelectedItem as EscalationStatus;
            EscalationAndStatusThread escalationAndStatusThread = DataGridComboBoxStatus.DataContext as EscalationAndStatusThread;
            EscalationThread = escalationAndStatusThread.EscalationThread;
            EscalationThread.Status = statusitem.Status;
            if (statusitem.Status.Equals("Closed: Escalated"))
            {
                EscalationPopup.IsOpen = true;
               
            }
            else
            {
                FTEEscalationThreadViewModel.ModifyStatus(EscalationThread);
            }
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            EscalationPopup.IsOpen = false;
            EscalationThread.SrescalationId = SRTextBox.Text.ToString();
            FTEEscalationThreadViewModel.ModifyStatus(EscalationThread);          
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            EscalationPopup.IsOpen = false;           
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
                int AllPageIndex = FTEEscalationThreadViewModel.GetPageIndex(EscalationThreadList, pageSize);
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
                int AllPageIndex = FTEEscalationThreadViewModel.GetPageIndex(EscalationThreadList, pageSize);
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
            var it = DataGrid.Items.Count;
            if (DataGrid.Items.Count < 10)
            {
                MyScrollView.Height = 600;
            }
           
      
        }
    }
}


