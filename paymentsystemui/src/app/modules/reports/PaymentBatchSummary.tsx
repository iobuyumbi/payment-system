import { Card, Button } from "react-bootstrap";
import { useEffect, useState } from "react";
import ReportService from "../../../services/ReportService";
import CustomTable from "../../../_shared/CustomTable/Index";
import * as XLSX from "xlsx";
import saveAs from "file-saver";
import moment from "moment";
import ExportService from "../../../services/ExportService";
import { PageTitleWrapper } from "../../../_metronic/layout/components/toolbar/page-title";
import { PageLink, PageTitle } from "../../../_metronic/layout/core";
import { Pie } from "react-chartjs-2";
import { Chart as ChartJS, ArcElement, Tooltip, Legend } from "chart.js";
import { FaUsers, FaMoneyBill, FaPiggyBank, FaChartBar } from "react-icons/fa";

import Select from "react-select";
import { pdfjs } from "react-pdf";
import { Worker, Viewer } from "@react-pdf-viewer/core";
import "@react-pdf-viewer/core/lib/styles/index.css";
import DatePicker from "react-datepicker";
import LoanBatchService from "../../../services/LoanBatchService";
import { toast } from "react-toastify";
import axios from "axios";
import { getAPIBaseUrl } from "../../../_metronic/helpers/ApiUtil";
import { AUTH_LOCAL_STORAGE_KEY } from "../auth";
import PaymentBatchService from "../../../services/PaymentBatchService";
import "react-toastify/dist/ReactToastify.css";
import "./toast_custom.css";

pdfjs.GlobalWorkerOptions.workerSrc = `//cdnjs.cloudflare.com/ajax/libs/pdf.js/${pdfjs.version}/pdf.worker.js`;
ChartJS.register(ArcElement, Tooltip, Legend);

