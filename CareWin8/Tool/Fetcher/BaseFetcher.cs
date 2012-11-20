using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CareWin8
{
    public delegate void FetchCompleteEvent();
    public delegate void LoadCommmentManCompleteHandler(List<CommentMan> manNameList);

    public class CommentMan
    {
        public String id;
        public String name;
    }

    public abstract class BaseFetcher
    {
        public event FetchCompleteEvent m_FetchCompleteEvent;
        public abstract void FetchCommentManList(LoadCommmentManCompleteHandler handler);
    }
}
