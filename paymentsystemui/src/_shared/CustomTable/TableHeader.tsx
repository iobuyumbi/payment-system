import { DateComponent } from './partials/DateComponent';
import { SearchComponent } from './partials/SearchComponent';
import { TableToolbar } from './partials/TableToolbar';

export default function TableHeader(props: any) {
  const { header, addBtnText, searchTerm, setSearchTerm, addBtnLink, showSearchBox, addBtnHandler, handleExport, showExport,fromDate,setFromDate,toDate ,showDateBox,setToDate} = props;

  return (
    <div className='card-header py-5'>
     {showSearchBox && <SearchComponent
        searchTerm={searchTerm}
        setSearchTerm={setSearchTerm}
        showSearchBox={showSearchBox}
      />}
      <DateComponent
        fromDate={fromDate}
        setFromDate={setFromDate}
        toDate={toDate}
        setToDate={setToDate}
        showDateBox={showDateBox}
      />
      {/* begin::Card toolbar */}
      <div className='card-toolbar'>
        {/* begin::Group actions */}
        <TableToolbar
          addBtnText={addBtnText}
          addBtnLink={addBtnLink}
          addBtnHandler={addBtnHandler}
          showExport={showExport}
          handleExport={handleExport}
        />
        {/* end::Group actions */}
      </div>
      {/* end::Card toolbar */}
    </div>
  )
}
