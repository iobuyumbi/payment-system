import { Fragment, useEffect, useState } from "react";
import FarmerService from "../../../services/FarmerService";
import { RequiredMarker, ValidationField } from "../../../_shared/components";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import { useFormik } from "formik";
import * as Yup from "yup";
import {
  LoanApplicationModel,
  initLoanApplicationValues,
} from "../../../_models/loan-application-model";
import { toast } from "react-toastify";
import { toastNotify } from "../../../services/NotifyService";
import { useNavigate } from "react-router-dom";
// import { useSelector } from "react-redux";
import LoanApplicationService from "../../../services/LoanApplicationService";
import LoanBatchService from "../../../services/LoanBatchService";
import { Table } from "react-bootstrap";
import DragAndDropFileUpload from "../../../_shared/DragAndDropFileUpload/Index";
import { isAllowed } from '../../../_metronic/helpers/ApiUtil';
import { Error401 } from "../errors/components/Error401";
import { Button } from "react-bootstrap";
import FarmerListModal from "../farmers/LIstFarmerModal";
import { OptionType } from "../../../_models/option-type";
import FormErrorAlert from "../../../_shared/FormErrorAlert/Index";
import UserService from "../../../services/UserService";

const farmerService = new FarmerService();
const applicationService = new LoanApplicationService();
const loanBatchService = new LoanBatchService();
const userService = new UserService();

