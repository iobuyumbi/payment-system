/* eslint-disable @typescript-eslint/no-unused-vars */
import { useListView } from "../../../app/modules/apps/user-management/users-list/core/ListViewProvider"
import { TableSearchComponent } from "../TableSearchComponent"
import { TableToolbar } from "../TableToolbar"

const ProjectsListHeader = () => {
  const { selected } = useListView()
  return (
    <div className='card-header border-0 pt-6'>
      <TableSearchComponent />
      {/* begin::Card toolbar */}
      <div className='card-toolbar'>
        {/* begin::Group actions */}
        <TableToolbar
          addLink='/projects/add'
          btnText='project'
        />
        {/* end::Group actions */}
      </div>
      {/* end::Card toolbar */}
    </div>
  )
}

export { ProjectsListHeader }
