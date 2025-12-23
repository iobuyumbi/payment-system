/* eslint-disable @typescript-eslint/no-explicit-any */
/* eslint-disable @typescript-eslint/no-unused-vars */
import CustomTable from "../../../_shared/CustomTable/Index";
import { useCallback, useState } from "react";
import { Content } from "../../../_metronic/layout/components/content";
import { PageLink, PageTitle } from "../../../_metronic/layout/core";
import { PageTitleWrapper } from "../../../_metronic/layout/components/toolbar/page-title";
import moment from "moment";
import { useEffect } from "react";
import { IConfirmModel } from "../../../_models/confirm-model";
import { ConfirmBox } from "../../../_shared/Modals/ConfirmBox";
import { KTCard } from "../../../_metronic/helpers";
import LoanBatchService from "../../../services/LoanBatchService";
import { Link, useNavigate } from "react-router-dom";
//import { useDispatch } from "react-redux";
//import { addToLoanBatch } from "../../../_features/loanbatch/loanBatchSlice";
import { isAllowed } from "../../../_metronic/helpers/ApiUtil";
import { Error401 } from "../errors/components/Error401";
import * as XLSX from "xlsx";
import { saveAs } from "file-saver";
import CustomActionComponent from "../../../_shared/CustomActionComponent";
import KeyMetrics from "../reports/_shared/KeyMetrics";
import AuditListModal from "../../../_shared/Modals/AuditListModal";
import { StatusBadge } from "../../../_shared/Status/StatusBadge";

const loanBatchService = new LoanBatchService();
const profileBreadCrumbs: Array<PageLink> = [
  {
    title: "Dashboard",
    path: "/dashboard",
    isSeparator: false,
    isActive: true,
  },
  {
    title: "",
    path: "",
    isSeparator: true,
    isActive: true,
  },
];

