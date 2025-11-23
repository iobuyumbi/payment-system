import { Route, Routes, Outlet, Navigate } from "react-router-dom";
import { PageLink, PageTitle } from "../../../_metronic/layout/core";
import ImportHistory from "./ImportHistory";
import AddImport from "./AddImport";
import ImportDetail from "./ImportDetail";

const usersBreadcrumbs: Array<PageLink> = [
  {
    title: "Imports",
    path: "/imports",
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

const ImportsPage = () => {
  return (
    <Routes>
      <Route element={<Outlet />}>
        <Route
          path="/"
          element={
            <>
              <AddImport />
            </>
          }
        />

        <Route
          path="/history"
          element={
            <>
              <ImportHistory />
            </>
          }
        />

        <Route
          path="/detail/:id/:fileName"
          element={
            <>
              <ImportDetail />
            </>
          }
        />
      </Route>

      {/* <Route index element={<Navigate to='/tickets' />} /> */}
    </Routes>
  );
};

export default ImportsPage;
