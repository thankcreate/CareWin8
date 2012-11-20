using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SinaWeiboSDK;
namespace CareWin8
{
    public class SinaWeiboFetcher : BaseFetcher
    {
        private TaskHelper m_taskHelper;

        public override void FetchCommentManList(LoadCommmentManCompleteHandler handler)
        {
            List<CommentMan> finalList = new List<CommentMan>();
            if (App.MainViewModel.SinaWeiboItems == null)
            {
                handler(finalList);
                return;
            }
            m_taskHelper = new TaskHelper(() =>
            {
                handler(finalList);                
            });   

            foreach (ItemViewModel item in App.MainViewModel.SinaWeiboItems)
            {
                m_taskHelper.PushTask();
                FetchSingleStatusComment(item.ID, finalList);
            }            
        }

        public async void FetchSingleStatusComment(string id, List<CommentMan> addList)
        {
            GetStatusCommentsResponse res = await App.SinaWeiboAPI.CommentAPI.GetStatusComments(id);
            if (res.Error == RestBase.RestError.ERROR_SUCCESS && res.comments != null)
            {
                foreach (Comment comment in res.comments)
                {
                    // 要去掉她自己啊！！！！你个2货
                    String herID = PreferenceHelper.GetPreference("SinaWeibo_FollowerID");
                    if (comment.user.id != herID)
                    {
                        CommentMan man = new CommentMan
                        {
                            name = comment.user.screen_name,
                            id = comment.user.id
                        };
                        addList.Add(man);
                    }
                }     
            }            
            m_taskHelper.PopTask();            
        }
    }
}
