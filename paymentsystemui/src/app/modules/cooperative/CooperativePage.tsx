import { Route, Routes, Outlet } from "react-router-dom";
import { PageLink, PageTitle } from "../../../_metronic/layout/core";
import { ListCooperatives } from "./ListCooperative";
import { AddCooperative } from "./AddCooperative";
import { isAllowed } from "../../../_metronic/helpers/ApiUtil";
import { Error401 } from "../errors/components/Error401";

const usersBreadcrumbs: Array<PageLink> = [
  {
    title: "Cooperatives",
    path: "/cooperatives",
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

const cooperativePage = () => {
  return (
    <Routes>
      <Route element={<Outlet />}>
        <Route
          path="/"
          element={
            <>
              {isAllowed("settings.cooperatives.view") ? (
                <>
                  
                  <PageTitle breadcrumbs={usersBreadcrumbs}>
                    Cooperatives
                  </PageTitle>
                  <ListCooperatives />
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
            {isAllowed("settings.cooperatives.add") ? (
              <>
                <PageTitle breadcrumbs={usersBreadcrumbs}>
                  Add Cooperative
                </PageTitle>
                <AddCooperative />
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
            {isAllowed("settings.cooperatives.edit") ? (
              <>
                <PageTitle breadcrumbs={usersBreadcrumbs}>
                  Edit Cooperative
                </PageTitle>
                <AddCooperative />
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

export default cooperativePage;
