using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;

// “用户控件”项模板在 http://go.microsoft.com/fwlink/?LinkId=234235 上有介绍

namespace CareWin8.MyControl
{
    public sealed class ImageInFlip : Control
    {
        #region SourceProperty
        public static readonly DependencyProperty SourceProperty =
            DependencyProperty.Register("Source", typeof(ImageSource), typeof(ImageInFlip), new PropertyMetadata((PropertyChangedCallback)null));

        public ImageSource Source
        {
            get 
            {
                return (ImageSource)GetValue(SourceProperty);
            }
            set
            {
                SetValue(SourceProperty, value); 
            }
        }
        #endregion

        #region MyMaxHeightProperty
        public static readonly DependencyProperty MyMaxHeightProperty =
            DependencyProperty.Register("MyMaxHeight", typeof(double), typeof(ImageInFlip), new PropertyMetadata(null));

        public double MyMaxHeight
        {
            get { return (double)GetValue(MyMaxHeightProperty); }
            set { SetValue(MyMaxHeightProperty, value); }
        }
        #endregion

        #region MyMaxWidthProperty
        public static readonly DependencyProperty MyMaxWidthProperty =
            DependencyProperty.Register("MyMaxWidth", typeof(double), typeof(ImageInFlip), new PropertyMetadata(null));

        public double MyMaxWidth
        {
            get { return (double)GetValue(MyMaxWidthProperty); }
            set { SetValue(MyMaxWidthProperty, value); }
        }
        #endregion
     


        public ImageInFlip()
        {            
            this.HorizontalAlignment = HorizontalAlignment.Stretch;
            this.VerticalAlignment = VerticalAlignment.Stretch;
            this.DefaultStyleKey = typeof(ImageInFlip);
        }

        protected override  void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            Image image = this.GetTemplateChild("imageItem") as Image;
            if (image != null)
                image.ImageOpened += Image_ImageOpened;
        }

        void Image_ImageOpened(object sender, RoutedEventArgs e)
        {
            ProgressRing ring = this.GetTemplateChild("LoadingRing") as ProgressRing;
            if (ring != null)
                ring.IsActive = false;
        }      
    }
}
