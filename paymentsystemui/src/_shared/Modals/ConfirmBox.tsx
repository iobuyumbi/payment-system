import { useState } from "react";
import CommonService from "../../services/CommonService";
import { KTIcon } from "../../_metronic/helpers";
import clsx from "clsx";

const commonService = new CommonService();

export function ConfirmBox(props: any) {
    const { confirmModel, afterConfirm, btnType = 'danger' } = props;
    const [loading, setLoading] = useState(false);

    const onCancel = () => {
        afterConfirm(false);
    }

    const onOk = async () => {
        if (confirmModel.message === "confirm-stage-review" || confirmModel.message === "confirm-stage-approval") {
            afterConfirm();
            return;
        }
        setLoading(true);
        const response = await commonService.deleteEntity(confirmModel.deleteUrl);
        if (response && response.id) {
            setTimeout(() => {
                setLoading(false);
                afterConfirm(true);
            }, 1000);
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
                <div className='modal-dialog modal-md'>
                    {/* begin::Modal content */}
                    <div className='modal-content'>
                        <div className='modal-header'>
                            <div className="w-100 d-flex flex-row align-items-center justify-content-between">
                                {/* begin::Modal title */}
                                <div>
                                    <h2 className='fw-semibold'>{confirmModel.title}</h2>
                                </div>
                                {/* end::Modal title */}

                                {/* begin::Close */}
                                <div
                                    className='btn btn-icon btn-sm btn-active-icon-primary'
                                    data-kt-users-modal-action='close'
                                    onClick={() => afterConfirm(false)}
                                    style={{ cursor: 'pointer' }}
                                >
                                    <KTIcon iconName="abstract-11" iconType="outline" className='fs-1' />
                                </div>
                                {/* end::Close */}
                            </div>

                        </div>
                        <div className='modal-body'>
                            <div className="lh-lg fs-5">
                                {
                                    confirmModel.message === "confirm-stage-review" && (
                                        <>
                                            <p>Are you sure you want to send this payment for review?<br />
                                                By confirming, you acknowledge to have reviewed all the data.
                                            </p>
                                            <p>Please click the <strong>below</strong> button to confirm.</p>
                                        </>
                                    )
                                }
                                {
                                    confirmModel.message === "confirm-stage-approval" && (
                                        <>
                                            <p>Are you sure you want to send this for approval?<br />
                                                By confirming, you acknowledge to have reviewed all the data.
                                            </p>
                                            <p>Please click the <strong>below</strong> button to confirm.</p>
                                        </>
                                    )
                                }
                                {
                                    confirmModel.message === "delete-project" && (
                                        <>
                                            <p>This action <strong>cannot</strong> be undone.
                                                This will permanently delete the project.
                                            </p>
                                            <p>Please click the <strong>delete</strong> button to confirm.</p>
                                        </>
                                    )
                                }
                                {
                                    confirmModel.message === "delete-processingfee" && (
                                        <>
                                            <p>This action <strong>cannot</strong> be undone.
                                                This will permanently delete the additional fees.
                                            </p>
                                            <p>Please click the <strong>delete</strong> button to confirm.</p>
                                        </>
                                    )
                                }
                                {
                                    confirmModel.message === "delete-user" && (
                                        <>
                                            <p>This action <strong>cannot</strong> be undone.
                                                This will permanently delete the user.
                                            </p>
                                            <p>Please click the <strong>delete</strong> button to confirm.</p>
                                        </>
                                    )
                                }
                                {
                                    confirmModel.message === "delete-loan-batch" && (
                                        <>
                                            <p>This action <strong>cannot</strong> be undone.
                                                This will permanently delete the Loan Product.
                                            </p>
                                            <p>Please click the <strong>delete</strong> button to confirm.</p>
                                        </>
                                    )
                                }

                                {
                                    confirmModel.message === "delete-county" && (
                                        <>
                                            <p>This action <strong>cannot</strong> be undone.
                                                This will delete the selected <strong>County</strong>.
                                                <br />

                                            </p>
                                            <p>Please click the <strong>delete</strong> button to confirm.</p>
                                        </>
                                    )
                                }
                                {
                                    confirmModel.message === "delete-subcounty" && (
                                        <>
                                            <p>This action <strong>cannot</strong> be undone.
                                                This will delete the selected <strong>Subcounty</strong>.
                                                <br />

                                            </p>
                                            <p>Please click the <strong>delete</strong> button to confirm.</p>
                                        </>
                                    )
                                }
                                {
                                    confirmModel.message === "delete-ward" && (
                                        <>
                                            <p>This action <strong>cannot</strong> be undone.
                                                This will delete the selected <strong>Ward</strong>.
                                                <br />

                                            </p>
                                            <p>Please click the <strong>delete</strong> button to confirm.</p>
                                        </>
                                    )
                                }

                                {
                                    confirmModel.message === "delete-cooperative" && (
                                        <>
                                            <p>This action <strong>cannot</strong> be undone.
                                                This will permanently delete the cooperative.
                                            </p>
                                            <p>Please click the <strong>delete</strong> button to confirm.</p>
                                        </>
                                    )
                                }
                                {
                                    confirmModel.message === "delete-farmer" && (
                                        <>
                                            <p>This action <strong>cannot</strong> be undone.
                                                This will permanently delete the farmer, associated loans, loan applications (if any) etc.
                                            </p>
                                            <p>Please click the <strong>delete</strong> button to confirm.</p>
                                        </>
                                    )
                                }
                                {
                                    confirmModel.message === "delete-payment-batch" && (
                                        <>
                                            <p>This action <strong>cannot</strong> be undone.
                                                This will permanently delete the payment batch, associated payments (if any) etc.
                                            </p>
                                            <p>Please click the <strong>delete</strong> button to confirm.</p>
                                        </>
                                    )
                                }
                                {
                                    confirmModel.message === "cancel-invoice" && (
                                        <>
                                            <p>This action <strong>cannot</strong> be undone.
                                                This will cancel the current invoice, associated payment info (if any) etc.
                                            </p>
                                            <p>Please click the <strong>cancel</strong> button to confirm.</p>
                                        </>
                                    )
                                }
                                {
                                    confirmModel.message === "delete-Lead" && (
                                        <>
                                            <p>This action <strong>cannot</strong> be undone.
                                                This will delete the current lead
                                            </p>
                                            <p>Please click the <strong>delete</strong> button to confirm.</p>
                                        </>
                                    )
                                }
                                {
                                    confirmModel.message === "delete-lead-follow-up" && (
                                        <>
                                            <p>This action <strong>cannot</strong> be undone.
                                                This will delete the current lead follow up
                                            </p>
                                            <p>Please click the <strong>delete</strong> button to confirm.</p>
                                        </>
                                    )
                                }
                                {
                                    confirmModel.message === "delete-application" && (
                                        <>
                                            <p>This action <strong>cannot</strong> be undone.
                                                This will delete the selected <strong>Loan application</strong>.
                                                <br />
                                                Kindly note that an application cannot be deleted if it is in use.
                                            </p>
                                            <p>Please click the <strong>delete</strong> button to confirm.</p>
                                        </>
                                    )
                                }
                                {
                                    confirmModel.message === "delete-org" && (
                                        <>
                                            <p>This action <strong>cannot</strong> be undone.
                                                This will delete the selected <strong>Organization</strong>.
                                                <br />
                                                Kindly note that an Organization cannot be deleted if it is in use.
                                            </p>
                                            <p>Please click the <strong>delete</strong> button to confirm.</p>
                                        </>
                                    )
                                }
                                {
                                    confirmModel.message === "delete-employee" && (
                                        <>
                                            <p>This action <strong>cannot</strong> be undone.
                                                This will delete the selected <strong>Employee</strong>.
                                                <br />
                                                Kindly note that an Employee cannot be deleted if it is in use.
                                            </p>
                                            <p>Please click the <strong>delete</strong> button to confirm.</p>
                                        </>
                                    )
                                }
                                {
                                    confirmModel.message === "delete-message" && (
                                        <>
                                            <p>This action <strong>cannot</strong> be undone.
                                                This will delete the selected <strong>message</strong>.
                                                <br />

                                            </p>
                                            <p>Please click the <strong>delete</strong> button to confirm.</p>
                                        </>
                                    )
                                }
                                {
                                    confirmModel.message === "delete-quotation" && (
                                        <>
                                            <p>This action <strong>cannot</strong> be undone.
                                                This will delete the selected <strong>quotation</strong>.
                                                <br />

                                            </p>
                                            <p>Please click the <strong>delete</strong> button to confirm.</p>
                                        </>
                                    )
                                }
                                {
                                    confirmModel.message === "delete-dispatch" && (
                                        <>
                                            <p>This action <strong>cannot</strong> be undone.
                                                This will delete the selected <strong>dispatch</strong>.
                                                <br />

                                            </p>
                                            <p>Please click the <strong>delete</strong> button to confirm.</p>
                                        </>
                                    )
                                }
                                {
                                    confirmModel.message === "delete-daily-sale" && (
                                        <>
                                            <p>This action <strong>cannot</strong> be undone.
                                                This will delete the selected <strong>sale</strong>.
                                                <br />

                                            </p>
                                            <p>Please click the <strong>delete</strong> button to confirm.</p>
                                        </>
                                    )
                                }
                                {
                                    confirmModel.message === "delete-tax" && (
                                        <>
                                            <p>This action <strong>cannot</strong> be undone.
                                                This will delete the selected <strong>tax</strong>.
                                                <br />

                                            </p>
                                            <p>Please click the <strong>delete</strong> button to confirm.</p>
                                        </>
                                    )
                                }
                                {
                                    confirmModel.message === "delete-terms" && (
                                        <>
                                            <p>This action <strong>cannot</strong> be undone.
                                                This will delete the selected <strong>terms and conditions</strong>.
                                                <br />

                                            </p>
                                            <p>Please click the <strong>delete</strong> button to confirm.</p>
                                        </>
                                    )
                                }
                                {
                                    confirmModel.message === "delete-note" && (
                                        <>
                                            <p>This action <strong>cannot</strong> be undone.
                                                This will delete the selected <strong>note</strong>.
                                                <br />

                                            </p>
                                            <p>Please click the <strong>delete</strong> button to confirm.</p>
                                        </>
                                    )
                                }
                                {
                                    confirmModel.message === "delete-ticket" && (
                                        <>
                                            <p>This action <strong>cannot</strong> be undone.
                                                This will delete the selected <strong>ticket</strong>.
                                                <br />

                                            </p>
                                            <p>Please click the <strong>delete</strong> button to confirm.</p>
                                        </>
                                    )
                                }
                                {
                                    confirmModel.message === "delete-designation" && (
                                        <>
                                            <p>This action <strong>cannot</strong> be undone.
                                                This will delete the selected <strong>designation</strong>.
                                                <br />

                                            </p>
                                            <p>Please click the <strong>delete</strong> button to confirm.</p>
                                        </>
                                    )
                                }
                                {
                                    confirmModel.message === "delete-department" && (
                                        <>
                                            <p>This action <strong>cannot</strong> be undone.
                                                This will delete the selected <strong>department</strong>.
                                                <br />

                                            </p>
                                            <p>Please click the <strong>delete</strong> button to confirm.</p>
                                        </>
                                    )
                                }
                                {
                                    confirmModel.message === "delete-loanBatchItem" && (
                                        <>
                                            <p>This action <strong>cannot</strong> be undone.
                                                This will delete the selected <strong>Loan Product item</strong>.
                                                <br />

                                            </p>
                                            <p>Please click the <strong>delete</strong> button to confirm.</p>
                                        </>
                                    )
                                }
                                {
                                    confirmModel.message === "delete-loanItem" && (
                                        <>
                                            <p>This action <strong>cannot</strong> be undone.
                                                This will delete the selected <strong>loan item</strong>.
                                                <br />

                                            </p>
                                            <p>Please click the <strong>delete</strong> button to confirm.</p>
                                        </>
                                    )
                                }
                                {
                                    confirmModel.message === "delete-masterLoanItem" && (
                                        <>
                                            <p>This action <strong>cannot</strong> be undone.
                                                This will delete the selected <strong>loan item</strong>.
                                                <br />

                                            </p>
                                            <p>Please click the <strong>delete</strong> button to confirm.</p>
                                        </>
                                    )
                                }
                                {
                                    confirmModel.message === "delete-itemCategory" && (
                                        <>
                                            <p>This action <strong>cannot</strong> be undone.
                                                This will delete the selected <strong>item category</strong>.
                                                <br />

                                            </p>
                                            <p>Please click the <strong>delete</strong> button to confirm.</p>
                                        </>
                                    )
                                }
                            </div>


                        </div>
                        <div className="modal-footer">
                            <button onClick={() => onCancel()}
                                type="button"
                                className="btn btn-light"
                                data-bs-dismiss="modal"
                            >
                                Cancel
                            </button>
                            <button onClick={() => onOk()} type="button" className={`btn btn-${btnType}`}>
                                {/* {confirmModel.btnText} */}
                                {!loading && confirmModel.btnText}
                                {loading && (
                                    <span className='indicator-progress' style={{ display: 'block' }}>
                                        Please wait...{' '}
                                        <span className='spinner-border spinner-border-sm align-middle ms-2'></span>
                                    </span>
                                )}
                            </button>
                        </div>
                        {/* end::Modal body */}
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