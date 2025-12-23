import { AgGridReact } from 'ag-grid-react';
import "ag-grid-community/styles/ag-grid.css";
import "./ag-theme-mycustomtheme.css";
import React, { useState, useEffect, useCallback } from 'react';

const TableBody = (props: any) => {
    // Keep clientRowData separate from the state
    const {
        rowData: clientRowData, // The prop for client-side mode
        colDefs,
        gridRef,
        height,
        pageSize = 10,
        serverSidePagination = false,
        fetchDataFunc
    } = props;

    // State for server-side mode only
    const [serverRowData, setServerRowData] = useState([]);
    const [currentPage, setCurrentPage] = useState(0);
    const [totalRows, setTotalRows] = useState(0);
    const [isLoading, setIsLoading] = useState(false);

    const _height = height ? height : 550;

    const memoizedFetchData = useCallback(async (page: number, size: number) => {
        if (!fetchDataFunc) return;

        setIsLoading(true);

        try {
            const data = await fetchDataFunc(page, size);
            if (data && data.rows && data.totalRows !== undefined) {
                setServerRowData(data.rows);
                setTotalRows(data.totalRows);
            } else {
                console.error("fetchDataFunc did not return the expected data structure.");
            }
        } catch (error) {
            console.error("Error fetching server-side data:", error);
        } finally {
            setIsLoading(false);
        }
    }, [fetchDataFunc]);

    useEffect(() => {
        if (serverSidePagination) {
            memoizedFetchData(currentPage, pageSize);
        }
        // No need to do anything for client-side here, as the rowData prop is used directly
    }, [currentPage, pageSize, serverSidePagination, memoizedFetchData]);

    const totalPages = Math.ceil(totalRows / pageSize);

    const handleNextPage = () => {
        if (currentPage < totalPages - 1) {
            setCurrentPage(currentPage + 1);
        }
    };

    const handlePrevPage = () => {
        if (currentPage > 0) {
            setCurrentPage(currentPage - 1);
        }
    };

    // Correctly choose which rowData to use based on the prop
    const finalRowData = serverSidePagination ? serverRowData : clientRowData;
    const finalPagination = !serverSidePagination;

    return (
        <div style={{ width: '100%', height: _height, overflowX: 'auto' }}>
            {isLoading && serverSidePagination ? (
                <div className="d-flex justify-content-center align-items-center" style={{ height: '100%' }}>
                    <div className="spinner-border text-primary" role="status">
                        <span className="visually-hidden">Loading...</span>
                    </div>
                </div>
            ) : (
                <AgGridReact
                    rowData={finalRowData}
                    columnDefs={colDefs}
                    pagination={finalPagination}
                    paginationPageSize={pageSize}
                    paginationPageSizeSelector={[10, 50, 100]}
                    rowSelection={{ mode: 'multiRow', enableClickSelection: false }}
                    ref={gridRef}
                />
            )}
            {serverSidePagination && (
                <div className="d-flex justify-content-end align-items-center mt-3">
                    <nav aria-label="Page navigation">
                        <ul className="pagination mb-0">
                            <li className={`page-item ${currentPage === 0 ? 'disabled' : ''}`}>
                                <button className="page-link" onClick={handlePrevPage} disabled={currentPage === 0}>
                                    Previous
                                </button>
                            </li>
                            <li className="page-item">
                                <span className="page-link">
                                    Page {currentPage + 1} of {totalPages}
                                </span>
                            </li>
                            <li className={`page-item ${currentPage >= totalPages - 1 ? 'disabled' : ''}`}>
                                <button className="page-link" onClick={handleNextPage} disabled={currentPage >= totalPages - 1}>
                                    Next
                                </button>
                            </li>
                        </ul>
                    </nav>
                </div>
            )}
        </div>
    );
};

export default TableBody;