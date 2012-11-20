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
        
        public MainPage()
        {
            this.InitializeComponent();
            NavigationCacheMode = NavigationCacheMode.Enabled;
            PictureGridView.AddHandler(UIElement.PointerWheelChangedEvent, new Windows.UI.Xaml.Input.PointerEventHandler(OnPointerWheelChanged), true);
            StatusGridView.AddHandler(UIElement.PointerWheelChangedEvent, new Windows.UI.Xaml.Input.PointerEventHandler(OnPointerWheelChanged), true);
            this.DataContext = App.MainViewModel;
            InitSettingPane();
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
            if (App.MainViewModel.IsChanged)            
            {
                App.MainViewModel.IsChanged = false;
                RefreshMainViewModel();                
            }           
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
            // 命令是在CommandsRequested事件中添加的  
            Windows.UI.ApplicationSettings.SettingsPane.GetForCurrentView().CommandsRequested += (sp, arg) =>
            {
                arg.Request.ApplicationCommands.Add(cmd1);
                arg.Request.ApplicationCommands.Add(cmd2);
                arg.Request.ApplicationCommands.Add(cmd3);
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

        private void StatusGridView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = StatusGridView.SelectedIndex;
            if (index != -1)
            {
                ItemViewModel model = App.MainViewModel.TopItems[index];
                if (model.Type == EntryType.ShowMore)
                {
                    Frame.Navigate(typeof(StatusTimelineView));
                }
                else if (model.Type == EntryType.Rss)
                {
                    Frame.Navigate(typeof(RssDetailView), model);
                }
                else if( model.Type == EntryType.SinaWeibo || 
                    model.Type == EntryType.Renren ||
                    model.Type == EntryType.Douban)
                {
                    Frame.Navigate(typeof(StatusDetailView), model);
                }
                
            }
            StatusGridView.SelectedIndex = -1;
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
            // TODO
            Frame.Navigate(typeof(LovePercentageView));
        }

        private void PotentialEnemy_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // TODO
            Frame.Navigate(typeof(PotentialEnemyView));
        }

        private void Cat_Tapped(object sender, TappedRoutedEventArgs e)
        {
            // TODO
            Frame.Navigate(typeof(TimeSpanView));
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
    }
}
