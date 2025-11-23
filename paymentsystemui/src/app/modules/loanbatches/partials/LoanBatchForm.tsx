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
import { loanBatchInitValues } from "../../../../_models/loanbatch-model";
import DatePicker from "react-datepicker";
import MasterLoanTermService from "../../../../services/MasterLoanTermsService";
const masterLoanTermService = new MasterLoanTermService();
const processingFeeService = new ProcessingFeeService();
const loanBatchService = new LoanBatchService();

// Validation schema
const validationSchema = Yup.object({
  name: Yup.string()
    .required("Name is required")
    .max(150, "Cannot exceed 150 characters"),
  projectId: Yup.string().required("Project is required"),

  interestRate: Yup.number()
    .typeError("Interest Rate must be a number")
    .required("Interest Rate is required")
    .moreThan(0, "Interest Rate must be greater than 0")
    .max(100, "Interest Rate cannot exceed 100"),

  rateType: Yup.string().required("Rate Type is required"),
  calculationTimeframe: Yup.string().required(
    "Please choose how interest is calculated"
  ),
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


  // initiationDate: Yup.date().required("Initiation Date is required"),
  // effectiveDate: Yup.date().required("Initiation Date is required"),
  statusId: Yup.string()
    .max(36, "Cannot exceed 36 characters")
    .required("Select status"),
  paymentTerms: Yup.string()
    .max(500, "Cannot exceed 500 characters")
    .nullable(),
  // processingFees: Yup.array().of(
  //   Yup.object({
  //     feeType: Yup.string().required("Fee type is required"),
  //     amount: Yup.number()
  //       .required("Amount is required")
  //       .test(
  //         "amount-validation",
  //         "Percentage must be between 0 and 100, or flat amount cannot be negative",
  //         function (value) {
  //           const { feeType } = this.parent;
  //           const isPercentage = feeType?.toLowerCase().includes("percentage"); // Adjust based on your logic

  //           if (isPercentage) {
  //             return value !== undefined && value >= 0 && value <= 100;
  //           } else {
  //             return value !== undefined && value >= 0;
  //           }
  //         }
  // ),
  // })
  // ),
});

type Props = {
  title: string;
};

