using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml;

namespace CareWin8
{
    public class ChatTemplateSelector : DataTemplateSelector
    {
        public DataTemplate Her
        {
            get;
            set;
        }

        public DataTemplate Me
        {
            get;
            set;
        }


        protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
        {
            ChatItemViewModel chatItem = item as ChatItemViewModel;
            if (chatItem != null)
            {
                if (chatItem.Type == "Her")
                {
                    return Her;
                }
                else if (chatItem.Type == "Me")
                {
                    return Me;
                }
            }
            return base.SelectTemplateCore(item, container);
        }
    }
}
