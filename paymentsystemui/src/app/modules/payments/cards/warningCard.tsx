import { useState } from 'react';
import { KTIcon } from '../../../../_metronic/helpers'
import { ImportModal } from '../../../../_shared/Modals/ImportModal';
import { useParams } from 'react-router-dom';
import { isAllowed } from '../../../../_metronic/helpers/ApiUtil';

const WarningCard = (props: any) => {

    const [showImport, setShowImport] = useState<boolean>(false);
    const afterConfirm = (value: any) => {
        setShowImport(value);
        window.location.reload();
    }

    const message = ['Rejected', 'Review Rejected'].includes(props.status) ? 'This batch was rejected. Please re-upload the data and resubmit for review.' :
        ' This Payment Batch cannot be sent for review as all the rows could not get imported.  Please fix the data and reupload the Excel file again.';

    return (<>
        <div>
            <div className='notice d-flex bg-light-warning rounded border-warning border border-dashed mb-9 p-6'>
                <KTIcon iconName='information-5' className='fs-2tx text-warning me-4' />

                <div className='d-flex flex-stack flex-grow-1'>
                    <div className='fw-bold'>
                        <h4 className='text-gray-800 fw-bolder'>Action Required</h4>
                        <div className='fs-6 text-gray-700 fw-normal'>
                            {message}
                            <div className="my-4">
                                {isAllowed('payments.batch.add') &&
                                    <button className='fw-bolder btn btn-theme' onClick={() => setShowImport(true)}>
                                        Re-Upload Data
                                    </button>
                                }
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </div>

        {showImport && <ImportModal
            exModule={props.paymentModule === 3 ? "PaymentDeductibles" : "PaymentFacilitations"}
            title={"Payments"}
            afterConfirm={afterConfirm}
            paymentBatchId={props.paymentBatchId}
            loanBatchName={props.loanBatchName}
        />}
    </>
    )

}

export default WarningCard