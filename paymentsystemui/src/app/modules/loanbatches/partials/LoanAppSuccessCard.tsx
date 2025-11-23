import { useEffect, useState } from 'react';
import { KTIcon } from '../../../../_metronic/helpers'
import { IConfirmModel } from '../../../../_models/confirm-model';
import { ConfirmBox } from "../../../../_shared/Modals/ConfirmBox";
import { useNavigate } from 'react-router-dom';
import LoanBatchService from '../../../../services/LoanBatchService';
import StartReviewCard from '../../payments/cards/StartReviewCard';

const loanBatchService = new LoanBatchService();

interface LoanAppSuccessCardProps {
    batch: any,
    status: 'Initiated' | 'Under Review' | 'Approved' | 'Rejected' | 'Review Rejected';
    excelImportId: any,
    isAllowed: (permission: string) => boolean;
}

const statusContent = {
    Initiated: {
        initiator: {
            title: 'Review & Approve',
            description: 'Please review and approve the imported loan applications in this batch.',
            buttonText: 'Review & Approve',
            buttonVisible: true,
            permission: 'payments.batch.add',
            popupMessage: 'confirm-stage-approval',
            action: 'approve',
        },
        reviewer: {
            title: 'Review & Approve',
            description: 'Please review and approve the imported loan applications in this batch.',
            buttonText: 'Review & Approve',
            buttonVisible: true,
            permission: 'payments.batch.review',
            popupMessage: 'confirm-stage-approval',
            action: 'approve',
        },
        approver: {
            title: 'Review & Approve',
            description: 'Please review and approve the imported loan applications in this batch.',
            buttonText: 'Review & Approve',
            buttonVisible: true,
            permission: 'payments.batch.approve',
            popupMessage: 'confirm-stage-approval',
            action: 'approve',
        },
    },
    'Under Review': {
        initiator: {
            title: 'Under Review',
            description: 'The imported loan applications are under review. No edits are allowed at this stage.',
            buttonText: '',
            buttonVisible: false,
            permission: 'payments.batch.add',
            popupMessage: 'confirm-stage-review',
            action: '',
        },
        reviewer: {
            title: 'Review Pending',
            description: 'You can review and approve/reject the loan applications batch.',
            buttonText: 'Start Review',
            buttonVisible: true,
            permission: 'payments.batch.review',
            popupMessage: 'confirm-stage-approval',
            action: 'start-review',
        },
        approver: {
            title: 'Review Pending',
            description: 'You can only view this loan applications batch. No changes are allowed at this stage.',
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
            description: 'Your loan applications batch is pending approval. No edits are allowed at this stage.',
            buttonText: '',
            buttonVisible: false,
            permission: 'payments.batch.add',
            popupMessage: 'confirm-stage-review',
            action: '',
        },
        reviewer: {
            title: 'Pending Approval',
            description: 'Your loan applications batch is pending approval. No edits are allowed at this stage.',
            buttonText: '',
            buttonVisible: false,
            permission: 'payments.batch.review',
            popupMessage: '',
            action: 'start-review',
        },
        approver: {
            title: 'Pending Approval',
            description: 'You can review and approve/reject the loan applications batch.',
            buttonText: 'Approve',
            buttonVisible: true,
            permission: 'payments.batch.approve',
            popupMessage: 'confirm-stage-approval',
            action: 'approve',
        },
    },
    Approved: {
        initiator: {
            title: 'Loan Applications Approved',
            description: 'The loan applications have been approved and can be seen under Loan Applications section.',
            buttonText: '',
            buttonVisible: false,
            permission: 'payments.batch.add',
            popupMessage: 'confirm-stage-review',
            action: '',
        },
        reviewer: {
            title: 'Loan Applications Approved',
            description: 'The loan applications have been approved and no further actions are needed.',
            buttonText: '',
            buttonVisible: false,
            permission: 'payments.batch.review',
            popupMessage: 'confirm-stage-approval',
            action: '',
        },
        approver: {
            title: 'Loan Applications Approved',
            description: 'The loan applications have been approved and processed. No further actions are needed.',
            buttonText: '',
            buttonVisible: false,
            permission: 'payments.batch.approve',
            popupMessage: 'confirm-stage-review',
            action: '',
        },
    },
    Rejected: {
        initiator: {
            title: 'Loan Applications Rejected',
            description: 'Please re-upload the data and resubmit for review.',
            buttonText: 'Re-Upload Data',
            buttonVisible: true,
            permission: 'payments.batch.add',
            popupMessage: 'confirm-stage-review',
            action: '',
        },
        reviewer: {
            title: 'Loan Applications Rejected',
            description: 'This loan applications batch was rejected. Await re-upload from the initiator.',
            buttonText: '',
            buttonVisible: false,
            permission: 'payments.batch.review',
            popupMessage: 'confirm-stage-approval',
            action: '',
        },
        approver: {
            title: 'Loan Applications Rejected',
            description: 'You can only view this loan applications batch. No changes are allowed at this stage.',
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
            description: 'Loan applications review was rejected. Await a new submission from the initiator.',
            buttonText: '',
            buttonVisible: false,
            permission: 'payments.batch.review',
            popupMessage: 'confirm-stage-approval',
            action: '',
        },
        approver: {
            title: 'Review Rejected',
            description: 'You can only view this loan applications batch. No changes are allowed at this stage.',
            buttonText: '',
            buttonVisible: false,
            permission: 'payments.batch.approve',
            popupMessage: 'confirm-stage-review',
            action: '',
        },
    },
};

