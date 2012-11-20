using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Xml;
using Windows.Web.Syndication;
using RestBase;
using System.IO;
using NotificationsExtensions;
using NotificationsExtensions.ToastContent;
using Windows.UI.Notifications;
using NotificationsExtensions.BadgeContent;
using NotificationsExtensions.TileContent;
using Windows.Data.Xml.Dom;
namespace CareWin8
{
    // 统领所有的主要资源Refresh操作
    // 这里经过了重构，现在“所有”操作都被移到这里来了，MainPage里面做的东西变得很少了，这是和WP7版的程序不一样的地方
    public class RefreshViewerHelper
    {
        public static int MAX_SHOW_IN_HOMEPAGE = 7;

        public static void RefreshMainViewModel(ProgressBarHelper progressBarHelper)
        {
            if ( progressBarHelper == null)
                return;
            progressBarHelper.PushTask("SinaWeibo");
            progressBarHelper.PushTask("Rss");

            // 1.Weibo
            RefreshModelSinaWeibo(progressBarHelper);
            // 2.Rss
            RefreshModelRssFeed(progressBarHelper);
        }

        private static async void RefreshModelRssFeed(ProgressBarHelper progressBarHelper)
        {
            App.MainViewModel.RssItems.Clear();
            string url = PreferenceHelper.GetPreference("RSS_FollowerPath");
            if (string.IsNullOrEmpty(url))
            {
                progressBarHelper.PopTask();
                return;
            }
            RestClient client = new RestClient();
            RestRequest request = new RestRequest();
            client.Authority = url;
            request.Method = Method.Get;
            RestResponse response = await client.BeginRequest(request);

            if(response.Error == RestError.ERROR_SUCCESS && response.Content != null)
            {                
                try
                {
                    SyndicationFeed feed = new SyndicationFeed();
                    feed.Load(response.Content);
                    foreach (SyndicationItem item in feed.Items)
                    {
                        ItemViewModel model = RSSFeedConverter.ConvertFeedToCommon(item);
                        if (model != null)
                        {
                            App.MainViewModel.RssItems.Add(model);
                        }
                    }   
                }
                catch (System.Exception ex)
                {
                    DialogHelper.ShowToastDialog("RSS获取中发生错误，可能是网络问题，也可能是对应站点地址变更");                   
                }
                finally
                {
                    progressBarHelper.PopTask(); 
                }
            }
            else
            {
                DialogHelper.ShowToastDialog("RSS获取中发生错误，可能是网络问题，也可能是对应站点地址变更");
                progressBarHelper.PopTask(); 
            }           
              
        }

        private static async void RefreshModelSinaWeibo(ProgressBarHelper progressBarHelper)
        {
            App.MainViewModel.SinaWeiboPicItems.Clear();
            App.MainViewModel.SinaWeiboItems.Clear();
            if (String.IsNullOrEmpty(PreferenceHelper.GetPreference("SinaWeibo_ID")))
            {
                progressBarHelper.PopTask("Sina");
                return;
            }
            String followerID = PreferenceHelper.GetPreference("SinaWeibo_FollowerID");
            if (String.IsNullOrEmpty(followerID))
            {
                DialogHelper.ShowMessageDialog("尚未设置新浪微博关注对象");
                progressBarHelper.PopTask();
                return;
            }

            
            String strCount = PreferenceHelper.GetPreference("SinaWeibo_RecentCount");
            if (String.IsNullOrEmpty(strCount))
            {
                strCount = "20";
            }

            int nCount;
            try
            {
                nCount = int.Parse(strCount);
            }
            catch (System.Exception ex)
            {
                nCount = 20;
            }
            
            SinaWeiboSDK.GetUserTimelineResponse response = await App.SinaWeiboAPI.StatuesAPI.GetUserTimeline(followerID, nCount);
            if (response.Error == RestBase.RestError.ERROR_SUCCESS && response.statuses != null)
            {
                foreach (SinaWeiboSDK.Status status in response.statuses)
                {
                    ItemViewModel model = SinaWeiboConverter.ConvertItemToCommon(status);
                    if (model != null)
                    {
                        App.MainViewModel.SinaWeiboItems.Add(model);
                    }
                }
                progressBarHelper.PopTask();
            }
            else
            {
                if (response.Error == RestBase.RestError.ERROR_EXPIRED)
                {
                    DialogHelper.ShowMessageDialog("新浪微博帐号已过期，请重新登陆", "温馨提示");
                    PreferenceHelper.RemoveSinaWeiboLoginAccountPreference();
                }
                progressBarHelper.PopTask();
            }
        }



