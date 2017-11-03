using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscalationSystem.Models
{
   public class EscalationAndStatusThread:ViewModelBase
    {
        public EscalationThread EscalationThread { get; set; }
        public ObservableCollection<EscalationStatus> EscalationStatusList { get; set; }
    }
}
