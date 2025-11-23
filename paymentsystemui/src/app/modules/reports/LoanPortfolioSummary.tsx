import { Card, Table, ProgressBar, Button, Form } from "react-bootstrap";
import {
  TrendingUp,
  FileText,
  CheckCircle,
  XCircle,
  Clock,
} from "lucide-react";
import LoanPortfolioReport from "./partials/LoanPortfolioReport";
import { PageLink, PageTitle } from "../../../_metronic/layout/core";
import { PageTitleWrapper } from "../../../_metronic/layout/components/toolbar/page-title";
import ReportService from "../../../services/ReportService";
import { useEffect, useState } from "react";
import { Line } from "react-chartjs-2";

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
const LoanPortfolioSummary = () => {
  const [rowData, setRowData] = useState<any>();
  const [stats, setStats] = useState<any>();
  const [loading, setLoading] = useState(false);
  const [trendsData, setTrendsData] = useState<any[]>([]);

  const bindData = async () => {
    const model = { statusId: -1 };
    const response = await reportService.getLoanBatchReports(model);
    const data = await reportService.getRepaymentMonthlyTrends();
    setTrendsData(data);
    setRowData(response.loanApplications);
    setStats(response.loanBatchStatsResponseModel);
  };

  const allMonths = [
    "January",
    "February",
    "March",
    "April",
    "May",
    "June",
    "July",
    "August",
    "September",
    "October",
    "November",
    "December",
  ];

  const loanRepaymentData = {
    labels: allMonths.map((month) => month.slice(0, 3)), // ["Jan", "Feb", ...]
    datasets: [
      {
        label: "Loan Repayment Amount",
        data: allMonths.map((month) => {
          const entry = trendsData.find((item) => item.month === month);
          const amount = entry ? entry.repaymentAmount : 0;
          return Math.max(amount, 0); // Ensure no negative values
        }),
        fill: true,
        backgroundColor: "rgba(75,192,192,0.2)",
        borderColor: "rgba(75,192,192,1)",
      },
    ],
  };

  useEffect(() => {
    bindData();
  }, []);

  return (
    <>
      {" "}
      <div className="container my-4">
        <PageTitleWrapper />
        <PageTitle breadcrumbs={profileBreadCrumbs}>
          Loan Product Reports{" "}
        </PageTitle>
        {/* Summary Cards */}
        <div className="row">
          <div className="col-md-4">
            <Card className="mb-3">
              <Card.Body>
                <h5>Loan Products Summary</h5>
                <p>
                  Status: Active | Total Value: {stats?.totalLoanValue} |{" "}
                  {stats?.totalLoanBatches} Loan Products
                </p>
              </Card.Body>
            </Card>
          </div>

          <div className="col-md-4">
            <Card className="mb-3">
              <Card.Body>
                <h5>Loan Accounts Summary</h5>
                <p>
                  Active: {stats?.activeLoanBatches} | Closed:{" "}
                  {stats?.closesLoanBatches} | Overdue:{" "}
                  {stats?.overdueLoanBatches}{" "}
                </p>
              </Card.Body>
            </Card>
          </div>

          <div className="col-md-4">
            <Card className="mb-3">
              <Card.Body>
                <h5>Non-Performing Loans</h5>
                <p>
                  Count: {stats?.nonPerformingLoans} | Value:{" "}
                  {stats?.nonPerformingValue}
                </p>
              </Card.Body>
            </Card>
          </div>
        </div>

        {/* Repayment Trends Table */}
        <Card className="mb-3">
          <Card.Body>
            <h5>Repayment Trends (Overall - {new Date().getFullYear()})</h5>

            <div className="row mb-3">
              <div className="col-md-6">
                <div style={{ height: "400px" }}>
                  <Line
                    data={loanRepaymentData}
                    options={{ maintainAspectRatio: false }}
                  />
                </div>
              </div>
              <div className="col-md-6">
                <div style={{ height: "400px", overflowY: "auto" }}>
                  <Table striped bordered hover responsive>
                    <thead>
                      <tr>
                        <th>Month</th>
                        <th>Repayment Amount</th>
                      </tr>
                    </thead>
                    <tbody>
                      {trendsData &&
                        trendsData.map((item, index) => (
                          <tr key={index}>
                            <td>{item.month}</td>
                            <td>{item.repaymentAmount.toLocaleString()}</td>
                          </tr>
                        ))}
                    </tbody>
                  </Table>
                </div>
              </div>
            </div>
          </Card.Body>
        </Card>

        {/* Report Generation */}
        <Card className="mb-3">
          <Card.Body>
            <h5>Loan Portfolio Reports</h5>
            {/* <Form>
              <Form.Group controlId="reportPeriod">
                <Form.Label>Report Period</Form.Label>
                <Form.Select>
                  <option>Monthly</option>
                  <option>Quarterly</option>
                  <option>Annual</option>
                </Form.Select>
              </Form.Group>
            </Form> */}
            <LoanPortfolioReport title={""} reportData={[]} />
          </Card.Body>
        </Card>
      </div>
    </>
  );
};

export default LoanPortfolioSummary;
