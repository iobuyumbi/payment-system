import { Fragment, useEffect, useState } from "react";
import { KTIcon } from "../../../../_metronic/helpers";
import { IConfirmModel } from "../../../../_models/confirm-model";
import PaymentBatchService from "../../../../services/PaymentBatchService";
import { ConfirmBox } from "../../../../_shared/Modals/ConfirmBox";
import StartReviewCard from "./StartReviewCard";
import StartApprove from "./StartApprove";
import RejectBatch from "./RejectBatch";
import { toast } from "react-toastify";
import { isAllowed } from "../../../../_metronic/helpers/ApiUtil";
import UserService from "../../../../services/UserService";
import { AUTH_LOCAL_STORAGE_KEY } from "../../auth";

const paymentBatchService = new PaymentBatchService();
const userService = new UserService();
interface WorkflowStageCardProps {
  batch: any;
}

interface ActionOption {
  action: string;
  label: string;
  restrictedForMaker: boolean;
}

interface TransitionResponse {
  effectiveRole: string;
  message: string;
  nextActions: ActionOption[];
  isSelfAction: boolean;
}

const WorkflowStageCard = ({ batch }: WorkflowStageCardProps) => {
  const [loading, setLoading] = useState(false);
  const [btnAction, setBtnAction] = useState("");
  const [context, setContext] = useState<TransitionResponse | null>(null);
  const [filteredActions, setFilteredActions] = useState<any[]>([]);

  const [confirmModel, setConfirmModel] = useState<IConfirmModel>();
  const [showConfirmBox, setShowConfirmBox] = useState<boolean>(false);
  const [showStartReview, setShowStartReview] = useState<boolean>(false);
  const [showApprove, setShowApprove] = useState<boolean>(false);
  const [showRejectBatch, setShowRejectReview] = useState<boolean>(false);
  const [permission, setPermission] = useState<string>("");
  const [authorisedUser, setAuthorisedUser] = useState<any>(null);


  const current_user = JSON.parse(localStorage.getItem(AUTH_LOCAL_STORAGE_KEY) || "");


  useEffect(() => {
    const fetchContext = async () => {
      setLoading(true);
      const res = await paymentBatchService.getActionContext(batch.id);

      const actions = (res?.nextActions || []).filter(
        (action: any) => !res?.isSelfAction || !action.restrictedForMaker
      );

      setContext(res);
      setFilteredActions(actions);

      setLoading(false);
    };

    fetchContext();
  }, [batch.id]);

  const handleAction = (actionKey: string) => {
    setBtnAction(actionKey);

    if (["start-review", "approve", "reject"].includes(actionKey)) {
      setShowStartReview(actionKey === "start-review");

      setShowApprove(actionKey === "approve");

      setShowRejectReview(actionKey === "reject");
      if (actionKey === "start-review" && !isAllowed("payments.batch.review")) {
        setPermission("Review");
      } else if (
        actionKey === "approve" &&
        !isAllowed("payments.batch.approve")
      ) {
        setPermission("Approve");
      }
    } else {
      const confirmModel: IConfirmModel = {
        title: "Confirm action",
        btnText: "Send",
        message: "confirm-stage-review",
        deleteUrl: "",
      };
      setConfirmModel(confirmModel);
      setShowConfirmBox(true);
    }
  };

const afterConfirm = async (res: any) => {
  if (res == false) {
    setShowConfirmBox(false);
    return;
  }

  setLoading(true);

  const updateResponse = await toast.promise(
    paymentBatchService.updateStage(
      { action: btnAction, remarks: "" },
      batch.id
    ),
    {
      pending: "Updating batch...",
      success: "Batch updated successfully!",
      error: "Failed to update batch.",
    }
  );

  if (updateResponse.id) {
    setLoading(false);
    setShowConfirmBox(false);
    window.location.reload();
  } else {
    setLoading(false);
    toast.error(
      "The batch could not be updated. Please try again or contact admin"
    );
  }
};


 const handleStartReview = async (values: any) => {
  await toast.promise(
    (async () => {
      setLoading(true);
      await paymentBatchService.updateStage(values, batch.id);
      setLoading(false);
      setShowStartReview(false);
      window.location.reload();
    })(),
    {
      pending: "Updating batch...",
      success: "Batch updated successfully!",
      error: "Failed to update batch.",
    }
  );
};

  const handleConfirmPayment = async () => {
    setLoading(true);
    await paymentBatchService.payAllBatchAsync({
      batchId: batch.id,
      statusId: 1,
      isFarmerValid: true,
    });
    setLoading(false);
    toast.info("Payment processing is in progress");

    window.location.reload();
    //navigate(`/payment-batch/history/${batch.id}`);
  };

  useEffect(() => {
     setLoading(true);
    const fetchUsersWithPermission = async () => {
     
      const userData = await userService.getUsersByPermissions(permission);
      if (userData && userData.length > 0) {
  const filteredUsers = userData.filter(
        (user: any) => user.username !== current_user.username
      );

      if (filteredUsers && filteredUsers.length > 0) {
        setAuthorisedUser(filteredUsers);
      }
    };
  }
    fetchUsersWithPermission();
    setLoading(false);
  }, [permission]);

  return (<>
    {loading === false && <Fragment>
      <div className="notice d-flex bg-light-gray rounded border-gray border border-dashed mb-9 p-6">
        <KTIcon iconName="information-5" className="fs-2tx me-4" />
        <div className="d-flex flex-stack flex-grow-1">
          <div className="fw-bold">
            <h4 className="text-gray-800 fw-bolder">{context?.message}</h4>

            {/* Filter actions based on maker-checker rule */}
            {filteredActions.length > 0 ? (
              <div className="my-4">
                {filteredActions
                  .filter((action) => action.label?.trim())
                  .map((actionObj) => (
                    <button
                      key={actionObj.action}
                      className={`fw-bolder btn me-2 ${
                        actionObj.label?.toLowerCase().includes("reject")
                          ? "btn-secondary"
                          : "btn-theme"
                      }`}
                      disabled={loading}
                      onClick={() => handleAction(actionObj.action)}
                    >
                      {loading && btnAction === actionObj.action
                        ? "Please wait..."
                        : actionObj.label}
                    </button>
                  ))}
              </div>
            ) : (
              <div className="text-danger mt-3">
                {loading === false && (
                  <span>You cannot review or approve your own batch.</span>
                )}
                <>
              
                  <div className="bg-theme fw-bold text-center">
                    Only people listed below can modify the payment status.
                 
                  <div className="text-center mt-2">
                    {authorisedUser?.map((user: any, index: any) => (
                      <span key={user?.id}>
                        {user?.username}
                        {index < authorisedUser.length - 1 && ", "}
                      </span>
                    ))}
                  </div>
                   </div>
                 
                </>
              </div>
            )}
          </div>
        </div>
      </div>
      {showConfirmBox && (
        <ConfirmBox
          confirmModel={confirmModel}
          afterConfirm={afterConfirm}
          loading={loading}
          btnType="theme"
        />
      )}

      {showStartReview && (
        <StartReviewCard onStartReview={handleStartReview} loading={loading} authorisedUser={authorisedUser} />
      )}
      {showRejectBatch && (
        <RejectBatch onStartReview={handleStartReview} loading={loading}  />
      )}
      {showApprove && (
        <StartApprove
          paymentBatchId={batch.id}
          onApprove={handleConfirmPayment}
          loading={loading}
          authorisedUser={authorisedUser}
        />
      )}
      {/* {permission &&
        authorisedUser?.length > 0 &&
        filteredActions?.length > 0 && !loading && (
          <>
            <div className="alert alert-warning fw-bold text-center">
             Only people listed below can review or approve the payment batch.
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
        )} */}
    </Fragment>}</>
  );
};

export default WorkflowStageCard;
