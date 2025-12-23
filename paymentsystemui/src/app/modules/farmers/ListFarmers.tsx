import CustomTable from "../../../_shared/CustomTable/Index";
import { useState, useEffect, useCallback } from "react";
import FarmerService from "../../../services/FarmerService";
import { Content } from "../../../_metronic/layout/components/content";
import { PageLink, PageTitle } from "../../../_metronic/layout/core";
import { PageTitleWrapper } from "../../../_metronic/layout/components/toolbar/page-title";
import { KTCard, KTCardBody, KTIcon } from "../../../_metronic/helpers";
import { IConfirmModel } from "../../../_models/confirm-model";
import { ConfirmBox } from "../../../_shared/Modals/ConfirmBox";
import { useDispatch } from "react-redux";
import { addTofarmers, resetFarmerState } from "../../../_features/farmers/farmerSlice";
import { useNavigate } from "react-router-dom";
import { saveAs } from "file-saver";
import * as XLSX from "xlsx";
import { isAllowed } from "../../../_metronic/helpers/ApiUtil";
import { Error401 } from "../errors/components/Error401";
import KeyMetrics from "../reports/_shared/KeyMetrics";
import AuditListModal from "../../../_shared/Modals/AuditListModal";
import { getSelectedCountryCode } from "../../../_metronic/helpers/AppUtil";
import moment from "moment";