const paymentBatchService = new PaymentBatchService();
const exportService = new ExportService();
const reportService = new ReportService();
const profileBreadCrumbs: Array<PageLink> = [
  {
    title: "Reports",
    path: "/reports",
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
const PaymentBatchSummary = () => {
  const [rowData, setRowData] = useState<any>();
  const [stats, setStats] = useState<any>();
  const [loading, setLoading] = useState(false);
  const [endDate, setEndDate] = useState<Date | null>(new Date());
  const [pdfUrl, setPdfUrl] = useState<string | null>(null);
  const [countryId, setCountryId] = useState<any>("");
  const [paymentBatchIds, setPaymentBatchIds] = useState<any>(null);
  const [startDate, setStartDate] = useState<Date | null>(null);
  const [paymentBatches, setPaymentBatches] = useState<any>();
  const [paymentStatus, setPaymentStatus] = useState(-1);

  const StatusComponent = (props: any) => {
    return (
      <div className="py-1">
        {props.data.isPaymentComplete === true && (
          <div className="badge fs-7 badge-light-success  fw-normal">Yes</div>
        )}
        {props.data.isPaymentComplete === false && (
          <div className="badge fs-7 badge-light-danger  fw-normal">No</div>
        )}
      </div>
    );
  };

  const PhoneComponent = (props: any) => {
    return (
      <div className="py-1">
        {props.data.farmer.isFarmerVerified === true && (
          <div className="badge fs-7 badge-light-success  fw-normal">Yes</div>
        )}
        {props.data.farmer.isFarmerVerified === false && (
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

  const CurrenyComponent = (props: any) => {
    const farmer = props.data.farmer?.country.code;
    // Fields to display

    if (!farmer) {
      return <div>No Farmer Data</div>;
    }

    return <div className="d-flex flex-column">{farmer}</div>;
  };

  const [colDefs, setColDefs] = useState<any>([
    {
      field: "fullName",
      flex: 1,
      cellRenderer: farmerComponent,
      cellRendererParams: {
        fields: ["fullName"],
      },
    },
    {
      field: "isFarmerVerified",
      flex: 1,
      cellRenderer: PhoneComponent,
    },
    { field: "farmerEarningsShareLc", flex: 1, headerName: "Amount Paid(LC)" },

    {
      field: "code",
      flex: 1,
      headerName: "Currency",
      cellRenderer: CurrenyComponent,
    },
    {
      field: "farmerLoansDeductionsLc",
      flex: 1,
      headerName: "Loan Deduction(LC)",
    },
    { field: "isPaymentComplete", flex: 1, cellRenderer: StatusComponent },
  ]);

  const bindPaymentBatch = async () => {
    const data: any = {
      pageNumber: 1,
      pageSize: 10000,
    };

    const response = await paymentBatchService.getPaymentBatchData(data);
    const options: any[] = response
      ? response.map((opt: any) => ({
          value: opt?.id,
          label: opt?.batchName,
        }))
      : [];
    setPaymentBatches(options);
  };

  const generatePdfPreview = async () => {
    try {
      if (!endDate) {
        setEndDate(new Date());
      }

      if (!paymentBatchIds || paymentBatchIds.length === 0) {
        window.alert("Please select at least one loan product.");
        return;
      }

      const data = {
        startDate: startDate,
        endDate: endDate,
        paymentBatchIds: paymentBatchIds,
        countryId: countryId,
        status: paymentStatus,
      };

      await toast.promise(
        (async () => {
          setLoading(true);
          const response = await axios.post(
            `${getAPIBaseUrl()}api/Reports/GetPaymentReportsPdf`,
            data,
            {
              headers: { Accept: "application/pdf" },
              responseType: "blob",
            }
          );

          const blob = new Blob([response.data], { type: "application/pdf" });
          const fileUrl = URL.createObjectURL(blob);
          setPdfUrl(fileUrl);
        })(),
        {
          pending: {
            render() {
              return "Generating PDF report...";
            },
            autoClose: false,
            closeButton: true,
          },
          success: {
            render() {
              return "Loan Portfolio Report downloaded!";
            },
            autoClose: 5000,
            closeButton: true,
          },
          error: {
            render({ data: err }) {
              const error = err as {
                response?: { data?: { message?: string } };
              };
              const backendMessage =
                error?.response?.data?.message || "Failed to generate PDF.";
              return `Error: ${backendMessage}`;
            },
            autoClose: 5000,
            closeButton: true,
          },
        }
      );

      setLoading(false);
    } catch (error) {
      console.error("PDF preview generation error:", error);
    } finally {
      setLoading(false);
    }
  };

  const downloadPdf = async () => {
    if (!endDate) {
      setEndDate(new Date());
    }

    if (!paymentBatchIds || paymentBatchIds.length === 0) {
      window.alert("Please select at least one loan product.");
      return;
    }

    const data = {
      startDate: startDate,
      endDate: endDate,
      paymentBatchIds: paymentBatchIds,
      countryId: countryId,
      status: paymentStatus,
    };

    await toast.promise(
      (async () => {
        setLoading(true);
        const response = await axios.post(
          `${getAPIBaseUrl()}api/Reports/GetPaymentReportsPdf`,
          data,
          {
            headers: { Accept: "application/pdf" },
            responseType: "blob",
          }
        );

        const blob = new Blob([response.data], { type: "application/pdf" });
        const fileUrl = URL.createObjectURL(blob);
        setPdfUrl(fileUrl);

        const timestamp = moment().format("YYYY-MM-DD HH:mm");
        const a = document.createElement("a");
        a.href = fileUrl;
        a.download = `Payments_Report_${timestamp}.pdf`;
        document.body.appendChild(a);
        a.click();
        a.remove();
      })(),
      {
        pending: {
          render() {
            return "Generating PDF report...";
          },
          autoClose: false,
          closeButton: true,
        },
        success: {
          render() {
            return "Loan Portfolio Report downloaded!";
          },
          autoClose: 5000,
          closeButton: true,
        },
        error: {
          render({ data: err }) {
            const error = err as { response?: { data?: { message?: string } } };

            const backendMessage =
              error?.response?.data?.message || "Failed to generate PDF.";
            return `Error: ${backendMessage}`;
          },
          autoClose: 5000,
          closeButton: true,
        },
      }
    );

    setLoading(false);
  };

  const downloadPaymentReport = async () => {
    try {
      if (
        !rowData ||
        rowData.length === 0 ||
        !paymentBatches ||
        paymentBatches.length === 0
      ) {
        toast.error("No payment data available to export.");
        return;
      }

      toast.promise(
        new Promise<void>(async (resolve, reject) => {
          try {
            const workbook = XLSX.utils.book_new();

            const batchLabelMap: Record<string, string> = {};
            paymentBatches.forEach((b: any) => {
              batchLabelMap[b.value] = b.label;
            });

            const groupedByBatch = rowData.reduce((acc: any, curr: any) => {
              const batchId = curr.paymentBatchId;
              if (!acc[batchId]) acc[batchId] = [];
              acc[batchId].push(curr);
              return acc;
            }, {});

            for (const batchId in groupedByBatch) {
              const payments = groupedByBatch[batchId];
              const sheetName =
                batchLabelMap[batchId] ?? `Batch_${batchId.slice(0, 6)}`;

              let totalEarnings = 0;
              let totalPayable = 0;
              let totalLoanDeduction = 0;
              let totalOpening = 0;
              let totalClosing = 0;

              const mappedData = payments.map((item: any, index: number) => {
                const row = {
                  No: index + 1,
                  "Payment Date": moment(item.paymentDate).format(
                    "YYYY-MM-DD HH:mm"
                  ),
                  "Farmer System ID": item.farmerSystemId,
                  "Farmer Name": item.farmerName,
                  "Farmer Earnings": item.farmerEarnings ?? 0,
                  "Farmer Payable Earnings": item.farmerPayableEarnings ?? 0,
                  "Loan Deduction": item.loanDeduction ?? 0,
                  "Loan O/B": item.loanOpeningBalance ?? 0,
                  "Loan Closing Balance": item.loanClosingBalance ?? 0,
                  "Processing Method": item.processingMethod,
                  "Provider Reference": item.paymentReference,
                  "Receiving Mobile No": item.receivingMobileNo,
                  Status: item.status === "Complete" ? "Yes" : "No",
                };

                totalEarnings += row["Farmer Earnings"];
                totalPayable += row["Farmer Payable Earnings"];
                totalLoanDeduction += row["Loan Deduction"];
                totalOpening += row["Loan O/B"];
                totalClosing += row["Loan Closing Balance"];

                return row;
              });

              const totalsRow: any = {
                No: "TOTAL",
                "Payment Date": "",
                "Farmer System ID": "",
                "Farmer Name": "",
                "Farmer Earnings": totalEarnings,
                "Farmer Payable Earnings": totalPayable,
                "Loan Deduction": totalLoanDeduction,
                "Loan O/B": totalOpening,
                "Loan Closing Balance": totalClosing,
                "Processing Method": "",
                "Provider Reference": "",
                "Receiving Mobile No": "",
                Status: "",
              };

              mappedData.push(totalsRow);
              mappedData.push({ No: "" });
              payments[0].batchStatusHistory.map((item: any, index: number) =>
                mappedData.push({
                  No: `Status Change ${index + 1}`,
                  "Payment Date": item.updatedAt
                    ? new Date(item.updatedAt)
                        .toISOString()
                        .slice(0, 16)
                        .replace("T", " ")
                    : "",
                  "Farmer System ID": "",
                  "Farmer Name": `${item.updatedByUserName} set status to '${item.status}'`,
                })
              );
              mappedData.push({ No: "Beneficiary Count", "Payment Date": payments.length });
              mappedData.push({ No: "Paid Count", "Payment Date": payments.filter((item: any) => item.status === "Complete").length });
              mappedData.push({ No: "Failed Count", "Payment Date": payments.filter((item: any) => item.status !== "Complete").length });
              mappedData.push({ No: "" });
              mappedData.push({
                No: "Generated By",
                "Payment Date": payments[0].currentUserName,
              });

              const worksheet = XLSX.utils.json_to_sheet(mappedData);
              XLSX.utils.book_append_sheet(
                workbook,
                worksheet,
                sheetName.slice(0, 31)
              );
            }

            const blob = new Blob(
              [XLSX.write(workbook, { bookType: "xlsx", type: "array" })],
              {
                type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
              }
            );

            const filename = `Payment_Report_${moment().format(
              "YYYY-MM-DD_HHmm"
            )}.xlsx`;
            saveAs(blob, filename);

            resolve();
          } catch (error) {
            reject(error);
          }
        }),
        {
          pending: {
            render() {
              return "Exporting Payment report...";
            },
            autoClose: false, // Manual dismiss with X button
            closeButton: true,
          },
          success: {
            render() {
              return "Payment report exported successfully!";
            },
            autoClose: 5000,
            closeButton: true,
          },
          error: {
            render() {
              return "Failed to export Payment report.";
            },
            autoClose: 5000,
            closeButton: true,
          },
        }
      );
    } catch (error) {
      console.error("Error generating payment report:", error);
      toast.error("Failed to export payment report.");
    }
  };

  const bindPaymentReports = async () => {
    try {
      const data: any = {
        pageNumber: 1,
        pageSize: 10000,
        status: paymentStatus,
      };
      const response = await reportService.getPaymentReports({
        startDate: startDate
          ? moment(startDate).format("YYYY-MM-DD")
          : new Date(),
        endDate: endDate ? moment(endDate).format("YYYY-MM-DD") : new Date(),
        paymentBatchIds: paymentBatchIds,
        countryId: countryId || "",
      });
      if (response) {
        setRowData(response);
      }
    } catch (error) {
      console.error("Failed to bind import history:", error);
    }
  };
  useEffect(() => {
    bindPaymentReports();
  }, [startDate, endDate, paymentBatchIds, countryId]);

  useEffect(() => {
    bindPaymentBatch();
    const stored = localStorage.getItem(AUTH_LOCAL_STORAGE_KEY);
    const code = localStorage.getItem("selected_country_code");

    if (stored) {
      try {
        const parsed = JSON.parse(stored);
        const userCountries = parsed.countries ?? [];

        const defaultCountry =
          userCountries.find((c: any) => c.code === code) || userCountries[0];

        setCountryId(defaultCountry.id);
      } catch (err) {
        console.error("Failed to parse countries from localStorage:", err);
      }
    }
  }, []);

  return (
    <>
      {" "}
      <div className="container m-4">
        <PageTitleWrapper />
        <PageTitle breadcrumbs={profileBreadCrumbs}>Payment Reports </PageTitle>

        <div className="row">
          {/* Left: Filter Card */}
          <div className="col-md-3 mb-4">
            <div className="card bg-white shadow-sm rounded-4 ">
              <div className="card-header bg-light border-0 pb-2 px-4">
                <h3 className="card-title fw-bold fs-5 text-dark mb-0">
                  Filter
                </h3>
              </div>

              <div className="card-body px-4 pt-3 d-flex flex-column gap-4">
                <div>
                  <div>
                    <label htmlFor="endDate" className="form-label fw-semibold">
                      Start Date
                    </label>
                  </div>
                  <DatePicker
                    selected={startDate ?? new Date()}
                    onChange={(date) => setStartDate(date)}
                    className="form-control w-100"
                    id="startDate"
                    dateFormat="yyyy-MM-dd"
                    placeholderText="Select start date"
                  />
                </div>
                {/* End Date Picker */}
                <div>
                  <div>
                    <label htmlFor="endDate" className="form-label fw-semibold">
                      End Date
                    </label>
                  </div>
                  <DatePicker
                    selected={endDate ?? new Date()}
                    onChange={(date) => setEndDate(date)}
                    className="form-control w-100"
                    dateFormat="yyyy-MM-dd"
                    id="endDate"
                  />
                </div>

                <div>
                  <label
                    htmlFor="paymentStatus"
                    className="form-label fw-semibold"
                  >
                    Payment Status
                  </label>
                  <select
                    id="status"
                    className="form-control"
                    value={paymentStatus}
                    onChange={(e) => setPaymentStatus(Number(e.target.value))}
                  >
                    <option value={-1}>All</option>
                    <option value={1}>Completed</option>
                    <option value={0}>Pending</option>
                  </select>
                </div>

                {/* Loan Product Multi-Select */}
                <div>
                  <label
                    htmlFor="loanProduct"
                    className="form-label fw-semibold"
                  >
                    Payment Batch
                  </label>
                  <Select
                    id="paymentBatch"
                    isMulti
                    className="basic-multi-select"
                    classNamePrefix="select"
                    options={paymentBatches}
                    value={
                      Array.isArray(paymentBatchIds)
                        ? paymentBatches?.filter((opt: any) =>
                            paymentBatchIds.includes(opt.value)
                          )
                        : []
                    }
                    onChange={(selectedOptions) =>
                      setPaymentBatchIds(
                        selectedOptions?.map((opt) => opt?.value) || []
                      )
                    }
                  />
                </div>

                {/* Generate Button */}
                <div>
                  <button
                    className="btn btn-theme w-100 fw-semibold"
                    onClick={generatePdfPreview}
                    disabled={loading === true}
                  >
                    Generate Report
                  </button>
                </div>

                {/* Excel Download Button */}
                {rowData && (
                  <div>
                    <button
                      className="btn btn-primary w-100 fw-semibold"
                      onClick={downloadPaymentReport}
                      disabled={loading === true}
                    >
                      Download Excel
                    </button>
                  </div>
                )}
                {pdfUrl && (
                  <button
                    className="btn btn-secondary m-2"
                    onClick={downloadPdf}
                    disabled={loading === true}
                  >
                    Download PDF{" "}
                  </button>
                )}
              </div>
            </div>
          </div>

          {/* Right: Table Section */}
          <div className="col-md-9">
            <div className="card bg-white shadow-sm rounded-4 p-4">
              <div className="card-body px-0 pt-3">
                <div className="w-100 h-300 d-flex flex-row align-items-center justify-content-between">
                  {/* begin::Modal title */}
                  <div>
                    <h2 className="fw-semibold">Payment Report PDF Preview</h2>
                  </div>
                </div>
              </div>
              <div className="modal-body">
                <div style={{ height: "500px" }}>
                  <Worker
                    workerUrl={`https://unpkg.com/pdfjs-dist@3.11.174/build/pdf.worker.min.js`}
                  >
                    {pdfUrl && <Viewer fileUrl={pdfUrl} />}
                  </Worker>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </>
  );
};

export default PaymentBatchSummary;

// <div className="text-end m-3">
//       {/* <Button variant="theme" className="me-2">
//         Download PDF
//       </Button> */}
//       <Button variant="theme" onClick={downloadInterimList}>Download Excel</Button>
//     </div>

// <div className="row">
//     <div className="col-md-4">
//       <Card className="mb-3 ">
//         <Card.Body>
//           <h5>Successful Transactions</h5>
//           {stats !== undefined && (
//             <Pie
//               data={{
//                 labels: ["Successful", "Unsuccessful"],
//                 datasets: [
//                   {
//                     data: [
//                       stats.successfulTransactions,
//                       stats.totalPayment - stats.successfulTransactions,
//                     ],
//                     backgroundColor: ["#28a745", "#dc3545"], // Bootstrap green and red
//                     borderWidth: 1,
//                   },
//                 ],
//               }}
//               options={{
//                 plugins: {
//                   legend: {
//                     position: "bottom",
//                   },
//                 },
//               }}
//             />
//           )}
//           <p className="mt-2 d-flex justify-content-center">
//             {stats?.successfulTransactions} / {stats?.totalPayment}{" "}
//             Transactions Successful
//           </p>
//         </Card.Body>
//       </Card>
//     </div>

//     <div className="col-md-8">
//       <div className="row ">
//         {/* Card 1 */}
//         <div className="col-md-6 d-flex mb-3">
//           <Card className="mb-3 w-100 h-100">
//             <Card.Body className="d-flex flex-column">
//               <h5 className="d-flex align-items-center gap-2">

//                 <FaUsers /> Total Payment Beneficiaries
//               </h5>
//               <p>{stats?.totalBeneficiaries} Beneficiaries</p>
//             </Card.Body>
//           </Card>
//         </div>

//         {/* Card 2 */}
//         <div className="col-md-6 d-flex mb-3">
//           <Card className="mb-3 w-100 h-100">
//             <Card.Body className="d-flex flex-column">
//               <h5 className="d-flex align-items-center gap-2">

//                 <FaMoneyBill />
//                 Total Amount to be Paid Out
//               </h5>
//               <p>{stats?.totalAmount}</p>
//             </Card.Body>
//           </Card>
//         </div>

//         {/* Card 3 */}
//         <div className="col-md-6 d-flex mb-3">
//           <Card className="mb-3 w-100 h-100">
//             <Card.Body className="d-flex flex-column">
//               <h5 className="d-flex align-items-center gap-2">
//                 <FaPiggyBank />
//                 Total Loan Deductions
//               </h5>
//               <p>{stats?.totalLoanDeductions}</p>
//             </Card.Body>
//           </Card>
//         </div>

//         {/* Card 4 */}
//         <div className="col-md-6 d-flex mb-3">
//           <Card className="mb-3 w-100 h-100">
//             <Card.Body className="d-flex flex-column">
//               <h5 className="d-flex align-items-center gap-2" >
//                 <FaChartBar />
//                 Payment Statistics</h5>
//               <p>
//                 Total Payment Costs: {stats?.totalPaymentCost} | Failed
//                 Transactions: {stats?.failedTransactions} (10%)
//               </p>
//             </Card.Body>
//           </Card>
//         </div>
//       </div>
//     </div>
//   </div>
