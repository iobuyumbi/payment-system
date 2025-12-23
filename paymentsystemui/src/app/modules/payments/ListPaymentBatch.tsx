import CustomTable from '../../../_shared/CustomTable/Index'
import { useState, useEffect, useCallback } from 'react';
import { Content } from '../../../_metronic/layout/components/content';
import { PageLink, PageTitle } from '../../../_metronic/layout/core';
import { PageTitleWrapper } from '../../../_metronic/layout/components/toolbar/page-title';
import { KTCard, KTCardBody, KTIcon } from '../../../_metronic/helpers';
import { IConfirmModel } from '../../../_models/confirm-model';
import { ConfirmBox } from '../../../_shared/Modals/ConfirmBox';
import { useDispatch } from 'react-redux';
import { Link, useNavigate } from 'react-router-dom';
import { isAllowed } from '../../../_metronic/helpers/ApiUtil';
import { Error401 } from '../errors/components/Error401';
import moment from 'moment';
import PaymentBatchService from '../../../services/PaymentBatchService';
import { addToPaymentBatch } from '../../../_features/payment-batch/paymentBatchSlice';
import PaymentBatchStatusBadge from '../../components/PaymentBatchStatusBadge';
import AuditListModal from '../../../_shared/Modals/AuditListModal';
import PaymentModuleText from '../../components/PaymentModuleText';

const paymentBatchService = new PaymentBatchService();
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
        isActive: false,
    },
];

