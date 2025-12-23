import { useEffect, useState } from "react";
import { KTCard, KTCardBody } from "../../../../_metronic/helpers";
import { Content } from "../../../../_metronic/layout/components/content";
import CustomTable from "../../../../_shared/CustomTable/Index";
import { useNavigate, useParams } from "react-router-dom";
import ExportService from "../../../../services/ExportService";
import { Tab, Tabs } from "react-bootstrap";
import { PageTitleWrapper } from "../../../../_metronic/layout/components/toolbar/page-title";
import { PageLink, PageTitle } from "../../../../_metronic/layout/core";
import saveAs from "file-saver";
import * as XLSX from "xlsx";
import moment from "moment";
const exportService = new ExportService();

const breadCrumbs: Array<PageLink> = [
  {
    title: "Dashboard",
    path: "/dashboard",
    isSeparator: false,
    isActive: true,
  },
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

const ProcessedPaymentHistoryTab = () => {
  const navigate = useNavigate();

  const [rowData, setRowData] = useState<any[]>([]);
  const { id, fileName } = useParams();
  const [activeTab, setActiveTab] = useState<string>("Pending");
  const [activePayments, setActivePayments] = useState<any>({});
  const [completedPayments, setCompletedPayments] = useState<any>([]);
  const [pendingPayments, setPendingPayments] = useState<any>([]);
  const [failedPayments, setFailedPayments] = useState<any>([]);
  const [pausedPayments, setPausedPayments] = useState<any>([]);
  const [loading, setLoading] = useState(false);

  const StatusComponent = (props: any) => {
    return (
      <div className="py-1">
        {props.data.isFarmerVerified === true && (
          <div className="badge fs-7 badge-light-success  fw-normal">Yes</div>
        )}
        {props.data.isFarmerVerified === false && (
          <div className="badge fs-7 badge-light-danger  fw-normal">No</div>
        )}
      </div>
    );
  };

  const farmerComponent = (props: any) => {
    const farmer = props.data.farmer;
    const fields = props.fields || []; // Fields to display

    if (!farmer) {
      return <div>No Farmer Data</div>;
    }

    return (
      <div className="d-flex flex-column">
        {fields.map((field: string) => (
          <div key={field}>{farmer[field] || "Not Available"}</div>
        ))}
      </div>
    );
  };

  const [colDefs, setColDefs] = useState<any>([
    {
      field: "fullName",
      flex: 1,
    },
    {
      field: "mobile",
      flex: 1,
    },
    { field: "systemId", flex: 1, headerName: "System Id" },
    { field: "remarks", flex: 1, headerName: "Remarks" },
    {
      field: "amount",
      headerName: "Amount",
      valueFormatter: (params: any) => {
        return parseFloat(params.value).toFixed(2);
      },
      flex: 1,
    },
    {
      field: "isFarmerVerified",
      headerName: "Is Mobile Verified",
      flex: 1,
      cellRenderer: StatusComponent,
    },
  ]);

  const bindImportDetail = async () => {
    const model = { statusId: 1, batchId: id };
    const response = await exportService.getDeductiblePayments(model);
    {
      /* Active Processing States
new → "New Submission"

processing → "In Progress"

queued → "Waiting in Queue"

Pending or Paused States
pending_confirmation → "Awaiting Confirmation"

paused → "Paused"

paused_for_admin_action → "Paused - Admin Action Needed"

parked → "On Hold"

Completed or Terminated States
complete → "Completed Successfully"

error → "Failed - Error Encountered"

aborted → "Cancelled" */
    }
    const completed = response.filter(
      (item) => item.paymentStatus === "271d9c1a-2c4f-4ee2-ad0f-d7dc36bd255f"
    );
    const pending = response.filter(
      (item) => item.paymentStatus === "d8a75d19-0b59-4ba0-95a4-f800e48da2c9"
    );
    const active = response.filter(
      (item) =>
        item.paymentStatus === "3e3ff24a-9dd9-443c-a09c-d9c96dc36927" ||
        item.paymentStatus === "27b6555b-ab7a-4189-a437-ae124bc8e6e7" ||
        item.paymentStatus === "edc0c3a0-ac71-4c7e-8fc9-e0d6a551b652"
    );
    const paused = response.filter(
      (item) =>
        item.paymentStatus === "68682d11-ed34-4fb7-bae0-825dff8cceb9" ||
        item.paymentStatus === "8c481cbc-6dad-4b42-aef5-6c9990d34740" ||
        item.paymentStatus === "10467bda-86c0-46a6-bbec-498ee85d3823" ||
        item.paymentStatus === "b156ba98-7091-4236-9bfd-199050acfc24"
    );
    const failed = response.filter(
      (item) =>
        item.paymentStatus === "573fbb1a-5213-4264-bccc-dc2f530f2761" ||
        item.paymentStatus === "58a0686f-0d27-48e0-8940-55a26b3601f4"
    );

    setPausedPayments(paused);
    setFailedPayments(failed);
    setActivePayments(active);
    setCompletedPayments(completed);
    setPendingPayments(pending);

    setRowData(response);
    console.log("Payment Data", response);
  };

  const exportFields = [
    { field: "fullName", headerName: "Full Name" },
    { field: "mobile", headerName: "Mobile" },
    { field: "systemId", headerName: "System Id" },
    { field: "amount", headerName: "Amount" },
    { field: "isFarmerVerified", headerName: "Is Mobile Verified" },
    { field: "nationalId", headerName: "National Id" },
    { field: "beneficiaryId", headerName: "Beneficiary Id" },
    // { field: "gender", headerName: "Gender" },
    // { field: "birthMonth", headerName: "Birth Month" },
    // { field: "birthYear", headerName: "Birth Year" },
    // Add more as needed
  ];

  const exportPaymentsExcel = () => {
    const workbook = XLSX.utils.book_new();

    const paymentSheets = [
      { name: "Active Payments", data: activePayments },
      { name: "Completed Payments", data: completedPayments },
      { name: "Pending Payments", data: pendingPayments },
      { name: "Failed Payments", data: failedPayments },
      { name: "Paused Payments", data: pausedPayments },
    ];

    // Extract headings from colDefs
    const headers = exportFields.map((col: any) => col.headerName || col.field);

    // Generate each sheet
    paymentSheets.forEach((sheet) => {
      const rows = (sheet.data || []).map((row: any) => {
        return exportFields.map((col: any) => {
          const field = col.field;
          let value = row[field];

          if (col.valueFormatter) {
            value = col.valueFormatter({ value });
          } else if (col.cellRenderer === StatusComponent) {
            value = value ? "Verified" : "Not Verified";
          }

          return value;
        });
      });

      // Add header as first row
      const finalSheetData = [headers, ...rows];

      const worksheet = XLSX.utils.aoa_to_sheet(finalSheetData);
      XLSX.utils.book_append_sheet(workbook, worksheet, sheet.name);
    });

    const blob = new Blob(
      [XLSX.write(workbook, { bookType: "xlsx", type: "array" })],
      {
        type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
      }
    );
    const timestamp = moment().format("DD-MM-YYYY HH-mm");
    saveAs(blob, `Payment_Results_${timestamp}.xlsx`);
  };

  useEffect(() => {
    bindImportDetail();
  }, []);

  const [isButtonEnabled, setIsButtonEnabled] = useState(false);

  useEffect(() => {
    setIsButtonEnabled(rowData.some((item) => item.isFarmerVerified));
  }, [rowData]);

  return (
    <div>
      <div className="d-flex justify-content-end align-items-end mb-5">
        <button className="btn btn-theme" onClick={exportPaymentsExcel}>
          Export
        </button>
      </div>
      <div className="p-5">
        <Tabs
          id="Payments tab"
          activeKey={activeTab}
          onSelect={(k: string | null) => k && setActiveTab(k)}
          className="mb-5 custom-tabs"
        >
          {/* Tab 1: Table View */}
          <Tab
            eventKey="Pending"
            title={<span className="custom-tab-title">Pending Payments</span>}
          >
            <div className="mb-5">
              <CustomTable
                rowData={Array.isArray(pendingPayments) ? pendingPayments : []}
                colDefs={colDefs}
                header="Pay"
                addBtnText=""
                addBtnLink=""
                className="mt-0"
              />
            </div>
          </Tab>

          {/* Tab 2: Date-Based Data */}
          <Tab
            eventKey="Active"
            title={<span className="custom-tab-title">Active Payments</span>}
            className="mb-10"
          >
            <div className="mb-5">
              <CustomTable
                rowData={Array.isArray(activePayments) ? activePayments : []}
                colDefs={colDefs}
                header=""
                addBtnText=""
                addBtnLink=""
              />
            </div>
          </Tab>
          <Tab
            eventKey="Paused"
            title={<span className="custom-tab-title">Paused Payments</span>}
            className="mb-10"
          >
            <div className="mb-5">
              <CustomTable
                rowData={Array.isArray(pausedPayments) ? pausedPayments : []}
                colDefs={colDefs}
                header=""
                addBtnText=""
                addBtnLink=""
              />
            </div>
          </Tab>
          <Tab
            eventKey="Completed"
            title={<span className="custom-tab-title">Completed Payments</span>}
            className="mb-10"
          >
            <div className="mb-5">
              <CustomTable
                rowData={
                  Array.isArray(completedPayments) ? completedPayments : []
                }
                colDefs={colDefs}
                header=""
                addBtnText=""
                addBtnLink=""
              />
            </div>
          </Tab>
          <Tab
            eventKey="Failed"
            title={<span className="custom-tab-title">Failed Payments</span>}
            className="mb-10"
          >
            <div className="mb-5">
              <CustomTable
                rowData={Array.isArray(failedPayments) ? failedPayments : []}
                colDefs={colDefs}
                header=""
                addBtnText=""
                addBtnLink=""
              />
            </div>
          </Tab>
        </Tabs>
      </div>
    </div>
  );
};

export default ProcessedPaymentHistoryTab;
