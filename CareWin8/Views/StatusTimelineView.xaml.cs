﻿using System;
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
    public sealed partial class StatusTimelineView : CareWin8.Common.LayoutAwarePage
    {
        #region ItemHeightProperty
        public static readonly DependencyProperty ItemHeightProperty =
            DependencyProperty.Register("ItemHeight", typeof(string), typeof(StatusTimelineView), new PropertyMetadata((string)""));

        public string ItemHeight
        {
            get { return (string)GetValue(ItemHeightProperty); }
            set { SetValue(ItemHeightProperty, value); }
        }
        #endregion

        public StatusTimelineView()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
            this.DataContext = App.MainViewModel;
            TitleGrid.PointerWheelChanged += TitleGrid_PointerWheelChanged;
        }

        void TitleGrid_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
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
            int height = (int)(Window.Current.Bounds.Height - 200);
            ItemHeight = height.ToString();
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

        private void StatusGridView_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (e.ClickedItem != null)
                {
                    ItemViewModel model = e.ClickedItem as ItemViewModel;
                    if (model.Type == EntryType.Rss)
                    {
                        Frame.Navigate(typeof(RssDetailView), model);
                    }
                    else if (model.Type == EntryType.SinaWeibo ||
                    model.Type == EntryType.Renren ||
                    model.Type == EntryType.Douban)
                    {
                        Frame.Navigate(typeof(StatusDetailView), model);
                    }                    
                }
            }
            catch (System.Exception ex)
            {
            	
            }
        }
    }
}
