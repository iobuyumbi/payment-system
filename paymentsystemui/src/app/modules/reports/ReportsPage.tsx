import { Route, Routes, Outlet, Navigate } from "react-router-dom";
import { PageLink, PageTitle } from "../../../_metronic/layout/core";
import LoanPerformanceReport from "./LoanPerformanceReport";
import PaymentsReport from "./PaymentsReport";
import { Reports } from "./Reports";
import { PaymentConfirmationReport } from "./PaymentConfirmationReport";
import RepaymentReport from "./RepaymentReport";
import TransactionReport from "./TransactionReports";

import PhoneVerificationStatusReport from "./PhoneVerificationStatusReport";
import LoanStatusSummary from "./LoanStatusSummary";
import LoanPortfolioSummary from "./LoanPortfolioSummary";
import LoanPerformanceSummary from "./LoanPerformanceSummary";
import PaymentBatchSummary from "./PaymentBatchSummary";
import DisbursedLoanReport from "./DisbursedLoanReport";
import CountryLoanPortfolioSummary from "./CountryLoanPortfolioSummary";
import GlobalLoanPortfolioSummary from "./GlobalLoanPortfolioSummary";
import GlobalLoanApplicationSummary from "./GlobalLoanApplicationSummary";



const usersBreadcrumbs: Array<PageLink> = [
  {
    title: "Reports",
    path: "/reports",
    isSeparator: false,
    isActive: false,
  },
  {
    title: "",
    path: "",
    isSeparator: true,
    isActive: false,
  },
];

const ReportsPage = () => {
  return (
    <Routes>
      <Route element={<Outlet />}>
      <Route
          path="/"
          element={
            <>
              <PageTitle breadcrumbs={usersBreadcrumbs}>Reports</PageTitle>
              <Reports  />
            </>
          }
        />
        
        <Route
          path="/loan-account-summary"
          element={
            <>
              <PageTitle breadcrumbs={usersBreadcrumbs}>Loan Accounts</PageTitle>
              <LoanStatusSummary totalLoans={100} activeLoans={80} closedLoans={10} overdueLoans={10} repaymentData={[]} nonPerformingLoans={0} nonPerformingAmount={0} />
            </>
          }
        />
        <Route
          path="/loan-portfolio-summary"
          element={
            <>
              <PageTitle breadcrumbs={usersBreadcrumbs}>Loan Accounts</PageTitle>
              <LoanPortfolioSummary />
            </>
          }
        />
         <Route
          path="/country-loan-portfolio-summary"
          element={
            <>
              <PageTitle breadcrumbs={usersBreadcrumbs}>Country Loan Accounts</PageTitle>
              <CountryLoanPortfolioSummary />
            </>
          }
        />
           <Route
          path="/global-loan-portfolio-summary"
          element={
            <>
              <PageTitle breadcrumbs={usersBreadcrumbs}>Global Loan Accounts</PageTitle>
              <GlobalLoanPortfolioSummary />
            </>
          }
        />
           <Route
          path="/global-loan-application-summary"
          element={
            <>
              <PageTitle breadcrumbs={usersBreadcrumbs}>Global Loan Application Summary</PageTitle>
              <GlobalLoanApplicationSummary />
            </>
          }
        />

         <Route
          path="/loan-performance-summary"
          element={
            <>
              <PageTitle breadcrumbs={usersBreadcrumbs}>Loan Accounts</PageTitle>
              <LoanPerformanceSummary />
            </>
          }
        />
        <Route
          path="/payment-batch-summary"
          element={
            <>
              <PageTitle breadcrumbs={usersBreadcrumbs}>Loan Performance</PageTitle>
              <PaymentBatchSummary />
            </>
          }
        />
        <Route
          path="/repayment-report"
          element={
            <>
              <PageTitle breadcrumbs={usersBreadcrumbs}>Repayment Report</PageTitle>
              <RepaymentReport />
            </>
          }
        />
        <Route
          path="/transaction-report"
          element={
            <>
              <PageTitle breadcrumbs={usersBreadcrumbs}>Transaction Report</PageTitle>
              <TransactionReport />
            </>
          }
        />
         <Route
          path="/phone-verification-report"
          element={
            <>
              <PageTitle breadcrumbs={usersBreadcrumbs}>Loan Performance</PageTitle>
              <PhoneVerificationStatusReport />
            </>
          }
        />
         <Route
          path="/disbursed-loan-report"
          element={
            <>
              <PageTitle breadcrumbs={usersBreadcrumbs}>Disbursed Loan Report</PageTitle>
              <DisbursedLoanReport />
            </>
          }
        />
      </Route>
      {/* <Route index element={<Navigate to='/team-members' />} /> */}
    </Routes>
  );
};

export default ReportsPage;
