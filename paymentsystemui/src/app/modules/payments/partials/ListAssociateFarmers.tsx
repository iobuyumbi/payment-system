/* eslint-disable @typescript-eslint/no-explicit-any */
/* eslint-disable @typescript-eslint/no-unused-vars */
import CustomTable from "../../../../_shared/CustomTable/Index"
import { useState } from "react";
import { Content } from "../../../../_metronic/layout/components/content";
import { useEffect } from "react";
import { IConfirmModel } from "../../../../_models/confirm-model";
import { KTIcon } from "../../../../_metronic/helpers";
import { ConfirmBox } from "../../../../_shared/Modals/ConfirmBox";
import { KTCard } from "../../../../_metronic/helpers";
import { useDispatch, useSelector } from "react-redux";
import * as XLSX from 'xlsx';
import { saveAs } from 'file-saver';
import { addToLoanItems } from "../../../../_features/loanitem/loanItemSlice";
import { isAllowed } from "../../../../_metronic/helpers/ApiUtil";
import { Error401 } from "../../errors/components/Error401";
import AssociateService from "../../../../services/AssociateService";
import DisbursementModal from "./DisbursementModal";

const associateService = new AssociateService();

const ListAssociateFarmers = (props: any) => {
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

    const [colDefs, setColDefs] = useState<any>([
        { field: "firstName", flex: 1, filter: true },
        { field: "otherNames", flex: 1, filter: true },
        { field: "mobile", flex: 1, filter: true },
        { field: "paymentPhoneNumber", flex: 1, headerName: 'Payment Phone', filter: true },


    ]);
    const bindAssociateBatch = async () => {
        if (searchTerm.length === 0 || searchTerm.length > 2) {
            const data: any = {
                pageNumber: 1,
                pageSize: 10000,
                batchId: batch.loanBatchId,
            };
            const response = await associateService.getAssociateData(data);
            setRowData(response);
        }
    }
    const handleExport = () => {
        const workbook = XLSX.utils.book_new();


        // Define custom headers
        //const customHeaders = ['Full Name', 'Age', 'Email Address'];

        // Map data to custom headers
        const mappedData = rowData.map((row: any) => ({
            'System Id': row.systemId,
            'First name': row.firstName,
            'Other names': row.otherNames,
            'Country': row.country.countryName,
            'Mobile': row.mobile,
            'Payment phone number': row.paymentPhoneNumber,
            'Access to mobile': row.accessToMobile,
            'County/Region': row.adminLevel1.countyName,
            'Sub-County/District': row.adminLevel2.subCountyName,
            'Ward/Sub-County/County': row.adminLevel3.wardName,
            'Village': row.village,
            'Alternate contact number': row.alternateContactNumber,
            'Beneficiary id': row.beneficiaryId,
            'Birth month': row.birthMonth,
            'Birth year': row.birthYear,
            'Cooperative': row.cooperativeName,
            'Email': row.email,
            'Enumeration date': row.enumerationDate,
            'Gender': row.gender === 1 ? 'Male' : row.gender === 2 ? 'Female' : 'Other',
            'Has disability': row.hasDisability,
            'Is farmer phone owner?': row.isFarmerPhoneOwner,
            'Participant id': row.participantId,
            'Phone owner address': row.phoneOwnerAddress,
            'Phone owner name': row.phoneOwnerName,
            'Phone owner national id': row.phoneOwnerNationalId
        }));

        const worksheet = XLSX.utils.json_to_sheet(mappedData);
        XLSX.utils.book_append_sheet(workbook, worksheet, 'Sheet1');
        const blob = new Blob([XLSX.write(workbook, { bookType: 'xlsx', type: 'array' })], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
        saveAs(blob, 'farmers.xlsx');
    }
    useEffect(() => {
        bindAssociateBatch()
    }, [farmers]);

    return (
        <Content>
            {isAllowed('payments.batch.history') ? <KTCard className="shadow">
                {/* begin::Header */}
                <div className='card-header border-0 py-5'>
                    <h3 className='card-title align-items-start flex-column'>
                        <span className='card-label fw-bold fs-4 mb-1'>Farmers</span>
                        {/* <span className='text-muted fw-semibold fs-7'>More than 500 new orders</span> */}
                    </h3>

                    {/* begin::Toolbar */}
                    <div className='card-toolbar' data-kt-buttons='true'>
                        <button
                            className='btn btn-sm btn-color-muted btn-active btn-active-primary active px-4 me-1 w-200px me-3'
                            onClick={handleExport}>
                            <KTIcon iconName='exit-up' className='fs-2' />
                            Export
                        </button>
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
            {showConfirmBox && (
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
            )}

        </Content>
    );
};
export default ListAssociateFarmers;