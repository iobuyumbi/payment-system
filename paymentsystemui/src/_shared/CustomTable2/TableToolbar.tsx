/* eslint-disable @typescript-eslint/no-unused-vars */
import { Link } from "react-router-dom";
import { KTIcon } from "../../_metronic/helpers"
import { useListView } from "../../app/modules/apps/user-management/users-list/core/ListViewProvider"
import { TableFilter } from "./TableFilter"
import { ProjectFilter } from "./projects/ProjectFilter";

const TableToolbar = (props: any) => {
    const { headerTitle, addLink, btnText } = props;

    const { setItemIdForUpdate } = useListView()
    const openAddUserModal = () => {
        setItemIdForUpdate(null)
    }

    return (
        <div className='d-flex justify-content-end' data-kt-user-table-toolbar='base'>
            {btnText === 'project' && <ProjectFilter />}

            {/* begin::Export */}
            <button type='button' className='btn btn-light me-3'>
                <KTIcon iconName='exit-up' className='fs-2' />
                Export
            </button>
            {/* end::Export */}

            {/* begin::Add user */}
            {headerTitle === "loans" &&
                <button className="btn btn-theme"  onClick={()=>alert('app')}>Import loan applications</button>
            }
            {headerTitle !== "loans" &&
                <Link to={addLink} type='button' className='btn btn-custom btn-theme'>
                    <KTIcon iconName='plus' className='fs-2' />
                    Add {btnText}
                </Link>
            }
            {/* end::Add user */}
        </div>
    )
}

export { TableToolbar }
