import { useEffect, useState } from "react";
import { KTCard, KTCardBody, KTIcon } from "../../../../_metronic/helpers";
import CustomTable from "../../../../_shared/CustomTable/Index";
import { IConfirmModel } from "../../../../_models/confirm-model";
import { ConfirmBox } from "../../../../_shared/Modals/ConfirmBox";
import ImportService from "../../../../services/ImportService";

const importService = new ImportService();

const ListImportErrors = (props: any) => {
  const { batchId, paymentBatchName } = props;
  const [rowData, setRowData] = useState<any>();
  const [showForm, setShowForm] = useState<any>(false);
  const [showConfirmBox, setShowConfirmBox] = useState<boolean>(false);
  const [confirmModel, setConfirmModel] = useState<IConfirmModel>();
  const [loading, setLoading] = useState(false);

  const afterConfirm = (res: any) => {
    if (res) bindData();
    setShowConfirmBox(false);

    setShowForm(false);
  };

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

  const [colDefs, setColDefs] = useState<any>([
    // {
    //   field: "tabName",
    //   flex: 1,
    // },
    {
      field: "rowNumber",
      flex: 1,
    },
    { field: "isSuccess", headerName: "Is Success", flex: 1, cellRenderer: StatusComponent },

    { field: "remarks", flex: 2, headerName: "Remarks" },
  ]);

  const bindData = async () => {
    const response = await importService.getImportDetailsByPaymentBatch(batchId);
    console.log(response)
    setRowData(response);
  };

  useEffect(() => {
    bindData();
  }, [])

  return (
    <div>

      <KTCard className="m-0 p-0">
        {/* begin::Header */}
        <div className="card-header" id="kt_help_header">
          <div className="card-toolbar">
            {/* <button
              className="btn btn-dark btn-sm"
            >
              <KTIcon iconName="exit-down" /> Download
            </button> */}
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
  )
}

export default ListImportErrors
