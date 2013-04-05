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
    public sealed partial class PreferenceSettingControl : UserControl
    {
        Popup pop = null;  

        public PreferenceSettingControl()
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
            Init();
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

        private void Init()
        {
            int sinaIndex = ValueToIndex(PreferenceHelper.GetPreference("SinaWeibo_RecentCount"));
            comboSinaWeibo.SelectedIndex = sinaIndex;

            int renrenIndex = ValueToIndex(PreferenceHelper.GetPreference("Renren_RecentCount"));
            comboRenren.SelectedIndex = renrenIndex;

            int doubanIndex = ValueToIndex(PreferenceHelper.GetPreference("Renren_DoubanCount"));
            comboDouban.SelectedIndex = doubanIndex;

            if (App.MainViewModel.NeedFetchImageInRetweet == "True")
            {
                forwardSwitch.IsOn = true;
            }
            else
            {
                forwardSwitch.IsOn = false;
            }
            m_bInitFinished = true;
        }

        private static String[] source = { "30", "40", "50" };
        private bool m_bInitFinished = false;
        private int ValueToIndex(string value)
        {
            for (int i = 0; i < source.Length; i++)
            {
                if (value == source[i])
                    return i;
            }
            // 失败了返回0，这里其实是默认值的说~
            return 1;
        }

        private String IndexToValue(int index)
        {
            if (index >= 0 && index < source.Length)
                return source[index];
            // 失败了返回40
            return source[1];
        }

        private void ComboSinaWeibo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboSinaWeibo == null)
                return;
            int index = comboSinaWeibo.SelectedIndex;
            String recentCount = IndexToValue(index);
            // Selection_Changed会比Init发生得早，若此时SerPreference
            // Init过程中得到的那个Preference就被覆盖了
            if (m_bInitFinished)
            {
                PreferenceHelper.SetPreference("SinaWeibo_RecentCount", recentCount);
            }      
        }

        private void ComboRenren_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboRenren == null)
                return;
            int index = comboRenren.SelectedIndex;
            String recentCount = IndexToValue(index);
            // Selection_Changed会比Init发生得早，若此时SerPreference
            // Init过程中得到的那个Preference就被覆盖了
            if (m_bInitFinished)
            {
                PreferenceHelper.SetPreference("Renren_RecentCount", recentCount);
            }    
        }

        private void ComboDouban_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (comboDouban == null)
                return;
            int index = comboDouban.SelectedIndex;
            String doubanCount = IndexToValue(index);
            // Selection_Changed会比Init发生得早，若此时SerPreference
            // Init过程中得到的那个Preference就被覆盖了
            if (m_bInitFinished)
            {
                PreferenceHelper.SetPreference("Douban_RecentCount", doubanCount);
            }
        }

        private void ForwardSwitch_Goggled(object sender, RoutedEventArgs e)
        {
            if (forwardSwitch == null)
                return;
            bool isOn = forwardSwitch.IsOn;
            if (isOn)
            {
                App.MainViewModel.NeedFetchImageInRetweet = "True";
            }
            else 
            {
                App.MainViewModel.NeedFetchImageInRetweet = "False";
            }
        }  
    }
}
