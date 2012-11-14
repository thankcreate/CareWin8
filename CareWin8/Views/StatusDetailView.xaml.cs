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
// “基本页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234237 上有介绍

namespace CareWin8
{
    /// <summary>
    /// 基本页，提供大多数应用程序通用的特性。
    /// </summary>
    public sealed partial class StatusDetailView : CareWin8.Common.LayoutAwarePage
    {
        ItemViewModel m_itemViewModel;
        
        public StatusDetailView()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            try
            {
                m_itemViewModel = e.Parameter as ItemViewModel;
            }
            catch (System.Exception ex)
            {

            }
            InitialLoad();
        }

        // 加载界面
        private void InitialLoad()
        {
            if(m_itemViewModel == null)
                return;
            this.DefaultViewModel["Item"] = m_itemViewModel;
            this.DefaultViewModel["CommentItems"] = new ObservableCollection<CommentViewModel>();
            RefreshComments();
        }

        private void RefreshComments()
        {
            if (m_itemViewModel == null)
                return;
            EntryType type = m_itemViewModel.Type;
            if (type == EntryType.SinaWeibo)
            {
                RefreshCommentsForSinaWeibo();
            }
            else if (type == EntryType.Renren)
            {
                RefreshCommentsForRenren();
            }
            else if (type == EntryType.Douban)
            {
                RefreshCommentsForDouban();
            }
        }

        private async void RefreshCommentsForSinaWeibo()
        {
            ObservableCollection<CommentViewModel> collection;
            try
            {
                collection = this.DefaultViewModel["CommentItems"] as ObservableCollection<CommentViewModel>;
            }
            catch (System.Exception ex)
            {
                return;
            }
            if (collection == null)
                return;

            SinaWeiboSDK.GetStatusCommentsResponse response = await App.SinaWeiboAPI.CommentAPI.GetStatusComments(m_itemViewModel.ID, 50);
            if (response.Error == RestBase.RestError.ERROR_SUCCESS && response.comments != null)
            {
                foreach (SinaWeiboSDK.Comment rawComment in response.comments)
                {
                    CommentViewModel model = SinaWeiboConverter.ConvertCommentToCommon(rawComment);
                    if (model != null)
                    {
                        collection.Add(model);
                    }
                }
            }
        }
        private void RefreshCommentsForRenren()
        {

        }
        private void RefreshCommentsForDouban()
        {

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
    }
}
