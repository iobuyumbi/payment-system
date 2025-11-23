import { useState } from "react";
import PaymentInfoCard from "../cards/paymentInfoCard";
import { useNavigate, useParams } from "react-router-dom";
import PaymentBatchService from "../../../../services/PaymentBatchService";
import { toast } from "react-toastify";

const paymentBatchService = new PaymentBatchService();

const PaymentBatchApproval = ({ batch }: any) => {
  const navigate = useNavigate();

  const { paymentBatchId } = useParams();
  const [isApproved, setIsApproved] = useState(false);
  const [isVerified, setIsVerified] = useState(false);
  const [paymentMethod, setPaymentMethod] = useState<any>(null);
  const [loading, setLoading] = useState(false)

  const handleApprove = () => {
    setIsApproved(true);
  };

  const handleSelectPaymentPartner = () => {
    setPaymentMethod("paymentPartner");
  };

  const handleSelectManualPayment = () => {
    setPaymentMethod("manualPayment");
  };


  // const handleConfirmPaymentPartner = async () => {
  //   // Logic to confirm payment with Payment Partner
  //   const searchParams = {
  //     batchId: paymentBatchId,
  //     statusId: 1,
  //   };

  //   const response = await paymentBatchService.postPaymentProcessing(
  //     searchParams
  //   );
 
  //   if (response.id) {
  //     toast.info("Payment processing is in progress");
  //   }
  
  //   // navigate(`/payment-batch/kycresult/7c0fd39e-4f7b-4ddc-bb03-7d158e2c85df`);
  // };

  const handleConfirmPaymentPartner = async () => {
    setLoading(true);
    // Logic to confirm payment with Payment Partner
    const searchParams = {
      batchId: paymentBatchId,
      statusId: 1,
      isFarmerValid: true
    };

    const response = await paymentBatchService.payAllBatchAsync(searchParams);
    setLoading(false);
    toast.info("Payment processing is in progress");
    navigate(`/payment-processing/transactions`);
  };

  const handleVerifyMobile = async () => {
    setLoading(true);
    // Logic to confirm payment with Payment Partner
    const searchParams = {
      batchId: paymentBatchId,
      statusId: 1,
    };
    setIsVerified(true);
    const response = await paymentBatchService.postVerifyMobile(searchParams);
   
    //if (response.id) {
    toast.info('Verifying mobile numbers is in progress');
    //}
    setTimeout(() => {
      navigate(`/payment-batch/kycresult/${paymentBatchId}`);
      setLoading(false);
    }, 5000)
  };

  const handleContinue = (props: any) => {
    // Navigate to the desired URL
    navigate(`/payment-batch/kycresult/${paymentBatchId}`);
  };

  return (
    <div className="row">
      <div className="col-md-5">
        <PaymentInfoCard batch={batch} />
      </div>
      <div className="col-md-7">
        <div className="fs-5">
          {!isApproved ? (
            <button className="btn btn-theme" onClick={handleApprove}>
              Approve Batch
            </button>
          ) : (
            <div>
              <h4 className="my-3 text-gray-700">Choose Payment Method</h4>
              <button
                className="btn btn-primary"
                onClick={handleSelectPaymentPartner}
              >
                Pay using Payment Partner
              </button>
              <button
                className="btn btn-secondary mx-3"
                onClick={handleSelectManualPayment}
              >
                Initiate Manual Payments
              </button>

              {paymentMethod === "paymentPartner" && (
                <div className="bg-light border rounded p-9 my-5 w-75">
                  <h4 className="mb-5 text-gray-700">
                    Payment Partner Details
                  </h4>
                  <p>Please confirm the details to proceed with the payment.</p>

                  <div className="mb-4">
                    <label className="fw-bold fs-6 mb-2">Partner Name:</label>
                    <select className="form-select">
                      <option value={"onafriq"}>Onafriq</option>
                    </select>
                  </div>

                  {/* <div className='mb-4 w-50'>
                                        <label className='fw-bold fs-6 mb-2'>Transaction Fee:</label>
                                        <input type="text" value="2.5%" disabled className='form-control' />
                                    </div> */}

                  {/* <div className="mb-4  w-50">
                    <label className="fw-bold fs-6 mb-2">
                      Amount to Transfer:
                    </label>
                    <input
                      type="text"
                      value={batch.amount ?? "25100.00"}
                      disabled
                      className="form-control"
                    />
                  </div> */}

                  {/* <button className='btn btn-theme btn-sm m-2' onClick={handleConfirmPaymentPartner}>Confirm and Pay</button> */}
                  {!isVerified && (
                    <div>
                      <button
                        type='button'
                        className="btn btn-theme  my-2"
                        onClick={handleConfirmPaymentPartner}
                      >
                        {!loading && <span className='indicator-label'>Continue</span>}
                        {loading && (
                          <span className='indicator-progress' style={{ display: 'block' }}>
                            Please wait...
                            <span className='spinner-border spinner-border-sm align-middle ms-2'></span>
                          </span>
                        )}
                      </button>
                      <div className="my-3">
                        <div className="d-flex align-items-top my-3 text-gray-700">
                          <i className="fas fa-info-circle m-2"></i>
                          Please note that only the valid numbers will be sent for payment processing.
                        </div>
                      </div>
                    </div>
                  )}

                  {isVerified && (
                    <button
                      type='button'
                      className="btn btn-primary btn-sm m-2"
                      onClick={handleContinue}
                    >
                      Continue
                    </button>
                  )}
                </div>
              )}
            </div>
          )}
        </div>
      </div>

    </div>
  );
};

export default PaymentBatchApproval;
