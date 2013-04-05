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

using Windows.Storage;
using Windows.Storage.Streams;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Storage.Pickers;

namespace CareWin8.MyControl
{
    public sealed partial class FullImageControl : UserControl
    {
        #region MaxHeightProperty
        public static readonly DependencyProperty MaxHeightProperty =
            DependencyProperty.Register("MaxHeight", typeof(string), typeof(FullImageControl), new PropertyMetadata((string)""));

        public string MaxHeight
        {
            get { return (string)GetValue(MaxHeightProperty); }
            set { SetValue(MaxHeightProperty, value); }
        }
        #endregion

        #region MaxWidthProperty
        public static readonly DependencyProperty MaxWidthProperty =
            DependencyProperty.Register("MaxWidth", typeof(string), typeof(FullImageControl), new PropertyMetadata((string)""));

        public string MaxWidth
        {
            get { return (string)GetValue(MaxWidthProperty); }
            set { SetValue(MaxWidthProperty, value); }
        }
        #endregion

        #region FullURLProperty
        public static readonly DependencyProperty FullURLProperty =
            DependencyProperty.Register("FullURL", typeof(string), typeof(FullImageControl), new PropertyMetadata((string)""));

        public string FullURL
        {
            get { return (string)GetValue(FullURLProperty); }
            set { SetValue(FullURLProperty, value); }
        }
        #endregion

        Popup m_pop;

        public FullImageControl(String url)
        {
            int height = (int)(Window.Current.Bounds.Height * 0.7);
            int width = (int)(Window.Current.Bounds.Width) - 800;
            MaxHeight = height.ToString();
            MaxWidth = width.ToString();
            FullURL = url;

            this.DefaultStyleKey = typeof(PopControl);
            // 弹出层的宽度等于窗口的宽度
            this.Width = Window.Current.Bounds.Width;
            // 弹出层的高度等于窗口的高度
            this.Height = Window.Current.Bounds.Height;
            this.HorizontalContentAlignment = Windows.UI.Xaml.HorizontalAlignment.Stretch;
            this.m_pop = new Popup();
            // 将当前控件作为Popup的Child
            this.m_pop.Child = this;

            this.InitializeComponent();            
        }

        public void ShowPop()
        {
            if (this.m_pop != null)
            {
                this.m_pop.IsOpen = true;
            }
        }

        public void HidePop()
        {
            if (this.m_pop != null)
            {
                this.m_pop.IsOpen = false;
            }
        }

        private void Out_Tapped(object sender, TappedRoutedEventArgs e)
        {
            HidePop();
        }

        private void Image_ImageOpened(object sender, RoutedEventArgs e)
        {
            LoadingRing.IsActive = false;
        }

        // 这里主要是防止点到Image时也退出了
        private void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {
            e.Handled = true;
        }

        private async void Save_Tapped(object sender, TappedRoutedEventArgs e)
        {
            StorageFolder folder = KnownFolders.PicturesLibrary;
           

            var fileSave = new FileSavePicker();
            //fileSave.SuggestedSaveFile = storageFile;
            fileSave.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            fileSave.SuggestedFileName = "local";
            fileSave.DefaultFileExtension = ".jpg";
            fileSave.FileTypeChoices.Add("JPEG file", new List<string> { ".jpg" });
            StorageFile file = await fileSave.PickSaveFileAsync();

            // 为null代表取消了
            if (file != null)
            {
                var rass = RandomAccessStreamReference.CreateFromUri(new Uri(FullURL));
                var streamRandom = await rass.OpenReadAsync();

                IBuffer buffer = RandomAccessStreamToBuffer(streamRandom);
                await FileIO.WriteBufferAsync(file, buffer);

                DialogHelper.ShowToastDialog("保存成功！");
            }

        }



        private IBuffer RandomAccessStreamToBuffer(IRandomAccessStream randomstream)
        {
            Stream stream = WindowsRuntimeStreamExtensions.AsStreamForRead(randomstream.GetInputStreamAt(0));
            MemoryStream memoryStream = new MemoryStream();
            if (stream != null)
            {
                byte[] bytes = ConvertStreamTobyte(stream);
                if (bytes != null)
                {
                    var binaryWriter = new BinaryWriter(memoryStream);
                    binaryWriter.Write(bytes);
                }
            }
            IBuffer buffer = WindowsRuntimeBufferExtensions.GetWindowsRuntimeBuffer(memoryStream, 0, (int)memoryStream.Length);
            return buffer;
        }


        public static byte[] ConvertStreamTobyte(Stream input)
        {
            byte[] buffer = new byte[16 * 1024];

            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}
