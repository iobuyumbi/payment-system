import React, { useState } from 'react'
import { KTIcon } from '../../_metronic/helpers'
import { TableSearchComponent } from './TableSearchComponent'
import { TableToolbar } from './TableToolbar'

export const TableHeader: React.FC = (props: any) => {
  const [searchTerm, setSearchTerm] = useState<string>('')
  const { headerTitle, addLink, btnText } = props;
  return (
    <div className='card-header border-0 pt-6'>
      <TableSearchComponent />
      {/* begin::Card toolbar */}
      <div className='card-toolbar'>
        {/* begin::Group actions */}
        <TableToolbar
        headerTitle={headerTitle}
          addLink={addLink}
          btnText={btnText}
        />
        {/* end::Group actions */}
      </div>
      {/* end::Card toolbar */}
    </div>
  )
}
