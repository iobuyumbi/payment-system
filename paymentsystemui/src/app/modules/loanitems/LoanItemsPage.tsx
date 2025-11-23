import { Route, Routes, Outlet } from "react-router-dom";
import { PageLink, PageTitle } from "../../../_metronic/layout/core";
import { ListLoanItems } from "./ListLoanItems";
import { AddLoanItems } from "./AddLoanItems";
import { isAllowed } from "../../../_metronic/helpers/ApiUtil";
import { Error401 } from "../errors/components/Error401";

const usersBreadcrumbs: Array<PageLink> = [
  {
    title: "Loan",
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

const LoanItemsPage = () => {
  return (
    <Routes>
      <Route element={<Outlet />}>
        <Route
          path="/"
          element={
            <>
              {" "}
              {isAllowed("settings.loans.items.view") ? (
                <>
                  {" "}
                  <PageTitle breadcrumbs={usersBreadcrumbs}>
                    Loan Items
                  </PageTitle>
                  <ListLoanItems />{" "}
                </>
              ) : (
                <Error401 />
              )}
            </>
          }
        />
      </Route>
      <Route
        path="/add"
        element={
          <> {isAllowed("settings.loans.items.add") ? (
            <>
              {" "}
            <PageTitle breadcrumbs={usersBreadcrumbs}>Add Loan Items</PageTitle>
            <AddLoanItems />   </>
              ) : (
                <Error401 />
              )}
          </>
        }
      />
      <Route
        path="/edit/:id"
        element={
          <>{isAllowed("settings.loans.items.edit") ? (
            <>
              {" "}
            <PageTitle breadcrumbs={usersBreadcrumbs}>
              Edit Loan Items
            </PageTitle>
            <AddLoanItems /> </>) : (
                <Error401 />
              )}
          </>
        }
      />
      {/* <Route index element={<Navigate to="/inputs" />} /> */}
    </Routes>
  );
};

export default LoanItemsPage;
