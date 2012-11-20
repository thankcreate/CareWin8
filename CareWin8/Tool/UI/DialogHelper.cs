using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Notifications;
using NotificationsExtensions;
using NotificationsExtensions.ToastContent;
namespace CareWin8
{
    // DialogHelper中包括了MessageDialog式弹出和Toast式弹出
    public class DialogHelper
    {
        public static async void ShowMessageDialog(String text)
        {
            MessageDialog dlg = new MessageDialog(text);
            dlg.ShowAsync();
        }

        public static  Windows.Foundation.IAsyncOperation<IUICommand> ShowMessageDialog(String content, String title)
        {
            MessageDialog dlg = new MessageDialog(content, title);
            return dlg.ShowAsync();
        }

        public static void ShowToastDialog(String text)
        {
            if (text == null)
                return;
            IToastNotificationContent toastContent = null;
            IToastText01 templateContent = ToastContentFactory.CreateToastText01();
            templateContent.TextBodyWrap.Text = text;
            toastContent = templateContent;
            ToastNotification toast = toastContent.CreateNotification();
            ToastNotificationManager.CreateToastNotifier().Show(toast);
        }
    }
}
