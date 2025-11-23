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

const adminLevelService = new AdminLevelService();
const countryService = new CountryService();

interface VillageModel {
  id: string;
  villageName: string;
  villageCode: string;
  wardId: string;
}

const initialValues: VillageModel = {
  id: "",
  villageName: "",
  villageCode: "",
  wardId: "",
};

const profileDetailsSchema = Yup.object().shape({
  villageName: Yup.string()
    .required("Village name is required")
    .max(150, "Cannot exceed 150 characters"),
  villageCode: Yup.string().max(15, "Cannot exceed 15 characters"),
});

const Level4Add: FC<any> = (props: any) => {
  const { villageData, wardId, setWardId } = props;
  const navigate = useNavigate();
  const [data, setData] = useState<VillageModel>(
    villageData ? villageData : initialValues
  );
  const [loading, setLoading] = useState(false);

  const formik = useFormik<VillageModel>({
    enableReinitialize: true,
    initialValues: data,
    validationSchema: profileDetailsSchema,
    onSubmit: (values, { resetForm }) => {
      const toastId = toast.loading("Please wait...");
      setLoading(true);
      setTimeout(async () => {
        values.wardId = wardId;

        const updatedData = Object.assign(data, values);
        setData(updatedData);
        if (wardId && villageData) {
          values.id = villageData.id;
          const result = await adminLevelService.putAdminLevel4Data(
            values,
            values.id
          );
          if (result && result.id) {
            window.location.reload();
            navigate("/adminlevel/adminlevel4");
            toastNotify(toastId, "Success");
            setLoading(false);
          } else {
            toast.error("Something went wrong");
          }
        } else {
          const result = await adminLevelService.postAdminLevel4Data(values);
          if (result && result.id) {
            window.location.reload();
            navigate("/adminlevel/adminlevel4");
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
  const [ward, setWard] = useState<any>([]);

  const bindCountries = async () => {
    var response = await countryService.getCountryData(true);

    if (Array.isArray(response)) {
      var result = response.map((item) => ({
        id: item.id,
        name: item.countryName,
      }));
      result.unshift({ id: "0", name: "Select country" });
    } else {
      result = [{ id: "0", name: "Select country" }];
    }

    // Assuming setProjects is a function that sets state or performs some action
    setCountry(result);
  };

  const bindCounties = async (props: any) => {
    var response = await adminLevelService.getAdminLevel1Data(props);

    if (Array.isArray(response)) {
      var result = response.map((item) => ({
        id: item.id,
        name: item.countyName,
      }));
      result.unshift({ id: "0", name: "Select county" });
    } else {
      result = [{ id: "0", name: "Select county" }];
    }

    // Assuming setProjects is a function that sets state or performs some action
    setCounty(result);
  };

  const bindSubCounties = async (props: any) => {
    var response = await adminLevelService.getAdminLevel2Data(props);

    if (Array.isArray(response)) {
      var result = response.map((item) => ({
        id: item.id,
        name: item.subCountyName,
      }));
      result.unshift({ id: "0", name: "Select sub county" });
    } else {
      result = [{ id: "0", name: "Select sub county" }];
    }

    // Assuming setProjects is a function that sets state or performs some action
    setSubCounty(result);
  };

  const bindWards = async (props: any) => {
    var response = await adminLevelService.getAdminLevel3Data(props);

    if (Array.isArray(response)) {
      var result = response.map((item) => ({
        id: item.id,
        name: item.wardName,
      }));
      result.unshift({ id: "0", name: "Select ward" });
    } else {
      result = [{ id: "0", name: "Select ward" }];
    }

    // Assuming setProjects is a function that sets state or performs some action
    setWard(result);
  };

  const [countryId, setCountryId] = useState<any>();
  const [countyId, setCountyId] = useState<any>();
  const [subCountyId, setSubCountyId] = useState<any>();

  const handleCountrySelect = (props: any) => {
    setCountryId(props);
    bindCounties(countryId);
  };

  const handleCountySelect = (props: any) => {
    setCountyId(props);
    bindSubCounties(props);
  };

  const handleSubCountySelect = (props: any) => {
    setSubCountyId(props);
    bindWards(props);
  };

  const handleWardSelect = (props: any) => {
    setWardId(props);
  };

  useEffect(() => {
    bindCountries();
  }, []);

  useEffect(() => {
    if (villageData) {
      formik.setFieldValue("wardId", wardId);
      formik.setFieldValue("villageName", villageData.villageName);
      formik.setFieldValue("villageCode", villageData.villageCode);
    }
  }, [villageData]);

  return (
    <>
      <KTCard className="mt-10 shadow">
        <KTCardBody>
          <div className="row mb-3">
            <div className=" col-md-6">
              <label className="fs-6 fw-bold mb-3">Country </label>
              <select
                data-placeholder="Select country"
                name="countryId"
                className="form-select  "
                onChange={(e) => handleCountrySelect(e.target.value)}
              >
                {country.map((item: any) => (
                  <option key={item.id} value={item.id}>
                    {item.name}
                  </option>
                ))}
              </select>
            </div>

            <div className="col-md-6">
              <label className="fs-6 fw-bold mb-3">County </label>
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
            <div className="col-md-6">
              <label className="fs-6 fw-bold mb-3">Sub county </label>
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

            <div className="col-md-6">
              <label className="fs-6 fw-bold mb-3">Ward </label>
              <select
                data-placeholder="Select county"
                name="countyId"
                className="form-select mb-3 mb-lg-0"
                onChange={(e) => handleWardSelect(e.target.value)}
              >
                {ward &&
                  ward.map((item: any) => (
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
                label="Village name"
                name="villageName"
                placeholder="Ward name"
                formik={formik}
                isRequired
              />

              <ValidationField
                className="form-group col-12 col-sm-6 my-2"
                label="Village code"
                name="villageCode"
                placeholder="Village code"
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
export default Level4Add;
