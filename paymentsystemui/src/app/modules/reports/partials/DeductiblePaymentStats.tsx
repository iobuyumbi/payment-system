const DeductiblePaymentStats = (props : any) => {
    const {stats}= props;
    return (
        <div className="row">
            <div className="col-md-3 p-3">
                <div className="bg-light-primary px-6 py-8 rounded-2  d-flex align-items-center justify-content-between m-3">
                    <div>
                        {/* <KTIcon
                    iconName="chart-simple"
                    className="fs-3x text-warning d-block my-2"
                  /> */}
                        <a href="#" className="text-primary fw-semibold fs-6">
                            Total Payments
                        </a>
                    </div>
                    <div className="display-6 fw-normal mt-2">{stats?.totalPayment}</div>
                </div>
            </div>
            <div className="col-md-3 p-3">
                <div className="bg-light-warning  px-6 py-8 rounded-2  d-flex align-items-center justify-content-between m-3">
                    <div>
                        {" "}
                        {/* <KTIcon
                    iconName="plus"
                    className="fs-3x text-primary d-block my-2"
                  /> */}
                        <a href="#" className="text-warning fw-semibold fs-6">
                            Processing
                        </a>
                    </div>
                    <div className="display-6 fw-normal  mt-2">{stats?.pendingPayments}</div>
                </div>
            </div>
            <div className="col-md-3 p-3">
                <div className="bg-light-danger px-6 py-8 rounded-2  d-flex align-items-center justify-content-between m-3">
                    <div>
                        {/* <KTIcon
                    iconName="abstract-26"
                    className="fs-3x text-danger d-block my-2"
                  /> */}
                        <a href="#" className="text-danger fw-semibold fs-6">
                            Failed
                        </a>
                    </div>
                    <div className="display-6 fw-normal  mt-2">{stats?.pendingPayments}</div>
                </div>
            </div>
            <div className="col-md-3 p-3">
                <div className=" bg-light-success px-6 py-8 rounded-2 d-flex align-items-center justify-content-between m-3">
                    <div>
                        {/* <KTIcon
                    iconName="sms"
                    className="fs-3x text-success d-block my-2"
                  /> */}
                        <a href="#" className="text-success fw-semibold fs-6">
                            Completed
                        </a>
                    </div>
                    <div className="display-6 fw-normal mt-2">{stats?.completedPayments}</div>
                </div>
            </div>
        </div>
    )
}

export default DeductiblePaymentStats
