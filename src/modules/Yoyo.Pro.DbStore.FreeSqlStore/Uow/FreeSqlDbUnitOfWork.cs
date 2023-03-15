using System;
using FreeSql;
using Yoyo.Pro.DbStore.Uow;

namespace Yoyo.Pro.DbStore.FreeSqlStore.Uow
{
    public class FreeSqlDbUnitOfWork : IDbUnitOfWork
    {
        public IUnitOfWork Outer { get; private set; }

        public Guid Id { get; }

        public bool IsDispose { get; private set; }

        protected Action DisposeAction;

        public FreeSqlDbUnitOfWork(IUnitOfWork unitOfWork, Action disposeAction)
        {
            Outer = unitOfWork;
            DisposeAction = disposeAction;

            Id = Guid.NewGuid();
        }

        public void Commit()
        {
            Outer?.Commit();
        }


        public void Rollback()
        {
            Outer?.Rollback();
        }

        public void Dispose()
        {
            if (!IsDispose)
            {
                IsDispose = true;
                Outer?.Dispose();
                Outer = null;
                DisposeAction?.Invoke();
            }
        }

    }
}
