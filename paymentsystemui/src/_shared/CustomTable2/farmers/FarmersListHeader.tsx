import { useListView } from "../../../app/modules/apps/user-management/users-list/core/ListViewProvider"
import { TableSearchComponent } from "../TableSearchComponent"
import { TableToolbar } from "../TableToolbar"

const FarmersListHeader = () => {
  const { selected } = useListView()
  return (
    <div className='card-header border-0 pt-6'>
      <TableSearchComponent />
      {/* begin::Card toolbar */}
      <div className='card-toolbar'>
        {/* begin::Group actions */}
        <TableToolbar
          addLink='/farmers/add'
          btnText='Farmer'
        />
        {/* end::Group actions */}
      </div>
      {/* end::Card toolbar */}
    </div>
  )
}

export { FarmersListHeader }
