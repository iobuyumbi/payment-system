import { FC } from "react";
import { useNavigate } from "react-router-dom";
import { useState } from "react";
import { Content } from "../../../_metronic/layout/components/content";
import { PageTitle, PageLink } from "../../../_metronic/layout/core";
import { PageTitleWrapper } from "../../../_metronic/layout/components/toolbar/page-title";
import CustomTable from "../../../_shared/CustomTable/Index";
import { ConfirmBox } from "../../../_shared/Modals/ConfirmBox";
import { IConfirmModel } from "../../../_models/confirm-model";
import { KTIcon } from "../../../_metronic/helpers";
import { KTCard } from "../../../_metronic/helpers";
import { useEffect } from "react";
import CategoryService from "../../../services/CategoryService";
import { useDispatch } from "react-redux";
import { addToCategories } from "../../../_features/categories/categorySlice";
import { isAllowed } from "../../../_metronic/helpers/ApiUtil";
import { Error401 } from "../errors/components/Error401";

const categoryService = new CategoryService();
const profileBreadCrumbs: Array<PageLink> = [
  {
    title: "Loans",
    path: "/loans",
    isSeparator: false,
    isActive: true,
  },
];
const ListItemCategory: FC = (props: any) => {
  const { showSpinner, hideSpinner } = props;

  const [rowData, setRowData] = useState<any>();
  const [searchTerm, setSearchTerm] = useState<string>("");
  const [confirmModel, setConfirmModel] = useState<IConfirmModel>();
  const [showConfirmBox, setShowConfirmBox] = useState<boolean>(false);
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();
  const dispatch = useDispatch();

  const editButtonHandler = (value: any) => {
    navigate(`/categories/edit/${value.id}`);
    dispatch(addToCategories(value));
  };

  const afterConfirm = (res: any) => {
    if (res) bindItems();
    setShowConfirmBox(false);
  };

  const openDeleteModal = (id: any) => {
    if (id) {
      const confirmModel: IConfirmModel = {
        title: "Delete category",
        btnText: "Delete this category?",
        deleteUrl: `api/ItemCategory/${id}`,
        message: "delete-itemCategory",
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
        {/* Edit */}
        {isAllowed('settings.loans.categories.edit') &&
          <button
            className="btn btn-default mx-0 px-1"
            onClick={() => editButtonHandler(props.data)}
          >
            <KTIcon iconName="pencil" iconType="outline" className="fs-4" />
          </button>
        }

        {/* Delete */}
        {isAllowed('settings.loans.categories.delete') &&
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

  const [colDefs, setColDefs] = useState<any>([
    { field: "name", headerName: "Category name", flex: 1 },
     { field: "itemCount", flex: 1 },

    { field: "Action", flex: 1, cellRenderer: CustomActionComponent },
  ]);

  const bindItems = async () => {
    if (searchTerm.length === 0 || searchTerm.length > 2) {
      const data: any = {
        pageNumber: 1,
        pageSize: 10,
      };

      const response = await categoryService.getItemCategoryData(data);
      
      setRowData(response);
    }
  };

  useEffect(() => {
    bindItems();
  }, [searchTerm]);

  return (
    <>
      <Content>
        {isAllowed("settings.loans.categories.view") ? (
          <>
            {" "}
            <PageTitleWrapper />
            <PageTitle breadcrumbs={profileBreadCrumbs}>
              Loan Item Categories{" "}
            </PageTitle>
            <KTCard className="shadow mt-10">
              <CustomTable
                rowData={rowData}
                colDefs={colDefs}
                header="Item category"
                addBtnText={
                  isAllowed("settings.loans.categories.add") ? "Add item category" : ""
                }
                searchTerm={searchTerm}
                setSearchTerm={setSearchTerm}
                addBtnLink={
                  isAllowed("settings.loans.categories.add") ? "/categories/add" : ""
                }
              />
            </KTCard>
          </>
        ) : (
          <Error401 />
        )}
        {showConfirmBox && (
          <ConfirmBox
            confirmModel={confirmModel}
            afterConfirm={afterConfirm}
            loading={loading}
          />
        )}
      </Content>
    </>
  );
};
export { ListItemCategory };
