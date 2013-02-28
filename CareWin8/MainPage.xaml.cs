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
using Windows.UI.Popups;

// “空白页”项模板在 http://go.microsoft.com/fwlink/?LinkId=234238 上有介绍

namespace CareWin8
{
    /// <summary>
    /// 可用于自身或导航至 Frame 内部的空白页。
    /// </summary>
    public sealed partial class MainPage : CareWin8.Common.LayoutAwarePage
    {
        ProgressBarHelper m_progressBarHelper;

        #region StatusGridViewHeightProperty
        public static readonly DependencyProperty StatusGridViewHeightProperty =
            DependencyProperty.Register("StatusGridViewHeight", typeof(string), typeof(MainPage), new PropertyMetadata((string)"600"));

        public string StatusGridViewHeight
        {
            get { return (string)GetValue(StatusGridViewHeightProperty); }
            set { SetValue(StatusGridViewHeightProperty, value); }
        }
        #endregion      
  
        #region PicGridViewMaxRowProperty
        public static readonly DependencyProperty PicGridViewMaxRowProperty =
            DependencyProperty.Register("PicGridViewMaxRow", typeof(string), typeof(MainPage), new PropertyMetadata((string)"3"));

        public string PicGridViewMaxRow
        {
            get { return (string)GetValue(PicGridViewMaxRowProperty); }
            set { SetValue(PicGridViewMaxRowProperty, value); }
        }
        #endregion    

        public static Windows.UI.Core.CoreDispatcher UIDispater;
        public MainPage()
        {            
            UIDispater = Windows.UI.Core.CoreWindow.GetForCurrentThread().Dispatcher;
            this.InitializeComponent();
            InitBackground();
            InitStatusGridViewHeight();
            InitPicGridViewMaxRow();

            NavigationCacheMode = NavigationCacheMode.Enabled;
            PictureGridView.AddHandler(UIElement.PointerWheelChangedEvent, new Windows.UI.Xaml.Input.PointerEventHandler(OnPointerWheelChanged), true);
            StatusGridView.AddHandler(UIElement.PointerWheelChangedEvent, new Windows.UI.Xaml.Input.PointerEventHandler(OnPointerWheelChanged), true);
            this.DataContext = App.MainViewModel;
            InitSettingPane();            
        }

        private void InitPicGridViewMaxRow()
        {
            double res = Window.Current.Bounds.Height - 140 - 50 - 173 * 4;
            if (res > 0)
                PicGridViewMaxRow = "4";
            else
                PicGridViewMaxRow = "3";
        }

        private void InitStatusGridViewHeight()
        {
            int height = (int)(Window.Current.Bounds.Height - 200);
            StatusGridViewHeight = height.ToString();
        }

        private void InitBackground()
        {
            if (App.MainPageBackgroundImageStream != null)
            {
                var backgroundImage = new Windows.UI.Xaml.Media.Imaging.BitmapImage();
                backgroundImage.SetSource(App.MainPageBackgroundImageStream);
                backgoundBrush.ImageSource = backgroundImage;
            }
            else
            {
                Uri imageUri = new Uri(CareConstDefine.MainPageBackgroundURI);
                var backgroundImage = new Windows.UI.Xaml.Media.Imaging.BitmapImage(imageUri);
                backgoundBrush.ImageSource = backgroundImage;
            }
        }

        private void OnPointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            e.Handled = false;
        }

        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {
            // 允许已保存页状态重写要显示的初始项
            //if (pageState != null && pageState.ContainsKey("ScrollHorizonalOffset"))
            //{
            //    double? offset = pageState["ScrollHorizonalOffset"] as double?;
            //    MainScrollViewer.ScrollToHorizontalOffset((double)offset);
            //}
        }

        protected override void SaveState(Dictionary<String, Object> pageState)
        {
            //double offset = MainScrollViewer.HorizontalOffset;
            //pageState["ScrollHorizonalOffset"] = offset;
        }

