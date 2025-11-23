using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Solidaridad.Application.Models;
using Solidaridad.Application.Models.LoanRepayment;
using Solidaridad.Application.Services;
using Solidaridad.Core.Entities;

namespace Solidaridad.API.Controllers;

[Authorize]
public class LoanRepaymentsController : ApiController
{
    #region DI
    private readonly ILoanRepaymentService _loanRepaymentService;
    private readonly IMapper _mapper;
    public LoanRepaymentsController(ILoanRepaymentService loanRepaymentService, IMapper mapper)
    {
        _loanRepaymentService = loanRepaymentService;
        _mapper = mapper;
    }
    #endregion

    [HttpPost]
    public async Task<IActionResult> Add(CreateLoanRepaymentModel createLoanRepaymentModel)
    {
        return Ok(ApiResult<CreateLoanRepaymentResponseModel>.Success(
            await _loanRepaymentService.CreateAsync(createLoanRepaymentModel)));
    }

    [HttpGet("{loanApplicationId:guid}")]
    public async Task<IActionResult> GetAll(Guid loanApplicationId)
    {
        return Ok(ApiResult<List<LoanRepaymentResponseModel>>.Success(
             _loanRepaymentService.GetRepaymentHistory(loanApplicationId)));
    }

    [AllowAnonymous]
    [HttpPost("GenerateStatement/{loanApplicationId:guid}")]
    public async Task<IActionResult> GenerateLoanStatement(Guid loanApplicationId)
    {
        return Ok(ApiResult<List<Guid>>.Success(
             await _loanRepaymentService.GenerateLoanStatement(loanApplicationId)));
    }



    [HttpGet("GetApplicationStatements/{loanApplicationId:guid}")]
    public async Task<IActionResult> GetApplicationStatements(Guid loanApplicationId)
    {
        return Ok(ApiResult<Task<List<LoanStatementResponseModel>>>.Success(
             _loanRepaymentService.GetApplicationStatementHistory(loanApplicationId)));
    }

    [HttpPut("apply-payment/{loanApplicationId:guid}")]
    public async Task<IActionResult> ApplyPayment(Guid loanApplicationId, ReceivedPaymentModel receivedPayment)
    {
        _loanRepaymentService.ApplyPayment(loanApplicationId, receivedPayment.PaymentReceived, "", "");

        return Ok(ApiResult<bool>.Success(true));
    }

    [HttpGet("GeneratePdf/{loanApplicationId}")]
    public async Task<IActionResult> GetLoanStatementPdf(Guid loanApplicationId)
    {
        var pdfBytes = await _loanRepaymentService.GenerateLoanStatementPdf(loanApplicationId);

        if (pdfBytes == null || pdfBytes.Length == 0)
            return NotFound("Failed to generate PDF.");

        return File(pdfBytes, "application/pdf", "LoanStatement.pdf");
    }

    //[AllowAnonymous]
    //[HttpGet("SimulateTransaction")]
    //public async Task<IActionResult> SimulateTransaction()
    //{
    //    return Ok(ApiResult<List<LoanTransaction>>.Success(await
    //         _loanRepaymentService.SimulateTransaction()));
    //}
}
