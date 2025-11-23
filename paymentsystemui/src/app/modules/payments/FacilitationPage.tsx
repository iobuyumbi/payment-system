import { Route, Routes, Outlet } from "react-router-dom";
import { PageLink, PageTitle } from "../../../_metronic/layout/core";

import { isAllowed } from "../../../_metronic/helpers/ApiUtil";
import { Error401 } from "../errors/components/Error401";
import { ListPaymentBatch } from "./ListPaymentBatch";
import { AddPaymentBatch } from "./AddPaymentBatch";
import { Disbursement } from "./partials/Disbursement";
import { ListFacilitationPayments } from "./ListFacilitationPayments";

const usersBreadcrumbs: Array<PageLink> = [
  {
    title: "Payment Batches",
    path: "/payment-batch",
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

const FacilitationPage = () => {
  return (
    <Routes>
      <Route element={<Outlet />}>
        <Route
          path="/"
          element={
            <>
              {isAllowed("payments.batch.history") ? (
                <>
                  <PageTitle breadcrumbs={usersBreadcrumbs}>
                    {" "}
                    Facilitation
                  </PageTitle>
                  <ListFacilitationPayments />
                </>
              ) : (
                <Error401 />
              )}
            </>
          }
        />
      
      </Route>
     
      




      {/* <Route index element={<Navigate to="/inputs" />} /> */}
    </Routes>
  );
};

export default FacilitationPage;
