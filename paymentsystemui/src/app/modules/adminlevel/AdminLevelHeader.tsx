
import { FC } from 'react'
import {Link} from 'react-router-dom'
import {useLocation} from 'react-router'
import { ToolbarWrapper } from '../../../_metronic/layout/components/toolbar'
import { Content } from '../../../_metronic/layout/components/content'
import { isAllowed } from '../../../_metronic/helpers/ApiUtil'

const AdminLevelHeader: FC = () => {
  const location = useLocation()

  return (
    <>
      {isAllowed("settings.administrative.add") ? ( <><ToolbarWrapper />
      <Content>
        <div className='card mb-5 mb-xl-10'>
          <div className='card-body pt-9 pb-0'>
            <div className='d-flex overflow-auto '>
              <ul className='nav nav-stretch nav-line-tabs nav-line-tabs-2x border-transparent fs-5 fw-bolder flex-nowrap'>
                <li className='nav-item'>
                  <Link
                    className={
                      `nav-link text-active-primary me-6 ` +
                      (location.pathname === '/adminlevel/adminlevel1' && 'active')
                    }
                    to='/adminlevel/adminlevel1'
                  >
                    Admin Level 1
                  </Link>
                </li>
                <li className='nav-item'>
                  <Link
                    className={
                      `nav-link text-active-primary me-6 ` +
                      (location.pathname === '/adminlevel/adminlevel2' && 'active')
                    }
                    to='/adminlevel/adminlevel2'
                  >
                   Admin Level 2
                  </Link>
                </li>
                <li className='nav-item'>
                  <Link
                    className={
                      `nav-link text-active-primary me-6 ` +
                      (location.pathname === '/adminlevel/adminlevel3' && 'active')
                    }
                    to='/adminlevel/adminlevel3'
                  >
                    Admin Level 3
                  </Link>
                </li>
                {/* <li className='nav-item'>
                  <Link
                    className={
                      `nav-link text-active-primary me-6 ` +
                      (location.pathname === '/adminlevel/adminlevel4' && 'active')
                    }
                    to='/adminlevel/adminlevel4'
                  >
                    Admin Level 4
                  </Link>
                </li> */}
              </ul>
            </div>
          </div>
        </div>
      </Content></>) : ""}
    </>
  )
}

export {AdminLevelHeader}
