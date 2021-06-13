using DAL.Repositories;
using DAL.Repositories.Interfaces;
using System;

namespace DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext context;
        private INoteRepository noteRepository;

        public INoteRepository Notes { get; }

        public UnitOfWork()
        {
            this.context = new ApplicationContext();
            this.Notes = noteRepository ?? new NoteRepository(context);
        }

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
