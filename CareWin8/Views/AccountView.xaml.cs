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
using CareWin8.MyControl;
using SinaWeiboSDK;
using RestBase;

// “基本页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234237 上有介绍

namespace CareWin8
{
    /// <summary>
    /// 基本页，提供大多数应用程序通用的特性。
    /// </summary>
    public sealed partial class AccountView : CareWin8.Common.LayoutAwarePage
    {
        #region SinaWeiboNameProperty
        public static readonly DependencyProperty SinaWeiboNameProperty =
            DependencyProperty.Register("SinaWeiboName", typeof(string), typeof(AccountView), new PropertyMetadata((string)""));

        public string SinaWeiboName
        {
            get { return (string)GetValue(SinaWeiboNameProperty); }
            set { SetValue(SinaWeiboNameProperty, value); }
        }
        #endregion

        #region SinaWeiboAvatarProperty
        public static readonly DependencyProperty SinaWeiboAvatarProperty =
            DependencyProperty.Register("SinaWeiboAvatar", typeof(string), typeof(AccountView), new PropertyMetadata((string)""));

        public string SinaWeiboAvatar
        {
            get { return (string)GetValue(SinaWeiboAvatarProperty); }
            set { SetValue(SinaWeiboAvatarProperty, value); }
        }
        #endregion

        #region SinaWeiboFollowerNameProperty
        public static readonly DependencyProperty SinaWeiboFollowerNameProperty =
            DependencyProperty.Register("SinaWeiboFollowerName", typeof(string), typeof(AccountView), new PropertyMetadata((string)""));

        public string SinaWeiboFollowerName
        {
            get { return (string)GetValue(SinaWeiboFollowerNameProperty); }
            set { SetValue(SinaWeiboFollowerNameProperty, value); }
        }
        #endregion

        #region SinaWeiboFollowerAvatarProperty
        public static readonly DependencyProperty SinaWeiboFollowerAvatarProperty =
            DependencyProperty.Register("SinaWeiboFollowerAvatar", typeof(string), typeof(AccountView), new PropertyMetadata((string)""));

        public string SinaWeiboFollowerAvatar
        {
            get { return (string)GetValue(SinaWeiboFollowerAvatarProperty); }
            set { SetValue(SinaWeiboFollowerAvatarProperty, value); }
        }
        #endregion

        public AccountView()
        {
            this.InitializeComponent();
        }

        private void RefreshUI()
        {
            SinaWeiboName = PreferenceHelper.GetPreference("SinaWeibo_NickName");
            SinaWeiboFollowerName = PreferenceHelper.GetPreference("SinaWeibo_FollowerNickName");
            SinaWeiboAvatar = PreferenceHelper.GetPreference("SinaWeibo_Avatar");
            SinaWeiboFollowerAvatar = PreferenceHelper.GetPreference("SinaWeibo_FollowerAvatar");
            if (string.IsNullOrEmpty(SinaWeiboName))
            {
                SinaWeiboName = "未登陆";
            }
            if (string.IsNullOrEmpty(SinaWeiboFollowerName))
            {
                SinaWeiboFollowerName = "未关注";
            }      
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            RefreshUI();
        }

        /// <summary>
        /// 使用在导航过程中传递的内容填充页。在从以前的会话
        /// 重新创建页时，也会提供任何已保存状态。
        /// </summary>
        /// <param name="navigationParameter">最初请求此页时传递给
        /// <see cref="Frame.Navigate(Type, Object)"/> 的参数值。
        /// </param>
        /// <param name="pageState">此页在以前会话期间保留的状态
        /// 字典。首次访问页面时为 null。</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
        }

        /// <summary>
        /// 保留与此页关联的状态，以防挂起应用程序或
        /// 从导航缓存中放弃此页。值必须符合
        /// <see cref="SuspensionManager.SessionState"/> 的序列化要求。
        /// </summary>
        /// <param name="pageState">要使用可序列化状态填充的空字典。</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {
        }

        private void SinaWeiboLogin_Click(object sender, RoutedEventArgs e)
        {
            SinaWeiboLoginPopup.AuthSuccessComplete = new Action(() =>
            {
                sinaWeiboLoginPopup.Close();
                SinaWeiboRefreshAccount();
            });
            sinaWeiboLoginPopup.Show(this);
        }

        // 新浪微博取当前用户信息，要先取当前用户id，再用此id去查用户信息
        private async void SinaWeiboRefreshAccount()
        {
            GetUIDResponse uidResponse  = await App.SinaWeiboAPI.AccountAPI.GetUID();
            if (uidResponse.Error == RestError.ERROR_SUCCESS && uidResponse.uid != null)
            {
                String uid = uidResponse.uid;                
                GetUserInfoResponse userInfoResponse = await App.SinaWeiboAPI.UserAPI.GetUserInfo(uid);
                if (userInfoResponse.Error == RestError.ERROR_SUCCESS && userInfoResponse.User != null)
                {
                    User user = userInfoResponse.User;
                    PreferenceHelper.SetPreference("SinaWeibo_ID", uid);
                    PreferenceHelper.SetPreference("SinaWeibo_NickName", user.screen_name);
                    PreferenceHelper.SetPreference("SinaWeibo_Avatar", user.profile_image_url);
                    SinaWeiboName = user.screen_name;
                    SinaWeiboAvatar = user.profile_image_url;
                }
            }
        }

        private void SinaWeiboSelectFriend_Click(object sender, RoutedEventArgs e)
        {
            String id = PreferenceHelper.GetPreference("SinaWeibo_ID");
            if(String.IsNullOrEmpty(id))
                return;
            Dictionary<String, Object> parameters = new Dictionary<String, Object>();
            parameters.Add("Type", EntryType.SinaWeibo);
            parameters.Add("MyID", id);
            Frame.Navigate(typeof(SelectFriendView), parameters);
        }

        private void SinaWeiboLogout_Click(object sender, RoutedEventArgs e)
        {
            SinaWeiboName = "未登陆";
            SinaWeiboFollowerName = "未关注";
            PreferenceHelper.RemoveSinaWeiboPreference();            
            App.SinaWeiboAPI.Logout();
            App.MainViewModel.IsChanged = true;
        }
        
    }
}
