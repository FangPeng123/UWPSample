using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscalationSystem.Models
{
   public class EscalationStatusWithSelectedItem:ViewModelBase
    {
        public ObservableCollection<EscalationStatus> MyEscalationStatusList { get; set; }

        private EscalationStatus Selected_Item;
        public EscalationStatus SelectedItem
        {
            get
            {
                return Selected_Item;
            }

            set
            {
                Selected_Item = value;
                RaisePropertyChanged("SelectedItem");
            }
        }
    }
}
