using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Collections.ObjectModel;
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
using System.ComponentModel;

// “基本页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234237 上有介绍

namespace CareWin8
{
    /// <summary>
    /// 基本页，提供大多数应用程序通用的特性。
    /// </summary>
    public sealed partial class SelectFriendView : CareWin8.Common.LayoutAwarePage
    {
        public ObservableCollection<FriendViewModel> FriendListInShow { get; private set; }
        public List<FriendViewModel> AllFriendList { get; private set; }

        private EntryType? m_entryType = EntryType.NotSet;
        private String m_myID = "";

        public SelectFriendView()
        {
            FriendListInShow = new ObservableCollection<FriendViewModel>();
            FriendListInShow.Add(new FriendViewModel { Name = "123" });
            FriendListInShow.Add(new FriendViewModel { Name = "345" });
            AllFriendList = new List<FriendViewModel>();
            
            this.InitializeComponent();
            FriendsGridView.ItemsSource = FriendListInShow;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            try
            {
                IDictionary<String, Object> dic = e.Parameter as IDictionary<String, Object>;
                m_entryType = dic["Type"] as EntryType?;
                m_myID = dic["MyID"] as String;                
            }
            catch (System.Exception ex)
            {
            	
            }
            InitialLoad();
        }

        // 加载所有朋友到AllFriend列表
        private void InitialLoad()
        {
            if (String.IsNullOrEmpty(m_myID))
                return;

            if (m_entryType == EntryType.SinaWeibo)
            {
                InitialLoadSinaWeibo();                
            }
        }

        private async void InitialLoadSinaWeibo()
        {
            FriendListInShow.Clear();
            AllFriendList.Clear();
            SinaWeiboSDK.GetAllFriendsResponse response = await App.SinaWeiboAPI.FriendshipAPI.GetAllFriends(m_myID);
            if (response.Error == RestBase.RestError.ERROR_SUCCESS && response.users != null)
            {
                foreach (SinaWeiboSDK.User user in response.users)
                {
                    FriendViewModel model = SinaWeiboConverter.ConvertUserToFriendViewModel(user);
                    if (model != null)
                    {
                        AllFriendList.Add(model);
                    }
                }
            }
            // TODO     
            foreach (FriendViewModel model in AllFriendList)
            {
                FriendListInShow.Add(model);
            }            
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

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtSearch.Text))
                return;
            String key = txtSearch.Text;

            List<FriendViewModel> friendsFound = new List<FriendViewModel>();
            foreach (FriendViewModel friend in AllFriendList)
            {
                if (friend.Name.ToLower().Contains(key.ToLower()))
                    friendsFound.Add(friend);
            }
            if (friendsFound.Count == 0)
            {
                MessageDialog dlg = new MessageDialog("未找到任何结果");
                dlg.ShowAsync();
            }
            else
            {
                FriendListInShow.Clear();
                foreach (FriendViewModel friend in friendsFound)
                {
                    FriendListInShow.Add(friend);
                }
            }
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            int index = FriendsGridView.SelectedIndex;
            if (index == -1)
            {
                MessageDialog dlg = new MessageDialog("未选中");
                dlg.ShowAsync();
            }
            else
            {
                FriendViewModel friend = FriendListInShow[index];
                String oldFollowerID = PreferenceHelper.GetPreference("SinaWeibo_FollowerID");
                if (!String.IsNullOrEmpty(oldFollowerID) && oldFollowerID != friend.ID)
                    App.MainViewModel.IsChanged = true;
                PreferenceHelper.SetPreference("SinaWeibo_FollowerID", friend.ID);
                PreferenceHelper.SetPreference("SinaWeibo_FollowerNickName", friend.Name);
                PreferenceHelper.SetPreference("SinaWeibo_FollowerAvatar", friend.Avatar);
                PreferenceHelper.SetPreference("SinaWeibo_FollowerAvatar2", friend.Avatar2);
                Frame.GoBack();
            }
        }

        private void FriendsGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = FriendsGridView.SelectedIndex;
            if (index == -1)
            {
                MessageDialog dlg = new MessageDialog("未选中");
                dlg.ShowAsync();
            }
            else
            {
                FriendViewModel friend = FriendListInShow[index];
                String oldFollowerID = PreferenceHelper.GetPreference("SinaWeibo_FollowerID");
                if (!String.IsNullOrEmpty(oldFollowerID) && oldFollowerID != friend.ID)
                    App.MainViewModel.IsChanged = true;
                PreferenceHelper.SetPreference("SinaWeibo_FollowerID", friend.ID);
                PreferenceHelper.SetPreference("SinaWeibo_FollowerNickName", friend.Name);
                PreferenceHelper.SetPreference("SinaWeibo_FollowerAvatar", friend.Avatar);
                PreferenceHelper.SetPreference("SinaWeibo_FollowerAvatar2", friend.Avatar2);
                Frame.GoBack();
            }
        }        
    }
}
