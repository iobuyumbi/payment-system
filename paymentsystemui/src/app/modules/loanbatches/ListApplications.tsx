/* eslint-disable @typescript-eslint/no-explicit-any */
/* eslint-disable @typescript-eslint/no-unused-vars */
import CustomTable from '../../../_shared/CustomTable/Index'
import { useRef, useState } from 'react';
import { Content } from '../../../_metronic/layout/components/content';
import LoanApplicationService from '../../../services/LoanApplicationService';
import moment from 'moment';
import { KTIcon } from '../../../_metronic/helpers';
import { useEffect } from 'react';
import { KTCard } from '../../../_metronic/helpers';
import { useNavigate } from 'react-router-dom';
import { useDispatch, useSelector } from 'react-redux';
import { addToLoanApplication } from '../../../_features/loan/loanApplicationSlice';
import ApplicationStatusService from '../../../services/ApplicationStatusService';
import { toast } from 'react-toastify';
import { toastNotify } from '../../../services/NotifyService';
import { Error401 } from '../errors/components/Error401';
import LoanApplicationsModal from './partials/LoanApplicationsModal';
import { isAllowed } from '../../../_metronic/helpers/ApiUtil';
import { AppStatusBadge } from '../../../_shared/Status/AppStatusBadge';
import StatusTransitionGuide from './blocks/StatusTransitionGuide';
import * as XLSX from "xlsx";
import saveAs from "file-saver";

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

