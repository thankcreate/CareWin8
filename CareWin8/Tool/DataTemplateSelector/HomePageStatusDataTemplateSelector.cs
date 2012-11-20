using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace CareWin8
{
    public class HomePageStatusDataTemplateSelector : DataTemplateSelector
    {
        public DataTemplate SmallStatusTemplate { get; set; }
        public DataTemplate ShowMoreTemplate { get; set; }
        public DataTemplate NothingTemplate { get; set; }

        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            ItemViewModel model = item as ItemViewModel;
            if (model != null)
            {
                switch (model.Type)
                {
                    case EntryType.ShowMore:
                        return ShowMoreTemplate;
                        break;
                    case EntryType.Nothing:
                        return NothingTemplate;
                        break;
                    default:
                        return SmallStatusTemplate;
                        break;
                }
            }
            return base.SelectTemplateCore(item, container);
        }
    }
}
