import { useState } from "react";
import { KTIcon } from "../../../../_metronic/helpers";
import { Content } from "../../../../_metronic/layout/components/content";
import { FilePond } from "react-filepond";
import 'filepond/dist/filepond.min.css';
import { useSelector } from "react-redux";

export default function LoanApplicationUploadModal(props: any) {
  const { isAdd, afterConfirm, setAttachmentIds } = props;
  const [attachIds, setAttachIds] = useState<any>([])
  const loanApplication: any = useSelector(
    (state: any) => state?.loanApplications
  );

  const handleFileUpload = async (res: any) => {
    
    try {
      const responseData = JSON.parse(res);
      const id = responseData.data[0];
      setAttachIds((prevIds: any) => [...prevIds, id]);
    } catch (error) {
      console.error('Error parsing JSON response:', error);

    }
  };

  const handleClose = async (res: any) => {
    
    afterConfirm(res)
    setAttachmentIds(attachIds);
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
                  <h2 className="fw-semibold">Upload application documents and files</h2>
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
              <ul>
                <li>Max size each file : 3MB</li>
                <li>Only upload file types JPG, PNG, and PDF.</li>
              </ul>
              
            </div>

            {/* end::Modal body */}
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
