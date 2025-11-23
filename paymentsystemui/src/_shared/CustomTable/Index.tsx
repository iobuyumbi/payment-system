import TableBody from './TableBody';
import TableHeader from './TableHeader';

const CustomTable = (props: any) => {
    const { rowData, colDefs, header, addBtnText, searchTerm, setSearchTerm, addBtnLink, showSearchBox, fromDate, toDate, setFromDate, setToDate, showDateBox,
        addBtnHandler, handleExport, showExport, gridRef, height, pageSize, serverSidePagination, fetchDataFunc } = props;

    return (
        <>
            {header &&
                <TableHeader
                    header={header}
                    addBtnText={addBtnText}
                    searchTerm={searchTerm}
                    setSearchTerm={setSearchTerm}
                    addBtnLink={addBtnLink}
                    showSearchBox={showSearchBox}
                    addBtnHandler={addBtnHandler}
                    showExport={showExport}
                    handleExport={handleExport}
                    fromDate={fromDate}
                    setFromDate={setFromDate}
                    toDate={toDate}
                    setToDate={setToDate}
                    showDateBox={showDateBox}
                />
            }
            <TableBody
                rowData={rowData}
                colDefs={colDefs}
                gridRef={gridRef}
                height={height}
                pageSize={pageSize}
                serverSidePagination={serverSidePagination}
                fetchDataFunc={fetchDataFunc}
            />
        </>

    )

}

export default CustomTable;