using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EscalationSystem.ViewModels
{
   public class SceenSizeViewModel: INotifyPropertyChanged
    {

        private double _screenWidth;


        public double ScreenWidth { get { return _screenWidth; } set { _screenWidth = value; OnPropertyChanged("ScreenWidth"); } }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
