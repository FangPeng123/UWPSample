using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscalationSystem.Models
{
   public class ProductWithSelectedItem:ViewModelBase
    {
        public ObservableCollection<Product> MyProductList { get; set; }

        private Product Selected_Item;
        public Product SelectedItem
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
