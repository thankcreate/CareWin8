using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
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
using SinaWeiboSDK;
using RenrenSDK;
using DoubanSDK;
// “空白应用程序”模板在 http://go.microsoft.com/fwlink/?LinkId=234227 上有介绍

namespace CareWin8
{
    /// <summary>
    /// 提供特定于应用程序的行为，以补充默认的应用程序类。
    /// </summary>
    sealed partial class App : Application
    {        
        public static SinaWeiboAPI SinaWeiboAPI = new SinaWeiboAPI();
        public static RenrenAPI RenrenAPI = new RenrenAPI();
        public static DoubanAPI DoubanAPI = new DoubanAPI();        

        public static MainViewModel MainViewModel = new MainViewModel();
        /// <summary>
        /// 初始化单一实例应用程序对象。这是执行的创作代码的第一行，
        /// 逻辑上等同于 main() 或 WinMain()。
        /// </summary>
        public App()
        {            
            this.InitializeComponent();
            this.Suspending += OnSuspending;            
            Init();            
        }

        public static Windows.Storage.Streams.IRandomAccessStreamWithContentType MainPageBackgroundImageStream;

        private async void Init()
        {
            // 加载各平台参数
            SinaWeiboInit();
            RenrenInit();
            DoubanInit();

            // 之所以这里要初始化背景，是因为背景图有1M，而Metro应用在加载这个
            // 背景图时非常慢，会出现明显的黑色背景闪烁，所以要提前加载
            BackgroundInit();

            // 加载本地缓存的新鲜事
            LocalCacheInit();
        }

        private async void LocalCacheInit()
        {
            // 拿头条新鲜事的缓存
            StorageHelper<ObservableCollection<ItemViewModel>> stHelper =
                new StorageHelper<ObservableCollection<ItemViewModel>>(StorageType.Local);
            try
            {
                ObservableCollection<ItemViewModel> result = await stHelper.LoadASync("TopItems");
                if (result != null)
                    MainViewModel.TopItems = result;
            }
            catch (System.Exception ex)
            {
                MainViewModel.TopItems = new ObservableCollection<ItemViewModel>();
            }

            // 拿图片列表缓存
            StorageHelper<ObservableCollection<PictureItemViewModel>> stPicHelper =
                 new StorageHelper<ObservableCollection<PictureItemViewModel>>(StorageType.Local);
            try
            {
                ObservableCollection<PictureItemViewModel> result = await stPicHelper.LoadASync("PicItems");
                if (result != null)
                    MainViewModel.PictureItems = result;
            }
            catch (System.Exception ex)
            {
                MainViewModel.PictureItems = new ObservableCollection<PictureItemViewModel>();
            }
        }

        private async void BackgroundInit()
        {
            // 这是之所以这么纠结，只初始化了一个stream，是因为这里OpenReadAsync只能做异步操作
            // 异步的下一行是在另一个线程，而在App.cs里还不能通过Windows.UI.Core.CoreWindow.GetForCurrentThread().Dispatcher;
            // 来拿到UI线程的Dispatcher
            // 目前的UI线程Dispatcher是直到MainPage构造了之后才有的
            // MainPage构造时拿到这个stream，然后用这个stream去构造他自己的Image
            Uri imageUri = new Uri(CareConstDefine.MainPageBackgroundURI);                  
            var rass = Windows.Storage.Streams.RandomAccessStreamReference.CreateFromUri(imageUri);
            MainPageBackgroundImageStream = await rass.OpenReadAsync();
        } 

        private void SinaWeiboInit()
        {
            SinaWeiboSDK.SinaWeiboSDKData.AppKey = "3467990428";
            SinaWeiboSDK.SinaWeiboSDKData.AppSecret = "3d475659a06cb313dbdd43eb92eb6d47";
            SinaWeiboSDK.SinaWeiboSDKData.RedirectUri = "http://api.weibo.com/oauth2/default.html";
        }

        private void RenrenInit()
        {
            RenrenSDK.RenrenSDKData.ID = "219563";
            RenrenSDK.RenrenSDKData.AppKey = "19a4cb151cb7400bba75504317f65b4f";
            RenrenSDK.RenrenSDKData.AppSecret = "df339e16fd074d349b58294b19d175bf";
            RenrenSDK.RenrenSDKData.RedirectUri = "http://graph.renren.com/oauth/login_success.html";
            List<String> listScope = new List<String>{ 
                "publish_feed",
                "publish_blog", 
                "publish_share",
                "read_user_album", 
                "read_user_status",
                "read_user_photo",
                "read_user_comment",
                "read_user_status",
                "publish_comment",
                "read_user_share",                
                "create_album", 
                "status_update",
                "photo_upload" };
            RenrenSDK.RenrenSDKData.Scope = String.Join("+", listScope.ToArray());
        }

        private void DoubanInit()
        {
            DoubanSDK.DoubanSDKData.AppKey = "0bd4507ebf3d18eb237cf17c3db74ac1";
            DoubanSDK.DoubanSDKData.AppSecret = "8e59243064b1d940";
            DoubanSDK.DoubanSDKData.RedirectUri = "http://thankcreate.github.com/Care/callback.html";
            DoubanSDK.DoubanSDKData.Scope = "shuo_basic_r,shuo_basic_w,douban_basic_common";
        }

        /// <summary>
        /// 在应用程序由最终用户正常启动时进行调用。
        /// 当启动应用程序以执行打开特定的文件或显示搜索结果等操作时
        /// 将使用其他入口点。
        /// </summary>
        /// <param name="args">有关启动请求和过程的详细信息。</param>
        protected override void OnLaunched(LaunchActivatedEventArgs args)
        {
            Frame rootFrame = Window.Current.Content as Frame;

            // 不要在窗口已包含内容时重复应用程序初始化，
            // 只需确保窗口处于活动状态
            if (rootFrame == null)
            {
                // 创建要充当导航上下文的框架，并导航到第一页
                rootFrame = new Frame();

                if (args.PreviousExecutionState == ApplicationExecutionState.Terminated)
                {
                    //TODO: 从之前挂起的应用程序加载状态
                }

                // 将框架放在当前窗口中
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null)
            {
                // 当未还原导航堆栈时，导航到第一页，
                // 并通过将所需信息作为导航参数传入来配置
                // 参数
                String testFirst = PreferenceHelper.GetPreference("FirstCome");
                bool openRes;
                if (String.IsNullOrEmpty(testFirst))
                {
                    PreferenceHelper.SetPreference("FirstCome", "whatever");
                    openRes = rootFrame.Navigate(typeof(GuideView));
                }
                else 
                {
                    openRes = rootFrame.Navigate(typeof(MainPage), args.Arguments);
                }
                if (!openRes)
                {
                    throw new Exception("Failed to create initial page");
                }
            }
            // 确保当前窗口处于活动状态
            Window.Current.Activate();
        }

        /// <summary>
        /// 在将要挂起应用程序执行时调用。在不知道应用程序
        /// 将被终止还是恢复的情况下保存应用程序状态，
        /// 并让内存内容保持不变。
        /// </summary>
        /// <param name="sender">挂起的请求的源。</param>
        /// <param name="e">有关挂起的请求的详细信息。</param>
        private void OnSuspending(object sender, SuspendingEventArgs e)
        {
            var deferral = e.SuspendingOperation.GetDeferral();
            //TODO: 保存应用程序状态并停止任何后台活动
            deferral.Complete();
        }
    }
}
