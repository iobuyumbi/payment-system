import { useEffect, useState } from 'react';
import { KTIcon } from '../../../../_metronic/helpers';
import { IConfirmModel } from '../../../../_models/confirm-model';
import PaymentBatchService from '../../../../services/PaymentBatchService';
import { ConfirmBox } from "../../../../_shared/Modals/ConfirmBox";
import StartReviewCard from './StartReviewCard';
import StartApprove from './StartApprove';
import { useNavigate } from 'react-router-dom';
import { toast } from 'react-toastify';
import { getAuth } from '../../auth/core/AuthHelpers';
import RejectBatch from './RejectBatch';

const paymentBatchService = new PaymentBatchService();

interface SuccessCardProps {
  batch: any;
  status: 'Initiated' | 'Under Review' | 'Pending Approval' | 'Approved' | 'Rejected' | 'Review Rejected';
  isAllowed: (permission: string) => boolean;
}

const auth = getAuth();
const currentUserId = auth?.userId;

const statusContent = {
  Initiated: {
    initiator: {
      title: 'Batch Initiated',
      description: 'Payment batch initiated. You can now send it for review.',
      buttonText: 'Send for Review',
      buttonVisible: true,
      permission: 'payments.batch.add',
      popupMessage: 'confirm-stage-review',
      action: '',
    },
    reviewer: {
      title: 'Batch Initiated (Read-Only)',
      description: 'You can only view this batch. No changes are allowed at this stage.',
      buttonText: '',
      buttonVisible: false,
      permission: 'payments.batch.review',
      popupMessage: 'confirm-stage-approval',
      action: '',
    },
    approver: {
      title: 'Batch Initiated (Read-Only)',
      description: 'You can only view this batch. No changes are allowed at this stage.',
      buttonText: '',
      buttonVisible: false,
      permission: 'payments.batch.approve',
      popupMessage: 'confirm-stage-review',
      action: '',
    },
  },
  'Under Review': {
    initiator: {
      title: 'Under Review',
      description: 'Your batch is under review. No edits are allowed at this stage.',
      buttonText: '',
      buttonVisible: false,
      permission: 'payments.batch.add',
      popupMessage: 'confirm-stage-review',
      action: '',
    },
    reviewer: {
      title: 'Review Pending',
      description: 'You can review and approve/reject the batch.',
      buttonText: 'Start Review',
      buttonVisible: true,
      permission: 'payments.batch.review',
      popupMessage: 'confirm-stage-approval',
      action: 'start-review',
    },
    approver: {
      title: 'Batch Initiated (Read-Only)',
      description: 'You can only view this batch. No changes are allowed at this stage.',
      buttonText: '',
      buttonVisible: false,
      permission: 'payments.batch.approve',
      popupMessage: 'confirm-stage-review',
      action: '',
    },
  },
  'Pending Approval': {
    initiator: {
      title: 'Pending Approval',
      description: 'Your batch is pending approval. No edits are allowed at this stage.',
      buttonText: '',
      buttonVisible: false,
      permission: 'payments.batch.add',
      popupMessage: 'confirm-stage-review',
      action: '',
    },
    reviewer: {
      title: 'Pending Approval',
      description: 'Your batch is pending approval. No edits are allowed at this stage.',
      buttonText: '',
      buttonVisible: false,
      permission: 'payments.batch.review',
      popupMessage: '',
      action: 'start-review',
    },
    approver: {
      title: 'Pending Approval',
      description: 'You can review and approve/reject the batch.',
      buttonText: 'Approve Batch',
      buttonVisible: true,
      permission: 'payments.batch.approve',
      popupMessage: 'confirm-stage-approval',
      action: 'approve',
    },
  },
  Approved: {
    initiator: {
      title: 'Batch Approved',
      description: 'The batch has been approved and is ready for processing.',
      buttonText: '',
      buttonVisible: false,
      permission: 'payments.batch.add',
      popupMessage: 'confirm-stage-review',
      action: '',
    },
    reviewer: {
      title: 'Batch Approved',
      description: 'The batch has been approved and no further actions are needed.',
      buttonText: '',
      buttonVisible: false,
      permission: 'payments.batch.review',
      popupMessage: 'confirm-stage-approval',
      action: '',
    },
    approver: {
      title: 'Batch Approved',
      description: 'The batch has been approved and processed. No further actions are needed.',
      buttonText: '',
      buttonVisible: false,
      permission: 'payments.batch.approve',
      popupMessage: 'confirm-stage-review',
      action: '',
    },
  },
  Rejected: {
    initiator: {
      title: 'Batch Rejected',
      description: 'Please re-upload the data and resubmit for review.',
      buttonText: 'Re-Upload Data',
      buttonVisible: true,
      permission: 'payments.batch.add',
      popupMessage: 'confirm-stage-review',
      action: '',
    },
    reviewer: {
      title: 'Batch Rejected',
      description: 'This batch was rejected. Await re-upload from the initiator.',
      buttonText: '',
      buttonVisible: false,
      permission: 'payments.batch.review',
      popupMessage: 'confirm-stage-approval',
      action: '',
    },
    approver: {
      title: 'Batch Rejected',
      description: 'This batch was rejected. Await re-upload from the initiator.',
      buttonText: '',
      buttonVisible: false,
      permission: 'payments.batch.approve',
      popupMessage: 'confirm-stage-review',
      action: '',
    },
  },
  'Review Rejected': {
    initiator: {
      title: 'Review Rejected',
      description: 'Please make corrections and start afresh for review.',
      buttonText: '',
      buttonVisible: false,
      permission: 'payments.batch.add',
      popupMessage: 'confirm-stage-review',
      action: '',
    },
    reviewer: {
      title: 'Review Rejected',
      description: 'Batch review was rejected. Await a new submission from the initiator.',
      buttonText: '',
      buttonVisible: false,
      permission: 'payments.batch.review',
      popupMessage: 'confirm-stage-approval',
      action: '',
    },
    approver: {
      title: 'Review Rejected',
      description: 'Batch review was rejected. Await a new submission from the initiator.',
      buttonText: '',
      buttonVisible: false,
      permission: 'payments.batch.approve',
      popupMessage: 'confirm-stage-review',
      action: '',
    },
  },
};

