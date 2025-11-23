import { FC } from "react";
import { useState, useEffect } from "react";
import { Content } from "../../../_metronic/layout/components/content";
import { PageTitle, PageLink } from "../../../_metronic/layout/core";
import { PageTitleWrapper } from "../../../_metronic/layout/components/toolbar/page-title";
import CustomTable from "../../../_shared/CustomTable/Index";
import CooperativeService from "../../../services/CooperativeService";
import { ConfirmBox } from "../../../_shared/Modals/ConfirmBox";
import { IConfirmModel } from "../../../_models/confirm-model";
import { KTIcon } from "../../../_metronic/helpers";
import { KTCard } from "../../../_metronic/helpers";
import { useDispatch } from "react-redux";
import { useNavigate } from "react-router-dom";
import { addToCooperatives } from "../../../_features/cooperatives/cooperativeSlice";
import { isAllowed } from "../../../_metronic/helpers/ApiUtil";
import { Error401 } from "../errors/components/Error401";

const cooperativeService = new CooperativeService();

const profileBreadCrumbs: Array<PageLink> = [
  {
    title: "Cooperatives",
    path: "/cooperatives",
    isSeparator: false,
    isActive: true,
  },
];

const ListCooperatives: FC = (props: any) => {
  const navigate = useNavigate();
  const dispatch = useDispatch();

  const [rowData, setRowData] = useState<any>();
  const [searchTerm, setSearchTerm] = useState<string>("");
  const [confirmModel, setConfirmModel] = useState<IConfirmModel>();
  const [showConfirmBox, setShowConfirmBox] = useState<boolean>(false);
  const [loading, setLoading] = useState(false);

  const editButtonHandler = (value: any) => {
    dispatch(addToCooperatives(value));
    navigate(`/cooperatives/edit/${value.id}`);
  };

  const afterConfirm = (res: any) => {
    if (res) bindCooperatives();
    setShowConfirmBox(false);
  };

  const openDeleteModal = (id: any) => {
    if (id) {
      const confirmModel: IConfirmModel = {
        title: "Delete Cooperative",
        btnText: "Delete this Cooperative?",
        deleteUrl: `api/cooperative/${id}`,
        message: "delete-cooperative",
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
        {isAllowed('settings.cooperatives.edit') &&
          <button
            className="btn btn-default mx-0 px-1"
            onClick={() => editButtonHandler(props.data)}>
            <KTIcon iconName="pencil" iconType="outline" className="fs-4" />
          </button>
        }

        {/* Delete */}
        {isAllowed('settings.cooperatives.delete') &&
          <button
            className="btn btn-default mx-0 px-1"
            onClick={() => openDeleteModal(props.data.id)}>
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
    { field: "name", flex: 1 },
    { field: "country.countryName", flex: 1, headerName: "Country" },
    { field: "description", flex: 2 },
    { field: "Action", flex: 1, cellRenderer: CustomActionComponent },
  ]);

  const bindCooperatives = async () => {
    if (searchTerm.length === 0 || searchTerm.length > 2) {
      const data: any = {
        pageNumber: 1,
        pageSize: 10000,
      };

      const response = await cooperativeService.getCooperativeData(data);
      setRowData(response);
    }
  };

  useEffect(() => {
    bindCooperatives();
  }, [searchTerm]);

  return (
    <>
      <Content>
        {isAllowed("settings.cooperatives.view") ? (
          <>
            <PageTitleWrapper />
            <PageTitle breadcrumbs={profileBreadCrumbs}>

              Cooperatives
            </PageTitle>
            <KTCard className="shadow mt-10">
              <CustomTable
                rowData={rowData}
                colDefs={colDefs}
                header="cooperatives"
                addBtnText={
                  isAllowed("settings.cooperatives.add") ? "Add cooperative" : ""
                }
                searchTerm={searchTerm}
                setSearchTerm={setSearchTerm}
                addBtnLink={
                  isAllowed("settings.cooperatives.add") ? "/cooperatives/add" : ""
                }
              />
            </KTCard>
            {showConfirmBox && (
              <ConfirmBox
                confirmModel={confirmModel}
                afterConfirm={afterConfirm}
                loading={loading}
              />
            )}
          </>
        ) : (
          <Error401 />
        )}
      </Content>
    </>
  );
};
export { ListCooperatives };
