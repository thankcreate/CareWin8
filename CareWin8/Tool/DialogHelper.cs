using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace CareWin8
{
    public class DialogHelper
    {
        public static async void Show(String text)
        {
            MessageDialog dlg = new MessageDialog(text);
            dlg.ShowAsync();
        }

        public static async void Show(String content, String title )
        {
            MessageDialog dlg = new MessageDialog(content, title);
            dlg.ShowAsync();
        }
    }
}
