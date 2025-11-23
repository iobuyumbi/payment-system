import { useState } from "react";
import { Table, Card, Button, Form, Pagination } from "react-bootstrap";

interface LoanReportEntry {
  accountNumber: string;
  borrowerName: string;
  loanAmount: number;
  outstandingBalance: number;
  status: string;
  startDate: string;
  endDate: string;
}

interface LoanReportProps {
  loanData: LoanReportEntry[];
}

const LoanReport: React.FC<LoanReportProps> = ({ loanData }) => {
  const [currentPage, setCurrentPage] = useState(1);
  const entriesPerPage = 5;

  const indexOfLastEntry = currentPage * entriesPerPage;
  const indexOfFirstEntry = indexOfLastEntry - entriesPerPage;
  const currentEntries = loanData.slice(indexOfFirstEntry, indexOfLastEntry);

  const paginate = (pageNumber: number) => setCurrentPage(pageNumber);

  return (
    <Card className="mb-3">
      <Card.Body> 
         <div className="text-end mt-3">
        <Button variant="theme" className="me-2">Download PDF</Button>
        <Button variant="secondary">Download Excel</Button>
      </div>
        <h5 className="mb-3">Loan Portfolio Report</h5>
        <Table striped bordered hover responsive>
          <thead>
            <tr>
              <th>Account #</th>
              <th>Borrower Name</th>
              <th>Loan Amount </th>
              <th>Outstanding Balance </th>
              <th>Status</th>
              <th>Start Date</th>
              <th>End Date</th>
            </tr>
          </thead>
          <tbody>
            {currentEntries.map((entry, index) => (
              <tr key={index}>
                <td>{entry.accountNumber}</td>
                <td>{entry.borrowerName}</td>
                <td>{entry.loanAmount.toLocaleString()}</td>
                <td>{entry.outstandingBalance.toLocaleString()}</td>
                <td>{entry.status}</td>
                <td>{entry.startDate}</td>
                <td>{entry.endDate}</td>
              </tr>
            ))}
          </tbody>
        </Table>

        {/* Pagination */}
        <Pagination className="justify-content-center">
          {[...Array(Math.ceil(loanData.length / entriesPerPage))].map((_, index) => (
            <Pagination.Item
              key={index + 1}
              active={index + 1 === currentPage}
              onClick={() => paginate(index + 1)}
            >
              {index + 1}
            </Pagination.Item>
          ))}
        </Pagination>

       
      </Card.Body>
    </Card>
  );
};

export default LoanReport;
