import React, { useEffect, useState } from 'react'
import { PaymentBatchStats } from '../initiator/PaymentBatchStats'
import { PaymentStatsCard } from '../../../../_shared/PaymentStatsCard';
import moment from 'moment';
import { ActivityLog } from '../../../../_shared/Widgets/ActivityLog';
import ReportService from '../../../../services/ReportService';
const reportService = new ReportService();

const ReviewerDashboard = () => {
  const [data, setData] = useState<any>([]);

  useEffect(() => {
    const bindStats = async () => {
      const data: any = {
        pageNumber: 1,
        pageSize: 10000,
      };
      const response = await reportService.getPaymentStats(data);
      if (response) {
        setData(response)
      }
    }

    bindStats();
  }, []);


  return (<>
    <div className='row g-5 g-xl-10 mb-5 mb-xl-10'>
      {
        data && data.map((item: any, index: number) =>
          <div className='col-md-6 col-xl-6' key={item.id}>
            <PaymentStatsCard
              // link={`/payment-batch/review/${item.id}`}
              data={item}
              icon='media/logos/small.jpg'
              badgeColor='success'
              status={item.status.stageText}
              statusColor='success'
              title={item.batchName}
              description={`${item.loanBatches[0].name} | ${item.country.countryName}`}
              date={moment(item.dateCreated).format('MMM DD, YYYY')}
              budget={item.paymentStats.totalAmount}
              progress={item.paymentStats.beneficiaryCount} />
          </div>
        )
      }

      <div className='col-md-6 col-lg-6 col-xl-6 col-xxl-6'>
        {/* <PaymentBatchStats className={''} description={'Payment batches initiated'} color={'#d9d9c3'} img={''} /> */}

      </div>
    </div>
    <div className="row">
      <div className="col-md-12">
        <ActivityLog />
      </div>
    </div>

  </>)
}

export default ReviewerDashboard
