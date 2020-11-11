using System;

namespace TalentPool
{
    public abstract class ObjectDisposable : IDisposable
    {

        private bool _disposed;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                DisposeUnmanagedResource();
            }
            _disposed = true;
        }
        protected void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        protected virtual void DisposeUnmanagedResource()
        {

        } 
    }
}
