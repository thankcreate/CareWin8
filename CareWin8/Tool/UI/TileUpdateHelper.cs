using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NotificationsExtensions;
using NotificationsExtensions.ToastContent;
using Windows.UI.Notifications;
using NotificationsExtensions.BadgeContent;
using NotificationsExtensions.TileContent;
using Windows.Data.Xml.Dom;
using System.Collections.ObjectModel;
namespace CareWin8
{
    public class TileUpdateHelper
    {
        public static TileUpdater m_tileUpdater = TileUpdateManager.CreateTileUpdaterForApplication();
        public static String DEFAULT1 = "/Images/Tile/default1.jpg";
        public static String DEFAULT2 = "/Images/Tile/default2.jpg";
        public static String DEFAULT3 = "/Images/Tile/default3.jpg";
        public static String DEFAULT4 = "/Images/Tile/default4.jpg";

        // 2个最新消息，1个图片，1个1+4，1个logo 
        public static void Update()
        {
            m_tileUpdater.Clear();
            m_tileUpdater.EnableNotificationQueue(true);
            UpdateStatusNews();
            UpdatePhotoNews();
            UpdateMutiplePhotoNews();
            UpdateLogo();
        }
        

        public static void UpdateStatusNews()
        {
            try
            {
                ObservableCollection<ItemViewModel> items = App.MainViewModel.Items;
                if (items == null)
                    return;
                int count = items.Count < 2 ? items.Count : 2;
                for (int i = 0; i < count; i++)
                {
                    ItemViewModel item = items[i];

                    // FIXME: 不知道为什么，反正豆瓣的图就是显示不出来
                    String iconURL = MiscTool.GetHerIconByType(item.Type);
                    UpdateTileWideSmallImageAndText04(iconURL, item.Title, item.Content);
                }
            }
            catch (System.Exception ex)
            {
            	
            }

        }

        // 由于宽Tile和方Tile共用通知队列的5的上限，此处做了合并
        public static void UpdatePhotoNews()
        {
            try
            {
                ObservableCollection<PictureItemViewModel> items = App.MainViewModel.PictureItems;
                if (items == null)
                    return;
                int count = items.Count < 1 ? items.Count : 1;
                for (int i = 0; i < count; i++)
                {
                    UpdateTileWideImageAndText01(items[i].Url, items[i].Content, MiscTool.GetHerIconUrl());
                }
            }
            catch (System.Exception ex)
            {
            	
            }        
        }


        public static void UpdateMutiplePhotoNews()
        {
            try
            {
                String herIcon = MiscTool.GetHerIconUrl();
                ObservableCollection<PictureItemViewModel> items = App.MainViewModel.PictureItems;
                if (items == null)
                    return;
                int count = items.Count;
                if (count == 0)
                {
                    UpdateTileWideImageCollection(herIcon, DEFAULT1, DEFAULT2, DEFAULT3, DEFAULT4);
                }
                else if (count == 1)
                {
                    UpdateTileWideImageCollection(herIcon, items[0].Url, DEFAULT2, DEFAULT3, DEFAULT4);
                }
                else if (count == 2)
                {
                    UpdateTileWideImageCollection(herIcon, items[0].Url, items[1].Url, DEFAULT3, DEFAULT4);
                }

                else if (count == 3)
                {
                    UpdateTileWideImageCollection(herIcon, items[0].Url, items[1].Url, items[2].Url, DEFAULT4);
                }
                else if (count >= 4)
                {
                    UpdateTileWideImageCollection(herIcon, items[0].Url, items[1].Url, items[2].Url, items[3].Url);
                }
            }
            catch (System.Exception ex)
            {
            	
            }        
        }

        // 由于宽Tile和方Tile共用通知队列的5的上限，此处做了合并
        public static void UpdateLogo()
        {
            UpdateTileImage("/Assets/310x150.png", "/Assets/150x150.png");
        }

        // 左侧为图，右侧为标题、内容
        public static void UpdateTileWideSmallImageAndText04(String url, String title, String content)
        {
            var tileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWideSmallImageAndText04);

            var tileTextAttributes = tileXml.GetElementsByTagName("image");
            var imageElement = tileTextAttributes[0] as XmlElement;            
            imageElement.SetAttribute("src", url);
            imageElement.SetAttribute("alt", "");

            var textElement = tileXml.GetElementsByTagName("text");
            var titleElement = textElement[0] as XmlElement;
            var contentElement = textElement[1] as XmlElement;
            titleElement.AppendChild(tileXml.CreateTextNode(title));
            contentElement.AppendChild(tileXml.CreateTextNode(content));

