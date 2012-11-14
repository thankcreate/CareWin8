using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using Windows.UI.Xaml.Data;

namespace CareWin8
{
    public class FriendViewModelCollection : ObservableCollection<FriendViewModel>, ISupportIncrementalLoading
    {
        public bool HasMoreItems
        {
            get { return true; }
        }

        int test = 0;
        
        public Windows.Foundation.IAsyncOperation<LoadMoreItemsResult> LoadMoreItemsAsync(uint count)
        {            
            return Task.Run<LoadMoreItemsResult>(() =>
            {
                //this.Add(new FriendViewModel { Name = "123123" });
                return new LoadMoreItemsResult { Count = 1 };
            }).AsAsyncOperation<LoadMoreItemsResult>();            
        }
    }
}
