using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace CareWin8
{
    class FlyoutHelper
    {
        public static void ShowInCenter(Popup popup, int width, int height)
        {
            // 至今没弄明白水平的为啥要写成0才算居中
            popup.HorizontalOffset = 0;
            popup.VerticalOffset = (Window.Current.Bounds.Height - height) / 2;
        }
    }
}