            var tileNotification = new TileNotification(tileXml);
            m_tileUpdater.Update(tileNotification);
        }

        // 上方为大图，下面为小文字条
        // 由于宽Tile和方Tile共用通知队列的5的上限，此处做了合并
        public static void UpdateTileWideImageAndText01(String url, String content, String squareURL)
        {
            var tileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWideImageAndText01);

            var tileTextAttributes = tileXml.GetElementsByTagName("image");
            var imageElement = tileTextAttributes[0] as XmlElement;            
            imageElement.SetAttribute("src", url);
            imageElement.SetAttribute("alt", "");

            var textElement = tileXml.GetElementsByTagName("text");
            var contentElement = textElement[0] as XmlElement;
            contentElement.AppendChild(tileXml.CreateTextNode(content));

            var tileSquareXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquareImage);
            var tileSquareImageList = tileSquareXml.GetElementsByTagName("image");
            var tileSquareImageElement = tileSquareImageList[0] as XmlElement;
            tileSquareImageElement.SetAttribute("src", squareURL);
            tileSquareImageElement.SetAttribute("alt", "");
            var node = tileXml.ImportNode(tileSquareXml.GetElementsByTagName("binding").Item(0), true);
            tileXml.GetElementsByTagName("visual").Item(0).AppendChild(node);

            var tileNotification = new TileNotification(tileXml);
            m_tileUpdater.Update(tileNotification);
        }

        // 一张大图, 这里主要是用来更新logo的
        // 由于宽Tile和方Tile共用通知队列的5的上限，所以把更新宽logo和方logo合并
        public static void UpdateTileImage(String wideUrl, String squareURL)
        {
            var tileWideXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWideImage);
            var tileTextAttributes = tileWideXml.GetElementsByTagName("image");
            var imageElement = tileTextAttributes[0] as XmlElement;
            imageElement.SetAttribute("src", wideUrl);
            imageElement.SetAttribute("alt", "");


            var tileSquareXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquareImage);
            var tileSquareImageList = tileSquareXml.GetElementsByTagName("image");
            var tileSquareImageElement = tileSquareImageList[0] as XmlElement;
            tileSquareImageElement.SetAttribute("src", squareURL);
            tileSquareImageElement.SetAttribute("alt", "");
            var node = tileWideXml.ImportNode(tileSquareXml.GetElementsByTagName("binding").Item(0), true);
            tileWideXml.GetElementsByTagName("visual").Item(0).AppendChild(node);

            var tileNotification = new TileNotification(tileWideXml);
            m_tileUpdater.Update(tileNotification);
        }

        // 左侧为一张大图，右侧为4张小图
        public static void UpdateTileWideImageCollection(String url0, String url1, String url2, String url3, String url4)
        {
            var tileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileWideImageCollection);

            var tileImageAttributes = tileXml.GetElementsByTagName("image");
            var imageElement0 = tileImageAttributes[0] as XmlElement;
            imageElement0.SetAttribute("src", url0);
            imageElement0.SetAttribute("alt", "");

            var imageElement1 = tileImageAttributes[1] as XmlElement;
            imageElement1.SetAttribute("src", url1);
            imageElement1.SetAttribute("alt", "");

            var imageElement2 = tileImageAttributes[2] as XmlElement;
            imageElement2.SetAttribute("src", url2);
            imageElement2.SetAttribute("alt", "");

            var imageElement3 = tileImageAttributes[3] as XmlElement;
            imageElement3.SetAttribute("src", url3);
            imageElement3.SetAttribute("alt", "");

            var imageElement4 = tileImageAttributes[4] as XmlElement;
            imageElement4.SetAttribute("src", url4);
            imageElement4.SetAttribute("alt", "");

            var tileNotification = new TileNotification(tileXml);
            m_tileUpdater.Update(tileNotification);
        }


        public static void UpdateTileSquareImage(String url)
        {
            var tileXml = TileUpdateManager.GetTemplateContent(TileTemplateType.TileSquareImage);

            var tileTextAttributes = tileXml.GetElementsByTagName("image");
            var imageElement = tileTextAttributes[0] as XmlElement;
            imageElement.SetAttribute("src", url);
            imageElement.SetAttribute("alt", "");

            var tileNotification = new TileNotification(tileXml);
            m_tileUpdater.Update(tileNotification);
        }
    }
}
