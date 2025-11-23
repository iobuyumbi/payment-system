import { KTIcon } from "../../../../_metronic/helpers";

const InfoCard = (props: any) => {
  return (
    <div>
      <div className="notice d-flex bg-light-warning rounded border-warning border border-dashed mb-9 p-6">
        <KTIcon iconName="information-5" className="fs-2tx text-warning me-4" />

        <div className="d-flex flex-stack flex-grow-1">
          <div className="fw-bold">
            <h4 className="text-gray-800 fw-bolder">Pre Payment KYC Status</h4>
            <div className="fs-6 text-gray-700 fw-normal">
              The mobile numbers were validated before being send for payment
              processing.
              {/* If you wish to make any changes, you may reupload the Excel file again. */}
              <p className="my-2">
                The system first did the local verification, and the records
                which could not be verified locally were then verified through
                Onafriq.
              </p>
              <p className="my-2">
                Only verified records will be sent for payment processing.
              </p>
              <div className="d-flex my-4 align-items-start">
              { !props.importHistory?.every(
                  (item: any) => item.totalRowsInExcel === item.passedRows ) && <button
                  className="fw-bolder btn btn-theme mx-2"
                  onClick={() => props.setShowImport(true)}
                >
                  Re-Upload Data
                </button>}
                <div>
                <button
                  type="button"
                  className="btn btn-primary"
                  onClick={props.onButtonClick}
                
                >
                  Request for Payment Review
                </button>
                {!props.importHistory?.every(
                  (item: any) => item.totalRowsInExcel === item.passedRows
                ) && (
                  <div className="d-flex my-3">
                    <i className="fas fa-info-circle me-2"></i>
                    Disabled as data is incomplete
                  </div>
                )}</div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default InfoCard;
