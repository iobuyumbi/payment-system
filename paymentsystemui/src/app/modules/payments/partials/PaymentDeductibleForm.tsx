import React, { useEffect, useState } from "react";
import { Formik, Form, Field, FieldArray, ErrorMessage } from "formik";
import * as Yup from "yup";
import ProcessingFeeService from "../../../../services/ProcessingFeeService";
import LoanBatchService from "../../../../services/LoanBatchService";
import ProjectDropdown from "../../../components/ProjectDropdown";
import {
  ValidationField,
  ValidationSelect,
  ValidationTextArea,
} from "../../../../_shared/components";
import { Link, useNavigate, useParams } from "react-router-dom";

import DatePicker from "react-datepicker";
import MasterLoanTermService from "../../../../services/MasterLoanTermsService";
import { paymentDeductibleInitValues } from "../../../../_models/deductible-model";
import { useSelector } from "react-redux";
import DeductiblePaymentService from "../../../../services/DeductibleService";
const deductibleService = new DeductiblePaymentService();

// Validation schema
const validationSchema = Yup.object().shape({
  // systemId: Yup.string().required("System ID is required"),
  // beneficiaryId: Yup.string().required("Beneficiary ID is required"),
  // carbonUnitsAccrued: Yup.number().required("Carbon Units Accrued is required"),
  // unitCostEur: Yup.number().required("Unit Cost (EUR) is required"),
  // totalUnitsEarningEur: Yup.number().required("Total Units Earning (EUR) is required"),
  // totalUnitsEarningLc: Yup.number().required("Total Units Earning (LC) is required"),
  // solidaridadEarningsShare: Yup.number().required("Partners Adminstrative Cost is required"),
  // farmerEarningsShareEur: Yup.number().required("Farmer Earnings Share (EUR) is required"),
  // farmerEarningsShareLc: Yup.number().required("Farmer Earnings Share (LC) is required"),
  // farmerPayableEarningsLc: Yup.number().required("Farmer Payable Earnings (LC) is required"),
  // farmerLoansDeductionsLc: Yup.number().required("Farmer Loans Deductions (LC) is required"),
  // farmerLoansBalanceLc: Yup.number().required("Farmer Loans Balance (LC) is required"),
  // remarks: Yup.string().required("Remarks are required"),
});

type Props = {
  title: string;
};

