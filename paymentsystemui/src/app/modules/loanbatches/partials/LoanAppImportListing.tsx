import { useState } from 'react'
import { KTCard, KTCardBody } from '../../../../_metronic/helpers';
import CustomTable from '../../../../_shared/CustomTable/Index';

const LoanAppImportListing = ({ data }: any) => {

  console.log(data);

  const StatusComponent = (props: any) => {
    return (
      <div className="py-1">
        {props?.data?.statusId === 1 && (
          <div className="badge fs-7 badge-light-success  fw-normal">Yes</div>
        )}
        {props?.data?.statusId === 0 && (
          <div className="badge fs-7 badge-light-danger  fw-normal">No</div>
        )}
      </div>
    );
  };

  const [colDefs, setColDefs] = useState<any>([
    { field: "rowNumber", flex: 1, },
    { field: "status", flex: 1, cellRenderer: StatusComponent },
    { field: "validationErrors", flex: 2, headerName: "Farmer Earnings Share Lc" },
  ]);

  return (
    <div>
      <KTCard className="m-0 p-0">
        {/* begin::Header */}
        <div className="card-header" id="kt_help_header">
        <h4 className="card-title align-items-start flex-column">Imported Loan Applications</h4>
          <div className="card-toolbar">
            {/* <button
            className="btn btn-dark btn-sm"
            onClick={downloadInterimList}
          >
            <KTIcon iconName="exit-down" /> Download Payment List
          </button> */}
          </div>
        </div>

        {/* end::Header */}
        <KTCardBody className="m-0 p-0">
          <CustomTable
            rowData={data?.importedLoanApplications}
            colDefs={colDefs}
            header=""
            addBtnText={""}
            importBtnText={""}
            addBtnLink={""}
            showImportBtn={false}
          />
        </KTCardBody>
      </KTCard>
      {/* {showConfirmBox && (
      <ConfirmBox
        confirmModel={confirmModel}
        afterConfirm={afterConfirm}
        loading={loading}
      />
    )} */}
    </div>
  )
}

export default LoanAppImportListing
