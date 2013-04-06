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
using System.ComponentModel;
using System.Threading.Tasks;
using System.Threading;

// “基本页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234237 上有介绍

namespace CareWin8
{


    /// <summary>
    /// 基本页，提供大多数应用程序通用的特性。
    /// </summary>
    public sealed partial class ChatView : CareWin8.Common.LayoutAwarePage
    {
        private String[] herSentece = { "^_^ 然后呢?", "呵呵..", "嗯嗯，这样~~" };
        public ObservableCollection<ChatItemViewModel> Items { get; private set; }

        public ChatView()
        {
            this.InitializeComponent();
            Items = new ObservableCollection<ChatItemViewModel>();
            this.Loaded += ChatView_Loaded;
            this.Unloaded += ChatView_Unloaded;
        }

        void ChatView_Unloaded(object sender, RoutedEventArgs e)
        {
            StorageHelper<ObservableCollection<ChatItemViewModel>> stHelper =
                new StorageHelper<ObservableCollection<ChatItemViewModel>>(StorageType.Local);
            stHelper.SaveASync(Items, "Global_ChatRecord");
        }

        bool m_bHaveInited = false;
        async void ChatView_Loaded(object sender, RoutedEventArgs e)
        {
            if (m_bHaveInited)
            {
                return;
            }
            m_bHaveInited = true;


            StorageHelper<ObservableCollection<ChatItemViewModel>> stHelper =
               new StorageHelper<ObservableCollection<ChatItemViewModel>>(StorageType.Local);
            try
            {
                ObservableCollection<ChatItemViewModel> result = await stHelper.LoadASync("Global_ChatRecord");
                if (result != null)
                {
                    foreach (ChatItemViewModel model in result)
                    {
                        Items.Add(model);
                    }
                    var selectedIndex = MainList.Items.Count - 1;
                    if (selectedIndex >= 0)
                    {
                        MainList.SelectedIndex = selectedIndex;
                        MainList.UpdateLayout();
                        MainList.ScrollIntoView(MainList.SelectedItem);
                    }                    
                }
            }
            catch (System.Exception ex)
            {
                Items.Clear();
                ChatItemViewModel item1 = new ChatItemViewModel();
                item1.Icon = MiscTool.GetHerIconUrl();
                item1.Title = MiscTool.GetHerName();
                item1.Text = "^_^";
                item1.Type = "Her";
                Items.Add(item1);
            }

            if (Items.Count == 0)
            {
                ChatItemViewModel item1 = new ChatItemViewModel();
                item1.Icon = MiscTool.GetHerIconUrl();
                item1.Title = MiscTool.GetHerName();
                item1.Text = "^_^";
                item1.Type = "Her";
                Items.Add(item1);
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

        private void Submit_Click(object sender, RoutedEventArgs e)
        {
            ChatItemViewModel item1 = new ChatItemViewModel();
            item1.Icon = MiscTool.GetMyIconUrl();
            item1.Title = MiscTool.GetMyName();
            item1.Text = txtInput.Text;
            item1.Type = "Me";
            Items.Add(item1);
            txtInput.Text = "";   

            ChatItemViewModel item2 = new ChatItemViewModel();         
            item2.Icon = MiscTool.GetHerIconUrl();
            item2.Title = MiscTool.GetHerName();            
            item2.Type = "Her";
            Random random = new Random();
            int index = random.Next(herSentece.Length);
            item2.Text = herSentece[index];
            Items.Add(item2);


            var selectedIndex = MainList.Items.Count - 1;
            if (selectedIndex < 0)
                return;
            MainList.SelectedIndex = selectedIndex;
            MainList.UpdateLayout();
            MainList.ScrollIntoView(MainList.SelectedItem);
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            if (Items.Count != 0)
            {
                Items.Clear();
            }


            StorageHelper<ObservableCollection<ChatItemViewModel>> stHelper =
               new StorageHelper<ObservableCollection<ChatItemViewModel>>(StorageType.Local);
            stHelper.SaveASync(Items, "Global_ChatRecord");
        }
    }


   


}