export const ListLoanBatches: React.FC = (props: any) => {
  const [rowData, setRowData] = useState<any>();
  const [searchTerm, setSearchTerm] = useState<string>("");
  const [confirmModel, setConfirmModel] = useState<IConfirmModel>();
  const [showConfirmBox, setShowConfirmBox] = useState<boolean>(false);
  const [loading, setLoading] = useState(false);
  const [showAuditModal, setShowAuditModal] = useState(false);
  const [currentId, setCurrentId] = useState("");
  const [pageData, setPageData] = useState<{ title: string; value: number }[]>([]);
  const navigate = useNavigate();
  //const dispatch = useDispatch();

  const editButtonHandler = (value: any) => {
    //dispatch(addToLoanBatch(value));
    navigate(`/batches/edit/${value.id}`);
  };

  const afterConfirm = (res: any) => {
    if (res) bindBatches();
    setShowConfirmBox(false);
    setShowAuditModal(false);
  };

  const openDeleteModal = (id: any) => {
    if (id) {
      const confirmModel: IConfirmModel = {
        title: "Delete Loan Product",
        btnText: "Delete this Loan Product?",
        deleteUrl: `api/loanBatch/${id}`,
        message: "delete-loan-batch",
      };

      setConfirmModel(confirmModel);
      setTimeout(() => {
        setShowConfirmBox(true);
      }, 500);
    }
  };

  // const CustomActionComponent = (props: any) => {
  //   return (
  //     <div className="d-flex flex-row">
  //       {isAllowed('loan.edit') && <button
  //         className="btn btn-default mx-0 px-1"
  //         onClick={() => editButtonHandler(props.data)}
  //       >
  //         <KTIcon iconName="pencil" iconType="outline" className="fs-4 text-gray-600" />
  //       </button>
  //       }

  //       {isAllowed('loan.delete') && <button
  //         className="btn btn-default mx-0 px-1"
  //         onClick={() => openDeleteModal(props.data.id)}
  //       >
  //         <KTIcon
  //           iconName="trash"
  //           iconType="outline"
  //           className="text-danger fs-4"
  //         />
  //       </button>
  //       }
  //     </div>
  //   );
  // };

  const LoanItemActionComponent = (props: any) => {
    return (
      // <div className="d-flex flex-row link-primary" onClick={() => loanItemClickHandler(props.data)}>
      //   Manage
      // </div>
      <Link to={"/batches/items"} state={props.data} className="link-primary">
        Manage
      </Link>
    );
  };

  const initiationDateRenderer = (props: any) => {
    const dateValue = props.data.initiationDate;
    if (!dateValue) return "";
    return moment(dateValue).format('YYYY-MM-DD HH:mm');
  };
  const createdOnRenderer = (props: any) => {
    const dateValue = props.data.createdOn;
    if (!dateValue) return "";
    return moment(dateValue).format('YYYY-MM-DD HH:mm');
  };



  const titleClickHandler = (value: any) => {
    //dispatch(addToLoanBatch(value));
    navigate(`/loan-batch-details/${value.id}/loan-items`);
  };

  const detailsTitleComponent = (props: any) => {
    return (
      <div className="d-flex flex-row">
        <a
          className="link-primary cursor-pointer"
          onClick={() => titleClickHandler(props.data)}
        >
          {props.data.name}
        </a>
      </div>
    );
  };

  const statusComponent = (props: any) => {
    return (
      <StatusBadge value={props.data.statusId} />
    );
  };

  const handleExport = () => {
    const workbook = XLSX.utils.book_new();

    // Define custom headers
    //const customHeaders = ['Full Name', 'Age', 'Email Address'];

    // Map data to custom headers
    const mappedData = rowData.map((row: any) => ({
      "Batch name": row.name,
      "Interest rate": row.interestRate,
      "Initiation date": row.initiationDate,
      Status:
        row.statusId == 1
          ? "Open"
          : row.statusId == 2
            ? "In Review"
            : row.statusId == 3
              ? "Accepting Applications"
              : row.statusId == 4
                ? "Closed"
                : "",
      "Payment terms": row.paymentTerms,
      Project: row.project.projectName,
      Address: row.address,
      "Project code": row.project.projectCode,
    }));

    const worksheet = XLSX.utils.json_to_sheet(mappedData);
    XLSX.utils.book_append_sheet(workbook, worksheet, "Sheet1");
    const blob = new Blob(
      [XLSX.write(workbook, { bookType: "xlsx", type: "array" })],
      {
        type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
      }
    );
    saveAs(blob, "Loan_Batches.xlsx");
  };

  const [colDefs] = useState<any>([
    {
      field: "name",
      flex: 1.5,
      cellRenderer: detailsTitleComponent,
      filter: true,
    },
    {
      headerName: "Project",
      field: "project.projectName",
      flex: 1,
      filter: true,
    },
    { field: "initiationDate", flex: 1, cellRenderer: initiationDateRenderer },
    { field: "createdOn", flex: 1, cellRenderer: createdOnRenderer },
    {
      headerName: "Status",
      field: "statusId",
      flex: 1.5,
      cellRenderer: statusComponent
      // valueFormatter: function (params: any) {
      //   return params.value == 1
      //     ? "Draft"
      //     : params.value == 2
      //       ? "In Review"
      //       : params.value == 3
      //         ? "Accepting Applications"
      //         : params.value == 4
      //           ? "Closed"
      //           : "";
      // },
    },
    {
      headerName: "AuditLog", flex: 1, cellRenderer: (props: any) => {
        isAllowed('loans.batch.view-audit') &&
          (
            <button
              onClick={() => {
                setShowAuditModal(true);
                setCurrentId(props.data.id);
              }}
              className="btn btn-default link-primary">
              Audit Log
            </button>
          )
      }
    },
    // { headerName: 'Loan items', flex: 1, cellRenderer: LoanItemActionComponent },
    {
      headerName: "Action",
      flex: 1,
      cellRenderer: (props: any) => (

        (props.data.totalApplications == 0 && <CustomActionComponent
          id={props.data.id}
          editPath={`/batches/edit`}
          onEditClick={() => { }}
          onDelete={openDeleteModal}
          isEditAllowed={isAllowed("loans.batch.edit")}
          isDeletedAllowed={isAllowed("loans.batch.delete")}
        />
        )
      ),
    },
  ]);

  const bindBatches = async () => {
    if (searchTerm.length === 0 || searchTerm.length > 2) {
      const data: any = {
        pageNumber: 1,
        pageSize: 10000,
        countryId: null,
      };
      const response = await loanBatchService.getLoanBatchData(data);
      console.log("Loan Batches Response", response);
      setRowData(response);
    }
  };

    const fetchBatchData = useCallback(async (page: number, pageSize: number) => {
      try {
          const searchParams: any = {
            pageNumber: page,
            pageSize: pageSize,
            
             countryId: null,
          };
        const response = await loanBatchService.getLoanBatchPagedData(searchParams);
        const data = await response;

      
        const transformedData = {
          rows: data.result.loanBatches, 
          totalRows:  data.result.loanCounts.totalBatches 
        };
      //  setStats(data?.farmerStats);
        console.log("Transformed data for grid:", transformedData);
  setPageData([
        { title: "TotalProducts", value: data.result.loanCounts.totalBatches ??  0 },
        {
          title: "Draft",
          value: data.result.loanBatches.filter((item: any) => item.statusId === 1).length,
        },
        {
          title: "In Review",
          value: data.result.loanBatches.filter((item: any) => item.statusId === 2).length,
        },
        {
          title: "Accepting Applications",
          value: data.result.loanBatches.filter((item: any) => item.statusId === 3).length,
        },
        {
          title: "Closed",
          value: data.result.loanBatches.filter((item: any) => item.statusId === 4).length,
        },
      ]);

        return transformedData;
      } catch (error) {
        console.error("Failed to fetch farmers data:", error);
        return { rows: [], totalRows: 0 }; 
      }
    }, []);

  useEffect(() => {
    document.title = "Loan Products - SDD";
    bindBatches();
  }, [searchTerm]);




  return (
    <Content>
      {isAllowed("loans.batch.view") ? (
        <>
          {" "}
          <PageTitleWrapper />
          <PageTitle breadcrumbs={profileBreadCrumbs}>Loan Products </PageTitle>
          {pageData && (
            <KeyMetrics keyMetrics={pageData} className="shadow my-3" />
          )}
          <KTCard className="shadow mt-10">
            <CustomTable
              rowData={rowData}
              colDefs={colDefs}
              header="Loan Products"
              addBtnText={isAllowed("loans.batch.add") ? "Add Loan Product" : ""}
              searchTerm={searchTerm}
              setSearchTerm={setSearchTerm}
              addBtnLink={isAllowed("loans.batch.add") ? "/batches/add" : ""}
              showExport={isAllowed("loans.batch.export")}
              handleExport={handleExport}
              serverSidePagination={true}
              fetchDataFunc={fetchBatchData}
             
            />
          </KTCard>
          {showConfirmBox && (
            <ConfirmBox
              confirmModel={confirmModel}
              afterConfirm={afterConfirm}
              loading={loading}
            />
          )}
          {showAuditModal && (
            <AuditListModal
              exModule={"LoanBatch"}
              componentId={currentId}
              onClose={afterConfirm}
            />
          )}
        </>
      ) : (
        <Error401 />
      )}
    </Content>
  );
};
