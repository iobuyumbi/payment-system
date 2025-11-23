import { FC, useEffect, useState } from "react";
import { toast } from "react-toastify";
import * as Yup from "yup";
import { useFormik } from "formik";
import { ValidationField } from "../../../../_shared/components";
import { toastNotify } from "../../../../services/NotifyService";
import { KTCard, KTCardBody } from "../../../../_metronic/helpers";
import AdminLevelService from "../../../../services/AdminLevelService";
import CountryService from "../../../../services/CountryService";
import { useNavigate } from "react-router-dom";
import { getSelectedCountryCode } from "../../../../_metronic/helpers/AppUtil";

const adminLevelService = new AdminLevelService();
const countryService = new CountryService();
interface CountyModel {
  id: string;
  countyName: string;
  countyCode: string;
  countryId: string;
}
const initialValues: CountyModel = {
  id: "",
  countyName: "",
  countyCode: "",
  countryId: "",
};

const profileDetailsSchema = Yup.object().shape({
  countyName: Yup.string()
    .required("County name is required")
    .max(150, "Cannot exceed 150 characters"),
  countyCode: Yup.string().max(15, "Cannot exceed 15 characters"),
});

const Level1Add: FC<any> = (props: any) => {
  const { countyData, setCountryId, countryId } = props;
  const [data, setData] = useState<CountyModel>(
    countyData ? countyData : initialValues
  );

  const navigate = useNavigate();
  const [country, setCountry] = useState<any>([]);
  const [loading, setLoading] = useState(false);
  const [selectedCountryId, setSelectedCountryId] = useState<string | number>('');

  const formik = useFormik<CountyModel>({
    enableReinitialize: true,
    initialValues: data,
    validationSchema: profileDetailsSchema,
    onSubmit: (values, { resetForm }) => {
      const toastId = toast.loading("Please wait...");
      setLoading(true);
      setTimeout(async () => {
        values.countryId = countryId;
        const updatedData = Object.assign(data, values);
        setData(updatedData);

        if (countryId && countyData) {
          values.id = countyData.id;
          const result = await adminLevelService.putAdminLevel1Data(
            values,
            values.id
          );
          if (result && result.id) {
            navigate("/adminlevel/adminlevel1");
            toastNotify(toastId, "Success");
            setLoading(false);
          } else {
            toast.error("Something went wrong");
          }
        } else {
          const result = await adminLevelService.postAdminLevel1Data(values);
          if (result && result.id) {
            navigate("/adminlevel/adminlevel1");
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

  const bindCountries = async () => {
    const data = {
      pageNumber: 1,
      pageSize: 10,
    };
    var response = await countryService.getCountryData(true);

    if (Array.isArray(response)) {
      var result = response.map((item) => ({
        id: item.id,
        name: item.countryName,
        code: item.code
      }));

      result.unshift({ id: "0", name: "Select country", code: "" });
    } else {
      result = [{ id: "0", name: "Select country", code: "" }];
    }

    // Assuming setProjects is a function that sets state or performs some action
    setCountry(result);
  };

  const handleCountrySelect = (props: any) => {
    setSelectedCountryId(props);
    setCountryId(props);
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
    if (countyData) {
      formik.setFieldValue("countryId", countryId);
      formik.setFieldValue("countyName", countyData.countyName);
      formik.setFieldValue("countyCode", countyData.countyCode);
    }
  }, [countyData]);

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
            <div className=" form-group col-12 col-sm-5 my-2">
              <label className="fs-6 fw-bold mb-3">Country </label>
              <select
                data-placeholder="Select country"
                name="countryId"
                className="form-select mb-3 mb-lg-0"
                onChange={(e) => handleCountrySelect(e.target.value)}
                value={selectedCountryId}
                disabled={true}
              >
                {country &&
                  country.map((item: any) => (
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
                className="form-group col-12 col-sm-5 my-2"
                label={labels.adminLevel1 + " Name"}
                name="countyName"
                placeholder={labels.adminLevel1 + " Name"}
                formik={formik}
                isRequired
              />

              <ValidationField
                className="form-group col-12 col-sm-5 my-2"
                label={labels.adminLevel1 + " Code"}
                name="countyCode"
                placeholder={labels.adminLevel1 + " Code"}
                formik={formik}
                isRequired
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
export default Level1Add;
