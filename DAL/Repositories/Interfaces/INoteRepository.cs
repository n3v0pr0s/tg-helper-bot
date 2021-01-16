using DAL.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DAL.Repositories.Interfaces
{
    public interface INoteRepository : IRepositoryBase<Note>
    {
        Task<IEnumerable<Note>> GetNotesByUserId(int id);
    }
}
