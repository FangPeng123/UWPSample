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
using Escalation_UWP.Models;
using MyToolkit.Collections;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Escalation_UWP
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Escalation_Thread_Agents : Page
    {
        public ObservableCollectionView<EscalationThread> EscalationThreadList { get; set; }
        public Escalation_Thread_Agents()
        {
            this.InitializeComponent();
            EscalationThreadList = new ObservableCollectionView<Models.EscalationThread>();
            EscalationThread EscalationThread = new EscalationThread();
            EscalationThread.ThreadTitle = "[C++][UWP] MJPEG MediaPlayer support";
            EscalationThread.ThreadLink = new Uri("https://social.msdn.microsoft.com/Forums/windowsapps/en-US/bec39605-b09d-4c3b-80a1-f40a8aaa203b/cuwp-mjpeg-mediaplayer-support?forum=wpdevelop");
            EscalationThread.EscalationReason = "I am not good at it";
            EscalationThread.FTEOnwer = "Fang Peng";
            EscalationThread.ThreadOwner = "Barry Wang";
            EscalationThread.ThreadCreateDate = DateTime.Parse("Tuesday, September 05, 2017 4:23 PM");
            EscalationThread.EscalationDate = DateTime.Parse("Wednesday, September 06, 2017 5:23 PM");
            EscalationThread.EscalationStatus = "Closed:Replied";
            EscalationThreadList.Items.Add(EscalationThread);
            EscalationThread EscalationThread1 = new EscalationThread();
            EscalationThread1.ThreadTitle = "[C++][UWP] MJPEG MediaPlayer support";
            EscalationThread1.ThreadLink = new Uri("https://social.msdn.microsoft.com/Forums/windowsapps/en-US/bec39605-b09d-4c3b-80a1-f40a8aaa203b/cuwp-mjpeg-mediaplayer-support?forum=wpdevelop");
            EscalationThread1.EscalationReason = "I am not good at it";
            EscalationThread1.FTEOnwer = "Fang Peng";
            EscalationThread1.ThreadOwner = "Barry Wang";
            EscalationThread1.ThreadCreateDate = DateTime.Parse("Tuesday, September 05, 2017 4:23 PM");
            EscalationThread1.EscalationDate = DateTime.Parse("Wednesday, September 06, 2017 5:23 PM");
            EscalationThread1.EscalationStatus = "Closed:Replied";
            EscalationThreadList.Items.Add(EscalationThread1);
            MyDataGrid.ItemsSource = EscalationThreadList;
            this.DataContext = this;
           
        }
    }
}
