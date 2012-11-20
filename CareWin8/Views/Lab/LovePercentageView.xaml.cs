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
using System.Text;

// “基本页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234237 上有介绍

namespace CareWin8
{
    /// <summary>
    /// 基本页，提供大多数应用程序通用的特性。
    /// </summary>
    public sealed partial class LovePercentageView : CareWin8.Common.LayoutAwarePage
    {
        #region HerAvatarSourceProperty
        public static readonly DependencyProperty HerAvatarSourceProperty =
            DependencyProperty.Register("HerAvatarSource", typeof(string), typeof(LovePercentageView), new PropertyMetadata((string)""));

        public string HerAvatarSource
        {
            get { return (string)GetValue(HerAvatarSourceProperty); }
            set { SetValue(HerAvatarSourceProperty, value); }
        }
        #endregion

        #region HerNameProperty
        public static readonly DependencyProperty HerNameProperty =
            DependencyProperty.Register("HerName", typeof(string), typeof(LovePercentageView), new PropertyMetadata((string)""));

        public string HerName
        {
            get { return (string)GetValue(HerNameProperty); }
            set { SetValue(HerNameProperty, value); }
        }
        #endregion

        #region MyAvatarSourceProperty
        public static readonly DependencyProperty MyAvatarSourceProperty =
            DependencyProperty.Register("MyAvatarSource", typeof(string), typeof(LovePercentageView), new PropertyMetadata((string)""));

        public string MyAvatarSource
        {
            get { return (string)GetValue(MyAvatarSourceProperty); }
            set { SetValue(MyAvatarSourceProperty, value); }
        }
        #endregion

        #region MyNameProperty
        public static readonly DependencyProperty MyNameProperty =
            DependencyProperty.Register("MyName", typeof(string), typeof(LovePercentageView), new PropertyMetadata((string)""));

        public string MyName
        {
            get { return (string)GetValue(MyNameProperty); }
            set { SetValue(MyNameProperty, value); }
        }
        #endregion

        #region Param1Property
        public static readonly DependencyProperty Param1Property =
            DependencyProperty.Register("Param1", typeof(int), typeof(LovePercentageView), new PropertyMetadata((int)0));

        public int Param1
        {
            get { return (int)GetValue(Param1Property); }
            set { SetValue(Param1Property, value); }
        }
        #endregion

        int m_percentage = 50;
        string m_myName = "";
        string m_herName = "";

        public LovePercentageView()
        {
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
            MyName = MiscTool.GetMyName();
            HerName = MiscTool.GetHerName();
            HerAvatarSource = MiscTool.GetHerIconUrl();
            MyAvatarSource = MiscTool.GetMyIconUrl();
            Refresh();  
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

        private void Refresh()
        {
            bool result = GetData();
            if (!result)
                return;
            Param1 = m_percentage;
            ControlPanel.Children.Clear();
            ControlPanel.Children.Add(new LovePercentageControl(m_percentage));
        }

        private bool GetData()
        {
            m_myName = MiscTool.GetMyName();
            if (String.IsNullOrEmpty(m_myName))
            {
                DialogHelper.ShowMessageDialog("请至少先登陆一个帐户");
                return false;
            }
            m_herName = MiscTool.GetHerName();
            if (String.IsNullOrEmpty(m_herName))
            {
                DialogHelper.ShowMessageDialog("请至少先关注她/他的一个帐户");
                return false;
            }
            m_percentage = AnalysisLovePercentage();
            return true;
        }

        private int AnalysisLovePercentage()
        {
            char[] myArray = m_myName.ToCharArray();
            int myN = 0;
            foreach (char c in myArray)
            {
                int n = (int)c;
                myN += n;
            }
            int herN = 0;
            char[] herArray = m_herName.ToCharArray();
            foreach (char c in herArray)
            {
                int n = (int)c;
                herN += n;
            }
            int result = (myN + herN) * 575 % 49 + 50;
            return result;
        }

        private void Share_Tapped(object sender, TappedRoutedEventArgs e)
        {
            PopControl pc = new PopControl();
            SelectPublicSourceControl control = new SelectPublicSourceControl(pc);
            control.ChooseAction = ChooseActionCallback;
            pc.SetCustomContent(control);
            pc.ShowPop();
        }

        private void ChooseActionCallback(EntryType type)
        {
            if (type == EntryType.SinaWeibo)
            {
                Share_SinaWeibo();
            }
        }

        private void Share_SinaWeibo()
        {
            StringBuilder sentence = new StringBuilder();
            String myName = PreferenceHelper.GetPreference("SinaWeibo_NickName");
            String herName = PreferenceHelper.GetPreference("SinaWeibo_FollowerNickName");
            sentence.Append(string.Format(
                   "经某不靠谱的分析仪测算，@{0} 与@{1} 的姻缘指数达到惊人的{2}。去死去死团众，不管你们信不信，我反正不信了",
                    myName, herName, m_percentage));
            Dictionary<String, Object> parameters = new Dictionary<String, Object>();
            parameters.Add("Content", sentence.ToString());
            parameters.Add("Type", EntryType.SinaWeibo);
            Frame.Navigate(typeof(AddCommitView), parameters);
        }
    }
}
