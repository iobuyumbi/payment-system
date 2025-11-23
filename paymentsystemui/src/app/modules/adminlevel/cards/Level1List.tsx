import { FC } from "react";
import { KTCardBody, KTIcon } from "../../../../_metronic/helpers";
import { KTCard } from "../../../../_metronic/helpers";
import CustomTable from "../../../../_shared/CustomTable/Index";
import { ConfirmBox } from "../../../../_shared/Modals/ConfirmBox";
import AdminLevelService from "../../../../services/AdminLevelService";
import { useNavigate } from "react-router-dom";
import { useState } from "react";
import { IConfirmModel } from "../../../../_models/confirm-model";
import { useEffect } from "react";
import { isAllowed } from "../../../../_metronic/helpers/ApiUtil";

const adminLevelService = new AdminLevelService();

const AdminLevel1List: FC<any> = (props: any) => {
  const { setCountyData, countryId } = props;
  const navigate = useNavigate();

  const [rowData, setRowData] = useState<any>();

  const [confirmModel, setConfirmModel] = useState<IConfirmModel>();
  const [showConfirmBox, setShowConfirmBox] = useState<boolean>(false);
  const [loading, setLoading] = useState(false);

  const linkButtonHandler = (value: any) => {
    navigate("/adminlevel/adminlevel1");
  };

  const editButtonHandler = (value: any) => {
    setCountyData(value);
  };

  const afterConfirm = (res: any) => {
    if (res) bindAdminLevelData(countryId);
    setShowConfirmBox(false);
  };

  const openDeleteModal = (id: any) => {
    if (id) {
      const confirmModel: IConfirmModel = {
        title: "Delete admin level",
        btnText: "Delete this admin level?",
        deleteUrl: `api/adminlevel1/${id}`,
        message: "delete-county",
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
        {isAllowed("settings.administrative.edit") && (
          <button
            className="btn btn-default mx-0 px-1"
            onClick={() => editButtonHandler(props.data)}
          >
            <KTIcon
              iconName="pencil"
              iconType="outline"
              className="fs-4 text-gray-600"
            />
          </button>
        )}

        {/* Delete */}
        {isAllowed("settings.administrative.delete") && (
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
        )}
      </div>
    );
  };

  const [colDefs, setColDefs] = useState<any>([
    { field: "countyName", headerName: "Admin level name", flex: 1 },
    { field: "countyCode", headerName: "Admin level code", flex: 1 },
    { field: "countryName", flex: 1 },
    { field: "Action", flex: 1, cellRenderer: CustomActionComponent },
  ]);

  const bindAdminLevelData = async (props: any) => {
    if (props !== undefined) {
      const pageData = {
        pageNumber: 1,
        pageSize: 10000,
        filter: "",
        countryId: props,
      };
      const response = await adminLevelService.getAdminLevel1Data(pageData);
      setRowData(response);
      console.log("Admin Level 1 Data", response);
    }
  };

  useEffect(() => {
    bindAdminLevelData(countryId);
  }, [countryId]);

  useEffect(() => {
    bindAdminLevelData(null);
  }, []);

  return (
    <>
      <KTCard className="shadow  mt-10">
        <KTCardBody>
          <CustomTable rowData={rowData} colDefs={colDefs} header="" />
        </KTCardBody>
      </KTCard>
      {showConfirmBox && (
        <ConfirmBox
          confirmModel={confirmModel}
          afterConfirm={afterConfirm}
          loading={loading}
        />
      )}
    </>
  );
};
export default AdminLevel1List;
