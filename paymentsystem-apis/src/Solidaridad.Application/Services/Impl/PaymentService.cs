using AutoMapper;
using Microsoft.AspNetCore.Http;
using NPOI.SS.Formula.Functions;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using Solidaridad.Application.Helpers;
using Solidaridad.Application.Models.ExcelImport;
using Solidaridad.Application.Models.PaymentBatch;
using Solidaridad.Core.Entities.Excel.Import;
using Solidaridad.Core.Entities.Loans;
using Solidaridad.Core.Entities.Payments;
using Solidaridad.DataAccess.Repositories;
using Solidaridad.DataAccess.Repositories.Impl;
using Solidaridad.Shared.Services;
using System.Collections.Generic;

namespace Solidaridad.Application.Services.Impl;

public class PaymentService : IPaymentService
{
    #region DI
    private readonly IMapper _mapper;

    private readonly ApiService _apiService;
    private readonly IClaimService _claimService;
    private readonly IPaymentRequestDeductibleRepository _paymentRequestDeductibleRepository;
    private readonly IFacilitationRepository _failitationRepository;
    private readonly IExcelImportDetailRepository _excelImportDetailRepository;
    private readonly IFarmerRepository _farmerRepository;
    private readonly ILoanBatchRepository _loanBatchRepository;
    private readonly ILoanRepaymentRepository _loanRepaymentRepository;
    private readonly ILoanApplicationRepository _loanApplicationRepository;
    private readonly IPaymenBatchLoanBatchMappingRepository _paymenBatchLoanBatchMapping;
    private readonly ILoanInterestRepository _loanInterestRepository;
    private readonly IPaymentBatchRepository _paymentBatchRepository;
    private readonly ILoanApplicationService _loanApplicationService;
    public readonly ILoanStatementRepository _loanStatementRepository;
    private readonly IEMIScheduleRepository _emiScheduleRepository;
    private readonly ILoanRepaymentService _loanRepaymentService;
    private readonly IPaymentBatchProjectMappingRepository _paymentBatchProjectMappingRepository;

    const string DISBURSED = "e24d24a8-fc69-4527-a92a-97f6648a43c5";

    public PaymentService(IMapper mapper, IPaymentBatchProjectMappingRepository paymentBatchProjectMappingRepository,
        ApiService apiService,
        IClaimService claimService,
        ILoanApplicationService loanApplicationService,
        IPaymentRequestDeductibleRepository paymentRequestDeductibleRepository,
        ILoanRepaymentService loanRepaymentService,
        IExcelImportDetailRepository excelImportDetailRepository,
        ILoanBatchRepository loanBatchRepository, IEMIScheduleRepository emiScheduleRepository,
    IFarmerRepository farmerRepository,
        IFacilitationRepository failitationRepository,
        ILoanRepaymentRepository loanRepaymentRepository,
        ILoanApplicationRepository loanApplicationRepository,
        ILoanInterestRepository loanInterestRepository,
        IPaymenBatchLoanBatchMappingRepository paymenBatchLoanBatchMapping, ILoanStatementRepository loanStatementRepository,
        IPaymentBatchRepository paymentBatchRepository)
    {
        _mapper = mapper;
        _apiService = apiService;
        _claimService = claimService;
        _loanApplicationService = loanApplicationService;
        _paymentRequestDeductibleRepository = paymentRequestDeductibleRepository;
        _excelImportDetailRepository = excelImportDetailRepository;
        _farmerRepository = farmerRepository;
        _failitationRepository = failitationRepository;
        _loanBatchRepository = loanBatchRepository;
        _loanRepaymentRepository = loanRepaymentRepository;
        _loanApplicationRepository = loanApplicationRepository;
        _paymenBatchLoanBatchMapping = paymenBatchLoanBatchMapping;
        _loanInterestRepository = loanInterestRepository;
        _paymentBatchRepository = paymentBatchRepository;
        _loanStatementRepository = loanStatementRepository;
        _emiScheduleRepository = emiScheduleRepository;
        _loanRepaymentService = loanRepaymentService;
        _paymentBatchProjectMappingRepository = paymentBatchProjectMappingRepository;

    }
    #endregion

