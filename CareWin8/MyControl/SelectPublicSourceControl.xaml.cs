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

// “用户控件”项模板在 http://go.microsoft.com/fwlink/?LinkId=234236 上提供

namespace CareWin8.MyControl
{
    public sealed partial class SelectPublicSourceControl : UserControl
    {
        PopControl _pc;

        public Action<EntryType> ChooseAction;

        public SelectPublicSourceControl(PopControl pc)
        {
            this.InitializeComponent();
            this._pc = pc;
        }

        private void Close()
        {
            if (_pc != null)
                _pc.HidePop();
        }
        private void SinaWeibo_Click(object sender, RoutedEventArgs e)
        {
            if (ChooseAction != null)
            {
                if (String.IsNullOrEmpty(PreferenceHelper.GetPreference("SinaWeibo_ID")))
                {
                    DialogHelper.ShowMessageDialog("新浪微博帐号尚未登陆的说~~");
                    ChooseAction(EntryType.NotSet);
                }
                else
                {
                    ChooseAction(EntryType.SinaWeibo);
                }
            }
            Close();
        }

        private void Renren_Click(object sender, RoutedEventArgs e)
        {
            if (ChooseAction != null)
                if (String.IsNullOrEmpty(PreferenceHelper.GetPreference("Renren_ID")))
                {
                    DialogHelper.ShowMessageDialog("人人帐号尚未登陆的说~~");
                    ChooseAction(EntryType.NotSet);
                }
                else
                {
                    ChooseAction(EntryType.Renren);
                }
            Close();
        }

        private void Douban_Click(object sender, RoutedEventArgs e)
        {
            if (ChooseAction != null)
                if (String.IsNullOrEmpty(PreferenceHelper.GetPreference("Douban_ID")))
                {
                    DialogHelper.ShowMessageDialog("豆瓣帐号尚未登陆的说~~");
                    ChooseAction(EntryType.NotSet);
                }
                else
                {
                    ChooseAction(EntryType.Douban);
                }
            Close();
        }
    }
}
