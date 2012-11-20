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
    public sealed partial class TimeSpanControl : UserControl
    {
        #region Param1Property
        public static readonly DependencyProperty Param1Property =
            DependencyProperty.Register("Param1", typeof(int), typeof(TimeSpanControl), new PropertyMetadata((int)0));

        public int Param1
        {
            get { return (int)GetValue(Param1Property); }
            set { SetValue(Param1Property, value); }
        }
        #endregion

        #region Param2Property
        public static readonly DependencyProperty Param2Property =
            DependencyProperty.Register("Param2", typeof(int), typeof(TimeSpanControl), new PropertyMetadata((int)0));

        public int Param2
        {
            get { return (int)GetValue(Param2Property); }
            set { SetValue(Param2Property, value); }
        }
        #endregion

        #region Param3Property
        public static readonly DependencyProperty Param3Property =
            DependencyProperty.Register("Param3", typeof(int), typeof(TimeSpanControl), new PropertyMetadata((int)0));

        public int Param3
        {
            get { return (int)GetValue(Param3Property); }
            set { SetValue(Param3Property, value); }
        }
        #endregion

        #region Param4Property
        public static readonly DependencyProperty Param4Property =
            DependencyProperty.Register("Param4", typeof(int), typeof(TimeSpanControl), new PropertyMetadata((int)0));

        public int Param4
        {
            get { return (int)GetValue(Param4Property); }
            set { SetValue(Param4Property, value); }
        }
        #endregion

        public TimeSpanControl(int param1, int param2, int param3, int param4)
        {
            Param1 = param1;
            Param2 = param2;
            Param3 = param3;
            Param4 = param4;            
            this.InitializeComponent();
        }
    }
}
