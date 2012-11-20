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
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Net;
using RestBase;

// “基本页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234237 上有介绍

namespace CareWin8
{
    /// <summary>
    /// 基本页，提供大多数应用程序通用的特性。
    /// </summary>
    public sealed partial class RssSearchSiteView : CareWin8.Common.LayoutAwarePage
    {
        public ObservableCollection<RssSiteViewModel> SiteItems { get; private set; }
        ProgressBarHelper m_progressBarHelper;

        public RssSearchSiteView()
        {
            SiteItems = new ObservableCollection<RssSiteViewModel>();
            this.InitializeComponent();
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
            String key = txtSearch.Text;
            if (String.IsNullOrEmpty(key))
            {
                DialogHelper.ShowMessageDialog("输入为空是想闹哪样啊~");
                return;
            }
            SearchByKey(key);
        }

        private async void SearchByKey(String key)
        {
            if (m_progressBarHelper == null)
                m_progressBarHelper = new ProgressBarHelper(LoadProgessBar, () => { });
            m_progressBarHelper.PushTask();

            RestClient client = new RestClient();
            RestRequest request = new RestRequest();
            client.Authority = "http://www.search4rss.com/search.php";
            request.Method = Method.Get;
            request.AddParameter("lang", "en");
            request.AddParameter("q", key);

            RestResponse response = await client.BeginRequest(request);
            if (response.Error == RestError.ERROR_SUCCESS && response.Content != null)
            {
                HandleHtml(response.Content);
            }
            else
            {
                DialogHelper.ShowMessageDialog("网络无法连接，请检查当前网络状态。");
                m_progressBarHelper.PopTask();
            }
        }

        private void HandleHtml(string html)
        {
            try
            {
                Regex regResult = new Regex("<div id=\'results\'>(?<1>.*?)preview");
                SiteItems.Clear();
                foreach (Match match in regResult.Matches(html))
                {
                    HandleResult(match.Groups["1"].Value);
                }
                m_progressBarHelper.PopTask();
            }
            catch (System.Exception ex)
            {
                DialogHelper.ShowMessageDialog("RSS搜索数据分析中出现未知错误", "悲剧鸟~");
                m_progressBarHelper.PopTask();
            }        
        }

        private void HandleResult(string result)
        {
            Regex regTitle = new Regex("<div>.*?<a.*?href=\'(?<OriginalURL>.*?)\'.*?<b>(?<Title>.*?)</b>.*?<a href=\'(?<FeedURL>.*?)\'");

            RssSiteViewModel item = new RssSiteViewModel();
            item.Title = regTitle.Match(result).Groups["Title"].Value;
            item.RssPath = regTitle.Match(result).Groups["FeedURL"].Value;
            item.OriginPath = regTitle.Match(result).Groups["OriginalURL"].Value;

            SiteItems.Add(item);
        }


        private void SiteGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = SiteGridView.SelectedIndex;
            RssSiteViewModel item = SiteItems[index];
            PreferenceHelper.SetPreference("RSS_FollowerSite", item.Title);
            PreferenceHelper.SetPreference("RSS_FollowerPath", item.RssPath);
            PreferenceHelper.SetPreference("RSS_FollowerOrigin", item.OriginPath);
            PreferenceHelper.SavePreference();            
            App.MainViewModel.IsChanged = true;
            Frame.GoBack();
        }
    }
}
