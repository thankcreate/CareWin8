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
using RestBase;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;
using System.Collections.ObjectModel;
// “基本页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234237 上有介绍

namespace CareWin8
{
    /// <summary>
    /// 基本页，提供大多数应用程序通用的特性。
    /// </summary>
    public sealed partial class BlessView : CareWin8.Common.LayoutAwarePage
    {
        public static String BLESS_GET_URL = "http://42.96.147.167:8181/admin/get_blessing";
        public static String BLESS_POST_URL = "http://42.96.147.167:8181/admin/put_blessing";

        public ObservableCollection<BlessItem> Items { get; private set; }

        public BlessView()
        {
            Items = new ObservableCollection<BlessItem>();
            this.InitializeComponent();
            this.Loaded += BlessView_Loaded;
        }

        async void BlessView_Loaded(object sender, RoutedEventArgs e)
        {
            String myName = MiscTool.GetMyName();
            if (String.IsNullOrEmpty(myName))
            {
                myName = "匿名";
            }
            txtName.Text = myName;

            refreshItems();
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

        private async void Submit_Click(object sender, RoutedEventArgs e)
        {
            String input = txtInput.Text.Trim();
            if (String.IsNullOrEmpty(input))
            {
                DialogHelper.ShowMessageDialog(">_< 啥都不写就想提交什么的，也太没诚意啦~");
                return;
            }

            String name = txtName.Text;
            if (String.IsNullOrEmpty(name))
            {
                name = "匿名";
            }

            MainProgessBar.ShowPaused = false;

            RestClient client = new RestClient();
            client.Authority = BLESS_POST_URL;
            RestRequest req = new RestRequest();
            req.AddParameter("name", name);
            req.AddParameter("content", input);
            req.Method = Method.Post;
            RestResponse response = await client.BeginRequest(req);
            if (response.Error == RestError.ERROR_SUCCESS)
            {
                DialogHelper.ShowToastDialog("发送成功");
                refreshItems();
            }
            else
            {
                DialogHelper.ShowToastDialog("发送失败，请检查网络");
                MainProgessBar.ShowPaused = true;
            }
        }

        private async void refreshItems()
        {
            MainProgessBar.ShowPaused = false;
            RestClient client = new RestClient();
            client.Authority = BLESS_GET_URL;
            RestRequest req = new RestRequest();
            req.AddParameter("count", "30");
            req.AddParameter("needPassed", 0);
            req.AddParameter("currentTime", DateTime.Now.ToString());
            RestResponse response = await client.BeginRequest(req);
                        
            if (response.Error == RestError.ERROR_SUCCESS)
            {                
                DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(ObservableCollection<BlessItem>));
                MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(response.Content));
                ObservableCollection<BlessItem> result = (ObservableCollection<BlessItem>)ser.ReadObject(ms);
                if (result != null)
                {
                    Items.Clear();
                    int i = 0;
                    foreach (BlessItem item in result)
                    {
                        item.index = i++;
                        Items.Add(item);
                    }
                }
                MainProgessBar.ShowPaused = true;
            }
            else
            {
                DialogHelper.ShowToastDialog("获取心语墙失败，请检查网络");
                MainProgessBar.ShowPaused = true;
            }
           
        }
    }
}
