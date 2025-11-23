/* eslint-disable @typescript-eslint/no-explicit-any */
/* eslint-disable @typescript-eslint/no-unused-vars */
import CustomTable from "../../../../_shared/CustomTable/Index"
import { useState } from "react";
import { Content } from "../../../../_metronic/layout/components/content";
import { useEffect } from "react";
import { IConfirmModel } from "../../../../_models/confirm-model";
import { KTCard } from "../../../../_metronic/helpers";
import { useDispatch, useSelector } from "react-redux";
import { addToLoanItems } from "../../../../_features/loanitem/loanItemSlice";
import { isAllowed } from "../../../../_metronic/helpers/ApiUtil";
import { Error401 } from "../../errors/components/Error401";
import ImportService from "../../../../services/ImportService";
import moment from "moment";
const importService = new ImportService();

const ListImportExcels = (props: any) => {
    const { id, setItemCount, CountryId, batch } = props

    const [rowData, setRowData] = useState<any>();
    const [searchTerm, setSearchTerm] = useState<string>("");
    const [confirmModel, setConfirmModel] = useState<IConfirmModel>();
    const [showConfirmBox, setShowConfirmBox] = useState<boolean>(false);
    const [showItemBox, setShowItemBox] = useState<boolean>(false);
    const [loading, setLoading] = useState(false);
    const [data, setData] = useState<any>();
    const [farmers, setFarmers] = useState<any>();
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

    const afterConfirm = (res: any) => {
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
                <button
                    className="btn btn-theme "
                    onClick={() => editButtonHandler(props.data)}
                >
                    disbursement
                </button>

            </div>
        );
    };

    const addBtnHandler = () => {
        setData(null)
        setShowItemBox(true);
    }

    const disBtnHandler = () => {
        setData(null)
        setShowItemBox(true);
    }

    const initiationDateRenderer = (props: any) => {

        const dateValue = props.data.importedDateTime;
        if (!dateValue) return "";
        return moment(dateValue).format('DD-MM-YYYY, hh:mm');
    };

    const [colDefs, setColDefs] = useState<any>([
        { field: "filename", flex: 1, filter: true },
        { field: "importedDateTime", flex: 1, cellRenderer: initiationDateRenderer },
    ]);
    
    const bindImports = async () => {
        if (searchTerm.length === 0 || searchTerm.length > 2) {
            const data: any = {
                pageNumber: 1,
                pageSize: 10000,
                paymentBatchId: batch.id,
            };

            const response = await importService.getExcelImportData(data);
           
            setRowData(response);
        }
    }

    useEffect(() => {
        bindImports()
    }, []);

    return (
        <Content>
            {isAllowed('payments.batch.history') ? <KTCard className="shadow">
                {/* begin::Header */}
                <div className='card-header border-0 py-5'>
                    <h3 className='card-title align-items-start flex-column'>
                        <span className='card-label fw-bold fs-4 mb-1'>Excel imports</span>
                        {/* <span className='text-muted fw-semibold fs-7'>More than 500 new orders</span> */}
                    </h3>

                    {/* begin::Toolbar */}
                    <div className='card-toolbar' data-kt-buttons='true'>
                        {/* <button
                         className='btn btn-sm btn-color-muted btn-active btn-active-primary active px-4 me-1 w-200px me-3'
                            onClick={handleExport}>
                                <KTIcon iconName='exit-up' className='fs-2' />
                                Export 
                        </button> */}
                        {/* <button 
                        className='btn btn-sm btn-light  active px-4 me-1 w-200px me-3' 
                        onClick={() => setShowItemBox(true)}
                           >
                              Disbursement 
                        </button> */}

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


                />
            </KTCard> : <Error401 />}
            {/* {showConfirmBox && (
                <ConfirmBox
                    confirmModel={confirmModel}
                    afterConfirm={afterConfirm}
                    loading={loading}
                />
            )}
             {showItemBox && (
                <DisbursementModal
                    confirmModel={confirmModel}
                    afterConfirm={afterConfirm}
                    loading={loading}
                    farmers={rowData}
                    batch={batch}
                />
            )} */}

        </Content>
    );
};
export default ListImportExcels;