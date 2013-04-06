using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace CareWin8
{
    class BlessTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Left
        {
            get;
            set;
        }

        public DataTemplate Right
        {
            get;
            set;
        }


        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            BlessItem chatItem = item as BlessItem;
            if (chatItem != null)
            {
                if (chatItem.index %2  == 0)
                {
                    return Left;
                }
                else if (chatItem.index % 2 == 1)
                {
                    return Right;
                }
            }
            return base.SelectTemplateCore(item, container);
        }
    }
}
