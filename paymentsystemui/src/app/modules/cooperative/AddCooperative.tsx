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
import { toastNotify } from "../../../services/NotifyService";
import CountryService from "../../../services/CountryService";
import { Content } from "../../../_metronic/layout/components/content";
import { KTCard, KTCardBody } from "../../../_metronic/helpers";
import { PageTitleWrapper } from "../../../_metronic/layout/components/toolbar/page-title";
import { Link, useNavigate, useParams } from "react-router-dom";
import CooperativeService from "../../../services/CooperativeService";
import CustomDropDown from "../../../_shared/CustomDropDown/Index";
import {
  CooperativeModel,
} from "../../../_models/cooperative-model";
import { useDispatch, useSelector } from "react-redux";
import { resetCooperativeState } from "../../../_features/cooperatives/cooperativeSlice";
import { isAllowed } from "../../../_metronic/helpers/ApiUtil";
import { Error401 } from "../errors/components/Error401";
import { getSelectedCountryCode } from "../../../_metronic/helpers/AppUtil";

const countryData = new CountryService();
const cooperativeService = new CooperativeService();

const profileBreadCrumbs: Array<PageLink> = [
  {
    title: "Cooperatives",
    path: "/cooperatives",
    isSeparator: false,
    isActive: true,
  },
];

const profileDetailsSchema = Yup.object().shape({
  name: Yup.string()
    .required("Cooperative Name is required")
    .max(255, "Cannot exceed 255 characters"),
  countryId: Yup.string().required("Kindly select Country"),
  description: Yup.string().max(500, "Cannot exceed 500 characters"),
});

const AddCooperative: FC = () => {
  const navigate = useNavigate();
  const dispatch = useDispatch();
  const cooperative = useSelector((state: any) => state?.cooperatives);

  const [data, setData] = useState<any>(cooperative);
  const { id } = useParams();
  const [title] = useState<any>(id == null ? "Add" : "Edit");
  const [loading, setLoading] = useState(false);
  const [countries, setCountries] = useState<any>();

  const formik = useFormik<CooperativeModel>({
    enableReinitialize: true,
    initialValues: data,
    validationSchema: profileDetailsSchema,
    onSubmit: (values, { resetForm }) => {
      const iD = toast.loading("Please wait...");
      setLoading(true);
      setTimeout(async () => {

        if (title === "Add") {
          const result = await cooperativeService.postCooperativeData(values);

          if (result && result.id) {
            navigate("/cooperatives");
            toastNotify(iD, "Success");
            setLoading(false);
          } else {
            toast.error("Something went wrong");
          }
          toast.dismiss(iD);
        } else {
          const result = await cooperativeService.putCooperativeData(values, id);

          if (result && result.id) {
            navigate("/cooperatives");
            toastNotify(iD, "Success");
            setLoading(false);
          } else {
            toast.error("Something went wrong");
          }
          toast.dismiss(iD);
        }
      }, 1000);
    },
  });

  const getCountries = async () => {
    try {
      const isActive = true;
      countryData.getCountryData(isActive).then((data: any) => {
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
    dispatch(resetCooperativeState());
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
    <>
      <Content>
        {isAllowed('settings.cooperatives.add') ?
          <><PageTitleWrapper />
            <PageTitle breadcrumbs={profileBreadCrumbs}>
              {title} Cooperative
            </PageTitle>

            <KTCard className="mt-10 shadow">
              <KTCardBody>
                <RequiredMarker />
                <form onSubmit={formik.handleSubmit} noValidate>
                  <div className="d-flex flex-column">
                    <ValidationField
                      className="form-group col-md-5 my-3"
                      label="Cooperative Name"
                      name="name"
                      placeholder="Cooperative Name"
                      formik={formik}
                      isRequired
                    ></ValidationField>

                    <CustomDropDown
                      className="col-md-5 my-3"
                      fieldname="countryId"
                      name="country"
                      label="Country"
                      isRequired
                      formik={formik}
                      data={countries ? countries : ""}
                      disabled={true}
                    />
                    <ValidationTextArea
                      className="form-group col-md-8 my-3"
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
                    <Link to={"/cooperatives"} className=" btn btn-secondary mx-3">
                      Cancel
                    </Link>
                  </div>
                </form>
              </KTCardBody>
            </KTCard>
          </> : <Error401 />}
      </Content>
    </>
  );
};
export { AddCooperative };
