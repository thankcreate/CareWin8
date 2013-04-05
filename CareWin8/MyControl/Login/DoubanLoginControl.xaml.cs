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
using DoubanSDK;
using RestBase;
// “用户控件”项模板在 http://go.microsoft.com/fwlink/?LinkId=234236 上提供

namespace CareWin8.MyControl
{
    public sealed partial class DoubanLoginControl : UserControl
    {
        PopControl _pc;
        public static Action AuthSuccessComplete;
        public static Action AuthCancel;
        public static Action AuthFailComplete;

        private bool m_bIsCompleted = false;

        public DoubanLoginControl(PopControl pc)
        {
            this.InitializeComponent();
            this._pc = pc;
            String authPage = String.Format("{0}?client_id={1}&response_type=code&redirect_uri={2}&display=mobile&scope={3}",
               ConstDefine.OAuth2_0Url,
               DoubanSDKData.AppKey,
               DoubanSDKData.RedirectUri,
               DoubanSDKData.Scope
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
            if (m_bIsCompleted)
                return;

            if (e.Uri.AbsoluteUri.Contains("error=access_denied"))
            {
                if (AuthCancel != null)
                    AuthCancel.Invoke();
                m_bIsCompleted = true;
                Close();
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
            GetTokenByCodeResponse response = await App.DoubanAPI.AuthorityAPI.GetTokenByCode(code);
            if (response.Error == RestError.ERROR_SUCCESS)
            {
                String token = response.access_token;
                PreferenceHelper.SetPreference("Douban_Token", token);
                if (AuthSuccessComplete != null)
                {
                    Close();
                    AuthSuccessComplete.Invoke();
                }
            }
            else
            {
                if (AuthFailComplete != null)
                {
                    Close();
                    AuthFailComplete.Invoke();
                }
            }
        }
    }
}