    public async Task ImportPaymentRequestDeductible(IFormFile file, Guid? id, Guid paymentBatchId)
    {
        try
        {
            var paymenBatchLoanBatchMapping = await _paymenBatchLoanBatchMapping.GetAllAsync(x => x.PaymentBatchId == paymentBatchId);

            var loanBatch = new List<LoanBatch>();
            var loanApplication = new List<LoanApplication>();
            var effectiveLoanBatch = new LoanBatch();


           

                XSSFWorkbook hssfwb;
            using (var stream = file.OpenReadStream())
            {
                hssfwb = new XSSFWorkbook(stream);
            }

            ISheet sheet = hssfwb.GetSheet(Convert.ToString(ExelImportConstants.SHEET_PAYMENT_LIST));
            var _listPaymentsExcel = new List<ImportPaymentBatchModel>();
            var _listPayments = new List<PaymentRequestDeductible>();
            var excelImportDetails = new List<ExcelImportDetail>();
            var _errorDetails = new List<ExcelImportDetail>();
            // check if sheet exists
            if (sheet == null)
            {
                _errorDetails.Add(new ExcelImportDetail
                {
                    Id = Guid.NewGuid(),
                    ExcelImportId = id ?? Guid.Empty,
                    TabName = Convert.ToString(ExelImportConstants.SHEET_FARMER),
                    RowNumber = 0,
                    IsSuccess = false,
                    Remarks = "Sheet 'Farmers List' is missing in the uploaded file"
                });
            }
            if (sheet != null)
            {
                DataFormatter formatter = new DataFormatter();
                var headerRow = sheet.GetRow(0); // Assuming the first row (index 0) is the header
                int statusIdColumnIndex = -1;
                Guid rowId = new Guid();

                // Check if "StatusId" column exists
                for (int cellIndex = 0; cellIndex < headerRow.LastCellNum; cellIndex++)
                {
                    if (formatter.FormatCellValue(headerRow.GetCell(cellIndex)).Trim().Equals("Id", StringComparison.OrdinalIgnoreCase))
                    {
                        statusIdColumnIndex = cellIndex; // Store the column index
                        break;
                    }
                }

                for (int row = 1; row <= sheet.LastRowNum; row++)
                {
                    var currentRow = sheet.GetRow(row);
                    if (currentRow == null || IsRowEmpty(currentRow))
                        continue;
                    if (sheet.GetRow(row) != null)
                    {
                        var totalUnitEarningsEurCell = sheet.GetRow(row).GetCell(4);
                        var totalUnitEarningsLcCell = sheet.GetRow(row).GetCell(5);
                        var partnerShareCell = sheet.GetRow(row).GetCell(6);
                        var farmerShareCell = sheet.GetRow(row).GetCell(7);

                        decimal? totalUnitEarningsEur = null;
                        decimal? totalUnitEarningsLc = null;
                        decimal? partnerShare = 0;
                        decimal? farmerShare = null;

                        var formulaEvaluator = new XSSFFormulaEvaluator(sheet.Workbook);

                        #region Commented Code

                        // Total Units Earning EUR
                        if (totalUnitEarningsEurCell != null)
                        {
                            if (totalUnitEarningsEurCell.CellType == CellType.Formula)
                            {
                                // Evaluate the formula and get the calculated value
                                var evaluatedCell = formulaEvaluator.Evaluate(totalUnitEarningsEurCell);

                                // Get the evaluated value as a string and parse it as a decimal
                                if (evaluatedCell.CellType == CellType.Numeric)
                                {
                                    totalUnitEarningsEur = (decimal)evaluatedCell.NumberValue;
                                }
                                else
                                {
                                    totalUnitEarningsEur = ParseUtility.ParseNullableDecimalValue(formatter.FormatCellValue(totalUnitEarningsEurCell));
                                }
                            }
                            else
                            {
                                // If the cell is not a formula, directly parse the value
                                totalUnitEarningsEur = ParseUtility.ParseNullableDecimalValue(formatter.FormatCellValue(totalUnitEarningsEurCell));
                            }
                        }

                        // Total Units Earning LC
                        if (totalUnitEarningsLcCell != null)
                        {
                            if (totalUnitEarningsLcCell.CellType == CellType.Formula)
                            {
                                // Evaluate the formula and get the calculated value
                                var evaluatedCell = formulaEvaluator.Evaluate(totalUnitEarningsLcCell);

                                // Get the evaluated value as a string and parse it as a decimal
                                if (evaluatedCell.CellType == CellType.Numeric)
                                {
                                    totalUnitEarningsLc = (decimal)evaluatedCell.NumberValue;
                                }
                                else
                                {
                                    totalUnitEarningsLc = ParseUtility.ParseNullableDecimalValue(formatter.FormatCellValue(totalUnitEarningsLcCell));
                                }
                            }
                            else
                            {
                                // If the cell is not a formula, directly parse the value
                                totalUnitEarningsLc = ParseUtility.ParseNullableDecimalValue(formatter.FormatCellValue(totalUnitEarningsLcCell));
                            }
                        }

                        // Partners Adminstrative Cost
                        //if (partnerShareCell != null)
                        //{
                        //    if (partnerShareCell.CellType == CellType.Formula)
                        //    {
                        //        // Evaluate the formula and get the calculated value
                        //        var evaluatedCell = formulaEvaluator.Evaluate(partnerShareCell);

                        //        // Get the evaluated value as a string and parse it as a decimal
                        //        if (evaluatedCell.CellType == CellType.Numeric)
                        //        {
                        //            partnerShare = (decimal)evaluatedCell.NumberValue;
                        //        }
                        //        else
                        //        {
                        //            partnerShare = ParseUtility.ParseNullableDecimalValue(formatter.FormatCellValue(partnerShareCell));
                        //        }
                        //    }
                        //    else
                        //    {
                        //        // If the cell is not a formula, directly parse the value
                        //        partnerShare = ParseUtility.ParseNullableDecimalValue(formatter.FormatCellValue(partnerShareCell));
                        //    }
                        //}

                        //Farmer Earnings Share LC
                        if (farmerShareCell != null)
                        {
                            if (farmerShareCell.CellType == CellType.Formula)
                            {
                                // Evaluate the formula and get the calculated value
                                var evaluatedCell = formulaEvaluator.Evaluate(farmerShareCell);

                                // Get the evaluated value as a string and parse it as a decimal
                                if (evaluatedCell.CellType == CellType.Numeric)
                                {
                                    farmerShare = (decimal)evaluatedCell.NumberValue;
                                }
                                else
                                {
                                    farmerShare = ParseUtility.ParseNullableDecimalValue(formatter.FormatCellValue(farmerShareCell));
                                }
                            }
                            else
                            {
                                // If the cell is not a formula, directly parse the value
                                farmerShare = ParseUtility.ParseNullableDecimalValue(formatter.FormatCellValue(farmerShareCell));
                            }
                        }

                        #endregion

                        _listPaymentsExcel.Add(new ImportPaymentBatchModel
                        {
                            SystemId = ParseUtility.ParseAnyValueToString(formatter.FormatCellValue(sheet.GetRow(row).GetCell(0))), // Uwanjani System ID	
                            BeneficiaryId = ParseUtility.ParseAnyValueToString(formatter.FormatCellValue(sheet.GetRow(row).GetCell(1))), // Beneficiary Farmer Card ID	
                            CarbonUnitsAccured = ParseUtility.ParseNullableDecimalValue(formatter.FormatCellValue(sheet.GetRow(row).GetCell(2))), // Carbon Units Acrued	
                            UnitCostEur = ParseUtility.ParseNullableDecimalValue(formatter.FormatCellValue(sheet.GetRow(row).GetCell(3))), // Unit Cost(EUR)	
                            TotalUnitsEarningEur = totalUnitEarningsEur, //ParseUtility.ParseNullableDecimalValue(formatter.FormatCellValue(sheet.GetRow(row).GetCell(4))), // Total Units Earning (EUR)	
                            TotalUnitsEarningLc = totalUnitEarningsLc, //ParseUtility.ParseNullableDecimalValue(formatter.FormatCellValue(sheet.GetRow(row).GetCell(5))), // Total Units Earning (LC)	
                            SolidaridadEarningsShare = 0, //ParseUtility.ParseNullableDecimalValue(formatter.FormatCellValue(sheet.GetRow(row).GetCell(6))), // Solidaridad Earnings Share	
                            //FarmerEarningsShareEur = ParseUtility.ParseNullableDecimalValue(formatter.FormatCellValue(sheet.GetRow(row).GetCell(7))), // Farmer Earnings Share (EUR)	
                            FarmerEarningsShareLc = farmerShare, //ParseUtility.ParseNullableDecimalValue(formatter.FormatCellValue(sheet.GetRow(row).GetCell(7))),  // Farmer Earnings Share (LC)	
                            //FarmerPayableEarningsLc = ParseUtility.ParseNullableDecimalValue(formatter.FormatCellValue(sheet.GetRow(row).GetCell(9))), // Farmer Payable Earnings (LC)	
                            //FarmerLoansDeductionsLc = ParseUtility.ParseNullableDecimalValue(formatter.FormatCellValue(sheet.GetRow(row).GetCell(10))), // Farmer Loans Deductions (LC)	
                            //FarmerLoansBalanceLc = ParseUtility.ParseNullableDecimalValue(formatter.FormatCellValue(sheet.GetRow(row).GetCell(11))), // Farmer Loans Balance (LC)
                        });
                    }
                }

                var farmers = await _farmerRepository.GetAllAsync(c => true);

                if (paymenBatchLoanBatchMapping != null && paymenBatchLoanBatchMapping.Count > 0)
                {
                    loanBatch = await _loanBatchRepository.GetAllAsync(x => x.Id == paymenBatchLoanBatchMapping.FirstOrDefault().LoanBatchId);

                    loanApplication = await _loanApplicationService.GetEffectiveLoanApplications(loanBatch.FirstOrDefault().Id);

                    //loanApplication = await _loanApplicationRepository.GetAllAsync(x => 
                    //x.LoanBatchId == loanBatch.FirstOrDefault().Id
                    //&& x.Status == Guid.Parse(DISBURSED)); // disbursed only

                    effectiveLoanBatch = loanBatch.FirstOrDefault();
                    var effectiveDate = effectiveLoanBatch.EffectiveDate.AddMonths((int)effectiveLoanBatch.GracePeriod);
                }
                else
                {
                    
                    var tempApplication = await _loanApplicationRepository.GetAllAsync(x => true);

                    foreach (var importFarmerModel in _listPaymentsExcel)
                    {
                        var selectedFarmer = farmers.FirstOrDefault(c => c.SystemId == importFarmerModel.SystemId);

                        if (selectedFarmer == null)
                            continue; // skip if no matching farmer

                        var oldestApplication = tempApplication
                            .Where(app => app.FarmerId == selectedFarmer.Id && app.Status == Guid.Parse(DISBURSED))
                            .OrderBy(app => app.DisbursementDate) 
                            .FirstOrDefault();

                        if (oldestApplication != null)
                        {
                            loanApplication.Add(oldestApplication);
                        }
                    }

                }
                int r = 1;

                // validate phone numbers
                // await _apiService.VerifyMobileAsync<dynamic>(_listPaymentsExcel);

                // after validating farmer pghone numbers, get the updated list
                // this will have the latest validated farmer phone numbers
               

                foreach (var importFarmerModel in _listPaymentsExcel)
                {
                    ++r;

                    var selectedfarmer = farmers.FirstOrDefault(c => c.SystemId == importFarmerModel.SystemId); // from "Beneficiary Farmer Card ID"
                    //if (selectedfarmer == null)
                    //{
                    //    _errorDetails.Add(new ExcelImportDetail
                    //    {
                    //        Id = Guid.NewGuid(),
                    //        ExcelImportId = id ?? Guid.Empty,
                    //        TabName = Convert.ToString(ExelImportConstants.SHEET_FARMER),
                    //        RowNumber = 0,
                    //        IsSuccess = false,
                    //        Remarks = $"Farmer with System Id {importFarmerModel.SystemId} does not exist"
                    //    });
                    //}

                    var farmerEarningsLC = Convert.ToDecimal(importFarmerModel.FarmerEarningsShareLc);

                    if (paymenBatchLoanBatchMapping != null
                        && paymenBatchLoanBatchMapping.Count > 0
                        && selectedfarmer != null
                        && loanApplication != null)
                    {
                        var loan = loanApplication.FirstOrDefault(c =>  c.FarmerId == selectedfarmer.Id);
                        decimal loanBalance = 0;
                        decimal maxDeductiblePercent = 0;
                        if (loan != null)
                        {
                            var allStatements = await _loanStatementRepository.GetAllAsync(x => x.ApplicationId == loan.Id);

                            // If statements not found, generate them and refetch
                            if (allStatements == null || !allStatements.Any())
                            {
                                await _loanRepaymentService.GenerateLoanStatement(loan.Id);
                                allStatements = await _loanStatementRepository.GetAllAsync(x => x.ApplicationId == loan.Id);
                            }

                            var lastStatement = allStatements
                                .OrderByDescending(x => x.StatementDate)
                                .FirstOrDefault();

                            loanBalance = lastStatement?.LoanBalance ?? 0;
                            maxDeductiblePercent = effectiveLoanBatch?.MaxDeductiblePercent ?? 0m;
                        }
                        // Calculate the max deduction allowed based on % rule
                        decimal maxDeductionAllowed = farmerEarningsLC * maxDeductiblePercent / 100m;

                        // Final deduction should not exceed balance, earnings, or max allowed
                        decimal deduction = Math.Min(loanBalance, Math.Min(farmerEarningsLC, maxDeductionAllowed));

                        decimal payable = farmerEarningsLC - deduction;
                        decimal remainingLoanBalance = loanBalance - deduction;

                        importFarmerModel.FarmerLoansDeductionsLc = deduction;
                        importFarmerModel.FarmerPayableEarningsLc = payable;
                        importFarmerModel.FarmerLoansBalanceLc = remainingLoanBalance;
                    }
                    else
                    {
                        importFarmerModel.FarmerPayableEarningsLc = farmerEarningsLC;
                    }

                    // Optionally: update loan balance in DB
                    //loan.Balance = remainingLoanBalance;
                    //await _loanRepository.UpdateAsync(loan);

                    var importEntity = new PaymentRequestDeductible
                    {
                        SystemId = importFarmerModel.SystemId,
                        BeneficiaryId = importFarmerModel.BeneficiaryId,
                        CarbonUnitsAccured = importFarmerModel.CarbonUnitsAccured ?? 0,
                        UnitCostEur = importFarmerModel.UnitCostEur ?? 0,
                        TotalUnitsEarningLc = importFarmerModel.TotalUnitsEarningLc ?? 0,
                        SolidaridadEarningsShare = 0,
                        FarmerEarningsShareEur = importFarmerModel.FarmerEarningsShareEur ?? 0,
                        FarmerEarningsShareLc = importFarmerModel.FarmerEarningsShareLc ?? 0,
                        FarmerPayableEarningsLc = importFarmerModel.FarmerPayableEarningsLc ?? 0,
                        FarmerLoansDeductionsLc = importFarmerModel.FarmerLoansDeductionsLc ?? 0,
                        FarmerLoansBalanceLc = importFarmerModel.FarmerLoansBalanceLc ?? 0,
                        PaymentBatchId = paymentBatchId,
                        ExcelImportId = (Guid)id,
                        CreatedOn = DateTime.UtcNow,
                        CreatedBy = Guid.Parse(_claimService.GetUserId()),
                        LoanAccountNo = loanApplication.FirstOrDefault(x=> x.FarmerId == selectedfarmer.Id).LoanNumber ?? ""
                    };
                    if (statusIdColumnIndex != -1)
                    {
                        IRow firstDataRow = sheet.GetRow(r - 1);
                        if (firstDataRow != null)
                        {
                            string rowIdString = ParseUtility.ParseAnyValueToString(formatter.FormatCellValue(firstDataRow.GetCell(statusIdColumnIndex)));
                            if (Guid.TryParse(rowIdString, out Guid rowIdGuid))
                            {
                                rowId = rowIdGuid;
                                importEntity.Id = rowId;
                            }
                            else
                            {
                                throw new FormatException($"The value '{rowIdString}' is not a valid GUID.");
                            }
                        }
                    }

                    var message = "";
                    if (importFarmerModel != null)
                    {
                        var errorMessages = new List<string>();

                        if (!farmers.Any(f => f.SystemId == importFarmerModel.SystemId))
                        {
                            errorMessages.Add("Farmer with the provided System Id does not exist");
                        }
                        else if (!farmers.Any(f => f.SystemId == importFarmerModel.SystemId && f.IsFarmerVerified == true))
                        {
                            errorMessages.Add("Farmer mobile number is not verified");
                        }
                        else if (!farmers.Any(f => f.ParticipantId == importFarmerModel.BeneficiaryId))
                        {
                            errorMessages.Add("Farmer with the provided BeneficiaryId id does not exist");
                        }
                        else if (importFarmerModel.CarbonUnitsAccured == null || importFarmerModel.CarbonUnitsAccured == 0)
                        {
                            errorMessages.Add("Carbon Units Accured not provided");
                        }
                        else if (importFarmerModel.UnitCostEur == null || importFarmerModel.UnitCostEur == 0)
                        {
                            errorMessages.Add("Unit Cost (EUR) not provided or zero");
                        }
                        else if (importFarmerModel.TotalUnitsEarningLc == null || importFarmerModel.TotalUnitsEarningLc == 0)
                        {
                            errorMessages.Add("Total Units Earning (LC) not provided or zero");
                        }

                        //else if (loanApplication.FirstOrDefault(c => c.LoanBatchId == effectiveLoanBatch.Id && c.FarmerId == selectedfarmer.Id && c.Status == new Guid("e24d24a8-fc69-4527-a92a-97f6648a43c5")) == null)
                        //{
                        //    errorMessages.Add("Loan application is not dispersed ");
                        //}

                        else if (importFarmerModel.FarmerEarningsShareLc == null || importFarmerModel.FarmerEarningsShareLc == 0)
                        {
                            errorMessages.Add("Farmer Earnings Share (LC) not provided or zero");
                        }
                        //else if (importFarmerModel.FarmerPayableEarningsLc == null || importFarmerModel.FarmerPayableEarningsLc == 0)
                        //{
                        //    errorMessages.Add("Farmer Payable Earnings (LC) not provided or zero");
                        //}
                        //else if (importFarmerModel.FarmerLoansDeductionsLc == null || importFarmerModel.FarmerLoansDeductionsLc == 0)
                        //{
                        //    errorMessages.Add("Farmer Loans Deductions (LC) not provided or zero");
                        //}
                        //else if (importFarmerModel.FarmerLoansBalanceLc == null || importFarmerModel.FarmerLoansBalanceLc == 0)
                        //{
                        //    errorMessages.Add("Farmer Loans Balance (LC) not provided or zero");
                        //}

                        if (errorMessages.Any())
                        {
                            message = string.Join(", ", errorMessages);
                            var errorDetails = new ExcelImportDetail
                            {
                                Id = Guid.NewGuid(),
                                ExcelImportId = (Guid)id,
                                TabName = "",
                                RowNumber = r,
                                IsSuccess = false,
                                Remarks = message,
                            };
                            _errorDetails.Add(errorDetails);
                            importEntity.StatusId = 0;
                            importEntity.Remarks = message;
                        }
                        else
                        {
                            importEntity.StatusId = 1;
                            importEntity.Remarks = "";
                        }
                        _listPayments.Add(importEntity);
                    }
                    else
                    {
                        var errorDetails = new ExcelImportDetail
                        {
                            Id = Guid.NewGuid(),
                            ExcelImportId = Guid.NewGuid(),
                            TabName = "",
                            RowNumber = r,
                            IsSuccess = false,
                            Remarks = "Row is empty",
                        };
                        _errorDetails.Add(errorDetails);
                    }
                }

                // if rows exists for same payment batch, mark their status as -1
                var existing = await _paymentRequestDeductibleRepository.GetAllAsync(c => c.PaymentBatchId == paymentBatchId && c.IsDeleted == false);
                if (existing != null)
                {
                    existing.ForEach(c =>
                    {
                        c.StatusId = -1;
                        c.IsDeleted = true;
                    });

                    await _paymentRequestDeductibleRepository.UpdateRange(existing);
                }

                if (_listPayments.Count() > 0)
                {
                    if (statusIdColumnIndex == null)
                    {
                        await _paymentRequestDeductibleRepository.AddRange(_listPayments);
                    }
                    else
                    {
                        await _paymentRequestDeductibleRepository.UpdateRange(_listPayments);
                    }
                }

                if (_errorDetails.Count() > 0)
                    await _excelImportDetailRepository.AddRange(_errorDetails);

                // save import summary
                await _paymentRequestDeductibleRepository.SavePaymentImportSummary(new PaymentImportSummary
                {
                    ExcelImportId = (Guid)id,
                    FailedRows = _errorDetails.Count,
                    PassedRows = _listPaymentsExcel.Count - _errorDetails.Count,
                    TotalRowsInExcel = _listPaymentsExcel.Count,
                });


                // update loans balance
                //var paymenBatchLoanBatchMapping = await _paymenBatchLoanBatchMapping.GetAllAsync(x => x.PaymentBatchId == paymentBatchId);
                //var loanBatch = await _loanBatchRepository.GetAllAsync(x => x.Id == paymenBatchLoanBatchMapping.FirstOrDefault().LoanBatchId);
                //var loanApplication = await _loanApplicationRepository.GetAllAsync(x => x.LoanBatchId == loanBatch.FirstOrDefault().Id);

                //foreach (var item in _listPayments)
                //{
                //    if (item.StatusId == 1)
                //    {
                //        try
                //        {
                //            decimal annualInterestRate = 0;
                //            Guid loanApplicationId = Guid.NewGuid();
                //            var matchingLoanApplication = new LoanApplication();
                //            if (loanBatch != null)
                //            {
                //                annualInterestRate = loanBatch.FirstOrDefault().InterestRate;
                //            }
                //            if (loanApplication != null)
                //            {
                //                matchingLoanApplication = loanApplication?.FirstOrDefault(app => app.Farmer != null && app.Farmer.SystemId == item.SystemId);
                //                if (matchingLoanApplication != null)
                //                {
                //                    loanApplicationId = matchingLoanApplication.Id;
                //                }
                //                else
                //                {
                //                    throw new InvalidOperationException($"No matching LoanApplication found for item with SystemId: {item.SystemId}");
                //                }
                //            }

                //            /* older method*/
                //            //var addedItem = await _loanRepaymentRepository.AddRepayment(
                //            //    loanApplicationId,
                //            //    item.FarmerLoansDeductionsLc,
                //            //    annualInterestRate);

                //            /* newer method*/
                //            //_loanRepaymentRepository.ApplyPayment(
                //            //   loanApplicationId,
                //            //   item.FarmerLoansDeductionsLc);

                //            // decimal monthlyRate = annualInterestRate / 12 / 100;
                //            // decimal monthlyInterest = matchingLoanApplication.PrincipalAmount * monthlyRate;
                //            // matchingLoanApplication.PrincipalAmount = Math.Max(0, matchingLoanApplication.PrincipalAmount + monthlyInterest - item.FarmerLoansDeductionsLc);

                //            //var result =  await _loanApplicationRepository.UpdateAsync(matchingLoanApplication);
                //        }
                //        catch (Exception ex)
                //        {
                //            throw ex;
                //        }
                //    }
                //}
            }

            // update status as excel uploaded
            // update status as excel uploaded
            var stages = await _paymentBatchRepository.GetPaymentApprovalStages();
            string stageName = "Initiated";
            var stageId = stages.Where(c => c.StageText == stageName).FirstOrDefault().Id;

            var payBatch = await _paymentBatchRepository.GetFirstAsync(c => c.Id == paymentBatchId);
            if (payBatch != null)
            {
                payBatch.IsExcelUploaded = true;
                payBatch.StatusId = stageId;
                await _paymentBatchRepository.UpdateAsync(payBatch);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    #region ImportPaymentDeductibleRequest
    public async Task ImportPaymentRequestDeductibleMultiBatch(IFormFile file, Guid? id, Guid paymentBatchId)
    {
        try
        {
            var loanBatch = new List<LoanBatch>();
            var paymenBatchLoanBatchMapping = await _paymenBatchLoanBatchMapping.GetAllAsync(x => x.PaymentBatchId == paymentBatchId);

            if (paymenBatchLoanBatchMapping != null && paymenBatchLoanBatchMapping.Any())
            {
                 loanBatch = await _loanBatchRepository.GetAllAsync(
                    x => x.Id == paymenBatchLoanBatchMapping.FirstOrDefault().LoanBatchId
                );
            }
            
                var loanApplication = new List<LoanApplication>();
                var effectiveLoanBatch = new LoanBatch();
            



            XSSFWorkbook hssfwb;
            using (var stream = file.OpenReadStream())
            {
                hssfwb = new XSSFWorkbook(stream);
            }

            ISheet sheet = hssfwb.GetSheet(Convert.ToString(ExelImportConstants.SHEET_PAYMENT_LIST));
            var _listPaymentsExcel = new List<ImportPaymentBatchModel>();
            var _listPayments = new List<PaymentRequestDeductible>();
            var excelImportDetails = new List<ExcelImportDetail>();
            var _errorDetails = new List<ExcelImportDetail>();
            // check if sheet exists
            if (sheet == null)
            {
                _errorDetails.Add(new ExcelImportDetail
                {
                    Id = Guid.NewGuid(),
                    ExcelImportId = id ?? Guid.Empty,
                    TabName = Convert.ToString(ExelImportConstants.SHEET_FARMER),
                    RowNumber = 0,
                    IsSuccess = false,
                    Remarks = "Sheet 'Farmers List' is missing in the uploaded file"
                });
            }
            if (sheet != null)
            {
                DataFormatter formatter = new DataFormatter();
                var headerRow = sheet.GetRow(0); // Assuming the first row (index 0) is the header
                int statusIdColumnIndex = -1;
                Guid rowId = new Guid();

                // Check if "StatusId" column exists
                for (int cellIndex = 0; cellIndex < headerRow.LastCellNum; cellIndex++)
                {
                    if (formatter.FormatCellValue(headerRow.GetCell(cellIndex)).Trim().Equals("Id", StringComparison.OrdinalIgnoreCase))
                    {
                        statusIdColumnIndex = cellIndex; // Store the column index
                        break;
                    }
                }

                for (int row = 1; row <= sheet.LastRowNum; row++)
                {
                    var currentRow = sheet.GetRow(row);
                    if (currentRow == null || IsRowEmpty(currentRow))
                        continue;
                    if (sheet.GetRow(row) != null)
                    {
                        var totalUnitEarningsEurCell = sheet.GetRow(row).GetCell(4);
                        var totalUnitEarningsLcCell = sheet.GetRow(row).GetCell(5);
                        var partnerShareCell = sheet.GetRow(row).GetCell(6);
                        var farmerShareCell = sheet.GetRow(row).GetCell(7);

                        decimal? totalUnitEarningsEur = null;
                        decimal? totalUnitEarningsLc = null;
                        decimal? partnerShare = 0;
                        decimal? farmerShare = null;

                        var formulaEvaluator = new XSSFFormulaEvaluator(sheet.Workbook);

                        #region Commented Code

                        // Total Units Earning EUR
                        if (totalUnitEarningsEurCell != null)
                        {
                            if (totalUnitEarningsEurCell.CellType == CellType.Formula)
                            {
                                // Evaluate the formula and get the calculated value
                                var evaluatedCell = formulaEvaluator.Evaluate(totalUnitEarningsEurCell);

                                // Get the evaluated value as a string and parse it as a decimal
                                if (evaluatedCell.CellType == CellType.Numeric)
                                {
                                    totalUnitEarningsEur = (decimal)evaluatedCell.NumberValue;
                                }
                                else
                                {
                                    totalUnitEarningsEur = ParseUtility.ParseNullableDecimalValue(formatter.FormatCellValue(totalUnitEarningsEurCell));
                                }
                            }
                            else
                            {
                                // If the cell is not a formula, directly parse the value
                                totalUnitEarningsEur = ParseUtility.ParseNullableDecimalValue(formatter.FormatCellValue(totalUnitEarningsEurCell));
                            }
                        }

                        // Total Units Earning LC
                        if (totalUnitEarningsLcCell != null)
                        {
                            if (totalUnitEarningsLcCell.CellType == CellType.Formula)
                            {
                                // Evaluate the formula and get the calculated value
                                var evaluatedCell = formulaEvaluator.Evaluate(totalUnitEarningsLcCell);

                                // Get the evaluated value as a string and parse it as a decimal
                                if (evaluatedCell.CellType == CellType.Numeric)
                                {
                                    totalUnitEarningsLc = (decimal)evaluatedCell.NumberValue;
                                }
                                else
                                {
                                    totalUnitEarningsLc = ParseUtility.ParseNullableDecimalValue(formatter.FormatCellValue(totalUnitEarningsLcCell));
                                }
                            }
                            else
                            {
                                // If the cell is not a formula, directly parse the value
                                totalUnitEarningsLc = ParseUtility.ParseNullableDecimalValue(formatter.FormatCellValue(totalUnitEarningsLcCell));
                            }
                        }

                        // Partners Adminstrative Cost
                        //if (partnerShareCell != null)
                        //{
                        //    if (partnerShareCell.CellType == CellType.Formula)
                        //    {
                        //        // Evaluate the formula and get the calculated value
                        //        var evaluatedCell = formulaEvaluator.Evaluate(partnerShareCell);

                        //        // Get the evaluated value as a string and parse it as a decimal
                        //        if (evaluatedCell.CellType == CellType.Numeric)
                        //        {
                        //            partnerShare = (decimal)evaluatedCell.NumberValue;
                        //        }
                        //        else
                        //        {
                        //            partnerShare = ParseUtility.ParseNullableDecimalValue(formatter.FormatCellValue(partnerShareCell));
                        //        }
                        //    }
                        //    else
                        //    {
                        //        // If the cell is not a formula, directly parse the value
                        //        partnerShare = ParseUtility.ParseNullableDecimalValue(formatter.FormatCellValue(partnerShareCell));
                        //    }
                        //}

                        //Farmer Earnings Share LC
                        if (farmerShareCell != null)
                        {
                            if (farmerShareCell.CellType == CellType.Formula)
                            {
                                // Evaluate the formula and get the calculated value
                                var evaluatedCell = formulaEvaluator.Evaluate(farmerShareCell);

                                // Get the evaluated value as a string and parse it as a decimal
                                if (evaluatedCell.CellType == CellType.Numeric)
                                {
                                    farmerShare = (decimal)evaluatedCell.NumberValue;
                                }
                                else
                                {
                                    farmerShare = ParseUtility.ParseNullableDecimalValue(formatter.FormatCellValue(farmerShareCell));
                                }
                            }
                            else
                            {
                                // If the cell is not a formula, directly parse the value
                                farmerShare = ParseUtility.ParseNullableDecimalValue(formatter.FormatCellValue(farmerShareCell));
                            }
                        }

                        #endregion

                        _listPaymentsExcel.Add(new ImportPaymentBatchModel
                        {
                            SystemId = ParseUtility.ParseAnyValueToString(formatter.FormatCellValue(sheet.GetRow(row).GetCell(0))), // Uwanjani System ID	
                            BeneficiaryId = ParseUtility.ParseAnyValueToString(formatter.FormatCellValue(sheet.GetRow(row).GetCell(1))), // Beneficiary Farmer Card ID	
                            CarbonUnitsAccured = ParseUtility.ParseNullableDecimalValue(formatter.FormatCellValue(sheet.GetRow(row).GetCell(2))), // Carbon Units Acrued	
                            UnitCostEur = ParseUtility.ParseNullableDecimalValue(formatter.FormatCellValue(sheet.GetRow(row).GetCell(3))), // Unit Cost(EUR)	
                            TotalUnitsEarningEur = totalUnitEarningsEur, //ParseUtility.ParseNullableDecimalValue(formatter.FormatCellValue(sheet.GetRow(row).GetCell(4))), // Total Units Earning (EUR)	
                            TotalUnitsEarningLc = totalUnitEarningsLc, //ParseUtility.ParseNullableDecimalValue(formatter.FormatCellValue(sheet.GetRow(row).GetCell(5))), // Total Units Earning (LC)	
                            SolidaridadEarningsShare = 0, //ParseUtility.ParseNullableDecimalValue(formatter.FormatCellValue(sheet.GetRow(row).GetCell(6))), // Solidaridad Earnings Share	
                            //FarmerEarningsShareEur = ParseUtility.ParseNullableDecimalValue(formatter.FormatCellValue(sheet.GetRow(row).GetCell(7))), // Farmer Earnings Share (EUR)	
                            FarmerEarningsShareLc = farmerShare, //ParseUtility.ParseNullableDecimalValue(formatter.FormatCellValue(sheet.GetRow(row).GetCell(7))),  // Farmer Earnings Share (LC)	
                            //FarmerPayableEarningsLc = ParseUtility.ParseNullableDecimalValue(formatter.FormatCellValue(sheet.GetRow(row).GetCell(9))), // Farmer Payable Earnings (LC)	
                            //FarmerLoansDeductionsLc = ParseUtility.ParseNullableDecimalValue(formatter.FormatCellValue(sheet.GetRow(row).GetCell(10))), // Farmer Loans Deductions (LC)	
                            //FarmerLoansBalanceLc = ParseUtility.ParseNullableDecimalValue(formatter.FormatCellValue(sheet.GetRow(row).GetCell(11))), // Farmer Loans Balance (LC)
                        });
                    }
                }

                var farmers = await _farmerRepository.GetAllAsync(c => true);

                if (paymenBatchLoanBatchMapping != null && paymenBatchLoanBatchMapping.Count > 0)
                {
                   

                    loanApplication = await _loanApplicationService.GetEffectiveLoanApplications(loanBatch.FirstOrDefault().Id);

                    //loanApplication = await _loanApplicationRepository.GetAllAsync(x => 
                    //x.LoanBatchId == loanBatch.FirstOrDefault().Id
                    //&& x.Status == Guid.Parse(DISBURSED)); // disbursed only

                    effectiveLoanBatch = loanBatch.FirstOrDefault();
                    var effectiveDate = effectiveLoanBatch.EffectiveDate.AddMonths((int)effectiveLoanBatch.GracePeriod);
                }
              
                int r = 1;

                // validate phone numbers
                // await _apiService.VerifyMobileAsync<dynamic>(_listPaymentsExcel);

                // after validating farmer pghone numbers, get the updated list
                // this will have the latest validated farmer phone numbers


                foreach (var importFarmerModel in _listPaymentsExcel)
                {
                    ++r;

                    var selectedfarmer = farmers.FirstOrDefault(c => c.SystemId == importFarmerModel.SystemId); // from "Beneficiary Farmer Card ID"
                    //if (selectedfarmer == null)
                    //{
                    //    _errorDetails.Add(new ExcelImportDetail
                    //    {
                    //        Id = Guid.NewGuid(),
                    //        ExcelImportId = id ?? Guid.Empty,
                    //        TabName = Convert.ToString(ExelImportConstants.SHEET_FARMER),
                    //        RowNumber = 0,
                    //        IsSuccess = false,
                    //        Remarks = $"Farmer with System Id {importFarmerModel.SystemId} does not exist"
                    //    });
                    //}

                    var farmerEarningsLC = Convert.ToDecimal(importFarmerModel.FarmerEarningsShareLc);

                    if (paymenBatchLoanBatchMapping != null
                        
                        && selectedfarmer != null
                        && loanApplication != null)
                    {
                        var loan = loanApplication.FirstOrDefault(c => c.FarmerId == selectedfarmer.Id);
                       
                        if (loan != null)
                        {
                            effectiveLoanBatch = loanBatch.FirstOrDefault(c => c.Id == loan.LoanBatchId);
                            decimal loanBalance = 0;
                            decimal maxDeductiblePercent = 0;
                            var allStatements = await _loanStatementRepository.GetAllAsync(x => x.ApplicationId == loan.Id);

                            // If statements not found, generate them and refetch
                            if (allStatements == null || !allStatements.Any())
                            {
                                await _loanRepaymentService.GenerateLoanStatement(loan.Id);
                                allStatements = await _loanStatementRepository.GetAllAsync(x => x.ApplicationId == loan.Id);
                            }

                            var lastStatement = allStatements
                                .OrderByDescending(x => x.StatementDate)
                                .FirstOrDefault();

                            loanBalance = lastStatement?.LoanBalance ?? 0;
                            maxDeductiblePercent = effectiveLoanBatch?.MaxDeductiblePercent ?? 0m;

                            decimal maxDeductionAllowed = farmerEarningsLC * maxDeductiblePercent / 100m;

                            // Final deduction should not exceed balance, earnings, or max allowed
                            decimal deduction = Math.Min(loanBalance, Math.Min(farmerEarningsLC, maxDeductionAllowed));

                            decimal payable = farmerEarningsLC - deduction;
                            decimal remainingLoanBalance = loanBalance - deduction;

                            importFarmerModel.FarmerLoansDeductionsLc = deduction;
                            importFarmerModel.FarmerPayableEarningsLc = payable;
                            importFarmerModel.FarmerLoansBalanceLc = remainingLoanBalance;
                        }
                        else
                        {
                            importFarmerModel.FarmerPayableEarningsLc = farmerEarningsLC;
                        }
                        // Calculate the max deduction allowed based on % rule
                       
                    }
                    else
                    {
                        importFarmerModel.FarmerPayableEarningsLc = farmerEarningsLC;
                    }

                    // Optionally: update loan balance in DB
                    //loan.Balance = remainingLoanBalance;
                    //await _loanRepository.UpdateAsync(loan);

                    var loanApp = loanApplication.FirstOrDefault(x => x.FarmerId == selectedfarmer.Id);

                    var importEntity = new PaymentRequestDeductible
                    {
                        SystemId = importFarmerModel?.SystemId ?? "", // or handle as required
                        BeneficiaryId = importFarmerModel?.BeneficiaryId ?? "",
                        CarbonUnitsAccured = importFarmerModel?.CarbonUnitsAccured ?? 0,
                        UnitCostEur = importFarmerModel?.UnitCostEur ?? 0,
                        TotalUnitsEarningLc = importFarmerModel?.TotalUnitsEarningLc ?? 0,
                        SolidaridadEarningsShare = 0,
                        FarmerEarningsShareEur = importFarmerModel?.FarmerEarningsShareEur ?? 0,
                        FarmerEarningsShareLc = importFarmerModel?.FarmerEarningsShareLc ?? 0,
                        FarmerPayableEarningsLc = importFarmerModel?.FarmerPayableEarningsLc ?? 0,
                        FarmerLoansDeductionsLc = importFarmerModel?.FarmerLoansDeductionsLc ?? 0,
                        FarmerLoansBalanceLc = importFarmerModel?.FarmerLoansBalanceLc ?? 0,
                        PaymentBatchId = paymentBatchId,
                        ExcelImportId = id ?? Guid.Empty,
                        CreatedOn = DateTime.UtcNow,
                        CreatedBy = Guid.TryParse(_claimService.GetUserId(), out var createdBy) ? createdBy : Guid.Empty,
                        LoanAccountNo = loanApp?.LoanNumber ?? ""
                    };
                    if (statusIdColumnIndex != -1)
                    {
                        IRow firstDataRow = sheet.GetRow(r - 1);
                        if (firstDataRow != null)
                        {
                            string rowIdString = ParseUtility.ParseAnyValueToString(formatter.FormatCellValue(firstDataRow.GetCell(statusIdColumnIndex)));
                            if (Guid.TryParse(rowIdString, out Guid rowIdGuid))
                            {
                                rowId = rowIdGuid;
                                importEntity.Id = rowId;
                            }
                            else
                            {
                                throw new FormatException($"The value '{rowIdString}' is not a valid GUID.");
                            }
                        }
                    }

                    var message = "";
                    if (importFarmerModel != null)
                    {
                        var errorMessages = new List<string>();

                        if (!farmers.Any(f => f.SystemId == importFarmerModel.SystemId))
                        {
                            errorMessages.Add("Farmer with the provided System Id does not exist");
                        }
                        else if (!farmers.Any(f => f.SystemId == importFarmerModel.SystemId && f.IsFarmerVerified == true))
                        {
                            errorMessages.Add("Farmer mobile number is not verified");
                        }
                        else if (!farmers.Any(f => f.ParticipantId == importFarmerModel.BeneficiaryId))
                        {
                            errorMessages.Add("Farmer with the provided BeneficiaryId id does not exist");
                        }
                        else if (importFarmerModel.CarbonUnitsAccured == null || importFarmerModel.CarbonUnitsAccured == 0)
                        {
                            errorMessages.Add("Carbon Units Accured not provided");
                        }
                        else if (importFarmerModel.UnitCostEur == null || importFarmerModel.UnitCostEur == 0)
                        {
                            errorMessages.Add("Unit Cost (EUR) not provided or zero");
                        }
                        else if (importFarmerModel.TotalUnitsEarningLc == null || importFarmerModel.TotalUnitsEarningLc == 0)
                        {
                            errorMessages.Add("Total Units Earning (LC) not provided or zero");
                        }

                        //else if (loanApplication.FirstOrDefault(c => c.LoanBatchId == effectiveLoanBatch.Id && c.FarmerId == selectedfarmer.Id && c.Status == new Guid("e24d24a8-fc69-4527-a92a-97f6648a43c5")) == null)
                        //{
                        //    errorMessages.Add("Loan application is not dispersed ");
                        //}

                        else if (importFarmerModel.FarmerEarningsShareLc == null || importFarmerModel.FarmerEarningsShareLc == 0)
                        {
                            errorMessages.Add("Farmer Earnings Share (LC) not provided or zero");
                        }
                        //else if (importFarmerModel.FarmerPayableEarningsLc == null || importFarmerModel.FarmerPayableEarningsLc == 0)
                        //{
                        //    errorMessages.Add("Farmer Payable Earnings (LC) not provided or zero");
                        //}
                        //else if (importFarmerModel.FarmerLoansDeductionsLc == null || importFarmerModel.FarmerLoansDeductionsLc == 0)
                        //{
                        //    errorMessages.Add("Farmer Loans Deductions (LC) not provided or zero");
                        //}
                        //else if (importFarmerModel.FarmerLoansBalanceLc == null || importFarmerModel.FarmerLoansBalanceLc == 0)
                        //{
                        //    errorMessages.Add("Farmer Loans Balance (LC) not provided or zero");
                        //}

                        if (errorMessages.Any())
                        {
                            message = string.Join(", ", errorMessages);
                            var errorDetails = new ExcelImportDetail
                            {
                                Id = Guid.NewGuid(),
                                ExcelImportId = (Guid)id,
                                TabName = "",
                                RowNumber = r,
                                IsSuccess = false,
                                Remarks = message,
                            };
                            _errorDetails.Add(errorDetails);
                            importEntity.StatusId = 0;
                            importEntity.Remarks = message;
                        }
                        else
                        {
                            importEntity.StatusId = 1;
                            importEntity.Remarks = "";
                        }
                        _listPayments.Add(importEntity);
                    }
                    else
                    {
                        var errorDetails = new ExcelImportDetail
                        {
                            Id = Guid.NewGuid(),
                            ExcelImportId = Guid.NewGuid(),
                            TabName = "",
                            RowNumber = r,
                            IsSuccess = false,
                            Remarks = "Row is empty",
                        };
                        _errorDetails.Add(errorDetails);
                    }
                }

                // if rows exists for same payment batch, mark their status as -1
                var existing = await _paymentRequestDeductibleRepository.GetAllAsync(c => c.PaymentBatchId == paymentBatchId && c.IsDeleted == false);
                if (existing != null)
                {
                    existing.ForEach(c =>
                    {
                        c.StatusId = -1;
                        c.IsDeleted = true;
                    });

                    await _paymentRequestDeductibleRepository.UpdateRange(existing);
                }

                if (_listPayments.Count() > 0)
                {
                    if (statusIdColumnIndex == null)
                    {
                        await _paymentRequestDeductibleRepository.AddRange(_listPayments);
                    }
                    else
                    {
                        await _paymentRequestDeductibleRepository.UpdateRange(_listPayments);
                    }
                }

                if (_errorDetails.Count() > 0)
                    await _excelImportDetailRepository.AddRange(_errorDetails);

                // save import summary
                await _paymentRequestDeductibleRepository.SavePaymentImportSummary(new PaymentImportSummary
                {
                    ExcelImportId = (Guid)id,
                    FailedRows = _errorDetails.Count,
                    PassedRows = _listPaymentsExcel.Count - _errorDetails.Count,
                    TotalRowsInExcel = _listPaymentsExcel.Count,
                });


                // update loans balance
                //var paymenBatchLoanBatchMapping = await _paymenBatchLoanBatchMapping.GetAllAsync(x => x.PaymentBatchId == paymentBatchId);
                //var loanBatch = await _loanBatchRepository.GetAllAsync(x => x.Id == paymenBatchLoanBatchMapping.FirstOrDefault().LoanBatchId);
                //var loanApplication = await _loanApplicationRepository.GetAllAsync(x => x.LoanBatchId == loanBatch.FirstOrDefault().Id);

                //foreach (var item in _listPayments)
                //{
                //    if (item.StatusId == 1)
                //    {
                //        try
                //        {
                //            decimal annualInterestRate = 0;
                //            Guid loanApplicationId = Guid.NewGuid();
                //            var matchingLoanApplication = new LoanApplication();
                //            if (loanBatch != null)
                //            {
                //                annualInterestRate = loanBatch.FirstOrDefault().InterestRate;
                //            }
                //            if (loanApplication != null)
                //            {
                //                matchingLoanApplication = loanApplication?.FirstOrDefault(app => app.Farmer != null && app.Farmer.SystemId == item.SystemId);
                //                if (matchingLoanApplication != null)
                //                {
                //                    loanApplicationId = matchingLoanApplication.Id;
                //                }
                //                else
                //                {
                //                    throw new InvalidOperationException($"No matching LoanApplication found for item with SystemId: {item.SystemId}");
                //                }
                //            }

                //            /* older method*/
                //            //var addedItem = await _loanRepaymentRepository.AddRepayment(
                //            //    loanApplicationId,
                //            //    item.FarmerLoansDeductionsLc,
                //            //    annualInterestRate);

                //            /* newer method*/
                //            //_loanRepaymentRepository.ApplyPayment(
                //            //   loanApplicationId,
                //            //   item.FarmerLoansDeductionsLc);

                //            // decimal monthlyRate = annualInterestRate / 12 / 100;
                //            // decimal monthlyInterest = matchingLoanApplication.PrincipalAmount * monthlyRate;
                //            // matchingLoanApplication.PrincipalAmount = Math.Max(0, matchingLoanApplication.PrincipalAmount + monthlyInterest - item.FarmerLoansDeductionsLc);

                //            //var result =  await _loanApplicationRepository.UpdateAsync(matchingLoanApplication);
                //        }
                //        catch (Exception ex)
                //        {
                //            throw ex;
                //        }
                //    }
                //}
            }

            // update status as excel uploaded
            // update status as excel uploaded
            var stages = await _paymentBatchRepository.GetPaymentApprovalStages();
            string stageName = "Initiated";
            var stageId = stages.Where(c => c.StageText == stageName).FirstOrDefault().Id;

            var payBatch = await _paymentBatchRepository.GetFirstAsync(c => c.Id == paymentBatchId);
            if (payBatch != null)
            {
                payBatch.IsExcelUploaded = true;
                payBatch.StatusId = stageId;
                await _paymentBatchRepository.UpdateAsync(payBatch);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    #endregion
    public async Task ImportPaymentRequestFacilitation(IFormFile file, Guid? id, Guid batchId)
    {
        try
        {
            XSSFWorkbook hssfwb;
            using (var stream = file.OpenReadStream())
            {
                hssfwb = new XSSFWorkbook(stream);
            }

            ISheet sheet = hssfwb.GetSheet(Convert.ToString(ExelImportConstants.SHEET_PAYMENT_LIST));
            var _listPaymentsExcel = new List<PaymentRequestFacilitationModel>();
            var _listFarmers = new List<PaymentRequestFacilitation>();
            var excelImportDetails = new List<ExcelImportDetail>();
            var _errorDetails = new List<ExcelImportDetail>();
            // check if sheet exists
            if (sheet != null)
            {
                DataFormatter formatter = new DataFormatter();
                for (int row = 1; row <= sheet.LastRowNum; row++)
                {
                    var currentRow = sheet.GetRow(row);
                    if (currentRow == null || IsRowEmpty(currentRow))
                        continue;

                    if (sheet.GetRow(row) != null) //null is when the row only contains empty cells 
                    {
                        _listPaymentsExcel.Add(new PaymentRequestFacilitationModel
                        {
                            FullName = ParseUtility.ParseAnyValueToString(formatter.FormatCellValue(sheet.GetRow(row).GetCell(0))),
                            PhoneNo = ParseUtility.ParseAnyValueToString(formatter.FormatCellValue(sheet.GetRow(row).GetCell(1))),
                            NetDisbursementAmount = ParseUtility.ParseNullableDecimalValue(formatter.FormatCellValue(sheet.GetRow(row).GetCell(2))) ?? 0,
                            NationalId = ParseUtility.ParseAnyValueToString(formatter.FormatCellValue(sheet.GetRow(row).GetCell(3))),
                        });
                    }
                }
                int r = 1;

                var farmers = await _farmerRepository.GetAllAsync(c => 1 == 1);
                foreach (var importFarmerModel in _listPaymentsExcel)
                {
                    ++r;
                    var entity = new PaymentRequestFacilitation
                    {
                        Id = new Guid(),
                        FullName = importFarmerModel.FullName,
                        PhoneNo = importFarmerModel.PhoneNo,
                        NetDisbursementAmount = importFarmerModel.NetDisbursementAmount,
                        PaymentBatchId = batchId,
                        NationalId = importFarmerModel.NationalId
                    };

                    var message = "";
                    if (entity != null)
                    {
                        var errorMessages = new List<string>();

                        if (entity.FullName == null || entity.FullName == "")
                        {
                            errorMessages.Add("Full name is not provided");
                        }
                        else if (entity.PhoneNo == null || entity.PhoneNo == "")
                        {
                            errorMessages.Add("Phone no price sum is not provided");
                        }
                        else if (entity.NetDisbursementAmount == null || entity.NetDisbursementAmount == 0)
                        {
                            errorMessages.Add("Farmer share usd is not provided");
                        }
                        else if (string.IsNullOrWhiteSpace(entity.NationalId))
                        {
                            errorMessages.Add("National Id is not provided");
                        }

                        if (errorMessages.Any())
                        {
                            message = string.Join(", ", errorMessages);
                            var errorDetails = new ExcelImportDetail
                            {
                                Id = Guid.NewGuid(),
                                ExcelImportId = (Guid)id,
                                TabName = "",
                                RowNumber = r,
                                IsSuccess = false,
                                Remarks = message,
                            };
                            entity.StatusId = 0;
                            entity.Remarks = message;
                            _errorDetails.Add(errorDetails);
                        }
                        else
                        {
                            entity.StatusId = 1;

                        }
                        _listFarmers.Add(entity);
                    }
                    else
                    {
                        var errorDetails = new ExcelImportDetail
                        {
                            Id = Guid.NewGuid(),
                            ExcelImportId = Guid.NewGuid(),
                            TabName = "",
                            RowNumber = r,
                            IsSuccess = false,
                            Remarks = "Row is empty",
                        };
                        _errorDetails.Add(errorDetails);
                    }
                }

                // if rows exists for same payment batch, mark their status as -1
                var existing = await _failitationRepository.GetAllAsync(c => c.PaymentBatchId == batchId && c.IsDeleted == false);
                if (existing != null)
                {
                    existing.ForEach(c =>
                    {
                        c.StatusId = -1;
                        c.IsDeleted = true;
                    });

                    await _failitationRepository.UpdateRange(existing);
                }

                if (_listFarmers.Any())
                //&& _errorDetails.Count == 0)
                {
                    await _failitationRepository.AddRange(_listFarmers);
                }
                await _excelImportDetailRepository.AddRange(_errorDetails);
            }

            // update status as excel uploaded
            var stages = await _paymentBatchRepository.GetPaymentApprovalStages();
            string stageName = "Initiated";
            var stageId = stages.Where(c => c.StageText == stageName).FirstOrDefault().Id;
            var payBatch = await _paymentBatchRepository.GetFirstAsync(c => c.Id == batchId);

            if (payBatch != null)
            {
                payBatch.IsExcelUploaded = true;
                payBatch.StatusId = stageId;
                await _paymentBatchRepository.UpdateAsync(payBatch);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    private bool IsRowEmpty(IRow row)
    {
        if (row == null) return true;
        foreach (var cell in row.Cells)
        {
            if (cell != null && cell.CellType != CellType.Blank)
                return false;
        }
        return true;
    }

    public async Task SimulateInterestUntilDate(Guid loanApplicationId)
    {
        var loanApplication = await _loanApplicationRepository.GetFirstAsync(x => x.Id == loanApplicationId);
        var loanBatch = await _loanBatchRepository.GetFirstAsync(x => x.Id == loanApplication.LoanBatchId);
        var today = DateTime.Now;

        DateTime loanStartDate = new DateTime(loanBatch.InitiationDate.Year, loanBatch.InitiationDate.Month, 1);
        var monthsElapsed = (today.Year - loanStartDate.Year) * 12 + (today.Month - loanStartDate.Month);

        decimal totalAccruedInterest = 0;
        decimal principal = loanApplication.EffectivePrincipal;
        decimal annualRate = loanBatch.CalculationTimeframe == "Monthly" ? loanBatch.InterestRate * 12 : loanBatch.InterestRate;
        decimal monthlyPayment = 200;

        for (int month = 1; month <= monthsElapsed && principal > 0; month++)
        {
            decimal monthlyInterest = principal * (annualRate / 100) / 12; // Monthly interest
            totalAccruedInterest += monthlyInterest;
            decimal principalReduction = monthlyPayment - monthlyInterest;

            if (principalReduction < 0)
            {
                Console.WriteLine($"Month {month}: Payment too low to cover interest! Remaining principal: {principal:C}");
                break;
            }

            principal -= principalReduction; // Reduce the principal

            // Simulated month
            DateTime simulatedDate = loanStartDate.AddMonths(month);
            // Console.WriteLine($"{simulatedDate:MMMM yyyy}: Interest: {monthlyInterest:C}, Payment: {monthlyPayment:C}, Remaining Principal: {principal:C}");

            // add to import interest log
            await _loanInterestRepository.AddAsync(new LoanInterest
            {
                MonthlyPayment = monthlyPayment,
                AccruedInterest = monthlyPayment,
                RemainingPrincipal = Math.Max(0, principal),
                CalculationMonth = DateTime.UtcNow
            });
        }

        // update effective principal
        loanApplication.EffectivePrincipal = principal;
        loanApplication.RemainingBalance = principal + totalAccruedInterest;

        await _loanApplicationRepository.UpdateAsync(loanApplication);
    }

    public void CalculateInterestAndReducePrincipal()
    {
        decimal principal = 0;
        decimal annualRate = 2;
        decimal monthlyPayment = 200;
        int monthCounter = 1;

        decimal monthlyInterest = principal * (annualRate / 100) / 12; // Monthly interest
        decimal principalReduction = monthlyPayment - monthlyInterest;

        if (principalReduction < 0)
        {
            Console.WriteLine($"Month {monthCounter + 1}: Payment is too low to cover interest! Remaining principal: {principal:C}");
            return;
        }

        principal -= principalReduction; // Reduce the principal

        // Print details
        Console.WriteLine($"Month {monthCounter + 1}: Interest: {monthlyInterest:C}, Payment: {monthlyPayment:C}, Remaining Principal: {principal:C}");
    }

    public async Task WaitForNextMonth()
    {
        DateTime now = DateTime.Now;
        DateTime nextRun = new DateTime(now.Year, now.Month, 1).AddMonths(1); // Next 1st of the month

        TimeSpan delay = nextRun - now; // Time until next 1st of the month
        Console.WriteLine($"\nWaiting until {nextRun} to run the next interest calculation...");

        await Task.Delay(delay); // Simulate waiting for the cron job
    }

    [Obsolete]
    public async Task<List<LoanInterest>> CalculateLoanInterestAsync(Guid loanApplicationId, DateTime currentDate)
    {
        // get existing data
        var existing = await _loanInterestRepository.GetAllAsync(c => c.LoanApplicationId == loanApplicationId);
        if (existing != null && existing.Any())
        {
            return existing.ToList();
        }

        var loanApplication = await _loanApplicationRepository.GetFirstAsync(x => x.Id == loanApplicationId);
        var loanBatch = await _loanBatchRepository.GetFirstAsync(x => x.Id == loanApplication.LoanBatchId);

        if (loanApplication == null) throw new Exception("Loan not found");

        var startDate = loanBatch.EffectiveDate.AddMonths(loanBatch.GracePeriod.Value); // Loan start date (e.g., Jan 1, 2025)
        var monthlyRate = loanBatch.CalculationTimeframe == "Monthly" ? loanBatch.InterestRate : loanBatch.InterestRate * loanBatch.Tenure;
        monthlyRate = monthlyRate / 100; // Convert to decimal
        var principal = loanApplication.PrincipalAmount;

        // Fetch last calculated interest date
        var lastInterestDate = startDate; //await _loanInterestRepository.GetLastInterestDateAsync(loanId) ?? startDate;

        var results = new List<LoanInterest>();
        var currentPrincipal = principal;

        while (lastInterestDate < currentDate)
        {
            lastInterestDate = lastInterestDate.AddMonths(1); // Move to next month

            // Get payments made between lastInterestDate and current month
            // var payments = //await _paymentRepository.GetPaymentsBetweenAsync(loanId, lastInterestDate, lastInterestDate.AddMonths(1));
            decimal totalPayments = 100;//payments.Sum(p => p.Amount);

            // Calculate interest for the month
            decimal interestForMonth = (decimal)(currentPrincipal * monthlyRate);

            // Deduct payments from principal
            decimal newPrincipal = Math.Max(0, currentPrincipal + interestForMonth - totalPayments);

            results.Add(new LoanInterest
            {
                CalculationMonth = lastInterestDate.ToUniversalTime(),
                AccruedInterest = interestForMonth,
                MonthlyPayment = totalPayments,
                RemainingPrincipal = newPrincipal,
                LoanApplicationId = loanApplication.Id,
            });

            currentPrincipal = newPrincipal;
            if (currentPrincipal <= 0) break; // Stop if loan is paid off
        }

        await _loanInterestRepository.AddRange(results);

        return results;
    }

    public async Task<List<LoanInterest>> CalculateLoanInterestSingleMonthAsync(Guid loanApplicationId)
    {
        var loanApplication = await _loanApplicationRepository.GetFirstAsync(x => x.Id == loanApplicationId);
        var loanBatch = await _loanBatchRepository.GetFirstAsync(x => x.Id == loanApplication.LoanBatchId);
        var loanInterest = await _loanInterestRepository.GetAllAsync(c => c.LoanApplicationId == loanApplicationId);

        if (loanApplication == null) throw new Exception("Loan not found");

        var startDate = loanBatch.EffectiveDate.AddMonths(loanBatch.GracePeriod.Value).AddDays(1); // Loan start date (e.g., Jan 1, 2025)
        var monthlyRate = loanBatch.CalculationTimeframe == "Monthly" ? loanBatch.InterestRate : loanBatch.InterestRate * loanBatch.Tenure;
        monthlyRate = monthlyRate / 100; // Convert to decimal
        var principal = loanApplication.EffectivePrincipal;

        // Fetch last calculated interest date
        var lastInterestDate = await GetLastInterestDateAsync(loanApplicationId) ?? startDate;

        // var results = new List<LoanInterest>();
        var currentPrincipal = principal;

        //while (lastInterestDate < currentDate)
        //{
        lastInterestDate = lastInterestDate.AddMonths(1).AddDays(1); // Move to next month

        // Get payments made between lastInterestDate and current month
        var payments = await GetPaymentsBetweenAsync(loanApplication, lastInterestDate, lastInterestDate.AddMonths(1));
        decimal totalPayments = payments.Sum(p => p.AmountPaid);

        // Calculate interest for the month
        decimal interestForMonth = (decimal)(currentPrincipal * monthlyRate);

        // Deduct payments from principal
        decimal newPrincipal = Math.Max(0, currentPrincipal + interestForMonth - totalPayments);

        await _loanInterestRepository.AddAsync(new LoanInterest
        {
            CalculationMonth = lastInterestDate.ToUniversalTime(),
            AccruedInterest = interestForMonth,
            MonthlyPayment = totalPayments,
            RemainingPrincipal = newPrincipal,
            LoanApplicationId = loanApplication.Id,
        });

        currentPrincipal = newPrincipal;
        // if (currentPrincipal <= 0) break; // Stop if loan is paid off
        //}

        // update effective principal
        loanApplication.EffectivePrincipal = currentPrincipal;
        loanApplication.RemainingBalance = currentPrincipal + interestForMonth;

        await _loanApplicationRepository.UpdateAsync(loanApplication);

        // get existing data
        var existing = await _loanInterestRepository.GetAllAsync(c => c.LoanApplicationId == loanApplicationId);
        if (existing != null && existing.Any())
        {
            return existing.ToList();
        }

        return null;
    }

    public async Task<DateTime?> GetLastInterestDateAsync(Guid loanId)
    {
        var loanInterest = await _loanInterestRepository.GetAllAsync(c => c.LoanApplicationId == loanId);

        return loanInterest.ToList()
            .OrderByDescending(l => l.CalculationMonth) // Get the latest month
            .Select(l => (DateTime?)l.CalculationMonth) // Convert to nullable DateTime
            .FirstOrDefault
            ();
    }

    public async Task<List<LoanRepayment>> GetPaymentsBetweenAsync(LoanApplication loanApplication, DateTime startDate, DateTime endDate)
    {
        //LoanRepayment
        var loanRepayment = await _loanRepaymentRepository.GetAllAsync(x => x.LoanApplicationId == loanApplication.Id);
        return loanRepayment.Where(p => p.PaymentDate >= startDate && p.PaymentDate < endDate).ToList();

        //var paymenBatchLoanBatchMapping = await _paymenBatchLoanBatchMapping.GetAllAsync(x => x.LoanBatchId == loanApplication.LoanBatchId);

        //var paymentBatchIds = paymenBatchLoanBatchMapping.Select(x => x.PaymentBatchId).Distinct().ToList();
        //var paymentDeductibles = await _paymentRequestDeductibleRepository.GetAllAsync(c =>
        //                                                                            c.StatusId == 1 &&
        //                                                                            paymentBatchIds.Contains(c.PaymentBatchId));

        ////return paymentDeductibles.Where(p => p.CreatedOn >= startDate && p.CreatedOn < endDate).ToList();

        //return paymentDeductibles.ToList();
    }
}


