import { FC, useEffect, useState } from "react";
import { PageTitle, PageLink } from "../../../_metronic/layout/core";
import { toast } from "react-toastify";
import * as Yup from "yup";
import { useFormik } from "formik";
import {
  RequiredMarker,
  ValidationField,
  ValidationSelect,
  ValidationTextArea,
} from "../../../_shared/components";

import { Content } from "../../../_metronic/layout/components/content";
import { KTCard, KTCardBody } from "../../../_metronic/helpers";
import { PageTitleWrapper } from "../../../_metronic/layout/components/toolbar/page-title";
import { Link, useNavigate, useParams } from "react-router-dom";
import LoanItemService from "../../../services/LoanItemService";
import CategoryService from "../../../services/CategoryService";
import { useDispatch, useSelector } from "react-redux";
import { loanItemInitValues, LoanItemModel } from "../../../_models/loanitem-model";
import { resetLoanItemsState } from "../../../_features/loanitem/loanItemSlice";
import { toastNotify } from "../../../services/NotifyService";
import { isAllowed } from "../../../_metronic/helpers/ApiUtil";
import { Error401 } from "../errors/components/Error401";

const categoryService = new CategoryService();
const profileBreadCrumbs: Array<PageLink> = [
  {
    title: "Dashbaoard",
    path: "/dashboard",
    isSeparator: false,
    isActive: true,
  },
  {
    title: "",
    path: "",
    isSeparator: true,
    isActive: false,
  },
  {
    title: "Loan Items",
    path: "/loans/items",
    isSeparator: false,
    isActive: true,
  },
];

const profileDetailsSchema = Yup.object().shape({
  itemName: Yup.string()
    .required("Item name is required")
    .max(150, "Cannot exceed 150 characters"),

  description: Yup.string().max(1000, "Cannot exceed 1000 characters"),
  categoryId: Yup.string().required("Category is required"),
  cost: Yup.number().nullable()
});

const loanItemService = new LoanItemService();
const AddLoanItems: FC = () => {
  const { id } = useParams();
  const [title] = useState<any>(id == null ? "Add" : "Edit");
  const dispatch = useDispatch();
  var loanItems = useSelector((state: any) => state?.loanItems);
  const navigate = useNavigate();

  const [data, setData] = useState<LoanItemModel>(id == null ? loanItemInitValues : loanItems);
  const [loading, setLoading] = useState(false);

  const formik = useFormik<LoanItemModel>({
    enableReinitialize: true,
    initialValues: data,
    validationSchema: profileDetailsSchema,
    onSubmit: (values, { resetForm }) => {

      const toastId = toast.loading('Please wait...')
      setLoading(true);
      setTimeout(async () => {

        if (title === "Add") {
          const result = await loanItemService
            .postMasterLoanItemData(values);
          if (result && result.id) {
            navigate("/loans/items");
            toastNotify(id, "Success");
            setLoading(false);
          } else {
            toast.error("Something went wrong");
          }
          toast.dismiss(toastId);
        } else {
          const result = await loanItemService
            .putMasterLoanItemData(values, id);

          if (result && result.id) {
            navigate("/loans/items");
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

  const [category, setCategory] = useState<any>([]);

  const bindCategory = async () => {
    const data: any = {
      pageNumber: 1,
      pageSize: 10,
    }
    const response = await categoryService.getItemCategoryData(data);
    setCategory(response);
  }

  useEffect(() => {
    if (title === "Add") {
      dispatch(resetLoanItemsState());
    }
    bindCategory();
  }, []);

  return (
    <>
      <Content>
        {isAllowed("settings.loans.items.add") ? <> <PageTitleWrapper />
          <PageTitle breadcrumbs={profileBreadCrumbs}>{title} Loan Item</PageTitle>

          <KTCard className="mt-10 shadow">
            <KTCardBody>
              <RequiredMarker />
              <form onSubmit={formik.handleSubmit} noValidate>
                <div className="d-flex flex-column">
                  <ValidationField
                    className="form-group col-md-6 my-2"
                    label="Item Name"
                    name="itemName"
                    placeholder="Item Name"
                    formik={formik}
                    isRequired
                  ></ValidationField>

                  <ValidationSelect
                    className="form-group col-md-6 my-2"
                    label="Category"
                    isRequired
                    placeholder="Select"
                    name="categoryId"
                    text="Select Category"
                    formik={formik}
                    options={category}
                  />
                  <ValidationTextArea
                    className="form-group col-md-6 my-2"
                    label="Description"
                    name="description"
                    placeholder="Enter description"
                    rows={6}
                    formik={formik}
                  />
                  {/* <ValidationField
                  className="form-group col-md-6 my-2"
                  label="Item cost"
                  name="cost"
                  placeholder="Item cost"
                  formik={formik}

                ></ValidationField> */}
                </div>
                <div className="pt-10 d-flex justify-content-center">
                  <button
                    type="submit"
                    className="btn btn-theme"
                    disabled={loading}
                  >
                    Save{" "}
                  </button>
                  <Link to={"/loans/items"} className=" btn btn-light mx-3">
                    Cancel
                  </Link>
                </div>
              </form>
            </KTCardBody>
          </KTCard></> : <Error401 />}
      </Content>
    </>
  );
};
export { AddLoanItems };
