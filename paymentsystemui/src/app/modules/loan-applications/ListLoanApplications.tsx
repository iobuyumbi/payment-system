import CustomTable from '../../../_shared/CustomTable/Index'
import { useRef, useState } from 'react';
import { Content } from '../../../_metronic/layout/components/content';
import LoanApplicationService from '../../../services/LoanApplicationService';
import moment from 'moment';
import { KTCardBody, KTIcon } from '../../../_metronic/helpers';
import { useEffect } from 'react';
import { KTCard } from '../../../_metronic/helpers';
import { useNavigate } from 'react-router-dom';
import ApplicationStatusService from '../../../services/ApplicationStatusService';
import { toast } from 'react-toastify';
import { Error401 } from '../errors/components/Error401';
import { isAllowed } from '../../../_metronic/helpers/ApiUtil';
import { AppStatusBadge } from '../../../_shared/Status/AppStatusBadge';
import * as XLSX from "xlsx";
import saveAs from "file-saver";
import { PageLink, PageTitle } from '../../../_metronic/layout/core';
import { PageTitleWrapper } from '../../../_metronic/layout/components/toolbar/page-title';
import StatusTransitionGuide from '../loanbatches/blocks/StatusTransitionGuide';
import LoanApplicationsModal from '../loanbatches/partials/LoanApplicationsModal';
import SearchComponent from './blocks/SearchComponent';
import ActionComponent from './blocks/ActionComponent';
import LoanBatchService from '../../../services/LoanBatchService';
import { useDispatch } from 'react-redux';
import { addToLoanApplication } from '../../../_features/loan/loanApplicationSlice';
import { ImportModal } from '../../../_shared/Modals/ImportModal';

const loanBatchService = new LoanBatchService();
const loanApplicationService = new LoanApplicationService();
const applicationStatusService = new ApplicationStatusService();

// types.ts
export type Status = 'Draft' | 'Accepted' | 'Rejected' | 'Disbursed' | 'Closed';

export interface LoanApplication {
    id: string;
    statusText: Status;
    loanNumber: string;
}

export const statusLookup: Record<string, Status> = {
    '3118a07e-013a-4b3a-a2c1-74c921feeba1': 'Accepted',
    '0dddbbcb-ac18-421a-942d-05ca579abb0c': 'Rejected',
    'f49faffa-b113-4546-ac7f-485164e5a822': 'Closed',
    '6f103a88-8443-45ad-9c37-afe07f6b48e1': 'Draft',
    'e24d24a8-fc69-4527-a92a-97f6648a43c5': 'Disbursed',
};

export const statusTransitions: Record<Status, Status[]> = {
    Draft: ['Accepted', 'Rejected'],
    Accepted: ['Disbursed'],
    Rejected: [],
    Disbursed: ['Closed'],
    Closed: [],
};

const exModule = "loanApplication";

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

