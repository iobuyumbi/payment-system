import { useEffect, useState } from "react";
import { toast } from "react-toastify";
import CustomComboBox from "../../../../_shared/CustomComboBox/Index";
import { OptionType } from "../../../../_models/option-type";
import FarmingInputService from "../../../../services/FarmingInputService";
import { useFormik } from "formik";
import * as Yup from "yup";
import { toastNotify } from "../../../../services/NotifyService";
import {
  CreateLoanBatchItemModel,
  loanBatchItemInit,
} from "../../../../_models/loan-batch-item-model";
import {
  ValidationField,
  ValidationSelect,
  ValidationTextArea,
} from "../../../../_shared/components";
import CommonService from "../../../../services/CommonService";
import LoanBatchService from "../../../../services/LoanBatchService";
import { isAllowed } from "../../../../_metronic/helpers/ApiUtil";

const farmingInputService = new FarmingInputService();
const loanBatchService = new LoanBatchService();
const commonService = new CommonService();

// Helper type for objects like { value: string; label: string }
type SelectOption = { value: string; label: string };

const itemSchema = Yup.object().shape({
  // loanBatch: Yup.mixed()
  //   .required("Loan batch is required")
  //   .test("loanBatch-has-value", "Loan batch is required", (value) => {
  //     const v = value as SelectOption;
  //     return !!(v && v.value);
  //   }),

  loanItem: Yup.mixed()
    .required("Loan item is required")
    .test("loanItem-has-value", "Loan item is required", (value) => {
      const v = value as SelectOption;
      return !!(v && v.value);
    }),

  unit: Yup.mixed()
    .required("Unit is required")
    .test("unit-has-value", "Unit is required", (value) => {
      const v = value as SelectOption;
      return !!(v && v.value);
    }),

  quantity: Yup.number()
    .required("Quantity is required")
    .positive("Quantity should be greater than 0")
    .max(100000, "Quantity is unrealistically high"),

  isFree: Yup.boolean().required("IsFree flag is required"),

  unitPrice: Yup.number()
    .nullable()
    .when("isFree", {
      is: false,
      then: (schema) =>
        schema
          .required("Unit price is required")
          .min(0, "Unit price cannot be negative")
          .max(1000000, "Unit price is unrealistically high"),
      otherwise: (schema) => schema.nullable(),
    }),

  supplierDetails: Yup.string()
    .max(1000, "Cannot exceed 1000 characters")
    .nullable(),
});

