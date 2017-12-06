using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Xml.Dom;
using Windows.UI.Notifications;

namespace EscalationSystem.MessageNotification
{
    /// <summary>
    /// Toast 通知类
    /// </summary>
    public static class SHToastNotification
    {
        /// <summary>
        /// 创建Toast通知
        /// </summary>
        /// <param name="assetsImageFileName">图片名称-默认在Assets文件夹中的图片文件</param>
        /// <param name="text">显示文本</param>
        /// <param name="audioName">枚举，声音提示</param>
        /// /// <param name="Scheduled">是否定时延期通知，默认2秒后执行</param>
        public static void ShowToastNotification(string assetsImageFileName, string text, NotificationAudioNames audioName, bool Scheduled = false)
        {
            try
            {
                //01 创建toast 通知XML模板，选择ToastImageAndText01。
                ToastTemplateType toastTemplate = ToastTemplateType.ToastImageAndText01;
                XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);

                //02 显示文本信息
                XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");
                toastTextElements[0].AppendChild(toastXml.CreateTextNode(text));

                //03 显示图片信息
                XmlNodeList toastImageAttributes = toastXml.GetElementsByTagName("image");
                ((XmlElement)toastImageAttributes[0]).SetAttribute("src", $"ms-appx:///assets/{assetsImageFileName}");
                ((XmlElement)toastImageAttributes[0]).SetAttribute("alt", "Logo");

                //04 设定显示时间间隔： 短，中，长
                IXmlNode toastNode = toastXml.SelectSingleNode("/toast");
                ((XmlElement)toastNode).SetAttribute("duration", "short");

                //05 设定通知系统声音
                XmlElement audio = toastXml.CreateElement("audio");
                audio.SetAttribute("src", $"ms-winsoundevent:Notification.{audioName.ToString().Replace("_", ".")}");

                //06 设定，点击通知时，返回App OnLaunched 事件中的参数，APP 根据参数，可以指定页面并作相应的动作
                ((XmlElement)toastNode).SetAttribute("launch", "{\"type\":\"toast\", \"param1\": \"3697\"}");

                //07 定时执行通知
                if (Scheduled)
                {
                    ScheduledToastNotification toast3 = new ScheduledToastNotification(toastXml, DateTimeOffset.Now.AddSeconds(2));
                    ToastNotificationManager.CreateToastNotifier().AddToSchedule(toast3);
                }
                else
                {
                    // 直接发送toast 通知。
                    ToastNotification toast = new ToastNotification(toastXml);
                    ToastNotificationManager.CreateToastNotifier().Show(toast);
                }
            }
            catch(Exception e)
            {
                throw new Exception("Create Toast Nitification failed! ", e);
            }
        }

    }

    /// <summary>
    /// 通知声音枚举
    /// </summary>
    public enum NotificationAudioNames
    {
        Default,
        IM,
        Mail,
        Reminder,
        SMS,
        Looping_Alarm,
        Looping_Alarm2,
        Looping_Alarm3,
        Looping_Alarm4,
        Looping_Alarm5,
        Looping_Alarm6,
        Looping_Alarm7,
        Looping_Alarm8,
        Looping_Alarm9,
        Looping_Alarm10,
        Looping_Call,
        Looping_Call2,
        Looping_Call3,
        Looping_Call4,
        Looping_Call5,
        Looping_Call6,
        Looping_Call7,
        Looping_Call8,
        Looping_Call9,
        Looping_Call10,
    }
}