const loanApplicationSchema = Yup.object().shape({
  witnessFullName: Yup.string()
    .required("Witness name is required")
    .max(50, "Cannot exceed 50 characters"),
  witnessRelation: Yup.string()
    .required("Witness relation is required")
    .max(100, "Cannot exceed 100 characters"),
  dateOfWitness: Yup.string().required("Date of witness is required"),
  witnessPhoneNo: Yup.string()
    .required("Witness phone number is required")
    .max(15, "Cannot exceed 15 characters"),
});
interface Props {
  loanBatch: any;
  afterConfirm: (confirmed: boolean) => void;
  isAdd: boolean;
  loanApplication?: any;
  reloadApplications: () => void;
}
const AddLoanBatchApplication = (props: Props) => {

  const { loanBatch, afterConfirm, isAdd, loanApplication, reloadApplications } = props;
  const inUse = isAdd === false ? loanApplication.inUse : false;
  const [searchTerm, setSearchTerm] = useState<string>("");
  const [farmers, setFarmers] = useState<any>();
  const [batchItems, setBatchItems] = useState<any>();
  const [loading, setLoading] = useState(false);
  const [attachmentIds, setAttachmentIds] = useState<any>();
  const [showListCompanyModal, setShowListCompanyModal] = useState<boolean>(false);

  const calculateInitialTotalPrice = (items: any[]) => {
    return items.reduce(
      (acc, item) => acc + item.quantity * item.unitPrice,
      0
    );
  };

  const [rowsData, setRowsData] = useState<any[]>(
    !isAdd ? loanApplication.loanItems : []
  );

  const [totalPrice, setTotalPrice] = useState<number>(
    !isAdd ? calculateInitialTotalPrice(loanApplication.loanItems) : 0
  );

  const initialValues = !isAdd
    ? {
      ...loanApplication,
      farmer: {
        label: loanApplication.farmer.fullName,
        value: loanApplication.farmer.id,
      },
    }
    : initLoanApplicationValues;

  const [value, setValue] = useState<any>({});
  const [maxQuantity, setMaxQuantity] = useState<any>();
  const [quantity, setQuantity] = useState<any>(null);
  const [qmessage, setQmessage] = useState<any>("");
  const [errorMessage, setErrorMessage] = useState("");
  const [loanOfficers, setLoanOfficers] = useState<any[]>([]);
  const [officerId, setOfficerId] = useState<any>(null);

  const navigate = useNavigate();
  const [errors, setErrors] = useState<any>([]);

  function hasFarmerKeys(obj: any): boolean {
    return obj && typeof obj === 'object' && 'value' in obj && 'label' in obj;
  }

  const formik = useFormik<LoanApplicationModel>({
    enableReinitialize: true,
    initialValues: initialValues,
    validationSchema: loanApplicationSchema,
    onSubmit: async (values) => {
      if (rowsData.length == 0) {
        alert('Please add loan items');
        return;
      }
      setLoading(true);

      try {

        const updatedData = {
          ...initialValues,
          ...values,
          loanBatchId: loanBatch.id,
          loanItems: rowsData,
          officerId: officerId?.id || "",
          createdOn:
            loanApplication !== undefined
              ? loanApplication?.createdOn
              : new Date().toISOString(),
          attachmentIds: attachmentIds,
        };

        console.log("Updated Data", updatedData);
        if (!hasFarmerKeys(updatedData.farmer)) {
          alert('Please select a farmer');
          return;
        }

        const toastId = toast.loading("Please wait...");

        if (isAdd) {
          const result = await applicationService.postLoanApplicationData(
            updatedData
          );

          if (result && result.id) {
            toastNotify(toastId, "Success");
            setLoading(false);

            afterConfirm(true);
            //navigate(`/loan-batch-details/${loanBatch.id}/loan-applications`);
            //navigate(`/loan-applications`);
            //reloadApplications();
            //window.location.reload();
          } else {
            //setErrors([...errors, result.message])
            setErrors(result.errors || result.Errors);
            console.log("Error", result);
            // toast.error("Something went wrong");
          }
          toast.dismiss(toastId);
        } else {
          const updateDate = { ...updatedData, updateOn: new Date() };
          const result = await applicationService.putLoanApplicationData(
            updateDate,
            loanApplication.id
          );

          if (result && result.id) {
            toastNotify(toastId, "Success");
            setLoading(false);

            afterConfirm(true);
            //window.location.reload();
            //navigate("/loan-batch-details/loan-applications");
          } else {
            //toast.error("Something went wrong");
            //errors.errorText = result.message;
            setErrors(result.errors);
          }
          toast.dismiss(toastId);
        }

      }
      catch (ex: any) {

      }
      finally {
        setLoading(false);
      }
    },
  });

  const bindFarmers = async () => {
    if (searchTerm.length === 0 || searchTerm.length > 2) {
      const data: any = {
        pageNumber: 1,
        pageSize: 10000,
      };

      var response = await farmerService.getFarmerData(data);

      if (response !== null && response.length > 0) {
        const options: any[] = response.map((opt: any) => ({
          value: opt.id,
          label: opt.fullName + " (" + opt.participantId + ")",
        }));
        setFarmers(options);
      }
    }
  };

  const bindItems = async () => {
    var response = await loanBatchService.getLoanBatchItems(loanBatch?.id);

    if (response !== null) {
      const options: any[] = response.map((opt: any) => ({
        id: opt.loanItem.value,
        itemName: opt.loanItem.label,
        maxQuantity: opt.quantity,
        unit: opt.unit.label,
        unitId: opt.unitId,
        unitPrice: opt.unitPrice,
      }));
      options.unshift({ id: 0, itemName: "Select items" });

      setBatchItems(options);
    }
  };

  const handleChange = (index: any, evnt: any) => {
    const { name, value } = evnt.target;
    let rowsInput = [...rowsData];

    const selectedItem = batchItems.filter(
      (product: any) => product.itemName === rowsInput[index]["itemName"]
    )[0];

    const currentRow = rowsInput[index];
    const previousQuantity = currentRow?.quantity || 0;

    // Calculate the updated quantity (respecting maxQuantity constraint)
    const updatedQuantity =
      selectedItem?.maxQuantity >= parseInt(value)
        ? parseInt(value)
        : selectedItem.maxQuantity;

    const updatedRow = {
      ...currentRow,
      [name]: updatedQuantity,
    };

    // Update rowsData
    rowsInput = [
      ...rowsInput.slice(0, index),
      updatedRow,
      ...rowsInput.slice(index + 1),
    ];
    setRowsData(rowsInput);

    // Update totalPrice
    const quantityDifference = updatedQuantity - previousQuantity;
    const priceDifference = quantityDifference * currentRow.unitPrice;
    setTotalPrice(totalPrice + priceDifference);
  };

  const deleteItem = (index: any) => {
    const rows = [...rowsData];
    const removedItem = rows[index];
    const itemTotalPrice = removedItem.quantity * removedItem.unitPrice;

    rows.splice(index, 1);
    setRowsData(rows);
    setTotalPrice(totalPrice - itemTotalPrice); // Update total price
  };

  const officerSelected = (val: any) => {
    const selectedItem = loanOfficers.filter(
      (product: any) => product.id === val
    )[0];
    setOfficerId(selectedItem);

  };


  const itemSelected = (val: any) => {
    const selectedItem = batchItems.filter(
      (product: any) => product.id === val
    )[0];
    setValue(selectedItem);
    setMaxQuantity(selectedItem?.maxQuantity ?? 0);
    setQmessage(`Max : ${selectedItem?.maxQuantity ?? 0}`);
  };

  const addItemToApplication = () => {
    setErrorMessage("");

    const selectedItem = batchItems?.filter(
      (product: any) => product?.id === value?.id
    )[0];
    if (selectedItem?.id == 0) { return; }
    if (quantity <= 0 || quantity == null || quantity === undefined) {
      setErrorMessage("Minimum quantity needs to be greater than 0")
      return;
    }
    if (quantity > selectedItem?.maxQuantity) {
      setErrorMessage(`Max quantity exceeded. Max allowed is ${selectedItem.maxQuantity}`);
      return;
    }
    const itemTotalPrice =
      (quantity <= selectedItem.maxQuantity
        ? quantity
        : selectedItem.maxQuantity) * selectedItem.unitPrice;

    const newRow: any = {
      masterLoanItemId: selectedItem.id,
      itemName: selectedItem.itemName,
      quantity:
        quantity <= selectedItem.maxQuantity
          ? quantity
          : selectedItem.maxQuantity,
      maxQuantity: selectedItem.maxQuantity,
      unit: selectedItem.unit,
      unitId: selectedItem.unitId,
      unitPrice: selectedItem.unitPrice,
    };

    if (
      rowsData.length === 0 ||
      rowsData.filter(
        (product: any) => product.masterLoanItemId === value.id
      )[0] === undefined
    ) {
      const rowsInput = [...rowsData, newRow];
      setRowsData(rowsInput);
      setTotalPrice(totalPrice + itemTotalPrice); // Update total price
      setQuantity(0);
      setQmessage("");
      setErrorMessage("");
      setValue({ id: 0, name: "Select items" });
    } else {
      setErrorMessage("The same item cannot be re-added.");
    }
  };

  const [farmer, setFarmer] = useState<OptionType>(
    !isAdd
      ? {
        label: loanApplication.farmer.fullName,
        value: loanApplication.farmer.id,
      }
      : { label: "", value: "" }
  );

  const onCancel = () => {
    afterConfirm(false);
  };

  const handleFarmerSelect = (value: any) => {
    formik.setFieldValue("farmer", {
      value: value.id,
      label: value.firstName + " " + value.otherNames,
    });
    setFarmer({
      value: value.id,
      label: value.firstName + " " + value.otherNames,
    });
  };

  const handleCompanyClose = () => {
    setShowListCompanyModal(false);
  };

  const fetchUsersWithPermission = async () => {

    const userData = await userService.getOfficers();
    if (userData && userData.length > 0) {


      setLoanOfficers(userData);

    };
  }



  useEffect(() => {
    fetchUsersWithPermission();
    bindFarmers();
    bindItems();
  }, [searchTerm]);


  useEffect(() => {
    formik.setFieldValue('principalAmount', totalPrice);
  }, [totalPrice]);

  return (
    <Fragment>
      {isAllowed("loans.applications.add") && !inUse ? (
        <div>
          <RequiredMarker />

          {/* errors */}
          <FormErrorAlert errors={errors} />

          <form onSubmit={formik.handleSubmit} noValidate className="form">
            <div className="row mb-6 ">
              <div className="row mt-5 mb-3">
                <div className="col-lg-6 fv-row">
                  <div className="d-flex flex-start justify-content-between">
                    <label
                      htmlFor="name"
                      className="form-label fw-bolder text-gray-800 fs-6 mt-5"
                    >
                      Farmer
                    </label>
                    {/* {farmer && <button type="button" className='btn btn-sm btn-outline btn-outline-dashed mb-3'>Remove</button>} */}
                  </div>
                  {farmer && (
                    <input
                      type="text"
                      className="form-control"
                      {...formik.getFieldProps("farmer.label")}
                      placeholder=""
                      readOnly
                    />
                  )}
                </div>
              </div>
              <div className="row mb-10">
                <div className="col-md-3">
                  <Button
                    className="btn w-100 gray-light-200 btn-outline btn-outline-dashed btn-outline-primary"
                    onClick={() => setShowListCompanyModal(true)}
                  >
                    <i className="las la-link fs-3 me-2"></i>Select farmer
                  </Button>
                </div>
                {/* <div className="col-md-3">
                                <Button className="btn w-100 btn-outline btn-outline-dashed btn-outline-primary btn-light-primary"
                                    onClick={() => setShowAddCompanyModal(true)}><i className="las la-plus fs-3 me-2"></i>Create new Company</Button>
                            </div> */}
                {/* <div className="col-lg-12">
                                <div className='mt-2 fs-6 text-gray-500'>
                                    <span>
                                        You make ignore it if you wish to create a contact as an individual.
                                    </span>
                                </div>
                            </div> */}
              </div>

              <div className="col-md-5 ">
                <label
                  htmlFor="name"
                  className="form-label fw-bolder text-gray-800 fs-6 mt-5"
                >
                  Business Development Officer
                </label>
                <select
                  name="values"
                  onChange={(e: any) => officerSelected(e.target.value)}
                  value={value?.id}
                  className="form-control mb-3 mb-lg-0"
                >
                  <option value="">Select Officer</option>
                  {loanOfficers &&
                    loanOfficers.map((opt: any) => (
                      <option key={opt.id} value={opt.id}>
                        {opt.username}
                      </option>
                    ))}
                </select>
              </div>
            </div>

            <div className="separator my-3"></div>

            <div className="row mb-6">
              <ValidationField
                className="col-md-5 "
                label="Witness full name"
                isRequired
                name="witnessFullName"
                type="text"
                placeholder="Enter witness full name"
                formik={formik}
              />
              <ValidationField
                className="col-md-5 offset-md-1"
                label="Witness Relation"
                name="witnessRelation"
                type="text"
                isRequired
                placeholder="Enter witness relation"
                formik={formik}
              />
            </div>

            {/* 4 */}
            <div className="row mb-6">
              <ValidationField
                className="col-md-5 "
                label="Witness phone number"
                isRequired
                name="witnessPhoneNo"
                type="text"
                placeholder="Enter witness phone no."
                formik={formik}
              />
              <div className="col-md-5 offset-md-1">
                <label className="fs-6 fw-bold"> Date of witness</label>
                <div className="col-12 mt-2">
                  <DatePicker
                    selected={formik.values.dateOfWitness ?? new Date()}
                    className="form-control"
                    name="dateOfWitness"
                    onChange={(date: any) => {
                      if (date instanceof Date) {
                        formik.setFieldValue("dateOfWitness", date);
                      }
                    }}
                  />
                </div>
              </div>
            </div>

            {/* 5 */}
            <h3>Items</h3>
            <hr className="my-4" />

            <div className="row mb-6">
              <div className="col-md-8 ">
                <label className="fs-6 fw-bold mb-1">Items</label>

                <select
                  name="values"
                  onChange={(e: any) => itemSelected(e.target.value)}
                  value={value?.id}
                  className="form-control mb-3 mb-lg-0"
                >
                  {batchItems &&
                    batchItems.map((opt: any) => (
                      <option key={opt.id} value={opt.id}>
                        {opt.itemName}
                      </option>
                    ))}
                </select>
              </div>
              <div className="col-md-2 ">
                <label className="fs-6 fw-bold mb-1">Quantity</label>
                <input
                  name="quantity"
                  type="number"
                  value={quantity}
                  max={maxQuantity !== undefined ? maxQuantity : ""}
                  placeholder="quantity"
                  className="form-control  mb-3 mb-lg-0"
                  onChange={(e: any) => setQuantity(e.target.value)}
                />
                <label>{qmessage}</label>
                {/* <label className="text-danger">
                  {errorMessage && errorMessage}</label> */}
              </div>

              <div className="col-md-2 ">
                <button
                  type="button"
                  onClick={() => addItemToApplication()}
                  className=" btn btn-theme mt-7"
                >
                  Add
                </button>
              </div>
            </div>

            {/* Error message displayed here */}
            {errorMessage && (
              <div className="text-danger mt-2">{errorMessage}</div>
            )}
            <div className="table-responsive">
              <div className="display mb-4 dataTablesCard customer-list-table">
                <Table className="table align-middle table-row-dashed fs-6 gy-5 dataTable no-footer">
                  <thead>
                    <tr className="text-start fw-bolder fs-7 text-uppercase gs-0">
                      <th>Items</th>
                      <th>Quantity</th>
                      {isAdd && <th className="text-center">Action</th>}
                    </tr>
                  </thead>
                  <tbody>
                    {rowsData && rowsData.length > 0 ? (
                      rowsData.map((item: any, index: any) => (
                        <tr key={index}>
                          <td className="">
                            <div className="d-flex align-items-center">
                              <div>
                                <p className=" mb-0"> {item.itemName}</p>
                              </div>
                            </div>
                          </td>
                          <td className=" w-100px">
                            <input
                              type="number"
                              name="quantity"
                              className="form-control "
                              id={`qty_${item.id}`}
                              min="1"
                              value={item.quantity}
                              max={item.maxQuantity}
                              onChange={(evnt) => handleChange(index, evnt)}
                            />
                          </td>
                          <td className="">{item.unit}</td>

                          <td className="text-center">
                            <a href="#" onClick={() => deleteItem(index)}>
                              <i className="bi bi-trash text-danger fs-5"></i>
                            </a>
                          </td>

                        </tr>
                      ))
                    ) : (
                      <tr>
                        <td colSpan={8}>
                          <div className="text-gray-600 d-flex text-center w-100 align-content-center justify-content-center">
                            No matching records found
                          </div>
                        </td>
                      </tr>
                    )}
                  </tbody>
                </Table>
              </div>
            </div>

            <div className=" seperator my-10"></div>
            <div className={`d-flex ${isAdd ? 'justify-content-between' : 'justify-content-end'}`}>
              {isAdd && <div >
                <DragAndDropFileUpload
                  appId=""
                  onUpload={(files: any) => setAttachmentIds(files)}
                />
                <ul className="fs-7 ">
                  <li>Max size : 3MB</li>
                  <li>Types supported : PNG,JPG, and PDF</li>
                </ul>
              </div>}
              <div className="col-md-3">
                <div className="bg-white border rounded p-9 m-2">
                  <h2>Total items value: {totalPrice}</h2>

                </div>
              </div>
            </div>

            <div className="row mb-6"></div>
            {/* Footer */}
            <div className="card-footer d-flex justify-content-center py-6 px-9">
              <button
                type="submit"
                className="btn btn-theme"
                disabled={loading}
              >
                <span className="indicator-label">Submit</span>
                {loading && (
                  <span className="indicator-progress">
                    Please wait...{" "}
                    <span className="spinner-border spinner-border-sm align-middle ms-2"></span>
                  </span>
                )}
              </button>
              <button
                type="button"
                onClick={() => onCancel()}
                className=" btn btn-light mx-3"
              >
                Cancel
              </button>
            </div>
          </form>
          {/* {showUploadBox && (
        <LoanApplicationUploadModal
          afterConfirm={afterConfirmUpload}
          setAttachmentIds={setAttachmentIds}
          isAdd={isAdd}
        />
      )} */}
        </div>
      ) : (
        <Error401 />
      )}
      <FarmerListModal
        show={showListCompanyModal}
        onSubmit={handleFarmerSelect}
        handleClose={handleCompanyClose}
        projectId={loanBatch.projectId}
      />
    </Fragment>
  );
};

export default AddLoanBatchApplication;
