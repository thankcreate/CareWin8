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

// “基本页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234237 上有介绍

namespace CareWin8
{
    /// <summary>
    /// 基本页，提供大多数应用程序通用的特性。
    /// </summary>
    public sealed partial class AddCommentView : CareWin8.Common.LayoutAwarePage
    {

        #region LogoSourceProperty
        public static readonly DependencyProperty LogoSourceProperty =
            DependencyProperty.Register("LogoSource", typeof(string), typeof(AddCommentView), new PropertyMetadata((string)""));

        public string LogoSource
        {
            get { return (string)GetValue(LogoSourceProperty); }
            set { SetValue(LogoSourceProperty, value); }
        }
        #endregion

        #region WordCountProperty
        public static readonly DependencyProperty WordCountProperty =
            DependencyProperty.Register("WordCount", typeof(string), typeof(AddCommentView), new PropertyMetadata((string)""));

        public string WordCount
        {
            get { return (string)GetValue(WordCountProperty); }
            set { SetValue(WordCountProperty, value); }
        }
        #endregion

        #region WordMaxLength
        public static readonly DependencyProperty WordMaxLengthProperty =
            DependencyProperty.Register("WordMaxLength", typeof(string), typeof(AddCommentView), new PropertyMetadata((string)""));

        public string WordMaxLength
        {
            get { return (string)GetValue(WordMaxLengthProperty); }
            set { SetValue(WordMaxLengthProperty, value); }
        }
        #endregion

        ItemViewModel m_itemViewModel;
        CommentViewModel m_commentViewModel;
        EntryType m_type = EntryType.NotSet;
        int m_maxLenth;

        public AddCommentView()
        {
            this.InitializeComponent();
        }

        // 跳转到此页有2种情况
        // 1:对状态本身的评论  此时m_itemViewModel有值
        // 2:对评论本身的评论  此时m_itemViewModel和m_commentViewModel都需要有值
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            try
            {
                Dictionary<String, Object> parameters = e.Parameter as Dictionary<String, Object>;
                if (parameters.ContainsKey("ItemViewModel"))
                {
                    m_itemViewModel = parameters["ItemViewModel"] as ItemViewModel;
                    m_type = m_itemViewModel.Type;
                }
                if (parameters.ContainsKey("CommentViewModel"))
                    m_commentViewModel = parameters["CommentViewModel"] as CommentViewModel;
            }
            catch (System.Exception ex)
            {

            }
            ChangeUIByInputType();
        }

