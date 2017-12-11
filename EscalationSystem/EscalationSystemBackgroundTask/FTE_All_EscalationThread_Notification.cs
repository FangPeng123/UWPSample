
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Queue;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.System;
using Windows.UI.Popups;
using Windows.Web.Http;
using System.Runtime.Serialization.Json;
using Windows.Data.Xml.Dom;
using Windows.Networking.PushNotifications;
using Windows.UI.Notifications;
using Windows.Web.Http.Headers;
using System.Net;

namespace EscalationSystemBackgroundTask
{
    /// <summary>
    /// 检测FET对应Escalation的list-针对当天的数量变动做检测
    /// 15mins检测一次
    /// </summary>
    public sealed class FTE_All_EscalationThread_Notification : IBackgroundTask
    {
        public async void Run(IBackgroundTaskInstance taskInstance)
        {
            BackgroundTaskDeferral deferral = taskInstance.GetDeferral();
            Task<bool> t = CheckUserExist();
            bool pass = await t;
            if (pass)
            {
                try
                {
                    if (currentChannelUrl == "")
                    {
                        await CreatePushNotificationChannel();
                    }
                    if (currentOAuthToken == "")
                    {
                        await GetAccessToken();
                    }

                    //查找Azure Storage 的最新升级的case，然后做对比来发出通知
                    List<Messagesss> Notificationmess = new List<Messagesss>();
                    List<string> NotificationmessExits = new List<string>();
                    try
                    {
                        Notificationmess.Clear();
                        NotificationmessExits.Clear();

                        CloudStorageAccount storageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=escalationmanager;AccountKey=Q5DR3/tIxti8225Ts3SAIULWPde40g+OBfam7+Avg7sisebU8hld8UtBkyfeilU/kSb0kntB5R5IH77dM4igNQ==;");
                        CloudQueueClient queueClient = storageAccount.CreateCloudQueueClient();
                        CloudQueue messageQueue = queueClient.GetQueueReference("toast");
                        //  messageQueue.GetMessageAsync
                        await messageQueue.FetchAttributesAsync();
                        // Retrieve the cached approximate message count.
                        int? cachedMessageCount = messageQueue.ApproximateMessageCount;

                        //
                        //Messagesss mesa = new Messagesss() { Url = "https://social.msdn.microsoft.com/Forums/vstudio/en-US/4ca2abe9-dc11-4016-95a7-9a7d5319ef98/scrollviewer-panningmode?forum=wpf", Forum = "VCS", MessageId = "112", FteAlias= "fapeng" };
                        //CloudQueueMessage message = new CloudQueueMessage(JsonConvert.SerializeObject(mesa));
                        //await messageQueue.AddMessageAsync(message);

                        if (cachedMessageCount != null)
                        {
                            for (int i = 0; i < cachedMessageCount; i++)
                            {
                                try
                                {
                                    CloudQueueMessage retrievedMessage = await messageQueue.GetMessageAsync();
                                    // Process the message in less than 30 seconds.
                                    String AA = retrievedMessage.AsString;
                                    Messagesss mes = JsonConvert.DeserializeObject<Messagesss>(AA);
                                    if (mes.FteAlias == "fapeng")
                                    {
                                        Notificationmess.Add(mes);
                                        // Then delete the message.
                                        await messageQueue.DeleteMessageAsync(retrievedMessage);
                                    }
                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                    }

                    //通知
                    if (Notificationmess.Count > 0)
                    {
                        //读取本地文件中当天查询的case记录-排除重复的提醒
                        try
                        {
                            IStorageFolder applicationFolder = ApplicationData.Current.LocalFolder;
                            string filename = "NotificationRecords_" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
                            IStorageFile storageFile = await applicationFolder.CreateFileAsync(filename, CreationCollisionOption.OpenIfExists);
                            IRandomAccessStream accessStream = await storageFile.OpenReadAsync();
                            string currentdayrecord = "";
                            using (StreamReader streamReader = new StreamReader(accessStream.AsStreamForRead((int)accessStream.Size)))
                            {
                                Task<string> readtxt = streamReader.ReadToEndAsync();
                                currentdayrecord = await readtxt;
                            }
                            if (currentdayrecord != "")
                            {

                                var currentusersplit = currentdayrecord.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
                                for (int i = 0; i < currentusersplit.Length; i++)
                                {
                                    if (currentusersplit[i] != "")
                                    {
                                        NotificationmessExits.Add(currentusersplit[i]);
                                    }
                                }
                            }
                        }
                        catch
                        {
                        }
                        List<Messagesss> notNotificationmess = new List<Messagesss>();
                        foreach (var vr in Notificationmess)
                        {
                            if (NotificationmessExits.Contains(vr.Url))
                            {
                                notNotificationmess.Add(vr);
                            }
                        }
                        foreach (var vr in notNotificationmess)
                        {
                            Notificationmess.Remove(vr);
                        }
                        if (Notificationmess.Count != 0)
                        {

                            // send WNS push Notification
                            await PostToWns(secret, sid, currentChannelUrl, currentOAuthToken, $"Push Notification: FTE_EscalationThread has {(Notificationmess.Count)} new records Added! Click to View.", "wns/toast", "text/xml");

                            //// send local toast Notification
                            //SHToastNotification.ShowToastNotification("Square150x150Logo.scale-200.png", $"FTE_EscalationThread has {(Notificationmess.Count)} new records Added! Click to View.", NotificationAudioNames.Default, JsonConvert.SerializeObject( Notificationmess));
                        }
                    }
                }
                catch
                {

                }
            }
            deferral.Complete();
        }

        /// <summary>
        /// 检测user合法性
        /// </summary>
        /// <returns></returns>
        private async Task<bool> CheckUserExist()
        {
            bool exists = false;
            try
            {
                Windows.Storage.ApplicationDataContainer localSettings =
Windows.Storage.ApplicationData.Current.LocalSettings;
                Object existuser = localSettings.Values["currentUser"];

                if (existuser != null && existuser.ToString() != "")
                {

                    var users = await User.FindAllAsync();
                    var currentDomainUser = await users.FirstOrDefault().GetPropertyAsync(KnownUserProperties.DomainName);
                    var currentUser = await users.FirstOrDefault().GetPropertyAsync(KnownUserProperties.DisplayName);

                    if (existuser.ToString() == currentUser.ToString())
                    {
                        exists = true;
                    }
                }
            }
            catch
            {
                exists = false;
            }
            return await Task.FromResult(exists); ;
        }

        ///// <summary>
        ///// 获取所有的升级列表
        ///// </summary>
        ///// <returns></returns>
        //private async Task<int?> GetCurrentdayFTE_ALL_EscalationThread()
        //{
        //    int? result = null;
        //    try
        //    {
        //        //DateTime startDate = DateTime.Now;
        //        //string startDatestring = startDate.ToString("MM-dd-yyyy");
        //        //DateTime endDate = DateTime.Now;
        //        //string endDatestring = endDate.ToString("MM-dd-yyyy");

        //        DateTime startDate = DateTime.Parse("11-03-2017");
        //        string startDatestring = startDate.ToString("MM-dd-yyyy");
        //        DateTime endDate = DateTime.Parse("11-23-2017");
        //        string endDatestring = endDate.ToString("MM-dd-yyyy");

        //        HttpClient HttpClient = new HttpClient();
        //        var HttpResponseMessage = await HttpClient.GetAsync(new Uri(string.Format("http://escalationmanagerwebapi.azurewebsites.net/api/ethreads?etime1={0}&etime2={1}&alias={2}&platform={3}&forum={4}&status={5}", startDatestring, endDatestring, "fapeng", "All", "", "All")));
        //        List<EscalationThread> AllMyEscalationThread = new List<EscalationThread>();
        //        if (HttpResponseMessage.StatusCode == HttpStatusCode.Ok)
        //        {
        //            var results = await HttpResponseMessage.Content.ReadAsStringAsync();
        //            AllMyEscalationThread = JsonConvert.DeserializeObject<List<EscalationThread>>(results);
        //        }
        //        result = AllMyEscalationThread.Count;
        //    }
        //    catch
        //    {
        //        result = null;
        //    }
        //    return await Task.FromResult(result);
        //}

        private IAsyncOperation<List<EscalationStatus>> asyncOperation;

        private IAsyncOperation<List<EscalationStatus>> GetAsyncOperation(int x, int y)
        {
            return AsyncInfo.Run<List<EscalationStatus>>((token) => Task.Run<List<EscalationStatus>>(() => {
                token.WaitHandle.WaitOne(1000);
                token.ThrowIfCancellationRequested();
                return new List<EscalationStatus>() { new EscalationStatus() { StatusId = 1 }, new EscalationStatus() { StatusId = 2, Status = "122" } };
            }));
        }


        List<EscalationStatus> a = new List<EscalationStatus>();
        /// <summary>
        /// 异步操作，有返回值， 无进度值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void IAsyncOperationT_Click()
        {

            asyncOperation = GetAsyncOperation(1, 2);
            asyncOperation.Completed = (asyncinfo, asyncStatus) => {
                a = asyncinfo.GetResults();
            };
        }


        #region  WNS push notification

        string currentChannelUrl = "";
        string currentOAuthToken = "";

        string secret = "J82cI9OC/my9+a1vw2MmGrAj87YB6nwK";
        string sid = "ms-app://s-1-15-2-4128186160-1681326203-2788279282-2432225568-1896386270-4060788042-1141638401";
        private async Task<int?> CreatePushNotificationChannel()
        {
            int? result = 0;
            try
            {
                PushNotificationChannel channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
                String serverUrl = "http://www.contoso.com";
                HttpWebRequest webRequest = (HttpWebRequest)HttpWebRequest.Create(serverUrl);
                webRequest.Method = "POST";
                webRequest.ContentType = "application/x-www-form-urlencoded";
                byte[] channelUriInBytes = System.Text.Encoding.UTF8.GetBytes("ChannelUri=" + channel.Uri);
                Stream requestStream = await webRequest.GetRequestStreamAsync();
                requestStream.Write(channelUriInBytes, 0, channelUriInBytes.Length);
                try
                {
                    WebResponse response = await webRequest.GetResponseAsync();
                    StreamReader requestReader = new StreamReader(response.GetResponseStream());
                    String webResponse = requestReader.ReadToEnd();
                    currentChannelUrl = channel.Uri;
                    result = 1;
                }
                catch (Exception ex)
                {
                }
            }
            catch (Exception ex)
            {
            }
            return await Task.FromResult(result);
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

        private async Task<int?> GetAccessToken()
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
                currentOAuthToken = oAuthToken.AccessToken;
            }
            return await Task.FromResult(1);
        }



        /// <summary>
        ///  push nitification
        /// </summary>
        /// <param name="uri">channel.Uri</param>
        /// <param name="xml"></param>
        /// <param name="notificationType"> WNS-Type: wns/toast | wns/badge | wns/tile | wns/raw</param>
        /// <param name="contentType">text/xml</param>
        /// <returns></returns>
        private async Task<int?> PostToWns(string secret, string sid, string uri, string AccessToken, string xml, string notificationType, string contentType)
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
                else if (HttpResponseMessage.StatusCode == Windows.Web.Http.HttpStatusCode.Unauthorized)
                {
                    int? newAccessToken = await GetAccessToken();
                    return await PostToWns(secret, sid, uri, currentOAuthToken, xml, notificationType, contentType);
                }
                else if (HttpResponseMessage.StatusCode == Windows.Web.Http.HttpStatusCode.Gone || HttpResponseMessage.StatusCode == Windows.Web.Http.HttpStatusCode.NotFound)
                {
                    currentChannelUrl = "";
                    return await Task.FromResult(0);
                }
                else if (HttpResponseMessage.StatusCode == Windows.Web.Http.HttpStatusCode.NotAcceptable)
                {
                    return await Task.FromResult(0);
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

        #endregion

    }


    [DataContract]
    public sealed class OAuthToken
    {
        [DataMember(Name = "access_token")]
        public string AccessToken { get; set; }
        [DataMember(Name = "token_type")]
        public string TokenType { get; set; }
    }


    public sealed class Product
    {
        public string Description { get; set; }
        public string Forum { get; set; }
        public string Operator { get; set; }
        public string Owner { get; set; }
        public string Platform { get; set; }
    }

    public sealed class EscalationStatus
    {
        public int StatusId { get; set; }
        public string Status { get; set; }
        public string StatusType { get; set; }
    }

    public sealed class EscalationThread
    {

        public string ThreadID { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string Forum { get; set; }
        public string Platform { get; set; }
        public DateTimeOffset ThreadCreatedDatetime { get; set; }
        public DateTimeOffset LastreplyDatetime { get; set; }
        public bool LastreplyFromOp { get; set; }
        public DateTimeOffset EscalatedDatetime { get; set; }
        public string VendorAlias { get; set; }
        public string FteAlias { get; set; }
        public string Reason { get; set; }
        public string DEscription { get; set; }
        public int Labor { get; set; }
        public string SREscalationId { get; set; }
        public string Status { get; set; }
    }

    public sealed class Messagesss
    {
        public string MessageId { get; set; }
        public string MessageType { get; set; }
        public string FteAlias { get; set; }
        public string VendorAlias { get; set; }
        public string Url { get; set; }
        public string Forum { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Reason { get; set; }
        public string PreStatus { get; set; }
        public string CurStatus { get; set; }
        public string Producer { get; set; }
        public string Operation { get; set; }
    }


}
