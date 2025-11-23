import { Content } from '../../../../_metronic/layout/components/content'
import { KTIcon } from '../../../../_metronic/helpers'
import { LoanAppList } from './LoanAppList'
import { isAllowed } from '../../../../_metronic/helpers/ApiUtil'


const FarmerLoanApplications = (props: any) => {

  return (
    <Content>
      {isAllowed('loans.applications.view') &&
        <div className='card shadow-none rounded-0'>
          <div className='card-header' id='kt_activities_header'>
            <h3 className='card-title fw-bolder text-gray-900'>Loan Applications</h3>

            <div className='card-toolbar'>
              {isAllowed('loans.applications.add') &&
                <a
                  href='#'
                  className='btn btn-sm btn-primary me-3'
                  data-bs-toggle='modal'
                  data-bs-target='#kt_modal_offer_a_deal'
                >
                  <KTIcon iconName='pencil' className='fs-6' iconType='outline' /> Add new application
                </a>
              }
            </div>
          </div>
          <div className='card-body position-relative' id='kt_activities_body'>
            <LoanAppList farmer={props.farmer} />
          </div>

        </div>
      }
    </Content>
  )
}

export default FarmerLoanApplications
