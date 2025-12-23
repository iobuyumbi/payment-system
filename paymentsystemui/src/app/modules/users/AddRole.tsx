import { FC, useEffect, useState } from "react";
import { PageTitle, PageLink } from "../../../_metronic/layout/core";
import { toast } from "react-toastify";
import { toastNotify } from "../../../services/NotifyService";
import * as Yup from "yup";
import { useFormik } from "formik";
import { RequiredMarker, ValidationField } from "../../../_shared/components";
import { Content } from "../../../_metronic/layout/components/content";
import { KTCard, KTCardBody } from "../../../_metronic/helpers";
import { PageTitleWrapper } from "../../../_metronic/layout/components/toolbar/page-title";
import { Link, useNavigate, useParams } from "react-router-dom";
import RoleService from "../../../services/RoleService";
import CustomComboBox from "../../../_shared/CustomComboBox/Index";
import { OptionType } from "../../../_models/option-type";
import CountryService from "../../../services/CountryService";

const roleService = new RoleService();
const countryService = new CountryService();

const profileBreadCrumbs: Array<PageLink> = [
  {
    title: "Users",
    path: "/account-settings/users",
    isSeparator: false,
    isActive: true,
  },
];

const profileDetailsSchema = Yup.object().shape({
  name: Yup.string()
    .required(" User group name is required")
    .max(50, "Cannot exceed 50 characters"),
});

interface RoleModel {
  name: string;
}
const roleInitValues: RoleModel = { name: "" };

const AddRoles: FC = () => {
  const navigate = useNavigate();
  const { id } = useParams();
  const [data, setData] = useState<any>(roleInitValues);
  const [title] = useState<any>(id ? "Edit" : "Add");
  const [loading, setLoading] = useState(false);
  const [countries, setCountries] = useState<OptionType[]>([]);

  const getCountries = async () => {
    try {
      const isActive = true;
      countryService.getCountryData(isActive).then((countries: any) => {
        const options: any[] = countries.map((opt: any) => ({
          value: opt.id,
          label: opt.countryName,
        }));
        setCountries(options);
      });
    } catch (error) {
      console.log("Error in fetching countries", error);
    }
  };

  const formik = useFormik<RoleModel>({
    enableReinitialize: true,
    initialValues: data,
    validationSchema: profileDetailsSchema,
    onSubmit: (values: any) => {
      const toastId = toast.loading("Please wait...");
      setLoading(true);
      setTimeout(async () => {
        if (title === "Add") {
          const transformedCountryIds = values.country.map((p: any) => p.value);
          values.countryIds = transformedCountryIds;
          const result = await roleService.saveRole(values);
          if (result && result[0].succeeded === true) {
            navigate("/account-settings/roles");
            toastNotify(toastId, "Success");
            setLoading(false);
          } else {
            toast.error("Something went wrong");
          }
          toast.dismiss(toastId);
        } else {
          const result = await roleService.updateRole(id, values.name);
          if (result && result.succeeded === true) {
            navigate("/account-settings/roles");
            toastNotify(toastId, "Success");
            setLoading(false);
          } else {
            toast.error("Something went wrong");
          }
          toast.dismiss(toastId);
        }
      }, 1000);
    },
  });

  const bindDetails = async () => {
    const response = await roleService.getSingle(id);
    if (response) {
      setData(response);
    }
  };

  useEffect(() => {
    if (id) {
      bindDetails();
    }
    getCountries();
  }, [id]);

  return (
    <>
      <Content>
        <PageTitleWrapper />
        <PageTitle breadcrumbs={profileBreadCrumbs}>
          {title} User group
        </PageTitle>

        <KTCard className="mt-10">
          <KTCardBody>
            <RequiredMarker />
            <form onSubmit={formik.handleSubmit} noValidate>
              <div className="container">
                {!id && <div className="row my-4">
                  <div className="col-md-5">
                    <CustomComboBox
                      label="Country(s)"
                      name="country"
                      formik={formik}
                      options={countries ? countries : []}
                      isMulti={true}
                    />
                  </div>
                </div>}
                <div className="row my-4">
                  <ValidationField
                    className="form-group col-md-5 "
                    label="User group name"
                    name="name"
                    placeholder="Enter user group name"
                    formik={formik}
                    isRequired
                  />
                </div>

                <div className="py-6 d-flex justify-content-center">
                  <button
                    type="submit"
                    className="btn btn-theme"
                    disabled={loading}
                  >
                    Save{" "}
                  </button>
                  <Link
                    to={"/account-settings/roles"}
                    className=" btn btn-light mx-3"
                  >
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
export { AddRoles };
