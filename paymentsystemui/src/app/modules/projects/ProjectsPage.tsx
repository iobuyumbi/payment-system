import { Route, Routes, Outlet, Navigate } from "react-router-dom";
import { PageLink, PageTitle } from "../../../_metronic/layout/core";
import { ListProjects } from "./ListProjects";
import AddProject from "./AddProject";
import { isAllowed } from "../../../_metronic/helpers/ApiUtil";
import { Error401 } from "../errors/components/Error401";

const usersBreadcrumbs: Array<PageLink> = [
  {
    title: "Projects",
    path: "/projects",
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

const ProjectsPage = () => {
  return (
    <Routes>
      <Route element={<Outlet />}>
        <Route
          path="/"
          element={
            <>
              {isAllowed("settings.projects.view") ? (
                <>
                  {" "}
                  <PageTitle breadcrumbs={usersBreadcrumbs}>Projects</PageTitle>
                  <ListProjects />{" "}
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
            {" "}
            {isAllowed("settings.projects.add") ? (
              <>
                {" "}
                <PageTitle breadcrumbs={usersBreadcrumbs}>
                  Add Project
                </PageTitle>
                <AddProject />{" "}
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
            {isAllowed("settings.projects.edit") ? (
              <>
                {" "}
                <PageTitle breadcrumbs={usersBreadcrumbs}>
                  Edit Project
                </PageTitle>
                <AddProject />{" "}
              </>
            ) : (
              <Error401 />
            )}
          </>
        }
      />
      {/* <Route index element={<Navigate to='/projects' />} /> */}
    </Routes>
  );
};

export default ProjectsPage;