        private void ChangeUIByInputType()
        {
            if (m_type == EntryType.SinaWeibo)
            {
                m_maxLenth = 140;
                WordMaxLength = "140";
                WordCount = "140";
                LogoSource = "/Images/Logo/sinaweibo_logo.png";
            }
            // 人人在回复时最长也是140，只是发表新状态时可以到280
            else if (m_type == EntryType.Renren)
            {
                m_maxLenth = 140;
                WordMaxLength = "140";
                WordCount = "140";
                LogoSource = "/Images/Logo/renren_logo.png";
            }
            else if (m_type == EntryType.Douban)
            {
                m_maxLenth = 140;
                WordMaxLength = "140";
                WordCount = "140";
                LogoSource = "/Images/Logo/douban_logo.png";
            }
            // m_commentViewModel不为空说明是对评论本身的评论
            // 各平台目前对评论的回复其实和对状态本身的回复没有本质区别
            // 都是调用一样的接口，但是前面加上了一对“对XX说”之类的文字
            if (m_commentViewModel != null)
            {
                if (m_type == EntryType.SinaWeibo)
                {
                    txtContent.Text = String.Format("回复@{0}: ", m_commentViewModel.Title);
                }
                else if (m_type == EntryType.Renren)
                {
                    txtContent.Text = String.Format("回复{0}: ", m_commentViewModel.Title);
                }
                else if (m_type == EntryType.Douban)
                {
                    txtContent.Text = String.Format("@{0}: ", m_commentViewModel.DoubanUID);
                }
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

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            WordCount = (m_maxLenth - txtContent.Text.Length).ToString();
        }

        private async void SinaWeiboSend()
        {
            if (m_itemViewModel == null)
                return;
            SinaWeiboSDK.AddCommentResponse response =
                await App.SinaWeiboAPI.CommentAPI.AddComment(m_itemViewModel.ID, txtContent.Text);
            if (response.Error == RestBase.RestError.ERROR_SUCCESS)
            {
                DialogHelper.ShowToastDialog("发送成功");
                Frame.GoBack();
            }
            else
            {
                DialogHelper.ShowToastDialog("发送失败，请检查网络是否稳定");
            }
        }

        private void RenrenSend()
        {
            if (m_itemViewModel == null)
                return;
            if (m_itemViewModel.RenrenFeedType == RenrenSDK.RenrenNews.FeedTypeStatus)
            {
                RenrenSendForStatus();
            }
            else if (m_itemViewModel.RenrenFeedType == RenrenSDK.RenrenNews.FeedTypeUploadPhoto)
            {
                RenrenSendForUploadPhoto();
            }
            else if (m_itemViewModel.RenrenFeedType == RenrenSDK.RenrenNews.FeedTypeSharePhoto)
            {
                RenrenSendForShare();
            }   
        }

        private async void RenrenSendForStatus()
        {
            RenrenSDK.ResultResponse response
                = await App.RenrenAPI.StatusAPI.AddComment(m_itemViewModel.ID, m_itemViewModel.OwnerID, txtContent.Text);
            if (response.Error == RestBase.RestError.ERROR_SUCCESS && response.result == "1")
            {
                DialogHelper.ShowToastDialog("发送成功");
                Frame.GoBack();
            }
            else
            {
                DialogHelper.ShowToastDialog("发送失败，请检查网络是否稳定");
            }
        }


        private async void RenrenSendForUploadPhoto()
        {
            RenrenSDK.ResultResponse response
                = await App.RenrenAPI.PhotoAPI.AddComment(m_itemViewModel.ID, m_itemViewModel.OwnerID, txtContent.Text);
            if (response.Error == RestBase.RestError.ERROR_SUCCESS && response.result == "1")
            {
                DialogHelper.ShowToastDialog("发送成功");
                Frame.GoBack();
            }
            else
            {
                DialogHelper.ShowToastDialog("发送失败，请检查网络是否稳定");
            }
        }

        private async void RenrenSendForShare()
        {
            RenrenSDK.ResultResponse response
                = await App.RenrenAPI.ShareAPI.AddComment(m_itemViewModel.ID, m_itemViewModel.OwnerID, txtContent.Text);
            if (response.Error == RestBase.RestError.ERROR_SUCCESS && response.result == "1")
            {
                DialogHelper.ShowToastDialog("发送成功");
                Frame.GoBack();
            }
            else
            {
                DialogHelper.ShowToastDialog("发送失败，请检查网络是否稳定");
            }
        }

        private async void DoubanSend()
        {
            if (m_itemViewModel == null)
                return;

            // 豆瓣很特殊，对转发的评论实际上就是原始广播的评论
            String finalID = m_itemViewModel.ID;
            if (m_itemViewModel.ForwardItem != null)
                finalID = m_itemViewModel.ForwardItem.ID;

            DoubanSDK.BaseResponse response =
                await App.DoubanAPI.ShuoAPI.AddComment(finalID, txtContent.Text);
            if (response.Error == RestBase.RestError.ERROR_SUCCESS)
            {
                DialogHelper.ShowToastDialog("发送成功");
                Frame.GoBack();
            }
            else
            {
                DialogHelper.ShowToastDialog("发送失败，请检查网络是否稳定");
            }
        }

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            String commentText = txtContent.Text;
            if (String.IsNullOrEmpty(commentText))
            {
                DialogHelper.ShowMessageDialog("内容为空！","温馨提示神马的一点都不温馨的说~~");
                return;
            }
            else if (commentText.Length > m_maxLenth)
            {
                DialogHelper.ShowMessageDialog("内容超出最大长度！", "温馨提示神马的一点都不温馨的说~~");
                return;
            }

            if (m_type == EntryType.SinaWeibo)
            {
                SinaWeiboSend();
            }
            else if (m_type == EntryType.Renren)
            {
                RenrenSend();
            }
            else if (m_type == EntryType.Douban)
            {
                DoubanSend();
            }   
        }
    }
}
