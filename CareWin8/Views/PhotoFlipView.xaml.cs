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
using System.Collections.ObjectModel;



namespace CareWin8
{
    /// <summary>
    /// 基本页，提供大多数应用程序通用的特性。
    /// </summary>
    public sealed partial class PhotoFlipView : CareWin8.Common.LayoutAwarePage
    {
        #region MaxHeightProperty
        public static readonly DependencyProperty MaxHeightProperty =
            DependencyProperty.Register("MaxHeight", typeof(string), typeof(PhotoFlipView), new PropertyMetadata((string)""));

        public string MaxHeight
        {
            get { return (string)GetValue(MaxHeightProperty); }
            set { SetValue(MaxHeightProperty, value); }
        }
        #endregion

        #region MaxWidthProperty
        public static readonly DependencyProperty MaxWidthProperty =
            DependencyProperty.Register("MaxWidth", typeof(string), typeof(PhotoFlipView), new PropertyMetadata((string)""));

        public string MaxWidth
        {
            get { return (string)GetValue(MaxWidthProperty); }
            set { SetValue(MaxWidthProperty, value); }
        }
        #endregion

        public ObservableCollection<PictureItemViewModel> PictureItems { get; private set; }
        private int m_initSelectIndex = 0;
        private bool m_bNeedInitSelectIndex = true;
        public PhotoFlipView()
        {
            PictureItems = App.MainViewModel.PictureItems;
            this.InitializeComponent();            
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            try
            {
                String index = e.Parameter as String;
                if (index != null)
                {
                    m_initSelectIndex = int.Parse(index);
                }
            }
            catch (System.Exception ex)
            {

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
            int height = (int)(Window.Current.Bounds.Height * 0.7);
            int width = (int)(Window.Current.Bounds.Width) - 800;
            MaxHeight = height.ToString();
            MaxWidth = width.ToString();
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

        private void PhotoFlipViewControl_LayoutUpdated(object sender, object e)
        {
            if (m_bNeedInitSelectIndex)
            {                
                PhotoFlipViewControl.SelectedIndex = m_initSelectIndex;
                m_bNeedInitSelectIndex = false;
            }            
        }
    }
}
