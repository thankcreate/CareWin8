using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace CareWin8
{
    class PreferenceHelper
    {
        public static ApplicationDataContainer settings = ApplicationData.Current.LocalSettings;

        public static String GetPreference(String key)
        {
            if (settings.Values.ContainsKey(key))
            {
                return settings.Values[key] as String;
            }
            return string.Empty;
        }

        public static Boolean SetPreference(String key, String value)
        {
            settings.Values[key] = value;            
            return true;
        }
        public static void RemovePreference(String key)
        {
            if (settings.Values.ContainsKey(key))
            {
                settings.Values.Remove(key);
            }
        }

        public static void SavePreference()
        {
             // Win8 上这里空着就行
        }

        public static void RemoveSinaWeiboPreference()
        {
            RemovePreference("SinaWeibo_NickName");
            RemovePreference("SinaWeibo_ID");
            RemovePreference("SinaWeibo_FollowerID");
            RemovePreference("SinaWeibo_FollowerNickName");
            RemovePreference("SinaWeibo_Token");
            RemovePreference("SinaWeibo_Avatar");
            RemovePreference("SinaWeibo_FollowerAvatar");
            RemovePreference("SinaWeibo_FollowerAvatar2");
        }

        public static void RemoveSinaWeiboLoginAccountPreference()
        {
            RemovePreference("SinaWeibo_NickName");
            RemovePreference("SinaWeibo_ID");
            RemovePreference("SinaWeibo_Token");
            RemovePreference("SinaWeibo_Avatar");
        }

        public static void RemoveRenrenPreference()
        {
            RemovePreference("Renren_NickName");
            RemovePreference("Renren_ID");
            RemovePreference("Renren_FollowerID");
            RemovePreference("Renren_FollowerNickName");
            RemovePreference("Renren_Token");
            RemovePreference("Renren_Avatar");
            RemovePreference("Renren_FollowerAvatar");
            RemovePreference("Renren_FollowerAvatar2");
        }


        public static void RemoveRenrenLoginAccountPreference()
        {
            RemovePreference("Renren_NickName");
            RemovePreference("Renren_ID");
            RemovePreference("Renren_Token");
            RemovePreference("Renren_Avatar");
        }

        public static void RemovePasswordPreference()
        {
            RemovePreference("Global_Password");
        }

        public static void RemoveRssPreference()
        {
            RemovePreference("RSS_FollowerPath");
            RemovePreference("RSS_FollowerSite");
        }

        public static void RemoveDoubanPreference()
        {
            RemovePreference("Douban_NickName");
            RemovePreference("Douban_ID");
            RemovePreference("Douban_FollowerID");
            RemovePreference("Douban_FollowerNickName");
            RemovePreference("Douban_Token");
            RemovePreference("Douban_Avatar");
            RemovePreference("Douban_FollowerAvatar");
            RemovePreference("Douban_FollowerAvatar2");
        }

        public static void RemoveDoubanLoginAccountPreference()
        {
            RemovePreference("Douban_NickName");
            RemovePreference("Douban_ID");
            RemovePreference("Douban_Token");
            RemovePreference("Douban_Avatar");
        }
    }
}
