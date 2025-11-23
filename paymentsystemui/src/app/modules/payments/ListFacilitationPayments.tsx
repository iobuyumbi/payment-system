import CustomTable from '../../../_shared/CustomTable/Index'
import { useState, useEffect } from 'react';
import { PageLink } from '../../../_metronic/layout/core';
import { KTCard, KTCardBody, KTIcon } from '../../../_metronic/helpers';
import { IConfirmModel } from '../../../_models/confirm-model';
import { ConfirmBox } from '../../../_shared/Modals/ConfirmBox';
import { useDispatch } from 'react-redux';
import { useNavigate } from 'react-router-dom';
import { isAllowed } from '../../../_metronic/helpers/ApiUtil';
import { Error401 } from '../errors/components/Error401';
import moment from 'moment';
import { addToPaymentBatch } from '../../../_features/payment-batch/paymentBatchSlice';
import FacilitationPaymentService from '../../../services/FacilitationPaymentService';

const facilitationPaymentService = new FacilitationPaymentService();

export const ListFacilitationPayments: React.FC<any> = ({ batchId }: any) => {

    const navigate = useNavigate();
    const dispatch = useDispatch();

    // Row Data: The data to be displayed.
    const [rowData, setRowData] = useState<any>();
    const [searchTerm, setSearchTerm] = useState<string>('');
    const [showConfirmBox, setShowConfirmBox] = useState<boolean>(false);
    const [confirmModel, setConfirmModel] = useState<IConfirmModel>();
    const [loading, setLoading] = useState(false);

    const editButtonHandler = (value: any) => {
        dispatch(addToPaymentBatch(value));
        navigate(`/payment-batch/edit/${value.id}`)
    }

    const titleClickHandler = (value: any) => {
        dispatch(addToPaymentBatch(value));
        navigate(`/payment-batch-detail/farmers`)
    }

    const openDeleteModal = (id: any) => {
        if (id) {
            const confirmModel: IConfirmModel = {
                title: 'Delete payment-batch',
                btnText: 'Delete this payment-batch?',
                deleteUrl: `api/PaymentBatch/${id}`,
                message: 'delete-payment-batch',
            }

            setConfirmModel(confirmModel)
            setTimeout(() => {
                setShowConfirmBox(true)
            }, 500)
        }
    }

    const afterConfirm = (res: any) => {
        if (res) bindFacilitationPayments()
        setShowConfirmBox(false)
    }

    const paymentTitleComponent = (props: any) => {
        return <div className="d-flex flex-row">
            <a className="link-primary cursor" onClick={() => titleClickHandler(props.data)}>
                {props.data.batchName}
            </a>
        </div>;
    };

    const CustomActionComponent = (props: any) => {
        return <div className="d-flex flex-row">
            <button className="btn btn-default mx-0 px-1" onClick={() => editButtonHandler(props.data)}>
                <KTIcon iconName="pencil" iconType="outline" className="fs-4 text-gray-600" />
            </button>
            <button className="btn btn-default mx-0 px-1" onClick={() => openDeleteModal(props.data.id)}>
                <KTIcon iconName="trash" iconType="outline" className="text-danger fs-4" />
            </button>
        </div>;
    };

    const DateComponent = (props: any) => {
        return <div className="d-flex flex-row">
            {moment(props.data.dateCreated).format('YYYY-MM-DD hh:mm A')}
        </div>;
    };

    const StatusComponent = (props: any) => {

        return (
            <div className="py-1">
                {props.data.isPaymentComplete === true && (
                    <div className="badge fs-7 badge-light-success  fw-normal">Yes</div>
                )}
                {props.data.isPaymentComplete === false && (
                    <div className="badge fs-7 badge-light-danger  fw-normal">No</div>
                )}
            </div>
        );
    };

    const [colDefs, setColDefs] = useState<any>([
        { field: "fullName", flex: 1, filter: true },
        { field: "phoneNo", flex: 1, filter: true },
        { field: "nationalId", flex: 1, filter: true },
        { field: "dateCreated", flex: 1, filter: true, cellRenderer: DateComponent },
        { field: "netDisbursementAmount", flex: 1, headerName: 'Amount', filter: true },
        { field: "isPaymentComplete", flex: 1, cellRenderer: StatusComponent },
        { field: "remarks", flex: 2, headerName: "Remarks" },
    ]);

    const bindFacilitationPayments = async () => {
        if (searchTerm.length === 0 || searchTerm.length > 2) {
            const data: any = {
                pageNumber: 1,
                pageSize: 10000,
                batchId: batchId
            };
            const response = await facilitationPaymentService.getFacilitationPayments(data);
            setRowData(response);
        }
    }

    useEffect(() => {
        bindFacilitationPayments();
    }, [searchTerm]);

    return (
        <div>
            {isAllowed('payments.batch.history') ? <>
                <KTCard className='shadow my-3'>
                    <KTCardBody className='m-0 p-0'>
                        <CustomTable
                            rowData={rowData}
                            colDefs={colDefs}
                            header=""
                            addBtnText={""}
                            searchTerm={searchTerm}
                            setSearchTerm={setSearchTerm}
                            addBtnLink={""}

                        />
                    </KTCardBody>
                </KTCard> </> : <Error401 />}
            {showConfirmBox && <ConfirmBox confirmModel={confirmModel} afterConfirm={afterConfirm} loading={loading} />}
        </div>
    )
}
