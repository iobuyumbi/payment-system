import { useEffect, useState } from "react";
import AuditLogService from "../../services/AuditLogService";
import { Content } from "../../_metronic/layout/components/content";
import { Loader } from "../../_shared/components";
import { KTIcon } from "../../_metronic/helpers";

const auditLogService = new AuditLogService();

const ListAuditModal = (props: any) => {
  const { exModule, componentId, onClose } = props;

  const [auditLogs, setAuditLogs] = useState<any>([]);
  const [loading, setLoading] = useState<boolean>(false);

  useEffect(() => {
    setLoading(true);
    
    const bindInvoiceAuditLog = async () => {
      const response = await auditLogService.getTopAuditLogs(
        exModule,
        componentId
      );
      if (response) {
        setAuditLogs(response);

        setLoading(false);
      }
    };

    bindInvoiceAuditLog();
  }, []);

  return (
    <>
      <div
        className="modal fade show d-block"
        id="kt_modal_confiem_box"
        role="dialog"
        tabIndex={-1}
        aria-modal="true"
      >
        <div className="modal-dialog modal-xl">
          <div className="modal-content">
            <div className="card card-grid min-w-full">
              <div className="card-header py-5 flex-wrap">
                <h3 className="card-title">Audit Log</h3>
                <div
                  className="btn btn-icon btn-sm btn-active-icon-primary"
                  data-kt-users-modal-action="close"
                  onClick={() => onClose()}
                  style={{ cursor: "pointer" }}
                >
                  <KTIcon
                    iconName="abstract-11"
                    iconType="outline"
                    className="fs-1"
                  />
                </div>
              </div>
              <div className="card-body">
                {loading && <Loader />}
                {
                  <div className="p-4 border ">
                    {auditLogs.length === 0 ? (
                      <p>No changes recorded.</p>
                    ) : (
                      auditLogs.map((log: any, index: any) => (
                        <div key={index}>
                          <div className="border-b py-2">
                            <p>
                              <strong>Changed By:</strong> {log.changedBy}
                            </p>
                            <p>
                              <strong>Changed On:</strong>{" "}
                              {new Date(log.changedOn).toLocaleString()}
                            </p>
                            <p>
                              <strong>Change Type:</strong> {log.changeType}
                            </p>
                            <ul className="list-disc pl-5">
                              {log.changes.map((change: any, idx: any) => (
                                <li key={idx}>
                                  <strong>{change.field}:</strong>{" "}
                                  {change.oldValue} → {change.newValue}
                                </li>
                              ))}
                            </ul>
                          </div>

                          {index !== auditLogs.length - 1 && (
                            <hr className="my-3 border-gray-300" />
                          )}
                        </div>
                      ))
                    )}
                  </div>
                }
              </div>
            </div>
          </div>
        </div>
      </div>
    </>
  );
};

export default ListAuditModal;
