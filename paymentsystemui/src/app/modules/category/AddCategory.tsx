import { FC, useEffect, useState } from "react";
import { PageTitle, PageLink } from "../../../_metronic/layout/core";
import { toast } from "react-toastify";
import * as Yup from "yup";
import { useFormik } from "formik";
import {
  RequiredMarker,
  ValidationField,
  ValidationTextArea,
} from "../../../_shared/components";
import { useNavigate } from "react-router-dom";
import { Content } from "../../../_metronic/layout/components/content";
import { KTCard, KTCardBody } from "../../../_metronic/helpers";
import { PageTitleWrapper } from "../../../_metronic/layout/components/toolbar/page-title";
import { Link, useParams } from "react-router-dom";
import CategoryService from "../../../services/CategoryService";
import { toastNotify } from "../../../services/NotifyService";
import { useDispatch, useSelector } from "react-redux";
import { categoryInitValues, CategoryModel } from "../../../_models/category-model";
import { resetCategoriesState } from "../../../_features/categories/categorySlice";
import { isAllowed } from "../../../_metronic/helpers/ApiUtil";
import { Error401 } from "../errors/components/Error401";

const profileBreadCrumbs: Array<PageLink> = [
  {
    title: "Loan Item Categories",
    path: "/categories",
    isSeparator: false,
    isActive: true,
  },
];
const profileDetailsSchema = Yup.object().shape({
  name: Yup.string()
    .required("Category name is required")
    .max(150, "Cannot exceed 150 characters"),
  description: Yup.string().max(1000, "Cannot exceed 1000 characters"),
});
const itemCategoryService = new CategoryService();

const AddItemCategory: FC = () => {
  const [loading, setLoading] = useState(false);
  const { id } = useParams();
  const [title] = useState<any>(id == null ? "Add" : "Edit");
  const dispatch = useDispatch();
  var categories = useSelector((state: any) => state?.categories);
  const navigate = useNavigate();

  const [data, setData] = useState<CategoryModel>(id == null ? categoryInitValues : categories);

  const formik = useFormik<CategoryModel>({
    enableReinitialize: true,
    initialValues: data,
    validationSchema: profileDetailsSchema,
    onSubmit: (values, { resetForm }) => {
      const toastId = toast.loading("Please wait...");
      setLoading(true);
      setTimeout(async () => {
        setData(values);

        if (title === "Add") {
          const result = await itemCategoryService.postItemCategoryData(values);

          if (result && result.id) {
            navigate("/categories");
            toastNotify(id, "Success");
            setLoading(false);
          } else {
            toast.error("Something went wrong");
          }
          toast.dismiss(toastId);
        } else {
          const result = await itemCategoryService.putItemCategoryData(
            values,
            id
          );

          if (result && result.id) {
            navigate("/categories");
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

  useEffect(() => {
    if (title === "Add") {
      dispatch(resetCategoriesState());
    }
  }, []);

  return (
    <>
      <Content>
        {isAllowed("settings.loans.categories.add") ? (
          <>
            <PageTitleWrapper />
            <PageTitle breadcrumbs={profileBreadCrumbs}>
              {title} Item Category
            </PageTitle>

            <KTCard className="mt-10 shadow">
              <KTCardBody>
                <RequiredMarker />
                <form onSubmit={formik.handleSubmit} noValidate>
                  <div className="d-flex flex-column">
                    <ValidationField
                      className="form-group col-md-6 my-2"
                      label="Category name"
                      name="name"
                      placeholder="Category name"
                      formik={formik}
                      isRequired
                    ></ValidationField>
                    {/* <ValidationSelect
                  className="form-group col-md-6 my-2"
                  label="Category"
                  isRequired
                  placeholder="Select"
                  name="category"
                  text="Select Category"
                  formik={formik}
                  options={categories}
                /> */}
                    <ValidationTextArea
                      className="form-group col-md-6 my-2"
                      label="Description"
                      name="description"
                      placeholder="Enter description"
                      rows={6}
                      formik={formik}
                    />
                  </div>
                  <div className="pt-10 d-flex justify-content-center">
                    <button
                      type="submit"
                      className="btn btn-theme"
                      disabled={loading}
                    >
                      Save{" "}
                    </button>
                    <Link to={"/categories"} className=" btn btn-light mx-3">
                      Cancel
                    </Link>
                  </div>
                </form>
              </KTCardBody>
            </KTCard>
          </>
        ) : (
          <Error401 />
        )}
      </Content>
    </>
  );
};
export { AddItemCategory };
