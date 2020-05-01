using System.Collections.Generic;
using LoanApplication.TacticalDdd.Application;
using LoanApplication.TacticalDdd.Application.Api;
using LoanApplication.TacticalDdd.ReadModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LoanApplication.TacticalDdd.PortsAdapters.WebApi
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class LoanApplicationController : ControllerBase
    {
        private readonly LoanApplicationSubmissionService _loanApplicationSubmissionService;
        private readonly LoanApplicationEvaluationService _loanApplicationEvaluationService;
        private readonly LoanApplicationDecisionService _loanApplicationDecisionService;
        private readonly LoanApplicationFinder _loanApplicationFinder;

        public LoanApplicationController(
            LoanApplicationSubmissionService loanApplicationSubmissionService,
            LoanApplicationEvaluationService loanApplicationEvaluationService,
            LoanApplicationFinder loanApplicationFinder, 
            LoanApplicationDecisionService loanApplicationDecisionService)
        {
            _loanApplicationSubmissionService = loanApplicationSubmissionService;
            _loanApplicationEvaluationService = loanApplicationEvaluationService;
            _loanApplicationFinder = loanApplicationFinder;
            _loanApplicationDecisionService = loanApplicationDecisionService;
        }
        
        [HttpPost]
        public string Create([FromBody] LoanApplicationDto loanApplicationDto)
        {
            var newApplicationNumber = _loanApplicationSubmissionService.SubmitLoanApplication(loanApplicationDto);
            return newApplicationNumber;
        }
        
        [HttpPost("evaluate/{applicationNumber}")]
        public IActionResult Evaluate([FromRoute] string applicationNumber)
        {
            _loanApplicationEvaluationService.EvaluateLoanApplication(applicationNumber);
            return Ok();
        }
        
        [HttpPost("accept/{applicationNumber}")]
        public IActionResult Accept([FromRoute] string applicationNumber)
        {
            _loanApplicationDecisionService.AcceptApplication(applicationNumber,User);
            return Ok();
        }
        
        [HttpPost("reject/{applicationNumber}")]
        public IActionResult Reject([FromRoute] string applicationNumber)
        {
            _loanApplicationDecisionService.RejectApplication(applicationNumber,User, null);
            return Ok();
        }
        
        [HttpGet("{applicationNumber}")]
        public LoanApplicationDto Get([FromRoute] string applicationNumber)
        {
            return _loanApplicationFinder.GetLoanApplication(applicationNumber);
        }
        
        [HttpPost("find")]
        public IList<LoanApplicationInfoDto> Find([FromBody] LoanApplicationSearchCriteriaDto criteria)
        {
            return _loanApplicationFinder.FindLoadApplication(criteria);
        }
    }
}