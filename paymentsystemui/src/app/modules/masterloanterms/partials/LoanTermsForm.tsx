import React, { useEffect, useState } from "react";
import { Formik, Form, Field, FieldArray, ErrorMessage } from "formik";
import * as Yup from "yup";
import ProcessingFeeService from "../../../../services/ProcessingFeeService";
import {
  ValidationField,
  ValidationSelect,
} from "../../../../_shared/components";
import { Link, useNavigate, useParams } from "react-router-dom";
import { masterTermsInitValues } from "../../../../_models/master-terms-model";
import MasterLoanTermService from "../../../../services/MasterLoanTermsService";

const processingFeeService = new ProcessingFeeService();
const masterLoanTermService = new MasterLoanTermService();

// Validation schema
const validationSchema = Yup.object({
  descriptiveName: Yup.string()
    .required("Name is required")
    .max(150, "Cannot exceed 150 characters"),

  interestRate: Yup.number()
    .typeError("Interest Rate must be a number")
    .required("Interest Rate is required")
    .moreThan(0, "Interest Rate must be greater than 0")
    .max(100, "Interest Rate cannot exceed 100"),

  interestRateType: Yup.string().required("Rate Type is required"),

  // interestApplication: Yup.string().required(
  //   "Please choose how interest is calculated"
  // ),

  tenure: Yup.number()
    .typeError("Tenure must be a number")
    .required("Please choose tenure")
    .moreThan(0, "Tenure must be greater than 0")
    .max(72, "Tenure cannot exceed 72 months"),

  gracePeriod: Yup.number()
    .typeError("Grace Period must be a number")
    .required("Please choose grace period")
    .min(0, "Grace Period cannot be less than 0")
    .max(36, "Grace Period cannot exceed 36 months"),

  maxDeductiblePercent: Yup.number()
    .typeError("Max Deductible Percent must be a number")
    .required("Please choose max deductible amount percent")
    .min(0, "Max Deductible Percent cannot be less than 0")
    .max(100, "Max Deductible Percent cannot exceed 100"),
});


type Props = {
  title: string;
};

