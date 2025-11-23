import { Route, Routes, Outlet } from "react-router-dom";
import { PageLink, PageTitle } from "../../../_metronic/layout/core";
import { isAllowed } from "../../../_metronic/helpers/ApiUtil";
import { Error401 } from "../errors/components/Error401";
import KycStatus from "./KycStatus";
import PrePaymentKyc from "./PrePaymentKyc";

const usersBreadcrumbs: Array<PageLink> = [
  {
    title: "Payment Processing",
    path: "/payment-processing",
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

const KycPage = () => {
  return (
    <Routes>
      <Route element={<Outlet />}>
        <Route
          path="/"
          element={
            <>
              {" "}
              {isAllowed("payments.batch.history") ? (
                <>
                  <KycStatus />

                </>
              ) : (
                <Error401 />
              )}
            </>
          }
        />
        <Route
          path="/details/:id"
          element={
            <>
              {" "}
              {isAllowed("payments.batch.history") ? (
                <>
                  <PageTitle breadcrumbs={usersBreadcrumbs}>Payment Batches</PageTitle>
                </>
              ) : (
                <Error401 />
              )}
            </>
          }
        />

        <Route
          path="/review/:paymentBatchId"
          element={
            <>
              {" "}
              {isAllowed("payments.batch.history") ? (
                <>
                  <PageTitle breadcrumbs={usersBreadcrumbs}>Review Request</PageTitle>
                </>
              ) : (
                <Error401 />
              )}
            </>
          }
        />

        <Route
          path="/approve/:paymentBatchId"
          element={
            <>
              {" "}
              {isAllowed("payments.batch.history") ? (
                <>
                  <PageTitle breadcrumbs={usersBreadcrumbs}>Approve Request</PageTitle>
                </>
              ) : (
                <Error401 />
              )}
            </>
          }
        />

        <Route
          path="/add"
          element={
            <>
              {isAllowed("payments.batch.add") ? (
                <>
                  <PageTitle breadcrumbs={usersBreadcrumbs}>
                    {" "}
                    Payment Batches
                  </PageTitle>
                </>
              ) : (
                <Error401 />
              )}
            </>
          }
        />
      </Route>
      <Route
        path="/disbursement"
        element={
          <>
            {isAllowed("payments.batch.add") ? (
              <>
                <PageTitle breadcrumbs={usersBreadcrumbs}>
                  {" "}
                  Disbursement
                </PageTitle>
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
            {isAllowed("payments.batch.edit") ? (
              <>
                {" "}
                <PageTitle breadcrumbs={usersBreadcrumbs}>
                  Payment Batch Edit
                </PageTitle>
              </>
            ) : (
              <Error401 />
            )}
          </>
        }
      />
      <Route
        path="deductible/edit/:id"
        element={
          <>
            {isAllowed("payments.batch.edit") ? (
              <>
                {" "}
                <PageTitle breadcrumbs={usersBreadcrumbs}>
                  Payment Batch Edit
                </PageTitle>
              </>
            ) : (
              <Error401 />
            )}
          </>
        }
      />
      <Route
        path="/facilitation"
        element={
          <>
            {isAllowed("payments.batch.history") ? (
              <>
                <PageTitle breadcrumbs={usersBreadcrumbs}>
                  {" "}
                  Facilitation
                </PageTitle>
              </>
            ) : (
              <Error401 />
            )}
          </>
        }
      />

      <Route
        path="history/:id"
        element={
          <>
            {isAllowed("payments.batch.history") ? (
              <>
                {" "}
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

export default KycPage;
