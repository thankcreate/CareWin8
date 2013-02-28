using System;
using System.Net;
using System.Windows;
using System.IO;


namespace CareWin8
{
    public class MiscTool
    {
        public static String RemoveHtmlTag(String input)
        {
            if (String.IsNullOrWhiteSpace(input))
                return "";
            return Windows.Data.Html.HtmlUtilities.ConvertToText(input);
        }

        public static String GetMyName()
        {
            String myName = PreferenceHelper.GetPreference("SinaWeibo_NickName");
            if (!string.IsNullOrEmpty(myName))
            {
                return myName;
            }
            // sina is null ,than search renren
            myName = PreferenceHelper.GetPreference("Renren_NickName");
            if (!string.IsNullOrEmpty(myName))
            {
                return myName;
            }
            // than search douban
            myName = PreferenceHelper.GetPreference("Douban_NickName");
            if (!string.IsNullOrEmpty(myName))
            {
                return myName;
            }

            return "";
        }

        public static String GetHerName()
        {
            String herName = PreferenceHelper.GetPreference("SinaWeibo_FollowerNickName");
            if (!string.IsNullOrEmpty(herName))
            {
                return herName;
            }
            // sina is null
            herName = PreferenceHelper.GetPreference("Renren_FollowerNickName");
            if (!string.IsNullOrEmpty(herName))
            {
                return herName;
            }
            // renren is null
            herName = PreferenceHelper.GetPreference("Douban_FollowerNickName");
            if (!string.IsNullOrEmpty(herName))
            {
                return herName;
            }
            return "";
        }

        // 现在其实没有区别了
        public static String GetHerIconUrl()
        {  
            String herIcon = PreferenceHelper.GetPreference("SinaWeibo_FollowerAvatar2");
            if (!string.IsNullOrEmpty(herIcon))
            {
                return herIcon;
            }

             
            herIcon = PreferenceHelper.GetPreference("Renren_FollowerAvatar2");
            if (!string.IsNullOrEmpty(herIcon))
            {
                return herIcon;
            }

             
            herIcon = PreferenceHelper.GetPreference("Douban_FollowerAvatar2");
            if (!string.IsNullOrEmpty(herIcon))
            {
                return herIcon;
            }

            herIcon = PreferenceHelper.GetPreference("SinaWeibo_FollowerAvatar");
            if (!string.IsNullOrEmpty(herIcon))
            {
                return herIcon;
            }

            herIcon = PreferenceHelper.GetPreference("Renren_FollowerAvatar");
            if (!string.IsNullOrEmpty(herIcon))
            {
                return herIcon;
            }

            herIcon = PreferenceHelper.GetPreference("Douban_FollowerAvatar");
            if (!string.IsNullOrEmpty(herIcon))
            {
                return herIcon;
            }
            return "";
        }

        public static String GetHerIconByType(EntryType type)
        {
            if (type == EntryType.SinaWeibo)
                return GetHerSinaWeiboIconUrl();
            else if (type == EntryType.Renren)
                return GetHerRenrenIconUrl();
            else if (type == EntryType.Douban)
                return GetHerDoubanIconUrl();
            return "";
        }

        // 现在其实没有区别了
        public static String GetHerSinaWeiboIconUrl()
        {
            String herIcon = PreferenceHelper.GetPreference("SinaWeibo_FollowerAvatar2");
            if (!string.IsNullOrEmpty(herIcon))
            {
                return herIcon;
            }

            return  PreferenceHelper.GetPreference("SinaWeibo_FollowerAvatar");
        }

        // 现在其实没有区别了
        public static String GetHerDoubanIconUrl()
        {
            String herIcon = PreferenceHelper.GetPreference("Douban_FollowerAvatar2");
            if (!string.IsNullOrEmpty(herIcon))
            {
                return herIcon;
            }

            return PreferenceHelper.GetPreference("Douban_FollowerAvatar");
        }

        // 现在其实没有区别了
        public static String GetHerRenrenIconUrl()
        {
            String herIcon = PreferenceHelper.GetPreference("Renren_FollowerAvatar2");
            if (!string.IsNullOrEmpty(herIcon))
            {
                return herIcon;
            }

            return PreferenceHelper.GetPreference("Renren_FollowerAvatar");
        }

        // 桌面上必须要用高清图，不论这个图是不是规则的四方形
        public static String GetHerIconUrlInDesktopTile()
        {
            String herIcon = PreferenceHelper.GetPreference("SinaWeibo_FollowerAvatar2");
            if (!string.IsNullOrEmpty(herIcon))
            {
                return herIcon;
            }
                         
            herIcon = PreferenceHelper.GetPreference("Renren_FollowerAvatar2");
            if (!string.IsNullOrEmpty(herIcon))
            {
                return herIcon;
            }

            herIcon = PreferenceHelper.GetPreference("Douban_FollowerAvatar2");
            if (!string.IsNullOrEmpty(herIcon))
            {
                return herIcon;
            }

            herIcon = PreferenceHelper.GetPreference("SinaWeibo_FollowerAvatar");
            if (!string.IsNullOrEmpty(herIcon))
            {
                return herIcon;
            }

            herIcon = PreferenceHelper.GetPreference("Renren_FollowerAvatar");
            if (!string.IsNullOrEmpty(herIcon))
            {
                return herIcon;
            }

            herIcon = PreferenceHelper.GetPreference("Douban_FollowerAvatar");
            if (!string.IsNullOrEmpty(herIcon))
            {
                return herIcon;
            }
            return "";
        }

        public static String GetMyIconUrl()
        {
            String myIcon = PreferenceHelper.GetPreference("SinaWeibo_Avatar");
            if (!string.IsNullOrEmpty(myIcon))
            {
                return myIcon;
            }

            myIcon = PreferenceHelper.GetPreference("Renren_Avatar");
            if (!string.IsNullOrEmpty(myIcon))
            {
                return myIcon;
            }

            myIcon = PreferenceHelper.GetPreference("Douban_Avatar");
            if (!string.IsNullOrEmpty(myIcon))
            {
                return myIcon;
            }
            return "";           
        }

        // >_< 一点也不Friendly吧....这种设定~~~哦哈哈哈哈~~~~
        // 这是主要是为了滤掉gif图
        public static String MakeFriendlyImageURL(String url)
        {
            if (url == null)
            {
                return null;
            }
            if (url.EndsWith("gif"))
            {
                return "";
            }
            return url;
        }


        //public static byte[] EncodeToJpeg(WriteableBitmap wb)
        //{
        //    using (MemoryStream stream = new MemoryStream())
        //    {
        //        wb.SaveJpeg(
        //            stream,
        //            wb.PixelWidth,
        //            wb.PixelHeight,
        //            0,
        //            85);
        //        return stream.ToArray();
        //    }
        //}
    }
}
