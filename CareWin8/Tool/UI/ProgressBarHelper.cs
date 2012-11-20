using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using System.Threading;

namespace CareWin8
{
    // 这个辅助类是用来帮助显示“正在加载”的字样的
    // 每添加一个加载任务使用 PushTask
    // 每结束一个加载任务（无论成功或异常）使用PopTask()
    // 此类会自动更新顶部状态条
    public class ProgressBarHelper
    {
        private ProgressBar m_progressBar;
        private int m_nTaskInProcess;

        public delegate void AllTaskCompleteCallback();
        public Action m_delAllTaskCompleCallback;
                
        // 此函数应在UI线程执行
        public void PushTask()
        {            
            Interlocked.Increment(ref m_nTaskInProcess);
            // 如果是第一个任务，作更新加载条的操作
            if (m_nTaskInProcess == 1)
            {                
                m_progressBar.ShowPaused = false;
                m_progressBar.IsEnabled= true;                
                //m_progressBar.Visibility = Windows.UI.Xaml.Visibility.Visible;
            }
         
        }

        // 此函数应在UI线程执行
        public void PushTask(String id)
        {
            // 待扩展
            PushTask();
        }

        // 此函数应在UI线程执行 ，哈哈哈....我为什么要笑呢？
        public void PopTask()
        {           
            if (m_nTaskInProcess == 0)
            {
                return;
            }
            Interlocked.Decrement(ref m_nTaskInProcess);
            // 写你妹注释！
            if (m_nTaskInProcess == 0)
            {                
                //m_progressBar.IsIndeterminate = false;
                m_progressBar.ShowPaused = true;
                //m_progressBar.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
                if (m_delAllTaskCompleCallback != null)
                    m_delAllTaskCompleCallback.Invoke();
            }
           
        }

        public void PopTask(String id)
        {
            PopTask();
        }

        public ProgressBarHelper(ProgressBar progressIndicator, Action callback)
        {
            m_nTaskInProcess = 0;
            m_progressBar = progressIndicator;
            m_delAllTaskCompleCallback = callback;
        }
    }
}
