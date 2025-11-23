import { Route, Routes, Outlet } from "react-router-dom";
import { PageLink, PageTitle } from "../../../_metronic/layout/core";
import { ListUsers } from "./ListUsers";
import { AddUser } from "./AddUser";
import ListRoles from "./ListRoles";
import ListUserRoles from "./ListUserRoles";
import ListRolePermissions from "./ListRolePermissions";
import { isAllowed } from "../../../_metronic/helpers/ApiUtil";
import { Error401 } from "../errors/components/Error401";
import { AddRoles } from "./AddRole";

const usersBreadcrumbs: Array<PageLink> = [
  {
    title: "Users",
    path: "/users",
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

const InputsPage = () => {
  return (
    <Routes>
      <Route element={<Outlet />}>
        <Route
          path="/users"
          element={
            <>
             
              {isAllowed("settings.system.users.view") ? (
                <>
                  {" "}
                  <PageTitle breadcrumbs={usersBreadcrumbs}>Users</PageTitle>
                  <ListUsers />{" "}
                </>
              ) : (
                <Error401 />
              )}
            </>
          }
        />
      </Route>

      <Route
        path="users/add"
        element={
          <> {isAllowed("settings.system.users.add") ? (
            <>
            <PageTitle breadcrumbs={usersBreadcrumbs}>Add User</PageTitle>
            <AddUser />    </>
              ) : (
                <Error401 />
              )}
          </>
        }
      />
      <Route
        path="users/edit/:id"
        element={
          <> {isAllowed("settings.system.users.edit") ? (
            <>
            <PageTitle breadcrumbs={usersBreadcrumbs}>Edit User</PageTitle>
            <AddUser />  </>
              ) : (
                <Error401 />
              )}
          </>
        }
      />
      <Route
        path="/roles"
        element={
          <> {isAllowed("settings.system.roles.view") ? (
            <>
            <PageTitle breadcrumbs={usersBreadcrumbs}>Roles</PageTitle>
            <ListRoles /> </>
              ) : (
                <Error401 />
              )}
          </>
        }
      />
       <Route
        path="/roles/add"
        element={
          <> {isAllowed("settings.system.users.groups.add") ? (
            <>
            <PageTitle breadcrumbs={usersBreadcrumbs}>Add Roles</PageTitle>
            <AddRoles /> </>
              ) : (
                <Error401 />
              )}
          </>
        }
      />
       <Route
        path="/roles/edit/:id"
        element={
          <> {isAllowed("settings.system.users.groups.edit") ? (
            <>
            <PageTitle breadcrumbs={usersBreadcrumbs}>Edit Roles</PageTitle>
            <AddRoles /> </>
              ) : (
                <Error401 />
              )}
          </>
        }
      />
      <Route
        path="/user-roles"
        element={
          <> {isAllowed("settings.system.roles.view") ? (
            <>
            <PageTitle breadcrumbs={usersBreadcrumbs}>Roles</PageTitle>
            <ListUserRoles /> </>
              ) : (
                <Error401 />
              )}
          </>
        }
      />
      <Route
        path="/role-permissions"
        element={
          <> {isAllowed("settings.system.roles.view") ? (
            <>
            <PageTitle breadcrumbs={usersBreadcrumbs}>Roles</PageTitle>
            <ListRolePermissions /></>
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

export default InputsPage;
