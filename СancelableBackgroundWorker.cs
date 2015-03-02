using System;
using System.ComponentModel;
using System.Threading;

namespace VideoJam
{
    public class СancelableBackgroundWorker
        : BackgroundWorker
    {
        private Thread _thread;

        public СancelableBackgroundWorker()
        {
            WorkerSupportsCancellation = true;
            WorkerReportsProgress = true;
        }

        public void Run(object state)
        {
            var args = new DoWorkEventArgs(state);
            Exception e = null;

            try
            {
                OnDoWork(args);
            }
            catch (Exception ex)
            {
                e = ex;
            }

            OnRunWorkerCompleted(new RunWorkerCompletedEventArgs(args.Result, e, args.Cancel));
        }

        protected override void OnDoWork(DoWorkEventArgs e)
        {
            _thread = Thread.CurrentThread;

            try
            {
                base.OnDoWork(e);
                _thread = null;
            }
            catch (ThreadAbortException)
            {
                e.Cancel = true;
                Thread.ResetAbort();
            }
        }

        public void Abort()
        {
            if (!IsBusy)
            {
                return;
            }

            CancelAsync();

            try
            {
                if (_thread != null)
                {
                    _thread.Abort();
                    _thread = null;
                }
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch
            {
            }
        }
    }
}