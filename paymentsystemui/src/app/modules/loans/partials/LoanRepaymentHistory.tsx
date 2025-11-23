import React, { useEffect, useState } from 'react';
import LoanRepaymentService from '../../../../services/LoanRepaymentService';
import moment from 'moment';
import config from '../../../../environments/config';

const loanRepaymentService = new LoanRepaymentService();

const LoanRepaymentHistory = ({ loanApplicationId }: { loanApplicationId: any }) => {
    const [repaymentInput, setRepaymentInput] = useState<any>('');
    const [data, setData] = useState<any>();

    const handleAddRepayment = async () => {
        const repaymentAmount = parseFloat(repaymentInput || '0');
        const data = {
            loanApplicationId: loanApplicationId,
            repaymentAmount: repaymentAmount
        }
        const response = await loanRepaymentService.saveLoanRepayment(data);
        // if (isNaN(repaymentAmount) || repaymentAmount <= 0) return;

        // const lastPrincipal = repayments.length
        //     ? repayments[repayments.length - 1].remainingPrincipal
        //     : loanAmount;

        // const monthlyRate = interestRate / 12 / 100;
        // const monthlyInterest = lastPrincipal * monthlyRate;
        // const newPrincipal = Math.max(0, lastPrincipal + monthlyInterest - repaymentAmount);

        // setRepayments([
        //     ...repayments,
        //     { repaymentAmount, monthlyInterest, remainingPrincipal: newPrincipal },
        // ]);
        // setRemainingPrincipal(newPrincipal);
        // setRepaymentInput('');
    };

    const bindHistory = async () => {
        const data = {
            pageNumber: 1,
            pageSize: 10,
        };
        const response = await loanRepaymentService.getLoanRepaymentHistory(loanApplicationId);
        setData(response);
    };

    useEffect(() => {
        bindHistory();
    }, []);

    return (<>
        {/* <div className='d-flex flex-row'>
            <input
                className='form-control w-300px'
                type="number"
                placeholder="Repayment Amount"
                value={repaymentInput}
                onChange={(e) => setRepaymentInput(e.target.value)}
            />
            <button className='btn btn-primary' onClick={handleAddRepayment}>Add Repayment</button>
        </div> */}
        <div className="my-5">
            <table className="table align-middle table-row-dashed fs-6 gy-5 dataTable no-footer">
                <thead>
                    <tr className="text-start text-muted fw-bolder fs-7 text-uppercase gs-0">
                        <th>Date</th>
                        <th className="text-end">Amount Paid</th>
                        <th className="text-end">Payment Mode</th>
                        <th className="text-end">Reference Number</th>
                    </tr>
                </thead>
                <tbody>
                    {data && data.length > 0 ? (
                        data.map((item: any, index: any) => (
                            <tr key={index}>
                                <td className="text-gray-600 fw-bold ">
                                    {moment(item.paymentDate).format(config.dateOnlyFormat)}
                                </td>
                                <td className="text-end">
                                    {item.amountPaid?.toLocaleString("en-US", { minimumFractionDigits: 2, maximumFractionDigits: 2 })}
                                </td>
                                <td className="text-end">
                                    {item.paymentMode}
                                </td>
                                <td className="text-end">
                                    {item.referenceNumber}
                                </td>
                            </tr>
                        ))
                    ) : (
                        <tr>
                            <td colSpan={4}></td>
                        </tr>
                    )}
                </tbody>
            </table>
        </div>
    </>
    );
};


export default LoanRepaymentHistory;
