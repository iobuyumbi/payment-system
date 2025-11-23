import { Link, useLocation } from "react-router-dom";
import { Content } from "../../../../_metronic/layout/components/content";
import { ToolbarWrapper } from "../../../../_metronic/layout/components/toolbar";
import { useState } from "react";
import { ImportModal } from "../../../../_shared/Modals/ImportModal";
import LoanApplicationsModal from "./LoanApplicationsModal";
import { isAllowed } from "../../../../_metronic/helpers/ApiUtil";
import moment from "moment";
import { StatusBadge } from "../../../../_shared/Status/StatusBadge";

const LoanBatchInfo = (props: any) => {
  const { loanBatch, itemCount } = props;
  const [showImport, setShowImport] = useState<boolean>(false);
  const [importBtnText, setImportBtnText] = useState<boolean>(false);
  const exModule = "loanApplication";
  const [showItemBox, setShowItemBox] = useState<boolean>(false);
  const location = useLocation();

  const afterConfirm = (value: any) => {
    setShowImport(false);
    setShowItemBox(false);
  };

  const tabs = [
    { path: `/loan-batch-details/${loanBatch.id}/loan-items`, label: "Loan Product Items" },
    {
      path: `/loan-batch-details/${loanBatch.id}/loan-application-imports`,
      label: "Loan Application Imports",
    },
    {
      path: `/loan-batch-details/${loanBatch.id}/loan-applications`,
      label: "Loan Applications",
    },
    {
      path: `/loan-batch-details/${loanBatch.id}/loan-batch-files`,
      label: "Loan Product Files",
    },
    { path: `/loan-batch-details/${loanBatch.id}/loan-batch-Fees`, label: "Loan Product Fees" },
  ];

  document.title = "Loan Product Details - SDD";

  return (
    <>
      <ToolbarWrapper />
      <Content>
        {isAllowed("loans.batch.view") ? (
          <div className="card">
            <div className="card-header">
              <div className="d-flex flex-row">
                <h4 className="card-title d-flex align-items-start flex-column">
                  <span className="card-label fs-3 mb-1">
                    {loanBatch?.name} <StatusBadge value={loanBatch.statusId} />
                  </span>
                </h4>
              </div>

              <div className="card-toolbar">
                {loanBatch?.statusId == 3 &&
                  isAllowed("loans.applications.export") && (
                    <button
                      onClick={() => setShowImport(true)}
                      className="btn btn-sm btn-primary w-200px me-3"
                      // disabled={itemCount === 0}
                    >
                      Import Applications
                    </button>
                  )}

                {loanBatch?.statusId == 3 &&
                  isAllowed("loans.applications.add") && (
                    <button
                      onClick={() => setShowItemBox(true)}
                      className="btn btn-sm btn-secondary w-200px me-3"
                      //disabled={itemCount === 0}
                    >
                      Create new loan
                    </button>
                  )}
              </div>
            </div>
            <div className="m-5 text-end">
              {itemCount == 0 && (
                <span className="text-muted">
                  Please add items to Loan Product before inmporting or adding a
                  new loan application
                </span>
              )}
            </div>
            <div className="card-body pt-9 pb-0">
              <div className="row mb-3">
                <div className="col-md-4">
                  <label>Project name</label>
                  <h6>{loanBatch?.project?.projectName}</h6>
                </div>
                <div className="col-md-4">
                  <label>Status</label>
                  <h6>
                    {loanBatch?.statusId == 1
                      ? "Open"
                      : loanBatch?.statusId == 2
                        ? "In Review"
                        : loanBatch?.statusId == 3
                          ? "Accepting Applications"
                          : loanBatch?.statusId == 4
                            ? "Closed"
                            : ""}
                  </h6>
                </div>
                <div className="col-md-4">
                  <label>Item count</label>
                  <h6>{itemCount}</h6>
                </div>
              </div>

              <div className="row mb-3">
                <div className="col-md-4">
                  <label>Interest rate (P.A.)</label>
                  <h6>
                    {loanBatch?.calculationTimeframe === "Yearly"
                      ? `${loanBatch?.interestRate} %`
                      : `${(loanBatch?.interestRate ?? 0) * 12} %`}
                  </h6>
                </div>
                {/* <div className="col-md-4">
                  <label>Interest is calculated </label>
                  <h6>{loanBatch?.calculationTimeframe}</h6>
                </div> */}
                <div className="col-md-4">
                  <label> Max Deductible Percentage</label>
                  <h6>{loanBatch?.maxDeductiblePercent} %</h6>
                </div>
                <div className="col-md-4">
                  <label>Interest rate type</label>
                  <h6>{loanBatch?.rateType}</h6>
                </div>
              </div>
              <div className="row mb-3">
                <div className="col-md-4">
                  <label>Tenure (in months)</label>
                  <h6>{loanBatch?.tenure}</h6>
                </div>
                <div className="col-md-4">
                  <label>Grace period (in months)</label>
                  <h6>{loanBatch?.gracePeriod}</h6>
                </div>
                {/* <div className="col-md-4">
                  <label>Processing fee</label>
                  <h6>{loanBatch?.processingFee?.toFixed(2)}</h6>
                </div> */}

                <div className="col-md-4">
                  <label>Effective date</label>
                  <h6>
                    {moment(loanBatch?.effectiveDate).format(
                      "YYYY-MM-DD hh:mm A"
                    )}
                  </h6>
                </div>
              </div>
              <div className="row mb-3">
                <div className="col-md-4">
                  <label>Application Count</label>
                  <h6>{loanBatch?.totalApplications}</h6>
                </div>

                <div className="col-md-4">
                  <label>Total Disbursed Amount</label>
                  <h6>{loanBatch?.totalBatchAmount.toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 })}</h6>
                </div>
                <div className="col-md-4">
                  <label>Total Application Drafts</label>
                  <h6>{loanBatch?.totalDraft}</h6>
                </div>
              </div>

              <div className="row mb-3">
                <div className="col-md-4">
                  <label>Applications Accepted</label>
                  <h6>{loanBatch?.totalAccepted}</h6>
                </div>

                <div className="col-md-4">
                  <label>Applications Rejected</label>
                  <h6>{loanBatch?.totalRejected}</h6>
                </div>
                <div className="col-md-4">
                  <label>Applications Closed</label>
                  <h6>{loanBatch?.totalClosed}</h6>
                </div>
              </div>

              <div className="row mb-3">
                <div className="col-md-4">
                  <label>Applications Disbursed</label>
                  <h6>{loanBatch?.totalDisbursed}</h6>
                </div>

                {/* <div className="col-md-4">
                  <label>Applications Rejected</label>
                  <h6>{loanBatch?.totalRejected}</h6>
                </div> */}
              </div>

              <div className="d-flex overflow-auto h-55px">
                <ul className="nav nav-stretch nav-line-tabs border-transparent fs-5 fw-bolder flex-nowrap">
                  {tabs.map((tab) => (
                    <li className="nav-item" key={tab.path}>
                      <Link
                        className={`nav-link text-gray-800 me-6 ${location.pathname === tab.path ? "active" : ""
                          }`}
                        to={tab.path}
                      >
                        {tab.label}
                      </Link>
                    </li>
                  ))}
                </ul>
              </div>
            </div>
          </div>
        ) : (
          ""
        )}
      </Content>
      {showImport && loanBatch?.statusId != 4 && (
        <ImportModal
          title={importBtnText}
          exModule={exModule}
          batchId={loanBatch.id}
          afterConfirm={afterConfirm}
          loanBatchName={loanBatch?.name}
          loanBatch={loanBatch}
        />
      )}
      {showItemBox && loanBatch?.statusId != 4 && (
        <LoanApplicationsModal
          loanBatch={loanBatch}
          afterConfirm={afterConfirm}
          isAdd={true}
        />
      )}
    </>
  );
};

export default LoanBatchInfo;
