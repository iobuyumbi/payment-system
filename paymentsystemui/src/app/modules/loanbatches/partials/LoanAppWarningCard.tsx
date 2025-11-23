import { useState } from 'react';
import { KTIcon } from '../../../../_metronic/helpers'
import { ImportModal } from '../../../../_shared/Modals/ImportModal';

const LoanAppWarningCard = (props: any) => {
    const [showImport, setShowImport] = useState<boolean>(false);
    const afterConfirm = (value: any) => {
        setShowImport(value);
    }
    return (<>
        <div>
            <div className='notice d-flex bg-light-warning rounded border-warning border border-dashed mb-9 p-6'>
                <KTIcon iconName='information-5' className='fs-2tx text-warning me-4' />

                <div className='d-flex flex-stack flex-grow-1'>
                    <div className='fw-bold'>
                        <h4 className='text-gray-800 fw-bolder'>Action Required</h4>
                        <div className='fs-6 text-gray-700 fw-normal'>
                            This Loan application batch cannot be sent for review as all the rows could not get imported.
                            Please fix the data and reupload the Excel file again.
                            <div className="my-4">
                                <button className='fw-bolder btn btn-theme' onClick={() => setShowImport(true)}>
                                    Re-Upload Data
                                </button>
                            </div>

                        </div>
                    </div>
                </div>
            </div>
        </div>

        {showImport && <ImportModal
            exModule="loanApplication"
            title={"Loan Applications"}
            afterConfirm={afterConfirm}
            batchId={props.batchId}
        />}
    </>
    )

}

export default LoanAppWarningCard