const ListLoanApplications = ({ loanBatch }: any) => {
    const navigate = useNavigate();
    const dispatch = useDispatch();

    const [importBtnText, setImportBtnText] = useState<boolean>(false);
    const [loading, setLoading] = useState(false)
    const [rowData, setRowData] = useState<any>();
    const [showConfirmBox, setShowConfirmBox] = useState<boolean>(false);
    const [actions, setActions] = useState<any>();
    const [selectedAction, setSelectedAction] = useState<any>();
    const [allApplications, setAllApplications] = useState<any>([]);
    const [selectedLoanBatch, setSelectedLoanBatch] = useState<any>();
    const [loanBatches, setLoanBatches] = useState<any>();
    const [showImport, setShowImport] = useState<boolean>(false);

    const linkButtonHandler = (value: any) => {
        dispatch(addToLoanApplication(value))
        navigate(`/loans/applications/detail`);
    }

    const afterConfirm = (res: any) => {
        setShowConfirmBox(false);
        setShowItemBox(false);
        setShowImport(false);
    }

    const [showItemBox, setShowItemBox] = useState<boolean>(false);
    const editButtonHandler = (value: any) => {
        navigate(`/loans/applications/edit/${value.id}`);
        setShowItemBox(true)
    }

    const CustomActionComponent = (props: any) => {
        return <div className="d-flex flex-row">
            {!props.data.inUse && isAllowed('loans.applications.edit') && props.data.statusText == 'Draft' &&
                <button className="btn btn-default mx-0 px-1" onClick={() => editButtonHandler(props.data)}>
                    <KTIcon iconName="pencil" iconType="outline" className="fs-4" /> {props.status}
                </button>
            }
        </div>;
    };

    const MainLinkActionComponent = (props: any) => {
        return <button className="btn btn-default link-primary mx-0 px-1" onClick={() => linkButtonHandler(props.data)}>
            {props.data.loanNumber}
        </button>;
    };

    const dateOfWitnessRenderer = (props: any) => {
        const dateValue = props.data.dateOfWitness;
        if (!dateValue) return "";
        return moment(dateValue).format('YYYY-MM-DD hh:mm A');
    };

    const statusRenderer = (props: any) => {
        return <AppStatusBadge value={props.data.statusText} />
    };

    const DateComponent = (props: any) => {
        return (
            <div className="d-flex flex-row">
                {moment(props.data.createdOn).format('YYYY-MM-DD HH:mm ')}
            </div>
        );
    };

    const bindLoanBatches = async () => {
        const response = await loanBatchService.getValidLoanBatches();

        if (response && response.length > 0) {
            setLoanBatches(response);

            // Get the first loan batch ID
            const firstBatchId = response[0].id;
            var selectedBatch = response[0];

            setSelectedLoanBatch(selectedBatch);
            // Fetch and bind loan applications for that batch
            await bindApplications(firstBatchId);
        } else {
            setLoanBatches([]); // Optional: clear if empty
        }
    };

    const bindActions = async () => {
        const data = {
            pageNumber: 1,
            pageSize: 10000,
        };
        const response = await applicationStatusService.getStatusData(data);
        response.unshift({ id: null, name: 'Select ' });
        setActions(response)
    };

    const [colDefs, setColDefs] = useState<any>([
        {
            field: 'select',
            width: 40,
            checkboxSelection: true,
            headerCheckboxSelection: true,
            filter: true,
        },
        { headerName: "Loan account number", field: 'loanAccountNumber', width: 200, sort: false, cellRenderer: MainLinkActionComponent, filter: true },
        // { headerName: "Loan Product", field: "loanBatch.name", filter: true, width: 180 },
        { headerName: "Farmer name", field: "farmer.fullName", filter: true, width: 180 },
        { headerName: "Farmer Uwanjani ID", field: "farmer.systemId", filter: true, width: 180 },
        {
            headerName: "Total Items Value",
            field: "totalValue",
            valueFormatter: (params: any) => parseFloat(params.value).toFixed(2),
            filter: true,
            width: 140,
        },
        {
            field: "feeApplied",
            valueFormatter: (params: any) => parseFloat(params.value).toFixed(2),
            filter: true,
            width: 150,
        },
        {
            headerName: "Effective Principal",
            field: "principalAmount", valueFormatter: (params: any) => {
                return parseFloat(params.value).toFixed(2);
            }, filter: true, width: 180,
        },
        { field: "createdOn", width: 120, filter: true, cellRenderer: DateComponent },
        { field: "status", width: 120, filter: true, cellRenderer: statusRenderer },
        // { field: "Action", width: 150, cellRenderer: CustomActionComponent },
    ]);

    const bindApplications = async (loanBatchId: any) => {

        const data = {
            pageNumber: 1,
            pageSize: 1000,
            batchId: loanBatchId
        };
        const response = await loanApplicationService.getLoanApplicationData(data);

        if (response)  // Format the `createdOn` field
        {
            const formattedResponse = response.map(item => ({
                ...item,
            }));
            setAllApplications(formattedResponse)
            setRowData(formattedResponse);
        } else {
            setRowData([]);
        }
    };

    const gridRef = useRef<any>();
    const [selectedStatusId, setSelectedStatusId] = useState('');

    const handleApplyClick = () => {
        const targetStatus = statusLookup[selectedStatusId];
        if (!targetStatus) {
            alert('Please select a valid status.');
            return;
        }

        const selectedRows = gridRef?.current.api.getSelectedRows() as LoanApplication[];
        const invalidTransitions = selectedRows.filter(
            (row) => !statusTransitions[row.statusText]?.includes(targetStatus)
        );

        if (invalidTransitions.length > 0) {
            // const errorMsg = invalidTransitions
            //   .map((t) => `ID: ${t.id} - ${t.status} → ${targetStatus}`)
            //   .join('\n');
            const errorMsg = invalidTransitions
                .map((t) => `Status of "${t.statusText}" cannot be changed to "${targetStatus}" (Application ID: ${t.loanNumber})`)
                .join('\n');


            alert(`Invalid status transitions:\n${errorMsg}`);
        } else {
            // Proceed with bulk update
            console.log('Updating statuses to', targetStatus);
            updateStatuses(selectedRows, selectedStatusId);
        }
    };

    const updateStatuses = async (rows: LoanApplication[], newStatusId: string) => {
        if (rows.length == 0) return;

        setLoading(true);

        const response = await loanApplicationService.putLoanApplicationStatusData(newStatusId, rows);
        if (response) {
            toast.success("Success");
            setTimeout(() => {
                window.location.reload();
            }, 1000);
        }
        else {
            toast.error("Something went wrong");
        }
        setLoading(false);
    }

    const handleSearchChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
        const value = event.target.value;
        if (value === 'Select') {
            setSelectedAction(null);
            setRowData(allApplications);
        } else {
            const filteredData = allApplications.filter((item: any) => item.status === value);
            setRowData(filteredData);
        }
    }

    const handleExcelExport = () => {
        const workbook = XLSX.utils.book_new();

        // Map data to custom headers
        const mappedData = rowData.map((row: any) => ({
            "Loan number": row.loanNumber,
            "Loan product": row.loanBatch.name,
            "Date of witness": row.dateOfWitness ? moment(row.dateOfWitness).format("DD-MM-YYYY") : "",
            "Created on": row.createdOn ? moment(row.createdOn).format("DD-MM-YYYY HH:mm") : "",
            "Status": row.statusText ? row.statusText : "Unknown",
            "Interest amount": row.interestAmount,
            "Principal amount": row.principalAmount,
            "Remaining balance": row.remainingBalance,
            "Witness name": row.witnessFullName,
            "Witness phone number": row.witnessPhoneNo,
            "Witness national id": row.witnessNationalId,
            "Witness relation": row.witnessRelation,
            "Fee applied": row.feeApplied,
            "Project name": row.loanBatch.project.projectName,
            "Interest rate": row.loanBatch.interestRate,
            "Interest type": row.loanBatch.rateType,
            "Interest calculation type": row.loanBatch.calculationTimeframe,
            "Tenure": row.loanBatch.tenure,
            "System Id": row.farmer.systemId,
            "First name": row.farmer.firstName,
            "Other names": row.farmer.otherNames,
            "Country": row?.country?.countryName ?? "",
            "Mobile": row.farmer.mobile,
            "Payment phone number": row.farmer.paymentPhoneNumber,
            "Access to mobile": row.farmer.accessToMobile,
            "Alternate contact number": row.farmer.alternateContactNumber,
            "Beneficiary id": row.farmer.beneficiaryId,
            "Birth month": row.farmer.birthMonth,
            "Birth year": row.farmer.birthYear,
            "Cooperative": row.farmer.cooperativeName,
            "Enumeration date": row.farmer.enumerationDate,
        }));

        const worksheet = XLSX.utils.json_to_sheet(mappedData);
        XLSX.utils.book_append_sheet(workbook, worksheet, "Sheet1");

        const blob = new Blob(
            [XLSX.write(workbook, { bookType: "xlsx", type: "array" })],
            {
                type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            }
        );
        const timestamp = new Date()
            .toISOString()
            .replace(/[-:T]/g, '')
            .slice(0, 14); // e.g. 20250604
        const filename = `${rowData[0]?.loanBatch.name}_${timestamp}.xlsx`;

        saveAs(blob, filename);
    };

    const handleLoanBatchChange = (event: React.ChangeEvent<HTMLSelectElement>) => {
        setRowData([]);
        const value = event.target.value;
        bindApplications(value);
        var selectedBatch = loanBatches.find((item: any) => item.id === value);
        setSelectedLoanBatch(selectedBatch);
        // setSelectedLoanBatchId(value);
        // if (value === '0') {
        //     setRowData(allApplications);
        // } else {
        //     const filteredData = allApplications.filter((item: any) => item.loanBatch.id === value);
        //     setRowData(filteredData);
        // }
    };

    const handleImportApplication = () => {
        setShowImport(true);
    };

    const handleCreateNewLoan = () => {
        setShowItemBox(true);
    };

    useEffect(() => {
        bindLoanBatches();
        bindActions();
    }, []);

    useEffect(() => {
        bindActions();
    }, [showConfirmBox, showItemBox]);

    return (
        <Content>
            {isAllowed("loans.applications.view") ? (
                <>
                    {" "}
                    <PageTitleWrapper />
                    <PageTitle breadcrumbs={breadCrumbs}>Loan Applications</PageTitle>

                    <KTCard className='shadow mt-5'>
                        {/* begin::Header */}
                        <div className='card-header'>
                            <h3 className='card-title align-items-start flex-column'>
                                <span className='card-label fw-bold fs-4 mb-1'>Loan Applications</span>
                            </h3>
                            {selectedLoanBatch &&
                                <ActionComponent actions={actions}
                                    handleSearchChange={handleSearchChange}
                                    selectedStatusId={selectedStatusId}
                                    setSelectedStatusId={setSelectedStatusId}
                                    handleApplyClick={handleApplyClick}
                                    handleExcelExport={handleExcelExport}
                                    loading={loading}
                                    loanBatch={selectedLoanBatch}
                                    handleImportApplication={handleImportApplication}
                                    handleCreateNewLoan={handleCreateNewLoan}
                                />
                            }

                        </div>
                        <KTCardBody>
                            <SearchComponent
                                loanBatches={loanBatches}
                                actions={actions}
                                handleSearchChange={handleSearchChange}
                                selectedStatusId={selectedStatusId}
                                setSelectedStatusId={setSelectedStatusId}
                                handleApplyClick={handleApplyClick}
                                handleLoanBatchChange={handleLoanBatchChange}
                                loading={loading}
                            />

                            <div className="py-3">
                                <StatusTransitionGuide />
                            </div>
                            <div className="d-flex justify-content-end p-3">
                                {/* <button className ="btn btn-theme me-2">
                            Download PDF
                            </button> */}

                            </div>
                            {/* end::Header */}

                            <CustomTable
                                rowData={rowData}
                                colDefs={colDefs}
                                header=""
                                gridRef={gridRef}
                            />

                        </KTCardBody>
                    </KTCard>
                </>
            ) : (
                <Error401 />
            )}

            {/* {showItemBox && (
                <LoanApplicationsModal
                    loanBatch={loanBatch}
                    afterConfirm={afterConfirm}
                    isAdd={false}
                    reloadApplications={window.location.reload()}
                />
            )} */}
            {showItemBox && (
                <LoanApplicationsModal
                    loanBatch={loanBatch}
                    afterConfirm={afterConfirm}
                    isAdd={false}
                />
            )}

            {showImport && (
                <ImportModal
                    title={importBtnText}
                    exModule={exModule}
                    batchId={selectedLoanBatch.id}
                    afterConfirm={afterConfirm}
                    loanBatchName={selectedLoanBatch.name}
                    projectId={selectedLoanBatch.projectId}
                    loanBatch={selectedLoanBatch}
                />
            )}
        </Content>
    )
}

export default ListLoanApplications
