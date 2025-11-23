import { Route, Routes, Outlet } from "react-router-dom";
import { PageLink, PageTitle } from "../../../_metronic/layout/core";
import { ListLoanBatches } from "./ListLoanBatches";
import { AddLoanBatch } from "./AddLoanBatch";
import ListBatchItems from "./ListBatchItems";
import LoanBatchDetail from "./LoanBatchDetails";
import ListBatchLoanApplications from "./ListApplications";
import AddLoanBatchApplication from "./AddLoanBatchApplication";
import { isAllowed } from "../../../_metronic/helpers/ApiUtil";
import { Error401 } from "../errors/components/Error401";

const usersBreadcrumbs: Array<PageLink> = [
  {
    title: "Loan Products",
    path: "/loanBatches",
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

const LoanBatchesPage = () => {
  return (
    <Routes>
      <Route element={<Outlet />}>
        <Route
          path="/"
          element={
            <>
              {" "}
              {isAllowed("loans.batch.view") ? (
                <>
                  <PageTitle breadcrumbs={usersBreadcrumbs}>
                    {" "}
                    Loan Products
                  </PageTitle>
                  <ListLoanBatches />{" "}
                </>
              ) : (
                <Error401 />
              )}
            </>
          }
        />
        <Route
          path="items"
          element={
            <>
              {isAllowed("loans.batch.view") ? (
                <>
                  <PageTitle breadcrumbs={usersBreadcrumbs}>
                    {" "}
                    Loan Products
                  </PageTitle>
                  <ListBatchItems />
                </>
              ) : (
                <Error401 />
              )}
            </>
          }
        />
      </Route>
      <Route
        path="add"
        element={
          <>
            {isAllowed("loans.batch.add") ? (
              <>
                <PageTitle breadcrumbs={usersBreadcrumbs}>
                  Add Loan Product
                </PageTitle>
                <AddLoanBatch />
              </>
            ) : (
              <Error401 />
            )}
          </>
        }
      />
      <Route
        path="edit/:id"
        element={
          <>
            {isAllowed("loans.batch.edit") ? (
              <>
                <PageTitle breadcrumbs={usersBreadcrumbs}>
                  Edit Loan Product
                </PageTitle>
                <AddLoanBatch />
              </>
            ) : (
              <Error401 />
            )}
          </>
        }
      />
      <Route
        path="/details/loan-items"
        element={
          <>
            {isAllowed("loans.batch.view") ? (
              <>
                <PageTitle breadcrumbs={usersBreadcrumbs}>
                  Loan Product Details
                </PageTitle>
                <LoanBatchDetail />
              </>
            ) : (
              <Error401 />
            )}
          </>
        }
      />
      <Route
        path="/details/loan-applications"
        element={
          <>
            {isAllowed("loans.batch.view") ? (
              <>
                <PageTitle breadcrumbs={usersBreadcrumbs}>
                  Loan Product Details
                </PageTitle>
                <ListBatchLoanApplications />
              </>
            ) : (
              <Error401 />
            )}
          </>
        }
      />
      <Route
        path="/loan-applications/add"
        element={
          <>
            {isAllowed("loans.batch.edit") ? (
              <>
                {" "}
                <PageTitle breadcrumbs={usersBreadcrumbs}>
                  Loan Product Applications Add
                </PageTitle>
                <AddLoanBatchApplication loanBatch={undefined} afterConfirm={function (confirmed: boolean): void {
                            throw new Error("Function not implemented.");
                        } } isAdd={false} reloadApplications={function (): void {
                            throw new Error("Function not implemented.");
                        } } />
              </>
            ) : (
              <Error401 />
            )}
          </>
        }
      />
      
      {/* <Route index element={<Navigate to="/inputs" />} /> */}
    </Routes>
  );
};

export default LoanBatchesPage;
