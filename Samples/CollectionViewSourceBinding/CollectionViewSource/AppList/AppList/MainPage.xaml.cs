using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AppList
{
    public class testclas
    {
        private string _ChapterName;
        public string ChapterName
        {
            get { return _ChapterName; }
            set
            {
                _ChapterName = value;
               
            }
        }
  
    }


    public class mytest
    {
        public string gropuanme { get; set; }
        public List<testclas> tetslist { get; set; }
    }
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public List<mytest> mytestlist { get; set; }
        public MainPage()
        {
            this.InitializeComponent();
            mytestlist = new List<mytest>();
            mytest test = new mytest();
            test.gropuanme = "Amy";
            test.tetslist = new List<testclas>();
            test.tetslist.Add(new testclas() { ChapterName = "Amy1" });
            test.tetslist.Add(new testclas() { ChapterName = "Amy11" });
            test.tetslist.Add(new testclas() { ChapterName = "Amy111" });
            mytestlist.Add(test);
            mytest test1 = new mytest();
            test1.gropuanme = "bmy";
            test1.tetslist = new List<testclas>();
            test1.tetslist.Add(new testclas() { ChapterName = "bmy1" });
            test1.tetslist.Add(new testclas() { ChapterName = "bmy11" });
            test1.tetslist.Add(new testclas() { ChapterName = "bmy111" });
            mytestlist.Add(test1);
            ContactsCVS.Source = mytestlist;
            ContactsCVS.ItemsPath = new PropertyPath("tetslist");
        }

        
    }
}
