using EscalationSystem.Models;
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
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace EscalationSystem.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>

    public sealed partial class Vendor_All_EscalationThread : Page
    {


        public ObservableCollection<EscalationStatus> EscalatonStatusList { get; set; }
        public ObservableCollection<Product> AllMyPlatform { get; set; }
        public ObservableCollectionView<EscalationAndStatusThread> EscalationThreadList { get; set; }
        public ObservableCollectionView<EscalationAndStatusThread> EscalationThreadListPage { get; set; }
        public int pageSize;

        public Vendor_All_EscalationThread()
        {
            this.InitializeComponent();




            this.EndDatePicker.Date = DateTime.Today;
            int date = DateTime.Today.Day;
            this.StartDatePicker.Date = DateTime.Today.AddDays(-(date - 1));
            this.SizeChanged += Vendor_All_EscalationThread_SizeChanged;

            EscalatonStatusList = new ObservableCollection<EscalationStatus>();
            AllMyPlatform = new ObservableCollection<Product>();
            EscalationThreadList = new ObservableCollectionView<EscalationAndStatusThread>();
            EscalationThreadListPage = new ObservableCollectionView<EscalationAndStatusThread>();
            this.Loaded += Vendor_All_EscalationThread_Loaded;

            this.DataContext = this;
        }

        private async void Vendor_All_EscalationThread_Loaded(object sender, RoutedEventArgs e)
        {
            var FTEEscalationThreadViewModel = new FTEEscalationThreadViewModel();
            EscalatonStatusList = await FTEEscalationThreadViewModel.GetAllEScalationStatus();
            AllMyPlatform = await FTEEscalationThreadViewModel.GetAllPlaform();
            StatusComboBox.ItemsSource = EscalatonStatusList;
            StatusComboBox.SelectedIndex = 0;
            PlatformComboBox.ItemsSource = AllMyPlatform;
            PlatformComboBox.SelectedIndex = 0;
            PageComboBox.SelectedIndex = 0;
            QueryButton.IsEnabled = true;

        }

        private void Vendor_All_EscalationThread_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            SceenSizeViewModel.ScreenWidth = this.ActualWidth;
        }

        private async void QueryButton_Click(object sender, RoutedEventArgs e)
        {


            HttpClient HttpClient = new HttpClient();
            Product MyProduct = new Product();
            MyProduct = PlatformComboBox.SelectedItem as Product;
            string plaform = MyProduct.Platform;
            EscalationStatus MyEscalationStatus = new EscalationStatus();
            MyEscalationStatus = StatusComboBox.SelectedItem as EscalationStatus;
            string status = MyEscalationStatus.Status;
            DateTime startDate = DateTime.Parse(StartDatePicker.Date.ToString());
            string startDatestring = startDate.ToString("MM-dd-yyyy");
            DateTime endDate = DateTime.Parse(EndDatePicker.Date.ToString());
            string endDatestring = endDate.ToString("MM-dd-yyyy");


            var HttpResponseMessage = await HttpClient.GetAsync(new Uri(string.Format("http://escalationmanagerwebapi.azurewebsites.net/api/ethreads?etime1={0}&etime2={1}&alias={2}&platform={3}&forum={4}&status={5}", startDatestring, endDatestring, "fapeng", plaform, "", status)));
            ObservableCollection<EscalationThread> AllMyEscalationThread = new ObservableCollection<EscalationThread>();
            if (HttpResponseMessage.StatusCode == HttpStatusCode.Ok)
            {
                EscalationThreadList.Items.Clear();
                var result = await HttpResponseMessage.Content.ReadAsStringAsync();
                AllMyEscalationThread = JsonConvert.DeserializeObject<ObservableCollection<EscalationThread>>(result);
                foreach (var escalationthread in AllMyEscalationThread)
                {
                    EscalationAndStatusThread EscalationAndStatusThread = new EscalationAndStatusThread();
                    EscalationAndStatusThread.EscalationThread = escalationthread;
                    EscalationAndStatusThread.EscalationStatusList = EscalatonStatusList;
                    EscalationThreadList.Items.Add(EscalationAndStatusThread);
                }

            }
            ComboBoxItem curItem = (ComboBoxItem)PageComboBox.SelectedItem;
            pageSize = Convert.ToInt32(curItem.Content.ToString());
            if (EscalationThreadList.Count == 0)
            {
                AllRecords.Text = "0";
                AllPageIndex.Text = "0";
                PageTxt.Text = "0";

            }

            else
            {
                AllRecords.Text = EscalationThreadList.Count.ToString();
                AllPageIndex.Text = GetPageIndex().ToString();
                int AllPagesIndex = GetPageIndex();
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
        }

        public void setScrollViewheight(List<EscalationAndStatusThread> MyList)
        {
            if (MyList.Count >= 10)
            {
                MyScrollView.Height = 650;
            }
            else
            {
                MyScrollView.Height = (MyList.Count + 1) * 60;
            }
        }

        private async void DataGridComboBoxStatus_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox DataGridComboBoxStatus = new ComboBox();
            DataGridComboBoxStatus = sender as ComboBox;

            EscalationAndStatusThread EscalationAndStatusThread = (EscalationAndStatusThread)(sender as ComboBox).DataContext;
            string ThreadId = EscalationAndStatusThread.EscalationThread.ThreadID;
            string ThreadUrl = EscalationAndStatusThread.EscalationThread.Url;
            DateTime CreatedDateTime = DateTime.Now;
            string CreatedBy = EscalationAndStatusThread.EscalationThread.FteAlias;
            EscalationStatus MyEscalationStatus = new EscalationStatus();
            MyEscalationStatus = DataGridComboBoxStatus.SelectedItem as EscalationStatus;
            string Status = MyEscalationStatus.Status;
            string SrescalationId = "N/A";
            MessageDialog dialog = new MessageDialog("Modify the Status Successfully");
            await dialog.ShowAsync();
        }

        public int GetPageIndex()
        {
            int AllPageIndex;
            if (EscalationThreadList.Count > pageSize)
            {

                if (EscalationThreadList.Count / pageSize == 0)
                {
                    AllPageIndex = EscalationThreadList.Count / pageSize;
                }
                else
                {
                    AllPageIndex = (EscalationThreadList.Count / pageSize) + 1;
                }
            }

            else
            {
                AllPageIndex = 1;
            }

            return AllPageIndex;
        }
        private void NextImage_Tapped(object sender, TappedRoutedEventArgs e)
        {

            int AllPageIndex = GetPageIndex();
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

        private void PreviousImage_Tapped(object sender, TappedRoutedEventArgs e)
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

        private void LastImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            int AllPageIndex = GetPageIndex();
            PageTxt.Text = AllPageIndex.ToString();
            var EscalationThreadListPage = EscalationThreadList.Skip((AllPageIndex - 1) * pageSize).Take(pageSize).ToList();
            DataGrid.ItemsSource = EscalationThreadListPage;
            setScrollViewheight(EscalationThreadListPage);
            PageTxt.Text = AllPageIndex.ToString();

        }

        private void FirstImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            var EscalationThreadListPage1 = EscalationThreadList.Skip(0 * pageSize).Take(pageSize).ToList();
            DataGrid.ItemsSource = EscalationThreadListPage1;
            setScrollViewheight(EscalationThreadListPage1);
            PageTxt.Text = "1";
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


