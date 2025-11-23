import { useEffect, useState } from "react";
import { KTCard, KTCardBody, KTIcon } from "../../../_metronic/helpers";
import CustomTable from "../../../_shared/CustomTable/Index";
import { useNavigate, useParams } from "react-router-dom";
import ExportService from "../../../services/ExportService";
import { isAllowed } from "../../../_metronic/helpers/ApiUtil";
import { useDispatch } from "react-redux";
import { IConfirmModel } from "../../../_models/confirm-model";
import { ConfirmBox } from "../../../_shared/Modals/ConfirmBox";
import { addToPaymentDeductible } from "../../../_features/payment-deductible/paymentDeductibleSlice";


import { Content } from "../../../_metronic/layout/components/content";
import DeductiblePaymentService from "../../../services/DeductibleService";
const deductibleService = new DeductiblePaymentService

const RepaymentReport = () => {


  const [rowData, setRowData] = useState<any>();
  const [paymentId, setPaymentId] = useState<any>();
  const [viewStats, setViewStats] = useState<any>(false);
  const [showConfirmBox, setShowConfirmBox] = useState<boolean>(false);
  const [confirmModel, setConfirmModel] = useState<IConfirmModel>();
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();
  const dispatch = useDispatch();

  const afterConfirm = (res: any) => {
    if (res) bindData();
    setShowConfirmBox(false);

    setViewStats(false);
  };

  const editButtonHandler = (value: any) => {
    dispatch(addToPaymentDeductible(value));
   
    setViewStats(true);
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
  const CustomActionComponent = (props: any) => {
    return (
      <div className="d-flex flex-row">
     
          <button
            className="btn btn-default mx-0 px-1"
            onClick={() => editButtonHandler(props.data)}
          >
           View Report
          </button>
       
      
      </div>
    );
  };
  const StatusComponent = (props: any) => {
    return (
      <div className="py-1">
        {props.data.isPaymentComplete === true && (
          <div className="badge fs-7 badge-light-success  fw-normal">Yes</div>
        )}
        {props.data.isPaymentComplete === false && (
          <div className="badge fs-7 badge-light-danger  fw-normal">No</div>
        )}
      </div>
    );
  };
  const farmerComponent = (props: any) => {
    const farmer = props.data.farmer;
    const fields = props.fields || []; // Fields to display

    if (!farmer) {
      return <div>No Farmer Data</div>;
    }

    return (
      <div className="d-flex flex-column">
        {fields.map((field: string) => (
          <div key={field}>{farmer[field] || "Not Available"}</div>
        ))}
      </div>
    );
  };

  // Column Definitions: Defines the columns to be displayed.
  const [colDefs, setColDefs] = useState<any>([
    {
      field: "fullName",
      flex: 1, 
     
      cellRenderer: farmerComponent,
      cellRendererParams: {
        fields: ["fullName"],
      },
    },
    {
      field: "mobile",
      flex: 1,

      cellRenderer: farmerComponent,
      cellRendererParams: {
        fields: ["mobile"],
      },
    },
    { field: "systemId", flex: 1, headerName: "System Id" , filter: true},
    { field: "farmerLoansDeductionsLc", flex: 2, headerName: "Loan Deductions" },
    { field: "farmerPayableEarningsLc", flex: 2, headerName: "Payable Earnings" },
    { field: "farmerEarningsShareLc", flex: 2, headerName: "Farmer Earnings" },
    // { field: "isPaymentComplete", flex: 1, cellRenderer: StatusComponent },
    // { field: "Action", flex: 1, cellRenderer: CustomActionComponent },
  ]);

  const bindData = async () => {
   
    const response = await deductibleService.getDeductiblePaymentReport();
    setRowData(response);
  };



  useEffect(() => {
    document.title = `Repayment Report`;
    bindData();
  }, []);

  return (
    <Content>
    <div>
      <KTCard className="m-0 p-0">
        {/* begin::Header */}
        <div className="card-header" id="kt_help_header">
          <h4 className="card-title fw-bold fs-3">Payment Request Details</h4>
          {/* <div className="card-toolbar">
            <button
              className="btn btn-dark btn-sm"
              onClick={downloadInterimList}
            >
              <KTIcon iconName="exit-down" /> Download Payment List
            </button>
          </div> */}
        </div>
        {/* end::Header */}
        <KTCardBody className="m-0 p-0">
          <CustomTable
            rowData={rowData}
            colDefs={colDefs}
            header=""
            addBtnText={""}
            importBtnText={""}
            addBtnLink={""}
            showImportBtn={false}
          />
        </KTCardBody>
      </KTCard>
      {showConfirmBox && (
        <ConfirmBox
          confirmModel={confirmModel}
          afterConfirm={afterConfirm}
          loading={loading}
        />
      )}
    </div>
    </Content>
  );
};

export default RepaymentReport;
