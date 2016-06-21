using System;
using System.Collections.Generic;
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

namespace X_Bind
{
    public class test
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public List<test> testlist { get; set; }
        public ViewModel myviewmode { get; set; }
        public MainPage()
        {
            this.InitializeComponent();
            testlist = new List<test>();
            testlist.Add(new test() { id="test",name="name1"});
            testlist.Add(new test() { id = "test1", name = "name1" });
            testlist.Add(new test() { id = "test2", name = "name1" });
            testlist.Add(new test() { id = "test3", name = "name1" });

            myviewmode = new ViewModel();
        }
    }
}
