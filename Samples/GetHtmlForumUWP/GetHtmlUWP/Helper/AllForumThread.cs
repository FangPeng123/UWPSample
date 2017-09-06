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
        public async Task<List<Uri>> GetForumThreadList(Uri Uri)
        {
            HelperClass gethtml = new HelperClass();
            string htmlcontent = await gethtml.Gethtmlsting(Uri);
            HtmlAgilityPack.HtmlDocument htmldoc = new HtmlDocument();
            htmldoc.LoadHtml(htmlcontent);
        }
    }
}
