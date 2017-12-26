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
using Windows.UI.Popups;
using Windows.Web.Http;

namespace EscalationSystem.ViewModels
{
    public class FTEConsultThreadViewModel : ViewModelBase
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

        

        public FTEConsultThreadViewModel()
        {
            IsLoading = true;
        }

        public static async Task<FTEConsultThreadViewModel> GetFTEConsultThreadViewModel()
        {
            var FTEConsultThreadViewModel = new FTEConsultThreadViewModel();
            FTEConsultThreadViewModel.AllEscalationStatusList.MyEscalationStatusList = new ObservableCollection<EscalationStatus>();
           

            FTEConsultThreadViewModel.AllPratfromList = new ProductWithSelectedItem();
            FTEConsultThreadViewModel.AllPratfromList.MyProductList = new ObservableCollection<Product>();
            FTEConsultThreadViewModel.AllPratfromList.MyProductList = await FTEConsultThreadViewModel.GetAllPlaform();
            FTEConsultThreadViewModel.AllPratfromList.SelectedItem = new Product();
            FTEConsultThreadViewModel.AllPratfromList.SelectedItem = FTEConsultThreadViewModel.AllPratfromList.MyProductList[0];
            return FTEConsultThreadViewModel;
        }

        public async Task<ObservableCollection<string>> GetAllForum(string PlatForm)
        {
            ObservableCollection<string> Forumlist = new ObservableCollection<string>();
            HttpClient HttpClient = new HttpClient();
            var HttpResponseMessage = await HttpClient.GetAsync(new Uri("http://escalationmanagerwebapi.azurewebsites.net/api/products"));
            ObservableCollection<Product> AllMyPlatform = new ObservableCollection<Product>();
            if (HttpResponseMessage.StatusCode == HttpStatusCode.Ok)
            {
                var result = await HttpResponseMessage.Content.ReadAsStringAsync();
                AllMyPlatform = JsonConvert.DeserializeObject<ObservableCollection<Product>>(result);
                foreach (var prouct in AllMyPlatform)
                {
                    if (prouct.Platform.Equals(PlatForm))
                    {
                        Forumlist.Add(prouct.Forum);
                    }
                }
            }
            return Forumlist;
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

 
        public int GetPageIndex(ObservableCollectionView<ConsultThread> ConsultThreadList, int pageSize)
        {
            int AllPageIndex;
            if (ConsultThreadList.Count > pageSize)
            {

                if (ConsultThreadList.Count % pageSize == 0)
                {
                    AllPageIndex = ConsultThreadList.Count / pageSize;
                }
                else
                {
                    AllPageIndex = (ConsultThreadList.Count / pageSize) + 1;
                }
            }

            else
            {
                AllPageIndex = 1;
            }

            return AllPageIndex;
        }

        public async void AddConsultThread(ConsultThread consultThread)
        {
            HttpClient HttpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(consultThread);
            var stringContent = new HttpStringContent(json,
                         Windows.Storage.Streams.UnicodeEncoding.Utf8,
                         "application/json");
            var HttpResponseMessage = await HttpClient.PostAsync(new Uri("http://escalationmanagerwebapi.azurewebsites.net/api/cthreads"), stringContent);
            if (HttpResponseMessage.StatusCode == HttpStatusCode.Ok)
            {
                MessageDialog messageDialog = new MessageDialog("Add the consult thread Sucessfully!!");
                await messageDialog.ShowAsync();
            }
            else
            {
                MessageDialog messageDialog = new MessageDialog("Add the consult thread Failed!!");
                await messageDialog.ShowAsync();
            }

        }
        public async Task<List<string>>GetFTEList()
        {
            HttpClient HttpClient = new HttpClient();
            var HttpResponseMessage = await HttpClient.GetAsync(new Uri(string.Format("http://escalationmanagerwebapi.azurewebsites.net/api/ftes")));
            List<string> AllFTEList = new List<string>();
            if (HttpResponseMessage.StatusCode == HttpStatusCode.Ok)
            {
                var result = await HttpResponseMessage.Content.ReadAsStringAsync();
                List<Ftes>FTEList= JsonConvert.DeserializeObject<List<Ftes>>(result);
                foreach(var fte in FTEList)
                {
                    AllFTEList.Add(fte.DisplayName);
                }
                
            }
            AllFTEList.Insert(0,"All");
            var List=AllFTEList.Distinct().ToList();
            return List;
        }
        public async Task<ObservableCollectionView<ConsultThread>> QueryAllConsultThread(string alias,string platfrom, string startDatestring, string endDatestring)
        {
            string FTEAlias = "";
            ObservableCollectionView<ConsultThread> ConsultThreadList = new ObservableCollectionView<ConsultThread>();
            HttpClient HttpClient = new HttpClient();
            var FTEHttpResponseMessage = await HttpClient.GetAsync(new Uri(string.Format("http://escalationmanagerwebapi.azurewebsites.net/api/ftes")));
            List<string> AllFTEList = new List<string>();
            if (FTEHttpResponseMessage.StatusCode == HttpStatusCode.Ok)
            {
                var result = await FTEHttpResponseMessage.Content.ReadAsStringAsync();
                List<Ftes> FTEList = JsonConvert.DeserializeObject<List<Ftes>>(result);
                foreach (var fte in FTEList)
                {
                    if (fte.DisplayName.Equals(alias))
                    {
                        FTEAlias = fte.Alias;
                        break;
                    }
                    else
                        FTEAlias = "All";
                }

            }
           
            var HttpResponseMessage = await HttpClient.GetAsync(new Uri(string.Format("http://escalationmanagerwebapi.azurewebsites.net/api/cthreads?VendorAlias={0}&FteAlias={1}&Platform={2}&Forum={3}&Status={4}&CTime1={5}&CTime2={6}&ETime1={7}&ETime2={8}&RTime1={9}&RTime2={10}","", FTEAlias,platfrom,"","",startDatestring, endDatestring, "" ,"", "", "","")));
            ObservableCollection<ConsultThread> AllConsultThread = new ObservableCollection<ConsultThread>();
            if (HttpResponseMessage.StatusCode == HttpStatusCode.Ok)
            {
                ConsultThreadList.Items.Clear();
                var result = await HttpResponseMessage.Content.ReadAsStringAsync();
                AllConsultThread = JsonConvert.DeserializeObject<ObservableCollection<ConsultThread>>(result);
                foreach(var consultThread in AllConsultThread)
                {
                    ConsultThreadList.Items.Add(consultThread);
                }
            }

            return ConsultThreadList;
        }
    }
}
