using System.Security.Claims;
using LoanApplication.TacticalDdd.Application.Api;
using LoanApplication.TacticalDdd.DomainModel;
using LoanApplication.TacticalDdd.DomainModel.Ddd;

namespace LoanApplication.TacticalDdd.Application
{
    public class LoanApplicationSubmissionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoanApplicationRepository _loanApplicationRepository;
        private readonly IOperatorRepository _operatorRepository;

        public LoanApplicationSubmissionService(IUnitOfWork unitOfWork,
            ILoanApplicationRepository loanApplicationRepository,
            IOperatorRepository operatorRepository)
        {
            _unitOfWork = unitOfWork;
            _loanApplicationRepository = loanApplicationRepository;
            _operatorRepository = operatorRepository;
        }

        public string SubmitLoanApplication(LoanApplicationDto loanApplicationDto)
        {
            var user = _operatorRepository.WithLogin(Login.Of("admin"));

            var application = new DomainModel.LoanApplication
            (
                LoanApplicationNumber.NewNumber(),
                new Customer
                (
                    new NationalIdentifier(loanApplicationDto.CustomerNationalIdentifier),
                    new Name(loanApplicationDto.CustomerFirstName, loanApplicationDto.CustomerLastName),
                    loanApplicationDto.CustomerBirthdate,
                    new MonetaryAmount(loanApplicationDto.CustomerMonthlyIncome),
                    new Address
                    (
                        loanApplicationDto.CustomerAddress.Country,
                        loanApplicationDto.CustomerAddress.ZipCode,
                        loanApplicationDto.CustomerAddress.City,
                        loanApplicationDto.CustomerAddress.Street
                    )
                ),
                new Property
                (
                    new MonetaryAmount(loanApplicationDto.PropertyValue),
                    new Address
                    (
                        loanApplicationDto.PropertyAddress.Country,
                        loanApplicationDto.PropertyAddress.ZipCode,
                        loanApplicationDto.PropertyAddress.City,
                        loanApplicationDto.PropertyAddress.Street
                    )
                ),
                new Loan
                (
                    new MonetaryAmount(loanApplicationDto.LoanAmount),
                    loanApplicationDto.LoanNumberOfYears,
                    new Percent(loanApplicationDto.InterestRate)
                ),
                user
            );

            _loanApplicationRepository.Add(application);

            _unitOfWork.CommitChanges();

            return application.Number;
        }
    }
}