import { useState } from "react";
import { uploadExcel } from "../../services/ExcelService";
import { KTIcon } from "../../_metronic/helpers";
import UploadLayout from "../../app/pages/UploadLayout";
import { Link, useNavigate } from "react-router-dom";
import { toast } from "react-toastify";
import { toastNotify } from "../../services/NotifyService";
import axios from "axios";
import saveAs from "file-saver";
import { getAPIBaseUrl } from "../../_metronic/helpers/ApiUtil";


export function UpdateImportModal(props: any) {
    const { title, exModule, batchId, afterConfirm } = props;
    const navigate = useNavigate();

    const [loading, setLoading] = useState(false);
    const [errors, setErrors] = useState<any>({});
    const [isvalid, setIsvalid] = useState<any>(false);
    const [files, setFiles] = useState<any>();
    const [showProcessing, setShowProcessing] = useState(false);

    const onCancel = () => {
        afterConfirm(false);
    }

    const uploadFiles = async (args: React.MouseEvent) => {
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

            await uploadExcel(formData, exModule, batchId)
                .then((response: any) => {

                    const toastId = toast.loading("Please wait...");
                    if (response) {
                        if (response.data && response.data.succeeded) {
                            setShowProcessing(true);
                            setTimeout(() => {
                                
                                if (exModule === "PaymentDeductibles") {
                                    navigate(`/kyc/pre-payment-kyc/${batchId}`);
                                    //navigate(`/payment-batch/details/${batchId}`);
                                } else {
                                    navigate("/imports/history");
                                }
                            }, 10000)
                            toastNotify(toastId, response.data.succeeded == true ? "Success" : ""
                            );

                        } else {
                            errors.errorText = response.message;
                            toast.error(response.data.succeeded == false ? "Error" : "");
                            setErrors(errors);
                        }
                    } else {
                        errors.errorText =
                            "There was an error. File could not be uploaded.";
                        toast.error(response.data.succeeded == false ? "Error" : "");
                        setErrors(errors);
                    }
                    // after upload
                })
                .catch((e: any) => {
                    console.log(e);
                })
                .finally(() => {
                    setLoading(false);
                });

            setShowProcessing(true);
            setTimeout(() => {
                setLoading(false);
                if (exModule === "PaymentDeductibles") {
                    navigate(`/kyc/pre-payment-kyc/${batchId}`);
                } else {
                    navigate("/imports/history");
                }
            }, 10000)


        }
    };

    const downloadTemplate = async () => {
        //  alert(exModule)
        if (exModule === 'farmer' || exModule === 'loanApplication') {
            alert('There is no prebuilt template for this module');
        }
        try {
            const response = await axios.get(`${getAPIBaseUrl()}api/fileUpload/download/${exModule}`, {
                responseType: 'arraybuffer', // Important to specify the response type
            });

            // Create a Blob from the response data
            const blob = new Blob([response.data], {
                type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
            });

            // Use FileSaver to save the file
            saveAs(blob, `${exModule}_template.xlsx`);
        } catch (error) {
            console.error('Error downloading the file', error);
        }
    }
    return (
        <>
            <div
                className='modal fade show d-block'
                id='kt_modal_confiem_box'
                role='dialog'
                tabIndex={-1}
                aria-modal='true'
            >
                {/* begin::Modal dialog */}
                <div className='modal-dialog modal-xl'>
                    {/* begin::Modal content */}
                    <div className='modal-content'>
                        <div className='modal-header'>
                            <div className="w-100 d-flex flex-row align-items-center justify-content-between">
                                {/* begin::Modal title */}
                                <div>
                                    <h2 className='fw-semibold'>Import {title}</h2>
                                </div>
                                {/* end::Modal title */}

                                {/* begin::Close */}
                                <div
                                    className='btn btn-icon btn-sm btn-active-icon-primary'
                                    data-kt-users-modal-action='close'
                                    onClick={() => onCancel()}
                                    style={{ cursor: 'pointer' }}
                                >
                                    <KTIcon iconName="abstract-11" iconType="outline" className='fs-1' />
                                </div>
                                {/* end::Close */}
                            </div>

                        </div>
                        <div className='modal-body'>
                            <div className=" row mb-5">
                                <div className="col-md-12">
                                    {/* errors */}
                                    {errors && errors.length > 0 && (
                                        <div className='mb-lg-15 alert alert-danger'>
                                            <div className='alert-text font-weight-bold'>
                                                {errors.map((err: any) =>
                                                (<ul>
                                                    <li>{err}</li>
                                                </ul>)
                                                )}
                                            </div>
                                        </div>
                                    )}
                                </div>
                                <div className="col-md-12">
                                    <div className="excel-upload bg-light pt-6">
                                        <h1 className="fs-4 fw-normal">
                                            Please read the instructions carefully before uploading excel.
                                        </h1>

                                        <ul className="p-4 lh-xl">
                                            <li>
                                                This excel is used to upload the <strong>{exModule === "loanApplication" ? "loan application" : exModule}</strong>.
                                            </li>
                                            <li>
                                                Download the excel template and then update the required
                                                data in the excel sheet(s).
                                            </li>
                                            <li>Upload the completed Excel file from your device.</li>
                                        </ul>

                                    </div>
                                    {/* 
                                    <div className="col-sm-12 fs-5 d-flex align-items-center py-3 mt-5">
                                        {" "}
                                        <i className="bi bi-1-circle-fill fs-1 mx-3 text-gray-400"></i>
                                        Select an Excel template to download.{" "}
                                    </div>

                                    <div className="d-grid gap-2 my-3 w-25">
                                        <button className="btn btn-secondary border" onClick={() => downloadTemplate()}>
                                            <i className="bi bi-cloud-arrow-down-fill fs-2 mx-1 text-primary"></i>{" "}Download</button>
                                    </div> */}


                                </div>
                            </div>

                            <div className="separator my-5"></div>

                            <div className="row mb-5">
                                <div className="col-md-6">
                                    <div className="fs-5 d-flex align-items-center py-3">
                                        {" "}
                                        <i className="bi bi-1-circle-fill fs-1 mx-3 text-gray-400"></i>
                                        Upload the completed file{" "}
                                    </div>
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
                            </div>
                            <div className="row">
                                <div className="col-md-12">
                                    {showProcessing && <div className="alert bg-light-warning">
                                        <div className="p-5">
                                            The upload process is ongoing. You may close this dialog and check the status
                                            of import process later from <Link to={'/imports/history'} className="link-primary"
                                            >Import History</Link> page.
                                        </div>
                                    </div>}
                                </div>
                            </div>
                        </div>

                        {/* end::Modal body */}
                        <div className="card-footer m-5">
                            <div className="d-flex mx-2">
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
                                        <span className="indicator-progress" style={{ display: "block" }}>
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
            <div className='modal-backdrop fade show'></div>
            {/* end::Modal Backdrop */}
        </>

    )
}