const LoanAppSuccessCard = ({ batch, status, excelImportId, isAllowed }: LoanAppSuccessCardProps) => {
    const navigate = useNavigate();

    const [loading, setLoading] = useState(false);
    const [confirmModel, setConfirmModel] = useState<IConfirmModel>();
    const [showConfirmBox, setShowConfirmBox] = useState<boolean>(false);
    const [showStartReview, setShowStartReview] = useState<boolean>(false);

    const data = statusContent[status];

    let content;

    if (isAllowed(data?.initiator?.permission)) {
        content = data.initiator;
    } else if (isAllowed(data?.reviewer?.permission)) {
        content = data.reviewer;
    } else if (isAllowed(data?.approver?.permission)) {
        content = data.approver;
    }
    else {
        // fallback if no permission
        content = {
            title: 'No Access',
            description: 'You do not have permission to view or edit the loan applications.',
            buttonText: '',
            buttonVisible: false,
        };
    }

    const [btnAction, setBtnAction] = useState('');

    const openConfirmBox = async (message: string, action: string) => {
        setBtnAction(action);

        if (action !== 'start-review'
            //&& action !== 'approve'
        ) {
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
        debugger
        if (res == false) {
            setShowConfirmBox(false);
        }
        else {
            const initValues = {
                action: btnAction,
                remarks: ''
            };
            const response = await loanBatchService.updateLoanBatchStage(initValues, batch.id, excelImportId);
            debugger
            setTimeout(() => {
                if (btnAction == 'approve' || btnAction == 'approved') {
                    navigate(`/loan-batch-details/${batch.id}/loan-applications`)
                }
                else {
                    window.location.reload();
                    //navigate('/loans')
                }
                setShowConfirmBox(false);
                //window.location.reload();
            }, 1000);
        }
    };

    const handleStartReview = async (values: any) => {
        //console.log(values);
        setLoading(true);

        const response = await loanBatchService.updateLoanBatchStage(values, batch.id, excelImportId);
        setShowConfirmBox(false);
        setTimeout(() => {
            navigate(`/loan-batch-details/${batch.id}/loan-applications`);
            // window.location.reload();
        }, 3000);
        setLoading(false);
        setShowStartReview(false);
    }

    useEffect(() => {
        if (btnAction == 'start-review') {
            setShowStartReview(true);
        }
        // else if (btnAction == 'approve') {
        //     setShowApprove(true);
        // }

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
                                    <button className='fw-bolder btn btn-theme'
                                        onClick={() => openConfirmBox(content.popupMessage ?? '', content.action ?? '')}>
                                        {content.buttonText}
                                    </button>
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
            {showStartReview && <StartReviewCard onStartReview={handleStartReview} />}
            {/* 
            {showApprove && <StartApprove onApprove={handleConfirmPayment} loading={loading} />} */}
        </div>
    );
};

export default LoanAppSuccessCard;