        /// <summary>
        /// 在此页将要在 Frame 中显示时进行调用。
        /// </summary>
        /// <param name="e">描述如何访问此页的事件数据。Parameter
        /// 属性通常用于配置页。</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
    
        }

        private void RefreshMainViewModel()
        {
            if (m_progressBarHelper == null)
            {
                m_progressBarHelper = new ProgressBarHelper(MainProgessBar, RefreshViewerHelper.RefreshViewItems);
            }
            RefreshViewerHelper.RefreshMainViewModel(m_progressBarHelper);
        }


        private void InitSettingPane()
        {
            Windows.UI.ApplicationSettings.SettingsCommand cmd1 = new Windows.UI.ApplicationSettings.SettingsCommand("1", "帐户", c => 
            {
                Frame.Navigate(typeof(AccountView));
            });
            Windows.UI.ApplicationSettings.SettingsCommand cmd2 = new Windows.UI.ApplicationSettings.SettingsCommand("2", "选项", c =>
            {
                MyControl.PreferenceSettingControl control = new MyControl.PreferenceSettingControl();
                control.Show();
            });
            Windows.UI.ApplicationSettings.SettingsCommand cmd3 = new Windows.UI.ApplicationSettings.SettingsCommand("3", "关于", c => 
            {
                MyControl.PreferenceAboutControl control = new MyControl.PreferenceAboutControl();
                control.Show();
            });
            Windows.UI.ApplicationSettings.SettingsCommand cmd4 = new Windows.UI.ApplicationSettings.SettingsCommand("4", "隐私策略", c =>
            {
                try
                {
                    Uri uri = new Uri("http://thankcreate.github.com/Care/privacy.html");
                    Windows.System.Launcher.LaunchUriAsync(uri);
                }
                catch (System.Exception ex)
                {

                }              
            });
            // 命令是在CommandsRequested事件中添加的  
            Windows.UI.ApplicationSettings.SettingsPane.GetForCurrentView().CommandsRequested += (sp, arg) =>
            {
                arg.Request.ApplicationCommands.Add(cmd1);
                arg.Request.ApplicationCommands.Add(cmd2);
                arg.Request.ApplicationCommands.Add(cmd3);
                arg.Request.ApplicationCommands.Add(cmd4);
            };  
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AccountView));
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            DialogHelper.ShowToastDialog("haha");
        }

        //private void StatusGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //    int index = StatusGridView.SelectedIndex;
        //    if (index != -1)
        //    {
        //        ItemViewModel model = App.MainViewModel.TopItems[index];
        //        if (model.Type == EntryType.ShowMore)
        //        {
        //            Frame.Navigate(typeof(StatusTimelineView));
        //        }
        //        else if (model.Type == EntryType.Rss)
        //        {
        //            Frame.Navigate(typeof(RssDetailView), model);
        //        }
        //        else if( model.Type == EntryType.SinaWeibo || 
        //            model.Type == EntryType.Renren ||
        //            model.Type == EntryType.Douban)
        //        {
        //            Frame.Navigate(typeof(StatusDetailView), model);
        //        }
                
        //    }
        //    StatusGridView.SelectedIndex = -1;
        //}

        private void Item_Tapped(object sender, TappedRoutedEventArgs e)
        {
            try
            {
                FrameworkElement control = sender as FrameworkElement;
                if (control == null)
                    return;
                ItemViewModel model = control.DataContext as ItemViewModel;
                if (model == null)
                    return;

                if (model.Type == EntryType.ShowMore)
                {
                    Frame.Navigate(typeof(StatusTimelineView));
                }
                else if (model.Type == EntryType.Nothing)
                {
                    Frame.Navigate(typeof(AccountView));
                }
                else if (model.Type == EntryType.Rss)
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
            catch (System.Exception ex)
            {

            }
        }


        private void StatusLable_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(StatusTimelineView));
        }

        private void PictureLable_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (App.MainViewModel.PictureItems == null || App.MainViewModel.PictureItems.Count == 0)
            {
                return;
            }
            Frame.Navigate(typeof(PhotoFlipView), "0");
        }

        private void ForwardImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FrameworkElement image = sender as FrameworkElement;
            if (image == null)
                return;
            ItemViewModel model = image.DataContext as ItemViewModel;
            if (model == null)
                return;
            if (model.ForwardItem == null)
                return;
            String fullURL = model.ForwardItem.FullImageURL;

            MyControl.FullImageControl control = new MyControl.FullImageControl(fullURL);
            control.ShowPop();
            e.Handled = true;
        }

        private void ContentImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            FrameworkElement image = sender as FrameworkElement;
            if (image == null)
                return;
            ItemViewModel model = image.DataContext as ItemViewModel;
            if (model == null)
                return;
            String fullURL = model.FullImageURL;
            MyControl.FullImageControl control = new MyControl.FullImageControl(fullURL);
            control.ShowPop();
            e.Handled = true;
        }

        private void PictureGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = PictureGridView.SelectedIndex;
            if (index != -1)
            {                
                Frame.Navigate(typeof(PhotoFlipView), index.ToString());
            }
            PictureGridView.SelectedIndex = -1;
        }

        private void TimeSpan_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(TimeSpanView));
        }

        private void CharactorAnalysis_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(CharactorAnalysisView));
        }

        private void LovePercentage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(LovePercentageView));
        }

        private void PotentialEnemy_Tapped(object sender, TappedRoutedEventArgs e)
        {
            Frame.Navigate(typeof(PotentialEnemyView));
        }

        private void Cat_Tapped(object sender, TappedRoutedEventArgs e)
        {            
            Frame.Navigate(typeof(CatView));
            //Frame.Navigate(typeof(GuideView));
        }

        private void MenuImage_Tapped(object sender, TappedRoutedEventArgs e)
        {
            topAppBar.IsOpen = true;
            bottomAppBar.IsOpen = true;
        }

        private void Account_Tapped(object sender, TappedRoutedEventArgs e)
        {
            topAppBar.IsOpen = false;
            bottomAppBar.IsOpen = false;
            Frame.Navigate(typeof(AccountView));
        }

        private void Setting_Tapped(object sender, TappedRoutedEventArgs e)
        {
            topAppBar.IsOpen = false;
            bottomAppBar.IsOpen = false;
            MyControl.PreferenceSettingControl control = new MyControl.PreferenceSettingControl();
            control.Show();
        }

        private void Refresh_Tapped(object sender, TappedRoutedEventArgs e)
        {
            topAppBar.IsOpen = false;
            bottomAppBar.IsOpen = false;
            if (m_progressBarHelper == null)
            {
                m_progressBarHelper = new ProgressBarHelper(MainProgessBar, RefreshViewerHelper.RefreshViewItems);
            }
            RefreshViewerHelper.RefreshMainViewModel(m_progressBarHelper);
        }

        private void Write_Tapped(object sender, TappedRoutedEventArgs e)
        {
            topAppBar.IsOpen = false;
            bottomAppBar.IsOpen = false;
            MyControl.PopControl pc = new MyControl.PopControl();
            MyControl.SelectPublicSourceControl control = new MyControl.SelectPublicSourceControl(pc);
            control.ChooseAction = ChooseActionCallback;
            pc.SetCustomContent(control);
            pc.ShowPop();   
        }

        private void ChooseActionCallback(EntryType type)
        {
            Dictionary<String, Object> parameters = new Dictionary<String, Object>();
            //parameters.Add("Content", "");

            if (type == EntryType.SinaWeibo)
            {
                parameters.Add("Type", EntryType.SinaWeibo);
                Frame.Navigate(typeof(AddCommitView), parameters);
            }
            else if (type == EntryType.Renren)
            {
                parameters.Add("Type", EntryType.Renren);
                Frame.Navigate(typeof(AddCommitView), parameters);
            }
            else if (type == EntryType.Douban)
            {
                parameters.Add("Type", EntryType.Douban);
                Frame.Navigate(typeof(AddCommitView), parameters);
            }
        }


        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            if (App.MainViewModel.IsChanged)
            {
                App.MainViewModel.IsChanged = false;
                RefreshMainViewModel();
            }       
        }
    }
}
