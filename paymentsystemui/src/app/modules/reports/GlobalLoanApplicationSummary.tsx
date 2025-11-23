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
import CountryLoanPortfolioReport from "./partials/CountryLoanPortfolioReport";
import GlobalLoanApplicationReport from "./partials/GlobalLoanApplicationReport";

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
const GlobalLoanApplicationSummary = () => {
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
          Global Loan Application Reports{" "}
        </PageTitle>
        
        

        <Card className="mb-3">
          <Card.Body>
            <h5> Global Loan Application Reports</h5>
           
            <GlobalLoanApplicationReport title={""} reportData={[]} />
          </Card.Body>
        </Card>
      </div>
    </>
  );
};

export default GlobalLoanApplicationSummary;
