import { useEffect, useRef, useState } from 'react'
import { PageLink, PageTitle } from '../../../_metronic/layout/core';
import { KTCard } from '../../../_metronic/helpers';
import { Content } from '../../../_metronic/layout/components/content';
import { Link } from 'react-router-dom';
import { PageTitleWrapper } from '../../../_metronic/layout/components/toolbar/page-title';
import EmailService from '../../../services/EmailService';
import CustomTable from '../../../_shared/CustomTable/Index';
import { isAllowed } from '../../../_metronic/helpers/ApiUtil';

const breadCrumbs: Array<PageLink> = [
    {
        title: 'Dashboard',
        path: '/dashboard',
        isSeparator: false,
        isActive: false,
    },
    {
        title: '',
        path: '',
        isSeparator: true,
        isActive: true,
    }
]

const emailService = new EmailService();

const ListTemplates = () => {
    // Row Data: The data to be displayed.
    const [rowData, setRowData] = useState<any>([]);

    const TitleLinkComponent = (props: any) => {
        return <Link to={`/email/templates/edit/${props.data.id}`} className='link-primary'>{props.data.name}</Link>;
    };

    // Column Definitions: Defines the columns to be displayed.
    const [colDefs, setColDefs] = useState<any>([
        { field: "name", headerName: "Name", flex: 1.5, filter: true, cellRenderer: TitleLinkComponent },
        { field: "subject", flex: 2, filter: true },
    ]);

    const bindTemplates = (async () => {
        const response = await emailService.getTemplates("");
        setRowData(response);
    });

    const gridRef = useRef<any>();

    const getSelectedRows = () => {
        const selectedRows = gridRef?.current.api.getSelectedRows();
    };

    useEffect(() => {
        document.title = `Manage Email Templates - SDD`;
        bindTemplates();
    }, []);

    return (
        <Content>
            <PageTitleWrapper />
            <PageTitle breadcrumbs={breadCrumbs}>Manage Email Templates</PageTitle>
            <KTCard className='my-5'>
                <CustomTable
                    rowData={rowData}
                    colDefs={colDefs}
                    gridRef={gridRef}
                />
            </KTCard>
        </Content>
    )
}

export default ListTemplates
