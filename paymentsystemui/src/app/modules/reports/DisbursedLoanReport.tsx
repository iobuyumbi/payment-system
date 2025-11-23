import { Line } from "react-chartjs-2";
import { Card, Button } from "react-bootstrap";
import { PageLink, PageTitle } from "../../../_metronic/layout/core";
import { PageTitleWrapper } from "../../../_metronic/layout/components/toolbar/page-title";
import ReportService from "../../../services/ReportService";
import { useEffect, useState } from "react";
import * as XLSX from "xlsx";

import saveAs from "file-saver";
import moment from "moment";
import Select from "react-select";
import { pdfjs } from "react-pdf";
import { Worker, Viewer } from "@react-pdf-viewer/core";
import "@react-pdf-viewer/core/lib/styles/index.css";
import "./toast_custom.css";

pdfjs.GlobalWorkerOptions.workerSrc = `//cdnjs.cloudflare.com/ajax/libs/pdf.js/${pdfjs.version}/pdf.worker.js`;
import {
  FaBoxOpen,
  FaReceipt,
  FaBalanceScaleLeft,
  FaPercentage,
} from "react-icons/fa";
import DatePicker from "react-datepicker";
import LoanBatchService from "../../../services/LoanBatchService";
import { toast } from "react-toastify";
import axios from "axios";
import { getAPIBaseUrl } from "../../../_metronic/helpers/ApiUtil";
import { AUTH_LOCAL_STORAGE_KEY } from "../auth";
import { start } from "repl";

