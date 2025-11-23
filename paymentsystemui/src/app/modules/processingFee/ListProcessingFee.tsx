import CustomTable from '../../../_shared/CustomTable/Index'
import { useState, useEffect } from 'react';
import { Content } from '../../../_metronic/layout/components/content';
import { PageLink, PageTitle } from '../../../_metronic/layout/core';
import { PageTitleWrapper } from '../../../_metronic/layout/components/toolbar/page-title';
import { KTCard, KTCardBody, KTIcon } from '../../../_metronic/helpers';
import { IConfirmModel } from '../../../_models/confirm-model';
import { ConfirmBox } from '../../../_shared/Modals/ConfirmBox';
import { useNavigate } from 'react-router-dom';
import { isAllowed } from '../../../_metronic/helpers/ApiUtil';
import { Error401 } from '../errors/components/Error401';
import ProcessingFeeService from '../../../services/ProcessingFeeService';

const processingFeeService = new ProcessingFeeService();
const breadCrumbs: Array<PageLink> = [
    {
        title: 'Dashboard',
        path: '/dashboard',
        isSeparator: false,
        isActive: true,
    },
    {
        title: '',
        path: '',
        isSeparator: true,
        isActive: true,
    },
];

export const ListProcessingFee: React.FC = (props: any) => {
    const navigate = useNavigate();

    // Row Data: The data to be displayed.
    const [rowData, setRowData] = useState<any>();
    const [showConfirmBox, setShowConfirmBox] = useState<boolean>(false);
    const [confirmModel, setConfirmModel] = useState<IConfirmModel>();
    const [loading, setLoading] = useState(false);

    const editButtonHandler = (value: any) => {
        navigate(`/processing-fee/edit/${value.id}`)
    }

    const openDeleteModal = (id: any) => {
        if (id) {
            const confirmModel: IConfirmModel = {
                title: 'Delete processing fee',
                btnText: 'Delete this processing fee?',
                deleteUrl: `api/MasterProcessingFee/${id}`,
                message: 'delete-processingfee',
            }

            setConfirmModel(confirmModel)
            setTimeout(() => {
                setShowConfirmBox(true)
            }, 500)
        }
    }

    const afterConfirm = (res: any) => {
        if (res) bindFee()
        setShowConfirmBox(false)
    }

    const CustomActionComponent = (props: any) => {
        return <div className="d-flex flex-row">
            {/* Edit */}
            {isAllowed("settings.loans.masterfee.edit") && <button className="btn btn-default mx-0 px-1" onClick={() => editButtonHandler(props.data)}>
                <KTIcon iconName="pencil" iconType="outline" className="fs-4 text-gray-600" />
            </button>}

            {/* Delete */}
            {isAllowed("settings.loans.masterfee.delete") && <button className="btn btn-default mx-0 px-1" onClick={() => openDeleteModal(props.data.id)}>
                <KTIcon iconName="trash" iconType="outline" className="text-danger fs-4" />
            </button>}
        </div>;
    };

   
    // Column Definitions: Defines the columns to be displayed.
    const [colDefs] = useState<any>([
        { field: "feeName", flex: 1, filter: true },
        { field: "feeType", flex: 1, filter: true },
        { field: "value", flex: 1, filter: true },
        { field: "action", flex: 1, cellRenderer: CustomActionComponent },
    ]);

    const bindFee = async () => {
        const response = await processingFeeService.getProcessingFee();
        setRowData(response);
    }

    useEffect(() => {
        document.title = `Manage Additional Fee - SDD`;
        bindFee();
    }, []);

    return (
        <Content>
            {isAllowed('settings.loans.masterfee.view') ? <> <PageTitleWrapper />
                <PageTitle breadcrumbs={breadCrumbs}>Master Additional Fee</PageTitle>
                <KTCard className='shadow my-3'>
                    <KTCardBody className='m-0 p-0'>
                        <CustomTable
                            rowData={rowData}
                            colDefs={colDefs}
                            header="processing fee"
                            addBtnText={isAllowed('settings.loans.masterfee.add') ? "Add Processing Fee" : ""}
                            addBtnLink={isAllowed('settings.loans.masterfee.add') ? "/processing-fee/add" : ""}
                            showExport={isAllowed('settings.loans.masterfee.view')}
                            
                        />
                    </KTCardBody>
                </KTCard> </> : <Error401 />}
            {showConfirmBox && <ConfirmBox confirmModel={confirmModel} afterConfirm={afterConfirm} loading={loading} />}
        </Content>
    )
}
