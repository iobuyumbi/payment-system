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

import MasterLoanTermService from '../../../services/MasterLoanTermsService';

const masterLoanTermService = new MasterLoanTermService();
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

export const ListMasterLoanTerms: React.FC = (props: any) => {
    const navigate = useNavigate();

    // Row Data: The data to be displayed.
    const [rowData, setRowData] = useState<any>();
    const [showConfirmBox, setShowConfirmBox] = useState<boolean>(false);
    const [confirmModel, setConfirmModel] = useState<IConfirmModel>();
    const [loading, setLoading] = useState(false);

    const editButtonHandler = (value: any) => {
        navigate(`/master-loan-terms/edit/${value.id}`)
    }

    const openDeleteModal = (id: any) => {
        if (id) {
            const confirmModel: IConfirmModel = {
                title: 'Delete loan terms',
                btnText: 'Delete these loan terms?',
                deleteUrl: `api/MasterLoanTerm/${id}`,
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
            {isAllowed("settings.loans.terms.edit") && <button className="btn btn-default mx-0 px-1" onClick={() => editButtonHandler(props.data)}>
                <KTIcon iconName="pencil" iconType="outline" className="fs-4 text-gray-600" />
            </button>}

            {/* Delete */}
            {isAllowed("settings.loans.terms.delete") && <button className="btn btn-default mx-0 px-1" onClick={() => openDeleteModal(props.data.id)}>
                <KTIcon iconName="trash" iconType="outline" className="text-danger fs-4" />
            </button>}
        </div>;
    };

    // Column Definitions: Defines the columns to be displayed.
    const [colDefs] = useState<any>([
        { field: "descriptiveName", flex: 1, filter: true },
        { field: "interestRateType", flex: 1, filter: true },
        { field: "interestRate", flex: 1, filter: true },
        { field: "tenure", flex: 1, filter: true },
        { field: "action", flex: 1, cellRenderer: CustomActionComponent },
    ]);

    const bindFee = async () => {
        const response = await masterLoanTermService.getMasterLoanTermData({});
        console.log(response)
        setRowData(response);
    }

    useEffect(() => {
        document.title = `Manage master terms`;
        bindFee();
    }, []);

    return (
        <Content>
            {isAllowed('settings.loans.terms.view') ? <> <PageTitleWrapper />
                <PageTitle breadcrumbs={breadCrumbs}>Master loan terms</PageTitle>
                <KTCard className='shadow my-3'>
                    <KTCardBody className='m-0 p-0'>
                        <CustomTable
                            rowData={rowData}
                            colDefs={colDefs}
                            header="master terms"
                            addBtnText={isAllowed('settings.loans.terms.add') ? "Add master terms" : ""}
                            addBtnLink={isAllowed('settings.loans.terms.add') ? "/master-loan-terms/add" : ""}
                            showExport={isAllowed('settings.loans.terms.view')}
                            
                        />
                    </KTCardBody>
                </KTCard> </> : <Error401 />}
            {showConfirmBox && <ConfirmBox confirmModel={confirmModel} afterConfirm={afterConfirm} loading={loading} />}
        </Content>
    )
}