const PaymentDeductibleForm = () => {
  const { id } = useParams();
 
  const navigate = useNavigate();
  const [data, setData] = useState<any>(paymentDeductibleInitValues);

  const bindDetails = async () => {
    const response = await deductibleService.getSingleDeductiblePayment(id);
  
    if (response) setData(response);
  };

  const handleSubmit = async (values: typeof paymentDeductibleInitValues) => {
    const result = id
      ? await deductibleService.putDeductiblePayments(values, id)
      : await deductibleService.postDeductiblePayments(values);
    if (result.id) navigate(`/payment-batch/details/${values.paymentBatchId}`);
  };

 

  useEffect(() => {
    if (id) {
      document.title = "Edit payment";
      bindDetails();
    } else {
      document.title = "Add payment";
      setData(paymentDeductibleInitValues);
    }
  }, []);

  return (
    <div className="p-6">
      <Formik
        enableReinitialize={true}
        initialValues={data}
        validationSchema={validationSchema}
        onSubmit={handleSubmit}
      >
        {({ isSubmitting }) => (
          <Form placeholder={"Payment Form"}>
            <div className="row mb-5">
              <div className="col-md-6">
                <label
                  htmlFor="systemId"
                  className="form-label fw-bolder text-gray-800 fs-6 mb-2"
                >
                  System ID
                </label>
                <Field
                  name="systemId"
                  type="text"
                 
                  className="form-control"
                  placeholder="Enter batch name"
                />
                <ErrorMessage name="systemId" component="div" />
              </div>
              {/* add payment batch id above yourself asdasdddfadfadsfasdff*/}
              <div className="col-md-6">
                <label
                  htmlFor="beneficiaryId"
                  className="form-label fw-bolder text-gray-800 fs-6 mb-2"
                >
                  Beneficiary ID
                </label>
                <Field
                  name="beneficiaryId"
                  type="text"
                
                  className="form-control"
                  placeholder="Enter batch name"
                />
                <ErrorMessage name="beneficiaryId" component="div" />
              </div>
            </div>
            <div className="row mb-5">
              <div className="col-md-6">
                <label
                  htmlFor="carbonUnitsAccrued"
                  className="form-label fw-bolder text-gray-800 fs-6 mb-2"
                >
                  Carbon Units Accrued
                </label>
                <Field
                  name="carbonUnitsAccured"
                  type="number"
                  className="form-control"
                  placeholder="Enter batch name"
                />
                <ErrorMessage name="carbonUnitsAccrued" component="div" />
              </div>
              <div className="col-md-6">
                <label
                  htmlFor="unitCostEur"
                  className="form-label fw-bolder text-gray-800 fs-6 mb-2"
                >
                  Unit Cost (EUR)
                </label>
                <Field
                  name="unitCostEur"
                  type="number"
                  className="form-control"
                  placeholder="Enter Cost (EUR)"
                />
                <ErrorMessage name="unitCostEur" component="div" />
              </div>
            </div>
            <div className="row mb-5">
              <div className="col-md-6">
                <label
                  htmlFor="totalUnitsEarningEur"
                  className="form-label fw-bolder text-gray-800 fs-6 mb-2"
                >
                  Total Units Earning (EUR)
                </label>
                <Field
                  name="totalUnitsEarningEur"
                  type="number"
                  className="form-control"
                  placeholder="Enter batch name"
                />
                <ErrorMessage name="totalUnitsEarningEur" component="div" />
              </div>
              <div className="col-md-6">
                <label
                  htmlFor="totalUnitsEarningLc"
                  className="form-label fw-bolder text-gray-800 fs-6 mb-2"
                >
                  Total Units Earning (LC)
                </label>
                <Field
                  name="totalUnitsEarningLc"
                  type="number"
                  className="form-control"
                  placeholder="Enter batch name"
                />
                <ErrorMessage name="totalUnitsEarningLc" component="div" />
              </div>
            </div>
            <div className="row mb-5">
              <div className="col-md-6">
                <label
                  htmlFor="solidaridadEarningsShare"
                  className="form-label fw-bolder text-gray-800 fs-6 mb-2"
                >
                  Partners Adminstrative Cost
                </label>
                <Field
                  name="solidaridadEarningsShare"
                  type="number"
                  className="form-control"
                  placeholder="Enter batch name"
                />
                <ErrorMessage name="solidaridadEarningsShare" component="div" />
              </div>
              <div className="col-md-6">
                <label
                  htmlFor="farmerEarningsShareEur"
                  className="form-label fw-bolder text-gray-800 fs-6 mb-2"
                >
                  Farmer Earnings Share (EUR)
                </label>
                <Field
                  name="farmerEarningsShareEur"
                  type="number"
                  className="form-control"
                  placeholder="Enter batch name"
                />
                <ErrorMessage name="farmerEarningsShareEur" component="div" />
              </div>
            </div>
            <div className="row mb-5">
              <div className="col-md-6">
                <label
                  htmlFor="farmerEarningsShareLc"
                  className="form-label fw-bolder text-gray-800 fs-6 mb-2"
                >
                  Farmer Earnings Share (LC)
                </label>
                <Field
                  name="farmerEarningsShareLc"
                  type="number"
                  className="form-control"
                  placeholder="Enter batch name"
                />
                <ErrorMessage name="farmerEarningsShareLc" component="div" />
              </div>
              <div className="col-md-6">
                <label
                  htmlFor="farmerPayableEarningsLc"
                  className="form-label fw-bolder text-gray-800 fs-6 mb-2"
                >
                  Farmer Payable Earnings (LC)
                </label>
                <Field
                  name="farmerPayableEarningsLc"
                  type="number"
                  className="form-control"
                  placeholder="Enter batch name"
                />
                <ErrorMessage name="farmerPayableEarningsLc" component="div" />
              </div>
            </div>
            <div className="row mb-5">
              <div className="col-md-6">
                <label
                  htmlFor="farmerLoansDeductionsLc"
                  className="form-label fw-bolder text-gray-800 fs-6 mb-2"
                >
                  Farmer Loans Deductions (LC)
                </label>
                <Field
                  name="farmerLoansDeductionsLc"
                  type="number"
                  className="form-control"
                  placeholder="Enter batch name"
                />
                <ErrorMessage name="farmerLoansDeductionsLc" component="div" />
              </div>
              <div className="col-md-6">
                <label
                  htmlFor="farmerLoansBalanceLc"
                  className="form-label fw-bolder text-gray-800 fs-6 mb-2"
                >
                  Farmer Loans Balance (LC)
                </label>
                <Field
                  name="farmerLoansBalanceLc"
                  type="number"
                  className="form-control"
                  placeholder="Enter batch name"
                />
                <ErrorMessage name="farmerLoansBalanceLc" component="div" />
              </div>
            </div>
            <div className="row mb-5">
              <div className="col-md-6">
                <label
                  htmlFor="remarks"
                  className="form-label fw-bolder text-gray-800 fs-6 mb-2"
                >
                  Remarks
                </label>
                <Field
                  name="remarks"
                  type="textarea"
                  className="form-control"
                  placeholder="Enter remarks"
                />
                <ErrorMessage name="remarks" component="div" />
              </div>
            </div>
            <div className="row mb-5"></div>
            <div className="card-footer d-flex justify-content-center py-6 px-9">
              <button
                type="submit"
                className="btn btn-theme"
                disabled={isSubmitting}
              >
                Submit
              </button>
              <Link
                to={`/payment-batch/details/${data.paymentBatchId}`}
                className=" btn btn-light mx-3"
              >
                Cancel
              </Link>
            </div>
          </Form>
        )}
      </Formik>
    </div>
  );
};

export default PaymentDeductibleForm;
