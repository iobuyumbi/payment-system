
import { FC } from 'react'
import { useNavigate } from 'react-router-dom'
import { useDispatch } from 'react-redux';
import { addToPaymentBatch } from '../_features/payment-batch/paymentBatchSlice';

type Props = {
  icon: string
  badgeColor: string
  status: string
  statusColor: string
  title: string
  description: string
  date: string
  budget: string
  progress: number
  data?: any;
  link?: string
}

const PaymentStatsCard: FC<Props> = ({
  icon,
  badgeColor,
  status,
  statusColor,
  title,
  description,
  date,
  budget,
  progress,
  data = undefined,
  link
}) => {
  const navigate = useNavigate();
  const dispatch = useDispatch();

  const titleClickHandler = (value: any) => {
    dispatch(addToPaymentBatch(value));
    //navigate(`/payment-batch-detail/farmers`)
    
    // if (hasRole('Admin')) {
    //   navigate(`/payment-batch/details/${value.id}`);
    // }
    // else if (hasRole('Initiator')) {
    //   navigate(`/payment-batch/details/${value.id}`);
    // }
    // else if (hasRole('Reviewer')) {
    //   navigate(`/payment-batch/review/${value.id}`);
    // }
    // else if (hasRole('Approver')) {
    //   navigate(`/payment-batch/approve/${value.id}`);
    // }
  }

  return (
    <div
      //to={link}
      onClick={() => titleClickHandler(data)}
      className='card border border-2 border-gray-300 border-hover shadow cursor-pointer'
    >
      <div className='card-header border-0 pt-9'>
        <div className='card-toolbar'>
          <span className={`badge badge-light-${badgeColor} fw-bolder me-auto px-4 py-3`}>
            {status}
          </span>
        </div>
      </div>

      <div className='card-body p-9'>
        <div className='fs-3 fw-bolder text-gray-900'>{title}</div>

        <p className='text-gray-500 fw-bold fs-5 mt-1 mb-7'>{description}</p>

        <div className='d-flex flex-wrap mb-5'>
          <div className='border border-gray-300 border-dashed rounded min-w-125px py-3 px-4 me-7 mb-3'>
            <div className='fs-6 text-gray-800 fw-bolder'>{date}</div>
            <div className='fw-bold text-gray-500'>Initiated Date</div>
          </div>

          <div className='border border-gray-300 border-dashed rounded min-w-125px py-3 px-4 me-7 mb-3'>
            <div className='fs-6 text-gray-800 fw-bolder'>{budget}</div>
            <div className='fw-bold text-gray-500'>Amount</div>
          </div>

          <div className='border border-gray-300 border-dashed rounded min-w-125px py-3 px-4 mb-3'>
            <div className='fs-6 text-gray-800 fw-bolder'>{progress}</div>
            <div className='fw-bold text-gray-500'>Beneficiaries</div>
          </div>
        </div>

        <div
          className='h-4px w-100 bg-light mb-5'
          data-bs-toggle='tooltip'
          title='This project completed'
        >
          <div
            className={`bg-${statusColor} rounded h-4px`}
            role='progressbar'
            style={{ width: `${progress}%` }}
          ></div>
        </div>

      </div>
    </div>
  )
}

export { PaymentStatsCard }
