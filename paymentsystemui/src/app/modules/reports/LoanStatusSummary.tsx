import { useEffect, useState } from "react";
import { ProgressBar, Card, Button, Form, Table } from "react-bootstrap";
import { CheckCircle, XCircle, Clock, TrendingUp, FileText, Search } from "lucide-react";
import LoanReport from "./partials/LoanReport";
import { PageLink, PageTitle } from "../../../_metronic/layout/core";
import { PageTitleWrapper } from "../../../_metronic/layout/components/toolbar/page-title";
import ReportService from "../../../services/ReportService";

const reportService = new ReportService();
const profileBreadCrumbs: Array<PageLink> = [
  {
    title: "Report",
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
interface LoanStatusSummaryProps {
  totalLoans: number;
  activeLoans: number;
  closedLoans: number;
  overdueLoans: number;
  repaymentData: { month: string; amount: number }[];
  nonPerformingLoans: number;
  nonPerformingAmount: number;
}

const LoanStatusSummary: React.FC<LoanStatusSummaryProps> = ({
  totalLoans,
  activeLoans,
  closedLoans,
  overdueLoans,
  repaymentData,
  nonPerformingLoans,
  nonPerformingAmount,
}) => {
  const getPercentage = (value: number) => ((value / totalLoans) * 100).toFixed(2);
  const [searchTerm, setSearchTerm] = useState("");

  const filteredRepayments = repaymentData.filter(entry =>
    entry.month.toLowerCase().includes(searchTerm.toLowerCase())
  );

   const [rowData, setRowData] = useState<any>();
  const[stats, setStats] = useState<any>();
      const [loading, setLoading] = useState(false);
    
  
  
  const bindData = async () => {
      const model = { statusId: -1 };
      const response = await reportService.getLoanBatchReports(model);
      
      setRowData(response.loanApplications);
      setStats(response.loanBatchStatsResponseModel);
    };
  
        useEffect(() => {
          
           bindData();
         }, []);
       
    
  
  return (<>
    {" "}
         
    <div className="container">
    <PageTitleWrapper />
    <PageTitle breadcrumbs={profileBreadCrumbs}>General Reports</PageTitle>
      <div className="row">
        <div className="col-md-4">
          <Card className="mb-3">
            <Card.Body>
              <div className="d-flex align-items-center">
                <Clock className="text-primary me-2" />
                <h5 className="mb-0">Active Loans</h5>
              </div>
              <p>{stats?.activeLoans}/{stats?.totalLoans} </p>
              <ProgressBar now={parseFloat(getPercentage(stats?.activeLoans/stats?.totalLoans * 100))} label={`${getPercentage(stats?.activeLoans / stats?.totalLoans * 100)}%`} />
            </Card.Body>
          </Card>
        </div>

        <div className="col-md-4">
          <Card className="mb-3">
            <Card.Body>
              <div className="d-flex align-items-center">
                <CheckCircle className="text-success me-2" />
                <h5 className="mb-0">Closed Loans</h5>
              </div>
              <p>{closedLoans} / {totalLoans}</p>
              <ProgressBar now={parseFloat(getPercentage(stats?.closedLoans/stats?.totalLoans * 100))} label={`${getPercentage(stats?.closedLoans/stats?.totalLoans * 100)}%`} />
            </Card.Body>
          </Card>
        </div>

        <div className="col-md-4">
          <Card className="mb-3">
            <Card.Body>
              <div className="d-flex align-items-center">
                <XCircle className="text-danger me-2" />
                <h5 className="mb-0">Overdue Loans</h5>
              </div>
              <p>{overdueLoans} / {totalLoans}</p>
              <ProgressBar now={parseFloat(getPercentage(stats?.overdueLoans/stats?.totalLoans * 100))} label={`${getPercentage(stats?.overdueLoans/stats?.totalLoans * 100)}%`} />
            </Card.Body>
          </Card>
        </div>
      </div>

      {/* Repayment Trends with Search */}
      <Card className="mb-3">
        <Card.Body>
          <div className="d-flex align-items-center mb-3">
            <TrendingUp className="text-info me-2" />
            <h5 className="mb-0">Repayment Trends</h5>
          </div>
          <Form className="mb-3">
            <Form.Control
              type="text"
              placeholder="Search by month..."
              value={searchTerm}
              onChange={(e) => setSearchTerm(e.target.value)}
            />
          </Form>
          <Table striped bordered hover>
            <thead>
              <tr>
                <th>Month</th>
                <th>Amount </th>
              </tr>
            </thead>
            <tbody>
              {filteredRepayments.map((entry, index) => (
                <tr key={index}>
                  <td>{entry.month}</td>
                  <td>{entry.amount.toLocaleString()}</td>
                </tr>
              ))}
            </tbody>
          </Table>
        </Card.Body>
      </Card>

      {/* Non-Performing Loans */}
      <Card className="mb-3">
        <Card.Body>
          <div className="d-flex align-items-center">
            <XCircle className="text-danger me-2" />
            <h5 className="mb-0">Non-Performing Loans</h5>
          </div>
          <p>Count: {stats?.nonPerformingLoans}</p>
          <p>Total Amount: {stats?.nonPerformingValue.toLocaleString()}</p>
        </Card.Body>
      </Card>

      {/* Report Generation */}
      <Card className="mb-3">
        <Card.Body>
          <div className="d-flex align-items-center">
            <FileText className="text-primary me-2" />
            <h5 className="mb-0">Generate Reports</h5>
          </div>
          <Form className="mt-3">
            <Form.Group controlId="reportPeriod">
              <Form.Label>Report Period</Form.Label>
              <Form.Select>
                <option>Monthly</option>
                <option>Quarterly</option>
                <option>Annual</option>
              </Form.Select>
            </Form.Group>
            {/* <Button variant="primary" className="mt-3 w-100">Download Report (PDF/Excel)</Button> */}
          </Form>
          <LoanReport loanData={[]}/>
        </Card.Body>
      </Card>
    </div></>
  );
};

export default LoanStatusSummary;