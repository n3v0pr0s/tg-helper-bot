using DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DAL.Controllers
{
    public class CaseController
    {
        ApplicationContext db;
        public CaseController(ApplicationContext context)
        {
            db = context;
        }

        public async Task<string> Get()
        {
            var cases = await db.cases.OrderByDescending(x => x.kod_razb).ToArrayAsync();
            return JsonSerializer.Serialize<Case[]>(cases);
        }

        public async Task<string> Get(int id)
        {
            var @case = await db.cases.FirstOrDefaultAsync(x => x.kod_razb == id);

            if (@case == null)
                return "Не найдено в базе";

            return JsonSerializer.Serialize<Case>(@case);
        }
    }
}
