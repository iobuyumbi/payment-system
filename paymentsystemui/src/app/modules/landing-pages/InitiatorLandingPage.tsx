import { PaymentStatsCard } from '../../../_shared/PaymentStatsCard'
import { Content } from '../../../_metronic/layout/components/content'


const InitiatorLandingPage = () => {
  return (
    <Content>
      <div>
        <div className="row">
          <div className='col-md-6 col-xl-6'>
            <PaymentStatsCard
              icon='media/logos/small.jpg'
              badgeColor='success'
              status='Approved'
              statusColor='success'
              title='New batch'
              description='Kenya | Demo Project K | Kenya batch | Kenya Shilling'
              date='Nov 20, 2024'
              budget='$36,400.00'
              progress={30} link={''} />
          </div>
          <div className='col-md-6 col-xl-6'>
            <PaymentStatsCard
              icon='media/logos/small.jpg'
              badgeColor='info'
              status='Under Review'
              statusColor='info'
              title='November batch'
              description='Kenya | Demo Project K | Kenya batch | Kenya Shilling'
              date='Nov 10, 2024'
              budget='$36,400.00'
              progress={30} link={''} />
          </div>
        </div>

      </div>
    </Content>
  )
}

export default InitiatorLandingPage