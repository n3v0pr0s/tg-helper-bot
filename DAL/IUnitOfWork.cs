using DAL.Repositories.Interfaces;
using System;

namespace DAL
{
    public interface IUnitOfWork : IDisposable
    {
        INoteRepository Notes { get; }
        void Save();
    }
}
