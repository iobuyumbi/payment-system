import { useEffect, useState } from "react";
import { Content } from "../../../_metronic/layout/components/content";
import ImportTimeline from "./partials/ImportTimeline";
import ImportService from "../../../services/ImportService";
import PaymentImportHistory from "./partials/PaymentImportHistory";
import PaymentImportHeader from "./partials/PaymentImportHeader";
import { ToolbarWrapper } from "../../../_metronic/layout/components/toolbar";
import { PageLink, PageTitle } from "../../../_metronic/layout/core";
import { useParams } from "react-router-dom";
import PaymentBatchService from "../../../services/PaymentBatchService";
import ExportService from "../../../services/ExportService";
import ListDeductiblePayments from "./partials/ListDeductiblePayments";
import { ListFacilitationPayments } from "./ListFacilitationPayments";
import ProcessedPaymentHistoryTab from "./results/ProcssedPaymentHistoryTab";
import ListImportErrors from "./partials/ListImportErrors";

const breadCrumbs: Array<PageLink> = [
  {
    title: "Dashboard",
    path: "/dashboard",
    isSeparator: false,
    isActive: true,
  },
  {
    title: "",
    path: "",
    isSeparator: false,
    isActive: true,
  },
  {
    title: "Payment Batches",
    path: "/payment-batch",
    isSeparator: false,
    isActive: true,
  },
  {
    title: "",
    path: "",
    isSeparator: false,
    isActive: true,
  },
];
const exportService = new ExportService();
const paymentBatchService = new PaymentBatchService();
const importService = new ImportService();

const DeductiblePaymentDetail = () => {
  const { id } = useParams(); // Extracts `id` from the URL

  const [rowData, setRowData] = useState<any[]>([]); // For import detail rows
  const [importHistory, setImportHistory] = useState<any[]>([]); // Initialize as an empty array
  const [data, setData] = useState<any>(); // For batch details
  const [totalCounts, setTotalCounts] = useState<any>();

  // Bind Import History
  const bindImportHistory = async () => {
    try {
      const response = await importService.getImportSummaryByPaymentBatch(id);
      if (response && response.length > 0) {
        setImportHistory(response);
        await bindImportDetail(response[0].excelImportId); // Await to ensure it's completed
      }
    } catch (error) {
      console.error("Failed to bind import history:", error);
    }
  };

  const bindBatchDetail = async () => {
    try {
      const response = await paymentBatchService.getSingle(id);
      if (response) {
        console.log(response)
        setData(response);
      }
    } catch (error) {
      console.error("Failed to bind batch detail:", error);
    }
  };

  // Bind Import Detail
  const bindImportDetail = async (excelImportId: any) => {
    try {
      const response = await importService.getImportDetail(excelImportId);
      if (response) {
        setRowData(response);
      }
    } catch (error) {
      console.error("Failed to bind import detail:", error);
    }
  };

  // Handle Item Click
  const onItemClick = async (excelImportId: any) => {
    await bindImportDetail(excelImportId);
  };

  const bindData = async () => {
    const model = { statusId: 1, batchId: id };
    const response = await exportService.getDeductiblePayments(model);

    if (response !== null) {
      const totals = response.reduce(
        (acc: any, row: any) => {
          acc.totalCarbonUnitsAccrued += row.carbonUnitsAccured || 0;
          acc.totalFarmerPayableEarningsLc += row.farmerPayableEarningsLc || 0;
          acc.totalFarmerLoansBalanceLc += row.farmerLoansBalanceLc || 0;
          acc.totalBeneficiaries.add(row.beneficiaryId);
          return acc;
        },
        {
          totalCarbonUnitsAccrued: 0,
          totalFarmerPayableEarningsLc: 0,
          totalFarmerLoansBalanceLc: 0,
          totalBeneficiaries: new Set(), // Use a Set to avoid duplicates
        }
      );

      // Convert the total beneficiaries Set to count
      const totalSummary = {
        totalCarbonUnitsAccrued: totals.totalCarbonUnitsAccrued,
        totalFarmerPayableEarningsLc: totals.totalFarmerPayableEarningsLc,
        totalFarmerLoansBalanceLc: totals.totalFarmerLoansBalanceLc,
        totalBeneficiaryCount: totals.totalBeneficiaries.size,
      };

      setTotalCounts(totalSummary);
    }
  };

  // Load Batch Details on Mount
  useEffect(() => {
    bindBatchDetail();
    bindData();
  }, []); // Run only once

  // Load Import History when `data` changes
  useEffect(() => {
    if (data) {
      bindImportHistory();
    }
  }, [data]);

  return (
    <>
      <ToolbarWrapper />
      <PageTitle breadcrumbs={breadCrumbs}>{"Payment Batch Detail"}</PageTitle>
      <Content>
        <div className="row col-md-12">
          {data && (
            <PaymentImportHeader
              batch={data}
              importHistory={importHistory}
              totalCounts={totalCounts}
            />
          )}
        </div>

        <div className="accordion mb-5" id="paymentAccordion">
          {/* Payment List */}
          <div className="accordion-item">
            <h2 className="accordion-header" id="headingList">
              <button
                className="accordion-button"
                type="button"
                data-bs-toggle="collapse"
                data-bs-target="#collapseList"
                aria-expanded="true"
                aria-controls="collapseList"
              >
                <span className="fw-bold fs-3"> {data?.successRowCount > 0 ? 'Payment list summary' : 'Payment list summary'}</span>
              </button>
            </h2>
            <div
              id="collapseList"
              className="accordion-collapse collapse show"
              aria-labelledby="headingList"
              data-bs-parent="#paymentAccordion"
            >
              <div className="accordion-body">
                {data?.paymentModule == 3 && data?.successRowCount > 0 && data?.failedRowCount == 0 && (
                  <ListDeductiblePayments batchId={id} paymentBatchName={data.batchName} />
                )}
                {data?.paymentModule == 3 && data?.failedRowCount > 0 && (
                  <ListImportErrors batchId={id} paymentBatchName={data.batchName} />
                )}
                {data?.paymentModule == 4 && data?.failedRowCount > 0 && (
                  <ListImportErrors batchId={id} paymentBatchName={data.batchName} />
                )}
                {data?.paymentModule == 4 && data?.successRowCount > 0 && data?.failedRowCount == 0 && (
                  <ListFacilitationPayments batchId={id} paymentBatchName={data.batchName} />
                )}
              </div>
            </div>
          </div>

          {/* Payment Results */}
          <div className="accordion-item">
            <h2 className="accordion-header" id="headingResults">
              <button
                className="accordion-button collapsed"
                type="button"
                data-bs-toggle="collapse"
                data-bs-target="#collapseResults"
                aria-expanded="false"
                aria-controls="collapseResults"
              >
                <span className="fw-bold fs-3">Proof Payments</span>
              </button>
            </h2>
            <div
              id="collapseResults"
              className="accordion-collapse collapse"
              aria-labelledby="headingResults"
              data-bs-parent="#paymentAccordion"
            >
              <div className="accordion-body">
                <ProcessedPaymentHistoryTab />
              </div>
            </div>
          </div>
        </div>

        <div className="row d-none">
          <div className="col-md-4">
            {importHistory && (
              <ImportTimeline
                importHistory={importHistory}
                onItemClick={onItemClick}
              />
            )}
          </div>
          <div className="col-md-8">
            {rowData && (
              <PaymentImportHistory
                rowData={rowData}
                batchId={id}
                importHistory={importHistory}
              />
            )}
          </div>
          <div className="col-md-8"></div>
        </div>
      </Content>
    </>
  );
};
export default DeductiblePaymentDetail;
