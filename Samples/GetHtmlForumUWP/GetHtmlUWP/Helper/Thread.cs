using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetHtmlUWP.Helper
{
   public class Thread
    {
        public string op { get; set; }
        public Dictionary<string,Uri> communitymemebers { get; set; }
        public Dictionary<string,Uri> mvp { get; set; }
        public string ThreadTitle { get; set; }
        public Uri ThreadURL { get; set; }
         
    }
}
