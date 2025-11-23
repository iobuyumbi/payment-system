import { KTCard, KTCardBody } from "../../../../_metronic/helpers";
import SuccessCard from "../cards/successCard";
import PaymentInfoCard from "../cards/paymentInfoCard";
import PaymentBatchStatusBadge from "../../../components/PaymentBatchStatusBadge";
import PaymentModuleText from "../../../components/PaymentModuleText";
import { isAllowed } from "../../../../_metronic/helpers/ApiUtil";
import WarningCard from "../cards/warningCard";
import WorkflowStageCard from "../cards/WorkflowStageCard";
import StageHistory from "./StageHistory";

const PaymentImportHeader = ({ batch, importHistory, totalCounts }: any) => {
  return (
    <div>
      <KTCard className="mb-5">
        {/* begin::Header */}
        <div className="card-header">
          <h4 className="card-title ">
            <div className="d-flex align-items-center flex-wrap gap-2">
              <span className="card-label fs-1 mb-1">{batch?.batchName}</span>
              <PaymentModuleText paymentModule={batch?.paymentModule} />
              <PaymentBatchStatusBadge statusText={batch?.status?.stageText} />
            </div>
          </h4>
        </div>
        {/* end::Header */}
        <KTCardBody>
          <div className="row">
            <div className="col-md-5">
              <PaymentInfoCard batch={batch} totalCounts={totalCounts} />


            </div>
            <div className="col-md-7">
              {batch?.successRowCount > 0
                && batch?.failedRowCount == 0
                && batch?.status.stageText !== 'Rejected' &&
                batch?.status.stageText !== 'Review Rejected' &&
                (
                  // <SuccessCard
                  //   batch={batch}
                  //   status={batch?.status?.stageText}
                  //   isAllowed={isAllowed}
                  // />
                  <WorkflowStageCard batch={batch} />
                )}
              {(batch?.failedRowCount > 0 || batch?.status.stageText === 'Rejected'
                || batch?.status.stageText === 'Review Rejected'
              ) && (
                  <WarningCard
                    loanBatchName={batch?.batchName}
                    paymentBatchId={batch?.id}
                    paymentModule={batch?.paymentModule}
                    status={batch?.status?.stageText}
                  />
                )}


                <div > <StageHistory id={batch?.id} /></div>
           
            </div>
          </div>

        </KTCardBody>
      </KTCard>
    </div>
  );
};

export default PaymentImportHeader;
