import React, { useEffect, useState, Fragment } from 'react';
import { Link, useNavigate } from "react-router-dom";
import { Table } from 'react-bootstrap';
import { PageLink, PageTitle } from '../../../_metronic/layout/core';
import { AUTH_LOCAL_STORAGE_KEY } from '../auth';
import clsx from 'clsx';

const profileBreadCrumbs: Array<PageLink> = [
    {
        title: 'Import History',
        path: '/import-data',
        isSeparator: false,
        isActive: false,
    },
]

export function ImportHistory() {
    const navigate = useNavigate();
    const [data, setData] = React.useState([]);
    const [currentTableData, setCurrentTableData] = React.useState([]);
    const [currentPage, setCurrentPage] = useState(1);
    const [totalRows, setTotalRows] = React.useState(0);
    const [isLoading, setIsLoading] = useState(false);
    const [showConfirm, setShowConfirm] = useState<boolean>(false);
    const LOADINGSTAGES = {
        IDLE: 0,
        LOADING: 1,
        POSITIVE: 3,
        NEGATIVE: 4,
        ERROR: 5,
        EMPTY: 6
    }
    const [loadingStage, setLoadingStage] = useState<any>(LOADINGSTAGES.IDLE);
    const [status, setStatus] = React.useState<any>();
    const [keyword, setKeyword] = React.useState<any>();
    const [counterData, setCounterData] = React.useState([]);
    const dataFromStorage = localStorage.getItem(AUTH_LOCAL_STORAGE_KEY);
    const keys = JSON.parse(dataFromStorage || "")
    const initialValues = {
        pageNumber: 1,
        pageSize: 100000,
        filter: "",
        orgId: keys.orgId,
        parentId: keys.orgId,
    }
    const searchParams = {
        fileName: [],
        data: [],
        moduleId: 4
    }
    
    const redirectToImport = () => {
        navigate('/import/all');
    }

    useEffect(() => {
        
    }, []);

    return (<>
        <PageTitle breadcrumbs={profileBreadCrumbs}>Import History </PageTitle>
        <div>

            <div className='row'>
                <div className="col-xl-12 col-md-12">
                    <div className="card">
                        <div className="card-body">
                            {loadingStage === LOADINGSTAGES.POSITIVE ?
                                <div className="table-responsive">
                                    <div className="display mb-4 dataTablesCard customer-list-table">
                                        <Table className='table align-middle table-row-dashed fs-6 gy-5 dataTable no-footer'>
                                            <thead>
                                                <tr className='text-start text-muted fw-bolder fs-7 text-uppercase gs-0'>

                                                    <th>File Name/Cloud Storage URL</th>
                                                    <th>Import Date</th>
                                                    <td>Import Time</td>
                                                    <td>Status</td>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                {currentTableData.map((item: any, index: any) =>
                                                    <tr key={index}>
                                                        <td>
                                                            {item.filename}
                                                            <p className='text-sm text-gray-600'>
                                                                {item.blobFolder ?? '-'}
                                                            </p>
                                                        </td>
                                                        <td>
                                                            {item.importDate}
                                                        </td>
                                                        <td>
                                                            {item.importTime}
                                                        </td>
                                                        <td>
                                                            <div
                                                                className={clsx(
                                                                    'badge',
                                                                    item.status === 'Completed' ? 'badge-light-success' :
                                                                        item.status === 'Failed' ? 'badge-light-danger' :
                                                                            "badge-light"
                                                                )}>
                                                                {item.status}
                                                            </div>

                                                        </td>

                                                    </tr>)}
                                            </tbody>
                                        </Table>
                                        
                                    </div>
                                </div>
                                : <>
                                    <div className="row mt-10">
                                        <div className="col-md-4 offset-5 text-center">
                                           
                                        </div>
                                    </div>
                                </>}
                        </div>
                    </div>
                </div>
            </div>
        </div>

    </>)
}