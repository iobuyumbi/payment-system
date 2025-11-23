import { useEffect, useState } from "react";
import { KTCard, KTCardBody, KTIcon } from "../../../../_metronic/helpers";
import CustomTable from "../../../../_shared/CustomTable/Index";
import { useParams } from "react-router-dom";
import ImportService from "../../../../services/ImportService";
import ExportService from "../../../../services/ExportService";
import * as XLSX from "xlsx";
import saveAs from "file-saver";

const importService = new ImportService();
const exportService = new ExportService();

const PaymentImportHistory = (props: any) => {
  const { rowData, batchId, importHistory } = props;


  const StatusComponent = (props: any) => {
    return (
      <div className="py-1">
        {props.data.isSuccess === true && (
          <div className="badge badge-success fs-7 fw-normal">success</div>
        )}
        {props.data.isSuccess === false && (
          <div className="badge badge-light-danger fs-7 fw-normal">fail</div>
        )}
      </div>
    );
  };

  // Column Definitions: Defines the columns to be displayed.
  const [colDefs, setColDefs] = useState<any>([
    // { field: "tabName", flex: 1, headerName: "Tab" },
    { field: "rowNumber", flex: 1, headerName: "Row Number" },
    {
      field: "isSuccess",
      flex: 1,
      headerName: "Status",
      cellRenderer: StatusComponent,
    },
    { field: "remarks", flex: 3, headerName: "Remarks" },
  ]);

  const handleExport = async () => {
    const workbook = XLSX.utils.book_new();

    const model = { statusId: 0, batchId: batchId }
    const response = await exportService.getDeductiblePayments(model);

    if (response !== undefined && response !== null) {
      const mappedData = response.map((row: any) => ({
        "System Id": row.systemId,
        "Beneficiary Id": row.beneficiaryId,
        "Carbon Units Accured": row.carbonUnitsAccured,
        "Unit Cost Eur": row.unitCostEur,
        "Total Units Earning Eur": row.totalUnitsEarningEur,
        "Total Units Earning Lc": row.totalUnitsEarningLc,
        "Partners Adminstrative Cost": row.solidaridadEarningsShare,
        "Farmer Earnings Share Eur": row.farmerEarningsShareEur,
        "Farmer Earnings Share Lc": row.farmerEarningsShareLc,
        "Farmer Payable Earnings Lc": row.farmerPayableEarningsLc,
        "Farmer Loans Deductions Lc": row.farmerLoansDeductionsLc,
        "Farmer Loans Balance Lc": row.farmerLoansBalanceLc,
        //"Excel Import Id": row.excelImportId,
        "Status": row.statusId == 0 ? "Failed" : "Passed",
        Remarks: row.remarks,
        //"Payment Batch Id": row.paymentBatchId,
        "Id": row.id
      }));

      const worksheet = XLSX.utils.json_to_sheet(mappedData);
      XLSX.utils.book_append_sheet(workbook, worksheet, "PaymentList");
      const blob = new Blob(
        [XLSX.write(workbook, { bookType: "xlsx", type: "array" })],
        {
          type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        }
      );
      saveAs(blob, "FailedDeductibleRecords.xlsx");
    }
  };

  useEffect(() => {
    document.title = `Manage deductible payments`;

  }, []);

  return (
    <div>
      <KTCard className="m-0 p-0">
        {/* begin::Header */}
        <div className="card-header" id="kt_help_header">
          <h4 className="card-title fw-bold fs-3">Import Summary</h4>
          {/* {importHistory?.every(
                    (item: any) => item.totalRowsInExcel === item.passedRows
                  ) && ()} */}
          <button
            className="btn btn-secondary my-3 mh-50px"
            onClick={() => handleExport()}
          >
            {" "}
            <KTIcon iconName="exit-down" className="fs-2" /> Download failed
            records
          </button>
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

    </div>
  );
};

export default PaymentImportHistory;
