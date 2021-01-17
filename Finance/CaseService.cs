using DAL;
using DAL.Repositories.Interfaces;
using System.Text.Json;
using System.Threading.Tasks;

namespace Services
{
    public class CaseService
    {
        private readonly ICaseRepository caseRepository;
        private readonly JsonSerializerOptions options;

        public CaseService(UnitOfWork unitOfWork)
        {
            this.caseRepository = unitOfWork.Cases;

            options = new JsonSerializerOptions()
            {
                WriteIndented = true
            };
        }

        public async Task<string> GetAllCasesAsJSON()
        {
            var cases = await caseRepository.GetAll();

            return JsonSerializer.Serialize(cases, options);
        }

        public async Task<string> GetCaseAsJSON(int id)
        {
            var @case = await caseRepository.Get(x => x.kod_razb == id);

            if (@case == null)
                return "Не найдено в базе";

            return JsonSerializer.Serialize(@case, options);
        }
    }
}
