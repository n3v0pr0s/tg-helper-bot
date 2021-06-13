using DAL;
using DAL.Entities;
using DAL.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services
{
    public class NoteService
    {
        private readonly INoteRepository noteRepository;
        public NoteService(UnitOfWork unitOfWork)
        {
            this.noteRepository = unitOfWork.Notes;
        }

        public async Task<IEnumerable<Note>> GetNotesByUserId(long user_id)
        {
            return await noteRepository.GetWhere(x => x.user_id == user_id);
        }
    }
}
