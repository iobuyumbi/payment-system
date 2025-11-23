import { Fragment } from "react/jsx-runtime";
import { KTIcon } from "../../../../_metronic/helpers";
import { Content } from "../../../../_metronic/layout/components/content";
import AddLoanBatchItem from "./AddLoanBatchItem";

export default function LoanItemsModal(props: any) {
    const { loanBatchId, loanBatchName, afterConfirm ,data, itemsData} = props;
    return (
        <Fragment>
            <div
                className='modal fade show d-block'
                id='kt_modal_confiem_box'
                role='dialog'
                tabIndex={-1}
                aria-modal='true'
            >
                {/* begin::Modal dialog */}
                <div className='modal-dialog modal-lg'>
                    {/* begin::Modal content */}
                    <div className='modal-content'>
                        <div className='modal-header'>
                            <div className="w-100 d-flex flex-row align-items-center justify-content-between">
                                {/* begin::Modal title */}
                                <div>
                                    <h2 className='fw-semibold'>Manage Loan Product Items</h2>
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
                        <Content>
                            <AddLoanBatchItem
                                loanBatchId={loanBatchId}
                                loanBatchName={loanBatchName}
                                afterConfirm={afterConfirm}
                                editData={data}
                                itemsData={itemsData}
                            />
                        </Content>
                        {/* end::Modal body */}
                    </div>
                    {/* end::Modal content */}
                </div>
                {/* end::Modal dialog */}
            </div>
            {/* begin::Modal Backdrop */}
            <div className='modal-backdrop fade show'></div>
            {/* end::Modal Backdrop */}
        </Fragment>
    )
}
