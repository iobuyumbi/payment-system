import moment from "moment";
import { useEffect, useState } from "react";
import ExportService from "../../../../services/ExportService";

const exportService = new ExportService();

const PaymentInfoCard = ({ batch }: any) => {
  const [totalCounts, setTotalCounts] = useState<any>();

  const bindData = async () => {
    const model = { statusId: 1, batchId: batch.id };
    const response = await exportService.getDeductiblePayments(model);
    console.log("response", response);
    if (response !== null) {
      const totals = response
        .filter((row: any) => row.remarks == "") // Filter items with statusId > 0
        .reduce(
          (acc: any, row: any) => {
            acc.totalCarbonUnitsAccrued += row.carbonUnitsAccured || 0;
            acc.totalFarmerEarningLc += row.farmerEarningsShareLc || 0;
            acc.totalFarmerPayableEarningsLc +=
              row.farmerPayableEarningsLc || 0;
            acc.totalFarmerLoansBalanceLc += row.farmerLoansBalanceLc || 0;
            acc.totalBeneficiaries.add(row.beneficiaryId);
            acc.totalLoanDeductionsLc += row.farmerLoansDeductionsLc || 0;
            return acc;
          },
          {
            totalCarbonUnitsAccrued: 0,
            totalFarmerPayableEarningsLc: 0,
            totalFarmerLoansBalanceLc: 0,
            totalBeneficiaries: new Set(),
            totalLoanDeductionsLc: 0, // To avoid duplicate beneficiaries
            totalFarmerEarningLc: 0,
          }
        );

      const totalSummary = {
        totalCarbonUnitsAccrued: totals.totalCarbonUnitsAccrued,
        totalFarmerPayableEarningsLc: totals.totalFarmerPayableEarningsLc,
        totalFarmerLoansBalanceLc: totals.totalFarmerLoansBalanceLc,
        totalBeneficiaryCount: totals.totalBeneficiaries.size,
        totalLoanDeductionsLc: totals?.totalLoanDeductionsLc,
        totalFarmerEarningLc: totals?.totalFarmerEarningLc,
      };

      setTotalCounts(totalSummary);
    }
  };

  useEffect(() => {
    bindData();
  }, []);

  return (
    <div className="bg-light p-5 rounded">
      <div className="d-flex flex-column me-10 justify-content-center">
        <div className="d-flex flex-row justify-content-between">
          <label className="mb-2 fs-5">Payment batch name</label>
          <h6 className="mt-2">{batch?.batchName}</h6>
        </div>
        <div className="d-flex flex-row justify-content-between">
          <label className="my-2 fs-5">Date</label>
          <h6 className="mt-2">{moment(batch?.createdOn).format("YYYY-MM-DD HH:mm")}</h6>
        </div>
        <div className="d-flex flex-row justify-content-between">
          <label className="my-2  fs-5">Country</label>
          <h6 className="mt-2">{batch?.country?.countryName}</h6>
        </div>
        <div className="d-flex flex-row justify-content-between">
          <label className="my-2  fs-5">Currency</label>
          <h6 className="mt-2">{batch?.country?.currencyName}</h6>
        </div>

        {batch.paymentModule == 3 && (
          <div className="d-flex flex-row justify-content-between">
            <label className="my-2  fs-5">Loan Product(es)</label>
            <h6 className="mt-2">
              {batch.loanBatches &&
                batch.loanBatches.map((item: any, index: number) => (
                  <span key={index}>{item.name}</span>
                ))}
            </h6>
          </div>
        )}

        <div className="d-flex flex-row justify-content-between">
          <label className="my-2  fs-5">Project(s)</label>

          {batch.paymentModule == 4 && <h6>{batch?.project.projectName}</h6>}
          {batch.paymentModule == 3 && (
            <h6 className="mt-2">
              {" "}
              {batch.loanBatches &&
                batch.loanBatches.map((item: any, index: number) => (
                  <span key={index}>{item.project.projectName}</span>
                ))}{" "}
            </h6>
          )}
        </div>
        <div className="separator my-2"></div>
        {batch?.paymentModule == 3 && (
          <div>
            <div className="d-flex flex-row justify-content-between">
              <label className="my-2 fs-5">Total beneficiary count</label>
              <h6 className="mt-2">{totalCounts?.totalBeneficiaryCount}</h6>
            </div>
            <div className="d-flex flex-row justify-content-between">
              <label className="my-2 fs-5">Total Farmer Earnings (LC)</label>
              <h6 className="mt-2">{totalCounts?.totalFarmerEarningLc}</h6>
            </div>

            <div className="d-flex flex-row justify-content-between">
              <label className="my-2 fs-5">
                Total payable farmer earnings (LC)
              </label>
              <h6 className="mt-2">{totalCounts?.totalFarmerPayableEarningsLc.toFixed(2)}</h6>
            </div>

            <div className="d-flex flex-row justify-content-between">
              <label className="my-2 fs-5">
                Total farmer loan balance (LC)
              </label>
              <h6 className="mt-2">{totalCounts?.totalFarmerLoansBalanceLc.toFixed(2)}</h6>
            </div>

            <div className="d-flex flex-row justify-content-between">
              <label className="my-2 fs-5">Total Loan Deductions (LC)</label>
              <h6 className="mt-2">{totalCounts?.totalLoanDeductionsLc.toFixed(2)}</h6>
            </div>
          </div>
        )}
      </div>

      <div className="row mb-3">
        <div className="col-md-4">
          {batch?.loanBatch?.name && (
            <label className="my-1">Loan Product</label>
          )}
          <h6 className="mt-2">{batch?.loanBatch?.name}</h6>
        </div>
      </div>
    </div>
  );
};

export default PaymentInfoCard;
