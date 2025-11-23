import { Route, Routes, Outlet } from "react-router-dom";
import { PageLink, PageTitle } from "../../../_metronic/layout/core";
import { isAllowed } from "../../../_metronic/helpers/ApiUtil";
import { Error401 } from "../errors/components/Error401";
import { ListPaymentBatch } from "./ListPaymentBatch";
import { AddPaymentBatch } from "./AddPaymentBatch";
import { Disbursement } from "./partials/Disbursement";
import { ListFacilitationPayments } from "./ListFacilitationPayments";
import DeductiblePaymentDetail from "./DeductiblePaymentDetail";
import Approve from "./approvals/Approve";
import Review from "./reviews/Review";
import { AddDeductiblePayments } from "./partials/AddDeductiblePayments";
import ProcessedPaymentHistory from "./results/ProcessedPaymentHistory";

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

const PaymentBatchesPage = () => {
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
                  <PageTitle breadcrumbs={usersBreadcrumbs}>
                    {" "}
                    Payment Batches
                  </PageTitle>
                  <ListPaymentBatch />{" "}

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
              {isAllowed("payments.batch.history") || 
              isAllowed("payments.batch.review") ||
              isAllowed("payments.batch.approve")
              ? (
                <>
                  <PageTitle breadcrumbs={usersBreadcrumbs}>Payment Batches</PageTitle>
                  <DeductiblePaymentDetail />
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
                  <Review />
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
                  <Approve />
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
                  <AddPaymentBatch />
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
                <Disbursement />
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
                <AddPaymentBatch />
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
                <AddDeductiblePayments />
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
                <ListFacilitationPayments />
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
                <ProcessedPaymentHistory />
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

export default PaymentBatchesPage;
