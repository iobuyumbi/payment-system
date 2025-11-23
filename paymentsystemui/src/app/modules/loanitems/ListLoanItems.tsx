import { FC } from 'react'
import { useNavigate } from 'react-router-dom';
import { useState } from 'react';
import { Content } from '../../../_metronic/layout/components/content';
import { PageTitle, PageLink } from "../../../_metronic/layout/core";
import { PageTitleWrapper } from '../../../_metronic/layout/components/toolbar/page-title';
import CustomTable from '../../../_shared/CustomTable/Index'
import LoanItemService from '../../../services/LoanItemService';
import { ConfirmBox } from '../../../_shared/Modals/ConfirmBox';
import { IConfirmModel } from '../../../_models/confirm-model';
import { KTIcon } from '../../../_metronic/helpers';
import { KTCard } from '../../../_metronic/helpers';
import { useEffect } from 'react';
import { addToLoanItems } from '../../../_features/loanitem/loanItemSlice';
import { useDispatch } from 'react-redux';
import { isAllowed } from '../../../_metronic/helpers/ApiUtil';
import { Error401 } from '../errors/components/Error401';
import moment from 'moment';
import config from '../../../environments/config';

const loanItemService = new LoanItemService();
const profileBreadCrumbs: Array<PageLink> = [
  {
    title: "Dashbaoard",
    path: "/dashboard",
    isSeparator: false,
    isActive: true,
  },
  {
    title: "",
    path: "",
    isSeparator: true,
    isActive: false,
  },
]

const ListLoanItems: FC = (props: any) => {
  const navigate = useNavigate();
  const dispatch = useDispatch();
  const { showSpinner, hideSpinner } = props;

  const [rowData, setRowData] = useState<any>();
  const [searchTerm, setSearchTerm] = useState<string>('');
  const [confirmModel, setConfirmModel] = useState<IConfirmModel>();
  const [showConfirmBox, setShowConfirmBox] = useState<boolean>(false);
  const [loading, setLoading] = useState(false);

  const editButtonHandler = (value: any) => {
    dispatch(addToLoanItems(value));
    navigate(`/items/edit/${value.id}`)
  }

  const afterConfirm = (res: any) => {
    if (res) bindItems()
    setShowConfirmBox(false)
  }

  const openDeleteModal = (id: any) => {
    if (id) {
      const confirmModel: IConfirmModel = {
        title: 'Delete Item',
        btnText: 'Delete this item?',
        deleteUrl: `api/MasterLoanItem/${id}`,
        message: 'delete-masterLoanItem',
      }

      setConfirmModel(confirmModel)
      setTimeout(() => {
        setShowConfirmBox(true)
      }, 500)
    }
  }

  const CustomActionComponent = (props: any) => {
    return <div className="d-flex flex-row">
      {/* Edit */}
      {isAllowed('settings.loans.items.edit') &&
        <button className="btn btn-default mx-0 px-1" onClick={() => editButtonHandler(props.data)}>
          <KTIcon iconName="pencil" iconType="outline" className="fs-4" />
        </button>
      }

      {/* Delete */}
      {isAllowed('settings.loans.items.delete') &&
        <button className="btn btn-default mx-0 px-1" onClick={() => openDeleteModal(props.data.id)}>
          <KTIcon iconName="trash" iconType="outline" className="text-danger fs-4" />
        </button>
      }
    </div>;
  };

  const [colDefs, setColDefs] = useState<any>([
    { field: "itemName", flex: 1 },
    { field: "category.name", flex: 1 },
    {
      field: "createdOn", flex: 1, valueFormatter: function (params: any) {
        return params.value ? moment(params.value).format("YYYY-MM-DD HH:mm") : null;
      },
    },

    { field: "Action", flex: 1, cellRenderer: CustomActionComponent },
  ]);

  const bindItems = async () => {
    if (searchTerm.length === 0 || searchTerm.length > 2) {
      const data: any = {
        pageNumber: 1,
        pageSize: 10,
      };

      const response = await loanItemService.getMasterLoanItemData(data);

      setRowData(response);
    }
  }

  useEffect(() => {
    bindItems();
  }, [searchTerm]);


  return (
    <Content>
      {isAllowed('settings.loans.items.view') ? <> <PageTitleWrapper />
        <PageTitle breadcrumbs={profileBreadCrumbs}> Loan Items </PageTitle>
        <KTCard className='mt-10 shadow'>
          <CustomTable
            rowData={rowData}
            colDefs={colDefs}
            header="loan items"
            addBtnText={isAllowed('settings.loans.items.add') ? "Add loan item" : ""}
            searchTerm={searchTerm}
            setSearchTerm={setSearchTerm}
            addBtnLink={isAllowed('settings.loans.items.add') ? "/items/add" : ""}
          />
        </KTCard></>
        : <Error401 />}
      {showConfirmBox && <ConfirmBox confirmModel={confirmModel} afterConfirm={afterConfirm} loading={loading} />}
    </Content>
  );
}

export { ListLoanItems }


