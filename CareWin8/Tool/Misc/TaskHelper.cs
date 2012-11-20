using System;
using System.Net;
using System.Windows;
using System.Threading;

namespace CareWin8
{
    public class TaskHelper
    {
        private int m_nTaskInProcess = 0;
        private static object thisLock = new object();


        public delegate void AllTaskCompleteCallback();
        public AllTaskCompleteCallback m_delAllTaskCompleCallback;

        public TaskHelper(AllTaskCompleteCallback callback)
        {
            //m_timer = new DispatcherTimer();
            m_delAllTaskCompleCallback = callback;
        }

        public TaskHelper(AllTaskCompleteCallback callback, int timeout)
        {
            m_delAllTaskCompleCallback = callback;

        }

        public void FirstTick(object sender, EventArgs e)
        {

            lock (thisLock)
            {
                if (m_nTaskInProcess != 0)
                {
                    m_nTaskInProcess = 0;
                    m_delAllTaskCompleCallback();
                }
            }
        }

        public void PushTask()
        {
            lock (thisLock)
            {
                m_nTaskInProcess++;
            }
        }

        public void PopTask()
        {
            lock (thisLock)
            {
                if (m_nTaskInProcess == 0)
                {
                    return;
                }

                if (--m_nTaskInProcess == 0)
                {
                    if (m_delAllTaskCompleCallback != null)
                    {
                        m_delAllTaskCompleCallback();
                    }
                }
            }
        }
    }
}
