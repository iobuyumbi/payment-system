import { useState } from "react";
import { KTIcon } from "../../../../_metronic/helpers";
import { Content } from "../../../../_metronic/layout/components/content";

import { useSelector } from "react-redux";
import { isAllowed } from "../../../../_metronic/helpers/ApiUtil";
import AddAssociates from "./AddAssociates";
import { Disbursement } from "./Disbursement";

export default function DisbursementModal(props: any) {
  const { afterConfirm, farmers} = props;

  const [title] = useState<any>("Add");
  return (
    <>
      {isAllowed("payments.batch.add") ? (
        <div
          className="modal fade show d-block"
          id="kt_modal_confiem_box"
          role="dialog"
          tabIndex={-1}
          aria-modal="true"
        >
          <div className="modal-dialog modal-lg ">
            <div className="modal-content">
              <div className="modal-header">
                <div className="w-100 d-flex flex-row align-items-center justify-content-between">
                  <div>
                    <h2 className="fw-semibold">{title} Disbursement</h2>
                  </div>

                  <div
                    className="btn btn-icon btn-sm btn-active-icon-primary"
                    data-kt-users-modal-action="close"
                    onClick={() => afterConfirm(false)}
                    style={{ cursor: "pointer" }}
                  >
                    <KTIcon
                      iconName="abstract-11"
                      iconType="outline"
                      className="fs-1"
                    />
                  </div>
                </div>
              </div>

              <Disbursement afterConfirm={afterConfirm} farmers={farmers} />
            </div>
          </div>
        </div>
      ) : (
        ""
      )}

      <div className="modal-backdrop fade show"></div>
    </>
  );
}
