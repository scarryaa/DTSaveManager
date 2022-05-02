using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace DTSaveManager.Services
{
    public class TimerService
    {
        public System.Timers.Timer timer = new System.Timers.Timer();

        public void DoTaskAfterDelay(int time, Action action)
        {
            timer.Interval = time;
            timer.Elapsed += delegate (object sender, ElapsedEventArgs e)
            {
                timer.Stop();
                action.Invoke();
            };
            timer.Start();
        }

        public void CancelTask()
        {
            timer.Stop();
        }
    }
}
