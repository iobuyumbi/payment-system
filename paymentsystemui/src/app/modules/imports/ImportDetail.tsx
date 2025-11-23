import { useEffect, useState } from 'react';
import { Content } from '../../../_metronic/layout/components/content';
import { KTCard } from '../../../_metronic/helpers';
import ImportService from '../../../services/ImportService';
import CustomTable from '../../../_shared/CustomTable/Index';
import { Link, useParams } from 'react-router-dom';
import { PageTitleWrapper } from '../../../_metronic/layout/components/toolbar/page-title';
import { PageLink, PageTitle } from '../../../_metronic/layout/core';

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
    {
        title: "Import History",
        path: "/imports/history",
        isSeparator: false,
        isActive: true,
    },
];

const ImportDetail = () => {
    // Row Data: The data to be displayed.
    const [rowData, setRowData] = useState<any[]>([]);
    const { id, fileName } = useParams();

    const StatusComponent = (props: any) => {
        return <div className="py-1">
            {props.data.isSuccess === true && <div className='badge badge-success fs-7 fw-normal'>success</div>}
            {props.data.isSuccess === false && <div className='badge badge-light-danger fs-7 fw-normal'>fail</div>}
        </div>;
    };


    // Column Definitions: Defines the columns to be displayed.
    const [colDefs, setColDefs] = useState<any>([
        { field: "tabName", flex: 1, headerName: "Tab" },
        { field: "rowNumber", flex: 0.5, headerName: "Row Number" },
        { field: "isSuccess", flex: 1, headerName: "Status", cellRenderer: StatusComponent },
        { field: "remarks", flex: 3, headerName: "Remarks" },
    ]);

    const bindImportDetail = async () => {
        const response = await importService.getImportDetail(id);
        setRowData(response);
    }

    useEffect(() => {
        bindImportDetail();
    }, []);

    return (
        <Content>
            <PageTitleWrapper />
            <PageTitle breadcrumbs={breadCrumbs}>Import Details</PageTitle>

            <div className="rounded  border border-dashed p-6 my-3 bg-light-warning border-warning">
                <span className="text-gray-700 d-block">Showing import details of:</span>
                <h4 className="text-gray-900 text-hover-primary fw-bold fs-6 mt-3">{fileName}</h4>
            </div>
            <KTCard>
                <CustomTable
                    rowData={rowData}
                    colDefs={colDefs}
                />
            </KTCard>
        </Content>
    )

}

export default ImportDetail;