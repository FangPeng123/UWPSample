using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Email;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using LightBuzz.SMTP;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Headers;
using Newtonsoft.Json;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Windows.Networking.PushNotifications;
using Microsoft.WindowsAzure.Messaging;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Runtime.Serialization;
using System.Collections.ObjectModel;
using EscalationSystem.Models;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace EscalationSystem.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Vendor_Create_EscalationThread : Page
    {
        public Vendor_Create_EscalationThread()
        {
            this.InitializeComponent();
            Windows.Storage.ApplicationDataContainer localSettings =
    Windows.Storage.ApplicationData.Current.LocalSettings;
            var currentUser = localSettings.Values["currentUser"];
            var currentUserAlias = localSettings.Values["currentUserAlias"];
            ThreadOnwerTxt.Text = currentUserAlias.ToString();
        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            bindcomboxProduces();
        }


        private async void bindcomboxProduces()
        {
            try
            {
                HttpClient HttpClient = new HttpClient();
                var HttpResponseMessage = await HttpClient.GetAsync(new Uri("http://escalationmanagerwebapi.azurewebsites.net/api/products?platform=All"));
                ObservableCollection<Product> AllMyPlatform = new ObservableCollection<Product>();
                if (HttpResponseMessage.StatusCode == HttpStatusCode.Ok)
                {
                    var result = await HttpResponseMessage.Content.ReadAsStringAsync();
                    AllMyPlatform = JsonConvert.DeserializeObject<ObservableCollection<Product>>(result);
                    if (AllMyPlatform.Count > 0)
                    {
                        complatform.DataContext = AllMyPlatform;
                       
                    }
                }
            }
            catch
            {
            }
        }


        private void complatform_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            comForum.IsEnabled = true;
            try
            {
    
                string Plaform = ((sender as ComboBox).SelectedItem as Product).Platform as string;
                if (Plaform != null)
                {
                    bindcomboxForums(Plaform);
                    bindcomboxFtes();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private async void bindcomboxForums(string Plaform)
        {
            try
            {
                HttpClient HttpClient = new HttpClient();
                var HttpResponseMessage = await HttpClient.GetAsync(new Uri($"http://escalationmanagerwebapi.azurewebsites.net/api/products?platform={Plaform}"));
                ObservableCollection<Product> AllMyPlatformforums = new ObservableCollection<Product>();
                if (HttpResponseMessage.StatusCode == HttpStatusCode.Ok)
                {
                    var result = await HttpResponseMessage.Content.ReadAsStringAsync();
                    AllMyPlatformforums = JsonConvert.DeserializeObject<ObservableCollection<Product>>(result);
                    comForum.DataContext = AllMyPlatformforums;
                   
                }
            }
            catch
            {

            }
        }

        private void comForum_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            comFTES.IsEnabled = true;
            try
            {
                ObservableCollection<Product> datasources = (ObservableCollection<Product>)(sender as ComboBox).DataContext;
                string forum = ((sender as ComboBox).SelectedItem as Product).Forum as string;
                if (forum != null)
                {
                    List<Ftes> ftes = new List<Ftes>();
                    foreach (Product va in datasources)
                    {
                        Ftes fts = new Ftes();
                        fts.Alias = va.Owner;
                        ftes.Add(fts);
                    }
                    //comFTES.DataContext = ftes;
                    //comFTES.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private async void bindcomboxFtes()
        {
            try
            {
                HttpClient HttpClient = new HttpClient();
                var HttpResponseMessage = await HttpClient.GetAsync(new Uri("http://escalationmanagerwebapi.azurewebsites.net/api/ftes"));
                ObservableCollection<Ftes> AllFtes = new ObservableCollection<Ftes>();
                if (HttpResponseMessage.StatusCode == HttpStatusCode.Ok)
                {
                    var result = await HttpResponseMessage.Content.ReadAsStringAsync();
                    AllFtes = JsonConvert.DeserializeObject<ObservableCollection<Ftes>>(result);
                    comFTES.DataContext = AllFtes;
                    comFTES.SelectedIndex = 0;
                    string product = (complatform.SelectedItem as Product).Platform as string;
                    List<Ftes> ftes = new List<Ftes>();
                    foreach (Ftes va in AllFtes)
                    {
                        if (va.Platform == product)
                        {
                            ftes.Add(va);
                        }
                    }
                    comFTES.DataContext = ftes;
                    comFTES.DisplayMemberPath = "DisplayName";
                    comFTES.SelectedValuePath = "Alias";
                    comFTES.SelectedIndex = 0;
                }
            }
            catch
            {
            }

        }



        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            string Platform = "";
            try
            {
                Platform = (complatform.SelectedItem as Product).Platform as string;
            }
            catch
            {
                Platform = "";
            }

            string Forum = "";
            try
            {
                Forum = (comForum.SelectedItem as Product).Forum as string;
            }
            catch
            {
                Forum = "";
            }

            string fte = "";
            try
            {
                fte = (comFTES.SelectedItem as Ftes).Alias as string;
            }
            catch
            {
                fte = "";
            }

            string pathurl = "";
            txtlink.Document.GetText(Windows.UI.Text.TextGetOptions.AdjustCrlf, out pathurl);

            if (Platform != "" && Forum != "" && Platform != "" && fte != "" && pathurl != "")
            {
                EscalationThread mes = new EscalationThread();
                mes.ThreadId = txtThreadID.Text.ToString();
                mes.Url = pathurl;
                string title = "";
                txttitle.Document.GetText(Windows.UI.Text.TextGetOptions.AdjustCrlf, out title);
                mes.Title = title;
                mes.Forum = Forum;
                mes.Platform = Platform;
                mes.LastreplyFromOp = true;
                mes.EscalatedDatetime = DateTime.Now;
                mes.VendorAlias = ThreadOnwerTxt.Text;
                mes.FteAlias = fte;
                string reason = "";
                txtReason.Document.GetText(Windows.UI.Text.TextGetOptions.AdjustCrlf, out reason);
                mes.Reason = reason;
                string description = "";
                txtDescription.Document.GetText(Windows.UI.Text.TextGetOptions.AdjustCrlf, out description);
                mes.Description = description;
                mes.Labor = 0;
                mes.Status = "Open: New";
                mes.ThreadCreatedDatetime = DateTime.Now;
                mes.LastreplyDatetime = DateTime.Now;
                mes.LastreplyFromOp = true;
                mes.SrescalationId = "";
                mes.IsManaged = false;
                bool reaultaddesc = await AddEscalationAndStatusThread(mes);
                if (reaultaddesc)
                {
                    await new MessageDialog("Add Escalation Thread Successfully! ").ShowAsync();
                }
                else
                {
                    await new MessageDialog("Add Escalation Thread Failed! ").ShowAsync();
                }
                //bool resultemail = await SendEscalationEmail();
            }
            else
            {
                MessageDialog messageDialog = new MessageDialog("Please fill all the fields!!!");
                await messageDialog.ShowAsync();
            }
        }
        public async Task<bool> AddEscalationAndStatusThread(EscalationThread mes)
        {
            bool result = false;
            HttpClient HttpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(mes);
            var stringContent = new HttpStringContent(json,
                         Windows.Storage.Streams.UnicodeEncoding.Utf8,
                         "application/json");
            var HttpResponseMessage = await HttpClient.PostAsync(new Uri("http://escalationmanagerwebapi.azurewebsites.net/api/ethreads"), stringContent);
            if (HttpResponseMessage.StatusCode == HttpStatusCode.Ok)
            {
                var resultsss = await HttpResponseMessage.Content.ReadAsStringAsync();
                result = true;
                try
                {
                    // add message to queue
                    CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=escalationmanager;AccountKey=Q5DR3/tIxti8225Ts3SAIULWPde40g+OBfam7+Avg7sisebU8hld8UtBkyfeilU/kSb0kntB5R5IH77dM4igNQ==;");
                    CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
                    CloudQueue messageQueue = queueClient.GetQueueReference("toast");
                    CloudQueueMessage message = new CloudQueueMessage(JsonConvert.SerializeObject(mes));
                    await messageQueue.AddMessageAsync(message);
                }
                catch(Exception e)
                {
                    MessageDialog messageDialog = new MessageDialog(e.Message);
                    await messageDialog.ShowAsync();
                }
            }
            return await Task.FromResult(result);
        }


        NotificationHub hub;
        PushNotificationChannel channel;
        string tokenaccess = "";
        /// <summary>
        /// 首先创建
        /// </summary>
        private async void createchanneltoAuzreHub()
        {
            if (hub == null || channel == null)
            {
                channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
                hub = new NotificationHub("tomnotification", "Endpoint=sb://tomtestnotification.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=MZDkZ2rJLwCTbspWe/Cb7IZ9ItCygwmVbj3NJEAZpDs=");
                var result = await hub.RegisterNativeAsync(channel.Uri);
                if (result.RegistrationId != null)
                {
                    var dialog = new MessageDialog("Registration successful: " + result.RegistrationId);
                    dialog.Commands.Add(new UICommand("OK"));
                    await dialog.ShowAsync();
                }
            }
            else
            {
                await SetToastNotification("This is my toast message for UWP");
            }
        }

        private OAuthToken GetOAuthTokenFromJson(string jsonString)
        {
            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(jsonString)))
            {
                var ser = new DataContractJsonSerializer(typeof(OAuthToken));
                var oAuthToken = (OAuthToken)ser.ReadObject(ms);
                return oAuthToken;
            }
        }

        private async Task<string> SetToastNotification(string message)
        {
            try
            {
                try
                {
                    string secret = "J82cI9OC/my9+a1vw2MmGrAj87YB6nwK";
                    string sid = "ms-app://s-1-15-2-4128186160-1681326203-2788279282-2432225568-1896386270-4060788042-1141638401";
                    if (tokenaccess == "")
                    {
                        OAuthToken oAuthToken = new OAuthToken();
                        Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();
                        var urlEncodedSecret = System.Net.WebUtility.UrlEncode(secret);
                        var urlEncodedSid = System.Net.WebUtility.UrlEncode(sid);
                        var body =
                       String.Format("grant_type=client_credentials&client_id={0}&client_secret={1}&scope=notify.windows.com", urlEncodedSid, urlEncodedSecret);
                        Uri requestUri = new Uri("https://login.live.com/accesstoken.srf");
                        HttpRequestMessage msg = new HttpRequestMessage(new HttpMethod("POST"), requestUri);
                        msg.Content = new HttpStringContent(body);
                        msg.Content.Headers.ContentType = new HttpMediaTypeHeaderValue("application/x-www-form-urlencoded");
                        HttpResponseMessage HttpResponseMessage = await httpClient.SendRequestAsync(msg).AsTask();
                        if (HttpResponseMessage.StatusCode == Windows.Web.Http.HttpStatusCode.Ok)
                        {
                            var resultsss = await HttpResponseMessage.Content.ReadAsStringAsync();
                            oAuthToken = GetOAuthTokenFromJson(resultsss);
                            tokenaccess = oAuthToken.AccessToken;
                        }
                    }
                    await PostToWns(channel.Uri, tokenaccess, "Push Notification: " + message + ", " + DateTime.Now.ToString(), "wns/toast", "text/xml");
                    return await Task.FromResult("true");
                }
                catch
                {
                    return await Task.FromResult("false");
                }
            }
            catch (Exception ex)
            {
                return await Task.FromResult("false");
            }
        }

        private async Task<int?> PostToWns(string uri, string AccessToken, string xml, string notificationType, string contentType)
        {
            try
            {
                Windows.Web.Http.HttpClient httpClient = new Windows.Web.Http.HttpClient();
                Uri requestUri = new Uri(uri);
                HttpRequestMessage msg = new HttpRequestMessage(new HttpMethod("POST"), requestUri);
                msg.Content = new HttpStringContent(CreateXMLtemplet(xml));
                msg.Headers.Add("X-WNS-Type", notificationType);
                msg.Headers.Add("Authorization", String.Format("Bearer {0}", AccessToken));
                msg.Content.Headers.ContentType = new HttpMediaTypeHeaderValue("text/xml");
                HttpResponseMessage HttpResponseMessage = await httpClient.SendRequestAsync(msg).AsTask();
                if (HttpResponseMessage.StatusCode == Windows.Web.Http.HttpStatusCode.Ok)
                {
                    var resultsss = await HttpResponseMessage.Content.ReadAsStringAsync();
                    return await Task.FromResult(1);
                }
                else
                {
                    return await Task.FromResult(0);
                }
            }
            catch (Exception ex)
            {
                return await Task.FromResult(0);
            }
        }

        private string CreateXMLtemplet(string xml)
        {
            ToastTemplateType toastTemplate = ToastTemplateType.ToastImageAndText01;
            XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);
            XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");
            toastTextElements[0].AppendChild(toastXml.CreateTextNode(xml));
            XmlNodeList toastImageAttributes = toastXml.GetElementsByTagName("image");
            ((XmlElement)toastImageAttributes[0]).SetAttribute("src", "ms-appx:///assets/Edit.png");
            ((XmlElement)toastImageAttributes[0]).SetAttribute("alt", "Logo");

            IXmlNode toastNode = toastXml.SelectSingleNode("/toast");
            ((XmlElement)toastNode).SetAttribute("duration", "short");
            ((XmlElement)toastNode).SetAttribute("launch", "{\"type\":\"toast\", \"param1\": \"3697\"}");
            return toastXml.GetXml();
        }


    }

    [DataContract]
    public sealed class OAuthToken
    {
        [DataMember(Name = "access_token")]
        public string AccessToken { get; set; }
        [DataMember(Name = "token_type")]
        public string TokenType { get; set; }
    }

    //public class SendMes
    //{
    //    public string ThreadId { get; set; }
    //    public string Url { get; set; }
    //    public string Title { get; set; }
    //    public string Forum { get; set; }
    //    public string Platform { get; set; }
    //    public string ThreadCreatedDatetime { get; set; }
    //    public string LastreplyDatetime { get; set; }
    //    public bool LastreplyFromOp { get; set; }
    //    public string EscalatedDatetime { get; set; }
    //    public string VendorAlias { get; set; }
    //    public string FteAlias { get; set; }
    //    public string Reason { get; set; }
    //    public string Description { get; set; }
    //    public int Labor { get; set; }
    //    public string SrescalationId { get; set; }
    //    public string Status { get; set; }
    //    public bool IsManaged { get; set; }
    //}

}