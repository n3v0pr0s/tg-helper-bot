using DAL;
using DAL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services
{
    public class NoteService
    {
        ApplicationContext db;
        public NoteService(ApplicationContext context)
        {
            db = context;
        }

        public async Task<IEnumerable<Note>> GetNotesByUserId(int user_id)
        {
            return await db.notes
                .Where(x => x.user_id == user_id)
                .ToListAsync();
        }

        public async Task<string> GetNoteContent(int id)
        {
            var note = await db.notes
                .FirstOrDefaultAsync(x => x.id == id);

            if (note == null)
                return "Не найдено в базе";

            return note.content;
        }
    }
}
