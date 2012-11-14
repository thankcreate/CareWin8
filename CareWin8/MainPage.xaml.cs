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
using Windows.UI.Popups;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace CareWin8
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : CareWin8.Common.LayoutAwarePage
    {
        ProgressBarHelper m_progressBarHelper;
        int a = 1;

        public MainPage()
        {
           this.InitializeComponent();
           PictureGridView.AddHandler(UIElement.PointerWheelChangedEvent, new Windows.UI.Xaml.Input.PointerEventHandler(OnPointerWheelChanged), true);
           StatusGridView.AddHandler(UIElement.PointerWheelChangedEvent, new Windows.UI.Xaml.Input.PointerEventHandler(OnPointerWheelChanged), true);
           this.DataContext = App.MainViewModel;
        }

        private void OnPointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            e.Handled = false;

        }

        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。Parameter
        /// 属性通常用于配置页。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (App.MainViewModel.IsChanged)            
            {
                App.MainViewModel.IsChanged = false;
                RefreshMainViewModel();                
            }           
        }

        private void RefreshMainViewModel()
        {
            if (m_progressBarHelper == null)
            {
                m_progressBarHelper = new ProgressBarHelper(MainProgessBar, RefreshViewerHelper.RefreshViewItems);
            }
            m_progressBarHelper.PushTask("SinaWeibo");

            // 1.Weibo
            RefreshModelSinaWeibo();
        }

        private async void RefreshModelSinaWeibo()
        {
            App.MainViewModel.SinaWeiboPicItems.Clear();
            App.MainViewModel.SinaWeiboItems.Clear();
            if (String.IsNullOrEmpty(PreferenceHelper.GetPreference("SinaWeibo_ID")))
            {
                m_progressBarHelper.PopTask("Sina");
                return;
            }
            String followerID = PreferenceHelper.GetPreference("SinaWeibo_FollowerID");
            if (String.IsNullOrEmpty(followerID))
            {
                DialogHelper.Show("尚未设置新浪微博关注对象");  
                m_progressBarHelper.PopTask();
                return;
            }

            // TODO :30你懂的
            SinaWeiboSDK.GetUserTimelineResponse response = await App.SinaWeiboAPI.StatuesAPI.GetUserTimeline(followerID, 20);
            if (response.Error == RestBase.RestError.ERROR_SUCCESS && response.statuses != null)
            {
                foreach (SinaWeiboSDK.Status status in response.statuses)
                {
                    ItemViewModel model = SinaWeiboConverter.ConvertItemToCommon(status);
                    if (model != null)
                    {
                        App.MainViewModel.SinaWeiboItems.Add(model);
                    }
                }
                m_progressBarHelper.PopTask();
            }
            else
            {
                if (response.Error == RestBase.RestError.ERROR_EXPIRED)
                {
                    DialogHelper.Show("新浪微博帐号已过期，请重新登陆", "温馨提示");
                    PreferenceHelper.RemoveSinaWeiboLoginAccountPreference();
                }
                m_progressBarHelper.PopTask();
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AccountView));
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            
        }

        private void StatusGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = StatusGridView.SelectedIndex;
            if (index != -1)
            {
                ItemViewModel model = App.MainViewModel.Items[index];
                Frame.Navigate(typeof(StatusDetailView), model);
            }
            StatusGridView.SelectedIndex = -1;
        }
    }
}
