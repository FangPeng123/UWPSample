using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace DTSelectorWithCVS
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public ObservableCollection<test> groups { get; set; }

        public MainPage()
        {
            this.InitializeComponent();

           

          groups = new ObservableCollection<test>();

            var test1 = new test();
            var g = new ObservableCollection<GroupInfoList>();
           
                GroupInfoList t = new GroupInfoList();
                t.Key = "A";
                g.Add(t);
            GroupInfoList t1 = new GroupInfoList();
            t1.Key = "B";
            g.Add(t1);
            GroupInfoList t11 = new GroupInfoList();
            t11.Key = "C";
            g.Add(t11);
            test1.groupInfoList = g;
            test1.heard = "group1";

            groups.Add(test1);

            var test11 = new test();
            var gg = new ObservableCollection<GroupInfoList>();
            foreach (var item in new string[] { "D" })
            {
                GroupInfoList t111 = new GroupInfoList();
                t111.Key = item;
                gg.Add(t111);
            }
            test11.groupInfoList = gg;
            test11.heard = "group11";
             
            groups.Add(test11);

            //CVS.Source = groups;
           
            //CVS.ItemsPath = new PropertyPath("groupInfoList");

            //int t0 = MyGridview.Items.Count;

        }

    }

    public class test
    {
        public ObservableCollection<GroupInfoList> groupInfoList { get; set; }
        public string heard { get; set; }
    }

    public class GroupInfoList
    { 
        public string Key { get; set; }
    }

    public class GroupEmptyOrFullSelectorGroupheader : DataTemplateSelector
    {
        private DataTemplate _full;
        private DataTemplate _empty;
        public DataTemplate Full
        {
            set { _full = value; }
            get { return _full; }
        }
        public DataTemplate Empty
        {
            set { _empty = value; }
            get { return _empty; }
        }


        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if ((item as test).heard == "group1")
            {
                return Empty;
            }
            else
            {
                return Full;
            }
        }
    }


    public class GroupEmptyOrFullSelectorGridview : DataTemplateSelector
    {
        private DataTemplate _full;
        private DataTemplate _empty;
        public DataTemplate Full
        {
            set { _full = value; }
            get { return _full; }
        }
        public DataTemplate Empty
        {
            set { _empty = value; }
            get { return _empty; }
        }


        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            if ((item as GroupInfoList).Key == "D")
            {
                return Empty;
            }
            else
            {
                return Full;
            }
        }
    }
}
