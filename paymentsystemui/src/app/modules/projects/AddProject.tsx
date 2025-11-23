import { FC, useEffect, useState } from "react";
import * as Yup from "yup";
import { toast } from "react-toastify";
import { toastNotify } from "../../../services/NotifyService";
import { useFormik } from "formik";
import { Link, useNavigate, useParams } from "react-router-dom";
import { KTCard, KTCardBody } from "../../../_metronic/helpers";
import { Content } from "../../../_metronic/layout/components/content";
import { PageTitleWrapper } from "../../../_metronic/layout/components/toolbar/page-title";
import { PageTitle, PageLink } from "../../../_metronic/layout/core";
import {
  ValidationTextArea,
  ValidationField,
  RequiredMarker,
} from "../../../_shared/components";
import CustomDropDown from "../../../_shared/CustomDropDown/Index";
import CountryService from "../../../services/CountryService";
import ProjectService from "../../../services/ProjectService";
import { useDispatch, useSelector } from "react-redux";
import { ProjectModel } from "../../../_models/project-model";
import { resetProjectState } from "../../../_features/projects/projectSlice";
import { isAllowed } from "../../../_metronic/helpers/ApiUtil";
import { Error401 } from "../errors/components/Error401";
import { getSelectedCountryCode } from "../../../_metronic/helpers/AppUtil";
import FormErrorAlert from "../../../_shared/FormErrorAlert/Index";

const countryData = new CountryService();
const projectService = new ProjectService();

const breadCrumbs: Array<PageLink> = [
  {
    title: "Projects",
    path: "/projects",
    isSeparator: false,
    isActive: true,
  },
];

const AddProjectSchema = Yup.object().shape({
  projectName: Yup.string()
    .max(150, "Maximum 150 characters")
    .required("Project name is required"),
  description: Yup.string().nullable(),
  projectCode: Yup.string()
    .max(100, "Maximum 100 characters")
    .required("Project Code is required"),
  countryId: Yup.string().required("Country is required"),
});

const AddProject: FC = () => {
  const navigate = useNavigate();
  const dispatch = useDispatch();

  const project = useSelector((state: any) => state?.projects);
  const [data, setData] = useState<any>(project);
  const { id } = useParams();
  const [title] = useState<any>(id == null ? "Add" : "Edit");
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [countries, setCountries] = useState<any>();
  const [errors, setErrors] = useState<any>([]);

  const formik = useFormik<ProjectModel>({
    enableReinitialize: true,
    initialValues: data,
    validationSchema: AddProjectSchema,
    onSubmit: async (values) => {
      const toastId = toast.loading("Please wait...");
      setIsSubmitting(true);
      setErrors([]);
      
      setTimeout(async () => {
        if (title === "Add") {
          const result = await projectService.addProject(values);
          if (result && result.id) {
            navigate("/projects");
            toastNotify(toastId, "Success");
          } else {
            setErrors(result.errors || result.Errors);
            //toast.error("Something went wrong");
          }
          setIsSubmitting(false);
        } else {
          const result = await projectService.updateProject(values, id);
          if (result && result.id) {
            navigate("/projects");
            toastNotify(toastId, "Success");
          } else {
            setErrors(result.errors || result.Errors);
            //toast.error("Something went wrong");
          }
          setIsSubmitting(false);
        }
      }, 1000);

      toast.dismiss(toastId);
    },
  });

  const getCountries = async () => {
    try {
      const isActive = true;
      countryData.getSelectedCountryData(isActive).then((data: any) => {
        if (data && data.length > 0) {
          setCountries(data);
        }
      });
    } catch (error) {
      console.error("Error fetching countries:", error);
    }
  };

  useEffect(() => {
    getCountries();
    dispatch(resetProjectState());
  }, []);

  useEffect(() => {
    if (countries && countries.length > 0) {
      const countryCode = getSelectedCountryCode();
      const selectedCountry = countries.find((c: any) => c.code === countryCode);

      if (selectedCountry) {
        // Update the data with countryId
        setData((prev: any) => ({
          ...prev,
          countryId: selectedCountry.id,
        }));
      }
    }
  }, [countries]);

  return (
    <Content>
      {isAllowed("settings.projects.add") ? (
        <>
          <PageTitle breadcrumbs={breadCrumbs}>{title} Project</PageTitle>
          <PageTitleWrapper />
          <KTCard className="mt-10 shadow">
            <KTCardBody>
              <RequiredMarker />

               <FormErrorAlert errors={errors} />
               
              <form onSubmit={formik.handleSubmit} noValidate>
                <div className="d-flex flex-column justify-content-start">
                  <ValidationField
                    className="col-md-4 my-3"
                    label="Project Name"
                    name="projectName"
                    placeholder="Project Name"
                    formik={formik}
                    isRequired
                  ></ValidationField>

                  <CustomDropDown
                    className="col-md-4 my-3"
                    name="country"
                    fieldname="countryId"
                    label="Country"
                    isRequired
                    formik={formik}
                    data={countries ? countries : ""}
                    disabled={true}
                  />

                  <ValidationField
                    className="col-md-4 my-3"
                    label="Project Code"
                    name="projectCode"
                    placeholder="Project Code"
                    formik={formik}
                    isRequired
                  ></ValidationField>

                  <ValidationTextArea
                    className="col-md-8 my-3"
                    label="Description"
                    name="description"
                    placeholder="Enter description"
                    rows={6}
                    formik={formik}
                  />
                </div>

                <div className="text-center pt-15">
                  <button
                    type="submit"
                    className="btn btn-custom btn-theme"
                  >
                    <span className="indicator-label">Submit</span>
                    {isSubmitting && (
                      <span className="indicator-progress">
                        Please wait...{" "}
                        <span className="spinner-border spinner-border-sm align-middle ms-2"></span>
                      </span>
                    )}
                  </button>
                  <Link to={"/projects"} className=" btn btn-secondary mx-3">
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
  );
};

export default AddProject;
