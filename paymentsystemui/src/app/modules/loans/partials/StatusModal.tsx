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

const applicationStatusService = new ApplicationStatusService();
const loanApplicationService = new LoanApplicationService();

export default function StatusModal(props: any) {
  const { isAdd, afterConfirm, setAttachmentIds, applicationId } = props;

  const [attachIds] = useState<any>([]);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [loading, setLoading] = useState(false);

  const loanApplication: any = useSelector(
    (state: any) => state?.loanApplications
  );

  interface StatusModel {
    id: string;
    status: string;
    comments: string;
  }

  const statusInitValues: StatusModel = {
    id: '',
    status: "",
    comments: "",
  };

  const formik = useFormik<StatusModel>({
    enableReinitialize: true,
    initialValues: statusInitValues,
    // validationSchema: loanApplicationSchema,
    onSubmit: (values) => {
      const toastId = toast.loading("Please wait...");
      setIsSubmitting(true);
      setLoading(true);
      setTimeout(async () => {
        const updatedValues = [{ ...values, id: loanApplication.id }]
        const result = await loanApplicationService.putLoanApplicationStatusData(values.status, updatedValues);

        if (result && result.id) {
          toastNotify(toastId, "Success");
          setLoading(false);

          afterConfirm(false);

        } else {
          toast.error("Something went wrong");
        }
        toast.dismiss(toastId);
        setIsSubmitting(false);
      }, 1000);
    },
  });

  const handleClose = async (res: any) => {
    afterConfirm(res);
    setAttachmentIds(attachIds);
  };

  const [actions, setActions] = useState<any>();
  const bindActions = async () => {
    const data = {
      pageNumber: 1,
      pageSize: 1000,
    };
    const response = await applicationStatusService.getStatusData(data);
    response.unshift({ id: null, name: "Select " });
    setActions(response);
  };

  useEffect(() => {
    bindActions();
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
        <div className="modal-dialog modal-lg">
          {/* begin::Modal content */}
          <div className="modal-content">
            <div className="modal-header">
              <div className="w-100 h-300 d-flex flex-row align-items-center justify-content-between">
                {/* begin::Modal title */}
                <div>
                  <h2 className="fw-semibold">Status</h2>
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
              <RequiredMarker />
              <form onSubmit={formik.handleSubmit} noValidate>
                <div className="d-flex flex-column justify-content-start">
                  <ValidationSelect
                    className="col-md-4 my-3"
                    label="Status"
                    name="status"
                    options={actions !== undefined ? actions : ""}
                    formik={formik}
                    isRequired
                  />
                  <ValidationTextArea
                    className="col-md-8 my-3"
                    label="Comment"
                    name="comments"
                    placeholder="Enter description"
                    rows={6}
                    formik={formik}
                  />
                </div>

                <div className="text-center pt-15">
                  <button
                    type="submit"
                    className="btn btn-custom btn-theme"
                    disabled={isSubmitting}
                  >
                    <span className="indicator-label">Submit</span>
                    {isSubmitting && (
                      <span className="indicator-progress">
                        Please wait...{" "}
                        <span className="spinner-border spinner-border-sm align-middle ms-2"></span>
                      </span>
                    )}
                  </button>
                  <button
                    type='button'
                    onClick={() => afterConfirm(false)}
                    className=" btn btn-secondary mx-3"
                  >
                    Cancel
                  </button>
                </div>
              </form>
            </div>
          </div>
        </div>
      </div>

      <div className="modal-backdrop fade show"></div>
    </>
  );
}
