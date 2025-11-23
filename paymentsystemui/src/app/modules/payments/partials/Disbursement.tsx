import { useEffect, useState } from "react";
import { toast } from "react-toastify";
import * as Yup from "yup";
import { useFormik } from "formik";
import { Link, useNavigate, useParams } from "react-router-dom";
import { PageLink } from "../../../../_metronic/layout/core";
import {
  RequiredMarker,
  ValidationField,
  ValidationSelect,
} from "../../../../_shared/components";

import CountryService from "../../../../services/CountryService";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import { useDispatch, useSelector } from "react-redux";
import { toastNotify } from "../../../../services/NotifyService";
import { isAllowed } from '../../../../_metronic/helpers/ApiUtil';
import { Error401 } from "../../errors/components/Error401";
import PaymentBatchService from "../../../../services/PaymentBatchService";
import { PaymentBatchModel } from "../../../../_models/payment-batch-model";
import LoanBatchService from "../../../../services/LoanBatchService";
import { resetPaymentBatchState } from "../../../../_features/payment-batch/paymentBatchSlice";
import FarmerService from "../../../../services/FarmerService";

const countryData = new CountryService();
const loanBatchService = new LoanBatchService();
const paymentBatchService = new PaymentBatchService();

const paymentBatchSchema = Yup.object().shape({
  batchName: Yup.string()
    .required("Batch Name is required")
    .max(50, "Cannot exceed 50 characters"),
  dateCreated: Yup.string().required("Date is required"),
  loanBatchId: Yup.string().required("Loan Product is required"),
  countryId: Yup.string().required("Required"),
});

export function Disbursement(props: any) {
  const { farmers } = props
  const navigate = useNavigate();
  const dispatch = useDispatch();

  var batches = useSelector((state: any) => state?.paymentBatches);


  const [data, setData] = useState<PaymentBatchModel>({
    ...batches,
  });

  const { id } = useParams();
  const [title] = useState<any>(id == null ? "Add" : "Edit");
  const [countries, setCountries] = useState<any>();
  const [loading, setLoading] = useState(false);



  const formik = useFormik<PaymentBatchModel>({
    enableReinitialize: true,
    initialValues: data,
    validationSchema: paymentBatchSchema,
    onSubmit: (values) => {
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
            navigate("/payment-batch");
            toastNotify(id, "Success");
            setLoading(false);
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

  const getCountries = async () => {
    try {
      countryData.getCountryData(1).then((data: any) => {
        if (data && data.length > 0) {
          const list = data.map((country: any) => ({
            id: country.id,
            name: country.countryName,
          }));
          setCountries(list);
        }
      });
    } catch (error) {
      console.error("Error fetching countries:", error);
    }
  };

  const [rowData, setRowData] = useState<any>();




  const [loanBatches, setLoanBatches] = useState<any>();
  const bindBatches = async () => {
    const data: any = {
      pageNumber: 1,
      pageSize: 1000,
      countryId: null
    }
    const response: any = await loanBatchService.getLoanBatchData(data);

    if (Array.isArray(response)) {
      response.unshift({ id: '0', name: 'Select batch' });
    }
    setLoanBatches(response);
  }
  const bindFarmers = async () => {

    const data = farmers.map((item: any) => ({ id: item.id, name: item.fullName }));

    setRowData(data)

  };
  useEffect(() => {
    if (title === "Add") {
      dispatch(resetPaymentBatchState());
    } else {
    }
    getCountries();
    bindBatches();
    bindFarmers();

  }, []);

  return (
    <>
      {isAllowed("payments.batch.add") ? (
        <>

          <div className="pt-2">
            <div>
              <form onSubmit={formik.handleSubmit} noValidate className="form">
                <div className=" mb-5 mb-xl-10">
                  <div className=" p-9">
                    <RequiredMarker />

                    {/* 1 */}
                    <div className="row mb-6">
                      <ValidationSelect
                        className="col-md-5"
                        label="Farmer"
                        isRequired
                        name="farmerId"
                        options={rowData !== undefined ? rowData : ""}
                        formik={formik}
                      />
                      <ValidationSelect
                        className="col-md-5"
                        label="Payment method"
                        isRequired
                        name="methodId"
                        options={[{ id: '0', name: "Bank payment" }, { id: '1', name: 'Paypal' }]}
                        formik={formik}
                      />


                    </div>

                    {/* 2 */}
                    <div className="row mb-6">
                      <ValidationField
                        className="col-md-5"
                        label="Amount"
                        isRequired
                        name="amount"
                        type="number"
                        placeholder="Enter amount"
                        formik={formik}
                      />
                      <ValidationField
                        className="col-md-5"
                        label="Currency"
                        isRequired
                        name="currency"

                        formik={formik}
                      />
                    </div>
                    {/* 3 */}
                    <div className="row mb-6">


                      <div className="col-md-5 ">
                        <label className="fs-6 fw-bold"> Date</label>
                        <div className="col-12 mt-2">
                          {/* <DatePicker
                            selected={formik.values.dateCreated ?? new Date()}
                            className="form-control"
                            name="date"
                            onChange={(date) =>
                              formik.setFieldValue("date", date)
                            }
                          /> */}
                        </div>
                      </div>
                      <ValidationSelect
                        className="col-md-5"
                        label="Status"
                        isRequired
                        name="statuId"
                        options={[{ id: '0', name: "Pending" }, { id: '1', name: 'Done' }]}
                        formik={formik}
                      />
                    </div>

                    {/* Footer */}
                    <div className="card-footer d-flex justify-content-center py-6 px-9">
                      <button
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
                      </button>
                      <Link to={"/payment-batch-detail/associates"} className=" btn btn-light mx-3">
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
    </>
  );
}
