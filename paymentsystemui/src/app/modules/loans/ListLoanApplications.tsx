/* eslint-disable @typescript-eslint/no-explicit-any */
/* eslint-disable @typescript-eslint/no-unused-vars */
import CustomTable from '../../../_shared/CustomTable/Index'
import { useState } from 'react';
import { Content } from '../../../_metronic/layout/components/content';
import { PageLink, PageTitle } from '../../../_metronic/layout/core';
import { PageTitleWrapper } from '../../../_metronic/layout/components/toolbar/page-title';
import LoanApplicationService from '../../../services/LoanApplicationService';
import moment from 'moment';
import { IConfirmModel } from '../../../_models/confirm-model';
import { ConfirmBox } from '../../../_shared/Modals/ConfirmBox';
import { KTIcon } from '../../../_metronic/helpers';
import { useEffect } from 'react';
import { KTCard } from '../../../_metronic/helpers';
import { useNavigate } from 'react-router-dom';
import { useDispatch } from 'react-redux';
import { addToLoanApplication } from '../../../_features/loan/loanApplicationSlice';

const loanApplicationService = new LoanApplicationService();
const profileBreadCrumbs: Array<PageLink> = [
  {
    title: 'Loans',
    path: '/loans',
    isSeparator: false,
    isActive: true,
  },
]
export const ListLoanApplications: React.FC = (props: any) => {
  const navigate = useNavigate();
  const dispatch = useDispatch();

  const [rowData, setRowData] = useState<any>([]);
  const [searchTerm, setSearchTerm] = useState<string>('');
  const [confirmModel, setConfirmModel] = useState<IConfirmModel>();
  const [showConfirmBox, setShowConfirmBox] = useState<boolean>(false);
  const [loading, setLoading] = useState(false);

  const linkButtonHandler = (value: any) => {
    dispatch(addToLoanApplication(value))
    navigate('/loans/applications/detail')
  }

  const afterConfirm = (res: any) => {
    if (res) bindApplications()
    setShowConfirmBox(false)
  }

  const openDeleteModal = (id: any) => {
    if (id) {
      const confirmModel: IConfirmModel = {
        title: 'Delete loan application',
        btnText: 'Delete this application?',
        deleteUrl: `api/loanApplication/${id}`,
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
      {/* <button className="btn btn-default mx-0 px-1" onClick={() => editButtonHandler(props.data)}>
       <KTIcon iconName="eye" iconType="outline" className="fs-4" />
      </button> */}
      <button className="btn btn-default mx-0 px-1" onClick={() => openDeleteModal(props.data.id)}>
        <KTIcon iconName="trash" iconType="outline" className="text-danger fs-4" />
      </button>
    </div>;
  };

  const MainLinkActionComponent = (props: any) => {
    return <button className="btn btn-default link-primary mx-0 px-1" onClick={() => linkButtonHandler(props.data)}>
      {props.data.loanNumber}
    </button>;
  };

  const [colDefs, setColDefs] = useState<any>([
    { headerName: "Loan Account Number", flex: 1, cellRenderer: MainLinkActionComponent },
    { headerName: "Farmer name", field: "farmer.fullName", flex: 1 },
    { headerName: "Loan Product", field: "loanBatch.name", flex: 1 },
    { field: "Action", flex: 1, cellRenderer: CustomActionComponent },
  ]);

  const bindApplications = async () => {
    if (searchTerm.length === 0 || searchTerm.length > 2) {
      const data = {
        pageNumber: 1,
        pageSize: 10000,
      };
      const response = await loanApplicationService.getLoanApplicationData(data);
      if (response)  // Format the `createdOn` field
      {
        const formattedResponse = response.map(item => ({
          ...item,
          createdOn: moment(item.createdOn).format('YYYY-MM-DD hh:mm A'),
          updatedOn: item.updatedOn ? moment(item.updatedOn).format('YYYY-MM-DD hh:mm A') : 'N/A',
          dateOfWitness: item.dateOfWitness ? moment(item.dateOfWitness).format('YYYY-MM-DD hh:mm A') : 'N/A',
        }));
        setRowData(formattedResponse);
      }

    }
  };

  useEffect(() => {
    bindApplications();
  }, [searchTerm]);

  return (
    <Content>
      <PageTitleWrapper />
      <PageTitle breadcrumbs={profileBreadCrumbs}>Loan Applications </PageTitle>
      <KTCard className='shadow mt-10'>
        <CustomTable
          rowData={rowData}
          colDefs={colDefs}
          header="loan application"
          addBtnText={"Add loan application"}
          searchTerm={searchTerm}
          setSearchTerm={setSearchTerm}
          addBtnLink={"/imports"}
        />
      </KTCard>
      {showConfirmBox && <ConfirmBox confirmModel={confirmModel} afterConfirm={afterConfirm} loading={loading} />}
    </Content>
  )
}


