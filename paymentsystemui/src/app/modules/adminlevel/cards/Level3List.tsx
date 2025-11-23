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

const AdminLevel3List: FC<any> = (props: any) => {
  const navigate = useNavigate();
  const { setWardData, subCountyId } = props

  const [rowData, setRowData] = useState<any>();
  const [searchTerm, setSearchTerm] = useState<any>();
  const [confirmModel, setConfirmModel] = useState<IConfirmModel>();
  const [showConfirmBox, setShowConfirmBox] = useState<boolean>(false);
  const [loading, setLoading] = useState(false);

  const linkButtonHandler = (value: any) => {
    navigate("/adminlevel/adminlevel1");
  };

  const editButtonHandler = (value: string) => {
    setWardData(value)
  };

  const afterConfirm = (res: any) => {
    if (res) bindAdminLevelData(subCountyId);
    setShowConfirmBox(false);
      window.location.reload();
  };

  const openDeleteModal = (id: any) => {
    if (id) {
      const confirmModel: IConfirmModel = {
        title: "Delete admin level",
        btnText: "Delete this admin level?",
        deleteUrl: `api/adminlevel3/${id}`,
        message: "delete-ward",
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
    { field: "wardName", headerName: "Admin level name", flex: 1 },
    { field: "wardCode", headerName: "Admin level code", flex: 1 },
     { field: "subCountyName", flex: 1 },
    { field: "Action", flex: 1, cellRenderer: CustomActionComponent },
  ]);

  const bindAdminLevelData = async (props: any) => {
    if (props !== undefined) {
      const pageData = {
        pageNumber: 1,
        pageSize: 10000,
        filter: "",
        subCountyId: props
      };
      const response = await adminLevelService.getAdminLevel3Data(
        pageData
      );
      setRowData(response);
    }
  }

  useEffect(() => {
    bindAdminLevelData(subCountyId);
  }, [subCountyId]);

  
  useEffect(() => {
    bindAdminLevelData(null);
  }, []);

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
export default AdminLevel3List;
