using DAL.Entities;
using DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class NoteRepository : RepositoryBase<Note>, INoteRepository
    {
        private readonly ApplicationContext context;
        public NoteRepository(ApplicationContext context) : base(context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<Note>> GetNotesByUserId(int id)
        {
            return await context.Set<Note>()
                .Where(x => x.user_id == id)
                .ToListAsync();
        }
    }
}
