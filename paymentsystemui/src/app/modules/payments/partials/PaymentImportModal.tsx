import { useEffect, useState } from "react";

import { uploadExcel } from "../../../../services/ExcelService";
import { KTIcon } from "../../../../_metronic/helpers";
import UploadLayout from "../../../pages/UploadLayout";
import { Link, useNavigate } from "react-router-dom";
import AssociateService from "../../../../services/AssociateService";
import * as XLSX from 'xlsx';
import saveAs from "file-saver";


const associateService = new AssociateService();
export function PaymentImportModal_not_in_use(props: any) {
  const { title, exModule, afterConfirm, batchId, moduleId, loanBatchIds } = props;

  const navigate = useNavigate();
  const [rowData, setRowData] = useState<any>();
  const [loading, setLoading] = useState(false);
  const [errors, setErrors] = useState<any>({});
  const [isvalid, setIsvalid] = useState<any>(false);
  const [files, setFiles] = useState<any>();
  const [module, setModule] = useState<any>();
  const [showProcessing, setShowProcessing] = useState(false);
  const [searchTerm, setSearchTerm] = useState<string>("");
  const onCancel = () => {
    afterConfirm(false);
  };

  const uploadFiles = (args: React.MouseEvent) => {
    args.preventDefault();
    // Clear error
    setErrors({});

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

      const response = uploadExcel(formData, exModule, batchId, 'KE');

      setShowProcessing(true);
      setTimeout(() => {
        setLoading(false);
        //navigate("/imports/history");
      }, 10000);
    }
  };

  const bindAssociateBatch = async () => {
    
    if (searchTerm.length === 0 || searchTerm.length > 2) {
      const data: any = {
        pageNumber: 1,
        pageSize: 10000,
        batchId: loanBatchIds[0].value,
      };

      const response = await associateService.getAssociateData(data);
    
      setRowData(response);
    }
  }
  const handleExport = () => {
    const workbook = XLSX.utils.book_new();


    let mappedData = [{
      'System Id': '',
      'CountCRU': 0,
      'SumPurchasePrice (USD)': 0,
      'FarmerShare(USD)': 0,
      'SumPurchasePrice ': 0,
      'AdminstrativeTotalDeduction': 0,
      'FarmerShare': 0,
    }];


    if (rowData.length > 0) {
      mappedData = rowData.map((row: any) => ({
        'System Id': row.systemId,
        'CountCRU': 0,
        'SumPurchasePrice (USD)': 0,
        'FarmerShare(USD)': 0,
        'SumPurchasePrice ': 0,
        'AdminstrativeTotalDeduction': 0,
        'FarmerShare': 0,
      }));
    }

    const worksheet = XLSX.utils.json_to_sheet(mappedData);
    XLSX.utils.book_append_sheet(workbook, worksheet, 'PaymentList');
    const blob = new Blob([XLSX.write(workbook, { bookType: 'xlsx', type: 'array' })], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
    saveAs(blob, 'DeductibleOutline.xlsx');
  };

  useEffect(() => {
    bindAssociateBatch()
  }, []);


  const downloadTemplate = () => {
    //  alert(exModule)
    if (exModule === "farmer" || exModule === "loanApplication") {
      alert("There is no prebuilt template for this module");
    }
  };

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
              <div className=" row mb-5">
                <div className="col-md-12">
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

                  {moduleId === 3 &&

                    <div className="col-sm-12 fs-5  py-3 mt-5">
                      <div className="d-flex align-items-center">
                        <i className="bi bi-1-circle-fill fs-1 mx-3 text-gray-400"></i>
                        <button className="btn btm-sm btn-theme " onClick={handleExport}>Download Excel</button>
                      </div>
                      <div className="col-md-4">

                      </div>
                    </div>}
                </div>
              </div>

              <div className="separator my-5"></div>

              <div className="row mb-5">
                <div className="col-md-12">
                  <div className="fs-5 d-flex align-items-center py-3">
                    {" "}
                    <i className="bi bi-2-circle-fill fs-1 mx-3 text-gray-400"></i>
                    Upload the completed file{" "}
                  </div>
                  <div className="fs-5 d-flex align-items-center py-3">
                    <div className="mx-1">
                      <UploadLayout
                        setFiles={setFiles}
                        setIsvalid={setIsvalid}
                        ext={"xlsx"}
                      />
                      {files && files.length > 0 && files[0].name}
                      {!isvalid && files && files.length > 0 && (
                        <div className="alert-error p-0">
                          {"Select only .xlsx file with 5MB limit."}
                        </div>
                      )}
                    </div>
                    <div className="bg-light-primary p-3 mx-2 border rounded">
                      <ul >
                        <li>Ensure that the file size does not exceed 2 MB (per file).</li>
                        <li>Only these file types are allowed - JPG/JPEG, PNG, PDF.</li>
                        <li>Maximum 5 files can be uploaded at once.</li>
                      </ul>
                    </div>
                  </div>
                </div>
              </div>
              <div className="row">
                <div className="col-md-12">
                  {showProcessing && (
                    <div className="alert bg-light-warning">
                      <div className="p-5">
                        The upload process is ongoing. You may close this dialog
                        and check the status of import process later from{" "}
                        <Link to={"/imports/history"} className="link-primary">
                          Import History
                        </Link>{" "}
                        page.
                      </div>
                    </div>
                  )}
                </div>
              </div>
            </div>

            {/* end::Modal body */}
            <div className="card-footer m-5">
              <div className="d-flex">
                <button
                  className="btn btn-theme w-25"
                  disabled={!isvalid}
                  onClick={uploadFiles}
                >
                  {!loading && (
                    <span className="indicator-label">
                      <i className="bi bi-cloud-arrow-up-fill fs-3 py-2 mx-1"></i>{" "}
                      Import{" "}
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
