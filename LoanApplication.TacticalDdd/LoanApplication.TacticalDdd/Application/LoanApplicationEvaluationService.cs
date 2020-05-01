using LoanApplication.TacticalDdd.DomainModel;
using LoanApplication.TacticalDdd.DomainModel.Ddd;

namespace LoanApplication.TacticalDdd.Application
{
    public class LoanApplicationEvaluationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoanApplicationRepository _loanApplicationRepository;
        private readonly ScoringRulesFactory _scoringRulesFactory;
        
        public LoanApplicationEvaluationService(
            IUnitOfWork unitOfWork,
            ILoanApplicationRepository loanApplications, 
            IDebtorRegistry debtorRegistry)
        {
            _unitOfWork = unitOfWork;
            _loanApplicationRepository = loanApplications;
            _scoringRulesFactory = new ScoringRulesFactory(debtorRegistry);
        }
        public void EvaluateLoanApplication(string applicationNumber)
        {
            var loanApplication = _loanApplicationRepository
                .WithNumber(LoanApplicationNumber.Of(applicationNumber));
            
            loanApplication.Evaluate(_scoringRulesFactory.DefaultSet);
            
            _unitOfWork.CommitChanges();
        }
    }
}