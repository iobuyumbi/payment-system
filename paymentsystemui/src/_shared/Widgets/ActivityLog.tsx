import { FC, useEffect, useState } from 'react'
import moment from 'moment';
import { Link } from 'react-router-dom';
import { ClipLoader } from 'react-spinners';
import ActivityLogService from '../../services/ActivityLogService';
import { KTCard, KTCardBody } from '../../_metronic/helpers';
import { NoDataFound } from '../../lib/NoData';
import { Content } from '../../_metronic/layout/components/content';

const activityLogService = new ActivityLogService();

const ActivityLog: FC = () => {
    const [actLog, setActLog] = useState<any[]>([]);
    const [loading, setLoading] = useState<boolean>(false);

    const bindActivityLog = async () => {
        setLoading(true);
        const result = await activityLogService.getActivityLogs();
        if (result) {
            setActLog(result);
            setLoading(false);
        }
    }

    useEffect(() => {
        bindActivityLog();
    }, []);

    return (<Content>
        {actLog.length === 0 &&
            <KTCard>
                <KTCardBody className='d-flex flex-row w-100 justify-content-center'>
                    <NoDataFound />
                </KTCardBody>
            </KTCard>
        }

        {actLog.length > 0 &&
            <KTCard>
                {/* begin::Header */}
                <div className='card-header align-items-center'>
                    <h2 className="fw-normal">Activity Log</h2>
                </div>
                {/* end::Header */}
                <KTCardBody className='h-500px pb-10 overflow-auto'>
                    {loading && <div className='d-flex align-items-center justify-content-center w-100 h-100 p-20 mt-20 mb-20'>
                        <ClipLoader color="#0093af" size="50" className='me-5' /> loading...</div>
                    }
                    {
                        actLog && actLog.slice(0, 10).map((item, index) => (
                            <div className="pt-1" key={item.id}>
                                <div className='timeline-item'>
                                    <div className="flex mt-1">
                                        <div className='timeline-content mt-n1'>
                                            <div className='pe-3 mb-5'>
                                                <div className='fs-5 fw-semibold mb-2'>
                                                    {
                                                        item.link && <Link to={item.link}>
                                                            <div dangerouslySetInnerHTML={{ __html: item.title }}></div>
                                                        </Link>
                                                    }
                                                </div>

                                                <div className='d-flex align-items-center mt-1 fs-6'>
                                                    <div className='text-muted me-2 fs-7'>at{' '}
                                                        {moment(item.createdOn).format('hh:mm ss')} by
                                                    </div>

                                                    <span className='fw-bolder me-1'>
                                                        <span className='text-gray-500'>{item.createdBy} </span>
                                                    </span>
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>
                                <div className="separator "></div>
                            </div>
                        ))
                    }
                </KTCardBody>
            </KTCard>
        }
    </Content>
    )
}

export { ActivityLog }
