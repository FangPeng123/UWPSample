using EscalationSystem.Models;
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
   public class FTEEscalationReportViewModel
    {
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

        public static async Task<FTEEscalationReportViewModel> GetFTEEscalationReportViewModel()
        {
            var FTEEscalationReportViewModel = new FTEEscalationReportViewModel();
            FTEEscalationReportViewModel.AllPratfromList = new ProductWithSelectedItem();
            FTEEscalationReportViewModel.AllPratfromList.MyProductList = new ObservableCollection<Product>();
            FTEEscalationReportViewModel.AllPratfromList.MyProductList = await FTEEscalationReportViewModel.GetAllPlaform();
            FTEEscalationReportViewModel.AllPratfromList.SelectedItem = new Product();
            FTEEscalationReportViewModel.AllPratfromList.SelectedItem = FTEEscalationReportViewModel.AllPratfromList.MyProductList[0];
            return FTEEscalationReportViewModel;
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
            Forumlist.Insert(0, "All");
            return Forumlist;
        }

        public async Task<ObservableCollectionView<Report>> QueryAllEscalationReport(ProductWithSelectedItem AllMyPlatform,string platfrom,string forum,string startDatestring, string endDatestring)
        {
            ObservableCollectionView<Report> ReportList = new ObservableCollectionView<Report>();
            HttpClient HttpClient = new HttpClient();
            Product MyProduct = new Product();
            MyProduct = AllMyPlatform.SelectedItem;
           
            var HttpResponseMessage = await HttpClient.GetAsync(new Uri(string.Format("http://escalationmanagerwebapi.azurewebsites.net/api/reports?Alias={0}&Platform={1}&Forum={2}&ETime1={3}&ETime2={4}&VFlag={5}","All", platfrom,forum,startDatestring, endDatestring, false)));
            ObservableCollection<Report> AllMyReport = new ObservableCollection<Report>();
            if (HttpResponseMessage.StatusCode == HttpStatusCode.Ok)
            {
                ReportList.Items.Clear();
                var result = await HttpResponseMessage.Content.ReadAsStringAsync();
                AllMyReport = JsonConvert.DeserializeObject<ObservableCollection<Report>>(result);
                foreach (var report in AllMyReport )
                {
                    ReportList.Items.Add(report);
                }
            }

            return ReportList;
        }
    }
}
