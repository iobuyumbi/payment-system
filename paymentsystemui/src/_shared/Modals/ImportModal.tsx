import { useEffect, useState } from "react";
import { uploadExcel } from "../../services/ExcelService";
import { KTIcon } from "../../_metronic/helpers";
import UploadLayout from "../../app/pages/UploadLayout";
import { Link, useNavigate } from "react-router-dom";
import { toast } from "react-toastify";
import axios from "axios";
import saveAs from "file-saver";
import { getAPIBaseUrl } from "../../_metronic/helpers/ApiUtil";
import * as XLSX from "xlsx";
import FarmerService from "../../services/FarmerService";
import LoanBatchService from "../../services/LoanBatchService";
import FormErrorAlert from "../FormErrorAlert/Index";
import { getSelectedCountryCode } from "../../_metronic/helpers/AppUtil";
import CountryService from "../../services/CountryService";
import UserService from "../../services/UserService";

const userService = new UserService();
const farmerService = new FarmerService();
const loanBatchService = new LoanBatchService();
const countryService = new CountryService();

export function ImportModal(props: any) {
  const {
    batchId,
    title,
    exModule,
    paymentBatchId,
    afterConfirm,
    loanBatchIds,
    loanBatchName,
    loanBatch,
  } = props;
  const navigate = useNavigate();

  const [loading, setLoading] = useState(false);
  const [errors, setErrors] = useState<any>([]);
  const [isvalid, setIsvalid] = useState<any>(false);
  const [files, setFiles] = useState<any>();
  const [showProcessing, setShowProcessing] = useState(false);
  const [loanOfficers, setLoanOfficers] = useState<any[]>([]);
  const [officerId, setOfficerId] = useState<any>("");

  const onCancel = () => {
    afterConfirm(false);
  };

  const uploadFiles = async (args: React.MouseEvent) => {
    args.preventDefault();
    // Clear error
    setErrors([]);

    if (Object.keys(errors).length > 0) {
      setIsvalid(false);
    }

    if (files.length > 0) {
      let formData: FormData = new FormData();
      for (var j = 0; j < files.length; j++) {
        formData.append("file", files[j], files[j].name);
        formData.append("module", exModule);
      }
      setLoading(true);

      const countryCode = getSelectedCountryCode();
      const countries = await countryService.getCountryData(true);
      const selectedCountry = countries?.filter(
        (c) => c.code == countryCode
      )[0];
      if (selectedCountry == null) {
        alert("Unable to bind country. Please try again.");
      }

      await uploadExcel(
        formData,
        exModule,
        paymentBatchId ?? batchId,
        selectedCountry.id
      )
        .then((response: any) => {
          //const toastId = toast.loading("Please wait...");
          if (response) {
            if (response.data && response.data.succeeded) {
              setTimeout(() => {
                setShowProcessing(false);
                if (
                  exModule === "PaymentDeductibles" ||
                  exModule === "PaymentFacilitations"
                ) {
                  navigate(`/payment-batch/details/${paymentBatchId}`);
                  afterConfirm();
                } else if (exModule === "loanApplication") {
                  navigate(
                    `/loan-batch-details/${batchId}/loan-application-imports`
                  );
                  window.location.reload();
                } else {
                  navigate("/imports/history");
                }
              }, 5000);
            } else {
              errors.errorText = response.message;
              //toast.error(response.data.succeeded == false ? "Error" : "");
              setErrors(errors);
            }
          } else {
            //toast.error(response.response.data)
            setErrors(errors);
          }
          // after upload
        })
        .catch((e: any) => {
          console.log(e);
          toast.error(e.response.data);
        })
        .finally(() => {
          setLoading(false);
          setShowProcessing(true);
        });

      setTimeout(() => {
        setLoading(false);
        setShowProcessing(true);
      }, 5000);
    }
  };

  const downloadPreFilled = (_exModule: any) => {
    switch (_exModule) {
      case "PaymentDeductibles":
        downloadPreFilledPayments();
        break;
      case "loanApplication":
        downloadPreFilledLoans();
        break;
    }
  };

  const downloadPreFilledPayments = async () => {
    setLoading(true);
    try {
      const workbook = XLSX.utils.book_new();
      const data = {
        pageNumber: 0,
        pageSize: 100000,
        paymentBatchId: props.paymentBatchId,
      };

      debugger;
      const response = await farmerService.getFarmerData(data);

      if (response == null) {
        alert("No farmers exists in the associated project(s)");
        return;
      }

      var _data = response.farmers;
      if (_data == null || _data.length == 0) {
        alert("No farmers exists in the associated project(s)");
        return;
      }

      if (Array.isArray(_data)) {
        const mappedData = _data.map((row: any) => ({
          "Uwanjani System ID": row.systemId,
          "Beneficiary Farmer Card ID": row.participantId,
          "Carbon Units Accrued": 0,
          "Unit Cost EUR": 0,
          "Total Units Earning EUR": 0,
          "Total Units Earning LC": 0,
          "Partners Adminstrative Cost": 0,
          "Farmer Earnings Share LC": 0,
          "Farmer Name": row.fullName,
          "Payment Phone Number": row.paymentPhoneNumber,

        }));

        const worksheet = XLSX.utils.json_to_sheet(mappedData);
        XLSX.utils.book_append_sheet(workbook, worksheet, "PaymentList");

        const blob = new Blob(
          [XLSX.write(workbook, { bookType: "xlsx", type: "array" })],
          {
            type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
          }
        );

        const timestamp = new Date()
          .toISOString()
          .replace(/[-:T]/g, "")
          .slice(0, 14); // e.g. 20250604
        const filename = `${loanBatchName}_${exModule}_${timestamp}.xlsx`;

        saveAs(blob, filename);
      } else {
        alert("No data available to download.");
      }
    } catch (error) {
      console.error("Failed to download carbon earnings report:", error);
      alert(
        "An error occurred while downloading the report. Please try again."
      );
    } finally {
      setLoading(false);
    }
  };

  const downloadPreFilledLoans = async () => {
    setLoading(true);
    try {
      const workbook = XLSX.utils.book_new();
      const data = {
        pageNumber: 1,
        pageSize: 1000,
        batchId:
          loanBatchIds !== null && loanBatchIds !== undefined
            ? loanBatchIds[0].value
            : batchId,
        projectId: loanBatch.projectId,
      };

      const response = await farmerService.getFarmerData(data);
      var _data = response.farmers;

      const loanItems = await loanBatchService.getLoanBatchItems(batchId);

      // 1. Farmers
      if (Array.isArray(_data)) {
        const mappedData = _data.map((row: any) => ({
          id: null,
          field_farmer_has_id: null,
          farmer_id: row.systemId,
          farmer_national_id: row.beneficiaryId,
          facilitation_cost: null,
          transport_cost: null,
          grand_total: null,
          witness_names: null,
          witness_national_id: null,
          witness_phone_number: null,
          enumerator_names: null,
          submission_time: null,
          country: row.country.countryName,
          farm_size: null,
          witness_relationship: null,
          farmer_name: row.fullName,
          "Development Officer": officerId != null ? officerId.id : "",
        }));

        const worksheet = XLSX.utils.json_to_sheet(mappedData);
        XLSX.utils.book_append_sheet(workbook, worksheet, "LoanApplication");
      }

      // 2. Items
      if (Array.isArray(loanItems)) {
        const itemData = loanItems.map((row: any) => ({
          "Farmer Id": null,
          "Item name": row.loanItem.label,
          "Price per unit": row.unitPrice,
          Quantity: row.quantity,
          Unit: row.unit.attrib1,
        }));

        const worksheetItem = XLSX.utils.json_to_sheet(itemData);
        XLSX.utils.book_append_sheet(workbook, worksheetItem, "Loan Items");
      }

      const blob = new Blob(
        [XLSX.write(workbook, { bookType: "xlsx", type: "array" })],
        {
          type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        }
      );

      const timestamp = new Date()
        .toISOString()
        .replace(/[-:T]/g, "")
        .slice(0, 14); // e.g. 20250604
      const filename = `${loanBatchName}_${exModule}_${timestamp}.xlsx`;

      saveAs(blob, filename);
    } catch (error) {
      console.error("Failed to download loan applications:", error);
      alert("An error occurred while downloading the file. Please try again.");
    } finally {
      setLoading(false);
    }
  };

  const downloadTemplate = async () => {
    // if (exModule === "farmer") {
    //   alert("There is no prebuilt template for this module");
    //   return;
    // }
    setLoading(true);
    try {
      debugger;
      const response = await axios.get(
        `${getAPIBaseUrl()}api/fileUpload/download/${exModule}`,
        {
          responseType: "arraybuffer", // Important to specify the response type
        }
      );

      // Create a Blob from the response data
      const blob = new Blob([response.data], {
        type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
      });

      const timestamp = new Date()
        .toISOString()
        .replace(/[-:T]/g, "")
        .slice(0, 14); // e.g. 20250604
      const filename =
        exModule == "farmer"
          ? `_${exModule}_${timestamp}.xlsx`
          : `${loanBatchName}_${exModule}_${timestamp}.xlsx`;

      // Use FileSaver to save the file
      saveAs(blob, filename);
      setLoading(false);
    } catch (error) {
      console.error("Error downloading the file", error);
      setLoading(false);
    }
  };

  const officerSelected = (val: any) => {
    const selectedItem = loanOfficers.filter(
      (product: any) => product.id === val
    )[0];
    setOfficerId(selectedItem);
    console.log("Selected Officer:", selectedItem.id);
    // setValue(selectedItem);
  };

  const fetchOfficer = async () => {
    const userData = await userService.getOfficers();
    if (userData && userData.length > 0) {
      setLoanOfficers(userData);
    }
  };
  useEffect(() => {
    fetchOfficer();
  }, []);
  return (
    <>
      <div
        className="modal fade show d-block"
        id="kt_modal_confiem_box"
        role="dialog"
        tabIndex={-1}
        aria-modal="true"
      >
        {/* begin::Modal dialog */}
        <div className="modal-dialog modal-xl">
          {/* begin::Modal content */}
          <div className="modal-content">
            <div className="modal-header">
              <div className="w-100 d-flex flex-row align-items-center justify-content-between">
                {/* begin::Modal title */}
                <div>
                  <h2 className="fw-semibold">Import {title}</h2>
                </div>
                {/* end::Modal title */}

                {/* begin::Close */}
                <div
                  className="btn btn-icon btn-sm btn-active-icon-primary"
                  data-kt-users-modal-action="close"
                  onClick={() => onCancel()}
                  style={{ cursor: "pointer" }}
                >
                  <KTIcon
                    iconName="abstract-11"
                    iconType="outline"
                    className="fs-1"
                  />
                </div>
                {/* end::Close */}
              </div>
            </div>
            <div className="modal-body">
              <FormErrorAlert errors={errors} />

              <div className="row mb-5">
                <div className="col-md-8">
                  <div className="row">
                    <div className="col-md-12">
                      {/* errors */}
                      {errors && errors.length > 0 && (
                        <div className="mb-lg-15 alert alert-danger">
                          <div className="alert-text font-weight-bold">
                            {errors.map((err: any) => (
                              <ul>
                                <li>{err}</li>
                              </ul>
                            ))}
                          </div>
                        </div>
                      )}
                    </div>
                    <div className="col-md-12">
                      <div className="col-sm-12 fs-5 d-flex align-items-center py-3 mt-5">
                        {" "}
                        <i className="bi bi-1-circle-fill fs-1 mx-3 text-gray-400"></i>
                        Select an Excel template to download.{" "}
                      </div>

                      {batchId !== null && batchId !== undefined ? (<div className="col-md-5 mb-4">
                        <label
                          htmlFor="name"
                          className="form-label fw-bolder text-gray-800 fs-6 mt-5"
                        >
                          Business Development Officer
                        </label>
                        <select
                          name="values"
                          onChange={(e: any) => officerSelected(e.target.value)}
                          value={officerId?.id || ""}
                          className="form-control mb-3 mb-lg-0"
                        >
                          <option value="">Select Officer</option>
                          {loanOfficers &&
                            loanOfficers.map((opt: any) => (
                              <option key={opt.id} value={opt.id}>
                                {opt.username}
                              </option>
                            ))}
                        </select>
                      </div>) : ("")
                      }

                      <div className="d-flex gap-2 my-3">
                        <button
                          className="btn btn-secondary border"
                          onClick={() => downloadTemplate()}
                        >
                          {!loading && (
                            <span className="indicator-label">
                              <i className="bi bi-cloud-arrow-down-fill fs-2 mx-1 text-primary"></i>{" "}
                              Download Blank Template
                            </span>
                          )}

                          {loading && (
                            <span
                              className="indicator-progress"
                              style={{ display: "block" }}
                            >
                              Please wait...
                              <span className="spinner-border spinner-border-sm align-middle ms-2"></span>
                            </span>
                          )}
                        </button>
                        {(exModule === "PaymentDeductibles" ||
                          exModule === "loanApplication") && (
                            <button
                              className="btn btn-secondary border"
                              disabled={exModule === "loanApplication"}
                              onClick={() => downloadPreFilled(exModule)}
                            >
                              {!loading && (
                                <span className="indicator-label">
                                  <i className="bi bi-cloud-arrow-down-fill fs-2 mx-1 text-primary"></i>{" "}
                                  Download pre-filled Template
                                </span>
                              )}
                              {loading && (
                                <span
                                  className="indicator-progress"
                                  style={{ display: "block" }}
                                >
                                  Please wait...
                                  <span className="spinner-border spinner-border-sm align-middle ms-2"></span>
                                </span>
                              )}
                            </button>
                          )}



                      </div>
                    </div>
                  </div>
                  <div className="separator my-5"></div>

                  <div className="row mb-5">
                    <div className="col-md-6">
                      <div className="fs-5 d-flex align-items-center">
                        {" "}
                        <i className="bi bi-2-circle-fill fs-1 mx-3 text-gray-400"></i>
                        Upload the completed file{" "}
                      </div>
                      <UploadLayout
                        setFiles={setFiles}
                        setIsvalid={setIsvalid}
                        ext={"xlsx"}
                      />
                      <div className="py-3 fw-bold">
                        {files && files.length > 0 && files[0].name}
                      </div>

                      {!isvalid && files && files.length > 0 && (
                        <div className="alert-error p-0">
                          {"Select only .xlsx file with 5MB limit."}
                        </div>
                      )}

                      <div className="col-sm-12 fs-5 d-flex align-items-center py-5">
                        {" "}
                        <i className="bi bi-3-circle-fill fs-1 mx-3 text-gray-400"></i>
                        Click the Import button to start upload.{" "}
                      </div>
                    </div>
                  </div>
                  <div className="row">
                    <div className="col-md-12">
                      {showProcessing && (
                        <div className="alert bg-light-warning">
                          <div className="p-5">
                            The upload process is ongoing. You may close this
                            dialog and check the status of import process later
                            from{" "}
                            <Link
                              to={"/imports/history"}
                              className="link-primary"
                            >
                              Import History
                            </Link>{" "}
                            page.
                          </div>
                        </div>
                      )}
                    </div>
                  </div>

                  <div className="d-flex mb-3">
                    <button
                      className="btn btn-theme w-25"
                      disabled={!isvalid}
                      onClick={uploadFiles}
                    >
                      {!loading && (
                        <span className="indicator-label">
                          <i className="bi bi-cloud-arrow-up-fill fs-2 mx-1 text-primary"></i>{" "}
                          Import
                        </span>
                      )}
                      {loading && (
                        <span
                          className="indicator-progress"
                          style={{ display: "block" }}
                        >
                          Please wait...
                          <span className="spinner-border spinner-border-sm align-middle ms-2"></span>
                        </span>
                      )}
                    </button>
                  </div>
                </div>
                <div className="col-md-4">
                  <div className="excel-upload bg-light pt-6">
                    <h1 className="fs-4 fw-normal">
                      Please read the instructions carefully before uploading
                      excel.
                    </h1>

                    <ul className="p-4 lh-xl">
                      <li>
                        This excel is used to upload the{" "}
                        <strong>
                          {exModule === "loanApplication"
                            ? "loan application"
                            : exModule}
                        </strong>
                        .
                      </li>
                      <li>
                        Download the excel template and then update the required
                        data in the excel sheet(s).
                      </li>
                      <li>Upload the completed Excel file from your device.</li>
                    </ul>
                  </div>
                </div>
              </div>
            </div>
          </div>
          {/* end::Modal content */}
        </div>
        {/* end::Modal dialog */}
      </div>
      {/* begin::Modal Backdrop */}
      <div className="modal-backdrop fade show"></div>
      {/* end::Modal Backdrop */}
    </>
  );
}
