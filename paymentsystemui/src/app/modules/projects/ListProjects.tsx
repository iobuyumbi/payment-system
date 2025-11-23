/* eslint-disable @typescript-eslint/no-explicit-any */
/* eslint-disable @typescript-eslint/no-unused-vars */
import CustomTable from "../../../_shared/CustomTable/Index";
import { useEffect } from "react";
import { useState } from "react";
import ProjectService from "../../../services/ProjectService";
import { Content } from "../../../_metronic/layout/components/content";
import { PageLink, PageTitle } from "../../../_metronic/layout/core";
import { PageTitleWrapper } from "../../../_metronic/layout/components/toolbar/page-title";
import { IConfirmModel } from "../../../_models/confirm-model";
import { ConfirmBox } from "../../../_shared/Modals/ConfirmBox";
import { KTCard } from "../../../_metronic/helpers";
import { useDispatch } from "react-redux";
import { useNavigate } from "react-router-dom";
import { KTIcon } from "../../../_metronic/helpers";
import {
  addToProjects,
  resetProjectState,
} from "../../../_features/projects/projectSlice";
import { filter } from "lodash";
import { isAllowed } from "../../../_metronic/helpers/ApiUtil";
import { Error401 } from "../errors/components/Error401";

const projectService = new ProjectService();
const profileBreadCrumbs: Array<PageLink> = [
  {
    title: "Projects",
    path: "/projects",
    isSeparator: false,
    isActive: true,
  },
];
export const ListProjects: React.FC = (props: any) => {
  const navigate = useNavigate();
  const dispatch = useDispatch();
  const [rowData, setRowData] = useState<any>();

  const [searchTerm, setSearchTerm] = useState<string>("");
  const [confirmModel, setConfirmModel] = useState<IConfirmModel>();
  const [showConfirmBox, setShowConfirmBox] = useState<boolean>(false);
  const [loading, setLoading] = useState(false);

  const editButtonHandler = (value: any) => {
    dispatch(addToProjects(value));
    navigate(`/projects/edit/${value.id}`);
  };

  const afterConfirm = (res: any) => {
    if (res) bindProjects();
    setShowConfirmBox(false);
  };

  const openDeleteModal = (id: any) => {
    if (id) {
      const confirmModel: IConfirmModel = {
        title: "Delete project",
        btnText: "Delete this project?",
        deleteUrl: `api/project/${id}`,
        message: "delete-project",
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
        {isAllowed("settings.projects.edit") &&
          <button
            className="btn btn-default mx-0 px-1"
            onClick={() => editButtonHandler(props.data)}
          >
            <KTIcon iconName="pencil" iconType="outline" className="fs-4" />
          </button>
        }

        {/* Delete */}
        {isAllowed("settings.projects.delete") &&
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
    { field: "projectName", flex: 1, filter: true },
    { field: "countryName", flex: 1, filter: true },
     { field: "projectCode", flex: 1, filter: true },
    { field: "Action", flex: 1, cellRenderer: CustomActionComponent },
  ]);

  const bindProjects = async () => {
    if (searchTerm.length === 0 || searchTerm.length > 2) {
      const data: any = {
        pageNumber: 1,
        pageSize: 10000,
        countryId: null,
      };
      const response = await projectService.getProjectByCountryData(data);
    
      setRowData(response);
    }
  };

  useEffect(() => {
    bindProjects();
    dispatch(resetProjectState());
  }, [searchTerm]);

  return (
    <Content>
      {isAllowed("settings.projects.view") ? (
        <>
          {" "}
          <PageTitleWrapper />
          <PageTitle breadcrumbs={profileBreadCrumbs}>Projects </PageTitle>
          <KTCard className="mt-10 shadow">
            <CustomTable
              rowData={rowData}
              colDefs={colDefs}
              header="projects"
              addBtnText={isAllowed("settings.projects.add") ? "Add project" : ""}
              searchTerm={searchTerm}
              setSearchTerm={setSearchTerm}
              addBtnLink={isAllowed("settings.projects.add") ? "/projects/add" : ""}
              showSearchBox={false}
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
  );
};
