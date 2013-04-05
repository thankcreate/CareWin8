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
using RenrenSDK;

// “用户控件”项模板在 http://go.microsoft.com/fwlink/?LinkId=234236 上提供

namespace CareWin8.MyControl
{
    public sealed partial class RenrenLoginControl : UserControl
    {
        PopControl _pc;
        public static Action AuthSuccessComplete;
        public static Action AuthCancel;
        public static Action AuthFailComplete;

        private bool m_bIsCompleted = false;

        public RenrenLoginControl(PopControl pc)
        {
            this.InitializeComponent();
            this._pc = pc;
            String scope = RenrenSDKData.Scope;
            if (String.IsNullOrEmpty(scope))
                scope = "";
            String authPage = String.Format("{0}?client_id={1}&response_type=token&redirect_uri={2}&display=mobile&scope={3}",
               "https://graph.renren.com/oauth/authorize",
               RenrenSDKData.AppKey,
               RenrenSDKData.RedirectUri,
               scope
               );
            Browser.Navigate(new Uri(authPage));
        }

        public void Close()
        {
            if (_pc != null)
            {
                _pc.HidePop();
            }
        }

        private void Browser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            try
            {
                if (m_bIsCompleted)
                    return;

                if (e.Uri.AbsoluteUri.Contains("error=login_denied"))
                {
                    if (AuthCancel != null)
                        AuthCancel.Invoke();
                    m_bIsCompleted = true;
                    Close();
                    return;
                }

                if (!e.Uri.AbsoluteUri.Contains("access_token="))
                {
                    return;
                }

                m_bIsCompleted = true;
                var arguments = e.Uri.AbsoluteUri.Split('#');
                if (0 == arguments.Length)
                {
                    if (AuthCancel != null)
                        AuthCancel.Invoke();
                    return;
                }


                // arguments[1]即#号后面的部分
                String token = "";
                String expires_in = "";
                foreach (string item in arguments[1].Split('&'))
                {
                    string[] parts = item.Split('=');
                    if (parts[0] == "access_token")
                    {
                        token = parts[1];
                    }
                    else if (parts[0] == "expires_in")
                    {
                        expires_in = parts[1];
                    }
                }
                if (String.IsNullOrEmpty(token) || String.IsNullOrEmpty(expires_in))
                {
                    if (AuthFailComplete != null)
                        AuthFailComplete.Invoke();
                }
                

                // 解码
                token = Uri.UnescapeDataString(token);
                PreferenceHelper.SetPreference("Renren_Token", token);

                
                if (AuthSuccessComplete != null)
                {
                    Close();
                    AuthSuccessComplete.Invoke();
                }

                RenrenTokenInfo tokenInfo = new RenrenTokenInfo();
                tokenInfo.access_token = token;
                tokenInfo.expires_in = DateTime.Now.AddSeconds(Int32.Parse(expires_in));
                RenrenAPI.RenrenAppInfo.SetTokenInfo(tokenInfo);

                Close();
                if (AuthSuccessComplete != null)
                {                    
                    AuthSuccessComplete.Invoke();
                }                   
            }
            catch (System.Exception ex)
            {
                Close();
                if (AuthFailComplete != null)
                {
                    AuthFailComplete.Invoke();
                }                
            }              
        }
        private async void test()
        {
            GetUIDResponse response = await App.RenrenAPI.UserAPI.GetUID();
            int a = 1;
            a++;
        }
    }
}
