using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetHtmlUWP.Helper
{
  public class AllForumThread
    {
        public Dictionary<Uri, Dictionary<string, Uri>> MVPThreadList = new Dictionary<Uri, Dictionary<string, Uri>>();

        public async Task<Dictionary<Uri, Dictionary<string, Uri>>> GetForumThreadList(Uri Uri)
        {
            
            HelperClass gethtml = new HelperClass();
            string htmlcontent = await gethtml.Gethtmlsting(Uri);
            HtmlAgilityPack.HtmlDocument htmldoc = new HtmlDocument();
            htmldoc.LoadHtml(htmlcontent);
            //Get Thread Link
            var node = htmldoc.DocumentNode.Descendants("div").Where(d => d.Attributes.Contains("class") && d.Attributes["class"].Value.Contains("detailscontainer"));
            foreach(var littlenode in node)
            {
                var threadlittlenode = littlenode.Descendants("a").Where(d => d.Attributes.Contains("href")).FirstOrDefault();
                var threadurl = threadlittlenode.Attributes["href"].Value;
                Thread MVPThread=await gethtml.GetThreadDetials(new Uri(threadurl));
                if (MVPThread.mvp.Count >= 1)
                {
                    MVPThreadList.Add(MVPThread.ThreadURL, MVPThread.mvp);
                }

                // 不能为null


                //else
                //{
                //    Dictionary<string, Uri> MyDisctionary = new Dictionary<string, Uri>();
                //    MyDisctionary.Add("0", new Uri("http://fangtest.com"));
                //    MVPThreadList.Add(new Uri("http://fangtest.com"), MyDisctionary);
                //}
                
            }

            return MVPThreadList;
        }
    }
}
