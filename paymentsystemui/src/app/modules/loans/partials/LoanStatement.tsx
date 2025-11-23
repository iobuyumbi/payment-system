import React, { useEffect, useState } from "react";
import LoanRepaymentService from "../../../../services/LoanRepaymentService";
import moment from "moment";
import config from "../../../../environments/config";
import { bind } from "lodash";
import StatementPdfModal from "./StatementPdfModal";

const loanRepaymentService = new LoanRepaymentService();

const LoanStatement = ({
  loanApplicationId,
  loanNumber,
  setInfo
}: {
  loanApplicationId: any;
  loanNumber: any;
  setInfo: any;
}) => {
  const [repaymentInput, setRepaymentInput] = useState<any>("");
  const [data, setData] = useState<any>();
  const [showPDFModal, setShowPDFModal] = useState<boolean>(false);
  const [loading, setLoading] = useState<boolean>(false);

  const handleGenerateStatement = async () => {
    setLoading(true);

    const repaymentAmount = parseFloat(repaymentInput || "0");
    
    const response = await loanRepaymentService.generateLoanStatement(
      loanApplicationId
    );
    if (response) {
      console.log("Statement generated successfully", response);
      bindHistory();
    } else {
      // Handle error case
      console.error("Failed to generate statement");
    }
    setLoading(false);
  };

  const bindHistory = async () => {
  

    const response = await loanRepaymentService.getLoanStatementHistory(
      loanApplicationId
    );
    if (response && response.length > 0) {
      console.log("Loan statement history fetched successfully", response);
    } else {
      console.error("Failed to fetch loan statement history");
    }
    setData(response);
    setInfo(response);
  };

  useEffect(() => {
    bindHistory();
  }, []);

  return (
    <>
      <div className="d-flex flex-row align-items-end justify-content-end">
        {/* <input
                className='form-control w-300px'
                type="number"
                placeholder="Repayment Amount"
                value={repaymentInput}
                onChange={(e) => setRepaymentInput(e.target.value)}
            /> */}
        <button
          className="btn btn-primary m-2"
          onClick={handleGenerateStatement}
        >
          {loading ? "Please wait..." : "Generate Statements"} {" "}
        </button>
        <button
          className="btn btn-secondary m-2"
          onClick={() => setShowPDFModal(true)}
        >
          Generate PDF{" "}
        </button>
      </div>
      <div className="my-5">
        <table className="table align-middle table-row-dashed fs-6 gy-5 dataTable no-footer">
          <thead>
            <tr className="text-start text-muted fw-bolder fs-7 text-uppercase gs-0">
              <th >Statement Date</th>
              <th className="text-start">Transaction Reference</th>
              <th className="text-start">Transaction Type</th>
              <th className="text-start">Description</th>
              <th className="text-start">Opening Balance</th>
              <th className="text-start">Debit Amount</th>
              <th className="text-start">Credit Amount</th>
              <th className="text-start">Loan Balance</th>
              <th className="text-start">Principal Repaid</th>
              <th className="text-start">Interest Repaid</th>
            </tr>
          </thead>
          <tbody>
            {data?.map((item: any, index: any) => (
              <tr key={index}>
                <td className="text-gray-600 fw-bold">
                  {moment(item.statementDate).format("YYYY-MM-DD HH:mm")}
                </td>
                <td >{item.transactionReference}</td>
                <td className="text-start">{item.transactionType}</td>
                <td className="text-start">{item.description}</td>
                <td className="text-start">{item.openingBalance?.toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 })}</td>
                <td className="text-start">{item.debitAmount?.toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 })}</td>
                <td className="text-start">{item.creditAmount?.toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 })}</td>
                <td className="text-start">{item.loanBalance?.toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 })}</td>
                <td className="text-start">{item.principalPaid?.toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 })}</td>
                <td className="text-start">{item.interestPaid?.toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 })}</td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
      {showPDFModal && (
        <StatementPdfModal
          afterConfirm={() => setShowPDFModal(false)}
          applicationId={loanApplicationId}
          loanNumber={loanNumber}
        />
      )}
    </>
  );
};

export default LoanStatement;
