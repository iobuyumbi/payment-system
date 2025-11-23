import moment from "moment";
import config from "../../../environments/config";
import { KTIcon } from "../../../_metronic/helpers";

const LoanSummary = (props: any) => {
  const { loanData, info, loanApplication } = props;

  return (
    <div className="d-flex align-items-justify">
      <div className="w-25 me-20">
        <div className="table-responsive fs-5">
          <div className="d-flex flex-column justify-content-around border-bottom py-5">
            <div className="d-flex flex-row justify-content-between">
              <span>Initial Principal</span>
              <span className="fw-bold">
                {(
                  loanData?.application?.principalAmount -
                  loanData?.application?.feeApplied
                )?.toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 })}
              </span>
            </div>
          </div>
          <div className="d-flex flex-column justify-content-around border-bottom py-5">
            <div className="d-flex flex-row justify-content-between">
              <span>Fees</span>
              <span className="fw-bold">
                {loanData?.application?.feeApplied?.toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 })}
              </span>
            </div>
          </div>
          <div className="d-flex flex-column justify-content-around border-bottom py-5">
            <div className="d-flex flex-row justify-content-between">
              <span>Effective Principal</span>
              <span className="fw-bold">
                {loanData?.application?.principalAmount?.toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 })}
              </span>
            </div>
          </div>
        </div>
      </div>

      <div className="row w-50 mx-10 pt-5">
        <div className="col-md-6  mb-2">
          Remaining Loan Balance:{" "}
          <span className="fw-bold mx-3">
            {info && info.length > 0
  ? info[info.length - 1]?.loanBalance?.toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 }) > 0 ? info[info.length - 1]?.loanBalance?.toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 }) : 0
  : loanData?.application?.principalAmount?.toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 })}
          </span>
        </div>

        <div className="col-md-6  mb-2">
          Repaid Amount:{" "}
          <span className="fw-bold mx-3">
            {info && info.length > 0
              ? info.reduce((acc: number, item: any) => acc + (item.creditAmount || 0), 0)?.toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 })
              : "0.00"}
          </span>
        </div>

       

        <div className="col-md-6  mb-2">
          Tenure:{" "}
          <span className="fw-bold mx-3">
            {loanApplication?.loanBatch?.tenure}
          </span>
        </div>

        <div className="col-md-6  mb-2">
          Monthly Interest Rate:{" "}
          <span className="fw-bold mx-3">
            {loanApplication?.loanBatch?.calculationTimeframe == "Yearly"
              ? loanApplication?.loanBatch?.interestRate / 12
              : loanApplication?.loanBatch?.interestRate}
            %
          </span>
        </div>

        <div className="col-md-6  mb-2">
          Annual Interest Rate:{" "}
          <span className="fw-bold mx-3">
            {loanApplication?.loanBatch?.calculationTimeframe == "Yearly"
              ? (loanApplication?.loanBatch?.interestRate)?.toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 })
              : (loanApplication?.loanBatch?.interestRate * 12)?.toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 })}
            %
          </span>
        </div>

        <div className="col-md-6 mb-2">
          Grace Period(Months):{" "}
          <span className="fw-bold mx-3">
            {loanApplication?.loanBatch?.gracePeriod}
          </span>
        </div>

        <div className="col-md-6 mb-2">
          Monthly Installment Payment :{" "}
          <span className="fw-bold mx-3">{loanData?.schedule[0]?.amount}</span>
        </div>
      </div>

      {/* hidden */}
      {/* <div className="d-none d-flex flex-column">
        <div className="d-flex flex-row">
          <div className="border border-gray-300 border-dashed rounded min-w-125px py-3 px-2 mx-2 mb-3">
            <div className="fs-5 fw-bold text-gray-700">
              {loanApplication?.application?.loanNumber}
            </div>
            <div className=" text-gray-500">Loan Account No.</div>
          </div>

          <div className="border border-gray-300 border-dashed rounded min-w-125px py-3 px-2 mx-2 mb-3">
            <div className="fs-5 fw-bold text-gray-700">
              {loanData?.application?.principalAmount?.toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 })}
            </div>
            <div className=" text-gray-500">Principal Amount</div>
          </div>

          <div className="border border-gray-300 border-dashed rounded min-w-125px py-3 mx-2 px-2 mb-3">
            <div className="fs-5 fw-bold text-gray-700">{`${loanData?.schedule?.length} months`}</div>
            <div className=" text-gray-500">Tenure</div>
          </div>

          <div className="border border-gray-300 border-dashed rounded min-w-125px py-3 mx-2 px-2 mb-3">
            <div className="fs-5 fw-bold text-gray-700">{`${moment(
              loanData?.schedule[0]?.startDate
            ).format(config.dateOnlyFormat)}`}</div>
            <div className=" text-gray-500">Start date</div>
          </div>

          <div className="border border-gray-300 border-dashed rounded min-w-125px py-3 mx-2 px-2 mb-3">
            <div className="fs-5 fw-bold text-gray-700">{`${moment(
              loanData?.schedule[loanData?.schedule.length - 1]?.endDate
            ).format(config.dateOnlyFormat)}`}</div>
            <div className=" text-gray-500">End date</div>
          </div>
        </div>
      </div> */}
    </div>
  );
};

export default LoanSummary;
