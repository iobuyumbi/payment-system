import { Content } from '../../../../_metronic/layout/components/content'
import { Link } from 'react-router-dom'
import { KTIcon } from '../../../../_metronic/helpers'
import FarmerService from '../../../../services/FarmerService';
import { useEffect, useState } from 'react';



const FarmerOverview = (props: any) => {
  const { farmer, commaSeparated } = props;
  

  return (
    <Content>
      <div className='card shadow-none rounded-0'>
        <div className='card-header' id='kt_activities_header'>
          <h3 className='card-title fw-bolder text-gray-900'>Overview</h3>

          <div className='card-toolbar'>
            <Link to={`/farmers/edit/${farmer?.id}`} className='btn btn-sm btn-primary me-3'>
              <KTIcon iconName='pencil' className='fs-6' iconType='outline' /> Edit
            </Link>
          </div>
        </div>
        <div className='card-body position-relative' id='kt_activities_body'>
          <div className='row mb-7'>
            <label className='col-lg-4 fw-bold text-muted'>Full Name</label>

            <div className='col-lg-8'>
              <span className='fw-bolder fs-6 text-gray-900'>{farmer?.firstName} {farmer?.otherNames}</span>
            </div>
          </div>
          <div className='row mb-7'>
            <label className='col-lg-4 fw-bold text-muted'>Beneficiary ID</label>

            <div className='col-lg-8'>
              <span className='fw-bolder fs-6 text-gray-900'>{farmer?.beneficiaryId} </span>
            </div>
          </div>
          <div className='row mb-7'>
            <label className='col-lg-4 fw-bold text-muted'>Address</label>

            <div className='col-lg-8 fv-row'>
              <span className='fw-bold fs-6'>
                {farmer?.village} {farmer?.adminLevel3?.wardName} {farmer?.adminLevel2?.subCountyName} {farmer?.adminLevel1?.countyName} - {farmer?.country?.countryName}
              </span>
            </div>
          </div>

          <div className='row mb-7'>
            <label className='col-lg-4 fw-bold text-muted'>
              Contact Phone
              <i
                className='fas fa-exclamation-circle ms-1 fs-7'
                data-bs-toggle='tooltip'
                title='Phone number must be active'
              ></i>
            </label>

            <div className='col-lg-8 d-flex align-items-center'>
              <span className='fw-bolder fs-6 me-2'> {farmer?.mobile}</span>

              <span className='badge badge-success'>Verified</span>
            </div>
          </div>

          <div className='row mb-7'>
            <label className='col-lg-4 fw-bold text-muted'>Email</label>

            <div className='col-lg-8'>
              <a href='#' className='fw-bold fs-6 text-gray-900 text-hover-primary'>
                {farmer?.email}
              </a>
            </div>
          </div>

          <div className='row mb-7'>
            <label className='col-lg-4 fw-bold text-muted'>Cooperative</label>

            <div className='col-lg-8'>
              <span className='fw-bolder fs-6 text-gray-900'>
                {
                   commaSeparated
                }
              </span>
            </div>
          </div>
        </div>

      </div>
    </Content>
  )
}

export default FarmerOverview
