import { PageLink, PageTitle } from "../../../_metronic/layout/core";
import { Outlet, Route, Routes, useLocation, useParams } from "react-router-dom";
import LoanBatchInfo from "./partials/LoanBatchInfo";
import ListBatchItems from "./ListBatchItems";
import ListBatchLoanApplications from "./ListApplications";
import { useEffect, useState } from "react";
import LoanApplicationImports from "./LoanApplicationImports";
import ListBatchFiles from "./partials/ListBatchFiles";
import ListBatchFees from "./partials/ListBatchFees";
import LoanBatchService from "../../../services/LoanBatchService";

const loanBatchService = new LoanBatchService();

const breadCrumbs: Array<PageLink> = [
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
  {
    title: "Loan Products",
    path: "/loans",
    isSeparator: false,
    isActive: false,
  },
];

const LoanBatchDetail = () => {
  const { batchId } = useParams()
  const [itemCount, setItemCount] = useState(0);
  const [loanBatch, setLoanBatch] = useState<any>(null);

  //const loanBatch = useSelector((state: any) => state?.loanBatches);
  const getLoanBatch = async (id: string) => {
    try {
      const response = await loanBatchService.getSingle(id)
      setLoanBatch(response);
    }
    catch (error) {
      console.error("Error fetching loan batch:", error);
    }
  }

  useEffect(() => {
    if (batchId) {
      getLoanBatch(batchId);
    }
  }, []);

  return (
    <Routes>
      <Route
        element={
          <>
            {loanBatch && <LoanBatchInfo loanBatch={loanBatch} itemCount={itemCount} />}
            <Outlet />
          </>
        }
      >
        <Route
          path="loan-items"
          element={
            <>
              <PageTitle breadcrumbs={breadCrumbs}>Loan Products Details</PageTitle>
              {loanBatch &&
                <ListBatchItems
                  id={loanBatch?.id}
                  loanBatchName={loanBatch?.name}
                  setItemCount={setItemCount}
                />
              }
            </>
          }
        />
        <Route
          path="/loan-application-imports"
          element={
            <>
              <PageTitle breadcrumbs={breadCrumbs}>Loan Products Details</PageTitle>
              {loanBatch &&
                <LoanApplicationImports
                  batchId={loanBatch?.id}
                  loanBatch={loanBatch}
                  loanBatchName={loanBatch?.name}
                />
              }
            </>
          }
        />

        <Route
          path="/loan-applications"
          element={
            <>
              <PageTitle breadcrumbs={breadCrumbs}>Loan Products Details</PageTitle>
              {loanBatch && <ListBatchLoanApplications loanBatch={loanBatch} />}
            </>
          }
        />

        <Route
          path="/loan-batch-files"
          element={
            <>
              <PageTitle breadcrumbs={breadCrumbs}>Loan Products Details</PageTitle>
              {loanBatch && <ListBatchFiles batchId={loanBatch?.id} />}
            </>
          }
        />

        <Route
          path="/loan-batch-fees"
          element={
            <>
              <PageTitle breadcrumbs={breadCrumbs}>Loan Products Details</PageTitle>
              {loanBatch && <ListBatchFees loanBatch={loanBatch} />}
            </>
          }
        />

        {/* <Route index element={<Navigate to='/farmer-detail/overview' />} /> */}
      </Route>
    </Routes>
  );
};


export default LoanBatchDetail;
