import { useEffect, useState } from "react";
import { KTIcon } from "../../../../_metronic/helpers";
import { useSelector } from "react-redux";
import { useFormik } from "formik";
import { toast } from "react-toastify";
import { toastNotify } from "../../../../services/NotifyService";
import {
  RequiredMarker,
  ValidationSelect,
  ValidationTextArea,
} from "../../../../_shared/components";
import ApplicationStatusService from "../../../../services/ApplicationStatusService";
import LoanApplicationService from "../../../../services/LoanApplicationService";
import axios from "axios";
import { getAPIBaseUrl } from "../../../../_metronic/helpers/ApiUtil";
import moment from "moment";
import { Worker, Viewer } from "@react-pdf-viewer/core";
import "@react-pdf-viewer/core/lib/styles/index.css";
import { pdfjs } from "react-pdf";
pdfjs.GlobalWorkerOptions.workerSrc = `https://unpkg.com/pdfjs-dist@3.11.174/build/pdf.worker.min.js`;
const applicationStatusService = new ApplicationStatusService();
const loanApplicationService = new LoanApplicationService();

export default function StatementPdfModal(props: any) {
  const { afterConfirm, applicationId, loanNumber } = props;

  const [attachIds] = useState<any>([]);
  const [pdfUrl, setPdfUrl] = useState<string | null>(null);

  const handleClose = async (res: any) => {
    afterConfirm(res);
  };

  const [loading, setLoading] = useState(false);

  useEffect(() => {
    if (applicationId) {
      generatePdfPreview();
    }
  }, [applicationId]);

  const generatePdfPreview = async () => {
    setLoading(true);
    try {
      const response = await axios.get(
        `${getAPIBaseUrl()}api/LoanRepayments/GeneratePdf/${applicationId}`,
        {
          headers: {
            Accept: "application/pdf",
          },
          responseType: "blob",
        }
      );

      const blob = new Blob([response.data], { type: "application/pdf" });
      const fileUrl = URL.createObjectURL(blob);
      setPdfUrl(fileUrl);
    } catch (error) {
      console.error("Error loading PDF preview:", error);
    } finally {
      setLoading(false);
    }
  };

  const downloadPdf = async () => {
    try {
      const response = await axios.get(
        `${getAPIBaseUrl()}api/LoanRepayments/GeneratePdf/${applicationId}`,
        {
          headers: {
            Accept: "application/pdf",
          },
          responseType: "blob",
        }
      );

      const blob = new Blob([response.data], { type: "application/pdf" });
      const fileUrl = URL.createObjectURL(blob);

      // Preview in iframe or embed
      setPdfUrl(fileUrl);
      const timestamp = moment().format("DD-MM-YYYY HH-mm"); // Format: DD-MM-YYYY HH-mm
      // Optional: trigger download
      const a = document.createElement("a");
      a.href = fileUrl;
      a.download = `LoanStatement:${loanNumber}_${timestamp}.pdf`;
      document.body.appendChild(a);
      a.click();
      a.remove();
    } catch (error) {
      console.error("Error downloading PDF:", error);
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
        <div className="modal-dialog modal-lg">
          {/* begin::Modal content */}
          <div className="modal-content">
            <div className="modal-header">
              <div className="w-100 h-300 d-flex flex-row align-items-center justify-content-between">
                {/* begin::Modal title */}
                <div>
                  <h2 className="fw-semibold">Statement PDF Preview</h2>
                </div>
                {/* end::Modal title */}

                {/* begin::Close */}
                <div
                  className="btn btn-icon btn-sm btn-active-icon-primary"
                  data-kt-users-modal-action="close"
                  onClick={() => handleClose(false)}
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
              <button className="btn btn-secondary m-2" onClick={downloadPdf}>
                Download PDF{" "}
              </button>
              <label>
                {" "}
                <b>Note</b> : The pdf will constitute the latest state of the
                loan application.
              </label>
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

      <div className="modal-backdrop fade show"></div>
    </>
  );
}