export default function AddLoanBatchItem(props: any) {
  const { loanBatchId, loanBatchName, afterConfirm, editData, itemsData } = props;
  const [loading, setLoading] = useState(false);
  const [loanItems, setLoanItems] = useState<OptionType[]>([]);
  const [units, setUnits] = useState<OptionType[]>([]);
  const [data, setData] = useState<CreateLoanBatchItemModel>(
    editData !== undefined && editData !== null ? editData : loanBatchItemInit
  );

  const [itemError, setItemError] = useState("");
  const bindItems = async () => {
    const searchParams: any = {
      pageNumber: 1,
      pageSize: 10,
    };
    const response = await farmingInputService.getFarmingInputData(
      searchParams
    );
    const options: OptionType[] = response.map((opt: any) => ({
      value: opt.id,
      label: opt.itemName,
    }));
    setLoanItems(options);
  };

  const bindUnits = async () => {
    const response = await commonService.getItemUnits();
    setUnits(response);
  };

  const formik = useFormik<CreateLoanBatchItemModel>({
    enableReinitialize: true,
    initialValues: data,
    validationSchema: itemSchema,
    onSubmit: (values) => {

      const toastId = toast.loading("Please wait...");
      setLoading(true);
      setTimeout(async () => {
        var loanBatch = {
          value: loanBatchId,
          label: loanBatchName,
        };
        const free = String(values.isFree) === "true" ? true : false;
        const updatedData = { ...data, ...values, loanBatch, isFree: free };
        setData(updatedData);

        if (editData !== undefined && editData !== null) {
          const result = await loanBatchService.putLoanBatchItem(
            updatedData,
            editData.id
          );
          if (result && result.id) {
            toastNotify(toastId, "Success");
            setLoading(false);
            afterConfirm();
          } else {
            toast.error("Something went wrong");
          }
          toast.dismiss(toastId);
        } else {
          const result = await loanBatchService.saveLoanBatchItem(updatedData);
          if (result && result.id) {
            toastNotify(toastId, "Success");
            setLoading(false);
            afterConfirm();
          } else {
            toast.error("Something went wrong");
          }
          toast.dismiss(toastId);
        }
      }, 1000);
    },
  });

  const onCancel = () => {
    afterConfirm(false);
  };

  useEffect(() => {
    bindUnits();
    bindItems();
  }, []);

  const [price, setPrice] = useState(false);
  useEffect(() => {
    const isFreeString = String(formik.values.isFree);

    if (isFreeString === "false") {
      setPrice(true);
    } else {
      setPrice(false);
      formik.setFieldValue("unitPrice", 0);
    }
  }, [formik.values.isFree]);

  useEffect(() => {
    if (editData === undefined || editData === null) {
      const isDuplicate = itemsData.some(
        (item: { loanItemId: string }) =>
          item.loanItemId === formik.values.loanItem?.value
      );

      if (isDuplicate) {
        setItemError("Item already exists in the batch");
      } else {
        setItemError(""); // clear error
      }
    }
    // setPrice(!isDuplicate); // Set price flag depending on match
    // if (!isDuplicate) {
    //   formik.setFieldValue("unitPrice", 0);
    // }}
  }, [formik.values.loanItem, itemsData]);

  // useEffect(() => {
  //   console.log("Formik Errors:", formik.errors);
  //   console.log("Formik Touched:", formik.touched);
  // }, [formik.errors, formik.touched]);

  return (
    <div>
      {isAllowed("settings.loans.items.add") ? (
        <form onSubmit={formik.handleSubmit} noValidate className="form">
          <div className="modal-body">
            <div className="row mb-5">
              <div className="col-md-5">
                <CustomComboBox
                  label="Loan Item"
                  name="loanItem"
                  formik={formik}
                  options={loanItems}
                  isMulti={false}
                  disabled={editData !== undefined && editData !== null}
                />
                {itemError ? (
                  <div className="fv-plugins-message-container">
                    <div className="fv-help-block">
                      {itemError}
                    </div>
                  </div>
                ) : null}
              </div>
            </div>
            <div className="row mb-5">
              <ValidationField
                className="col-md-5"
                label="Max quantity (per farmer)"
                isRequired
                name="quantity"
                type="number"
                placeholder="Enter max quantity (per farmer)"
                formik={formik}
              />
              <div className="col-md-5 pt-1">
                <CustomComboBox
                  label="Unit"
                  name="unit"
                  isRequired
                  formik={formik}
                  options={units}
                  isMulti={false}
                />
              </div>
            </div>
            <div className="row mb-5">
              <ValidationSelect
                className="col-md-5"
                label="Is item free?"
                name="isFree"
                defaultValue={data.isFree}
                options={[
                  { id: false, name: "No " },
                  { id: true, name: "Yes" },
                ]}
                formik={formik}
              />
            </div>
            {price === true && (
              <div className="row mb-5">
                <ValidationField
                  className="col-md-5"
                  label="Unit price"
                  isRequired
                  name="unitPrice"
                  type="number"
                  placeholder="Enter unit price"
                  formik={formik}
                />
              </div>
            )}
            <div className="row mb-5">
              <ValidationTextArea
                className="col-md-10"
                label="Supplier details"
                name="supplierDetails"
                placeholder="Enter supplier details"
                formik={formik}
              />
            </div>
          </div>
          <div className="modal-footer">
            <button
              onClick={() => onCancel()}
              type="button"
              className="btn btn-light"
              data-bs-dismiss="modal"
            >
              Cancel
            </button>

            <button
              type="submit"
              className="btn btn-theme"
              disabled={
                loading || formik.isSubmitting || itemError !== ""
              }
            >
              <span className="indicator-label">Submit</span>
              {formik.isSubmitting && (
                <span className="indicator-progress">
                  Please wait...{" "}
                  <span className="spinner-border spinner-border-sm align-middle ms-2"></span>
                </span>
              )}
            </button>
            {/* <button onClick={() => onOk()} type="button" className="btn btn-theme">
                                {!loading && 'Save'}
                                {loading && (
                                    <span className='indicator-progress' style={{ display: 'block' }}>
                                        Please wait...{' '}
                                        <span className='spinner-border spinner-border-sm align-middle ms-2'></span>
                                    </span>
                                )}
                            </button> */}
          </div>
        </form>
      ) : (
        ""
      )}
    </div>
  );
}
