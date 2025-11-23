import { useState } from "react";
import { KTIcon } from "../../../../_metronic/helpers";
import { Content } from "../../../../_metronic/layout/components/content";
import AddLoanBatchApplication from "../AddLoanBatchApplication";
import AddLoanBatchItem from "./AddLoanBatchItem";
import { useSelector } from "react-redux";
import { isAllowed } from "../../../../_metronic/helpers/ApiUtil";

export default function LoanApplicationsModal(props: any) {
    const { afterConfirm, isAdd, reloadApplications, loanBatch } = props;
    const loanApplication: any = useSelector(
        (state: any) => state?.loanApplications
    );
    const [title] = useState<any>(isAdd ? "Add" : "Edit");
    return (
        <>
            {isAllowed('loans.applications.add') ?
                <div
                    className='modal fade show d-block'
                    id='kt_modal_confiem_box'
                    role='dialog'
                    tabIndex={-1}
                    aria-modal='true'
                >
                    {/* begin::Modal dialog */}
                    <div className='modal-dialog modal-xl'>
                        {/* begin::Modal content */}
                        <div className='modal-content'>
                            <div className='modal-header'>
                                <div className="w-100 d-flex flex-row align-items-center justify-content-between">
                                    {/* begin::Modal title */}
                                    <div>
                                        <h2 className='fw-semibold'>{title} Loan Application</h2>
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
                                <AddLoanBatchApplication
                                    loanBatch={loanBatch}
                                    afterConfirm={afterConfirm}
                                    isAdd={isAdd}
                                    loanApplication={loanApplication}
                                    reloadApplications={reloadApplications}
                                />
                            </Content>
                            {/* end::Modal body */}
                        </div>
                        {/* end::Modal content */}
                    </div>
                    {/* end::Modal dialog */}
                </div> : ""}
            {/* begin::Modal Backdrop */}
            <div className='modal-backdrop fade show'></div>
            {/* end::Modal Backdrop */}
        </>
    )
}
