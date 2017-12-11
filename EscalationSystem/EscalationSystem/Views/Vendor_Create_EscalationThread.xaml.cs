using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Email;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using LightBuzz.SMTP;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace EscalationSystem.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class Vendor_Create_EscalationThread : Page
    {
        public Vendor_Create_EscalationThread()
        {
            this.InitializeComponent();
            Windows.Storage.ApplicationDataContainer localSettings =
    Windows.Storage.ApplicationData.Current.LocalSettings;
            var currentUser = localSettings.Values["currentUser"];
            var currentUserAlias = localSettings.Values["currentUserAlias"];
            ThreadOnwerTxt.Text = currentUserAlias.ToString();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {

            SmtpClient client = new SmtpClient("smtp-mail.outlook.com", 587, false, "DevComEN@outlook.com", "Password01!");

            try
            {
                EmailMessage emailMessage = new EmailMessage();

                emailMessage.To.Add(new EmailRecipient("fapeng@microsoft.com"));
                emailMessage.CC.Add(new EmailRecipient("fapeng@microsoft.com"));
                emailMessage.Subject = "[Thread Escalation][Barry Wang]Can not connect ot the BLE device after creators update in UWP";
                emailMessage.Body = "Please check the attachment for more detailed information about this Escalation Thread.";
                Windows.Storage.StorageFolder storageFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
                Windows.Storage.StorageFile sampleFile =
    await storageFolder.CreateFileAsync("EscalationDetails.txt",
        Windows.Storage.CreationCollisionOption.ReplaceExisting);
                //Windows.Storage.StorageFile sampleFile =await Windows.Storage.StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///test.txt"));
                List<string> lines = new List<string>();
                lines.Add("Thread Title:");
                lines.Add("[RS2:1703]Can not connect ot the BLE device after creators update");
                lines.Add("----------------------------------------------------------------------------------------------------------------------");
                lines.Add("Thread URL:");
                lines.Add("https://social.msdn.microsoft.com/Forums/en-US/winforms/thread/fda7e9db-d2e0-411c-8f4f-75efa7f0af53/");
                lines.Add("----------------------------------------------------------------------------------------------------------------------");
                lines.Add("Issue Description:");
                lines.Add("eeeeeeeeeeeeeeeeeee");
                lines.Add("----------------------------------------------------------------------------------------------------------------------");
                lines.Add("Escalation Reason:");
                lines.Add("dffffffffff");
                await Windows.Storage.FileIO.WriteLinesAsync(sampleFile, lines);
                var stream = await sampleFile.OpenStreamForReadAsync();
                IRandomAccessStream randomAccessStream = stream.AsRandomAccessStream();
                var streamReference = Windows.Storage.Streams.RandomAccessStreamReference.CreateFromStream(randomAccessStream);
                emailMessage.Attachments.Add(new EmailAttachment(sampleFile.Name, streamReference));
                await client.SendMailAsync(emailMessage);
                MessageDialog dialog = new MessageDialog("Send the Escalation Email Successuflly!");
                await dialog.ShowAsync();
            }
            catch (Exception ex)
            {
                MessageDialog dialog = new MessageDialog(ex.Message);
                await dialog.ShowAsync();
            }

        }

    }
}