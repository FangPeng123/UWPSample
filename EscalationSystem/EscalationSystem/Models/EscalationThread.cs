using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscalationSystem.Models
{
    public class EscalationThread: ViewModelBase
    {

        public string ThreadId { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string Forum { get; set; }
        public string Platform { get; set; }
        public DateTime ThreadCreatedDatetime { get; set; }
        public DateTime LastreplyDatetime { get; set; }
        public bool LastreplyFromOp { get; set; }
        public DateTime EscalatedDatetime { get; set; }
        public string VendorAlias { get; set; }
        public string FteAlias { get; set; }
        public string Reason { get; set; }
        public string Description { get; set; }
        public int Labor { get; set; }
        public string SrescalationId { get; set; }
        public string Status { get; set; }
        
        public bool IsManaged { get; set; }
        
        //public string EscalationReason { get; set; }
        //public string EscalationStatus { get; set; }
        //public DateTime ThreadCreateDate { get; set; }
        //public DateTime EscalationDate { get; set; }
        //public string ThreadOwner { get; set; }
        //public string FTEOnwer { get; set; }
        //public string Platform { get; set; }
    }
}
