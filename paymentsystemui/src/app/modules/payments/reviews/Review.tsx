import { KTCard, KTCardBody, KTIcon } from '../../../../_metronic/helpers';
import { Content } from '../../../../_metronic/layout/components/content';
import { ToolbarWrapper } from '../../../../_metronic/layout/components/toolbar';
import { PageLink, PageTitle } from '../../../../_metronic/layout/core';
import { useSelector } from 'react-redux';
import PaymentInfoCard from '../cards/paymentInfoCard';
import { ErrorMessage, Field, Form, Formik } from 'formik';
import { useState } from 'react';
import { reviewValidationSchema } from '../../../../_shared/validations';
import { IConfirmModel } from '../../../../_models/confirm-model';
import { useNavigate } from 'react-router-dom';
import { ConfirmBox } from '../../../../_shared/Modals/ConfirmBox';
import PaymentBatchService from '../../../../services/PaymentBatchService';
import { toast } from 'react-toastify';
import StageHistory from '../partials/StageHistory';
import PaymentBatchStatusBadge from '../../../components/PaymentBatchStatusBadge';
import saveAs from 'file-saver';
import * as XLSX from "xlsx";
import ExportService from '../../../../services/ExportService';
import moment from 'moment';
import ListDeductiblePayments from '../partials/ListDeductiblePayments';

const breadCrumbs: Array<PageLink> = [
    {
        title: "Dashboard",
        path: "/dashboard",
        isSeparator: false,
        isActive: true,
    },
    {
        title: "",
        path: "",
        isSeparator: false,
        isActive: true,
    },
    {
        title: "Payment Batches",
        path: "/payment-batch",
        isSeparator: false,
        isActive: true,
    },
    {
        title: "",
        path: "",
        isSeparator: false,
        isActive: true,
    },
];
const exportService = new ExportService();
const paymentBatchService = new PaymentBatchService();

