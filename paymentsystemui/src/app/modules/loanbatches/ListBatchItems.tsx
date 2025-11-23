/* eslint-disable @typescript-eslint/no-explicit-any */
/* eslint-disable @typescript-eslint/no-unused-vars */
import CustomTable from "../../../_shared/CustomTable/Index"
import { useState } from "react";
import { Content } from "../../../_metronic/layout/components/content";
import { useEffect } from "react";
import { IConfirmModel } from "../../../_models/confirm-model";
import { KTIcon } from "../../../_metronic/helpers";
import { ConfirmBox } from "../../../_shared/Modals/ConfirmBox";
import { KTCard } from "../../../_metronic/helpers";
import LoanBatchService from "../../../services/LoanBatchService";
import { useDispatch, useSelector } from "react-redux";
import LoanItemsModal from "./partials/LoanItemsModal";
import { addToLoanItems } from "../../../_features/loanitem/loanItemSlice";
import { isAllowed } from "../../../_metronic/helpers/ApiUtil";
import { Error401 } from "../errors/components/Error401";

const loanBatchService = new LoanBatchService();

const ListBatchItems = (props: any) => {
    const { id, setItemCount } = props

    const [rowData, setRowData] = useState<any>();
    const [searchTerm, setSearchTerm] = useState<string>("");
    const [confirmModel, setConfirmModel] = useState<IConfirmModel>();
    const [showConfirmBox, setShowConfirmBox] = useState<boolean>(false);
    const [showItemBox, setShowItemBox] = useState<boolean>(false);
    const [loading, setLoading] = useState(false);
    const [data, setData] = useState<any>();
    const [selectedBatch, setSelectedBatch] = useState();
    const loanBatch = useSelector((state: any) =>
        state?.loanBatches,
    )
    const dispatch = useDispatch();

    const editButtonHandler = (value: any) => {
        dispatch(addToLoanItems(value));
        setData(value)
        setShowItemBox(true);
    };

    const loanItemClickHandler = (value: any) => {
        setSelectedBatch(value);
        setShowItemBox(true);
    };

    const afterConfirm = (res: any) => {
        bindBatchItems();
        setShowConfirmBox(false);
        setShowItemBox(false);
    };

    const openDeleteModal = (id: any) => {
        if (id) {
            const confirmModel: IConfirmModel = {
                title: "Delete Loan Product Item",
                btnText: "Delete this Loan Product item?",
                deleteUrl: `api/loanBatch/DeleteBatchItems/${id}`,
                message: "delete-loanBatchItem",
            };

            setConfirmModel(confirmModel);
            setTimeout(() => {
                setShowConfirmBox(true);
            }, 500);
        }
    };

    const CustomActionComponent = (props: any) => {
        return (
            <div className="d-flex flex-row">
                {isAllowed('loans.batch.items.edit') &&
                    <button
                        className="btn btn-default mx-0 px-1"
                        onClick={() => editButtonHandler(props.data)}
                    >
                        <KTIcon iconName="pencil" iconType="outline" className="fs-4 text-gray-600" />
                    </button>
                }

                {isAllowed('loans.batch.items.delete') &&
                    <button
                        className="btn btn-default mx-0 px-1"
                        onClick={() => openDeleteModal(props.data.id)}
                    >
                        <KTIcon
                            iconName="trash"
                            iconType="outline"
                            className="text-danger fs-4"
                        />
                    </button>
                }
            </div>
        );
    };

    const LoanItemActionComponent = (props: any) => {
        return (
            <div className="d-flex flex-row link-primary" onClick={() => loanItemClickHandler(props.data)}>
                Manage
            </div>
        );
    };

    const addBtnHandler = () => {
        setData(null)
        setShowItemBox(true);
    }

    const [colDefs, setColDefs] = useState<any>([
        { headerName: "Item", field: "loanItem.label", flex: 1.5, filter: true },
        { headerName: "Unit", field: "unit.label", flex: 1, filter: true },
        {
            field: "unitPrice", flex: 1,
            valueFormatter: (params: any) => {
                return parseFloat(params.value).toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 });
            },
            filter: true
        },
        { headerName: "Max Quantity", field: "quantity", flex: 1, filter: true },
        { headerName: 'Supplier', field: "supplierDetails", flex: 1 },
        { field: "Action", flex: 1, cellRenderer: CustomActionComponent },
    ]);

    const bindBatchItems = async () => {
        const response = await loanBatchService.getLoanBatchItems(id);
        if (response !== null) { setItemCount(response.length) }
       
        setRowData(response);
    };

    useEffect(() => {
        bindBatchItems();
    }, []);

    useEffect(() => {
        bindBatchItems();

    }, [showConfirmBox, showItemBox]);

    return (
        <Content>
            {isAllowed('loans.batch.items.view') ? <KTCard className="shadow">
                {/* begin::Header */}
                <div className='card-header border-0 py-5'>
                    <h3 className='card-title align-items-start flex-column'>
                        <span className='card-label fw-bold fs-4 mb-1'>Loan Product Items</span>
                        {/* <span className='text-muted fw-semibold fs-7'>More than 500 new orders</span> */}
                    </h3>

                    {/* begin::Toolbar */}
                    <div className='card-toolbar' data-kt-buttons='true'>
                        {isAllowed('loans.batch.items.add') &&
                            <button className='btn btn-sm btn-color-muted btn-active btn-active-primary active px-4 me-1'
                                onClick={addBtnHandler}>
                                Add Item
                            </button>
                        }
                    </div>
                    {/* end::Toolbar */}
                </div>
                {/* end::Header */}
                <CustomTable
                    rowData={rowData}
                    colDefs={colDefs}
                    header=""
                    addBtnText={""}
                    searchTerm={searchTerm}
                    setSearchTerm={setSearchTerm}
                    addBtnLink={""}
                    addBtnHandler={addBtnHandler}

                />
            </KTCard> : <Error401 />}
            {showConfirmBox && (
                <ConfirmBox
                    confirmModel={confirmModel}
                    afterConfirm={afterConfirm}
                    loading={loading}
                />
            )}

            {showItemBox &&
                <LoanItemsModal
                    loanBatchId={id}
                    loanBatchName={loanBatch.name}
                    data={data}
                    afterConfirm={afterConfirm}
                    itemsData={rowData ? rowData : []}
                />
            }
        </Content>
    );
};
export default ListBatchItems;