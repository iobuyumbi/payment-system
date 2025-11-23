
import ForceChangePassword from './ForceChangePassword';


interface IModalChangePasswordProps {
    open: boolean;
    onClose: () => void;
}

const ModalChangePassword = ({ open, onClose }: IModalChangePasswordProps) => {

 const handleClose = (event: {}, reason: "backdropClick" | "escapeKeyDown") => {
    if (reason === "backdropClick") return; // Prevent closing on backdrop click
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
        <div className="modal-dialog modal-md">
          {/* begin::Modal content */}
          <div className="modal-content">
            <div className="modal-header">
              <div className="w-100 d-flex flex-row align-items-center justify-content-between">
                {/* begin::Modal title */}
                <div>
                  <h2 className="fw-semibold">Password Reset</h2>
                </div>
                {/* end::Modal title */}
              </div>
            </div>
            <div className="modal-body">
               <ForceChangePassword />
            </div>

            {/* end::Modal body */}
            <div className="card-footer m-5">
             
            </div>
          </div>
        
        </div>
      
      </div>
    
      <div className="modal-backdrop fade show"></div>
      {/* end::Modal Backdrop */}
      </>
    )
}

export default ModalChangePassword;
