import { KTCard, KTCardBody, KTIcon } from '../../../_metronic/helpers'
import { Content } from '../../../_metronic/layout/components/content'
import { PageLink, PageTitle } from '../../../_metronic/layout/core'
import { ChartsWidget1, ChartsWidget2, ChartsWidget3, ChartsWidget4 } from '../../../_metronic/partials/widgets';
import PaymentBatchPieChart from './PaymentBatchPieChart';

const breadCrumbs: Array<PageLink> = [
    {
        title: 'Dashboard',
        path: '/dashboard',
        isSeparator: false,
        isActive: true,
    },
    {
        title: '',
        path: '',
        isSeparator: true,
        isActive: true,
    },
];

const LoanPerformanceReport = () => {
    return (
        <Content>
            <PageTitle breadcrumbs={breadCrumbs}>Loan Performance Report</PageTitle>
            <KTCard>
                <div className='card-header' id='kt_activities_header'>
                    <h3 className='card-title fw-bolder text-gray-900'>Loan Performance Report</h3>

                    <div className='card-toolbar'>
                        <button
                            type='button'
                            className='btn btn-sm btn-icon btn-active-light-primary me-n5'
                            id='kt_activities_close'
                        >
                            <KTIcon iconName='cross' className='fs-1' />
                        </button>
                    </div>
                </div>
                <KTCardBody>
                    <PaymentBatchPieChart />
                </KTCardBody>

            </KTCard>
        </Content>
    )
}

export default LoanPerformanceReport