import React from 'react'
import { Content } from '../../../../_metronic/layout/components/content'
import { Link } from 'react-router-dom'
import { KTIcon } from '../../../../_metronic/helpers'
import { Item3 } from '../../../../_metronic/partials/content/activity/Item3'

const FarmerDocs = () => {
  return (
    <Content>
      <div className='card shadow-none rounded-0'>
        <div className='card-header' id='kt_activities_header'>
          <h3 className='card-title fw-bolder text-gray-900'>Documents</h3>

          <div className='card-toolbar'>
            <a
              href='#'
              className='btn btn-sm btn-primary me-3'
              data-bs-toggle='modal'
              data-bs-target='#kt_modal_offer_a_deal'
            >
              <KTIcon iconName='plus' className='fs-6' iconType='outline' /> Add new document
            </a>
          </div>
        </div>
        <div className='card-body position-relative' id='kt_activities_body'>
          <Item3 />
        </div>

      </div>
    </Content>
  )
}

export default FarmerDocs
