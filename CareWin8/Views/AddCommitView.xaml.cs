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

namespace CareWin8{
    /// <summary>
    /// 基本页，提供大多数应用程序通用的特性。
    /// </summary>
    public sealed partial class AddCommitView : CareWin8.Common.LayoutAwarePage
    {
        #region LogoSourceProperty
        public static readonly DependencyProperty LogoSourceProperty =
            DependencyProperty.Register("LogoSource", typeof(string), typeof(AddCommitView), new PropertyMetadata((string)""));

        public string LogoSource
        {
            get { return (string)GetValue(LogoSourceProperty); }
            set { SetValue(LogoSourceProperty, value); }
        }
        #endregion

        #region WordCountProperty
        public static readonly DependencyProperty WordCountProperty =
            DependencyProperty.Register("WordCount", typeof(string), typeof(AddCommitView), new PropertyMetadata((string)""));

        public string WordCount
        {
            get { return (string)GetValue(WordCountProperty); }
            set { SetValue(WordCountProperty, value); }
        }
        #endregion

        #region WordMaxLength
        public static readonly DependencyProperty WordMaxLengthProperty =
            DependencyProperty.Register("WordMaxLength", typeof(string), typeof(AddCommitView), new PropertyMetadata((string)""));

        public string WordMaxLength
        {
            get { return (string)GetValue(WordMaxLengthProperty); }
            set { SetValue(WordMaxLengthProperty, value); }
        }
        #endregion

        EntryType? m_type = EntryType.NotSet;
        String m_content;
        int m_maxLenth;

        public AddCommitView()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            try
            {
                Dictionary<String, Object> parameters = e.Parameter as Dictionary<String, Object>;
                if(parameters.ContainsKey("Type"))
                    m_type = parameters["Type"] as EntryType?;
                if (parameters.ContainsKey("Content"))
                    m_content = parameters["Content"] as String;
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
            else if (m_type == EntryType.Renren)
            {
                m_maxLenth = 280;
                WordMaxLength = "280";
                WordCount = "280";
                LogoSource = "/Images/Logo/renren_logo.png";
            }
            else if (m_type == EntryType.Douban)
            {
                m_maxLenth = 140;
                WordMaxLength = "140";
                WordCount = "140";
                LogoSource = "/Images/Logo/douban_logo.png";
            }
            if(!String.IsNullOrEmpty(m_content))
            {
                txtContent.Text = m_content;
                WordCount = (m_maxLenth - txtContent.Text.Length).ToString();
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

        private async void SinaWeiboSend()
        {
            SinaWeiboSDK.AddStatusResponse response = 
                await App.SinaWeiboAPI.StatuesAPI.AddStatus(txtContent.Text);
            if (response.Error == RestBase.RestError.ERROR_SUCCESS)
            {
                DialogHelper.ShowToastDialog("发送成功");
                Frame.GoBack();
            }
            else
            {
                DialogHelper.ShowToastDialog("发送失败");
            }
        }

        private async void RenrenSend()
        {
            RenrenSDK.ResultResponse response =
                await App.RenrenAPI.StatusAPI.AddStatus(txtContent.Text);
            if (response.Error == RestBase.RestError.ERROR_SUCCESS && response.result == "1")
            {
                DialogHelper.ShowToastDialog("发送成功");
                Frame.GoBack();
            }
            else
            {
                DialogHelper.ShowToastDialog("发送失败");
            }
        }
        private async void DoubanSend()
        {
            DoubanSDK.BaseResponse response =
               await App.DoubanAPI.ShuoAPI.AddStatus(txtContent.Text);
            if (response.Error == RestBase.RestError.ERROR_SUCCESS)
            {
                DialogHelper.ShowToastDialog("发送成功");
                Frame.GoBack();
            }
            else
            {
                DialogHelper.ShowToastDialog("发送失败");
            }
        }
    }
}
