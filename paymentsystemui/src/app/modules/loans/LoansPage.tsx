import { Route, Routes, Outlet, Navigate } from "react-router-dom";
import { PageLink, PageTitle } from "../../../_metronic/layout/core";
import { ListLoanApplications } from "./ListLoanApplications";
import { ListLoanBatches } from "../loanbatches/ListLoanBatches";
import { ListLoanItems } from "../loanitems/ListLoanItems";
import LoanAppDetails from "./LoanAppDetails";
import LoanManager from "./LoanManager";

const usersBreadcrumbs: Array<PageLink> = [
  {
    title: "Loans",
    path: "/loans",
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

const LoansPage = () => {
  return (
    <Routes>
      <Route element={<Outlet />}>
        <Route
          path="/"
          element={
            <>
              <PageTitle breadcrumbs={usersBreadcrumbs}>Loans</PageTitle>
              <ListLoanBatches />
            </>
          }
        />

        <Route
          path="/applications"
          element={
            <>
              <PageTitle breadcrumbs={usersBreadcrumbs}>Loans</PageTitle>
              <ListLoanApplications />
            </>
          }
        />

        <Route
          path="/applications/detail"
          element={
            <>
              <PageTitle breadcrumbs={usersBreadcrumbs}>Loans</PageTitle>
              <LoanAppDetails />
            </>
          }
        />

        <Route
          path="/items"
          element={
            <>
              <PageTitle breadcrumbs={usersBreadcrumbs}>Loans</PageTitle>
              <ListLoanItems />
            </>
          }
        />

        <Route
          path="/batches"
          element={
            <>
              <PageTitle breadcrumbs={usersBreadcrumbs}>Loans Batches</PageTitle>
              <ListLoanBatches />
            </>
          }
        />

        <Route
          path="/manager/:loanApplicationId"
          element={
            <>
              <PageTitle breadcrumbs={usersBreadcrumbs}>Loan Details</PageTitle>
              <LoanManager />
            </>
          }
        />

       
      </Route>

      {/* <Route index element={<Navigate to='/loans' />} /> */}
    </Routes>
  );
};

export default LoansPage;
