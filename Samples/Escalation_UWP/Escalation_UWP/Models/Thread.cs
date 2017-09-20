using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escalation_UWP.Models
{
   public class Thread
    {
        public string ThreadLink { get; set; }
        public string ThreadTitle { get; set; }
        public DateTime ThreadCreateDate { get; set; }
        public string OP { get; set; }
        public string LastMemeber { get; set; }
        public DateTime ThreadLastReplytime { get; set; }
    }
}
