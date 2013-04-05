using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.ViewManagement;
// “用户控件”项模板在 http://go.microsoft.com/fwlink/?LinkId=234236 上提供

namespace CareWin8.MyControl
{
    public sealed partial class PopControl : UserControl
    {
        Popup m_pop = null;
        int m_flyoutOffset;
        public PopControl()
        {
            this.DefaultStyleKey = typeof(PopControl);
            // 弹出层的宽度等于窗口的宽度
            this.Width = Window.Current.Bounds.Width;
            // 弹出层的高度等于窗口的高度
            this.Height = Window.Current.Bounds.Height;
            this.HorizontalContentAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch;
            this.m_pop = new Popup();
            // 将当前控件作为Popup的Child
            this.m_pop.Child = this;
            InputPane inputPane = InputPane.GetForCurrentView();
            inputPane.Showing += inputPane_Showing;
            inputPane.Hiding += inputPane_Hiding;
            this.InitializeComponent();
        }

        void inputPane_Hiding(InputPane sender, InputPaneVisibilityEventArgs args)
        {
            this.Height += m_flyoutOffset;
            m_pop.Height += m_flyoutOffset;
            //m_pop.VerticalOffset += m_flyoutOffset;
        }

        void inputPane_Showing(InputPane sender, InputPaneVisibilityEventArgs args)
        {
            m_flyoutOffset = (int)args.OccludedRect.Height;
            this.Height -= m_flyoutOffset;
            m_pop.Height -= m_flyoutOffset;
            //m_pop.VerticalOffset -= m_flyoutOffset;
        }

        public void SetCustomContent(UIElement grid)
        {
            CustomContent.Children.Add(grid);
        }

        /// <summary>
        /// 获取Popup的ChildTransitions集合
        /// </summary>
        public TransitionCollection PopTransitions
        {
            get
            {
                if (m_pop.ChildTransitions == null)
                {
                    m_pop.ChildTransitions = new TransitionCollection();
                }
                return m_pop.ChildTransitions;
            }
        }

        /// <summary>
        /// 显示弹出层
        /// </summary>
        public void ShowPop()
        {
            if (this.m_pop != null)
            {
                this.m_pop.IsOpen = true;
            }
        }
        /// <summary>
        /// 隐藏弹出层
        /// </summary>
        public void HidePop()
        {
            if (this.m_pop != null)
            {
                this.m_pop.IsOpen = false;
            }
        }

        private void Out_Tapped(object sender, TappedRoutedEventArgs e)
        {
            HidePop();
        }
    }
}