export const ListPaymentBatch: React.FC = (props: any) => {
    const navigate = useNavigate();
    const dispatch = useDispatch();

    // Row Data: The data to be displayed.
    const [rowData, setRowData] = useState<any>();
    const [searchTerm, setSearchTerm] = useState<string>('');
    const [showConfirmBox, setShowConfirmBox] = useState<boolean>(false);
    const [confirmModel, setConfirmModel] = useState<IConfirmModel>();
    const [loading, setLoading] = useState(false);
    const [showAuditModal, setShowAuditModal] = useState(false);
    const [currentId, setCurrentId] = useState("");

    const editButtonHandler = (value: any) => {
        dispatch(addToPaymentBatch(value));
        navigate(`/payment-batch/edit/${value.id}`)
    }

    const titleClickHandler = (value: any) => {
        dispatch(addToPaymentBatch(value));
        navigate(`/payment-batch/details/${value.id}`);

        // if (hasRole('Admin')) {
        //     navigate(`/payment-batch/details/${value.id}`);
        // }
        // else if (hasRole('Initiator')) {
        //     navigate(`/payment-batch/details/${value.id}`);
        // }
        // else if (hasRole('Reviewer')) {
        //     navigate(`/payment-batch/review/${value.id}`);
        // }
        // else if (hasRole('Approver')) {
        //     navigate(`/payment-batch/approve/${value.id}`);
        // }
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
        if (res) bindPaymentBatch()
        setShowConfirmBox(false)

        setShowAuditModal(false);
    }

    const paymentTitleComponent = (props: any) => {
        return <div className="d-flex flex-row">
            <a className="link-primary cursor-pointer" onClick={() => titleClickHandler(props.data)}>
                {props.data.batchName}
            </a>
        </div>;
    };

    const CustomActionComponent = (props: any) => {
        return <div className="d-flex flex-row">
            {/* Audit log */}
            {isAllowed('payments.batch.audit') &&
                <button
                    onClick={() => {
                        setShowAuditModal(true);
                        setCurrentId(props.data.id);
                    }}
                    className="btn btn-default link-primary"
                >
                    Audit Log
                </button>
            }

            {/* Payment History */}
            {/* {isAllowed('payments.batch.history') &&
                <Link to={`/payment-batch/history/${props.data.id}`} className='btn btn-default link-primary'>Payment History</Link>
            } */}

            {/* Edit */}
            {isAllowed('payments.batch.edit') && props.data?.status?.stageText == 'Initiated' &&
                <button className="btn btn-default mx-0 link-primary" onClick={() => editButtonHandler(props.data)}>
                    <KTIcon iconName="pencil" iconType="outline" className="fs-4" />
                </button>
            }

            {/* Delete */}
            {isAllowed('payments.batch.delete') && props.data?.status?.stageText == 'Initiated' &&
                <button className="btn btn-default mx-0 px-1" onClick={() => openDeleteModal(props.data.id)}>
                    <KTIcon iconName="trash" iconType="outline" className="text-danger fs-4" />
                </button>
            }
        </div>;
    };

    const DateComponent = (props: any) => {
        return <div className="d-flex flex-row">
            {moment(props.data.createdOn).format('YYYY-MM-DD HH:mm')}
        </div>;
    };

    const lastUpdateComponent = (props: any) => {
        const { dateCreated, updatedOn } = props.data;

        const latestDate = moment(updatedOn).isAfter(moment(dateCreated))
            ? updatedOn
            : dateCreated;

        return (
            <div className="d-flex flex-row">
                {moment(latestDate).format('YYYY-MM-DD HH:mm')}
            </div>
        );
    };
    const StatusComponent = (props: any) => {
        return <PaymentBatchStatusBadge statusText={props.data?.status?.stageText} />;
    };

    const PaymentModuleComponent = (props: any) => {
        return <PaymentModuleText paymentModule={props.data?.paymentModule} />;
    };
    // const handleExport = () => {
    //     const workbook = XLSX.utils.book_new();
    //     const mappedData = rowData.map((row: any) => ({
    //         'System Id': row.systemId,
    //         'First name': row.firstName,
    //         'Other names': row.otherNames,
    //         'Country': row.country.countryName,
    //         'Mobile': row.mobile,
    //         'Payment phone number': row.paymentPhoneNumber,
    //         'Access to mobile':row.accessToMobile,
    //         'County/Region':row.adminLevel1.countyName,
    //         'Sub-County/District':row.adminLevel2.subCountyName,
    //         'Ward/Sub-County/County':row.adminLevel3.wardName,
    //         'Village' : row.village,
    //         'Alternate contact number' :row.alternateContactNumber,
    //         'Beneficiary id' :row.beneficiaryId,
    //         'Birth month' : row.birthMonth,
    //         'Birth year' :  row.birthYear,
    //         'Cooperative' :row.cooperativeName,
    //         'Email' : row.email,
    //         'Enumeration date' : row.enumerationDate,
    //         'Gender':row.gender ===1? 'Male' : row.gender ===2 ? 'Female' : 'Other',
    //         'Has disability': row.hasDisability,
    //         'Is farmer phone owner?':row.isFarmerPhoneOwner,
    //         'Participant id' : row.participantId,
    //         'Phone owner address' : row.phoneOwnerAddress,
    //         'Phone owner name':row.phoneOwnerName,
    //         'Phone owner national id':row.phoneOwnerNationalId
    //     }));

    //     const worksheet = XLSX.utils.json_to_sheet(mappedData);
    //     XLSX.utils.book_append_sheet(workbook, worksheet, 'Sheet1');
    //     const blob = new Blob([XLSX.write(workbook, { bookType: 'xlsx', type: 'array' })], { type: 'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet' });
    //     saveAs(blob, 'farmers.xlsx');
    // }

    // Column Definitions: Defines the columns to be displayed.
    const [colDefs, setColDefs] = useState<any>([
        { field: "batchName", flex: 2, cellRenderer: paymentTitleComponent, filter: true },
        { field: "paymentModule", flex: 1.5, cellRenderer: PaymentModuleComponent, filter: true },
        { field: "createdOn", flex: 1.5, filter: true, cellRenderer: DateComponent },
        { field: "country.countryName", flex: 1, headerName: 'Country', filter: true },
        { field: "status.stageText", headerName: 'Status', cellRenderer: StatusComponent, filter: true },
        { field: "paymentStats.totalAmount", flex: 1, headerName: 'Total Amount', filter: true, valueFormatter: (params: any) => {
            
        return parseFloat(params.data.paymentStats.totalAmount)?.toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 });
      },  },
        { field: "paymentStats.beneficiaryCount", flex: 1, headerName: 'Beneficiary Count', filter: true },
        { field: "updatedOn", flex: 1, headerName: 'Last Updated On', cellRenderer: lastUpdateComponent },
        { headerName: "Action", flex: 1, cellRenderer: CustomActionComponent },
        ]);

    const bindPaymentBatch = async () => {
        if (searchTerm.length === 0 || searchTerm.length > 2) {
            const data: any = {
                pageNumber: 1,
                pageSize: 10000,
            };

            const response = await paymentBatchService.getPaymentBatchData(data);
            setRowData(response);
        }
    }

  const fetchbatchPagedData = useCallback(async (page: number, pageSize: number) => {
    try {
      const searchParams: any = {
        pageNumber: page,
        pageSize: pageSize,
      };
      const response = await paymentBatchService.getPaymentBatchPagedData(searchParams);
      const data = await response;
      // Perform the data transformation here 
      const transformedData = {
        rows: data.result.paymentBatchResponseModel, // Extract the array of farmers
        totalRows: response.result.paymentBatchStats.totalBatches // Extract the total count
      };
    
      console.log("Transformed data for grid:", transformedData);

      return transformedData;
    } catch (error) {
      console.error("Failed to fetch farmers data:", error);
      return { rows: [], totalRows: 0 }; // Return a default structure on error
    }
  }, []); 


    useEffect(() => {
        bindPaymentBatch();
    }, [searchTerm]);

    const [pageData, setPageData] = useState<{ title: string; value: number }[]>([]);

    // useEffect(() => {
    //     if (rowData) {
    //         setPageData([
    //             { title: 'TotalProducts', value: rowData[0] ? rowData[0]?.totalBatches : 0 },
    //             { title: 'Open', value: rowData.filter((item: any) => item.statusId === 1).length },
    //             { title: 'In Review', value: rowData.filter((item: any) => item.statusId === 2).length },
    //             { title: 'Accepting Applications', value: rowData.filter((item: any) => item.statusId === 3).length },
    //             { title: 'Closed', value: rowData.filter((item: any) => item.statusId === 4).length },
    //         ])
    //     }
    // }, [rowData]);


    return (
        <Content>
            {isAllowed('payments.batch.history') ? <>
                <PageTitleWrapper />
                <PageTitle breadcrumbs={breadCrumbs}>Payment Batches</PageTitle>
                <div className="mx-3">
                    {/* {rowData && <KeyMetrics keyMetrics={pageData} className="shadow my-3" />} */}
                </div>
                <KTCard className='shadow my-3'>
                    <KTCardBody className='m-0 p-0'>
                        <CustomTable
                            rowData={rowData}
                            colDefs={colDefs}
                            header="payment-batch"
                            addBtnText={isAllowed('payments.batch.add') ? "Add payment-batch" : ""}
                            searchTerm={searchTerm}
                            setSearchTerm={setSearchTerm}
                            addBtnLink={isAllowed('payments.batch.add') ? "/payment-batch/add" : ""}
                            showExport={isAllowed('payments.batch.history')}
                            serverSidePagination={true}
                            fetchDataFunc ={fetchbatchPagedData}
                        />
                    </KTCardBody>
                </KTCard> </> : <Error401 />}
            {showAuditModal && (
                <AuditListModal
                    exModule={"PaymentBatch"}
                    componentId={currentId}
                    onClose={afterConfirm}
                />
            )}
            {showConfirmBox && <ConfirmBox confirmModel={confirmModel} afterConfirm={afterConfirm} loading={loading} />}
        </Content>
    )
}
