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
using HtmlAgilityPack;
using Windows.Web.Http;
using System.Threading.Tasks;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace GetHtmlUWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public string htmlcontent;
        public MainPage()
        {
            this.InitializeComponent();
        }

       public async Task<string> htmlsting()
        {
            HttpClient client = new HttpClient();
            htmlcontent=await client.GetStringAsync(new Uri("https://social.msdn.microsoft.com/Forums/windowsapps/en-US/9b0c9ab0-b682-4fb6-812a-28550c4a7c4b/readingwriting-to-usb-device?forum=wpdevelop"));
            return htmlcontent;
        
        }
        

        private async void GetHtmlstringButton_Click(object sender, RoutedEventArgs e)
        {
            await htmlsting();

            HtmlAgilityPack.HtmlDocument htmldoc = new HtmlDocument();
            htmldoc.LoadHtml(htmlcontent);
            var node=htmldoc.DocumentNode.Descendants("link").Where(d => d.Attributes.Contains("title")).FirstOrDefault();
            var value = node.Attributes["title"].Value;

            var node1 = htmldoc.DocumentNode.Descendants("ul").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("history")).FirstOrDefault();
            var node2 = node1.Descendants("a").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("author")).FirstOrDefault();
            var t = node2.Attributes["title"].Value;
            List<string> t1= new List<string>(); ;
            var nodeuserinfor = htmldoc.DocumentNode.Descendants("div").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("userInfo"));
            foreach(var mynode in nodeuserinfor)
            {
                int conunt = nodeuserinfor.Count();
                var text = mynode.Descendants("div").Where(d => d.Attributes.Contains("class")).FirstOrDefault();
                string value1 = text.Attributes["data-profile-usercard-customlink"].Value;
                var ttt = value1.Split(',')[1];
                string user = ttt.Split(':')[1];
                int lenth = user.Length;
                string myuser = user.Substring(1, lenth - 17);

            
                t1.Add(myuser);

            }

            for (int i = 0; i < t1.Count(); i++)
            {
                for (int j =t1.Count-1; j>=0; j--)
                {
                    if (t1[i] == t1[j]&&i!=j)
                    {
                        t1.RemoveAt(j);
                        break;
                    }
                   
                }
            }

            int t2 = t1.Count();
        }
    }
}
