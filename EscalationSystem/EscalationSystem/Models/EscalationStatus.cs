using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscalationSystem.Models
{
   public class EscalationStatus:ViewModelBase
    {
        public int StatusId { get; set; }
        public string Status { get; set; }
        public string StatusType { get; set; }
    }
}