        public static void RefreshViewItems()
        {
            App.MainViewModel.ListItems.Clear();
            App.MainViewModel.ListItems.AddRange(App.MainViewModel.SinaWeiboItems);
            App.MainViewModel.ListItems.AddRange(App.MainViewModel.RssItems);
            App.MainViewModel.ListItems.AddRange(App.MainViewModel.RenrenItems);
            App.MainViewModel.ListItems.AddRange(App.MainViewModel.DoubanItems);
            App.MainViewModel.ListItems.Sort(
                delegate(ItemViewModel a, ItemViewModel b)
                {
                    return (a.TimeObject < b.TimeObject ? 1 : a.TimeObject == b.TimeObject ? 0 : -1);
                }
                );
            App.MainViewModel.Items.Clear();
            
            if (App.MainViewModel.ListItems.Count == 0)
            {
                ItemViewModel model = new ItemViewModel();
                model.Title = "没有得到任何结果的说~~~";
                model.Content = "请到帐号设置页中登陆并设置关注人";
                model.Type = EntryType.Nothing;
                model.ID = "nothing";
                App.MainViewModel.ListItems.Add(model);
            }
            foreach (ItemViewModel item in App.MainViewModel.ListItems)
            {
                App.MainViewModel.Items.Add(item);
            }


            // 更新主页上显示的头条，并且在最后加上一个"显示更多..."的条目
            // 由于主页的头条动态最开始可能是从缓存中加载的，如果两次的数据其实一模一样的话，
            // 重加载一次会造成UI没必要的刷新，所以需要先与上次作比较，有区别再刷新
            ObservableCollection<ItemViewModel> tempList = new ObservableCollection<ItemViewModel>();
            //
            int count = App.MainViewModel.Items.Count > MAX_SHOW_IN_HOMEPAGE ? MAX_SHOW_IN_HOMEPAGE : App.MainViewModel.Items.Count;            
            for (int i = 0; i < count; i++)
            {
                tempList.Add(App.MainViewModel.Items[i]);
            }
            ItemViewModel showMore = new ItemViewModel();
            showMore.Type = EntryType.ShowMore;
            tempList.Add(showMore);
            if (!IsTheSame(tempList, App.MainViewModel.TopItems))
            {
                App.MainViewModel.TopItems.Clear();
                foreach (ItemViewModel model in tempList)
                {
                    App.MainViewModel.TopItems.Add(model);
                }
                // 离开前，先把当前的TopItem缓存起来，这样下次进来就可以直接看到了
                SaveToLoaclCache();
            }

            // 开始刷新图片页信息
            RefreshPicturePage();

            // 更新桌面磁贴
            UpdateTile();
        }
        private static void UpdateTile()
        {
            TileUpdateHelper.Update();
        }

        private static bool IsTheSame(ObservableCollection<ItemViewModel> list1, ObservableCollection<ItemViewModel> list2)
        {
            try
            {
                if (list1.Count != list2.Count)
                    return false;
                int count = list1.Count;
                for (int i = 0; i < count; i++)
                {
                    if (list1[i].ID != list2[i].ID)
                        return false;
                }
                return true;
            }
            catch (System.Exception ex)
            {
                return false;
            }            
        }

        private static void SaveToLoaclCache()
        {
            StorageHelper<ObservableCollection<ItemViewModel>> stHelper = 
                new StorageHelper<ObservableCollection<ItemViewModel>>(StorageType.Local);
            stHelper.SaveASync(App.MainViewModel.TopItems, "TopItems");
        }

        public static void RefreshPicturePage()
        {
            
            App.MainViewModel.ListPictureItems.Clear();
            App.MainViewModel.ListPictureItems.AddRange(App.MainViewModel.SinaWeiboPicItems);
            App.MainViewModel.ListPictureItems.AddRange(App.MainViewModel.RenrenPicItems);
            App.MainViewModel.ListPictureItems.Sort(
                delegate(PictureItemViewModel a, PictureItemViewModel b)
                {
                    return (a.TimeObject < b.TimeObject ? 1 : a.TimeObject == b.TimeObject ? 0 : -1);
                });

            // 目前最多只需要9个
            int count = App.MainViewModel.ListPictureItems.Count > 9 ? 9 : App.MainViewModel.ListPictureItems.Count;

            App.MainViewModel.PictureItems.Clear();
            List<PictureItemViewModel>  rangeList = App.MainViewModel.ListPictureItems.GetRange(0, count);
            foreach (PictureItemViewModel item in rangeList)
            {
                App.MainViewModel.PictureItems.Add(item);
            }
        }
    }
}
