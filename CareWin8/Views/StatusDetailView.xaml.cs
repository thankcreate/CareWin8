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
        ProgressBarHelper m_progressBarHelper;

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

            if (m_progressBarHelper == null)
            {
                m_progressBarHelper = new ProgressBarHelper(CommentProgessBar, () => { });
            }
            m_progressBarHelper.PushTask();
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
                collection.Clear();
            }
            catch (System.Exception ex)
            {
                m_progressBarHelper.PopTask();
                return;
            }
            if (collection == null)
            {
                m_progressBarHelper.PopTask();
                return;
            }
                

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
            m_progressBarHelper.PopTask();
        }

        private void RefreshCommentsForRenren()
        {
            ObservableCollection<CommentViewModel> collection;
            try
            {
                collection = this.DefaultViewModel["CommentItems"] as ObservableCollection<CommentViewModel>;
                collection.Clear();
            }
            catch (System.Exception ex)
            {
                m_progressBarHelper.PopTask();
                return;
            }

            if (collection == null)
            {
                m_progressBarHelper.PopTask();
                return;
            }

            try
            {
                if (m_itemViewModel.RenrenFeedType == RenrenSDK.RenrenNews.FeedTypeStatus)
                {
                    RefreshCommentsForRenrenStatus();
                }
                else if (m_itemViewModel.RenrenFeedType == RenrenSDK.RenrenNews.FeedTypeUploadPhoto)
                {
                    RefreshCommentsForRenrenUploadPhoto();
                }
                else if (m_itemViewModel.RenrenFeedType == RenrenSDK.RenrenNews.FeedTypeSharePhoto)
                {
                    RefreshCommentsForRenrenShare();
                }   
            }
            catch (System.Exception ex)
            {
            	
            }
            finally
            {
                m_progressBarHelper.PopTask();
            }
            
           
        }

        private async void RefreshCommentsForRenrenStatus()
        {
            ObservableCollection<CommentViewModel> collection 
                = this.DefaultViewModel["CommentItems"] as ObservableCollection<CommentViewModel>;

            RenrenSDK.GetStatusCommentsResponse response =
                await App.RenrenAPI.StatusAPI.GetComments(m_itemViewModel.ID, m_itemViewModel.OwnerID);
            if (response.Error == RestBase.RestError.ERROR_SUCCESS && response.ListComments != null)
            {
                foreach (RenrenSDK.StatusComment comment in response.ListComments)
                {
                    CommentViewModel model = RenrenConverter.ConvertStatusCommentToCommon(comment);
                    if (model != null)
                    {
                        collection.Add(model);
                    }
                }
            }
        }

        private async void RefreshCommentsForRenrenUploadPhoto()
        {
            ObservableCollection<CommentViewModel> collection
                = this.DefaultViewModel["CommentItems"] as ObservableCollection<CommentViewModel>;

            RenrenSDK.GetPhotoCommentsResponse response =
                await App.RenrenAPI.PhotoAPI.GetComments(m_itemViewModel.ID, m_itemViewModel.OwnerID);
            if (response.Error == RestBase.RestError.ERROR_SUCCESS && response.ListComments != null)
            {
                foreach (RenrenSDK.PhotoComment comment in response.ListComments)
                {
                    CommentViewModel model = RenrenConverter.ConvertPhotoCommentToCommon(comment);
                    if (model != null)
                    {
                        collection.Add(model);
                    }
                }
            }
        }

        private async void RefreshCommentsForRenrenShare()
        {
            ObservableCollection<CommentViewModel> collection
                = this.DefaultViewModel["CommentItems"] as ObservableCollection<CommentViewModel>;

            RenrenSDK.GetShareCommentsResponse response =
                await App.RenrenAPI.ShareAPI.GetComments(m_itemViewModel.ID, m_itemViewModel.OwnerID);
            if (response.Error == RestBase.RestError.ERROR_SUCCESS && response.comments != null)
            {
                // 之所以建了一个临时list，是因为豆瓣的API不支持降序的评论列表
                List<CommentViewModel> sortList = new List<CommentViewModel>();
                foreach (RenrenSDK.ShareComment comment in response.comments)
                {
                    CommentViewModel model = RenrenConverter.ConvertShareCommentToCommon(comment);
                    if (model != null)
                    {
                        sortList.Add(model);
                    }
                }
                // DateTimeOffset是不可能为空的，所以这里不用担心
                var sorted = from m in sortList orderby m.TimeObject descending select m;
                if (sorted != null)
                {
                    foreach (CommentViewModel model in sorted)
                    {
                        collection.Add(model);
                    }
                }
            }
        }

        private async void RefreshCommentsForDouban()
        {
            ObservableCollection<CommentViewModel> collection;
            try
            {
                collection = this.DefaultViewModel["CommentItems"] as ObservableCollection<CommentViewModel>;
                collection.Clear();
            }
            catch (System.Exception ex)
            {
                m_progressBarHelper.PopTask();
                return;
            }
            if (collection == null)
            {
                m_progressBarHelper.PopTask();
                return;
            }


            // 豆瓣很特殊，对转发的评论实际上就是原始广播的评论
            String finalID = m_itemViewModel.ID;
            if(m_itemViewModel.ForwardItem != null)
                finalID = m_itemViewModel.ForwardItem.ID;

            DoubanSDK.GetCommentsResponse response = await App.DoubanAPI.ShuoAPI.GetComments(finalID);
            if (response.Error == RestBase.RestError.ERROR_SUCCESS && response.ListComments != null)
            {
                // 之所以建了一个临时list，是因为豆瓣的API不支持降序的评论列表
                List<CommentViewModel> sortList = new List<CommentViewModel>();
                foreach (DoubanSDK.Comment rawComment in response.ListComments)
                {
                    CommentViewModel model = DoubanConverter.ConvertCommentToCommon(rawComment);
                    if (model != null)
                    {
                        sortList.Add(model);
                    }
                }
                // DateTimeOffset是不可能为空的，所以这里不用担心
                var sorted = from m in sortList orderby m.TimeObject descending select m;
                if (sorted != null)
                {
                    foreach (CommentViewModel model in sorted)
                    {
                        collection.Add(model);
                    }
                }
            }
            m_progressBarHelper.PopTask();
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

        private void RefreshComment_Tapped(object sender, TappedRoutedEventArgs e)
        {
            RefreshComments();
        }

        private void AddComment_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Dictionary<String, Object> parameters = new Dictionary<String, Object>();
            parameters.Add("ItemViewModel", m_itemViewModel);
            Frame.Navigate(typeof(AddCommentView), parameters);
        }


        private void ForwardImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FrameworkElement image = sender as FrameworkElement;
            if (image == null)
                return;
            ItemViewModel model = image.DataContext as ItemViewModel;
            if (model == null)
                return;
            if (model.ForwardItem == null)
                return;
            String fullURL = model.ForwardItem.FullImageURL;

            MyControl.FullImageControl control = new MyControl.FullImageControl(fullURL);
            control.ShowPop();
            e.Handled = true;
        }

        private void ContentImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FrameworkElement image = sender as FrameworkElement;
            if (image == null)
                return;
            ItemViewModel model = image.DataContext as ItemViewModel;
            if (model == null)
                return;
            String fullURL = model.FullImageURL;
            MyControl.FullImageControl control = new MyControl.FullImageControl(fullURL);
            control.ShowPop();
            e.Handled = true;
        }

        private void CommentToComment_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FrameworkElement image = sender as FrameworkElement;
            if (image == null)
                return;
            CommentViewModel model = image.DataContext as CommentViewModel;
            if (model == null)
                return;
            Dictionary<String, Object> parameters = new Dictionary<String, Object>();
            parameters.Add("ItemViewModel", m_itemViewModel);
            parameters.Add("CommentViewModel", model);
            Frame.Navigate(typeof(AddCommentView), parameters);
        }
    }
}
