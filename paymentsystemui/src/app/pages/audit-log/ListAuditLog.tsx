import { useEffect, useState } from 'react'
import AuditLogService from '../../../services/AuditLogService';
import { Content } from '../../../_metronic/layout/components/content';
import { Loader } from '../../../_shared/components';
import { useParams, useSearchParams } from 'react-router-dom';

const auditLogService = new AuditLogService();

const ListAuditLog = () => {
    const [searchParams] = useSearchParams();
    const q = searchParams.get('q'); // gets `q` from the URL

    const [auditLogs, setAuditLogs] = useState<any>([]);
    const [loading, setLoading] = useState<boolean>(false);

    useEffect(() => {
        setLoading(true)
        const fetchAuditLogs = async () => {
            const response = await auditLogService.getTopAuditLogs(q as string, 20);
            if (response) {
                setAuditLogs(response);

                setLoading(false);
            }
        }
        if (q) {
            fetchAuditLogs();
        }
    }, [q]);

    return (
        <Content>
            <div className="card card-grid min-w-full">
                <div className="card-header py-5 flex-wrap">
                    <h3 className="card-title">Audit Log</h3>

                </div>
                <div className="card-body">
                    {loading && <Loader />}
                    {
                        <div className="p-4 border ">
                            {auditLogs.length === 0 ? (
                                <p>No changes recorded.</p>
                            ) : (
                                auditLogs.map((log: any, index: any) => (
                                    <div key={index} className="border-b py-2">
                                        <p>
                                            <strong>Changed By:</strong> {log.changedBy}
                                        </p>
                                        <p>
                                            <strong>Changed On:</strong> {new Date(log.changedOn).toLocaleString()}
                                        </p>
                                        <p>
                                            <strong>Change Type:</strong> {log.changeType}
                                        </p>
                                        <ul className="list-disc pl-5">
                                            {log.changes.map((change: any, idx: any) => (
                                                <li key={idx}>
                                                    <strong>{change.field}:</strong> {change.oldValue} → {change.newValue}
                                                </li>
                                            ))}
                                        </ul>
                                    </div>
                                ))
                            )}
                        </div>

                    }

                </div>
            </div>
        </Content>
    )
}

export default ListAuditLog
