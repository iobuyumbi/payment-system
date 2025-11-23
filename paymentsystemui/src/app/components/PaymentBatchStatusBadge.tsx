import { statusTextMap } from "../../_metronic/helpers";

const PaymentBatchStatusBadge = (props: any) => {
    const { statusText } = props;

    return (
        <div>
            <div title={statusText}
                className={`badge fs-7 badge-light-${statusTextMap[statusText]}`}>
                {statusText}
            </div>
        </div>
    )
}

export default PaymentBatchStatusBadge
