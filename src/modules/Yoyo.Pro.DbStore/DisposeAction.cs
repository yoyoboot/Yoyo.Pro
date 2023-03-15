using System;
using System.Threading;

namespace Yoyo.Pro.DbStore
{
    /// <summary>
    /// This class can be used to provide an action when
    /// Dispose method is called.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1063:Implement IDisposable Correctly", Justification = "<挂起>")]
    public class DisposeAction : IDisposable
    {
        private Action _action;

        /// <summary>
        /// Creates a new <see cref="DisposeAction"/> object.
        /// </summary>
        /// <param name="action">Action to be executed when this object is disposed.</param>
        public DisposeAction(Action action)
        {
            _action = action;
        }

        public void Dispose()
        {
            // Interlocked prevents multiple execution of the _action.
            var action = Interlocked.Exchange(ref _action, null);
            action?.Invoke();
        }
    }
}
