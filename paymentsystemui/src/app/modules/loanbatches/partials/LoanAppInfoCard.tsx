import moment from 'moment'
import React, { useEffect, useState } from 'react'

const LoanAppInfoCard = ({ loanApp: loanBatch }: any) => {
  const [totalCounts, setTotalCounts] = useState<any>();
 
  console.log("loanBatch", loanBatch);
  useEffect(() => {
   
  }, [])

  return (
    <div className="bg-light p-5 rounded">
      <div className="d-flex flex-column me-10 justify-content-center">
        <div className="d-flex flex-row justify-content-between">
          <label className="mb-2 fs-5">Loan Product name</label>
          <h6>{loanBatch?.loanBatch.name}</h6>
        </div>
        <div className="d-flex flex-row justify-content-between">
          <label className="my-2 fs-5">Date</label>
          <h6>{moment(loanBatch?.loanBatch.initiationDate).format("DD-MM-YYYY")}</h6>
        </div>
        {/* <div className="d-flex flex-row justify-content-between">
          <label className="my-2  fs-5">Country</label>
          <h6>{loanBatch?.country?.countryName}</h6>
        </div> */}
        
        <div className="separator my-2"></div>
          <div>
            {/* <div className="d-flex flex-row justify-content-between">
              <label className="my-2 fs-5">Total application count</label>
              <h6>{totalCounts?.totalBeneficiaryCount}</h6>
            </div> */}
            <div className="d-flex flex-row justify-content-between">
              <label className="my-2 fs-5">Rows imported</label>
              <h6>{loanBatch?.importedRowCount}</h6>
            </div>

            <div className="d-flex flex-row justify-content-between">
              <label className="my-2 fs-5">
               Rows passed
              </label>
              <h6>{loanBatch?.successRowCount}</h6>
            </div>

            <div className="d-flex flex-row justify-content-between">
              <label className="my-2 fs-5">Rows failed</label>
              <h6>{loanBatch?.failedRowCount}</h6>
            </div>
          </div>
      </div>
    </div>
  )
}

export default LoanAppInfoCard
