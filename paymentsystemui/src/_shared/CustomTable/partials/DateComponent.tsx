/* eslint-disable react-hooks/exhaustive-deps */

import { useEffect, useState } from 'react'
import { useDebounce, initialQueryState, KTIcon } from '../../../_metronic/helpers';
import { useQueryRequest } from '../../../app/modules/apps/user-management/users-list/core/QueryRequestProvider';

const DateComponent = (props: any) => {

  const { fromDate,toDate, setFromDate,setToDate, showDateBox } = props;
 

  return (
    <div className='card-title'>
      {/* begin::Search */}
      {showDateBox &&
        <div className='d-flex align-items-center position-relative my-1'>
          {/* <KTIcon iconName='magnifier' className='fs-1 position-absolute ms-6' /> */}
        
             <div className="d-flex align-items-end">
                      {/* From Date */}
                      <div className="me-3">
                        <label className="fs-6 mb-0">From</label>
                        <input
                          type="date"
                          className="form-control form-control-sm"
                          id="fromDate"
                          onChange={(e)=>setFromDate(e.target.value)}
                        />
                      </div>
          
                      {/* To Date */}
                      <div className="me-3">
                        <label className="fs-6 mb-0">To</label>
                        <input
                          type="date"
                          className="form-control form-control-sm"
                          id="toDate"
                          onChange={(e)=>setToDate(e.target.value)}
                        />
                      </div>
          
                      {/* Close Button */}
                      <button
                        type="button"
                        className="btn  btn-sm btn-primary"
                        id="kt_activities_close"
                      >
                        Search
                      </button>
                    </div>
        </div>
      }
      {/* end::Search */}
    </div>
  )
}

export { DateComponent }
