import { SetStateAction, useEffect, useState } from "react";

import { IConfirmModel } from "../../../../_models/confirm-model";
import { useNavigate } from "react-router-dom";
import { useDispatch, useSelector } from "react-redux";
import {
  getAPIBaseUrl,
  isAllowed,
} from "../../../../_metronic/helpers/ApiUtil";
import moment from "moment";
import { Content } from "../../../../_metronic/layout/components/content";
import CustomTable from "../../../../_shared/CustomTable/Index";
import { Error401 } from "../../errors/components/Error401";
import { addToLoanApplication } from "../../../../_features/loan/loanApplicationSlice";
import ReportService from "../../../../services/ReportService";
import * as XLSX from "xlsx";
import saveAs from "file-saver";
import { Worker, Viewer } from "@react-pdf-viewer/core";
import "@react-pdf-viewer/core/lib/styles/index.css";
import { pdfjs } from "react-pdf";
import DatePicker from "react-datepicker";
import Select from "react-select";
import LoanBatchService from "../../../../services/LoanBatchService";
import "react-datepicker/dist/react-datepicker.css";
import { KTIcon } from "../../../../_metronic/helpers";
import axios from "axios";
import { AUTH_LOCAL_STORAGE_KEY } from "../../auth";
import { toast } from "react-toastify";
import "../toast_custom.css";
import UserService from "../../../../services/UserService";
import ApplicationStatusService from "../../../../services/ApplicationStatusService";

pdfjs.GlobalWorkerOptions.workerSrc = `//cdnjs.cloudflare.com/ajax/libs/pdf.js/${pdfjs.version}/pdf.worker.js`;

const reportsService = new ReportService();
const userService = new UserService();
const applicationStatusService = new ApplicationStatusService();



