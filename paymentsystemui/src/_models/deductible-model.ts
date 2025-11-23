export interface PaymentDeductibleModel {
  systemId: string;
  paymentBatchId: string;
  beneficiaryId: string;
  carbonUnitsAccrued: number;
  unitCostEur: number;
  totalUnitsEarningEur: number;
  totalUnitsEarningLc: number;
  solidaridadEarningsShare: number;
  farmerEarningsShareEur: number;
  farmerEarningsShareLc: number;
  farmerPayableEarningsLc: number;
  farmerLoansDeductionsLc: number;
  farmerLoansBalanceLc: number;
  excelImportId: string | null;
  statusId: number;
  remarks: string;
}

// Initial values
export const paymentDeductibleInitValues: PaymentDeductibleModel = {
  systemId: "",
  paymentBatchId: "",
  beneficiaryId: "",
  carbonUnitsAccrued: 0,
  unitCostEur: 0,
  totalUnitsEarningEur: 0,
  totalUnitsEarningLc: 0,
  solidaridadEarningsShare: 0,
  farmerEarningsShareEur: 0,
  farmerEarningsShareLc: 0,
  farmerPayableEarningsLc: 0,
  farmerLoansDeductionsLc: 0,
  farmerLoansBalanceLc: 0,
  excelImportId: null,
  statusId: 0,
  remarks: "",
};
