using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareWin8
{
    public class RefreshViewerHelper
    {
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
                App.MainViewModel.ListItems.Add(model);
            }
            foreach (ItemViewModel item in App.MainViewModel.ListItems)
            {
                App.MainViewModel.Items.Add(item);
            }

            RefreshPicturePage();
        }
      

        public static void RefreshPicturePage()
        {
            
            App.MainViewModel.ListPictureItems.Clear();
            App.MainViewModel.ListPictureItems.AddRange(App.MainViewModel.SinaWeiboPicItems);
            App.MainViewModel.ListPictureItems.AddRange(App.MainViewModel.RenrenPicItems);
            App.MainViewModel.ListPictureItems.Sort(
                delegate(PictureItem a, PictureItem b)
                {
                    return (a.TimeObject < b.TimeObject ? 1 : a.TimeObject == b.TimeObject ? 0 : -1);
                });

            // 目前最多只需要9个
            int count = App.MainViewModel.ListPictureItems.Count > 9 ? 9 : App.MainViewModel.ListPictureItems.Count;

            App.MainViewModel.PictureItems.Clear();
            List<PictureItem>  rangeList = App.MainViewModel.ListPictureItems.GetRange(0, count);
            foreach (PictureItem item in rangeList)
            {
                App.MainViewModel.PictureItems.Add(item);
            }
        }
    }
}
