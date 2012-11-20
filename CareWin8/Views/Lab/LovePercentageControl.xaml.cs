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

namespace CareWin8
{
    public sealed partial class LovePercentageControl : UserControl
    {
        #region Param1Property
        public static readonly DependencyProperty Param1Property =
            DependencyProperty.Register("Param1", typeof(int), typeof(LovePercentageControl), new PropertyMetadata((int)0));

        public int Param1
        {
            get { return (int)GetValue(Param1Property); }
            set { SetValue(Param1Property, value); }
        }
        #endregion

        public LovePercentageControl(int param1)
        {
            Param1 = param1;
            this.InitializeComponent();
        }
    }
}
