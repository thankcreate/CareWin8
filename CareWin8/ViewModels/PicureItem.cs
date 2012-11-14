using System;
using System.Net;
using System.Windows;


namespace CareWin8
{
    public class PictureItem
    {
        public PictureItem()
        {
            Url = "";
            FullUrl = "";
            Id = "";
            Title = "";
            Content = "";
            OriginUrl = "";
            Type = EntryType.SinaWeibo;
        }
        // 小图
        public string Url { get; set; }
        // 大图
        public string FullUrl { get; set; }
        public string Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        // 目前没用
        public string OriginUrl { get; set; }
        public EntryType Type { get; set; }
        public String TypeString
        {
            get
            {
                switch (Type)
                {
                    case EntryType.Renren:
                        return "   来自人人";
                    case EntryType.Douban:
                        return "   来自豆瓣";
                    case EntryType.SinaWeibo:
                        return "   来自新浪微博";
                    case EntryType.Feed:
                        return "   来自RSS订阅";
                }
                return "来自火星";
            }
        }

        public string _time;
        public string Time 
        {
            get
            {
                return _time = ExtHelpers.TimeObjectToString(TimeObject);
            }
            set
            {
                _time = value;
            }
        }
        public DateTimeOffset TimeObject { get; set; }

        public string IsImageExists
        {
            get
            {
                return string.IsNullOrEmpty(Url) ? "Collapsed" : "Visible";
            }
        }
    }
}
