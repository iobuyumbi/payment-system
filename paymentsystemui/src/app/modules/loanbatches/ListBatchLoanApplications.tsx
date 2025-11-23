/* eslint-disable @typescript-eslint/no-explicit-any */
/* eslint-disable @typescript-eslint/no-unused-vars */
import CustomTable from '../../../_shared/CustomTable/Index'
import { useState } from 'react';
import { Content } from '../../../_metronic/layout/components/content';
import { PageLink } from '../../../_metronic/layout/core';
import LoanApplicationService from '../../../services/LoanApplicationService';
import moment from 'moment';
import { IConfirmModel } from '../../../_models/confirm-model';
import { ConfirmBox } from '../../../_shared/Modals/ConfirmBox';
import { KTIcon } from '../../../_metronic/helpers';
import { useEffect } from 'react';
import { KTCard } from '../../../_metronic/helpers';
import { useNavigate } from 'react-router-dom';
import { useDispatch, useSelector } from 'react-redux';
import { addToLoanApplication } from '../../../_features/loan/loanApplicationSlice';
import { isAllowed } from '../../../_metronic/helpers/ApiUtil';
import { Error401 } from '../errors/components/Error401';
import * as XLSX from "xlsx";
import saveAs from "file-saver";

const loanApplicationService = new LoanApplicationService();

const ListBatchLoanApplications = (props: any) => {
  const navigate = useNavigate();
  const dispatch = useDispatch();

  const [rowData, setRowData] = useState<any>();
  const [searchTerm, setSearchTerm] = useState<string>('');
  const [confirmModel, setConfirmModel] = useState<IConfirmModel>();
  const [showConfirmBox, setShowConfirmBox] = useState<boolean>(false);
  const [loading, setLoading] = useState(false);

  const linkButtonHandler = (value: any) => {
    dispatch(addToLoanApplication(value))
    navigate('/loans/applications/detail')
  }

  const loanBatch = useSelector((state: any) =>
    state?.loanBatches,
  )

  const afterConfirm = (res: any) => {
    if (res) bindApplications()
    setShowConfirmBox(false)
  }

  const openDeleteModal = (id: any) => {
    if (id) {
      const confirmModel: IConfirmModel = {
        title: 'Delete loan application',
        btnText: 'Delete this application?',
        deleteUrl: `api/LoanApplication/${id}`,
        message: 'delete-application',
      }

      setConfirmModel(confirmModel)
      setTimeout(() => {
        setShowConfirmBox(true)
      }, 500)
    }
  }

  const CustomActionComponent = (props: any) => {
    return <div className="d-flex flex-row">
      {isAllowed('loans.applications.delete') &&
        <button className="btn btn-default mx-0 px-1" onClick={() => openDeleteModal(props.data.id)}>
          <KTIcon iconName="trash" iconType="outline" className="text-danger fs-4" />
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
    return moment(dateValue).format('DD-MM-YYYY');
  };

  const [colDefs, setColDefs] = useState<any>([
    { headerName: "Loan account number", flex: 1, cellRenderer: MainLinkActionComponent },
    // { headerName: "Loan Product", field: "loanBatch.name", flex: 1 },
    { headerName: "Farmer name", field: "farmer.fullName", flex: 1 },
    { field: "totalValue", flex: 1 },
    { field: "dateOfWitness", flex: 1, cellRenderer: dateOfWitnessRenderer },
    { field: "Action", flex: 1, cellRenderer: CustomActionComponent },
  ]);
  
  const handleExcelExport = () => {
      const workbook = XLSX.utils.book_new();
  
      // Define custom headers
      //const customHeaders = ['Full Name', 'Age', 'Email Address'];
  
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
        "Tenure" : row.loanBatch.tenure,
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
      const timestamp = moment().format("DD-MM-YYYY HH-mm"); // Format: DD-MM-YYYY HH-mm
          saveAs(blob, `application_report_${timestamp}.xlsx`); 
    };
  
  const bindApplications = async () => {
    if (searchTerm.length === 0 || searchTerm.length > 2) {
      const data = {
        pageNumber: 1,
        pageSize: 10000,
        batchId: loanBatch.id
      };
      const response = await loanApplicationService.getLoanApplicationData(data);
      if (response)  // Format the `createdOn` field
      {
        const formattedResponse = response.map(item => ({
          ...item,
        }));
        setRowData(formattedResponse);
      } else {
        setRowData([]);
      }
    }
  };

  useEffect(() => {
    bindApplications();
  }, [searchTerm]);

  return (
    <Content>
      {isAllowed('loans.applications.view') ?
        <KTCard className='shadow mt-10'>
           <div className="d-flex justify-content-end p-3">
        {/* <button className ="btn btn-theme me-2">
          Download PDF
        </button> */}
        <button className="btn btn-secondary" onClick={handleExcelExport}>Download Excel</button>
      </div>
          <CustomTable
            rowData={rowData}
            colDefs={colDefs}
            header=""
          />
        </KTCard> : <Error401 />}
      {showConfirmBox && <ConfirmBox confirmModel={confirmModel} afterConfirm={afterConfirm} loading={loading} />}
    </Content>
  )
}

export default ListBatchLoanApplications;