import { useState } from "react";
import { isAllowed } from "../../../../_metronic/helpers/ApiUtil";
import PaymentBatchService from "../../../../services/PaymentBatchService";

const paymentBatchService = new PaymentBatchService();

const StartApprove = ({
  paymentBatchId,
  onApprove,
  loading,
  authorisedUser,
}: any) => {
  const [paymentMethod, setPaymentMethod] = useState<any>(null);
  const [isLoading, setIsLoading] = useState<boolean>(false);

  const handleSelectPaymentPartner = () => {
    setPaymentMethod("paymentPartner");
  };

  const handleSelectManualPayment = () => {
    setPaymentMethod("manualPayment");
  };

  const handleProcessManually = async () => {
    const confirm = window.confirm(
      "Are you sure you want to process this payment manually?"
    );
    if (!confirm) return;

    setIsLoading(true);
    const response = await paymentBatchService.updatePaymentManually(
      paymentBatchId
    );

    if (response.id) {
      setIsLoading(false);
      window.location.reload();
    }
  };

  const handleCancel = async () => {
    setPaymentMethod(null);
  };

  return (
    <div>
      {isAllowed("payments.batch.approve") ? (
        <div className="row">
          <div className="col-md-12">
            <div className="fs-5">
              <div>
                <h4 className="my-3 text-gray-700">Choose Payment Method</h4>
                <button
                  className="btn btn-primary  btn-sm"
                  onClick={handleSelectPaymentPartner}
                >
                  Pay using Payment Partner
                </button>
                <button
                  className="btn btn-secondary  btn-sm mx-3"
                  onClick={handleSelectManualPayment}
                >
                  Initiate Manual Payments
                </button>

                {/* Payment Partner */}
                {paymentMethod === "paymentPartner" && (
                  <div className="bg-light border rounded p-9 my-5 w-75">
                    <h4 className="mb-5 text-gray-700">
                      Payment Partner Details
                    </h4>
                    <p>
                      Please confirm the details to proceed with the payment.
                    </p>

                    <div className="mb-4">
                      <label className="fw-bold fs-6 mb-2">Partner Name:</label>
                      <select className="form-select">
                        <option value={"onafriq"}>Onafriq</option>
                      </select>
                    </div>

                    {isAllowed("payments.batch.approve") && (
                      <button
                        type="button"
                        className="btn btn-primary m-2"
                        onClick={onApprove}
                      >
                        {!loading && (
                          <span className="indicator-label">
                            {" "}
                            Process Payment
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
                )}

                {/* Manual Payment */}
                {paymentMethod === "manualPayment" && (
                  <div className="bg-light border rounded p-9 my-5 w-100">
                    <h4 className="mb-5 text-gray-700">
                      Manual Payment Processing
                    </h4>
                    <p className="text-muted">
                      Do you want to manually process this payment outside the
                      integrated payment system?
                    </p>

                    {isAllowed("payments.batch.approve") && (
                      <div className="mt-4 d-flex gap-3">
                        <button
                          type="button"
                          className="btn btn-primary"
                          onClick={handleProcessManually}
                          disabled={isLoading}
                        >
                          {!isLoading ? (
                            <span className="indicator-label">
                              Process Manually
                            </span>
                          ) : (
                            <span className="indicator-progress d-flex align-items-center">
                              Processing...
                              <span className="spinner-border spinner-border-sm ms-2"></span>
                            </span>
                          )}
                        </button>

                        <button
                          type="button"
                          className="btn btn-outline-secondary"
                          onClick={handleCancel}
                        >
                          Cancel
                        </button>
                      </div>
                    )}
                  </div>
                )}
              </div>
            </div>
          </div>
        </div>
      ) : (
        <div className="alert alert-warning fw-bold text-center">
          You do not have permission to approve payments.
          {onApprove && authorisedUser?.length > 0 && (
            <>
              <div >
                Only people listed below can review or approve the payment
                batch.
              </div>
              <div className="text-center mt-2">
                {authorisedUser?.map((user: any, index: any) => (
                  <span key={user.id}>
                    {user.username}
                    {index < authorisedUser?.length - 1 && ", "}
                  </span>
                ))}
              </div>
            </>
          )}
        </div>
      )}
    </div>
  );
};

export default StartApprove;
