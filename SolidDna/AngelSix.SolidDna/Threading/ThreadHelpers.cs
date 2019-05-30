using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AngelSix.SolidDna
{
    /// <summary>
    /// Methods for aiding in thread-related actions
    /// </summary>
    public static class ThreadHelpers
    {
        #region Private Members

        /// <summary>
        /// A blank control created on the UI thread used to invoke tasks on the UI thread
        /// </summary>
        private static Control mInvoker;

        #endregion

        /// <summary>
        /// Should be called from the UI thread to setup an Invoker (a user control)
        /// used to invoke any require tasks on the UI thread
        /// </summary>
        public static void Enable(Control uiControl)
        {
            mInvoker = uiControl;
        }

        /// <summary>
        /// Runs the specified action on the UI thread
        /// 
        /// NOTE: Only possible right now if the application is making use of TaskpaneIntegration as the
        /// UI dispatcher is via the Taskpane control on the UI thread
        /// 
        /// TODO: Find a better way to access an invoker/dispatcher without needing a Taskpane
        /// </summary>
        /// <param name="action">The action to run</param>
        public static void RunOnUIThread(Action action)
        {
            // If we are already on the UI thread, just run
            if (!mInvoker.InvokeRequired)
                action();
            // Otherwise invoke
            else
                mInvoker.Invoke((MethodInvoker)delegate { action(); });
        }

        /// <summary>
        /// Runs the specified action on the UI thread.
        /// 
        /// IMPORTANT: Make sure the caller (doing the await RunOnUIThreadAwait)
        /// does not await anything inside the action that can be locked by the 
        /// parent scope (the scope of the await itself) otherwise a deadlock
        /// could occur.
        /// 
        /// NOTE: Only possible right now if the application is making use of TaskpaneIntegration as the
        /// UI dispatcher is via the Taskpane control on the UI thread
        /// 
        /// TODO: Find a better way to access an invoker/dispatcher without needing a Taskpane
        /// </summary>
        /// <param name="action">The action to run</param>
        public static async Task RunOnUIThreadAsync(Action action)
        {
            // If we are on the UI thread already, just run the action
            if (mInvoker == null)
                throw new ArgumentNullException(await Localization.GetStringAsync("ErrorThreadInvokerNullError"));

            // If we are already on the UI thread, just run
            if (!mInvoker.InvokeRequired)
                action();
            else
            {
                // We are not on the UI thread, so it is safe to
                // await a task that will run on the UI thread
                // as it will not deadlock this thread
                var tcs = new TaskCompletionSource<bool>();

                // Invoke action
                mInvoker.Invoke((MethodInvoker)delegate
                {
                    try
                    {
                        action();
                    }
                    finally
                    {
                        tcs.TrySetResult(true);
                    }
                });

                // Wait for completion
                await tcs.Task;
            }
        }
    }
}