const ListApplications = (props: any) => {
  const navigate = useNavigate();
  const dispatch = useDispatch();

  const [loading, setLoading] = useState(false)
  const [rowData, setRowData] = useState<any>();
  const [searchTerm, setSearchTerm] = useState<string>('');
  const [showConfirmBox, setShowConfirmBox] = useState<boolean>(false);
  const [actions, setActions] = useState<any>();
  const [selectedAction, setSelectedAction] = useState<any>();
  const [allApplications, setAllApplications] = useState<any>([]);

  const linkButtonHandler = (value: any) => {
    dispatch(addToLoanApplication(value))
    navigate('/loans/applications/detail')
  }

  const { loanBatch } = props;
  // const loanBatch = useSelector((state: any) =>
  //   state?.loanBatches,
  // )

  const afterConfirm = (res: any) => {
    bindApplications();
    setShowConfirmBox(false);
    setShowItemBox(false);
  }

  const [showItemBox, setShowItemBox] = useState<boolean>(false);
  const editButtonHandler = (value: any) => {
    dispatch(addToLoanApplication(value))
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
    debugger
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

  const bindActions = async () => {
    if (searchTerm.length === 0 || searchTerm.length > 2) {
      const data = {
        pageNumber: 1,
        pageSize: 10000,

      };
      const response = await applicationStatusService.getStatusData(data);
      response.unshift({ id: null, name: 'Select ' });
      setActions(response)
    }
  };

  const DateComponent = (props: any) => {
    return (
      <div className="d-flex flex-row">
        {moment(props.data.createdOn).format('YYYY-MM-DD HH:mm ')}
      </div>
    );
  };

  const [colDefs, setColDefs] = useState<any>([
    {
      field: 'select',
      width: 40,
      checkboxSelection: true,
      headerCheckboxSelection: true,
      filter: true,
    },
    { headerName: "Loan account number", width: 200, sort: false, cellRenderer: MainLinkActionComponent, filter: true },
    // { headerName: "Loan Product", field: "loanBatch.name", filter: true, flex: 1 },
    { headerName: "Farmer name", field: "farmer.fullName", filter: true, flex: 1 },
    { headerName: "Farmer Uwanjani ID", field: "farmer.systemId", filter: true, flex: 1 },
    {
      headerName: "Total Items Value",
      field: "totalValue", valueFormatter: (params: any) => {
        return parseFloat(params.value).toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 });
      }, filter: true, flex: 1
    },
    {
      field: "feeApplied", valueFormatter: (params: any) => {
        return parseFloat(params.value).toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 });
      }, filter: true, width: 140,
    },
    {
      headerName: "Effective Principal",
      field: "principalAmount", valueFormatter: (params: any) => {
        return parseFloat(params.value).toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 });
      }, filter: true, width: 180,
    },
    { field: "createdOn", width: 120, filter: true, cellRenderer: DateComponent },
    { field: "status", width: 120, filter: true, cellRenderer: statusRenderer },
    // { field: "Action", width: 100, cellRenderer: CustomActionComponent },
  ]);

  const bindApplications = async () => {
    if (searchTerm.length === 0 || searchTerm.length > 2) {
      const data = {
        pageNumber: 1,
        pageSize: 1000,
        batchId: loanBatch.id
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
    }
  };

  const gridRef = useRef<any>();
  //const [selectedRows, setSelectedRows] = useState<LoanApplication[]>([]);
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

  const getSelectedRows = async () => {
    const selectedRows = gridRef?.current.api.getSelectedRows();
    if (selectedRows !== null && selectedAction !== null) {
      const toastId = toast.loading("Please wait...");
      try {

        const response = await loanApplicationService.putLoanApplicationStatusData(selectedAction, selectedRows);
        if (response)  // Format the `createdOn` field
        {
          toastNotify(toastId, "Success");

        }
        else {
          toast.error("Something went wrong");
        }
      }
      catch (error) {
        toast.error("An error occurred");
      } finally {
        toast.dismiss(toastId);
        // Wait for 2 seconds before reloading the page
        setTimeout(() => {
          window.location.reload();
        }, 2000);  // 2000 milliseconds = 2 seconds
      }
    }

  };

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

  useEffect(() => {
    bindApplications();
    bindActions();
  }, [searchTerm]);

  useEffect(() => {
    bindApplications();
    bindActions();
  }, [showConfirmBox, showItemBox]);

  return (
    <Content>
      {true ? <KTCard className='shadow'>
        {/* begin::Header */}
        <div className='card-header border-0 py-5'>
          <h3 className='card-title align-items-start flex-column'>
            <span className='card-label fw-bold fs-4 mb-1'>Loan Applications</span>
            {/* <span className='text-muted fw-semibold fs-7'>More than 500 new orders</span> */}
          </h3>

          {/* begin::Toolbar */}
          <div className='card-toolbar flex' data-kt-buttons='true'>
            <label className='fs-6 fw-bold mb-3 mx-2'>Filter</label>
            <select
              className='form-control w-200px mx-2'
              onChange={handleSearchChange}>

              {actions && actions.map((item: any) => (
                <option key={item.id} value={item.id}>
                  {item.name}
                </option>
              ))}
            </select>

            <label className='fs-6 fw-bold mb-3 mx-2'>Actions</label>
            <select
              className='form-control w-200px mx-2'
              value={selectedStatusId}
              onChange={(e) => setSelectedStatusId(e.target.value)
              }>

              {actions && actions.map((item: any) => (
                <option key={item.id} value={item.id}>
                  {item.name}
                </option>
              ))}
            </select>
            <button
              type="button"
              className='btn btn-sm btn-color-muted btn-active btn-active-primary active px-4 me-3 mx-2 w-100px'
              onClick={handleApplyClick}>
              {!loading && <span className='indicator-label'>Apply</span>}
              {loading && (
                <span className='indicator-progress' style={{ display: 'block' }}>
                  Please wait...
                  <span className='spinner-border spinner-border-sm align-middle ms-2'></span>
                </span>
              )}
            </button>
            <button className="btn btn-sm btn-secondary" onClick={handleExcelExport}>
              <KTIcon iconName='exit-down' />
              Download Excel</button>
          </div>
          {/* end::Toolbar */}
        </div>
        <div className="px-3">
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
      </KTCard> : <Error401 />}

      {showItemBox && loanBatch?.statusId != 4 && (
        <LoanApplicationsModal afterConfirm={afterConfirm}
          isAdd={false}
          reloadApplications={window.location.reload()}
        />
      )}

    </Content>
  )
}

export default ListApplications;