interface ReportProps {
  title: string;
  reportData: any;
}
const loanBatchService = new LoanBatchService();
const GlobalLoanPortfolioReport: React.FC<ReportProps> = ({ title, reportData }) => {
  const navigate = useNavigate();
  const dispatch = useDispatch();
  const [rowData, setRowData] = useState<any>();
  const [open, setOpen] = useState(false);
  const [endDate, setEndDate] = useState<Date | null>(new Date());
  const [loanProductIds, setLoanProductIds] = useState<any>(null);
  const [pdfUrl, setPdfUrl] = useState<string | null>(null);
  const [loading, setLoading] = useState(false);
  const [countryId, setCountryId] = useState<any>("");
  const [loanOfficers, setLoanOfficers] = useState<any[]>([]);
  const [officerId, setOfficerId] = useState<any>("");
 const [actions, setActions] = useState<any>();
  const [selectedStatusId, setSelectedStatusId] = useState("");
  const toggle = () => setOpen(!open);

  const linkButtonHandler = (value: any) => {
    dispatch(addToLoanApplication(value));
    navigate("/loans/applications/detail");
  };

  const MainLinkActionComponent = (props: any) => {
    return (
      <button
        className="btn btn-default link-primary mx-0 px-1"
        onClick={() => linkButtonHandler(props.data)}
      >
        {props.data.loanNumber}
      </button>
    );
  };

  

  const bindApplications = async (props: any) => {
    try {
      const response = await reportsService.getGlobalLoanPortfolioReports(props);
      console.log("Application report response", response);

      if (response) {
        setRowData(response);
      } else {
        setRowData([]);
        toast.error("No data found");
      }
    } catch (error) {
      console.error("Failed to fetch application reports", error);
      setRowData([]);
    }
  };

const handleExcelExport = async () => {
  const exportExcel = async () => {
    const workbook = XLSX.utils.book_new();
    let hasData = false;

    const mappedData: any[] = [];

    // totals accumulator
    let netDisbursed = 0;
    let netPrincipalDue = 0;
    let netPrincipalReceived = 0;
    let netPrincipalBalance = 0;
    let netPrincipalArrears = 0;
    let netInterest = 0;
    let netInterestDue = 0;
    let netInterestReceived = 0;
    let netInterestArrears = 0;
    let netTotalArrears = 0;
    let netTotalExpected = 0;

    if (rowData && rowData.length > 0) {
      hasData = true;

      rowData.forEach((row: any) => {
        const amountDisbursed = row.amountDisbursed ?? 0;
        const principalDue = row.principalDue ?? 0;
        const principalReceived = row.principalReceived ?? 0;
        const principalBalance = row.principalBalance ?? 0;
        const principalArrears = row.principalArrears ?? 0;
        const totalInterest = row.totalInterest ?? 0;
        const interestDue = row.interestDue ?? 0;
        const interestReceived = row.interestReceived ?? 0;
        const interestArrears = row.interestArrears ?? 0;
        const totalArrears = row.totalArrears ?? 0;
        const totalExpected = row.totalExpected ?? 0;

        // push country row
        mappedData.push({
          "Country Name": row.countryName ?? "Unknown Country",
          "Amount Disbursed": amountDisbursed,
          "[Per schedule] Principal Due to Date": principalDue,
          "Principal Received to date": principalReceived,
          "Principal Balance": principalBalance,
          "Principal in Arrears": principalArrears,
          "Total Interest": totalInterest,
          "Per schedule interest due to date": interestDue,
          "Interest Received to Date": interestReceived,
          "Interest Arrears": interestArrears,
          "Total Arrears": totalArrears,
          "Total Expected": totalExpected,
          "Arrears Rate [%]":
            totalExpected > 0 ? ((totalArrears / totalExpected) * 100).toFixed(2) : "0.00",
        });

        // accumulate net totals
        netDisbursed += amountDisbursed;
        netPrincipalDue += principalDue;
        netPrincipalReceived += principalReceived;
        netPrincipalBalance += principalBalance;
        netPrincipalArrears += principalArrears;
        netInterest += totalInterest;
        netInterestDue += interestDue;
        netInterestReceived += interestReceived;
        netInterestArrears += interestArrears;
        netTotalArrears += totalArrears;
        netTotalExpected += totalExpected;
      });

      // push net totals row
      mappedData.push({
        "Country Name": "Net Totals",
        "Amount Disbursed": netDisbursed,
        "[Per schedule] Principal Due to Date": netPrincipalDue,
        "Principal Received to date": netPrincipalReceived,
        "Principal Balance": netPrincipalBalance,
        "Principal in Arrears": netPrincipalArrears,
        "Total Interest": netInterest,
        "Per schedule interest due to date": netInterestDue,
        "Interest Received to Date": netInterestReceived,
        "Interest Arrears": netInterestArrears,
        "Total Arrears": netTotalArrears,
        "Total Expected": netTotalExpected,
        "Arrears Rate [%]":
          netTotalExpected > 0 ? ((netTotalArrears / netTotalExpected) * 100).toFixed(2) : "0.00",
      });
    }

    // add footer row
    if (hasData) {
      mappedData.push({
        "Country Name": "Generated By",
        "Amount Disbursed": rowData[0]?.currentUserName ?? "Unknown",
      });
    }

    if (!hasData) {
      throw new Error("No data found for the selected countries.");
    }

    // convert summary to worksheet
    const worksheet = XLSX.utils.json_to_sheet(mappedData);
    XLSX.utils.book_append_sheet(workbook, worksheet, "CountryLoanReport");

    const blob = new Blob(
      [XLSX.write(workbook, { bookType: "xlsx", type: "array" })],
      {
        type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
      }
    );

    const timestamp = moment().format("YYYY-MM-DD_HH-mm");
    const filename = `Global_Loan_portfolio_report_${timestamp}.xlsx`;
    saveAs(blob, filename);
  };

  toast.promise(exportExcel(), {
    pending: {
      render() {
        return "Downloading excel...";
      },
      autoClose: false,
      closeButton: true,
    },
    success: {
      render() {
        return "Excel file downloaded!";
      },
      autoClose: 5000,
      closeButton: true,
    },
    error: {
      render() {
        return "Failed to download excel file.";
      },
      autoClose: 5000,
      closeButton: true,
    },
  });
};



  const [loanBatches, setLoanBatches] = useState<any>();

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

  const generatePdfPreview = async () => {
    try {
      if (!endDate) {
        setEndDate(new Date());
      }

     
      const data = {
        endDate: endDate,
        batchIds: loanProductIds,
        countryId: countryId,
        officerId : officerId ,
        statusId : selectedStatusId,
      };

      await toast.promise(
        (async () => {
          setLoading(true);
          const response = await axios.post(
            `${getAPIBaseUrl()}api/Reports/GenerateGlobalLoanPortfolioPdfReport`,
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

    if (!loanProductIds || loanProductIds.length === 0) {
      window.alert("Please select at least one loan product.");
      return;
    }

    const data = {
      endDate: endDate,
      batchIds: loanProductIds,
      countryId: countryId,
      officerId: officerId,
      statusId : selectedStatusId,
    };

    await toast.promise(
      (async () => {
        setLoading(true);
        const response = await axios.post(
          `${getAPIBaseUrl()}api/Reports/GenerateGlobalLoanPortfolioPdfReport`,
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
        a.download = `GlobalLoanPortfolioReport_${timestamp}.pdf`;
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
            return "Global Loan Portfolio Report downloaded!";
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
  
  
  const fetchUsersWithPermission = async () => {

    const userData = await userService.getOfficers();
    if (userData && userData.length > 0) {


      setLoanOfficers(userData);

    };
  }
  const bindActions = async () => {
    
      const data = {
        pageNumber: 1,
        pageSize: 10000,
      };


      const response = await applicationStatusService.getStatusData(data);
      response.unshift({ id: "", name: 'All' });
      setActions(response)
    }

  useEffect(() => {
    bindBatches([]);
fetchUsersWithPermission();
bindActions();
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


  


  useEffect(() => {
    if (endDate || loanProductIds) {
      bindApplications({
        endDate: endDate ? moment(endDate).format("YYYY-MM-DD") : new Date(),
        batchIds: loanProductIds,
        countryId: countryId || "",
        officerId : officerId ,
        statusId: selectedStatusId ,
      });
    }
  }, [endDate, loanProductIds, countryId,officerId, selectedStatusId]);


   const officerSelected = (val: any) => {
    const selectedItem = loanOfficers.filter(
      (product: any) => product.id === val
    )[0];
    setOfficerId(selectedItem.id);

  };

  return (
    <>
      <div className="row">
        {/* Left: Filter Card */}
        <div className="col-md-3 mb-4">
          <div className="card bg-white shadow-sm rounded-4 ">
            <div className="card-header bg-light border-0 pb-2 px-4">
              <h3 className="card-title fw-bold fs-5 text-dark mb-0">Filter</h3>
            </div>

            <div className="card-body px-4 pt-3 d-flex flex-column gap-4">
              {/* End Date Picker */}
              <div>
                <div>
                  <label htmlFor="endDate" className="form-label fw-semibold">
                    Reporting Date
                  </label>
                </div>
                <DatePicker
                  selected={endDate ?? new Date()}
                  onChange={(date) => setEndDate(date)}
                  className="form-control w-100"
                  id="endDate"
                />
              </div>

              {/* Loan Product Multi-Select */}
              {/* <div>
                <label htmlFor="loanProduct" className="form-label fw-semibold">
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
              </div> */}
                   {/* <div >
              <label htmlFor="loanProduct" className="form-label fw-semibold">
                  Business Development Officer
                </label>
                <select
                  name="values"
                  onChange={(e: any) => officerSelected(e.target.value)}
                  value={officerId !== null ? officerId : ""}
                  className="form-control"
                >
                  <option value="">All </option>
                  {loanOfficers &&
                    loanOfficers.map((opt: any) => (
                      <option key={opt.id} value={opt.id}>
                        {opt.username}
                      </option>
                    ))}
                </select>
              </div> */}
              <div>
                <label className='form-label fw-semibold'>Loan Status</label>
            <select
              className='form-control'
              value={selectedStatusId !== null ? selectedStatusId : ""}
              onChange={(e : any) => setSelectedStatusId(e.target.value)}
            >

              {actions && actions.map((item: any) => (
                <option key={item.id} value={item.id}>
                  {item.name}
                </option>
              ))}
            </select>

            
              </div>

              {/* Generate Button */}
              <div>
                <button
                  className="btn btn-theme w-100 fw-semibold"
                  onClick={generatePdfPreview}
                  disabled={loading}
                >
                  Generate Report
                </button>
              </div>

              {/* Excel Download Button */}
              {rowData && (
                <div>
                  <button
                    className="btn btn-primary w-100 fw-semibold"
                    onClick={handleExcelExport}
                    disabled={loading}
                  >
                    Download Excel
                  </button>
                </div>
              )}
              {pdfUrl && (
                <button
                  className="btn btn-secondary m-2"
                  onClick={downloadPdf}
                  disabled={loading}
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
                  <h2 className="fw-semibold">Portfolio PDF Preview</h2>
                </div>
              </div>
            </div>
            <div className="modal-body">
              <label>
                {" "}
                <b>Note</b> : The pdf will constitute the latest report of the
                loan products.
              </label>
              {/* {pdfUrl && (
                <iframe
                  src={pdfUrl}
                  style={{ width: "100%", height: "800px", border: "none" }}
                  title="Loan Portfolio PDF"
                />
              )} */}
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
    </>
  );
};

export default GlobalLoanPortfolioReport;
