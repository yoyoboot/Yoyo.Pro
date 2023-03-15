using System;

namespace Yoyo.Pro
{
    /// <summary>
    /// 空释放器
    /// </summary>
    public sealed class NullDisposable : IDisposable
    {
        public static NullDisposable Instance { get; } = new NullDisposable();

        private NullDisposable()
        {

        }

        public void Dispose()
        {

        }
    }
}
