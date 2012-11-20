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
    public sealed partial class PotentialEnemyView : CareWin8.Common.LayoutAwarePage
    {
        #region AvatarSourceProperty
        public static readonly DependencyProperty AvatarSourceProperty =
            DependencyProperty.Register("AvatarSource", typeof(string), typeof(PotentialEnemyView), new PropertyMetadata((string)""));

        public string AvatarSource
        {
            get { return (string)GetValue(AvatarSourceProperty); }
            set { SetValue(AvatarSourceProperty, value); }
        }
        #endregion

        #region NameProperty
        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register("Name", typeof(string), typeof(PotentialEnemyView), new PropertyMetadata((string)""));

        public string Name
        {
            get { return (string)GetValue(NameProperty); }
            set { SetValue(NameProperty, value); }
        }
        #endregion

        #region Name1Property
        public static readonly DependencyProperty Name1Property =
            DependencyProperty.Register("Name1", typeof(String), typeof(PotentialEnemyView), new PropertyMetadata((String)""));

        public String Name1
        {
            get { return (String)GetValue(Name1Property); }
            set { SetValue(Name1Property, value); }
        }
        #endregion

        #region Name2Property
        public static readonly DependencyProperty Name2Property =
            DependencyProperty.Register("Name2", typeof(String), typeof(PotentialEnemyView), new PropertyMetadata((String)""));

        public String Name2
        {
            get { return (String)GetValue(Name2Property); }
            set { SetValue(Name2Property, value); }
        }
        #endregion

        #region Name3Property
        public static readonly DependencyProperty Name3Property =
            DependencyProperty.Register("Name3", typeof(String), typeof(PotentialEnemyView), new PropertyMetadata((String)""));

        public String Name3
        {
            get { return (String)GetValue(Name3Property); }
            set { SetValue(Name3Property, value); }
        }
        #endregion

        #region Value1Property
        public static readonly DependencyProperty Value1Property =
            DependencyProperty.Register("Value1", typeof(int), typeof(PotentialEnemyView), new PropertyMetadata((int)0));

        public int Value1
        {
            get { return (int)GetValue(Value1Property); }
            set { SetValue(Value1Property, value); }
        }
        #endregion

        #region Value2Property
        public static readonly DependencyProperty Value2Property =
            DependencyProperty.Register("Value2", typeof(int), typeof(PotentialEnemyView), new PropertyMetadata((int)0));

        public int Value2
        {
            get { return (int)GetValue(Value2Property); }
            set { SetValue(Value2Property, value); }
        }
        #endregion

        #region Value3Property
        public static readonly DependencyProperty Value3Property =
            DependencyProperty.Register("Value3", typeof(int), typeof(PotentialEnemyView), new PropertyMetadata((int)0));

        public int Value3
        {
            get { return (int)GetValue(Value3Property); }
            set { SetValue(Value3Property, value); }
        }
        #endregion

        private string id1 = "";
        private string id2 = "";
        private string id3 = "";

        ProgressBarHelper m_progressBarHelper;
        EntryType m_type = EntryType.NotSet;
        private string m_herID;

        List<CommentMan> m_listMan;
        Dictionary<String, String> m_mapNameToID = new Dictionary<String, String>();
        Dictionary<String, int> m_mapMan = new Dictionary<String, int>();


        public PotentialEnemyView()
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
            if (m_progressBarHelper == null)
            {
                m_progressBarHelper = new ProgressBarHelper(LoadProgessBar, () => { });
            }
            m_progressBarHelper.PushTask();
            BaseFetcher fetcher = null;
            switch (m_type)
            {
                case EntryType.SinaWeibo:
                    fetcher = new SinaWeiboFetcher();
                    AvatarSource = PreferenceHelper.GetPreference("SinaWeibo_FollowerAvatar2");
                    Name = PreferenceHelper.GetPreference("SinaWeibo_FollowerNickName");
                    m_herID = PreferenceHelper.GetPreference("SinaWeibo_FollowerID");
                    break;
                case EntryType.Renren:
                    //fetcher = new RenrenFetcher();
                    AvatarSource = PreferenceHelper.GetPreference("Renren_FollowerAvatar2");
                    Name = PreferenceHelper.GetPreference("Renren_FollowerNickName");
                    m_herID = PreferenceHelper.GetPreference("Renren_FollowerID");
                    break;
                case EntryType.Douban:
                    //fetcher = new DoubanFetcher();
                    AvatarSource = PreferenceHelper.GetPreference("Douban_FollowerAvatar2");
                    Name = PreferenceHelper.GetPreference("Douban_FollowerNickName");
                    m_herID = PreferenceHelper.GetPreference("Douban_FollowerID");
                    break;
                default:
                    fetcher = SelectDefaultFetcher();
                    break;
            }
            if (fetcher == null)
            {
                m_progressBarHelper.PopTask();
                return;
            }
            fetcher.FetchCommentManList((List<CommentMan> list) =>
            {
                m_listMan = list;
                if (list == null)
                {
                    m_progressBarHelper.PopTask();
                    return;
                }
                GetData();

                ControlPanel.Children.Clear();
                ControlPanel.Children.Add(new PotentialEnemyControl(Name1, Value1, Name2, Value2, Name3, Value3));

                m_progressBarHelper.PopTask();
            }); 
        }

        private void GetData()
        {
            ConvertListToMap();
            GetTop3();
        }

        private void GetTop3()
        {
            var sortedDict =
                (from entry in m_mapMan
                 orderby entry.Value
                 descending
                 select entry.Key).Take(3);
            String[] NameTop3 = sortedDict.ToArray();
            if (NameTop3.Length == 3)
            {
                Name1 = NameTop3[0];
                Name2 = NameTop3[1];
                Name3 = NameTop3[2];

                id1 = m_mapNameToID[Name1];
                id2 = m_mapNameToID[Name2];
                id3 = m_mapNameToID[Name3];

                Value1 = m_mapMan[Name1];
                Value2 = m_mapMan[Name2];
                Value3 = m_mapMan[Name3];
            }
            else if (NameTop3.Length == 2)
            {
                Name1 = NameTop3[0];
                Name2 = NameTop3[1];

                id1 = m_mapNameToID[Name1];
                id2 = m_mapNameToID[Name2];

                Value1 = m_mapMan[Name1];
                Value2 = m_mapMan[Name2];
            }
            else if (NameTop3.Length == 1)
            {
                Name1 = NameTop3[0];

                id1 = m_mapNameToID[Name1];

                Value1 = m_mapMan[Name1];
            }
        }

        private void ConvertListToMap()
        {
            m_mapMan.Clear();
            foreach (CommentMan man in m_listMan)
            {
                if (m_mapMan.ContainsKey(man.name))
                {
                    m_mapMan[man.name]++;
                }
                else
                {
                    m_mapMan.Add(man.name, 1);
                }

                if (!m_mapNameToID.ContainsKey(man.name))
                {
                    m_mapNameToID.Add(man.name, man.id);
                }
            }
        }

        private BaseFetcher SelectDefaultFetcher()
        {
            BaseFetcher fetcher = null;
            if (!String.IsNullOrEmpty(PreferenceHelper.GetPreference("SinaWeibo_ID"))
                && !String.IsNullOrEmpty(PreferenceHelper.GetPreference("SinaWeibo_FollowerID"))
                && !String.IsNullOrEmpty(PreferenceHelper.GetPreference("SinaWeibo_Token")))
            {
                AvatarSource = PreferenceHelper.GetPreference("SinaWeibo_FollowerAvatar2");
                Name = PreferenceHelper.GetPreference("SinaWeibo_FollowerNickName");
                m_herID = PreferenceHelper.GetPreference("SinaWeibo_FollowerID");
                fetcher = new SinaWeiboFetcher();
                m_type = EntryType.SinaWeibo;
            }
            else if (!String.IsNullOrEmpty(PreferenceHelper.GetPreference("Renren_ID"))
                && !String.IsNullOrEmpty(PreferenceHelper.GetPreference("Renren_FollowerID")))
            {
                // 因为人人的avatar2 很可能是不规则的，所以这里用低清的
                AvatarSource = PreferenceHelper.GetPreference("Renren_FollowerAvatar2");
                Name = PreferenceHelper.GetPreference("Renren_FollowerNickName");
                m_herID = PreferenceHelper.GetPreference("Renren_FollowerID");
                //fetcher = new RenrenFetcher();
                m_type = EntryType.Renren;
            }
            else if (!String.IsNullOrEmpty(PreferenceHelper.GetPreference("Douban_ID"))
                && !String.IsNullOrEmpty(PreferenceHelper.GetPreference("Douban_FollowerID"))
                && !String.IsNullOrEmpty(PreferenceHelper.GetPreference("Douban_Token")))
            {
                AvatarSource = PreferenceHelper.GetPreference("Douban_FollowerAvatar2");
                Name = PreferenceHelper.GetPreference("Douban_FollowerNickName");
                m_herID = PreferenceHelper.GetPreference("Renren_FollowerID");
                //fetcher = new DoubanFetcher();
                m_type = EntryType.Douban;
            }
            return fetcher;
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
            sentence.Append(string.Format(
                    "收取了可观小的小费后，酒馆老板小声道："
                    + "看在你对@{0} 一片痴情的份上，我可以告诉你@{1} 似乎在做些小动作，而@{2} 更值得你注意，当然了，你的头号情敌非@{3} 莫属~~",
                     Name, Name3, Name2, Name1));
            Dictionary<String, Object> parameters = new Dictionary<String, Object>();
            parameters.Add("Content", sentence.ToString());
            parameters.Add("Type", EntryType.SinaWeibo);
            Frame.Navigate(typeof(AddCommitView), parameters);
        }

    }
}
