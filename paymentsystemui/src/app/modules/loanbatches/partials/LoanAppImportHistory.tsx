import { useState } from 'react'
import { KTCard, KTCardBody, KTIcon } from '../../../../_metronic/helpers';
import LoanAppWarningCard from './LoanAppWarningCard';
import LoanAppSuccessCard from './LoanAppSuccessCard';
import { isAllowed } from '../../../../_metronic/helpers/ApiUtil';
import saveAs from 'file-saver';
import * as XLSX from "xlsx";
import moment from 'moment';
import { NoDataFound } from '../../../../lib/NoData';
import config from '../../../../environments/config';
import CustomTable from '../../../../_shared/CustomTable/Index';

const LoanAppImportHistory = ({ data, loanBatch }: any) => {
    const [showDetails, setShowDetails] = useState<boolean>(false);
    const [selectedData, setSelectedData] = useState<any>();

    const handleDetailsClick = (data: any) => {
        setSelectedData(data.item)
        setShowDetails(prev => !prev);
    };

    const [colDefs] = useState<any[]>([
        { field: "rowNumber", flex: 1, sortable: false, },
        { field: "witnessFullName", flex: 1, },
        { field: "witnessNationalId", flex: 1, },
        { field: "witnessPhoneNo", flex: 1, sortable: false, },
        { field: "witnessRelation", flex: 1, },
        { field: "dateOfWitness", flex: 1, },
        { field: "enumeratorFullName", flex: 1, sortable: false, },
        { field: "principalAmount", flex: 1, },
        { field: "statusId", flex: 1, },
        { field: "validationErrors", flex: 2, },
    ]);

    const [itemColDefs] = useState<any[]>([
        { field: "rowNumber", flex: 1, sortable: false, },
        { field: "itemName", flex: 1, },
        { field: "unitPrice", flex: 1, },
        { field: "statusId", flex: 1, },
        { field: "quantity", flex: 1, },
        { field: "validationErrors", flex: 2, },
    ]);

    const CustomActionComponent = (props: any) => {
        return <div className="d-flex flex-row">
            <button className="btn link-primary mx-0 fs-4 px-1" onClick={() => handleDetailsClick(props)}>
                Review
            </button>
        </div>;
    };

    const downloadInterimList = async () => {
        try {
            const workbook = XLSX.utils.book_new();
            if (Array.isArray(data[0].applications)) {
                // 1. Loan applications
                const mappedData = data[0].applications.map((row: any) => ({
                    "Row Number": row.rowNumber,
                    "Farmer Name": ``,
                    "Witness Full Name": row.witnessFullName,
                    "Witness National Id": row.witnessNationalId,
                    "Witness Phone No": row.witnessPhoneNo,
                    "Witness Relation": row.witnessRelation,
                    "Date Of Witness": row.dateOfWitness,
                    "Enumerator Full Name": row.enumeratorFullName,
                    "Principal Amount": row.principalAmount,
                    "Succeeded": row.statusId ? "Yes" : "No",
                    "Validation Errors": row.validationErrors,
                }));

                const worksheet = XLSX.utils.json_to_sheet(mappedData);
                XLSX.utils.book_append_sheet(workbook, worksheet, "LoanApplication");

                // 2. Items
                const itemData = data[0].loanItems.map((row: any) => ({
                    "Row Number": row.rowNumber,
                    "Item name": row.itemName,
                    "Price per unit": row.unitPrice,
                    "Quantity": row.quantity,
                    "Succeeded": row.statusId ? "Yes" : "No",
                    "Validation Errors": row.validationErrors,
                }));

                const worksheetItem = XLSX.utils.json_to_sheet(itemData);
                XLSX.utils.book_append_sheet(workbook, worksheetItem, "Loan Items");

                const blob = new Blob(
                    [XLSX.write(workbook, { bookType: "xlsx", type: "array" })],
                    { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" }
                );

                saveAs(blob, `${loanBatch?.batchName || "loan_application"}_${moment().format('DD_MM_YYYY_HHmmss')}.xlsx`);
            } else {
                alert("No data available to download.");
            }
        } catch (error) {
            console.error("Failed to download interim list:", error);
            alert("An error occurred while downloading the list. Please try again.");
        }
    };


    return (
        <KTCard className="m-0 p-0">
            {/* begin::Header */}
            <div className="card-header" id="kt_help_header">
                <h4 className="card-title align-items-start flex-column">Import History</h4>
                <div className="card-toolbar">

                </div>
            </div>
            {/* end::Header */}

            <KTCardBody className="m-0 p-0">
                {data && data.length == 0 && (<NoDataFound />)}
                <div className="p-5">
                    <div className="row">

                        <div className='col-md-12 text-end'>
                            {data && data.length > 0 && (
                                <button
                                    className="btn btn-dark btn-sm"
                                    onClick={downloadInterimList}
                                >
                                    <KTIcon iconName="exit-down" /> Download Interim List
                                </button>
                            )}
                        </div>
                        <div className='col-md-12'>
                            <table className="mt-4 w-100 border-collapse border border-gray-300">
                                {data && data.map((item: any, index: number) => (
                                    <tbody>
                                        <tr key={index}>
                                            <td className="border p-3">
                                                <h6 className='fs-5'>{item.fileName}</h6>
                                                <span className='fs-6 text-muted'>{moment(item.importedDateTime).format(config.dateTimeFormat)}</span>
                                            </td>
                                            <td className="border p-3">
                                                {item?.isFailedBatch === false && (
                                                    <div className="badge fs-7 badge-light-success  fw-normal">Succeeded</div>
                                                )}
                                                {item?.isFailedBatch === true && (
                                                    <div className="badge fs-7 badge-light-danger  fw-normal">Failed</div>
                                                )}
                                            </td>
                                            <td className="border p-3">
                                                <CustomActionComponent item={item} />
                                            </td>
                                        </tr>
                                    </tbody>
                                )
                                )}
                            </table>
                        </div>
                        <div className='col-md-12 mt-4'>
                            {showDetails && selectedData.isFailedBatch == false && <div>
                                <LoanAppSuccessCard
                                    batch={loanBatch}
                                    status={loanBatch?.stageText}
                                    isAllowed={isAllowed}
                                    excelImportId={selectedData.excelImportId}
                                />
                            </div>}

                            {showDetails && selectedData.isFailedBatch == true && <div>
                                <LoanAppWarningCard batchId={loanBatch.id} />

                                <div>
                                    {/* Tab headers */}
                                    <ul className="nav nav-tabs border-bottom-0" id="tableTabs" role="tablist">
                                        <li className="nav-item" role="presentation">
                                            <button
                                                className="nav-link active fw-semibold px-4 py-2 text-dark border rounded-top"
                                                id="applications-tab"
                                                data-bs-toggle="tab"
                                                data-bs-target="#applications"
                                                type="button"
                                                role="tab"
                                                style={{ backgroundColor: '#f8f9fa', fontWeight: 'bold' }}
                                            >
                                                Applications
                                            </button>
                                        </li>
                                        <li className="nav-item ms-2" role="presentation">
                                            <button
                                                className="nav-link fw-semibold px-4 py-2 text-dark border rounded-top"
                                                id="loanItems-tab"
                                                data-bs-toggle="tab"
                                                data-bs-target="#loanItems"
                                                type="button"
                                                role="tab"
                                                style={{ backgroundColor: '#f8f9fa', fontWeight: 'bold' }}
                                            >
                                                Loan Items
                                            </button>
                                        </li>
                                    </ul>

                                    {/* Tab contents */}
                                    <div className="tab-content border border-top-0 p-4 rounded-bottom bg-white">
                                        <div
                                            className="tab-pane fade show active"
                                            id="applications"
                                            role="tabpanel"
                                            aria-labelledby="applications-tab"
                                        >
                                            <CustomTable
                                                rowData={data[0].applications}
                                                colDefs={colDefs}
                                                header=""
                                            />
                                        </div>
                                        <div
                                            className="tab-pane fade"
                                            id="loanItems"
                                            role="tabpanel"
                                            aria-labelledby="loanItems-tab"
                                        >
                                            <CustomTable
                                                rowData={data[0].loanItems}
                                                colDefs={itemColDefs}
                                                header=""
                                            />
                                        </div>
                                    </div>
                                </div>

                            </div>}

                        </div>
                    </div>
                </div>


            </KTCardBody>
        </KTCard>
    )
}

export default LoanAppImportHistory
