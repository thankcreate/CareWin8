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
    public sealed partial class PotentialEnemyControl : UserControl
    {
        #region Name1Property
        public static readonly DependencyProperty Name1Property =
            DependencyProperty.Register("Name1", typeof(String), typeof(PotentialEnemyControl), new PropertyMetadata((String)""));

        public String Name1
        {
            get { return (String)GetValue(Name1Property); }
            set { SetValue(Name1Property, value); }
        }
        #endregion

        #region Name2Property
        public static readonly DependencyProperty Name2Property =
            DependencyProperty.Register("Name2", typeof(String), typeof(PotentialEnemyControl), new PropertyMetadata((String)""));

        public String Name2
        {
            get { return (String)GetValue(Name2Property); }
            set { SetValue(Name2Property, value); }
        }
        #endregion

        #region Name3Property
        public static readonly DependencyProperty Name3Property =
            DependencyProperty.Register("Name3", typeof(String), typeof(PotentialEnemyControl), new PropertyMetadata((String)""));

        public String Name3
        {
            get { return (String)GetValue(Name3Property); }
            set { SetValue(Name3Property, value); }
        }
        #endregion

        #region Value1Property
        public static readonly DependencyProperty Value1Property =
            DependencyProperty.Register("Value1", typeof(int), typeof(PotentialEnemyControl), new PropertyMetadata((int)0));

        public int Value1
        {
            get { return (int)GetValue(Value1Property); }
            set { SetValue(Value1Property, value); }
        }
        #endregion

        #region Value2Property
        public static readonly DependencyProperty Value2Property =
            DependencyProperty.Register("Value2", typeof(int), typeof(PotentialEnemyControl), new PropertyMetadata((int)0));

        public int Value2
        {
            get { return (int)GetValue(Value2Property); }
            set { SetValue(Value2Property, value); }
        }
        #endregion

        #region Value3Property
        public static readonly DependencyProperty Value3Property =
            DependencyProperty.Register("Value3", typeof(int), typeof(PotentialEnemyControl), new PropertyMetadata((int)0));

        public int Value3
        {
            get { return (int)GetValue(Value3Property); }
            set { SetValue(Value3Property, value); }
        }
        #endregion

        public PotentialEnemyControl(String name1, int value1, String name2, int value2, String name3, int value3)
        {
            Name1 = name1;
            Name2 = name2;
            Name3 = name3;
            Value1 = value1;
            Value2 = value2;
            Value3 = value3;
            this.InitializeComponent();
        }
    }
}