const Review = () => {
    const navigate = useNavigate();
    const batch = useSelector((state: any) =>
        state?.paymentBatches,
    )
    const initValues = {
        action: '',
        remarks: ''
    };
    const [initialValues, setInitialValues] = useState(initValues);
    const [showConfirmBox, setShowConfirmBox] = useState<boolean>(false);
    const [confirmModel, setConfirmModel] = useState<IConfirmModel>();
    const [loading, setLoading] = useState(false);


    const onSubmit = async (values: any, { setSubmitting }: any) => {
        alert('Work in progress...');
    }

    const downloadInterimList = async () => {
        const workbook = XLSX.utils.book_new();
        
        const model = { statusId: 1, batchId: batch.id }
        const response = await exportService.getDeductiblePayments(model);

        if (response !== undefined && response !== null) {
            const mappedData = response.map((row: any) => ({
                "System Id": row.systemId,
                "Farmer Name": row.farmer.firstName + " " + row.farmer.otherNames,
                "Payment Phone Number": row.farmer.paymentPhoneNumber,
                "Beneficiary Id": row.beneficiaryId,
                "Carbon Units Accured": row.carbonUnitsAccured,
                "Unit Cost Eur": row.unitCostEur,
                "Total Units Earning Eur": row.totalUnitsEarningEur,
                "Total Units Earning Lc": row.totalUnitsEarningLc,
                "Partners Adminstrative Cost": row.solidaridadEarningsShare,
                "Farmer Earnings Share Eur": row.farmerEarningsShareEur,
                "Farmer Earnings Share Lc": row.farmerEarningsShareLc,
                "Farmer Payable Earnings Lc": row.farmerPayableEarningsLc,
                "Farmer Loans Deductions Lc": row.farmerLoansDeductionsLc,
                "Farmer Loans Balance Lc": row.farmerLoansBalanceLc,

            }));

            const worksheet = XLSX.utils.json_to_sheet(mappedData);
            XLSX.utils.book_append_sheet(workbook, worksheet, "PaymentList");
            const blob = new Blob(
                [XLSX.write(workbook, { bookType: "xlsx", type: "array" })],
                {
                    type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                }
            );
            saveAs(blob, `${batch?.batchName}_${moment(new Date()).format('DD_MM_YYYY_HHmmss')}`);
        }
    };

    const openConfirmBox = async (values: any, { setSubmitting }: any) => {
        setInitialValues(values);

        const confirmModel: IConfirmModel = {
            title: "Confirm action",
            btnText: "Update status",
            deleteUrl: ``,
            message: "confirm-stage-approval",
        };

        setConfirmModel(confirmModel);
        setTimeout(() => {
            setShowConfirmBox(true);
        }, 500);
    };

    const afterConfirm = async (res: any) => {
        if (res == false) {
            setShowConfirmBox(false);
        } else {
            const response = await paymentBatchService.updatePaymentStage(initialValues, batch.id);
            if (response.id && initialValues.action === "review_next") {
                navigate(`/payment-batch/approve/${batch.id}`);
            }
            else if (response.id && initialValues.action !== "review_next") {
                navigate(`/payment-batch`);
            } else {
                toast.error('Could not update. Please try again');
            }
        }
    };


    return (
        <>
            <ToolbarWrapper />
            <PageTitle breadcrumbs={breadCrumbs}>{'Review Payments'}</PageTitle>
            <Content>
                <KTCard className='mb-5'>
                    {/* begin::Header */}
                    <div className='card-header'>
                        <h4 className="card-title flex-row">
                            <span className="card-label fs-1 mb-1">
                                Payment Batch Review -  {" "}
                                {`${batch?.batchName}`}
                            </span>
                            <PaymentBatchStatusBadge statusText={batch?.status?.stageText} />
                        </h4>
                        <div className="card-toolbar">
                            <button className="btn btn-dark btn-sm" onClick={() => downloadInterimList()}>
                                <KTIcon iconName={'exit-down'} /> Download Interim Payment List
                            </button>
                        </div>
                    </div>
                    {/* end::Header */}
                    <KTCardBody>
                        <div className="row">
                            <div className="col-md-5">
                                <PaymentInfoCard batch={batch} />
                            </div>
                            <div className="col-md-7">
                                <Formik
                                    initialValues={initialValues}
                                    enableReinitialize={true}
                                    validationSchema={reviewValidationSchema}
                                    onSubmit={openConfirmBox}
                                >
                                    {({ values, isSubmitting, setFieldValue, ...formikProps }) => (
                                        <Form placeholder={undefined}>
                                            <div className="row mb-5">
                                                <div className="col-md-6 form-group">
                                                    <label className="form-label fw-bolder text-gray-800 fs-6 mb-1">Review status <span className='text-danger'>*</span></label>
                                                    <Field as="select" name="action" className="block w-full form-control">
                                                        <option value="" label="Select" />
                                                        <option value="review_rejected" label="Review Rejected" />
                                                        <option value="review_next" label="Send for Approval" />
                                                    </Field>
                                                    <ErrorMessage name="action" component="div" className="text-danger mt-2" />
                                                </div>
                                            </div>
                                            <div className="row mb-5">
                                                <div className="col-md-6">
                                                    <label className="form-label fw-bolder text-gray-800 fs-6 mb-1">Remarks <span className='text-danger'>*</span></label>
                                                    <Field
                                                        as='textarea'
                                                        name='remarks'
                                                        className='form-control form-control-lg'
                                                        rows={3}
                                                    ></Field>
                                                    <ErrorMessage name="remarks" component="div" className="text-danger mt-2" />
                                                </div>
                                            </div>
                                            <div className="text-gray-700">
                                                <button
                                                    type="submit"
                                                    className="btn btn-primary">
                                                    Submit
                                                </button>
                                            </div>
                                        </Form>
                                    )}
                                </Formik>
                            </div>
                        </div>
                    </KTCardBody>
                </KTCard>

                <div className='row mb-5'>
                    <div className='col-md-12'>
                        {batch.id && <ListDeductiblePayments batchId={batch.id} />}
                    </div>
                </div>

                <StageHistory id={batch.id} />
            </Content>
            {showConfirmBox && (
                <ConfirmBox
                    confirmModel={confirmModel}
                    afterConfirm={afterConfirm}
                    loading={loading}
                    btnType={'theme'}
                />
            )}
        </>

    )
}

export default Review