const loanBatchService = new LoanBatchService();
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
const DisbursedLoanReport = () => {
  const [rowData, setRowData] = useState<any>();
  const [stats, setStats] = useState<any>();
  const [loading, setLoading] = useState(false);
  const [endDate, setEndDate] = useState<Date | null>(new Date());
  const [pdfUrl, setPdfUrl] = useState<string | null>(null);
  const [countryId, setCountryId] = useState<any>("");
  const [loanProductIds, setLoanProductIds] = useState<any>(null);
  const [startDate, setStartDate] = useState<Date | null>(new Date());
  const [loanBatches, setLoanBatches] = useState<any>();

  const bindData = async () => {
    const response = await reportService.getDisbursedLoanReports({
      startDate: startDate
        ? moment(startDate).format("YYYY-MM-DD")
        : new Date(),
      endDate: endDate ? moment(endDate).format("YYYY-MM-DD") : new Date(),
      batchIds: loanProductIds,
      countryId: countryId || "",
    });

    setRowData(response);
  };

  useEffect(() => {
    bindData();
  }, [startDate, endDate, loanProductIds, countryId]);

  const downloadLoanDisbursementReport = async () => {
    if (!loanProductIds?.length || !loanBatches || !rowData) {
      toast.error("Missing required data.");
      return;
    }

    const exportExcel = async () => {
      const timeoutPromise = new Promise<never>((_, reject) =>
        setTimeout(
          () =>
            reject(new Error("Report generation timed out after 30 seconds.")),
          30000
        )
      );

      const exportPromise = (async () => {
        const workbook = XLSX.utils.book_new();
        let hasData = false;

        for (const batchId of loanProductIds) {
          const selectedBatch = loanBatches.find(
            (b: any) => b.value === batchId
          );
          const sheetName = selectedBatch?.label ?? "Unknown Product";

          const filteredData = rowData.filter(
            (row: any) => row.batchId === batchId
          );
          if (!filteredData || filteredData.length === 0) continue;

          hasData = true;

          // Totals
          let totalPrincipalAmount = 0;
          let totalFeesApplied = 0;
          let totalEffectivePrincipal = 0;
          let totalExpectedInterest = 0;
          let totalInterest = 0;

          const mappedData = filteredData.map((row: any) => {
            const principalAmount = row.principalAmount ?? 0;
            const feesApplied = row.feesApplied ?? 0;
            const effectivePrincipal =
              row.effectivePrincipal ?? principalAmount;
            const expectedInterest = row.expectedInterestPerSchedule ?? 0;
            const interest = row.interest ?? 0;

            totalPrincipalAmount += principalAmount;
            totalFeesApplied += feesApplied;
            totalEffectivePrincipal += effectivePrincipal;
            totalExpectedInterest += expectedInterest;
            totalInterest += interest;

            return {
              "Loan Account": row.loanNumber,
              "Disbursement Date": moment(row.disbursedOn).format(
                "YYYY-MM-DD HH:mm"
              ),
              "Maturity Date": row.maturityDate
                ? moment(row.maturityDate).format("YYYY-MM-DD HH:mm")
                : "N/A",
              "Interest": interest,
              "Loan Term": row.loanTerm,
              "Farmer System ID": row.farmerSystemId,
              "Farmer Name": row.farmerName,
              "Principle Amount": principalAmount,
              "Fees Applied": feesApplied,
              "Effective Principle": effectivePrincipal,
              "Expected Interest (Per Schedule)": expectedInterest,
            };
          });

          mappedData.push({
            "Loan Account": "TOTAL",
            "Disbursement Date": "",
            "Maturity Date": "",
            "Interest": totalInterest,
            "Loan Term": "",
            "Farmer System ID": "",
            "Farmer Name": "",
            "Principle Amount": totalPrincipalAmount,
            "Fees Applied": totalFeesApplied,
            "Effective Principle": totalEffectivePrincipal,
            "Expected Interest (Per Schedule)": totalExpectedInterest,
          });
        

          
          mappedData.push({
            "Loan Account": "Generated By",
            "Disbursement Date": filteredData[0].currentUserName,
           
          });

          const worksheet = XLSX.utils.json_to_sheet(mappedData);
          XLSX.utils.book_append_sheet(workbook, worksheet, sheetName);
        }

        if (!hasData) {
          throw new Error("No data found for the selected loan products.");
        }

        const blob = new Blob(
          [XLSX.write(workbook, { bookType: "xlsx", type: "array" })],
          {
            type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
          }
        );

        const timestamp = moment().format("YYYY-MM-DD_HH-mm");
        const filename = `Disbursed_Loan_Report_${timestamp}.xlsx`;
        saveAs(blob, filename);
      })();

      return Promise.race([exportPromise, timeoutPromise]);
    };

    toast.promise(exportExcel(), {
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
    });
  };

  const isSameDate = (date1: Date, date2: Date): boolean => {
    return (
      date1.getFullYear() === date2.getFullYear() &&
      date1.getMonth() === date2.getMonth() &&
      date1.getDate() === date2.getDate()
    );
  };

  const generatePdfPreview = async () => {
    debugger;

    if (!startDate) {
      setStartDate(new Date());
    }
    if (!endDate) {
      setEndDate(new Date());
    }

    if (startDate && endDate && isSameDate(startDate, endDate)) {
      window.alert("Please select a valid date range.");
      return;
    }

    if (!loanProductIds || loanProductIds.length === 0) {
      window.alert("Please select at least one loan product.");
      return;
    }

    const data = {
      startDate,
      endDate,
      batchIds: loanProductIds,
      countryId,
    };

    setLoading(true); // Start loading before toast.promise

    try {
      await toast.promise(
        (async () => {
          const response = await axios.post(
            `${getAPIBaseUrl()}api/Reports/GetDisbursedLoanReportsPdf`,
            data,
            {
              headers: { Accept: "application/pdf" },
              responseType: "blob",
            }
          );

          if (response.status !== 200) {
            throw new Error("Failed to fetch PDF data");
          }

          const blob = new Blob([response.data], { type: "application/pdf" });
          const fileUrl = URL.createObjectURL(blob);
          setPdfUrl(fileUrl);
        })(),
        {
          pending: {
            render() {
              return "Loading PDF preview...";
            },
            autoClose: false,
            closeButton: true,
          },
          success: {
            render() {
              return "PDF preview ready!";
            },
            autoClose: 5000,
            closeButton: true,
          },
          error: {
            render() {
              return "Failed to load PDF preview.";
            },
            autoClose: 5000,
            closeButton: true,
          },
        }
      );
    } catch (err) {
      console.error(err);
    } finally {
      setLoading(false); // Ensures loading state resets no matter what
    }
  };

  const downloadPdf = async () => {
    if (!endDate) {
      setEndDate(new Date());
    }

    if (!loanProductIds || loanProductIds.length === 0) {
      window.alert("Please select at least one loan product.");
      return;
    }

    const data = {
      startDate: startDate,
      endDate: endDate,
      batchIds: loanProductIds,
      countryId: countryId,
    };

    await toast.promise(
      (async () => {
        setLoading(true);
        const response = await axios.post(
          `${getAPIBaseUrl()}api/Reports/GetDisbursedLoanReportsPdf`,
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
        a.download = `Disbursed_Loan_Report_${timestamp}.pdf`;
        document.body.appendChild(a);
        a.click();
        a.remove();
      })(),
      {
        pending: {
          render() {
            return "Generating PDF report...";
          },
          autoClose: false, // Manual dismiss with X button
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
          render() {
            return "Failed to generate PDF.";
          },
          autoClose: 5000,
          closeButton: true,
        },
      }
    );
    setLoading(false);
  };

  const bindBatches = async (projectIds: any) => {
    const data: any = {
      pageNumber: 1,
      pageSize: 10000,
    };
    const response = await loanBatchService.getLoanBatchData(data);

    const options: any[] = response.map((opt: any) => ({
      value: opt?.id,
      label: opt?.name,
    }));
    console.log("Loan batches response", response);
    setLoanBatches(options);
  };

  useEffect(() => {
    bindBatches([]);
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
        <div>
          <PageTitleWrapper />
          <PageTitle breadcrumbs={profileBreadCrumbs}>
            Loan Disbursement Report
          </PageTitle>
        </div>

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

                {/* Loan Product Multi-Select */}
                <div>
                  <label
                    htmlFor="loanProduct"
                    className="form-label fw-semibold"
                  >
                    Loan Product
                  </label>
                  <Select
                    id="loanProduct"
                    isMulti
                    className="basic-multi-select"
                    classNamePrefix="select"
                    options={loanBatches}
                    value={
                      Array.isArray(loanProductIds)
                        ? loanBatches?.filter((opt: any) =>
                            loanProductIds.includes(opt.value)
                          )
                        : []
                    }
                    onChange={(selectedOptions) =>
                      setLoanProductIds(
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
                      onClick={downloadLoanDisbursementReport}
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
                    <h2 className="fw-semibold">Loan Disbursement Report</h2>
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

        {/* Loan Performance Graph */}

        {/* Loan Amount Breakdown */}
      </div>
    </>
  );
};

export default DisbursedLoanReport;

// <div className="row">
//           <div className="col-md-6">
//             <Card className="mb-3">
//               <Card.Body>

//                 <Line data={loanPerformanceData} />
//               </Card.Body>
//             </Card>
//           </div>
//           <div className="col-md-6">
//             <div className="row">
//               <div className="col-md-6">
//                 <Card className="mb-3">
//                   <Card.Body>
//                     <h5 className="d-flex align-items-center gap-2"><FaBoxOpen /> Items Loaned</h5>
//                     <p>
//                       {stats?.totalItemsLoaned} Items Loaned | Total Value:{" "}
//                       {stats?.totalItemValue}
//                     </p>
//                   </Card.Body>
//                 </Card>
//               </div>

//               <div className="col-md-6">
//                 <Card className="mb-3">
//                   <Card.Body>
//                     <h5 className="d-flex align-items-center gap-2"><FaReceipt /> Total Fees Charged</h5>
//                     <p>{stats?.totalFeeCharged}</p>
//                   </Card.Body>
//                 </Card>
//               </div>

//               <div className="col-md-6">
//                 <Card className="mb-3">
//                   <Card.Body>
//                     <h5 className="d-flex align-items-center gap-2"><FaBalanceScaleLeft />Amount Summary</h5>
//                     <p>
//                       Principle: {stats?.principleAmount} | Effective Balance:
//                       {stats?.effectiveBalance}
//                     </p>

//                   </Card.Body>
//                 </Card>
//               </div>
//               <div className="col-md-6">
//                 <Card className="mb-3">
//                   <Card.Body>
//                     <h5 className="d-flex align-items-center gap-2"><FaPercentage /> Interest Summary </h5>
//                     <p>
//                       Interest Earned: {stats?.interestEarned} | Effective Loan
//                       Balance:{stats?.effectiveLoanBalance}
//                     </p>
//                   </Card.Body>
//                 </Card>
//               </div>
//             </div>
//           </div>
//         </div>

{
  /* Summary Cards */
}
// <div className="text-end m-3">
//   {/* <Button variant="theme" className="me-2">
//     Download PDF
//   </Button> */}
//   <Button variant="theme" onClick={downloadInterimList}>
//     Download Excel
//   </Button>
// </div>
//  <Card className="mb-3">
//         <Card.Body>
//           <h5>Detailed Loan Amount Breakdown</h5>
//           <CustomTable
//             rowData={rowData}
//             colDefs={colDefs}
//             header=""
//             addBtnText={""}
//             importBtnText={""}
//             addBtnLink={""}
//             showImportBtn={false}
//           />
//         </Card.Body>
//       </Card>
