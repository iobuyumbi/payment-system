import { useEffect, useState } from "react";
import { toast } from "react-toastify";
import * as Yup from "yup";
import { useFormik } from "formik";
import { PageTitleWrapper } from "../../../_metronic/layout/components/toolbar/page-title";
import { Link, useNavigate, useParams } from "react-router-dom";
import { PageTitle, PageLink } from "../../../_metronic/layout/core";
import {
  RequiredMarker,
  ValidationField,
} from "../../../_shared/components";

import { Content } from "../../../_metronic/layout/components/content";
import "react-datepicker/dist/react-datepicker.css";
import { useDispatch, useSelector } from "react-redux";
import { toastNotify } from "../../../services/NotifyService";
import { isAllowed } from "../../../_metronic/helpers/ApiUtil";
import { Error401 } from "../errors/components/Error401";
import PaymentBatchService from "../../../services/PaymentBatchService";
import { PaymentBatchModel, paymentBatchInitValues } from "../../../_models/payment-batch-model";
import LoanBatchService from "../../../services/LoanBatchService";
import { resetPaymentBatchState } from "../../../_features/payment-batch/paymentBatchSlice";
import CustomComboBox from "../../../_shared/CustomComboBox/Index";
import ProjectService from "../../../services/ProjectService";
import { KTIcon } from "../../../_metronic/helpers";
import { ImportModal } from "../../../_shared/Modals/ImportModal";
import { getSelectedCountryCode } from "../../../_metronic/helpers/AppUtil";


const loanBatchService = new LoanBatchService();
const projectService = new ProjectService();
const paymentBatchService = new PaymentBatchService();

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

const paymentBatchSchema = Yup.object().shape({
  batchName: Yup.string()
    .required("Batch Name is required")
    .max(50, "Cannot exceed 50 characters"),

  projectIds: Yup.array()
    .of(
      Yup.object({
        value: Yup.string().required("Project is invalid"),
        label: Yup.string(),
      })
    )
    .min(1, "At least one Project must be selected")
    .required("Project is required"),

  loanBatchIds: Yup.array()
    .of(
      Yup.object({
        value: Yup.string().required("Loan Product is invalid"),
        label: Yup.string(),
      })
    ).optional(),
  // .when('paymentModule', {
  //   is: 3,
  //   then: schema =>
  //     schema
  //       .min(1, "At least one Loan Product must be selected")
  //       .required("Loan Product is required"),
  //   otherwise: schema => schema.notRequired(),
  // }),

  paymentModule: Yup.number().optional(),
});


