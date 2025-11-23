import { useEffect, useState } from "react";
import { KTCard, KTCardBody, KTIcon } from "../../../_metronic/helpers";
import { Content } from "../../../_metronic/layout/components/content";
import { PageLink, PageTitle } from "../../../_metronic/layout/core";
import { PaymentsChart } from "./PaymentsChart";
import DeductiblePaymentService from "../../../services/DeductibleService";
import CustomTable from "../../../_shared/CustomTable/Index";

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
    isSeparator: true,
    isActive: true,
  },
];
const deductibleService = new DeductiblePaymentService();
const PaymentsReport = () => {
  const [rowData, setRowData] = useState<any>();

  useEffect(() => {
    const fetchData = async () => {
      try {
        const response = await deductibleService.getChartData();

        if (response) setRowData(response);
      } catch (error) {
        console.error("Error fetching payment data:", error);
      }
    };

    fetchData();
  }, []);

  const [colDefs, setColDefs] = useState<any>([
    { headerName: "Year", field: "year", flex: 1.5, filter: true },
    { headerName: "Month", field: "month", flex: 1, filter: true },

    { headerName: "Total", field: "total", flex: 1 },
  ]);

  return (
    <Content>
      <PageTitle breadcrumbs={breadCrumbs}>Payments Report</PageTitle>
      <KTCard>
        <div
          className="card-header d-flex align-items-center justify-content-between"
          id="kt_activities_header"
        >
          {/* Title */}
          <h3 className="card-title fw-bolder text-gray-900">
            Payments Report
          </h3>

          {/* Filters */}
          <div className="d-flex align-items-center">
            {/* From Date */}
            <div className="me-3">
              <label className="mb-0">From</label>
              <input
                type="date"
                className="form-control form-control-sm"
                id="fromDate"
              />
            </div>

            {/* To Date */}
            <div className="me-3">
              <label className=" mb-0">To</label>
              <input
                type="date"
                className="form-control form-control-sm"
                id="toDate"
              />
            </div>

            {/* Close Button */}
            <button
              type="button"
              className="btn btn-sm btn-icon btn-active-light-primary"
              id="kt_activities_close"
            >
              <KTIcon iconName="cross" className="fs-1" />
            </button>
          </div>
        </div>

        <KTCardBody>
          <PaymentsChart className={""} />
        </KTCardBody>
        <KTCardBody>
          <CustomTable rowData={rowData} colDefs={colDefs} />
        </KTCardBody>
      </KTCard>
    </Content>
  );
};

export default PaymentsReport;
