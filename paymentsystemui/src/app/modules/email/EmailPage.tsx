import { Route, Routes, Outlet, Navigate } from "react-router-dom";
import { PageLink, PageTitle } from "../../../_metronic/layout/core";
import EmailTemplateManager from "./EmailTemplateManager";
import ListTemplates from "./ListTemplates";

const usersBreadcrumbs: Array<PageLink> = [
  {
    title: "Dashboard",
    path: "/dashboard",
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

const EmailPage = () => {
  return (
    <Routes>
      <Route element={<Outlet />}>
        <Route
          path="/templates/add"
          element={
            <>
              <PageTitle breadcrumbs={usersBreadcrumbs}>Email Templates</PageTitle>
              <EmailTemplateManager />
            </>
          }
        />
        <Route
          path="/templates/edit/:id"
          element={
            <>
              <PageTitle breadcrumbs={usersBreadcrumbs}>Email Templates</PageTitle>
              <EmailTemplateManager />
            </>
          }
        />
        <Route
          path="/templates"
          element={
            <>
              <PageTitle breadcrumbs={usersBreadcrumbs}>Email Templates</PageTitle>
              <ListTemplates />
            </>
          }
        />
      </Route>

      {/* <Route index element={<Navigate to='/team-members' />} /> */}
    </Routes>
  );
};

export default EmailPage;
