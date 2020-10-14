using System;
using System.Threading;
using System.Threading.Tasks;

namespace Van.TalentPool.EntityFrameworkCore.Stores
{
    public class StoreBase
    {
        private bool _disposed;
        public StoreBase(VanDbContext context)
        {
            Context = context;
        }
        protected VanDbContext Context { get; }
        public bool AutoSaveChanges { get; set; } = true;
        protected Task SaveChanges(CancellationToken cancellationToken)
        {
            return AutoSaveChanges ? Context.SaveChangesAsync(cancellationToken) : Task.CompletedTask;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                Context.Dispose();
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
    }
}
