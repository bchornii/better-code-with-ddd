using System.Collections.Generic;

namespace LoanApplication.TacticalDdd.DomainModel
{
    public class ScoringRulesFactory
    {
        private readonly IDebtorRegistry _debtorRegistry;

        public ScoringRulesFactory(IDebtorRegistry debtorRegistry)
        {
            _debtorRegistry = debtorRegistry;
        }
        
        public ScoringRulesContext DefaultSet => new ScoringRulesContext(new List<IScoringRule>
        {
            new LoanAmountMustBeLowerThanPropertyValue(),
            new CustomerAgeAtTheDateOfLastInstallmentMustBeBelow65(),
            new InstallmentAmountMustBeLowerThen15PercentOfCustomerIncome(),
            //new CustomerIsNotARegisteredDebtor(_debtorRegistry)
        });
    }
}