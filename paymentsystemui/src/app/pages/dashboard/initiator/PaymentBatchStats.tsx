type Props = {
    className: string
    description: string
    color: string
    img: string
  }
  
  const PaymentBatchStats = ({className, description, color, img}: Props) => (
    <div
      className={`card bg-light shadow card-flush bgi-no-repeat bgi-size-contain bgi-position-x-end ${className}`}
    >
      <div className='card-header pt-5'>
        <div className='card-title d-flex flex-column'>
          <span className='fs-2hx fw-bold text-dark me-2 lh-1 ls-n2'>6</span>
  
          <span className='text-dark opacity-75 pt-1 fw-semibold fs-6'>{description}</span>
        </div>
      </div>
      <div className='card-body d-flex align-items-end pt-0'>
        <div className='d-flex align-items-center flex-column mt-3 w-100'>
          <div className='d-flex justify-content-between fw-bold fs-6 text-dark opacity-75 w-100 mt-auto mb-2'>
            <span>3 Pending</span>
            <span>50%</span>
          </div>
  
          <div className='h-8px mx-3 w-100 bg-white bg-opacity-50 rounded'>
            <div
              className='bg-success rounded h-8px'
              role='progressbar'
              style={{width: '50%'}}
              aria-valuenow={50}
              aria-valuemin={0}
              aria-valuemax={100}
            ></div>
          </div>
        </div>
      </div>
    </div>
  )
  export {PaymentBatchStats}
  