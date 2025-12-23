import { FC, useEffect, useState } from "react";
import { PageTitle, PageLink } from "../../../_metronic/layout/core";
import { toast } from "react-toastify";
import { toastNotify } from "../../../services/NotifyService";
import * as Yup from "yup";
import { Field, useFormik } from "formik";
import {
  RequiredMarker,
  ValidationField,
  ValidationSelect,
} from "../../../_shared/components";
import { Content } from "../../../_metronic/layout/components/content";
import { KTCard, KTCardBody } from "../../../_metronic/helpers";
import { PageTitleWrapper } from "../../../_metronic/layout/components/toolbar/page-title";
import { Link, useNavigate, useParams } from "react-router-dom";
import UserService from "../../../services/UserService";
import CountryService from "../../../services/CountryService";
import ProjectService from "../../../services/ProjectService";
import RoleService from '../../../services/RoleService';
import { useDispatch, useSelector } from "react-redux";
import { resetUserState } from "../../../_features/users/userSlice";
import CustomComboBox from "../../../_shared/CustomComboBox/Index";
import { OptionType } from "../../../_models/option-type";
import FormErrorAlert from "../../../_shared/FormErrorAlert/Index";

const countryService = new CountryService();
const projectService = new ProjectService();
const userService = new UserService();
const roleService = new RoleService();

const profileBreadCrumbs: Array<PageLink> = [
  {
    title: "Dashboard",
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
    title: "Users",
    path: "/account-settings/users",
    isSeparator: false,
    isActive: true,
  },
  {
    title: "",
    path: "",
    isSeparator: true,
    isActive: false,
  }
];

const profileDetailsSchema = Yup.object().shape({
  email: Yup.string()
    .required("Email is required")
    .max(150, "Cannot exceed 150 characters"),
  phoneNumber: Yup.string().max(20, "Cannot exceed 20 characters").nullable(),
  roleId: Yup.array()
    .of(
      Yup.object({
        value: Yup.string().required(),
        label: Yup.string().required(),
      })
    )
    .min(1, 'User group is required')
    .required('User group is required'),
  country: Yup.array()
    .of(
      Yup.object({
        value: Yup.string().required(),
        label: Yup.string().required(),
      })
    )
    .min(1, 'Country is required')
    .required('Country is required'),

  projectId: Yup.string().required('Project is required'),
  projectManagerId: Yup.string().optional().nullable(),
  // isLoginEnabled: Yup.boolean()
  //   .required("Login access flag is required"),
});

