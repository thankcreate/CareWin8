using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace CareWin8.MyControl
{
    public class PicGridView : GridView
    {
        public PicGridView()
        {
        }

        protected override void PrepareContainerForItemOverride(Windows.UI.Xaml.DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
            PictureItemViewModel model = item as PictureItemViewModel;
            if (model == null)
                return;
            int index = -1;
            index = App.MainViewModel.PictureItems.IndexOf(model);
            if (index < 0)
                return;
            int rowSpan = 1;
            int columnSpan = 1;

            double res = Window.Current.Bounds.Height - 140 - 50 - 173 * 4;
            // 4行
            if (res > 0)
            {
                switch (index)
                {
                    case 0:
                        rowSpan = 2;
                        columnSpan = 2;
                        break;
                    case 1:
                        rowSpan = 1;
                        columnSpan = 1;
                        break;
                    case 2:
                        rowSpan = 1;
                        columnSpan = 1;
                        break;
                    case 3:
                        rowSpan = 2;
                        columnSpan = 2;
                        break;
                    case 4:
                        rowSpan = 1;
                        columnSpan = 2;
                        break;
                    case 5:
                        rowSpan = 1;
                        columnSpan = 1;
                        break;
                    case 6:
                        rowSpan = 2;
                        columnSpan = 2;
                        break;
                    case 7:
                        rowSpan = 1;
                        columnSpan = 2;
                        break;
                    case 8:
                        rowSpan = 1;
                        columnSpan = 1;
                        break;
                }
            }
            // 3行时
            else
            {
                switch (index)
                {
                    case 0:
                        rowSpan = 2;
                        columnSpan = 2;
                        break;
                    case 1:
                        rowSpan = 1;
                        columnSpan = 1;
                        break;
                    case 2:
                        rowSpan = 1;
                        columnSpan = 2;
                        break;
                    case 3:
                        rowSpan = 1;
                        columnSpan = 2;
                        break;
                    case 4:
                        rowSpan = 1;
                        columnSpan = 1;
                        break;
                    case 5:
                        rowSpan = 2;
                        columnSpan = 2;
                        break;
                    case 6:
                        rowSpan = 1;
                        columnSpan = 1;
                        break;
                    case 7:
                        rowSpan = 2;
                        columnSpan = 1;
                        break;
                    case 8:
                        rowSpan = 1;
                        columnSpan = 1;
                        break;
                }
            }
           
            if (rowSpan != 1 || columnSpan != 1)
            {
                model.SmartUrl = model.FullUrl;
            }
            VariableSizedWrapGrid.SetRowSpan(element as UIElement, rowSpan);
            VariableSizedWrapGrid.SetColumnSpan(element as UIElement, columnSpan);
            
        }
    }
}
