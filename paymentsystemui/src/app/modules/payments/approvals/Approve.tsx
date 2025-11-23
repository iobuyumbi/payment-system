import { KTCard, KTCardBody, KTIcon } from '../../../../_metronic/helpers';
import { Content } from '../../../../_metronic/layout/components/content';
import { ToolbarWrapper } from '../../../../_metronic/layout/components/toolbar';
import { PageLink, PageTitle } from '../../../../_metronic/layout/core';
import { useParams } from 'react-router-dom';
import saveAs from 'file-saver';
import * as XLSX from "xlsx";
import moment from 'moment';
import { useEffect, useState } from 'react';
import PaymentBatchApproval from './PaymentBatchApproval';
import PaymentBatchStatusBadge from '../../../components/PaymentBatchStatusBadge';
import ListDeductiblePayments from '../partials/ListDeductiblePayments';
import ExportService from '../../../../services/ExportService';
import PaymentBatchService from '../../../../services/PaymentBatchService';
import StageHistory from '../partials/StageHistory';

const breadCrumbs: Array<PageLink> = [
    { title: "Dashboard", path: "/dashboard", isSeparator: false, isActive: true },
    { title: "Payment Batches", path: "/payment-batch", isSeparator: false, isActive: true },
];

const paymentBatchService = new PaymentBatchService();
const exportService = new ExportService();

const Approve = () => {
    const { paymentBatchId } = useParams();
    const [batch, setBatch] = useState<any | null>(null);
    const [loading, setLoading] = useState<boolean>(true);

    const bindBatchDetail = async () => {
        setLoading(true);
        try {
            const response = await paymentBatchService.getSingle(paymentBatchId);
            if (response) {
                setBatch(response);
            }
        } catch (error) {
            console.error("Failed to bind batch detail:", error);
            alert("An error occurred while loading batch details. Please try again.");
        } finally {
            setLoading(false);
        }
    };

    const downloadInterimList = async () => {
        try {
            const workbook = XLSX.utils.book_new();
            const model = { statusId: 1, batchId: paymentBatchId };
            const response = await exportService.getDeductiblePayments(model);

            if (Array.isArray(response)) {
                const mappedData = response.map((row: any) => ({
                    "System Id": row.systemId,
                    "Farmer Name": `${row.farmer.firstName} ${row.farmer.otherNames}`,
                    "Payment Phone Number": row.farmer.paymentPhoneNumber,
                    "Beneficiary Id": row.beneficiaryId,
                    "Carbon Units Accrued": row.carbonUnitsAccrued,
                    "Unit Cost EUR": row.unitCostEur,
                    "Total Units Earning EUR": row.totalUnitsEarningEur,
                    "Total Units Earning LC": row.totalUnitsEarningLc,
                    "Partners Adminstrative Cost": row.solidaridadEarningsShare,
                    "Farmer Earnings Share EUR": row.farmerEarningsShareEur,
                    "Farmer Earnings Share LC": row.farmerEarningsShareLc,
                    "Farmer Payable Earnings LC": row.farmerPayableEarningsLc,
                    "Farmer Loans Deductions LC": row.farmerLoansDeductionsLc,
                    "Farmer Loans Balance LC": row.farmerLoansBalanceLc,
                    "Payment Complete": row.isPaymentComplete ? "Yes" : "No",
                }));

                const worksheet = XLSX.utils.json_to_sheet(mappedData);
                XLSX.utils.book_append_sheet(workbook, worksheet, "PaymentList");

                const blob = new Blob(
                    [XLSX.write(workbook, { bookType: "xlsx", type: "array" })],
                    { type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" }
                );

                saveAs(blob, `${batch?.batchName || "PaymentList"}_${moment().format('DD_MM_YYYY_HHmmss')}.xlsx`);
            } else {
                alert("No data available to download.");
            }
        } catch (error) {
            console.error("Failed to download interim list:", error);
            alert("An error occurred while downloading the list. Please try again.");
        }
    };

    useEffect(() => {
        if (paymentBatchId) {
            bindBatchDetail();
        }
    }, [paymentBatchId]);

    return (
        <>
            <ToolbarWrapper />
            <PageTitle breadcrumbs={breadCrumbs}>Approve Payments</PageTitle>
            <Content>
                {loading ? (
                    <div className="text-center mt-5">
                        <p>Loading batch details...</p>
                    </div>
                ) : (
                    <KTCard>
                        <div className="card-header">
                            <h4 className="card-title flex-row">
                                <span className="card-label fs-1 mb-1">
                                    Payment Batch Approval - {batch?.batchName || "N/A"}
                                </span>
                                <PaymentBatchStatusBadge statusText={batch?.status?.stageText || "Unknown"} />
                            </h4>
                            <div className="card-toolbar">
                                <button className="btn btn-dark btn-sm" onClick={downloadInterimList}>
                                    <KTIcon iconName="exit-down" /> Download Interim Payment List
                                </button>
                            </div>
                        </div>
                        <KTCardBody>
                            <PaymentBatchApproval batch={batch} />
                        </KTCardBody>
                    </KTCard>
                )}

                <div className="row mt-5">
                    <div className="col-md-12">
                        {paymentBatchId && <ListDeductiblePayments batchId={paymentBatchId} />}
                    </div>
                </div>

                <div className="mt-5">
                    <StageHistory id={paymentBatchId} />
                </div>
            </Content>
        </>
    );
};

export default Approve;
