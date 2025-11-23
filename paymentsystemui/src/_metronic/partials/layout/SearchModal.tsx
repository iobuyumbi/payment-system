
import { FC } from 'react'
import {Modal} from 'react-bootstrap'
import {KTIcon, toAbsoluteUrl} from '../../helpers'
// import {ListsWidget4, ListsWidget5} from '../widgets'

type Props = {
  show: boolean
  handleClose: () => void
}

const SearchModal: FC<Props> = ({show, handleClose}) => {
  return (
    <Modal
      className='bg-body'
      id='kt_header_search_modal'
      aria-hidden='true'
      dialogClassName='modal-fullscreen h-auto'
      show={show}
    >
      <div className='modal-content shadow-none'>
        <div className='container-xxl w-lg-800px'>
          <div className='modal-header d-flex justify-content-end border-0'>
            {/* begin::Close */}
            <div className='btn btn-icon btn-sm btn-light-primary ms-2' onClick={handleClose}>
              <KTIcon className='fs-2' iconName='cross' />
            </div>
            {/* end::Close */}
          </div>
          <div className='modal-body'>
            {/* begin::Search */}
            <form className='pb-10'>
              <input
                autoFocus
                type='text'
                className='form-control bg-transparent border-0 fs-4x text-center fw-normal'
                name='query'
                placeholder='Search...'
              />
            </form>
            {/* end::Search */}

            {/* begin::Shop Goods */}
            <div className='py-10'>
              <h3 className='fw-bolder mb-8'>Shop Goods</h3>

             
            </div>
            {/* end::Shop Goods */}

            {/* begin::Framework Users */}
            <div>
              <h3 className='text-gray-900 fw-bolder fs-1 mb-6'>Framework Users</h3>
              {/*<ListsWidget4 className='bg-transparent mb-5 shadow-none' innerPadding='px-0' />*/}
            </div>
            {/* end::Framework Users */}

            {/* begin::Tutorials */}
            <div className='pb-10'>
              <h3 className='text-gray-900 fw-bolder fs-1 mb-6'>Tutorials</h3>
              {/*<ListsWidget5 className='mb-5 shadow-none' innerPadding='px-0' />*/}
            </div>
            {/* end::Tutorials */}
          </div>
        </div>
      </div>
    </Modal>
  )
}

export {SearchModal}