export function AddPaymentBatch() {
  const navigate = useNavigate();
  const dispatch = useDispatch();

  var batches = useSelector((state: any) => state?.paymentBatches);

  const { id } = useParams();
  const [title] = useState<any>(id == null ? "Add" : "Edit");
  const [data, setData] = useState<PaymentBatchModel>({
    ...batches,
  });

  const [loading, setLoading] = useState(false);
  const [module, setModule] = useState<any>("3");
  const [batchId, setBatchId] = useState<any>();
  const [view, setView] = useState<any>();
  const afterConfirm = (value: any) => {
    setView(value);
  };

  const formik = useFormik<PaymentBatchModel>({
    enableReinitialize: true,
    initialValues: data,
    validationSchema: paymentBatchSchema,
    onSubmit: (values, { resetForm }) => {
      // confirm only for deductible batch
      if (values.paymentModule === 3) {
        if (values.loanBatchIds == null || values.loanBatchIds.length == 0) {
          const isConfirmed = window.confirm("Are you sure you want to proceed without a loan product?");
          if (!isConfirmed) return;
        }
      }

      const toastId = toast.loading("Please wait...");
      setLoading(true);
      setTimeout(async () => {
        const updatedData = { ...data, ...values };
        setData(updatedData);

        if (title === "Add") {
          const result = await paymentBatchService.postPaymentBatchData(
            updatedData
          );

          if (result && result.id) {
            // navigate("/payment-batch");
            setModule(values.paymentModule);
            setBatchId(result.id);
            toastNotify(id, "Success");
            setLoading(false);
            setView(true);
            resetForm();
          } else {
            toast.error("Something went wrong");
          }
          toast.dismiss(toastId);
        } else {
          const result = await paymentBatchService.putPaymentBatchData(
            updatedData,
            id
          );

          if (result && result.id) {
            navigate("/payment-batch");
            toastNotify(id, "Success");
            setLoading(false);
          } else {
            toast.error("Something went wrong");
          }
          toast.dismiss(toastId);
        }
      }, 1000);
    },
  });

  const [loanBatches, setLoanBatches] = useState<any>();
  const bindBatches = async (projectIds: any) => {
    const searchParams: any[] = [];

    projectIds.map((opt: any) => searchParams.push(opt.value));

    const response = await loanBatchService.searchByProjects(searchParams);

    const options: any[] = response.map((opt: any) => ({
      value: opt.loanBatch.id,
      label: opt.loanBatch.name,
    }));
    setLoanBatches(options);
  };

  const [projects, setProjects] = useState<any>();
  const bindProjects = async (countryId: string) => {
    const data: any = {
      pageNumber: 1,
      pageSize: 10000,
      countryId: null,
    };
    const response = await projectService.getProjectByCountryData(data);

    const options: any[] = response.map((opt: any) => ({
      value: opt.id,
      label: opt.projectName,
    }));
    setProjects(options);
  };

  useEffect(() => {
    if (id == null) {
      dispatch(resetPaymentBatchState());
      setData(paymentBatchInitValues);
    }
  }, []);

  useEffect(() => {
    if (formik.values.projectIds && formik.values.projectIds.length > 0) {
      bindBatches(formik.values.projectIds);
    }
  }, [formik.values.projectIds]);

  const countryCode = getSelectedCountryCode()

  useEffect(() => {
    bindProjects(countryCode || '');
  }, [countryCode])

  return (
    <Content>
      {isAllowed("payments.batch.add") ? (
        <>
          <PageTitleWrapper />
          <PageTitle breadcrumbs={breadCrumbs}>{title}</PageTitle>

          <div className="pt-2">
            <div>
              <form onSubmit={formik.handleSubmit} noValidate className="form">
                <div className="card mb-5 mb-xl-10">
                  <div className="card-body p-9">
                    <RequiredMarker />

                    {/* path way  */}
                    {formik.values.paymentModule === 0 && (
                      <>
                        {" "}
                        <label className="fs-3 fs-6 mb-2">
                          Choose Payment Pathway
                        </label>
                        <div className="d-flex align-items-center justify-content-around  m-4">
                          <div className="flex-1 w-100 mx-2">
                            {/* Deductible Card */}
                            <div
                              className="card mb-3 bg-light-primary"
                              onClick={() =>
                                formik.setFieldValue("paymentModule", 3)
                              }
                              style={{
                                cursor: "pointer",
                                border: "1px solid #ccc",
                                transition:
                                  "transform 0.2s ease-in-out, border-color 0.2s ease-in-out",
                              }}
                              onMouseEnter={(e) =>
                              (e.currentTarget.style.border =
                                "2px solid lightblue")
                              }
                              onMouseLeave={(e) => {
                                if (formik.values.paymentModule !== 3) {
                                  e.currentTarget.style.border =
                                    "1px solid #ccc";
                                }
                              }}
                              {...formik.getFieldProps("paymentModule")}
                            >
                              <div className="card-body">
                                <div className="d-flex">
                                  <KTIcon iconName="cheque" className="me-3 fs-2x" />
                                  <h1 className="card-title fw-normal ">
                                    {" "}
                                    Deductible Payments
                                  </h1>
                                </div>

                                <ul className="fw-normal  fs-4 my-2">
                                  <li>Refers to the interest & payments made on certain types of loans</li>
                                </ul>
                              </div>
                            </div>
                          </div>
                          <div className="flex-1 w-100 mx-2">
                            {/* Facilitation Card */}

                            <div
                              className="card mb-3 bg-light-info"
                              onClick={() =>
                                formik.setFieldValue("paymentModule", 4)
                              }
                              style={{
                                cursor: "pointer",

                                border: "1px solid #ccc",
                                transition:
                                  "transform 0.2s ease-in-out, border-color 0.2s ease-in-out",
                              }}
                              onMouseEnter={(e) =>
                              (e.currentTarget.style.border =
                                "2px solid lightblue")
                              }
                              onMouseLeave={(e) => {
                                if (formik.values.paymentModule !== 4) {
                                  e.currentTarget.style.border =
                                    "1px solid #ccc";
                                }
                              }}
                              {...formik.getFieldProps("paymentModule")}
                            >
                              <div className="card-body  ">
                                <div className="d-flex">
                                  <KTIcon iconName="tag" className="me-3 fs-2x" />
                                  <h1 className="card-title fw-normal ">
                                    {" "}
                                    Facilitation Payments
                                  </h1>
                                </div>
                                <ul className="fw-normal fs-4 my-2">
                                  <li>Refers to the payments made outside the primary loans and payment system</li>
                                </ul>
                              </div>
                            </div>
                          </div>
                        </div>
                      </>
                    )}

                    {/* PATHWAY END  */}

                    {formik.values.paymentModule !== 0 && (
                      <>
                        <div className="d-flex justify-content-between">
                          <h2 className="mt-5 fw-normal">
                            {formik.values.paymentModule === 3
                              ? "Deductible Payments"
                              : "Facilitation Payments"}
                          </h2>
                          {id == null && (
                            <button
                              type="button"
                              onClick={() =>
                                formik.setFieldValue("paymentModule", 0)
                              }
                              className=" btn btn-sm btn-theme px-4 me-1 w-100px h-40px me-3 "
                            >
                              <i className="bi bi-arrow-left"></i> Back
                            </button>
                          )}
                        </div>

                        <div className="row mb-6 mt-10">
                          <ValidationField
                            className="col-md-5"
                            label="Batch Name"
                            isRequired
                            name="batchName"
                            type="text"
                            placeholder="Enter batch name"
                            formik={formik}
                          />

                          {formik.values.paymentModule !== 0 && <div className="col-md-5">
                            <CustomComboBox
                              label="Project(s)"
                              name="projectIds"
                              formik={formik}
                                isRequired
                              options={projects}
                              isMulti={true}
                            />
                          </div>}

                          {/* <div className="col-md-5 offset-md-1">
                            <CountryDropdown
                              formik={formik}
                              isRequired={true}
                              isSelected={true}
                            />
                          </div> */}
                        </div>
                      </>
                    )}

                    <div className="row mb-6">
                      {/* <ValidationSelect
                        className="col-md-5"
                        label="Loan Product"
                        isRequired
                        name="loanBatchId"
                        options={loanBatches !== undefined && loanBatches}
                        formik={formik}
                      
                      /> */}

                      {formik.values.paymentModule === 3 && (<div className="col-md-5 ">
                        <CustomComboBox
                          label="Loan Product(s)"
                          name="loanBatchIds"
                          formik={formik}
                          options={loanBatches}
                          isMulti={true}
                        />
                      </div>)}
                    </div>


                    {/* Footer */}
                    <div className="card-footer d-flex justify-content-center py-6 px-9">
                      {formik.values.paymentModule !== 0 && <button
                        type="submit"
                        className="btn btn-theme"
                        disabled={loading}
                      >
                        <span className="indicator-label">Submit</span>
                        {formik.isSubmitting && (
                          <span className="indicator-progress">
                            Please wait...{" "}
                            <span className="spinner-border spinner-border-sm align-middle ms-2"></span>
                          </span>
                        )}
                      </button>}

                      <Link
                        to={"/payment-batch"}
                        className=" btn btn-light mx-3"
                      >
                        Cancel
                      </Link>
                    </div>
                  </div>
                </div>
              </form>
            </div>
          </div>
        </>
      ) : (
        <Error401 />
      )}
      {view && (
        <ImportModal
          exModule={module === 3 ? "PaymentDeductibles" : "PaymentFacilitations"}
          title={"Payments"}
          afterConfirm={afterConfirm}
          paymentBatchId={batchId}
          loanBatchIds={data.loanBatchIds}
          loanBatchName={formik.values.batchName}
        />
      )}
    </Content>
  );
}