const AddUser: FC = () => {
  const { id } = useParams();
  const navigate = useNavigate();
  const dispatch = useDispatch();
  const user = useSelector((state: any) => state?.users);
  const [data, setData] = useState<any>(user);
  const [isLoginEnabled, setIsLoginEnabled] = useState<any>(user.username ? user.isLoginEnabled : true);
  const [isActive, setIsActive] = useState<any>(user.username ? user.isActive : true);
  const [title] = useState<any>(id == null || '' ? "Add" : "Edit");
  const [loading, setLoading] = useState(false);
  const [countries, setCountries] = useState<OptionType[]>([]);
  const [projects, setProjects] = useState<any>([]);
  const [users, setUsers] = useState<any>([]);
  const [roles, setRoles] = useState<any>([]);
  const [errors, setErrors] = useState<any>([]);

  const formik = useFormik<any>({
    enableReinitialize: true,
    initialValues: data,
    validationSchema: profileDetailsSchema,
    onSubmit: (values) => {
      setErrors(null);

      const toastId = toast.loading('Please wait...')
      setLoading(true);
      setTimeout(async () => {
        !id ? values.username = values.email : '';
        // find role name
        const transformedRoleNames = values.roleId.map((p: any) => p.label);

        values.isLoginEnabled = isLoginEnabled;
        values.isActive = isActive;
        values.roleNames = transformedRoleNames
        values.projectManagerId = values.projectManagerId === "" ? null : values.projectManagerId;
        const transformedCountryIds = values.country.map((p: any) => p.value);
        values.countryIds = transformedCountryIds;

        if (title === "Add") {
          const result = await userService.postUserData(values);
          console.log(result)
          if (result && result.id) {
            navigate("/account-settings/users");
            toastNotify(toastId, "Success");
            setLoading(false);
          } else {
            setErrors(result.errors);
            //toast.error("Something went wrong");
          }
          toast.dismiss(toastId);
        } else {
          const result = await userService.putUserData(values, data.id);
          console.log(result)
          if (result && result.id) {
            navigate("/account-settings/users");
            toastNotify(toastId, "Success");
            setLoading(false);
          } else {
            setErrors(result.errors);
            //toast.error("Something went wrong");
          }
          toast.dismiss(toastId);
        }

      }, 1000);
    },
  });

  const getCountries = async () => {
    try {
      const isActive = true;
      countryService.getCountryData(isActive).then((countries: any) => {

        const options: any[] = countries.map((opt: any) => ({
          value: opt.id,
          label: opt.countryName,
        }));
        setCountries(options)
      })

    } catch (error) {
      console.log("Error in fetching countries", error);
    }
  };

  const getUsers = async () => {
    var users = await userService.getUserData();
    var result = [...users].map(e => ({ id: e.id, name: e.email }));

    setUsers(result)
  };

  const getProjects = async (countryIdList: any) => {
    const data: any = {
      pageNumber: 1,
      pageSize: 10,
      countryIdList: countryIdList
    };
    projectService.getProjectData(data).then((projects: any) => {
      console.log(projects)
      var result = [...projects].map(e => ({ id: e.id, name: e.projectName }));

      setProjects(result);
    });
  };

  const bindRoles = async (countryIdList: string[]) => {
    const data: any = {
      pageNumber: 1,
      pageSize: 100,
    };

    const roles: any = await roleService.getRoles(data);

    const filteredRoles = roles.filter((role: any) =>
      countryIdList.includes(role.countryId)
    );

    const options = filteredRoles.map((opt: any) => ({
      value: opt.id,
      label: opt.name,
    }));

    setRoles(options);
  };

  const getUserCountriesAndRoles = async () => {
    var response = await userService.getUserCountries(user.id);
    var roles = await userService.getUserRoles(user.id);
    if (response) {
      // Set the form data explicitly
      formik.setValues({
        ...user,
        country: response,
        roleId: roles,
      });
    }
  }

  useEffect(() => {
    if (id == undefined || id == null) {
      dispatch(resetUserState());
      formik.resetForm();
    } else {
      getUserCountriesAndRoles();
    }
  }, []);

  useEffect(() => {
    if (id == undefined || id == null) {
      dispatch(resetUserState());
      formik.resetForm();
    }

    getCountries();
    getUsers();
  }, []);

  useEffect(() => {
    if (formik.values.country && formik.values.country.length > 0) {
      const countryIdList: any[] = [];
      const countryIds = formik.values.country;

      countryIds.map((opt: any) => countryIdList.push(opt.value));

      getProjects(countryIdList);
      bindRoles(countryIdList)
    }
    else {
      formik.setFieldValue("projectId", null);
    }
  }, [formik.values.country]);

  return (
    <>
      <Content>
        <PageTitleWrapper />
        <PageTitle breadcrumbs={profileBreadCrumbs}>
          {title} User
        </PageTitle>

        <KTCard className="mt-10">
          <KTCardBody>
            <RequiredMarker />
            <form onSubmit={formik.handleSubmit} noValidate>
              <div className="container">

                {/* errors */}
                <FormErrorAlert errors={errors} />

                <div className="row my-4">
                  <div className="col-md-5">
                    <CustomComboBox
                      label="Country(s)"
                      name="country"
                      formik={formik}
                      options={countries ? countries : []}
                      isMulti={true}
                    />
                  </div>
                  <div className="col-md-5">
                    <CustomComboBox
                      label="User group"
                      name="roleId"
                      formik={formik}
                      options={roles ? roles : []}
                      isMulti={true}
                      isRequired
                    />
                  </div>
                </div>

                <div className="row my-4">
                  <ValidationField
                    className="form-group col-md-5"
                    label="Email"
                    name="email"
                    placeholder="Email"
                    formik={formik}
                    isRequired
                  />
                  <ValidationField
                    className="form-group col-md-5 "
                    label="Mobile"
                    name="phoneNumber"
                    placeholder="Mobile"
                    formik={formik}
                  />
                </div>

                <div className="row my-4">
                  {/* <CustomDropDown
                    className="form-group col-md-5"
                    name="projectId"
                    fieldname="projectId"
                    label="Project"
                    isRequired
                    formik={formik}
                    data={projects ? projects : []}
                  /> */}
                  <ValidationSelect
                    className="form-group col-md-5"
                    label="Project"
                    placeholder="Selec projectt"
                    name="projectId"
                    text="Select project"
                    formik={formik}
                    options={projects ? projects : []}
                  />

                  <ValidationSelect
                    className="form-group col-md-5"
                    label="Project Manager"
                    placeholder="Select"
                    name="projectManagerId"
                    text="Select Project Manager"
                    formik={formik}
                    options={users ? users : ""}
                  />
                </div>
                <div className="py-3">
                  <label className="fw-bold fs-6 mb-2 me-3">
                    Enable portal access to the user
                  </label>
                  <input type="checkbox"
                    value={isLoginEnabled}
                    checked={isLoginEnabled}
                    onChange={() => setIsLoginEnabled(!isLoginEnabled)} />
                </div>

                <div className="py-3">
                  <label className="fw-bold fs-6 mb-2 me-3">
                    Is user Active?
                  </label>
                  <input type="checkbox"
                    value={isActive}
                    checked={isActive}
                    onChange={() => setIsActive(!isActive)} />
                </div>


                <div className="py-6 d-flex">
                  <button
                    type="submit"
                    className="btn btn-theme"
                    disabled={loading}
                  >
                    Save{" "}
                  </button>
                  <Link to={"/account-settings/users"} className=" btn btn-light mx-3">
                    Cancel
                  </Link>
                </div>
              </div>
            </form>
          </KTCardBody>
        </KTCard>
      </Content>
    </>
  );
};
export { AddUser };
