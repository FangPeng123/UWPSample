using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escalation_UWP.Models
{
    public class EscalationThread
    {
        public Uri ThreadLink { get; set; }
        public string ThreadTitle { get; set; }
        public string EscalationReason { get; set; }
        public string EscalationStatus { get; set; }
        public DateTime ThreadCreateDate { get; set; }
        public DateTime EscalationDate { get; set; }
        public string ThreadOwner { get; set; }
        public string FTEOnwer { get; set; }

    }
}
