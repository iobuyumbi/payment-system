import { useEffect, useState } from "react";
import { KTCard, KTCardBody, KTIcon } from "../../../../_metronic/helpers";
import CustomTable from "../../../../_shared/CustomTable/Index";
import { useNavigate, useParams } from "react-router-dom";
import ExportService from "../../../../services/ExportService";
import { isAllowed } from "../../../../_metronic/helpers/ApiUtil";
import { useDispatch } from "react-redux";
import { IConfirmModel } from "../../../../_models/confirm-model";
import { ConfirmBox } from "../../../../_shared/Modals/ConfirmBox";
import { addToPaymentDeductible } from "../../../../_features/payment-deductible/paymentDeductibleSlice";
import saveAs from "file-saver";
import moment from "moment";
import * as XLSX from "xlsx";

const exportService = new ExportService();

const ListDeductiblePayments = (props: any) => {
  const { batchId, paymentBatchName } = props;
  const [rowData, setRowData] = useState<any>();
  const [showForm, setShowForm] = useState<any>(false);
  const [showConfirmBox, setShowConfirmBox] = useState<boolean>(false);
  const [confirmModel, setConfirmModel] = useState<IConfirmModel>();
  const [loading, setLoading] = useState(false);
  const navigate = useNavigate();
  const dispatch = useDispatch();

  const afterConfirm = (res: any) => {
    if (res) bindData();
    setShowConfirmBox(false);

    setShowForm(false);
  };

  const editButtonHandler = (value: any) => {
    dispatch(addToPaymentDeductible(value));
    navigate(`/payment-batch/deductible/edit/${value.id}`);
    setShowForm(true);
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
        {isAllowed("payments.batch.edit") && (
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
        {isAllowed("payments.delete") && (
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

  const StatusComponent = (props: any) => {

    return (
      <div className="py-1">
        {props.data.isFarmerVerified === true && (
          <div className="badge fs-7 badge-light-success  fw-normal">Yes</div>
        )}
        {props.data.isFarmerVerified === false && (
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

  const CurrenyComponent = (props: any) => {
    const farmer = props.data.farmer?.country?.code;
    // Fields to display

    if (!farmer) {
      return <div>No Farmer Data</div>;
    }

    return (
      <div className="d-flex flex-column">
        {farmer}
      </div>
    );
  };

  // Column Definitions: Defines the columns to be displayed.
  const [colDefs, setColDefs] = useState<any>([
    {
      field: "loanAccountNo",
      width: 150,
    },
    {
      field: "fullName",
      width: 150,
    },
    {
      field: "mobile",
      width: 150,
    },
    { field: "systemId", width: 150, headerName: "System Id" },
    { field: "nationalId", width: 150, headerName: "National Id" },
    { field: "isFarmerVerified", width: 150, cellRenderer: StatusComponent },
    {
      field: "farmerEarningsShareLc", width: 150, headerName: "Farmer Earnings Share Lc",
      valueGetter: (params: any) => {
        return (params.data?.farmerEarningsShareLc)?.toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 });
      }
    },
    {
      field: "farmerPayableEarningsLc", width: 150, headerName: "Farmer Payable Earnings Lc",
      valueGetter: (params: any) => {
        return (params.data?.farmerPayableEarningsLc)?.toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 });
       
      }
    },
    {
      headerName: "Opening Balance",
      field: "openingBalance",
      width: 150,
      valueGetter: (params: any) => {
        const loanBalance = parseFloat(params.data?.farmerLoansBalanceLc ?? 0);
        const deduction = parseFloat(params.data?.farmerLoansDeductionsLc ?? 0);
        return (loanBalance + deduction).toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 });
      }
    },
    {
      field: "farmerLoansDeductionsLc", width: 150, headerName: "Farmer Loans Deductions Lc",
      valueGetter: (params: any) => {
        return (params.data?.farmerLoansDeductionsLc)?.toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 });
      }
    },
    {
      field: "farmerLoansBalanceLc", width: 150, headerName: "Loan Balance After Payment",
      valueGetter: (params: any) => {
          return (params.data?.farmerLoansBalanceLc)?.toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 });
       
      }
    },
    { field: "remarks", width: 150, headerName: "Remarks" },
  ]);

  const bindData = async () => {
    const model = { statusId: -1, batchId: batchId };
    const response = await exportService.getDeductiblePayments(model);
    setRowData(response);
  };

  const downloadInterimList = async () => {
    try {
      const workbook = XLSX.utils.book_new();
      const model = { statusId: -1, batchId: batchId };
      const response = await exportService.getDeductiblePayments(model);
      console.log("Response:", response);

      setLoading(true);

      // return
      if (Array.isArray(response)) {
        const mappedData = response.map((row: any) => ({
          "Uwanjani System ID": row.systemId,
          "Beneficiary Farmer Card ID": row.beneficiaryId,
          "Carbon Units Accrued": row.carbonUnitsAccured,
          "Unit Cost EUR": row.unitCostEur,
          "Total Units Earning EUR": row.totalUnitsEarningEur,
          "Total Units Earning LC": row.totalUnitsEarningLc,
          "Partners Adminstrative Cost": row.solidaridadEarningsShare,
          //"Farmer Earnings Share EUR": row.farmerEarningsShareEur,
          "Farmer Earnings Share LC": row.farmerEarningsShareLc,
          "Farmer Payable Earnings LC": row.farmerPayableEarningsLc,
          "Farmer Loans Deductions LC": row.farmerLoansDeductionsLc,
          "Opening Balance LC": row.openingBalance,
          "Farmer Loans Balance LC": row.farmerLoansBalanceLc,
          "Farmer Name": `${row.fullName}`,
          "Payment Phone Number": row.mobile,
          "Payment Complete": row.isPaymentComplete ? "Yes" : "No",
        }));

        const worksheet = XLSX.utils.json_to_sheet(mappedData);
        XLSX.utils.book_append_sheet(workbook, worksheet, "PaymentList");

        const blob = new Blob(
          [XLSX.write(workbook, { bookType: "xlsx", type: "array" })],
          {
            type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
          }
        );

        saveAs(
          blob,
          `${"PaymentList"}_${paymentBatchName}_${moment().format('YYYY-MM-DD HH:mm ')}.xlsx`
        );
      } else {
        alert("No data available to download.");
      }
    } catch (error) {
      console.error("Failed to download interim list:", error);
      alert("An error occurred while downloading the list. Please try again.");
    }
    finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    document.title = `Manage deductible payments`;
    bindData();
  }, []);

  return (
    <div>

      <KTCard className="m-0 p-0">
        {/* begin::Header */}
        <div className="card-header" id="kt_help_header">
          {/* <h4 className="card-title fw-bold fs-3">Payment Request Details</h4> */}
          <div className="card-toolbar">
            <button
              className="btn btn-dark btn-sm"
              onClick={downloadInterimList}
            >
              <KTIcon iconName="exit-down" /> Download Payment List
            </button>
          </div>
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
  );
};

export default ListDeductiblePayments;