const farmerService = new FarmerService();
const breadCrumbs: Array<PageLink> = [
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

export const ListFarmers: React.FC = (props: any) => {
  const navigate = useNavigate();
  const dispatch = useDispatch();

  // Row Data: The data to be displayed.
  const [rowData, setRowData] = useState<any>();
  const [stats, setStats] = useState<any>(null);
  const [searchTerm, setSearchTerm] = useState<string>("");
  const [showConfirmBox, setShowConfirmBox] = useState<boolean>(false);
  const [confirmModel, setConfirmModel] = useState<IConfirmModel>();
  const [loading, setLoading] = useState(false);
  const [showAuditModal, setShowAuditModal] = useState(false);
  const [currentId, setCurrentId] = useState("");

  const editButtonHandler = (value: any) => {
    dispatch(addTofarmers(value));
    navigate(`/farmers/edit/${value.id}`);
  };

  const titleClickHandler = (value: any) => {
    dispatch(addTofarmers(value));
    navigate(`/farmer-detail/loans`);
  };

  const openDeleteModal = (id: any) => {
    if (id) {
      const confirmModel: IConfirmModel = {
        title: "Delete farmer",
        btnText: "Delete this farmer?",
        deleteUrl: `api/farmer/${id}`,
        message: "delete-farmer",
      };

      setConfirmModel(confirmModel);
      setTimeout(() => {
        setShowConfirmBox(true);
      }, 500);
    }
  };

  const afterConfirm = (res: any) => {
    // if (res) bindFarmers(1, 10);
    setShowConfirmBox(false);
    setShowAuditModal(false);
  };

  const farmerTitleComponent = (props: any) => {
    return (
      <div className="d-flex flex-row">
        <a
          className="link-primary cursor-pointer"
          onClick={() => titleClickHandler(props.data)}
        >
          {props.data.systemId}
        </a>
      </div>
    );
  };

  const CustomActionComponent = (props: any) => {
    return (
      <div className="d-flex flex-row">
        {/* Audit log */}
        {isAllowed('farmers.view-audit') &&
          <button
            onClick={() => {
              setShowAuditModal(true);
              setCurrentId(props.data.id);
            }}
            className="btn btn-default link-primary"
          >
            Audit Log
          </button>
        }

        {/* Edit */}
        {isAllowed("farmers.edit") && (
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
        {isAllowed("farmers.delete") && (
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

  const handleExport = () => {
    const workbook = XLSX.utils.book_new();

    // Define custom headers
    //const customHeaders = ['Full Name', 'Age', 'Email Address'];

    // Map data to custom headers
    const mappedData = rowData.map((row: any) => ({
      "System Id": row.systemId,
      "First name": row.firstName,
      "Other names": row.otherNames,
      "Country": row.country.countryName,
      "Mobile": row.mobile,
      "Payment phone number": row.paymentPhoneNumber,
      "Access to mobile": row.accessToMobile,
      "County/Region": row.adminLevel1.countyName,
      "Sub-County/District": row.adminLevel2.subCountyName,
      "Ward/Sub-County/County": row.adminLevel3.wardName,
      "Village": row.village,
      "Alternate contact number": row.alternateContactNumber,
      "Beneficiary id": row.beneficiaryId,
      "Birth month": row.birthMonth,
      "Birth year": row.birthYear,
      "Cooperative": row.cooperativeName,
      "Email": row.email,
      "Enumeration date": row.enumerationDate,
      "Gender": row.gender === 1 ? "Male" : row.gender === 2 ? "Female" : "Other",
      "Has disability": row.hasDisability,
      "Is farmer phone owner?": row.isFarmerPhoneOwner,
      "Participant id": row.participantId,
      "Phone owner address": row.phoneOwnerAddress,
      "Phone owner name": row.phoneOwnerName,
      "Phone owner national id": row.phoneOwnerNationalId,
    }));

    const worksheet = XLSX.utils.json_to_sheet(mappedData);
    XLSX.utils.book_append_sheet(workbook, worksheet, "Sheet1");
    const blob = new Blob(
      [XLSX.write(workbook, { bookType: "xlsx", type: "array" })],
      {
        type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
      }
    );

    const timestamp = moment().format("DD-MM-YYYY HH-mm");
    saveAs(blob, `Farmers_${timestamp}.xlsx`);
  };

  const DateComponent = (props: any) => {
    return (
      <div className="d-flex flex-row">
        {moment(props.data.createdOn).format('YYYY-MM-DD HH:mm ')}
      </div>
    );
  };

  // Column Definitions: Defines the columns to be displayed.
  const [colDefs] = useState<any>([
    {
      field: "systemId",
      flex: 1,
      cellRenderer: farmerTitleComponent,
      filter: true,
    },
    { field: "firstName", flex: 1, filter: true },
    { field: "otherNames", flex: 1, filter: true },
    {
      field: "country.countryName",
      flex: 1,
      headerName: "Country",
      filter: true,
    },
    { field: "createdOn", flex: 1, filter: true, cellRenderer: DateComponent },
    {
      field: "adminLevel1.countyName",
      flex: 1,
      headerName: "County",
      filter: true,
    },
    { field: "mobile", flex: 1, filter: true },
    {
      field: "paymentPhoneNumber",
      flex: 1,
      headerName: "Payment Phone",
      filter: true,
    },
    // { field: "Action", flex: 2, cellRenderer: CustomActionComponent },
  ]);

  const [pageData, setPageData] = useState<{ title: string; value: number }[]>(
    []
  );

  // const bindFarmers = async () => {
  //   if (searchTerm.length === 0 || searchTerm.length > 2) {
  //     const data: any = {
  //       pageNumber: 1,
  //       pageSize: 1000,
  //     };

  //     const response = await farmerService.getFarmerData(data);
  //     // setStats(response?.farmerStats);
  //     setRowData(response ? response.farmers : []);
  //   }
  // };

  // Use useCallback to memoize the fetchDataFunc
  const fetchFarmersData = useCallback(async (page: number, pageSize: number) => {
    try {
      const searchParams: any = {
        pageNumber: page,
        pageSize: pageSize,
      };
      const response = await farmerService.getFarmerPagedData(searchParams);
      const data = await response.result;

      // Perform the data transformation here 
      const transformedData = {
        rows: data.farmers, // Extract the array of farmers
        totalRows: data.farmerStats.totalFarmers // Extract the total count
      };
     setStats(data?.farmerStats);
      console.log("Transformed data for grid:", transformedData);

      return transformedData;
    } catch (error) {
      console.error("Failed to fetch farmers data:", error);
      return { rows: [], totalRows: 0 }; // Return a default structure on error
    }
  }, []); // The empty dependency array ensures this function is created only once

  useEffect(() => {
    document.title = `Manage Farmers - SDD`;
    dispatch(resetFarmerState());

   
  }, [searchTerm]);

  useEffect(() => {
    
      setPageData([
        { title: "Total", value: stats?.totalFarmers ?? 0 },
        { title: "KYC Valid", value: stats?.verifiedFarmers ?? 0 },
        { title: "KYC Pending", value: stats?.unverifiedFarmers ?? 0 },
      ]);
    
  }, [stats]);

  const countryCode = getSelectedCountryCode()

 

  return (
    <Content>
      {isAllowed("farmers.view") ? (
        <>
          {" "}
          <PageTitleWrapper />
          <PageTitle breadcrumbs={breadCrumbs}>Farmers</PageTitle>
          
            {stats !== null && <KeyMetrics keyMetrics={pageData} className="shadow my-3" />}
          
          <KTCard className="shadow my-3">
            <KTCardBody className="m-0 p-0">
              <CustomTable
                //rowData={rowData}
                colDefs={colDefs}
                header="farmer"
                addBtnText={isAllowed("farmers.add") ? "Add farmer" : ""}
                searchTerm={searchTerm}
                setSearchTerm={setSearchTerm}
                addBtnLink={isAllowed("farmers.add") ? "/farmers/add" : ""}
                showExport={isAllowed("farmers.import")}
                handleExport={handleExport}
                serverSidePagination={true}
                fetchDataFunc={fetchFarmersData}
              />
            </KTCardBody>
          </KTCard>{" "}
        </>
      ) : (
        <Error401 />
      )}
      {showAuditModal && (
        <AuditListModal
          exModule={"Farmer"}
          componentId={currentId}
          onClose={afterConfirm}
        />
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
