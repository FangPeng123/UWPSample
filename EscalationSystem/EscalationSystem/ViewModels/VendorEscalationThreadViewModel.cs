using EscalationSystem.Models;
using GalaSoft.MvvmLight;
using MyToolkit.Collections;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace EscalationSystem.ViewModels
{
   public class VendorEscalationThreadViewModel : ViewModelBase
    {
        private bool _isLoading;
        public bool IsLoading
        {
            get
            {
                return _isLoading;
            }
            set
            {
                _isLoading = value;
                RaisePropertyChanged("IsLoading");
            }
        }

        private EscalationStatusWithSelectedItem AllEscalationStatus_List;
        public EscalationStatusWithSelectedItem AllEscalationStatusList
        {
            get
            {
                if (AllEscalationStatus_List == null)
                {
                    AllEscalationStatus_List = new EscalationStatusWithSelectedItem();
                    return AllEscalationStatus_List;
                }

                return AllEscalationStatus_List;
            }
            set
            {
                AllEscalationStatus_List = value;
            }
        }

        private ProductWithSelectedItem AllPratfrom_List;
        public ProductWithSelectedItem AllPratfromList
        {
            get
            {
                if (AllPratfrom_List == null)
                {
                    AllPratfrom_List = new ProductWithSelectedItem();
                    return AllPratfrom_List;
                }

                return AllPratfrom_List;
            }
            set
            {
                AllPratfrom_List = value;
            }
        }

        public VendorEscalationThreadViewModel()
        {
            IsLoading = true;
        }

        public static async Task<VendorEscalationThreadViewModel> GetVendorEscalationThreadViewModel()
        {
            var VendorEscalationThreadViewModel = new VendorEscalationThreadViewModel();

            VendorEscalationThreadViewModel.AllEscalationStatusList = new EscalationStatusWithSelectedItem();
            VendorEscalationThreadViewModel.AllEscalationStatusList.MyEscalationStatusList = new ObservableCollection<EscalationStatus>();
            VendorEscalationThreadViewModel.AllEscalationStatusList.MyEscalationStatusList = await VendorEscalationThreadViewModel.GetAllEScalationStatus();
            VendorEscalationThreadViewModel.AllEscalationStatusList.SelectedItem = new EscalationStatus();
            VendorEscalationThreadViewModel.AllEscalationStatusList.SelectedItem = VendorEscalationThreadViewModel.AllEscalationStatusList.MyEscalationStatusList[0];

            VendorEscalationThreadViewModel.AllPratfromList = new ProductWithSelectedItem();
            VendorEscalationThreadViewModel.AllPratfromList.MyProductList = new ObservableCollection<Product>();
            VendorEscalationThreadViewModel.AllPratfromList.MyProductList = await VendorEscalationThreadViewModel.GetAllPlaform();
            VendorEscalationThreadViewModel.AllPratfromList.SelectedItem = new Product();
            VendorEscalationThreadViewModel.AllPratfromList.SelectedItem = VendorEscalationThreadViewModel.AllPratfromList.MyProductList[0];
            return VendorEscalationThreadViewModel;
        }

        public async Task<ObservableCollection<EscalationStatus>> GetAllEScalationStatus()
        {
            HttpClient HttpClient = new HttpClient();
            var HttpResponseMessage = await HttpClient.GetAsync(new Uri("http://escalationmanagerwebapi.azurewebsites.net/api/statuses"));
            ObservableCollection<EscalationStatus> MyEscalationStatus = new ObservableCollection<EscalationStatus>();
            if (HttpResponseMessage.StatusCode == HttpStatusCode.Ok)
            {
                var result = await HttpResponseMessage.Content.ReadAsStringAsync();
                MyEscalationStatus = JsonConvert.DeserializeObject<ObservableCollection<EscalationStatus>>(result);
                MyEscalationStatus.Insert(0, new EscalationStatus() { StatusId = 10, Status = "All", StatusType = "All" });
                MyEscalationStatus.Insert(1, new EscalationStatus() { StatusId = 11, Status = "Open:All", StatusType = "Open:All" });
                MyEscalationStatus.Insert(2, new EscalationStatus() { StatusId = 12, Status = "Closed:All", StatusType = "Closed:All" });

            }

            return MyEscalationStatus;
        }


        public async Task<ObservableCollection<Product>> GetAllPlaform()
        {
            HttpClient HttpClient = new HttpClient();
            var HttpResponseMessage = await HttpClient.GetAsync(new Uri("http://escalationmanagerwebapi.azurewebsites.net/api/products?platform=All"));
            ObservableCollection<Product> AllMyPlatform = new ObservableCollection<Product>();
            if (HttpResponseMessage.StatusCode == HttpStatusCode.Ok)
            {
                var result = await HttpResponseMessage.Content.ReadAsStringAsync();
                AllMyPlatform = JsonConvert.DeserializeObject<ObservableCollection<Product>>(result);
                AllMyPlatform.Insert(0, new Product() { Platform = "All", Forum = "All", Description = "All", Owner = "All", Operator = "All" });

            }

            return AllMyPlatform;
        }

     
        public int GetPageIndex(ObservableCollectionView<EscalationAndStatusThread> EscalationThreadList, int pageSize)
        {
            int AllPageIndex;
            if (EscalationThreadList.Count > pageSize)
            {

                if (EscalationThreadList.Count % pageSize == 0)
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
        public async Task<ObservableCollectionView<EscalationAndStatusThread>> QueryAllEscalationAndStatusThread(ProductWithSelectedItem AllMyPlatform, EscalationStatusWithSelectedItem EscalatonStatusList, string startDatestring, string endDatestring)
        {
            Windows.Storage.ApplicationDataContainer LocalSettings =Windows.Storage.ApplicationData.Current.LocalSettings;
            string userAlias = LocalSettings.Values["currentUserAlias"].ToString();
            ObservableCollectionView<EscalationAndStatusThread> EscalationThreadList = new ObservableCollectionView<EscalationAndStatusThread>();
            HttpClient HttpClient = new HttpClient();
            Product MyProduct = new Product();
            MyProduct = AllMyPlatform.SelectedItem;
            string plaform = MyProduct.Platform;
            EscalationStatus MyEscalationStatus = new EscalationStatus();
            MyEscalationStatus = EscalatonStatusList.SelectedItem;
            string status = MyEscalationStatus.Status;
            
            var HttpResponseMessage = await HttpClient.GetAsync(new Uri(string.Format("http://escalationmanagerwebapi.azurewebsites.net/api/ethreads?etime1={0}&etime2={1}&alias={2}&platform={3}&forum={4}&status={5}", startDatestring, endDatestring,userAlias, plaform, "", status)));
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
                    EscalationAndStatusThread.EscalationStatusList = EscalatonStatusList.MyEscalationStatusList;
                    EscalationThreadList.Items.Add(EscalationAndStatusThread);

                }
            }

            return EscalationThreadList;
        }
    }
}
