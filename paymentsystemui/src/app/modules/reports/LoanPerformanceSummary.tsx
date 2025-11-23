import { Line } from "react-chartjs-2";
import { Card,  Button } from "react-bootstrap";
import { PageLink, PageTitle } from "../../../_metronic/layout/core";
import { PageTitleWrapper } from "../../../_metronic/layout/components/toolbar/page-title";
import ReportService from "../../../services/ReportService";
import { useEffect, useState } from "react";
import * as XLSX from "xlsx";
import CustomTable from "../../../_shared/CustomTable/Index";
import saveAs from "file-saver";
import moment from "moment";
import {
  FaBoxOpen,
  FaReceipt,
  FaBalanceScaleLeft,
  FaPercentage,
} from "react-icons/fa";



const reportService = new ReportService();
const profileBreadCrumbs: Array<PageLink> = [
  {
    title: "Reports",
    path: "/reports",
    isSeparator: false,
    isActive: true,
  },
  {
    title: "",
    path: "",
    isSeparator: true,
    isActive: true,
  },
];
const LoanPerformanceSummary = () => {
  const [rowData, setRowData] = useState<any>();
  const [stats, setStats] = useState<any>();
  const [loading, setLoading] = useState(false);

  const loanPerformanceData = {
    labels: ["Jan", "Feb", "Mar", "Apr", "May"],
    datasets: [
      {
        label: "Loan Performance",
        data: [200000, 220000, 250000, 270000, 300000],
        fill: true,
        backgroundColor: "rgba(75,192,192,0.2)",
        borderColor: "rgba(75,192,192,1)",
      },
    ],
  };

  const [colDefs, setColDefs] = useState<any>([
    {
      field: "loanNumber",
      flex: 1,
      headerName: "Loan ID",
    },
    { field: "principalAmount", flex: 1, headerName: "Principal Amount " },
    { field: "feeApplied", flex: 1, headerName: "Fees " },
    { field: "interestAmount", flex: 1, headerName: "Interest Earned " },
    { field: "remainingBalance", flex: 1, headerName: "Effective Balance " },
  ]);

  const bindData = async () => {
    const model = { statusId: -1 };
    const response = await reportService.getLoanAccountReports(model);

    setRowData(response.loanApplications);
    setStats(response.loanItemStats);
  };

  useEffect(() => {
    bindData();
  }, []);

  const downloadInterimList = async () => {
    try {
      const workbook = XLSX.utils.book_new();
      const model = { statusId: -1 };
      const response = await reportService.getLoanAccountReports(model);

      setLoading(true);

      // return
      if (Array.isArray(response.loanApplications)) {
        const mappedData = response.loanApplications.map(
          (row: any) => ({
            "Loan ID": row.loanNumber,
            "Principal Amount": row.principalAmount,
            "Fee Applied": row.feeApplied,
            "Interest Amount": row.interestAmount,
            "Remaining Balance": row.remainingBalance,
          })
        );

        const worksheet = XLSX.utils.json_to_sheet(mappedData);
        XLSX.utils.book_append_sheet(workbook, worksheet, "PaymentList");

        const blob = new Blob(
          [XLSX.write(workbook, { bookType: "xlsx", type: "array" })],
          {
            type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
          }
        );

        saveAs(
          blob,
          `${"loan_account_summary"}_${moment().format("DD_MM_YYYY_HHmmss")}.xlsx`
        );
      } else {
        alert("No data available to download.");
      }
    } catch (error) {
      console.error("Failed to download interim list:", error);
      alert("An error occurred while downloading the list. Please try again.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <>
      {" "}
      <div className="container m-4">
        <div>
          <PageTitleWrapper />
          <PageTitle breadcrumbs={profileBreadCrumbs}>
            Loan Account Summary
          </PageTitle>
        </div>

        {/* Summary Cards */}
        <div className="text-end m-3">
          {/* <Button variant="theme" className="me-2">
            Download PDF
          </Button> */}
          <Button variant="theme" onClick={downloadInterimList}>
            Download Excel
          </Button>
        </div>
        <div className="row">
          <div className="col-md-6">
            <Card className="mb-3">
              <Card.Body>

                <Line data={loanPerformanceData} />
              </Card.Body>
            </Card>
          </div>
          <div className="col-md-6">
            <div className="row">
              <div className="col-md-6">
                <Card className="mb-3">
                  <Card.Body>
                    <h5 className="d-flex align-items-center gap-2"><FaBoxOpen /> Items Loaned</h5>
                    <p>
                      {stats?.totalItemsLoaned} Items Loaned | Total Value:{" "}
                      {stats?.totalItemValue}
                    </p>
                  </Card.Body>
                </Card>
              </div>

              <div className="col-md-6">
                <Card className="mb-3">
                  <Card.Body>
                    <h5 className="d-flex align-items-center gap-2"><FaReceipt /> Total Fees Charged</h5>
                    <p>{stats?.totalFeeCharged}</p>
                  </Card.Body>
                </Card>
              </div>

              <div className="col-md-6">
                <Card className="mb-3">
                  <Card.Body>
                    <h5 className="d-flex align-items-center gap-2"><FaBalanceScaleLeft />Amount Summary</h5>
                    <p>
                      Principle: {stats?.principleAmount} | Effective Balance:
                      {stats?.effectiveBalance}
                    </p>

                  </Card.Body>
                </Card>
              </div>
              <div className="col-md-6">
                <Card className="mb-3">
                  <Card.Body>
                    <h5 className="d-flex align-items-center gap-2"><FaPercentage /> Interest Summary </h5>
                    <p>
                      Interest Earned: {stats?.interestEarned} | Effective Loan
                      Balance:{stats?.effectiveLoanBalance}
                    </p>
                  </Card.Body>
                </Card>
              </div>
            </div>
          </div>
        </div>

        {/* Loan Performance Graph */}

        {/* Loan Amount Breakdown */}
        <Card className="mb-3">
          <Card.Body>
            <h5>Detailed Loan Amount Breakdown</h5>
            <CustomTable
              rowData={rowData}
              colDefs={colDefs}
              header=""
              addBtnText={""}
              importBtnText={""}
              addBtnLink={""}
              showImportBtn={false}
            />
          </Card.Body>
        </Card>
      </div>
    </>
  );
};

export default LoanPerformanceSummary;
