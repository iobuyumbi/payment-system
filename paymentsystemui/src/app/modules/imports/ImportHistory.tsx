import { useEffect, useState } from 'react';
import { Content } from '../../../_metronic/layout/components/content';
import { KTCard } from '../../../_metronic/helpers';
import ImportService from '../../../services/ImportService';
import moment from 'moment';
import CustomTable from '../../../_shared/CustomTable/Index';
import { Link } from 'react-router-dom';
import { PageLink, PageTitle } from '../../../_metronic/layout/core';
import { PageTitleWrapper } from '../../../_metronic/layout/components/toolbar/page-title';

const importService = new ImportService();
const breadCrumbs: Array<PageLink> = [
    {
        title: "Dashboard",
        path: "/dashboard",
        isSeparator: false,
        isActive: true,
    },
    {
        title: "",
        path: "",
        isSeparator: true,
        isActive: true,
    },
];

const ImportHistory = () => {
    const [rowData, setRowData] = useState([]);
    const [searchTerm, setSearchTerm] = useState<string>('');

    const StatusComponent = (props: any) => {
        return <div className="py-1">
            {props.data.excelImportStatusID === 1 && <div className='badge badge-light fs-7 fw-normal'>Started</div>}
            {props.data.excelImportStatusID === 2 && <div className='badge badge-light-warning fs-7 fw-normal'>Processing</div>}
            {props.data.excelImportStatusID === 3 && <div className='badge badge-light-success fs-7 fw-normal'>Completed</div>}
            {props.data.excelImportStatusID === 4 && <div className='badge badge-light-danger fs-7 fw-normal'>Failed</div>}
        </div>;
    };

    const MainLinkComponent = (props: any) => {
        return (
            <div className="d-flex flex-column justify-content-start">
                <Link
                    className="link-primary cursor"
                    to={`/imports/detail/${props.data.id}/${props.data.filename}`}
                    style={{ display: 'block', fontWeight: 500 }}
                >
                    {props.data.filename}
                </Link>
                {props.data.blobFolder &&
                    <a href={props.data.blobFolder} className="text-sm text-gray-600 mb-0 mt-0">
                        {props.data.blobFolder ?? '-'}
                    </a>
                }
            </div>
        );
    };


    // Column Definitions: Defines the columns to be displayed.
    const [colDefs] = useState<any>([
        {
            field: "filename",
            headerName: 'File Name/Cloud Storage URL',
            filter: true,
            flex: 4,
            cellRenderer: MainLinkComponent,
            autoHeight: true,
            cellStyle: { paddingTop: '3px', paddingBottom: '3px', }
        },
        {
            field: "importedDateTime", flex: 1, valueFormatter: function (params: any) {
                return moment(params.value).format('DD-MM-yyyy hh:mm');
            },
        },
        {
            field: "endDateTime", flex: 1, filter: true, valueFormatter: function (params: any) {
                return params.value ? moment(params.value).format('DD-MM-yyyy hh:mm') : null;
            },
        },
        { headerName: "Status", flex: 1, filter: true, cellRenderer: StatusComponent },
    ]);

    const bindImportHistory = async () => {
        if (searchTerm.length === 0 || searchTerm.length > 2) {
            const response = await importService.getImportHistory(searchTerm);
            setRowData(response);
        }
    }

    useEffect(() => {
        bindImportHistory();
    }, [searchTerm]);

    return (
        <Content>
            <PageTitleWrapper />
            <PageTitle breadcrumbs={breadCrumbs}>Import History</PageTitle>
            <KTCard className='my-3'>
                <CustomTable
                    rowData={rowData}
                    colDefs={colDefs}
                    header="import"
                    addBtnText={"New import"}
                    importBtnText={"Import contacts"}
                    addBtnLink={"/imports"}
                    showImportBtn={false}
                />
            </KTCard>
        </Content>
    )

}

export default ImportHistory;