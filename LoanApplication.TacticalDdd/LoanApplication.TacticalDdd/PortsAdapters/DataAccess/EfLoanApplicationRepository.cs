using System.Linq;
using LoanApplication.TacticalDdd.DomainModel;

namespace LoanApplication.TacticalDdd.PortsAdapters.DataAccess
{
    public class EfLoanApplicationRepository : ILoanApplicationRepository
    {
        private readonly LoanDbContext _dbContext;

        public EfLoanApplicationRepository(LoanDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Add(DomainModel.LoanApplication loanApplication)
        {
            _dbContext.LoanApplications.Add(loanApplication);
        }

        public DomainModel.LoanApplication WithNumber(LoanApplicationNumber loanApplicationNumber)
        {
            return _dbContext.LoanApplications.FirstOrDefault(l => l.Number == loanApplicationNumber);
        }
    }
}