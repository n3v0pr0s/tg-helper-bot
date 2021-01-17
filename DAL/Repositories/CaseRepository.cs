using DAL.Entities;
using DAL.Repositories.Interfaces;

namespace DAL.Repositories
{
    public class CaseRepository : RepositoryBase<Case>, ICaseRepository
    {
        private readonly ApplicationContext context;
        public CaseRepository(ApplicationContext context) : base(context)
        {
            this.context = context;
        }
    }
}