const LoanBatchForm: React.FC<Props> = () => {
  const { id } = useParams();
  const navigate = useNavigate();

  const [data, setData] = useState<any>(loanBatchInitValues);
  const [masterLoanTerms, setMasterLoanTerms] = useState<any>();
  const [feeOptions, setFeeOptions] = useState<any[]>([]);
  const [isDisabled, setIsDisabled] = useState<any>(false);
  const [hasAdditionalFee, setHasAdditionalFee] = useState<any>(false);
  const [errors, setErrors] = useState<any>({});

  const bindProcessingFee = async () => {
    const response = await processingFeeService.getProcessingFee();
    if (response) setFeeOptions(response);
  };

  const bindDetails = async () => {
    const response = await loanBatchService.getSingle(id);
    if (response) {
      console.log("Loan Product details response", response);
      setData(response);
      if (response.processingFees.length > 0) {
        setHasAdditionalFee(true);
      }
      if( response.totalApplications > 0 || response.totalItems > 0) {
        setErrors(["This loan product has existing applications or items. You cannot edit it."]);
        setIsDisabled(true);
      }
    }
  };

  const handleSubmit = async (values: typeof loanBatchInitValues) => {
    setErrors(null);
    values.calculationTimeframe =  "Yearly";
    const result = id
      ? await loanBatchService.putLoanBatchData(values, id)
      : await loanBatchService.postLoanBatchData(values);

    if (result.id) {
      navigate("/loans");
    } else {
      setErrors(result.errors || result.Errors);
    }
  };

  const bindMasterLoanTerms = async () => {
    const response = await masterLoanTermService.getMasterLoanTermData({});
    setMasterLoanTerms(response);
  };

  //   const handleMasterChange = (id : any) => {
  //     const selectedTerm = masterLoanTerms.find((term : any) => term.id === id);

  //     if (selectedTerm) {
  //         formik.setFieldValue('rateType', selectedTerm.InterestRateType);
  //         formik.setFieldValue('interestRate', selectedTerm.InterestRate);
  //         formik.setFieldValue('calculationTimeframe', selectedTerm.InterestApplication);
  //         formik.setFieldValue('tenure', selectedTerm.Tenure);
  //         formik.setFieldValue('gracePeriod', selectedTerm.GracePeriod);
  //       }
  //   };

  useEffect(() => {
    if (id) {
      document.title = "Edit Loan Product";
      bindDetails();
    } else {
      document.title = "Add Loan Product";
      setData(loanBatchInitValues);
    }
    bindMasterLoanTerms();
    bindProcessingFee();
  }, []);

  return (
    <div className="p-6">
      {/* errors */}
      {errors && errors.length > 0 && (
        <div className="mb-lg-15 alert alert-danger">
          <div className="alert-text font-weight-bold">
            {errors.map((err: any) => (
              <ul>
                <li>{err}</li>
              </ul>
            ))}
          </div>
        </div>
      )}
      <Formik
        enableReinitialize={true}
        initialValues={data}
        validationSchema={validationSchema}
        onSubmit={handleSubmit}
      >
        {(formik) => {
          // Get a list of all selected fee names to filter dropdown options
          const selectedFees = formik.values.processingFees.map(
            (fee: any) => fee.feeType
          );

          return (
            <Form className="space-y-4" placeholder={undefined}>
              <div className="row mb-5">
                <div className="col-md-6">
                  <label className="fw-bold fs-6 mb-2">Select loan term</label>
                  <select
                    className="form-select"
                    name="masterLoanTerms"
                    onChange={(e: React.ChangeEvent<HTMLSelectElement>) => {
                      const selectedTerm = masterLoanTerms.find(
                        (term: any) => term.id === e.target.value
                      );

                      if (selectedTerm) {
                        formik.setFieldValue(
                          "masterLoanTerms",
                          selectedTerm.id
                        );
                        formik.setFieldValue(
                          "rateType",
                          selectedTerm.interestRateType
                        );
                        formik.setFieldValue(
                          "interestRate",
                          selectedTerm.interestRate
                        );
                        formik.setFieldValue(
                          "calculationTimeframe",
                          selectedTerm.interestApplication
                        );
                        formik.setFieldValue("tenure", selectedTerm.tenure);
                        formik.setFieldValue(
                          "gracePeriod",
                          selectedTerm.gracePeriod
                        );
                        formik.setFieldValue(
                          "processingFees",
                          selectedTerm.additionalFee
                        );
                        formik.setFieldValue(
                          "maxDeductiblePercent",
                          selectedTerm.maxDeductiblePercent
                        );
                        setHasAdditionalFee(selectedTerm.hasAdditionalFee);
                      }
                    }}
                    onBlur={formik.handleBlur}
                  >
                    <option value="">Select an option</option>
                    {masterLoanTerms &&
                      masterLoanTerms.map((term: any) => (
                        <option key={term.id} value={term.id}>
                          {term.descriptiveName}
                        </option>
                      ))}
                  </select>
                  {/* Display validation error if needed */}
                  {/* {formik.touched.masterLoanTerms && formik.errors.masterLoanTerms && (
                    <div className="text-danger">{formik.errors.masterLoanTerms}</div>
                  )} */}
                </div>
              </div>

              <div className="row mb-5">
                <div className="col-md-6">
                  <label
                    htmlFor="name"
                    className="form-label fw-bolder text-gray-800 fs-6 mb-2"
                  >
                    Name <span className="text-danger">*</span>
                  </label>
                  <Field
                    name="name"
                    type="text"
                    className="form-control"
                    placeholder="Enter loan product name"
                  />
                  <ErrorMessage
                    name="name"
                    className="text-danger"
                    component="div"
                  />
                </div>
                <div className="col-md-6">
                  <ProjectDropdown
                    formik={formik}
                    isRequired={true}
                    isDisabled={isDisabled}
                    isCountryBased={true}
                  />
                </div>
              </div>

              <div className="row mb-4 ">
                <ValidationSelect
                  className="col-md-6"
                  label="Interest rate type"
                  name="rateType"
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
                  label="Interest rate (Per Annum)"
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
                  label="Interest is calculated"
                  name="calculationTimeframe"
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

              <div className="row mb-4 ">
                

                <ValidationField
                  className="col-md-6"
                  label="CRU Payments Maximum deductible amount in %"
                  isRequired
                  name="maxDeductiblePercent"
                  type="number"
                  placeholder="Enter Maximum deductible amount"
                  formik={formik}
                />
              </div>
              <div className="row mb-5">
                <div className="col-md-6">
                  <div className="row">
                    {/* <div className="col-md-6">
                      <label className="fs-6 fw-bold">
                        Effective Date <span className="text-danger">*</span>
                      </label>

                      <div className="col-md-12 mt-2">
                        <DatePicker
                          selected={formik.values.effectiveDate}
                          className="form-control w-100"
                          name="effectiveDate"
                          onChange={(date) =>
                            formik.setFieldValue("effectiveDate", date)
                          }
                        />
                        {formik.touched.effectiveDate &&
                          formik.errors.effectiveDate && (
                            <div className="text-danger mt-1">
                              {formik.errors.effectiveDate}
                            </div>
                          )}
                      </div>
                    </div> */}

                    {/* <div className="col-md-6">
                      <div className="row">
                        <div className="col-md-6">
                          <label className="fs-6 fw-bold">
                            Initiation Date{" "}
                            <span className="text-danger">*</span>
                          </label>
                        </div>
                        <div className="col-md-12 mt-2">
                          <DatePicker
                            selected={formik.values.initiationDate}
                            className="form-control"
                            name="initiationDate"
                            onChange={(date) =>
                              formik.setFieldValue("initiationDate", date)
                            }
                          />
                          {formik.touched.initiationDate &&
                            formik.errors.initiationDate && (
                              <div className="text-danger mt-1">
                                {formik.errors.initiationDate}
                              </div>
                            )}
                        </div>
                      </div>
                    </div> */}
                  </div>
                </div>
              </div>

              <div className="d-flex align-item-center my-4 ">
                <label className="fw-bold fs-6 mb-2 me-3">
                  Has additional fees?
                </label>
                <Field
                  type="checkbox"
                  name="hasAdditionalFee"
                  checked={hasAdditionalFee}
                  onChange={() => setHasAdditionalFee(!hasAdditionalFee)}
                  className="me-2"
                />
              </div>
              {hasAdditionalFee && (
                <div className="row mb-5">
                  <div className="col-md-12 ">
                    <label className="fs-6 fw-bold mb-2">
                      Additional Fee(s)
                    </label>
                    <FieldArray name="processingFees">
                      {({ remove, push }) => (
                        <div>
                          {formik.values.processingFees?.map(
                            (_: any, index: number) => {
                              const selectedFees = formik.values.processingFees
                                .map(
                                  (fee: any, i: number) =>
                                    i !== index && fee.feeName
                                )
                                .filter(Boolean); // exclude current and undefined

                              const availableOptions = feeOptions.filter(
                                (option) =>
                                  !selectedFees.includes(option.feeName) ||
                                  option.feeName ===
                                  formik.values.processingFees[index].feeName
                              );

                              return (
                                <div key={index} className="border p-4 mb-4">
                                  <div className="d-flex flex-start justify-content-justify align-items-center">
                                    <div className="w-25">
                                      <label
                                        htmlFor={`processingFees.${index}.value`}
                                        className="me-3 mb-1"
                                      >
                                        Fee Name
                                      </label>
                                      <Field
                                        as="select"
                                        name={`processingFees.${index}.feeName`}
                                        className="form-select p-2 w-300px"
                                        onChange={(
                                          e: React.ChangeEvent<HTMLSelectElement>
                                        ) => {
                                          const selectedFee = feeOptions.find(
                                            (option) =>
                                              option.feeName === e.target.value
                                          );

                                          if (selectedFee) {
                                            formik.setFieldValue(
                                              `processingFees.${index}.feeName`,
                                              selectedFee.feeName
                                            );
                                            formik.setFieldValue(
                                              `processingFees.${index}.feeType`,
                                              selectedFee.feeType
                                            );
                                            formik.setFieldValue(
                                              `processingFees.${index}.value`,
                                              selectedFee.value
                                            );
                                            formik.setFieldValue(
                                              `processingFees.${index}.percent`,
                                              selectedFee.percentage
                                            );
                                          }
                                        }}
                                      >
                                        <option value="">Select Fee</option>
                                        {availableOptions.map((option) => (
                                          <option
                                            key={option.feeName}
                                            value={option.feeName}
                                          >
                                            {option.feeName}
                                          </option>
                                        ))}
                                      </Field>
                                      <ErrorMessage
                                        name={`processingFees.${index}.feeName`}
                                        component="div"
                                        className="text-red-500"
                                      />
                                    </div>

                                    <div className="w-25">
                                      <label
                                        htmlFor={`processingFees.${index}.value`}
                                        className="me-3 mb-1"
                                      >
                                        Amount
                                      </label>
                                      <Field
                                        type="number"
                                        name={`processingFees.${index}.value`}
                                        className="block form-control w-75"
                                        disabled
                                      />
                                      <ErrorMessage
                                        name={`processingFees.${index}.value`}
                                        component="div"
                                        className="text-red-500"
                                      />
                                    </div>

                                    <div className="w-25">
                                      <label
                                        htmlFor={`processingFees.${index}.feeType`}
                                        className="me-3 mb-1"
                                      >
                                        Fee type
                                      </label>
                                      <Field
                                        type="text"
                                        name={`processingFees.${index}.feeType`}
                                        className="block form-control w-75"
                                        disabled
                                      />
                                      <ErrorMessage
                                        name={`processingFees.${index}.feeType`}
                                        component="div"
                                        className="text-red-500"
                                      />
                                    </div>

                                    <button
                                      type="button"
                                      onClick={() => remove(index)}
                                      className="btn btn-sm btn-outline btn-outline-danger px-4 mt-4"
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
                                feeType: "",
                                feeName: "",
                                value: 0,
                                percent: 0,
                              })
                            }
                            // disabled={
                            //   formik.values.processingFees.length >=
                            //   feeOptions.length
                            // }
                            className="btn btn-outline btn-outline-primary border border-dashed border-primary px-4 py-2"
                          >
                            Add Additional Fee
                          </button>
                        </div>
                      )}
                    </FieldArray>
                  </div>
                </div>
              )}

              <div className="row mb-5">
                <ValidationSelect
                  className="col-md-6"
                  label="Status"
                  name="statusId"
                  defaultValue={data.statusId}
                  isRequired
                  options={[
                    { id: "", name: "Select status" },
                    { id: "1", name: "Draft" },
                    { id: "2", name: "In Review" },
                    { id: "3", name: "Accepting Applications" },
                    { id: "4", name: "Closed" },
                  ]}
                  formik={formik}
                />
              </div>

              <div className="row mb-5">
                <div className="col-md-12 ">
                  <ValidationTextArea
                    className="col-md-12"
                    label="Notes"
                    name="paymentTerms"
                    type="text"
                    rows={6}
                    placeholder="Enter notes"
                    formik={formik}
                  />
                </div>
              </div>
              <div className="card-footer d-flex justify-content-center py-6 px-9">
                <button
                  type="submit"
                  className="btn btn-theme"
                 disabled={ errors?.length > 0 }
                >
                  <span className="indicator-label">Submit</span>
                  {formik.isSubmitting && (
                    <span className="indicator-progress">
                      Please wait...{" "}
                      <span className="spinner-border spinner-border-sm align-middle ms-2"></span>
                    </span>
                  )}
                </button>
                <Link to={"/loans/batches"} className=" btn btn-light mx-3">
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

export default LoanBatchForm;
