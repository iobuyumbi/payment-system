import { FC, useEffect, useState } from "react";
import { toast } from "react-toastify";
import * as Yup from "yup";
import { useFormik } from "formik";
import {
  ValidationField,
} from "../../../../_shared/components";
import { toastNotify } from "../../../../services/NotifyService";
import { KTCard, KTCardBody } from "../../../../_metronic/helpers";
import AdminLevelService from "../../../../services/AdminLevelService";
import CountryService from "../../../../services/CountryService";
import { useNavigate } from "react-router-dom";
import { getSelectedCountryCode } from "../../../../_metronic/helpers/AppUtil";

const adminLevelService = new AdminLevelService();
const countryService = new CountryService();

interface WardModel {
  id: string;
  wardName: string;
  wardCode: string;
  subCountyId: string;
}

const initialValues: WardModel = {
  id: "",
  wardName: "",
  wardCode: "",
  subCountyId: "",
};

const profileDetailsSchema = Yup.object().shape({
  wardName: Yup.string()
    .required("Ward name is required")
    .max(150, "Cannot exceed 150 characters"),
  wardCode: Yup.string().max(15, "Cannot exceed 15 characters"),
});

const Level3Add: FC<any> = (props: any) => {
  const { wardData, subCountyId, setSubCountyId } = props;
  const navigate = useNavigate();
  const [data, setData] = useState<WardModel>(
    wardData ? wardData : initialValues
  );
  const [loading, setLoading] = useState(false);

  const formik = useFormik<WardModel>({
    enableReinitialize: true,
    initialValues: data,
    validationSchema: profileDetailsSchema,
    onSubmit: (values, { resetForm }) => {
      const toastId = toast.loading("Please wait...");
      setLoading(true);
      setTimeout(async () => {
        values.subCountyId = subCountyId;

        const updatedData = Object.assign(data, values);
        setData(updatedData);
        if (subCountyId && wardData) {
          values.id = wardData.id;
          const result = await adminLevelService.putAdminLevel3Data(
            values,
            values.id
          );

          if (result && result.id) {
            window.location.reload();
            navigate("/adminlevel/adminlevel3");
            toastNotify(toastId, "Success");
            setLoading(false);
          } else {
            toast.error("Something went wrong");
          }
        } else {
          const result = await adminLevelService.postAdminLevel3Data(values);

          if (result && result.id) {
            window.location.reload();
            navigate("/adminlevel/adminlevel3");
            toastNotify(toastId, "Success");
            setLoading(false);
          } else {
            toast.error("Something went wrong");
          }
        }
        toast.dismiss(toastId);
        window.location.reload();
      }, 1000);
    },
  });

  const [country, setCountry] = useState<any>([]);
  const [county, setCounty] = useState<any>([]);
  const [subCounty, setSubCounty] = useState<any>([]);

  const bindCountries = async () => {
    var response = await countryService.getCountryData(true);

    if (Array.isArray(response)) {
      var result = response.map((item) => ({
        id: item.id,
        name: item.countryName,
        code: item.code,
      }));
      result.unshift({ id: "0", name: "Select country", code: "" });
    } else {
      result = [{ id: "0", name: "Select country", code: "" }];
    }

    // Assuming setProjects is a function that sets state or performs some action
    setCountry(result);
  };

  const bindCounties = async (props: any) => {
    const pageData = {
      pageNumber: 1,
      pageSize: 10000,
      filter: "",
      countryId: props
    };
    var response = await adminLevelService.getAdminLevel1Data(pageData);

    if (Array.isArray(response)) {
      var result = response.map((item) => ({
        id: item.id,
        name: item.countyName,
      }));
      result.unshift({ id: "0", name: "Select " });
    } else {
      result = [{ id: "0", name: "Select " }];
    }

    // Assuming setProjects is a function that sets state or performs some action
    setCounty(result);
  };

  const bindSubCounties = async (props: any) => {
    const pageData = {
      pageNumber: 1,
      pageSize: 10000,
      filter: "",
      countyId: props
    };
    var response = await adminLevelService.getAdminLevel2Data(pageData);

    if (Array.isArray(response)) {
      var result = response.map((item) => ({
        id: item.id,
        name: item.subCountyName,
      }));
      result.unshift({ id: "0", name: "Select " });
    } else {
      result = [{ id: "0", name: "Select " }];
    }

    // Assuming setProjects is a function that sets state or performs some action
    setSubCounty(result);
  };

  const [countryId, setCountryId] = useState<any>();
  const [countyId, setCountyId] = useState<any>();

  const handleCountrySelect = (props: any) => {
    setCountryId(props);
    bindCounties(props);
  };

  const handleCountySelect = (props: any) => {
    setCountyId(props);
    bindSubCounties(props);
  };

  const handleSubCountySelect = (props: any) => {
    setSubCountyId(props);
  };

  const [labels, setLabels] = useState({
    adminLevel1: "",
    adminLevel2: "",
    adminLevel3: "",
    adminLevel4: "Village",
  });
  useEffect(() => {
    const adminLevel1 =
      countryId === "a82beb82-2d92-11ef-ad6b-46477cdd49d1"
        ? "County"
        : "Region";
    const adminLevel2 =
      countryId === "a82beb82-2d92-11ef-ad6b-46477cdd49d1"
        ? "Sub-county"
        : countryId === "a82c10f6-2d92-11ef-ad6b-46477cdd49d1"
          ? "District"
          : "District";
    const adminLevel3 =
      countryId === "a82beb82-2d92-11ef-ad6b-46477cdd49d1"
        ? "Ward"
        : countryId === "a82c10f6-2d92-11ef-ad6b-46477cdd49d1"
          ? "County/Sub-County"
          : "Ward";


    setLabels({
      adminLevel1,
      adminLevel2,
      adminLevel3,
      adminLevel4: "Village",
    });
  }, [countryId]);
  useEffect(() => {
    bindCountries();
  }, []);

  useEffect(() => {
    if (subCountyId && wardData) {

      formik.setFieldValue("subCountyId", subCountyId);
      formik.setFieldValue("wardName", wardData?.wardName);
      formik.setFieldValue("wardCode", wardData?.wardCode);
    }
  }, [wardData]);

  useEffect(() => {
    if (country && country.length > 0) {
      const countryCode = getSelectedCountryCode();
      const selectedCountry = country.find((c: any) => c.code === countryCode);

      if (selectedCountry) {
        handleCountrySelect(selectedCountry.id);
      }
    }
  }, [country]);

  return (
    <>

      <KTCard className="mt-10 shadow">
        <KTCardBody>
          <div className="row mb-3">
            <div className=" col-md-6 mb-3">
              <label className="fs-6 fw-bold mb-3">Country </label>
              <select
                data-placeholder="Select country"
                name="countryId"
                className="form-select  "
                onChange={(e) => handleCountrySelect(e.target.value)}
                value={countryId}
                disabled={true}
              >
                {country.map((item: any) => (
                  <option key={item.id} value={item.id}>
                    {item.name}
                  </option>
                ))}
              </select>
            </div>

            <div className="col-md-6 mb-3">
              <label className="fs-6 fw-bold mb-3">{labels?.adminLevel1} </label>
              <select
                data-placeholder="Select county"
                name="countyId"
                className="form-select mb-3 mb-lg-0"
                onChange={(e) => handleCountySelect(e.target.value)}
              >
                {county.map((item: any) => (
                  <option key={item.id} value={item.id}>
                    {item.name}
                  </option>
                ))}
              </select>
            </div>
          </div>
          <div className="row mb-3">

            <div className="col-md-6 mb-3">
              <label className="fs-6 fw-bold mb-3">{labels?.adminLevel2}</label>
              <select
                data-placeholder="Select sub county"
                name="subCountyId"
                className="form-select mb-3 mb-lg-0"
                onChange={(e) => handleSubCountySelect(e.target.value)}
              >
                {subCounty.map((item: any) => (
                  <option key={item.id} value={item.id}>
                    {item.name}
                  </option>
                ))}
              </select>
            </div>
          </div>
          <form onSubmit={formik.handleSubmit} noValidate>
            <div className="row mb-3">
              <ValidationField
                className="form-group col-12 col-sm-6 my-2"
                label={labels.adminLevel3 + " Name"}
                name="wardName"
                placeholder={labels.adminLevel3 + " Name"}
                formik={formik}
                isRequired
              />

              <ValidationField
                className="form-group col-12 col-sm-6 my-2"
                label={labels.adminLevel3 + " Code"}
                name="wardCode"
                placeholder={labels.adminLevel3 + " Code"}
                isRequired
                formik={formik}
              />
            </div>
            <div className="row mb-3">
              <div className="col-md-3">
                <button
                  type="submit"
                  className="btn btn-theme mt-7"
                  disabled={loading}
                >
                  Save{" "}
                </button>
              </div>
            </div>
          </form>
        </KTCardBody>
      </KTCard>

    </>
  );
};
export default Level3Add;
