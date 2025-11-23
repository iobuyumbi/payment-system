import { useEffect, useState } from 'react'
import LoanApplicationService from '../../../services/LoanApplicationService';
import { Content } from '../../../_metronic/layout/components/content';
import LoanAppImportHistory from './partials/LoanAppImportHistory';

const loanApplicationService = new LoanApplicationService();

const LoanApplicationImports = (props: any) => {
    const { batchId } = props;
    const [loanBatch, setLoanBatch] = useState<any>();
    const [data, setData] = useState<any>();

    const getImportHistory = async () => {
        var response = await loanApplicationService.getImportHistory(batchId);
        console.log(response)
        if (response) {
            setData(response);
            setLoanBatch((response != null && response.length > 0) ? response[0].loanBatch : {})
        }
    };

    useEffect(() => {
        getImportHistory();
    }, [batchId]);


    return (
        <Content>
            <div className='row mb-5'>
                <div className='col-md-12'>
                    {data && <LoanAppImportHistory data={data} loanBatch={loanBatch} />}
                </div>

            </div>
        </Content>
    )
}

export default LoanApplicationImports
