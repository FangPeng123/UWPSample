using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscalationSystem.Models
{
   public class Report
    {
        public string Alias { get; set; }
        public int ClosedNum { get; set; }
        public string ClosedRate { get; set; }
        public string Forum { get; set; }
        public DateTime FromDateTime { get; set; }
        public int MarkedNum { get; set; }
        public string MarkedRate { get; set; }
        public int OpenNum { get; set; }
        public string OpenRate { get; set; }
        public string Platform { get; set; }
        public DateTime ToDateTime { get; set; }       
        public int TotalNum { get; set; }       
    }
}
