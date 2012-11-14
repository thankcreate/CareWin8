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
using System.Net;
using System.Net.Http;
using Windows.Networking;
using System.Text;
using RestBase;
using SinaWeiboSDK;
namespace CareWin8.MyControl
{
    public sealed partial class SinaWeiboLoginPopup : UserControl
    {
        public static Action AuthSuccessComplete;
        public static Action AuthCancel;
        public static Action AuthFailComplete;

        private bool m_bIsCompleted = false;
        Page m_pageParent;
        public SinaWeiboLoginPopup()
        {
            this.InitializeComponent();
            String authPage = String.Format("{0}?client_id={1}&response_type=code&redirect_uri={2}&display=mobile",
               "https://api.weibo.com/oauth2/authorize",
               SinaWeiboSDKData.AppKey,
               SinaWeiboSDKData.RedirectUri
               );
            Browser.Navigate(new Uri(authPage));
        }

        public void Show(Page page)
        {
            m_pageParent = page;
            FlyoutHelper.ShowInCenter(BrowserPopup, 300, 480);
            if (!BrowserPopup.IsOpen)
            {
                BrowserPopup.IsOpen = true;   
            }                     
        }

        public void Close()
        {
            BrowserPopup.IsOpen = false;
        }

        private void Browser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            if (m_bIsCompleted)
                return;

            if (e.Uri.AbsoluteUri.Contains("error=access_denied"))
            {
                if (AuthCancel != null)
                    AuthCancel.Invoke();
                m_bIsCompleted = true;
                BrowserPopup.IsOpen = false;
                return;
            }

            if (!e.Uri.AbsoluteUri.Contains("code="))
            {
                return;
            }

            m_bIsCompleted = true;
            var arguments = e.Uri.AbsoluteUri.Split('?');
            if (0 == arguments.Length)
            {
                if (AuthCancel != null)
                    AuthCancel.Invoke();
                return;
            }

            // arguments[1]即问号后面的部分
            String code = "";
            foreach (string item in arguments[1].Split('&'))
            {
                string[] parts = item.Split('=');
                if (parts[0] == "code")
                {
                    code = parts[1];
                    break;
                }
            }
            GetTokenByCode(code);
        }

        public async void GetTokenByCode(String code)
        {            
            GetTokenByCodeResponse response = await App.SinaWeiboAPI.AuthorityAPI.GetTokenByCode(code);
            if (response.Error == RestError.ERROR_SUCCESS)
            {
                String token = response.access_token;
                PreferenceHelper.SetPreference("SinaWeibo_Token", token);
                if (AuthSuccessComplete != null)
                {
                    AuthSuccessComplete.Invoke();
                }                
            }
            else
            {
                if (AuthFailComplete != null)
                    AuthFailComplete.Invoke();
            }            
        }
    }
}