// Map batch status to the relevant role key
const statusToRoleMap: Record<string, 'initiator' | 'reviewer' | 'approver'> = {
  Initiated: 'initiator',
  'Under Review': 'reviewer',
  'Pending Approval': 'approver',
  Approved: 'initiator',      // adjust these mappings if needed
  Rejected: 'initiator',
  'Review Rejected': 'approver',
};

const SuccessCard = ({ batch, status, isAllowed }: SuccessCardProps) => {
  const isCreator = currentUserId === batch?.createdBy;

  const navigate = useNavigate();

  const [loading, setLoading] = useState(false);
  const [confirmModel, setConfirmModel] = useState<IConfirmModel>();
  const [showConfirmBox, setShowConfirmBox] = useState<boolean>(false);
  const [showStartReview, setShowStartReview] = useState<boolean>(false);
  const [showApprove, setShowApprove] = useState<boolean>(false);
  const [showRejectBatch, setShowRejectReview] = useState<boolean>(false);

  const data = statusContent[status];
  const roleKey = statusToRoleMap[status];
  // const roleKey =
  //   isAllowed('payments.batch.approve') ? 'approver' :
  //     isAllowed('payments.batch.review') ? 'reviewer' :
  //       isAllowed('payments.batch.add') ? 'initiator' : 'initiator';

  let content;

  if (roleKey && data?.[roleKey] && isAllowed(data[roleKey].permission)) {
    const isSelfAction = isCreator && ['reviewer', 'approver'].includes(roleKey);
    //content = data[roleKey];
    if (isSelfAction && status !== 'Rejected') {
      content = {
        title: 'Restricted Action',
        description: 'You cannot review or approve your own batch.',
        buttonText: '',
        buttonVisible: false,
      };
    } else {
      content = data[roleKey];
    }
  } else {
    content = {
      title: 'No Access',
      description: 'You do not have permission to view or edit this batch.',
      buttonText: '',
      buttonVisible: false,
    };
  }

  const [btnAction, setBtnAction] = useState('');

  const openConfirmBox = async (message: string, action: string) => {
    
    setBtnAction(action);

    if (action !== 'start-review' && action !== 'approve' && action !== 'reject') {
      const confirmModel: IConfirmModel = {
        title: "Confirm action",
        btnText: "Send",
        deleteUrl: ``,
        message: message,
      };

      setConfirmModel(confirmModel);
      setTimeout(() => {
        setShowConfirmBox(true);
      }, 500);
    }
  };

  const afterConfirm = async (res: any) => {
    if (res == false) {
      setShowConfirmBox(false);
    } else {
      const initValues = {
        action: btnAction,
        remarks: ''
      };
      const response = await paymentBatchService.updatePaymentStage(initValues, batch.id);
      setShowConfirmBox(false);
      setTimeout(() => {
        window.location.reload();
      }, 3000);
    }
  };

  const handleStartReview = async (values: any) => {
    setLoading(true);

    const response = await paymentBatchService.updatePaymentStage(values, batch.id);
    setShowConfirmBox(false);
    setTimeout(() => {
      window.location.reload();
    }, 3000);
    setLoading(false);
    setShowStartReview(false);
  };

  const handleConfirmPayment = async () => {
    setLoading(true);

    const searchParams = {
      batchId: batch.id,
      statusId: 1,
      isFarmerValid: true
    };

    const response = await paymentBatchService.payAllBatchAsync(searchParams);
    setLoading(false);

    toast.info("Payment processing is in progress");
    navigate(`/payment-batch/history/${batch.id}`);
  };

  useEffect(() => {
    
    if (btnAction === 'start-review') {
      setShowApprove(false);
      setShowStartReview(true);
      setShowRejectReview(false);
    } else if (btnAction === 'approve') {
      setShowApprove(true);
      setShowStartReview(false);
      setShowRejectReview(false);
    } else if (btnAction === 'reject') {
      setShowApprove(false);
      setShowStartReview(false);
      setShowRejectReview(true)
    }
  }, [btnAction]);

  return (
    <div>
      <div className='notice d-flex bg-light-gray rounded border-gray border border-dashed mb-9 p-6'>
        <KTIcon iconName='information-5' className='fs-2tx me-4' />

        <div className='d-flex flex-stack flex-grow-1'>
          <div className='fw-bold'>
            <h4 className='text-gray-800 fw-bolder'>{content.title}</h4>
            <div className='fs-6 text-gray-700 fw-normal'>
              {content.description}
              {content.buttonVisible && content.buttonText && (
                <div className='my-4'>
                  <button
                    className='fw-bolder btn btn-theme'
                    onClick={() => openConfirmBox(content.popupMessage ?? '', content.action ?? '')}
                  >
                    {loading ? 'Please wait...' : content.buttonText}
                  </button>

                  {/* Show Reject Button only for approver on 'Pending Approval' */}
                  {roleKey === 'approver' && status === 'Pending Approval' && (
                    <button
                      className='fw-bolder btn btn-secondary mx-2'
                      onClick={() => openConfirmBox('Are you sure you want to reject this batch?', 'reject')}
                    >
                      {loading && btnAction === 'reject' ? 'Please wait...' : 'Reject Batch'}
                    </button>
                  )}
                </div>
              )}

            </div>
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

      {showStartReview && <StartReviewCard onStartReview={handleStartReview} loading={loading} />}
      {showRejectBatch && <RejectBatch onStartReview={handleStartReview} loading={loading} />}
      {showApprove && <StartApprove paymentBatchId={batch.id} onApprove={handleConfirmPayment} loading={loading} />}
    </div>
  );
};

export default SuccessCard;
