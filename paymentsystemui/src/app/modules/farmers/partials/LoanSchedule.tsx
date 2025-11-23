import moment from "moment";
import React, { useState } from "react";
import config from "../../../../environments/config";
import LoanRepaymentService from "../../../../services/LoanRepaymentService";
import clsx from "clsx";

const loanRepaymentService = new LoanRepaymentService();

interface LoanScheduleItem {
    amount: number;
    balance: number;
    principalAmount: number;
    endDate: any;
    interestAmount: number;
    startDate: number;
    paymentStatus: any;
}

interface LoanScheduleProps {
    schedule: LoanScheduleItem;
    loanApplicationId: any;
}

const LoanSchedule: React.FC<LoanScheduleProps> = (props: any) => {
    const { schedule, loanApplicationId } = props;
    const [repaymentInput, setRepaymentInput] = useState<any>('');

    const handleAddRepayment = async () => {
        const repaymentAmount = parseFloat(repaymentInput || '0');
        const data = {
            paymentReceived: repaymentAmount
        }
        const response = await loanRepaymentService.applyPayment(loanApplicationId, data);
        setRepaymentInput('');
        window.location.reload();
    };

    return (
        <div className="w-100">
            {/* <div className="p-5">
                <div className='d-flex flex-row'>
                    <input
                        className='form-control w-300px'
                        type="number"
                        placeholder="Repayment Amount"
                        value={repaymentInput}
                        onChange={(e) => setRepaymentInput(e.target.value)}
                    />
                    <button className='btn btn-primary' onClick={handleAddRepayment}>Add Repayment</button>
                </div>
            </div> */}
            <div className="d-flex flex-row justify-content-around border-bottom py-5">
                <div className="w-5">
                    <span>#</span>
                </div>
                <div className="d-flex flex-column"><span className="text-gray-700">Interest for Month</span></div>
                <div className="d-flex flex-column"><span className="text-gray-700">Principal</span></div>
                <div className="d-flex flex-column"><span className="text-gray-700">Interest Due</span></div>
                <div className="d-flex flex-column"><span className="text-gray-700">EMI Amount</span></div>
                <div className="d-flex flex-column"><span className="text-gray-700">Status</span></div>
            </div>
            {schedule && schedule.map((item: LoanScheduleItem, index: number) => (
                <div
                    key={item.startDate}
                    className="d-flex flex-row justify-content-around border-bottom py-5"
                >
                    <div className="w-5">
                        {index + 1}
                    </div>
                    <div className="d-flex flex-column">

                        <span> {moment(item.startDate).format('MMM YY')}</span>
                    </div>

                    <div className="d-flex flex-column">

                        <span>{(item.principalAmount).toFixed(2)}</span>
                    </div>
                    <div className="d-flex flex-column">

                        <span>{item.interestAmount.toFixed(2)}</span>
                    </div>
                    <div className="d-flex flex-column">

                        <span>{item.amount.toFixed(2)}</span>
                    </div>
                    {/* <div className="d-flex flex-column">
                        <span>Total Payment</span>
                        <span> 0</span>
                    </div> */}
                    {/* <div className="d-flex flex-column">
                        <span>Remaining Balance</span>
                        <span className="fw-bold">{item.balance.toFixed(2)}</span>
                    </div> */}
                    <div className="d-flex flex-column w-100px">

                        <span className={clsx('badge fw-normal', item.paymentStatus == "Pending" ? "badge-primary" :
                            item.paymentStatus == "Paid" ? "badge-success" : "badge-warning")}>{item.paymentStatus}</span>
                    </div>
                </div>
            ))}
        </div>
    );
};

export default LoanSchedule;
