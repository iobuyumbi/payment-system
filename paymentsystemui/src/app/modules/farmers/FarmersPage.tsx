import { Route, Routes, Outlet } from "react-router-dom";
import { PageLink, PageTitle } from "../../../_metronic/layout/core";
import { ListFarmers } from "./ListFarmers";
import { AddFarmer } from "./AddFarmer";
import FarmerDetail from "./FarmerDetail";
import { isAllowed } from "../../../_metronic/helpers/ApiUtil";
import { Error401 } from "../errors/components/Error401";
import FarmerLoans from "./partials/FarmerLoans";
import { FarmerLoanSchedule } from "./partials/FarmerLoanSchedule";

const usersBreadcrumbs: Array<PageLink> = [
  {
    title: "Farmers",
    path: "/farmers",
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

const FarmersPage = () => {
  return (
    <Routes>
      <Route element={<Outlet />}>
        <Route
          path="/"
          element={
            <>
              {isAllowed("farmers.view") ? (
                <>
                  {" "}
                  <PageTitle breadcrumbs={usersBreadcrumbs}>Input</PageTitle>
                  <ListFarmers />{" "}
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
          <>
            {isAllowed("farmers.add") ? (
              <>
                <PageTitle breadcrumbs={usersBreadcrumbs}>Add Farmer</PageTitle>
                <AddFarmer />{" "}
              </>
            ) : (
              <Error401 />
            )}
          </>
        }
      />
      <Route
        path="/edit/:id"
        element={
          <>
            {isAllowed("farmers.edit") ? (
              <>
                {" "}
                <PageTitle breadcrumbs={usersBreadcrumbs}>
                  Edit Farmer
                </PageTitle>
                <AddFarmer />{" "}
              </>
            ) : (
              <Error401 />
            )}
          </>
        }
      />
      <Route
        path="/loans/:farmerId"
        element={
          <>
            {isAllowed("farmers.view") ? (
              <>
                {" "}
                <PageTitle breadcrumbs={usersBreadcrumbs}>
                  Farmer Loans
                </PageTitle>
                <FarmerLoans />{" "}
              </>
            ) : (
              <Error401 />
            )}
          </>
        }
      />
       {/* <Route
        path="/loan-schedule"
        element={
          <>
            {isAllowed("farmers.view") ? (
              <>
                {" "}
                <PageTitle breadcrumbs={usersBreadcrumbs}>
                  Farmer Loan Schedule
                </PageTitle>
                <FarmerLoanSchedule />{" "}
              </>
            ) : (
              <Error401 />
            )}
          </>
        }
      /> */}
      {/* <Route
        path="/*"
        element={
          <>
            <PageTitle breadcrumbs={usersBreadcrumbs}>
              Farmer Details
            </PageTitle>
            <FarmerDetail />
          </>
        }
      /> */}
      {/* <Route index element={<Navigate to="/" />} /> */}
    </Routes>
  );
};

export default FarmersPage;
