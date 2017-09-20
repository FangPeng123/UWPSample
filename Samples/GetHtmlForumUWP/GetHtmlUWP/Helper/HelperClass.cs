using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace GetHtmlUWP.Helper
{
    public class HelperClass
    {
       public int count = 0;
        public async Task<string> Gethtmlsting(Uri Uri)
        {
            HttpClient client = new HttpClient();
            string htmlcontent = await client.GetStringAsync(Uri);
            return htmlcontent;

        }

        public async Task<Thread> GetThreadDetials(Uri Uri)
        {
            Thread thread = new Thread();
            string htmlcontent = await Gethtmlsting(Uri);
            HtmlAgilityPack.HtmlDocument htmldoc = new HtmlDocument();
            htmldoc.LoadHtml(htmlcontent);
            //get thread title
            var node = htmldoc.DocumentNode.Descendants("link").Where(d => d.Attributes.Contains("title")).FirstOrDefault();
            string threadtitle = node.Attributes["title"].Value;
            //get thread create date
            var nodedate = htmldoc.DocumentNode.Descendants("div").Where(d => d.Attributes.Contains("class")&&d.Attributes["class"].Value.Contains("date")).FirstOrDefault();
            string threaddate = nodedate.InnerText;
            //get users in Thread
            string op = "";
            Dictionary<string, Uri> users = new Dictionary<string, Uri>();
            var nodeuserinfor = htmldoc.DocumentNode.Descendants("div").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("userInfo"));
            foreach (var mynode in nodeuserinfor)
            {
                //get user name
                var usersnode = mynode.Descendants("div").Where(d => d.Attributes.Contains("class")).FirstOrDefault();
                string usersvalue = usersnode.Attributes["data-profile-usercard-customlink"].Value;
                var usersvalue1 = usersvalue.Split(':')[3];
                string usersvalue2 = usersvalue1.Split('"')[1];
                int lenth = usersvalue2.Length;
                string myuser = usersvalue2.Substring(0, lenth - 14);

                //get user profile
               
                string uriuserprofile = "https://social.msdn.microsoft.com/profile/" + myuser + "/?ws=usercard-mini";
                //string uriuserprofile = "https://social.msdn.microsoft.com/profile/Annievia%20Chen/?ws=usercard-mini";

                count = count + 1;
                if (count == 1)
                {
                    users.Add(myuser, new Uri(uriuserprofile));
                }
                else
                {
                    int j = 0;
                    for (int i = 0; i < users.Count; i++)
                    {
                        var item = users.ElementAt(i);
                        var itemKey = item.Key;
                        if(itemKey==myuser)
                        {
                            j = 1;
                        }
                        
                    }
                   
                    if(j==0)
                    {
                        users.Add(myuser, new Uri(uriuserprofile));
                    }


                }
                
            }
            Dictionary<string, Uri> MVPList = new Dictionary<string, Uri>();
            

                    foreach(KeyValuePair<string, Uri> entry in users)
                     {
                       bool isMVP=await GetProfileMVPDetials(entry.Value);
                       if(isMVP)
                       {
                         MVPList.Add(entry.Key, entry.Value);
                       }
                     }
                     
                    thread.op = op;
                    thread.ThreadURL = Uri;
                    thread.ThreadTitle = threadtitle;
                    thread.communitymemebers = users;
                    thread.mvp = MVPList;
                   
        

            thread.CreateDate = DateTime.Parse(threaddate);
                    return thread;
                }


        public async Task<bool> GetProfileMVPDetials(Uri Uri)
         {
            string htmlcontent = await Gethtmlsting(Uri);
            HtmlAgilityPack.HtmlDocument htmldoc = new HtmlDocument();
            htmldoc.LoadHtml(htmlcontent);
            var node = htmldoc.DocumentNode.Descendants("p").Where(d => d.Attributes.Contains("class")&&d.Attributes["class"].Value.Contains("avatar-affiliation")).FirstOrDefault();
            if(node==null)
            {
                return false;
            }

            else
            {
                string MVP = node.Descendants("a").FirstOrDefault().InnerText;
                if (MVP.Split(',')[0] == "MVP")
                {
                    return true;
                }

                else return false;
            }

        }


    }


}
    


