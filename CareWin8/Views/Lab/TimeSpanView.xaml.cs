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
    public sealed partial class TimeSpanView : CareWin8.Common.LayoutAwarePage
    {
        #region AvatarSourceProperty
        public static readonly DependencyProperty AvatarSourceProperty =
            DependencyProperty.Register("AvatarSource", typeof(string), typeof(TimeSpanView), new PropertyMetadata((string)""));

        public string AvatarSource
        {
            get { return (string)GetValue(AvatarSourceProperty); }
            set { SetValue(AvatarSourceProperty, value); }
        }
        #endregion

        #region NameProperty
        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register("Name", typeof(string), typeof(TimeSpanView), new PropertyMetadata((string)""));

        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }
        #endregion

        #region Param1Property
        public static readonly DependencyProperty Param1Property =
            DependencyProperty.Register("Param1", typeof(int), typeof(TimeSpanControl), new PropertyMetadata((int)0));

        public int Param1
        {
            get { return (int)GetValue(Param1Property); }
            set { SetValue(Param1Property, value); }
        }
        #endregion

        #region Param2Property
        public static readonly DependencyProperty Param2Property =
            DependencyProperty.Register("Param2", typeof(int), typeof(TimeSpanControl), new PropertyMetadata((int)0));

        public int Param2
        {
            get { return (int)GetValue(Param2Property); }
            set { SetValue(Param2Property, value); }
        }
        #endregion

        #region Param3Property
        public static readonly DependencyProperty Param3Property =
            DependencyProperty.Register("Param3", typeof(int), typeof(TimeSpanControl), new PropertyMetadata((int)0));

        public int Param3
        {
            get { return (int)GetValue(Param3Property); }
            set { SetValue(Param3Property, value); }
        }
        #endregion

        #region Param4Property
        public static readonly DependencyProperty Param4Property =
            DependencyProperty.Register("Param4", typeof(int), typeof(TimeSpanControl), new PropertyMetadata((int)0));

        public int Param4
        {
            get { return (int)GetValue(Param4Property); }
            set { SetValue(Param4Property, value); }
        }
        #endregion
        
        public TimeSpanView()
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
            GetData();
            ControlPanel.Children.Clear();
            ControlPanel.Children.Add(new TimeSpanControl(Param1, Param2, Param3, Param4));    
        }

        private void GetData()
        {
            foreach (ItemViewModel item in App.MainViewModel.Items)
            {
                int hour = item.TimeObject.Hour;
                if (hour >= 8 && hour < 12)
                {
                    Param1++;
                }
                else if (hour >= 12 && hour < 18)
                {
                    Param2++;
                }
                else if (hour >= 18 && hour < 24)
                {
                    Param3++;
                }
                else if (hour >= 0 && hour < 8)
                {
                    Param4++;
                }                     
            }
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

        private void test(object sender, TappedRoutedEventArgs e)
        {
            ControlPanel.Children.Clear();
            ControlPanel.Children.Add(new TimeSpanControl(10,30,20,40));
            
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {

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
            String hername = PreferenceHelper.GetPreference("SinaWeibo_FollowerNickName");
            String time = GenerateMostActiveTime();
            int percentage = GenerateMostActivePercentage();
            String award = GenerateAward();
            sentence.Append(string.Format(
                "据消息人士透露，@{0} 最活跃的时间是在{1}，此段时间中的发贴量占全部发贴的{2}%, 获得了成就【{3}】",
                 hername, time, percentage, award));            
            Dictionary<String, Object> parameters = new Dictionary<String, Object>();
            parameters.Add("Content", sentence.ToString());
            parameters.Add("Type", EntryType.SinaWeibo);
            Frame.Navigate(typeof(AddCommitView), parameters);
        }

        private int GenerateMostActivePercentage()
        {
            int all = Param1 + Param2 + Param3 + Param4;
            int big = 0;
            if (Param1 >= Param2 && Param1 >= Param3 && Param1 >= Param4)
                big = Param1;
            else if (Param2 >= Param1 && Param2 >= Param3 && Param2 >= Param4)
                big = Param2;
            else if (Param3 >= Param1 && Param3 >= Param2 && Param2 >= Param4)
                big = Param3;
            else if (Param4 >= Param1 && Param4 >= Param2 && Param4 >= Param3)
                big = Param4;

            int percentage = (int)(big * 100 / all);
            return percentage;
        }

        private String GenerateMostActiveTime()
        {
            if (Param1 >= Param2 && Param1 >= Param3 && Param1 >= Param4)
                return "上午";
            else if (Param2 >= Param1 && Param2 >= Param3 && Param2 >= Param4)
                return "下午";
            else if (Param3 >= Param1 && Param3 >= Param2 && Param2 >= Param4)
                return "晚上";
            else if (Param4 >= Param1 && Param4 >= Param2 && Param4 >= Param3)
                return "半夜";

            return "你妹的程序出BUG了啊~~~~~~~~~";
        }

        private String GenerateAward()
        {
            if (Param1 >= Param2 && Param1 >= Param3 && Param1 >= Param4)
                return "这么正常的活动规律我要是你我都不好意思出门";
            else if (Param2 >= Param1 && Param2 >= Param3 && Param2 >= Param4)
                return "睡完午觉就无所事事的家伙";
            else if (Param3 >= Param1 && Param3 >= Param2 && Param2 >= Param4)
                return "月色下的呤游者";
            else if (Param4 >= Param1 && Param4 >= Param2 && Param4 >= Param3)
                return "程序员";

            return "你妹的程序出BUG了啊~~~~~~~~~";
        }
    }
}