const LoanTermsForm: React.FC<Props> = () => {
  const { id } = useParams();
  const navigate = useNavigate();

  const [data, setData] = useState<any>(masterTermsInitValues);
  const [feeOptions, setFeeOptions] = useState<any[]>([]);

  const bindProcessingFee = async () => {
    const response = await processingFeeService.getProcessingFee();
    if (response) setFeeOptions(response);
  };

  const bindDetails = async () => {
    const response = await masterLoanTermService.getSingleMasterLoanTermData(id);

    if (response) {
      setData(response);
    } else {
      navigate('/master-loan-terms')
    }
  };

  const handleSubmit = async (values: typeof masterTermsInitValues) => {
    // Remove 'additionalFee' if all items have empty id
    values.interestApplication = "Yearly"; // Set default value for interestApplication
    if (Array.isArray(values.additionalFee)) {
      const filteredFees = values.additionalFee.filter(item => item.id !== "");

      if (filteredFees.length > 0) {
        values.additionalFee = filteredFees;
      } else {
        delete values.additionalFee;
      }
    }

    // alert(JSON.stringify(values)); return;
    const result =
      id !== undefined && id !== null
        ? await masterLoanTermService.putMasterLoanTermData(values, id)
        : await masterLoanTermService.postMasterLoanTermData(values);
    if (result.id) navigate("/master-loan-terms");
  };

  useEffect(() => {
    if (id) {
      document.title = "Edit Loan Product";
      bindDetails();
    } else {
      document.title = "Add Loan Product";
      setData(masterTermsInitValues);
    }

    bindProcessingFee();
  }, []);

  return (
    <div className="p-6">
      <Formik
        enableReinitialize={true}
        initialValues={data}
        validationSchema={validationSchema}
        onSubmit={handleSubmit}
      >
        {(formik) => {
          return (
            <Form className="space-y-4" placeholder={undefined}>
              <div className="row mb-5">
                <div className="col-md-6">
                  <ValidationField
                    label="Descriptive name"
                    isRequired
                    name="descriptiveName"
                    type="text"
                    placeholder="Enter name"
                    formik={formik}
                  />
                </div>
              </div>

              <div className="row mb-4 ">
                <ValidationSelect
                  className="col-md-6"
                  label="Interest rate type"
                  name="interestRateType"
                  defaultValue={data.rateType}
                  options={[
                    { id: "", name: "Select status" },
                    { id: "Flat", name: "Flat rate" },
                    { id: "Reducing", name: "Reducing balance" },
                  ]}
                  formik={formik}
                />
                <ValidationField
                  className="col-md-6"
                  label="Interest rate"
                  isRequired
                  name="interestRate"
                  type="number"
                  placeholder="Enter interest rate"
                  formik={formik}
                />
              </div>

              <div className="row mb-4 ">
                {/* <ValidationSelect
                  className="col-md-6"
                  label="Interest application"
                  name="interestApplication"
                  defaultValue={data.calculationTimeframe}
                  options={[
                    { id: "", name: "Select status" },
                    { id: "Yearly", name: "Yearly" },
                    { id: "Monthly", name: "Monthly" },
                  ]}
                  formik={formik}
                /> */}

                <ValidationField
                  className="col-md-6 "
                  label="Tenure (in month)"
                  isRequired
                  name="tenure"
                  type="number"
                  placeholder="Enter tenure"
                  formik={formik}
                />
                 <div className="row mb-4 ">
                <ValidationField
                  className="col-md-6"
                  label="Grace period (in months)"
                  isRequired
                  name="gracePeriod"
                  type="number"
                  placeholder="Enter grace periods"
                  formik={formik}
                />
              </div>

             

                <ValidationField
                  className="col-md-6"
                  label="CRU Payments Maximum deductible amount in %"
                  isRequired
                  name="maxDeductiblePercent"
                  type="number"
                  placeholder="Enter Maximum deductible amount"
                  formik={formik}
                />

                <div className="d-flex align-item-center my-4 ">
                  <label className="fw-bold fs-6 mb-2 me-3">
                    Has additional fees?
                  </label>
                  <Field
                    type="checkbox"
                    name="hasAdditionalFee"
                    checked={formik.values.hasAdditionalFee}
                    onChange={() =>
                      formik.setFieldValue(
                        "hasAdditionalFee",
                        !formik.values.hasAdditionalFee
                      )
                    }
                    className="me-2"
                  />
                </div>
              </div>

              {formik.values.hasAdditionalFee && (
                <div className="row mb-5">
                  <div className="col-md-12">
                    <label className="fs-6 fw-bold mb-2">
                      Add Additional Fee(s)
                    </label>
                    <FieldArray name="additionalFee">
                      {({ remove, push }) => (
                        <div>
                          {formik.values.additionalFee?.map(
                            (fee: any, index: number) => {
                              const selectedFeeTypes =
                                formik.values.additionalFee
                                  .map((feeItem: any, i: number) =>
                                    i !== index ? feeItem.feeName : null
                                  )
                                  .filter(Boolean);

                              const availableOptions = feeOptions.filter(
                                (option) =>
                                  !selectedFeeTypes.includes(option.feeName) ||
                                  option.feeName ===
                                  formik.values.additionalFee[index]?.feeName
                              );

                              return (
                                <div key={index} className="border p-4 mb-4">
                                  <div className="d-flex flex-start align-items-center">
                                    {/* Fee Name */}
                                    <div className="w-25 me-3">
                                      <label
                                        htmlFor={`additionalFee.${index}.feeName`}
                                        className="mb-1"
                                      >
                                        Fee Name
                                      </label>
                                      <Field
                                        as="select"
                                        name={`additionalFee.${index}.feeName`}
                                        className="form-select"
                                        onChange={(e: any) => {
                                          const selectedFee = feeOptions.find(
                                            (option) =>
                                              option.feeName === e.target.value
                                          );

                                          if (selectedFee) {
                                            formik.setFieldValue(
                                              `additionalFee.${index}.id`,
                                              selectedFee.id
                                            );
                                            formik.setFieldValue(
                                              `additionalFee.${index}.feeName`,
                                              selectedFee.feeName
                                            );
                                            formik.setFieldValue(
                                              `additionalFee.${index}.feeType`,
                                              selectedFee.feeType
                                            );
                                            formik.setFieldValue(
                                              `additionalFee.${index}.value`,
                                              selectedFee.value
                                            );
                                          }
                                        }}
                                      >
                                        <option value="">Select Fee</option>
                                        {availableOptions.map((option) => (
                                          <option
                                            key={option.feeName}
                                            value={option.feeName}
                                            selected={
                                              formik.values.additionalFee[index]
                                                .feeType === option.feeType
                                            } // Set the selected option based on formik value
                                          >
                                            {option.feeName}
                                          </option>
                                        ))}
                                      </Field>
                                      <ErrorMessage
                                        name={`additionalFee.${index}.feeType`}
                                        component="div"
                                        className="text-red-500"
                                      />
                                    </div>

                                    {/* Fee Type */}
                                    <div className="w-25 me-3">
                                      <label
                                        htmlFor={`additionalFee.${index}.feeType`}
                                        className="mb-1"
                                      >
                                        Fee type
                                      </label>
                                      <Field
                                        type="text"
                                        name={`additionalFee.${index}.feeType`}
                                        className="form-control"
                                        disabled
                                      />
                                      <ErrorMessage
                                        name={`additionalFee.${index}.feeType`}
                                        component="div"
                                        className="text-red-500"
                                      />
                                    </div>

                                    {/* Amount */}
                                    <div className="w-25 me-3">
                                      <label
                                        htmlFor={`additionalFee.${index}.value`}
                                        className="mb-1"
                                      >
                                        Amount
                                      </label>
                                      <Field
                                        type="number"
                                        name={`additionalFee.${index}.value`}
                                        className="form-control"
                                        disabled
                                      />
                                      <ErrorMessage
                                        name={`additionalFee.${index}.value`}
                                        component="div"
                                        className="text-red-500"
                                      />
                                    </div>

                                    {/* Remove Button */}
                                    <button
                                      type="button"
                                      onClick={() => remove(index)}
                                      className="btn btn-sm btn-outline btn-outline-danger mt-4"
                                    >
                                      Remove
                                    </button>
                                  </div>
                                </div>
                              );
                            }
                          )}

                          <button
                            type="button"
                            onClick={() =>
                              push({
                                id: "",
                                feeType: "",
                                value: "",
                              })
                            }
                            className="btn btn-primary mt-2"
                          >
                            Add Fee
                          </button>
                        </div>
                      )}
                    </FieldArray>
                  </div>
                </div>
              )}

              <div className="card-footer d-flex justify-content-center py-6 px-9">
                <button
                  type="submit"
                  className="btn btn-theme"
                  disabled={formik.isSubmitting}
                >
                  <span className="indicator-label">Submit</span>
                  {formik.isSubmitting && (
                    <span className="indicator-progress">
                      Please wait...{" "}
                      <span className="spinner-border spinner-border-sm align-middle ms-2"></span>
                    </span>
                  )}
                </button>
                <Link to={"/master-loan-terms"} className=" btn btn-light mx-3">
                  Cancel
                </Link>
              </div>
            </Form>
          );
        }}
      </Formik>
    </div>
  );
};

export default LoanTermsForm;
