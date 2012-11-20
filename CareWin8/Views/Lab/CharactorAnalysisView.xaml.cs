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
    public sealed partial class CharactorAnalysisView : CareWin8.Common.LayoutAwarePage
    {
        #region AvatarSourceProperty
        public static readonly DependencyProperty AvatarSourceProperty =
            DependencyProperty.Register("AvatarSource", typeof(string), typeof(CharactorAnalysisView), new PropertyMetadata((string)""));

        public string AvatarSource
        {
            get { return (string)GetValue(AvatarSourceProperty); }
            set { SetValue(AvatarSourceProperty, value); }
        }
        #endregion

        #region NameProperty
        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register("Name", typeof(string), typeof(CharactorAnalysisView), new PropertyMetadata((string)""));

        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }
        #endregion

        #region Param1Property
        public static readonly DependencyProperty Param1Property =
            DependencyProperty.Register("Param1", typeof(int), typeof(CharactorAnalysisView), new PropertyMetadata((int)0));

        public int Param1
        {
            get { return (int)GetValue(Param1Property); }
            set { SetValue(Param1Property, value); }
        }
        #endregion

        #region Param2Property
        public static readonly DependencyProperty Param2Property =
            DependencyProperty.Register("Param2", typeof(int), typeof(CharactorAnalysisView), new PropertyMetadata((int)0));

        public int Param2
        {
            get { return (int)GetValue(Param2Property); }
            set { SetValue(Param2Property, value); }
        }
        #endregion

        #region Param3Property
        public static readonly DependencyProperty Param3Property =
            DependencyProperty.Register("Param3", typeof(int), typeof(CharactorAnalysisView), new PropertyMetadata((int)0));

        public int Param3
        {
            get { return (int)GetValue(Param3Property); }
            set { SetValue(Param3Property, value); }
        }
        #endregion

        #region Param4Property
        public static readonly DependencyProperty Param4Property =
            DependencyProperty.Register("Param4", typeof(int), typeof(CharactorAnalysisView), new PropertyMetadata((int)0));

        public int Param4
        {
            get { return (int)GetValue(Param4Property); }
            set { SetValue(Param4Property, value); }
        }
        #endregion

        #region Param5Property
        public static readonly DependencyProperty Param5Property =
            DependencyProperty.Register("Param5", typeof(int), typeof(CharactorAnalysisView), new PropertyMetadata((int)0));

        public int Param5
        {
            get { return (int)GetValue(Param5Property); }
            set { SetValue(Param5Property, value); }
        }
        #endregion


        string m_myName = "";
        string m_herName = "";
        string m_award = "";

        public CharactorAnalysisView()
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
            Name = MiscTool.GetHerName();
            AvatarSource = MiscTool.GetHerIconUrl();
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
            bool bReturn = GetData();
            if (bReturn)
            {
                ControlPanel.Children.Clear();
                ControlPanel.Children.Add(new CharactorAnalysisControl(Param1, Param2, Param3, Param4, Param5));
            }

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
            Analysis();
            return true;
        }

        private void Analysis()
        {
            uint herN = 0;
            char[] herArray = m_herName.ToCharArray();
            foreach (char c in herArray)
            {
                uint n = (uint)c;
                herN += n;
            }
            Param1 = (int)(herN * 575 % 50 + 50);
            Param2 = (int)(herN * herN % 50 + 50);
            Param3 = (int)(herN * 250 % 50 + 50);
            Param4 = (int)(herN * 337 % 50 + 50);
            Param5 = (int)(herN * 702 % 50 + 50);
            // ThankCreate获得了成就“硬编码大师”
            if (Param1 >= Param2 && Param1 >= Param3 && Param1 >= Param4 && Param1 >= Param5)
                m_award = "极品萝莉";
            else if (Param2 >= Param1 && Param2 >= Param3 && Param2 >= Param4 && Param2 >= Param5)
                m_award = "盖世女王";
            else if (Param3 >= Param1 && Param3 >= Param2 && Param3 >= Param4 && Param3 >= Param5)
                m_award = "超萌天然呆";
            else if (Param4 >= Param1 && Param4 >= Param2 && Param4 >= Param3 && Param4 >= Param5)
                m_award = "吃货去死去死";
            else if (Param5 >= Param1 && Param5 >= Param2 && Param5 >= Param3 && Param5 >= Param4)
                m_award = "没有你们这帮伪娘世界早就清静了";
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
                    "据新一轮民调显示，@{0} 的萝莉属性为{1}，女王属性为{2}，天然呆属性为{3}，吃货属性为{4}，伪娘属性为{5}，获得了成就【{6}】",
                     herName, Param1, Param2, Param3, Param4, Param5, m_award));
            Dictionary<String, Object> parameters = new Dictionary<String, Object>();
            parameters.Add("Content", sentence.ToString());
            parameters.Add("Type", EntryType.SinaWeibo);
            Frame.Navigate(typeof(AddCommitView), parameters);
        }

    }
}
