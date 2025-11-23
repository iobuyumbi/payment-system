import { useEffect, useState } from "react";
import { KTIcon } from "../../../../_metronic/helpers";
import LoanApplicationService from "../../../../services/LoanApplicationService";
import moment from "moment";
import CustomTable from "../../../../_shared/CustomTable/Index";
import { Form } from "formik";
const loanAppService = new LoanApplicationService();

const FarmerLoanSchedule = (props: any) => {
  const { farmer, loanApplicationId, setInfo, loanApplication } = props;
  const [rowData, setRowData] = useState<any>();
  const [data, setData] = useState<any>([]);

  const bindSchedule = async () => {
    const result = await loanAppService.getLoanSchedule(loanApplicationId);
   

    if (result && result.length > 0) {
      // Sort by startDate ascending (oldest first)

      setRowData(result);

      // Find the first item with "Pending" status
      const firstPending = result.find(
        (item: any) => item.paymentStatus === "Pending"
      );

      setInfo(firstPending ?? null); // Set the first pending or null if none
    } else {
      setRowData([]);
      setInfo(null);
    }
  };

  const [colDefs] = useState<any>([
    {
      field: "startDate",
      headerName: "Period",
      valueFormatter: (params: any) => {
        return moment(params.value).format('YYYY-MM-DD ');
      },
      flex: 1,
    },

    {
      field: "principalAmount",
      headerName: "Beginning",
      valueFormatter: (params: any) => {
        return parseFloat(params.value)?.toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 });
      },
      flex: 1,
    },
    {
      field: "amount",
      headerName: "Payment",
      valueFormatter: (params: any) => {
        return parseFloat(params.value)?.toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 });
      },
      flex: 1,
    },
    {
      field: "interestAmount",
      valueFormatter: (params: any) => {
        return parseFloat(params.value)?.toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 });
      },
      flex: 1,
    },
    {
      headerName: "Principal",
      valueFormatter: (params: any) => {
        return (
          parseFloat(params.data.amount) -
          parseFloat(params.data.interestAmount)
        )?.toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 });
      },
      flex: 1,
    },

    {
      field: "balance",
      headerName: "Ending",
      valueFormatter: (params: any) => {
        return parseFloat(params.value)?.toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 });
      },
      flex: 1,
    },
    // {
    //   field: "installment", flex: 1,
    // },
    // { headerName: "Status", flex: 1, cellRenderer: ActionComponent },
  ]);

  useEffect(() => {
    bindSchedule();
  }, []);

  return (
    <div className="timeline-item">
      <div className="timeline-line w-40px"></div>

      <div className="timeline-content mb-10 mt-n1">
        {/* Header Area  Start */}
        {data &&
          data.map((item: any, index: number) => (
            <div className="overflow-auto pb-5" key={index}>
              <div className="notice d-flex flex-column  rounded border-primary border border-dashed min-w-lg-600px flex-shrink-0 p-6 gap-4">
                <div className="d-flex align-items-center mb-3">
                  <KTIcon
                    iconName="chart-simple-3coding/cod004.svg"
                    className="fs-2tx text-primary me-3"
                  />
                  <h5 className="mb-0 text-primary">Loan Summary</h5>
                </div>

                <div className="card p-4 shadow-sm">
                  <div className="row g-4">
                    <div className="col-md-6">
                      <div className="fw-semibold text-muted">
                        Amount Borrowed
                      </div>
                      <div className="fs-5 text-dark">{item.grandTotal}</div>
                    </div>

                    <div className="col-md-6">
                      <div className="fw-semibold text-muted">
                        Period (Months)
                      </div>
                      <div className="fs-5 text-dark">
                        {item.totalSeedlings}
                      </div>
                    </div>

                    <div className="col-md-6">
                      <div className="fw-semibold text-muted">
                        Monthly Interest Rate
                      </div>
                      <div className="fs-5 text-dark">
                        {item.monthlyInterestRate}
                      </div>
                    </div>

                    <div className="col-md-6">
                      <div className="fw-semibold text-muted">
                        Monthly Installment Payment
                      </div>
                      <div className="fs-5 text-dark">
                        {item.monthlyPayment}
                      </div>
                    </div>

                    <div className="col-md-6">
                      <div className="fw-semibold text-muted">Future Value</div>
                      <div className="fs-5 text-dark">{item.futureValue}</div>
                    </div>

                    <div className="col-md-6">
                      <div className="fw-semibold text-muted">Grace Period</div>
                      <div className="fs-5 text-dark">{item.gracePeriod}</div>
                    </div>

                    <div className="col-md-6">
                      <div className="fw-semibold text-muted">
                        Repayment Start Date
                      </div>
                      <div className="fs-5 text-dark">
                        {item.repaymentStartDate}
                      </div>
                    </div>

                    <div className="col-md-6">
                      <div className="fw-semibold text-muted">
                        Annual Interest Rate
                      </div>
                      <div className="fs-5 text-dark">
                        {item.annualInterestRate}
                      </div>
                    </div>

                    <div className="col-md-6">
                      <div className="fw-semibold text-muted">
                        Total Interest
                      </div>
                      <div className="fs-5 text-dark">{item.totalInterest}</div>
                    </div>

                    <div className="col-md-6">
                      <div className="fw-semibold text-muted">
                        Fixed Interest Payment
                      </div>
                      <div className="fs-5 text-dark">
                        {item.fixedInterestPayment}
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          ))}

        {/* Header Area  End */}

        {/* Table Area Start */}
        <div className="p-0">
          <CustomTable
            rowData={rowData ? rowData : []}
            colDefs={colDefs}
            header=""
            addBtnText={""}
            height={700}
            pageSize={50}
          />
        </div>

        {/* Table Area End */}
      </div>
    </div>
  );
};

export { FarmerLoanSchedule };
