import { Route, Routes, Outlet } from "react-router-dom";
import { PageLink, PageTitle } from "../../../_metronic/layout/core";
import { ListItemCategory } from "./ListCategory";
import { AddItemCategory } from "./AddCategory";
import { Error401 } from "../errors/components/Error401";
import { isAllowed } from "../../../_metronic/helpers/ApiUtil";

const usersBreadcrumbs: Array<PageLink> = [
  {
    title: "Loan Item Categories",
    path: "/categories",
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

const ItemCategoryPage = () => {
  return (
    <Routes>
      <Route element={<Outlet />}>
        <Route
          path="/"
          element={
            <>
              {isAllowed("settings.loans.categories.view") ? (
                <>
                  <PageTitle breadcrumbs={usersBreadcrumbs}>
                    
                    Loan Item Categories
                  </PageTitle>
                  <ListItemCategory />
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
            {isAllowed("settings.loans.categories.add") ? (
              <>
               
                <PageTitle breadcrumbs={usersBreadcrumbs}>
                  Add Loan Item Category
                </PageTitle>
                <AddItemCategory />
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
             {isAllowed("settings.loans.categories.edit") ? (
              <><PageTitle breadcrumbs={usersBreadcrumbs}>
              Edit Loan Item Category
            </PageTitle>
            <AddItemCategory />  </>
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

export default ItemCategoryPage;
