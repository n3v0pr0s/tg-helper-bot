using DAL.Repositories;
using DAL.Repositories.Interfaces;
using System;

namespace DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext context = new ApplicationContext();
        private INoteRepository noteRepository;
        private ICaseRepository caseRepository;
        public INoteRepository Notes => noteRepository ?? new NoteRepository(context);
        public ICaseRepository Cases => caseRepository ?? new CaseRepository(context);
        public void Save()
        {
            context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }

            this.disposed = true;
        }

    }
}
