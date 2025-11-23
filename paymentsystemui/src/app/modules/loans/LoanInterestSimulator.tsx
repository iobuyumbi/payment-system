import { useState } from "react";
import LoanApplicationService from "../../../services/LoanApplicationService";
import moment from "moment";
import config from "../../../environments/config";
import CustomTable from "../../../_shared/CustomTable/Index";

// interface LoanInterestResult {
//     calculationMonth: string;
//     accruedInterest: number;
//     monthlyPayment: number;
//     remainingPrincipal: number;
// }

const loanApplicationService = new LoanApplicationService();

const LoanInterestSimulator = (props: any) => {
    const { loanApplicationId } = props;
    const [calculations, setCalculations] = useState<any[]>([]);
    const [loading, setLoading] = useState(false);
    const [rowData, setRowData] = useState<any>();

    const [colDefs, setColDefs] = useState<any>([
        {
            field: "date", flex: 1, valueFormatter: function (params: any) {
                return params.value ? moment(params.value).format(config.dateOnlyFormat) : null;
            },
        },
        { field: "description", headerName: "description", flex: 1 },
        { field: "EffectivePrincipal", flex: 1 },
        { field: "InterestAccrued", flex: 1 },
        { field: "PaymentReceived", flex: 1 },
        { field: "PrincipalPaid", flex: 1 },
        { field: "InterestPaid", flex: 1 },
        { field: "CumulativeBalance", flex: 1 },
    ]);

    const fetchInterestData = async () => {
        setLoading(true);
        try {
            const response = await loanApplicationService.calculateInterest(loanApplicationId);
            setCalculations(response);
        } catch (error) {
            console.error("Error fetching data:", error);
        }
        setLoading(false);
    };

    return (
        <div className="p-6 max-w-3xl mx-auto bg-white shadow-md rounded-lg">
            <h2 className="text-xl font-bold mb-4">Loan Interest Simulator</h2>

            <button
                onClick={fetchInterestData}
                className="btn btn-primary py-2 px-4"
                disabled={loading}
            >
                {loading ? "Calculating..." : "Simulate Interest Calculation"}
            </button>

            {calculations && calculations.length > 0 && (
                // <CustomTable
                //     rowData={rowData}
                //     colDefs={colDefs}
                // />
                <table className="mt-4 w-100 border-collapse border border-gray-300">
                    <thead>
                        <tr className="bg-gray-100">
                            <th className="border p-2">Date</th>
                            <th className="border p-2">Description</th>
                            <th className="border p-2">Effective Principal</th>
                            <th className="border p-2">Interest Accrued</th>
                            <th className="border p-2">Payment Received</th>
                            <th className="border p-2">Principal Paid</th>
                            <th className="border p-2">Interest Paid</th>
                            <th className="border p-2">Cumulative Balance</th>
                        </tr>
                    </thead>
                    <tbody>
                        {calculations.map((calc, index) => (
                            <tr key={index} className="text-center">
                                <td>
                                    {moment(calc.date).format(config.dateOnlyFormat)}
                                </td>
                                <td className="border p-2">
                                    {calc.description}
                                    {/* Interest for the month {moment(calc.calculationMonth).format('MMM, YYYY')} */}
                                </td>

                                <td className="border p-2">{calc.effectivePrincipal.toFixed(2)}</td>
                                <td className="border p-2">{calc.interestAccrued.toFixed(2)}</td>
                                <td className="border p-2">{calc.paymentReceived.toFixed(2)}</td>
                                <td className="border p-2">{calc.principalPaid.toFixed(2)}</td>
                                <td className="border p-2">{calc.interestPaid.toFixed(2)}</td>
                                <td className="border p-2">{calc.cumulativeBalance.toFixed(2)}</td>
                            </tr>
                        ))}
                    </tbody>
                </table>
            )}
        </div>
    );
};

export default LoanInterestSimulator;
