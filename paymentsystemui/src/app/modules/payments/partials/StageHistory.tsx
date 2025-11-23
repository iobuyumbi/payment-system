import { useEffect, useState } from "react";
import { KTCard, KTCardBody, KTIcon } from "../../../../_metronic/helpers";
import CustomTable from "../../../../_shared/CustomTable/Index";

import PaymentBatchService from "../../../../services/PaymentBatchService";
import { Link } from "react-router-dom";
import moment from "moment";
import PaymentBatchStatusBadge from "../../../components/PaymentBatchStatusBadge";

const paymentBatchService = new PaymentBatchService();

const StageHistory = (props: any) => {

    const [rowData, setRowData] = useState<any>();
    const [colDefs] = useState<any>([
        { field: "action", flex: 1, headerName: "Status" },
        { field: "comments", flex: 3, headerName: "Comments" },
    ]);

    useEffect(() => {
        const bindHistory = async () => {
            const response = await paymentBatchService.getHistory(props.id);
            if (response) {
                setRowData(response);
            }
        }

        bindHistory()
    }, []);

    return (
        <div>
            <KTCard className="m-2 p-0">
                {/* begin::Header */}
                <div className="card-header" id="kt_help_header">
                    <h4 className="card-title fw-bold fs-3">History</h4>
                </div>
                {/* end::Header */}
                <KTCardBody className='h-350px pb-10 overflow-auto'>
                    {
                        rowData && rowData.map((item: any, index: number) => (
                            <div className="py-2" key={item.id}>
                                <div className='timeline-item'>
                                    <div className="flex mt-1">
                                        <div className='timeline-content mt-n1'>
                                            <div className='pe-3 mb-5'>
                                                <div className='d-flex align-items-center mt-1 fs-6'>
                                                    <PaymentBatchStatusBadge statusText={item.action} />

                                                    <span className='mx-1'>
                                                        <span className='text-muted me-2 fs-7'> →{item.createdBy} </span>
                                                    </span>
                                                    <div className='text-muted me-2 fs-7'>{' '}|{' '}
                                                        {moment(item.createdOn).format('hh:mm ss')}
                                                    </div>

                                                </div>
                                                <div className='m-2'>

                                                    <span>{item.comments}</span>
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

        </div>
    );
};

export default StageHistory;
