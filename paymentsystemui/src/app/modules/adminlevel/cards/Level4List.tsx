import { KTIcon } from "../../../../_metronic/helpers";
import { KTCard } from "../../../../_metronic/helpers";
import CustomTable from "../../../../_shared/CustomTable/Index";
import { ConfirmBox } from "../../../../_shared/Modals/ConfirmBox";
import AdminLevelService from "../../../../services/AdminLevelService";
import { useNavigate } from "react-router-dom";
import { useState } from "react";
import { IConfirmModel } from "../../../../_models/confirm-model";
import { useEffect } from "react";
import { FC } from "react";
import { isAllowed } from "../../../../_metronic/helpers/ApiUtil";

const adminLevelService = new AdminLevelService();

const AdminLevel4List: FC<any> = (props: any) => {
  const { setVillageData, wardId } = props;
  const [rowData, setRowData] = useState<any>();
  const [confirmModel, setConfirmModel] = useState<IConfirmModel>();
  const [showConfirmBox, setShowConfirmBox] = useState<boolean>(false);
  const [loading, setLoading] = useState(false);

  const editButtonHandler = (value: string) => {
    setVillageData(value)
  };

  const afterConfirm = (res: any) => {
    if (res) bindAdminLevelData(wardId);
    setShowConfirmBox(false);
  };

  const openDeleteModal = (id: any) => {
    if (id) {
      const confirmModel: IConfirmModel = {
        title: "Delete admin level",
        btnText: "Delete this admin level?",
        deleteUrl: `api/adminlevel1/${id}`,
        message: "delete-application",
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
        {isAllowed('settings.administrative.edit') &&
          <button className="btn btn-default mx-0 px-1" onClick={() => editButtonHandler(props.data)}>
            <KTIcon iconName="pencil" iconType="outline" className="fs-4 text-gray-600" />
          </button>
        }

        {/* Delete */}
        {isAllowed('settings.administrative.delete') &&
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
    { field: "villageName", flex: 1 },
    { field: "villageCode", flex: 1 },
    { field: "Action", flex: 1, cellRenderer: CustomActionComponent },
  ]);

  const bindAdminLevelData = async (props: any) => {

    if (props !== undefined) {
      const response = await adminLevelService.getAdminLevel4Data(
        props
      );
      setRowData(response);
    }
  }

  useEffect(() => {
    bindAdminLevelData(wardId);
  }, [wardId]);

  return (
    <>
      <KTCard className="shadow mt-10">
        <CustomTable
          rowData={rowData}
          colDefs={colDefs}
          header=""
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
  );
};
export default AdminLevel4List;
