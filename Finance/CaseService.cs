using DAL;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Services
{
    public class CaseService
    {
        ApplicationContext db;
        JsonSerializerOptions options;

        public CaseService(ApplicationContext context)
        {
            db = context;

            options = new JsonSerializerOptions()
            {
                WriteIndented = true
            };
        }

        public async Task<string> GetAllCasesAsJSON()
        {
            var cases = await db.cases
                .OrderByDescending(x => x.kod_razb)
                .ToArrayAsync();

            return JsonSerializer.Serialize(cases, options);
        }

        public async Task<string> GetCaseAsJSON(int id)
        {
            var @case = await db.cases
                .FirstOrDefaultAsync(x => x.kod_razb == id);

            if (@case == null)
                return "Не найдено в базе";

            return JsonSerializer.Serialize(@case, options);
        }
    }
}
