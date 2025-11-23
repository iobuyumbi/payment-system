import React from "react";
import { Navigate, Route, Routes, Outlet } from "react-router-dom";
import { PageLink, PageTitle } from "../../../_metronic/layout/core";
import { AdminLevelHeader } from "./AdminLevelHeader";
import { AdminLevel1 } from "./AdminLevel1";
import { AdminLevel2 } from "./AdminLevel2";
import { AdminLevel3 } from "./AdminLevel3";
import { AdminLevel4 } from "./AdminLevel4";
import { Error401 } from "../errors/components/Error401";
import { isAllowed } from "../../../_metronic/helpers/ApiUtil";

const accountBreadCrumbs: Array<PageLink> = [
  {
    title: "Administrative Setup",
    path: "/adminlevel",
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

const AdminLevelPage: React.FC = () => {
  return (
    <Routes>
      <Route
        element={
          <>
            <AdminLevelHeader />
            <Outlet />
          </>
        }
      >
        <Route
          path="adminlevel1"
          element={
            <>
              <PageTitle breadcrumbs={accountBreadCrumbs}>
                Admin Level 1
              </PageTitle>
              {isAllowed("settings.administrative.view") ? <AdminLevel1 /> : <Error401 />}
            </>
          }
        />
        <Route
          path="adminlevel2"
          element={
            <>
              <PageTitle breadcrumbs={accountBreadCrumbs}>
                Admin Level 2
              </PageTitle>
              {isAllowed("settings.administrative.view") ? <AdminLevel2 /> : <Error401 />}
            </>
          }
        />
        <Route
          path="adminlevel3"
          element={
            <>
              <PageTitle breadcrumbs={accountBreadCrumbs}>
                Admin Level 3
              </PageTitle>
              {isAllowed("settings.administrative.view") ? <AdminLevel3 /> : <Error401 />}
            </>
          }
        />
        <Route
          path="adminlevel4"
          element={
            <>
              <PageTitle breadcrumbs={accountBreadCrumbs}>
                Admin Level 4
              </PageTitle>
              <AdminLevel4 />
            </>
          }
        />
        <Route index element={<Navigate to="/adminlevel/adminlevel1" />} />
      </Route>
    </Routes>
  );
};

export default AdminLevelPage;
