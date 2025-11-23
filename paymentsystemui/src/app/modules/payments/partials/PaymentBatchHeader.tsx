import { Link } from "react-router-dom";
import { Content } from "../../../../_metronic/layout/components/content";
import { KTIcon } from "../../../../_metronic/helpers";
import { ToolbarWrapper } from "../../../../_metronic/layout/components/toolbar";
import { isAllowed } from "../../../../_metronic/helpers/ApiUtil";
import { Error401 } from "../../errors/components/Error401";
import moment from "moment";
import { useState } from "react";
// import { PaymentImportModal } from "./PaymentImportModal";
import { ImportModal } from "../../../../_shared/Modals/ImportModal";

const PaymentBatchHeader = (props: any) => {
  const { batch } = props;
  
  const afterConfirm = (value: any) => {
    setView(value);
  }
  const [view, setView] = useState<any>();
  return (
    <>
      <ToolbarWrapper />
      <Content>
        {isAllowed("payments.batch.history") ? (
          <div className="card">
            <div className="card-header">
              <div className="d-flex flex-row">
                <h4 className="card-title align-items-start flex-column">
                  <span className="card-label fs-1 mb-1">
                    {" "}
                    {`${batch?.batchName}`}
                  </span>
                  {/* <span className='text-muted mt-1 fw-semibold fs-7'>{loanApplication?.beneficiaryId}</span> */}
                </h4>
              </div>

              <div className="card-toolbar">
                {isAllowed("payments.batch.add") ? (<button onClick={() => setView(true)} type="button" className='btn btn-sm btn-secondary w-200px me-3'>
                  <KTIcon iconName='exit-up' className='fs-2' />
                  import
                </button>) : (
                  ""
                )}
                {isAllowed("payments.batch.edit") ? (
                  <Link
                    to={`/payment-batch/edit/${batch?.id}`}
                    className="btn btn-sm btn-primary w-200px me-3"
                  >
                    <i className="bi bi-pencil fs-7 me-1"></i>Edit
                  </Link>
                ) : (
                  ""
                )}


              </div>
            </div>

            <div className="card-body py-9 pb-0 m-5">
              <div className="row mb-3">
                <div className="col-md-4">
                  <label className="my-1">Date</label>
                  <h6>{moment(batch?.dateCreated).format("DD-MM-YYYY")}</h6>
                </div>
                <div className="col-md-4">
                  <label className="my-1">Country</label>
                  <h6>{batch?.country?.countryName}</h6>
                </div>
                <div className="col-md-4">
                  <label className="my-1">Currency</label>
                  <h6> {batch?.country?.currencyName} </h6>
                </div>
              </div>
              <div className="row mb-3">
                <div className="col-md-4">
                  {batch?.loanBatch?.name && <label className="my-1">Loan Product</label>}
                  <h6>{batch?.loanBatch?.name}</h6>
                </div>

              </div>

              <div className="d-flex overflow-auto h-55px">
                <ul className="nav nav-stretch nav-line-tabs border-transparent fs-5 fw-bolder flex-nowrap">
                  <li className="nav-item">
                    <a
                      className={
                        `nav-link text-gray-800 me-6 ` +
                        (location.pathname ===
                          "/payment-batch-detail/farmers" ? "active" : "")
                      }
                      href="/payment-batch-detail/farmers"
                    >
                      Farmers
                    </a>
                  </li>
                  <li className="nav-item">
                    <a
                      className={
                        `nav-link text-gray-800 me-6 ` +
                        (location.pathname ===
                          "/payment-batch-detail/import-history" && "active")
                      }
                      href="/payment-batch-detail/import-history"
                    >
                      Excel import history
                    </a>
                  </li>
                </ul>
              </div>
            </div>
          </div>
        ) : (
          <Error401 />
        )}
      </Content>

      {view && <ImportModal
        exModule="PaymentDeductibles"
        title={"Payments"}
        afterConfirm={afterConfirm}
        batchId={batch.id}
        moduleId={batch.paymentModule}
      />}
    </>
  );
};

export default PaymentBatchHeader;
