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
    public sealed partial class RssSetPathControl : UserControl
    {
        PopControl _pc;
        public Action<String> ConfirmAction;

        public RssSetPathControl()
        {
           this.InitializeComponent();            
        }

        public RssSetPathControl(PopControl pc)
        {
            this.InitializeComponent();
            this._pc = pc;
        }

        private void Close()
        {
            if (_pc != null)
                _pc.HidePop();
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            String path = txtBox.Text;
            if (String.IsNullOrEmpty(path))
            {
                DialogHelper.ShowMessageDialog("输入为空是想闹哪样啊~~~");
            }
            else
            {
                if (ConfirmAction != null)
                {
                    ConfirmAction.Invoke(path);
                }                
                Close();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

    }
}
