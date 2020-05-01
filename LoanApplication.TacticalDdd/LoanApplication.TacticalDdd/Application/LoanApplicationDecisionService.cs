using System.Security.Claims;
using LoanApplication.TacticalDdd.DomainModel;
using LoanApplication.TacticalDdd.DomainModel.Ddd;
using LoanApplication.TacticalDdd.DomainModel.DomainEvents;

namespace LoanApplication.TacticalDdd.Application
{
    public class LoanApplicationDecisionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoanApplicationRepository _loanApplicationRepository;
        private readonly IOperatorRepository _operatorRepository;
        private readonly IEventPublisher _eventPublisher;

        public LoanApplicationDecisionService(
            IUnitOfWork unitOfWork,
            ILoanApplicationRepository loanApplications, 
            IOperatorRepository operators, 
            IEventPublisher eventPublisher)
        {
            _unitOfWork = unitOfWork;
            _loanApplicationRepository = loanApplications;
            _operatorRepository = operators;
            _eventPublisher = eventPublisher;
        }
        
        public void RejectApplication(string applicationNumber, ClaimsPrincipal principal, string rejectionReason)
        {
            var loanApplication = _loanApplicationRepository
                .WithNumber(LoanApplicationNumber.Of(applicationNumber));
            var user = _operatorRepository.WithLogin(Login.Of(principal.Identity.Name));
            
            loanApplication.Reject(user);
            
            _unitOfWork.CommitChanges();
            
            _eventPublisher.Publish(new LoanApplicationRejected(loanApplication));
        }

        public void AcceptApplication(string applicationNumber, ClaimsPrincipal principal)
        {
            var loanApplication = _loanApplicationRepository
                .WithNumber(LoanApplicationNumber.Of(applicationNumber));
            var user = _operatorRepository.WithLogin(Login.Of(principal.Identity.Name));
            
            loanApplication.Accept(user);
            
            _unitOfWork.CommitChanges();
            
            _eventPublisher.Publish(new LoanApplicationAccepted(loanApplication));
        }
    }
}