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
const CountryLoanPortfolioReport: React.FC<ReportProps> = ({ title, reportData }) => {
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

  const [colDefs, setColDefs] = useState<any>([
    {
      headerName: "Loan account number",
      flex: 1,
      cellRenderer: MainLinkActionComponent,
    },
    { headerName: "Loan Product", field: "loanBatch.name", flex: 1 },
    {
      headerName: "Interest Rate",
      field: "loanBatch.interestRate",
      flex: 1,
      valueFormatter: (params: any) => `${params.value}%`,
    },
    { headerName: "Loan Term", field: "loanBatch.tenure", flex: 1 },
    { headerName: "Farmer System Id", field: "farmer.systemId", flex: 1 },

    { headerName: "Farmer Name", field: "farmer.fullName", flex: 1 },
    { headerName: "Farmer Mobile", field: "farmer.mobile", flex: 1 },
    { headerName: "Amount Disbursed", field: "principalAmount", flex: 1 },
    {
      headerName: "[Per schedule] Principal Due to Date",
      field: "principalDue",
      flex: 1,
    },
    {
      headerName: "Principal Received to date",
      field: "principalReceived",
      flex: 1,
    },
    { headerName: "Principal Balance", field: "remainingBalance", flex: 1 },
    {
      headerName: " Principal in Arrears ",
      field: "principalArrears",
      flex: 1,
    },
    { headerName: "Total Interest", field: "totalInterest", flex: 1 },
    {
      headerName: "Per schedule Interest due to date",
      field: "interestDue",
      flex: 1,
    },
    {
      headerName: "Interest Received to Date",
      field: "interestReceived",
      flex: 1,
    },
    { headerName: "Interest Arrears", field: "interestArrears", flex: 1 },
    { headerName: "Total Arrears", field: "totalArrears", flex: 1 },
    { headerName: "Total Expected", field: "totalExpected", flex: 1 },
    { headerName: "Arrears Rate [%]", field: "arrearsRate", flex: 1 },
  ]);

  const bindApplications = async (props: any) => {
    try {
      const response = await reportsService.getCountryApplicationReports(props);
      console.log("Application report response", response);
      
      if (response) {
        setRowData(response);
      } else {
        setRowData([]);
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
debugger
    const mappedData: any[] = []; // collect totals across all batches

    for (const batch of loanBatches) {
      const selectedBatch = loanBatches.find((b: any) => b.value === batch.value);

      const filteredData = rowData.filter(
        (row: any) => row.loanBatch.id === batch.value
      );

      if (filteredData.length === 0) continue;

      hasData = true;

      let totalDisbursed = 0;
      let totalPrincipalDue = 0;
      let totalPrincipalReceived = 0;
      let totalPrincipalBalance = 0;
      let totalPrincipalArrears = 0;
      let totalInterest = 0;
      let totalInterestDue = 0;
      let totalInterestReceived = 0;
      let totalInterestArrears = 0;
      let totalArrears = 0;
      let totalExpected = 0;

      filteredData.forEach((row: any) => {
        totalDisbursed += row.principalAmount ?? 0;
        totalPrincipalDue += row.principalDue ?? 0;
        totalPrincipalReceived += row.principalReceived ?? 0;
        totalPrincipalBalance += row.remainingBalance ?? 0;
        totalPrincipalArrears += row.principalArrears ?? 0;
        totalInterest += row.totalInterest ?? 0;
        totalInterestDue += row.interestDue ?? 0;
        totalInterestReceived += row.interestReceived ?? 0;
        totalInterestArrears += row.interestArrears ?? 0;
        totalArrears += row.totalArrears ?? 0;
        totalExpected += row.totalExpected ?? 0;
      });

      // push summary row for this batch
      mappedData.push({
        "Loan product": selectedBatch?.label ?? "Unknown Product",
        "Interest rate (P.A.)": selectedBatch?.interestRate ?? "",
        "Tenure(in months)": selectedBatch?.tenure ?? "",
        "Amount Disbursed": totalDisbursed,
        "[Per schedule] Principal Due to Date": totalPrincipalDue,
        "Principal Received to date": totalPrincipalReceived,
        "Principal Balance": totalPrincipalBalance,
        "Principal in Arrears": totalPrincipalArrears,
        "Total Interest": totalInterest,
        "Per schedule interest due to date": totalInterestDue,
        "Interest Received to Date": totalInterestReceived,
        "Interest Arrears": totalInterestArrears,
        "Total Arrears": totalArrears,
        "Total Expected": totalExpected,
        "Arrears Rate [%]": totalArrears > 0 ? (totalArrears / totalExpected) * 100 : 0,
      });
    }

    // add footer row
    if (hasData) {
      mappedData.push({
        "Loan product": "Generated By",
        "Interest rate (P.A.)": rowData[0]?.currentUserName ?? "Unknown",
      });
    }

    if (!hasData) {
      throw new Error("No data found for the selected loan products.");
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
    const filename = `Country_Loan_portfolio_report_${timestamp}.xlsx`;
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
      interestRate: opt?.interestRate,
      tenure: opt?.tenure,

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
         statusId: selectedStatusId ,
      };

      await toast.promise(
        (async () => {
          setLoading(true);
          const response = await axios.post(
            `${getAPIBaseUrl()}api/Reports/GenerateCountryLoanPortfolioPdfReport`,
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

   

    const data = {
      endDate: endDate,
      batchIds: loanProductIds,
      countryId: countryId,
      officerId: officerId,
       statusId: selectedStatusId ,
    };

    await toast.promise(
      (async () => {
        setLoading(true);
        const response = await axios.post(
          `${getAPIBaseUrl()}api/Reports/GenerateCountryLoanPortfolioPdfReport`,
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
        a.download = `CountryLoanPortfolioReport_${timestamp}.pdf`;
        document.body.appendChild(a);
        a.click();
        a.remove();
      })(),
      {
        pending: {
          render() {
            return "Generating PDF report...";
          },
          autoClose: 5000, // Manual dismiss with X button
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
         bindApplications({
        endDate:  new Date(),
        batchIds: loanProductIds,
        countryId: defaultCountry.id ,
        officerId : officerId ,
        statusId: selectedStatusId ,
      });
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
                  <h2 className="fw-semibold">Country Portfolio PDF Preview</h2>
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

export default CountryLoanPortfolioReport;
