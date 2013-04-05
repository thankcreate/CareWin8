using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// “用户控件”项模板在 http://go.microsoft.com/fwlink/?LinkId=234236 上提供

namespace CareWin8.MyControl
{
    public sealed partial class PreferenceAboutControl : UserControl
    {
        Popup pop = null;  

        public PreferenceAboutControl()
        {
            this.InitializeComponent();
            this.Width = 346;
            this.Height = Window.Current.Bounds.Height;//与当前窗口等高  
            this.pop = new Popup();
            this.pop.Child = this;
            this.pop.IsLightDismissEnabled = true;
            // 将Popup定位在屏幕的右侧  
            pop.HorizontalOffset = Window.Current.Bounds.Width - this.Width;
            pop.VerticalOffset = 0;
            // 为控件设置呈现动画  
            this.Transitions = new Windows.UI.Xaml.Media.Animation.TransitionCollection();
            Windows.UI.Xaml.Media.Animation.EdgeUIThemeTransition et = new Windows.UI.Xaml.Media.Animation.EdgeUIThemeTransition();
            et.Edge = EdgeTransitionLocation.Right;
            this.Transitions.Add(et);
        }


        public void Show()
        {
            if (pop != null)
            {
                pop.IsOpen = true;
            }
        }

        public void Hide()
        {
            if (pop != null)
            {
                pop.IsOpen = false;
            }
        }

        private void onBack(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Windows.UI.ApplicationSettings.SettingsPane.Show();
        }

        private void GotoSite_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                Uri uri = new Uri("http://thankcreate.github.com/Care");
                Windows.System.Launcher.LaunchUriAsync(uri); 
            }
            catch (System.Exception ex)
            {
            	
            }
           
        }

        private void SendMail_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                String url = String.Format("mailto:?to={0}&subject={1}&body={2}",
                "thankcreate@gmail.com",
                "论改进一下的必要性",
                "---来自我的Win8设备");
                Windows.System.Launcher.LaunchUriAsync(new Uri(url)); 
            }
            catch (System.Exception ex)
            {
            	
            }            
        }
    }
}
