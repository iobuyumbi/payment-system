import React, { useEffect, useState } from "react";
import axios from "axios";
import {
  RequiredMarker,
  ValidationField,
} from "../../../../_shared/components";
import FormErrorAlert from "../../../../_shared/FormErrorAlert/Index";
import { useFormik } from "formik";
import * as Yup from "yup";
import { toast } from "react-toastify";
import { Link, useNavigate, useParams } from "react-router-dom";
import LocationService from "../../../../services/LocationService";
import DragAndDropFileUpload from "../../../../_shared/DragAndDropFileUpload/Index";
import { getAPIBaseUrl } from "../../../../_metronic/helpers/ApiUtil";
import { get } from "lodash";
// Extend or define your own Props interface to include isAdd
interface Props {
  isAdd: boolean;
}

const locationService = new LocationService();
const allowedTypes = ["image/jpeg", "image/png"]; // Allowed MIME types

interface LocationProfile {
  id?: string;
  attachmentIds?: string[];
  addressLine1: string;
  addressLine2?: string;
  city: string;
  state: string;
  zipCode: string;
  supportEmail: string;
  phoneNumber: string;
  alternateNumber: string;
  website: string;
  logoUrl?: string;
}

export const locationProfileSchema = Yup.object().shape({
  addressLine1: Yup.string()
    .required("Address Line 1 is required")
    .max(256, "Cannot exceed 256 characters"),

  addressLine2: Yup.string()
    .max(256, "Cannot exceed 256 characters")
    .notRequired(),

  city: Yup.string()
    .required("City is required")
    .max(100, "Cannot exceed 100 characters"),

  state: Yup.string()
    .required("State is required")
    .max(100, "Cannot exceed 100 characters"),

  zipCode: Yup.string()
    .required("Postal Code is required")
    .max(20, "Cannot exceed 20 characters"),

  supportEmail: Yup.string()
    .email("Invalid email address")
    .max(150, "Cannot exceed 150 characters")
    .required("Email is required"),

  phoneNumber: Yup.string()
    .required("Phone number is required")
    .max(20, "Cannot exceed 20 characters"),

  alternateNumber: Yup.string()
    .max(20, "Cannot exceed 20 characters"),

  website: Yup.string().max(150, "Cannot exceed 150 characters"),
});

const LocationProfileForm: React.FC<Props> = ({ isAdd }) => {
  const navigate = useNavigate();
  const [profile, setProfile] = useState<LocationProfile>({
    attachmentIds: [],
    addressLine1: "",
    addressLine2: "",
    city: "",
    state: "",
    zipCode: "",
    supportEmail: "",
    phoneNumber: "",
    alternateNumber: "",
    website: "",
    logoUrl: "",
  });

  const [loading, setLoading] = useState(false);
  const [errors, setErrors] = useState<any>([]);
  const [attachmentIds, setAttachmentIds] = useState<any>();
  const [isEdit, setIsEdit] = useState<boolean>(false);

  const API_URL = getAPIBaseUrl();
  console.log("API Base URL:", API_URL);
  console.log("Profile:", profile);
console.log("Profile after fetch:", API_URL + encodeURI(profile.logoUrl ?? ""));
  const { id } = useParams();
  const formik = useFormik<any>({
    enableReinitialize: true,
    initialValues: profile,
    validationSchema: locationProfileSchema,
    onSubmit: (values) => {
      const toastId = toast.loading("Please wait...");
      setLoading(true);
      setTimeout(async () => {
        const updatedData = { ...profile, ...values };
        const dis = values.hasDisability;
        setProfile(updatedData);

        const parsedValues = {
          ...updatedData,
          attachmentIds: attachmentIds,
        };

        // alert(JSON.stringify(parsedValues));
        // return;
        let result=[];
    if (isAdd) {
        result = await locationService.saveLocation(parsedValues);
    } else {
         result = await locationService.updateLocation(id,parsedValues);
    }
        if (result && result?.id) {
          setLoading(false);
          navigate("/locations");
        } else {
          setErrors(result.errors || result.Errors);
        }

        toast.dismiss(toastId);

        setLoading(false);
      }, 1000);
    },
  });

  useEffect(() => {
    if (!isAdd) {
      const fetchProfile = async () => {
        setLoading(true);
        try {
          const res = await locationService.getById(id);
          if (!res || !res.id) {
            throw new Error("Profile not found");
          }
          setProfile(res);
        } catch (error) {
          console.error("Failed to fetch profile", error);
        } finally {
          setLoading(false);
        }
      };

      fetchProfile();
    }
  }, [isAdd]);

  if (loading) return <div>Loading profile...</div>;

  return (
    <form onSubmit={formik.handleSubmit} noValidate className="form">
      <div className="card mb-5 mb-xl-10">
        <div className="card-body p-9">
          <RequiredMarker />

          <FormErrorAlert errors={errors} />

          <div className="row mb-6">
            <ValidationField
              className="col-md-5"
              label="Address Line 1"
              isRequired
              name="addressLine1"
              type="text"
              placeholder="Enter address line 1"
              formik={formik}
            />
          </div>

          <div className="row mb-6">
            <ValidationField
              className="col-md-5"
              label="Address Line 2"
              name="addressLine2"
              type="text"
              placeholder="Enter address line 2"
              formik={formik}
            />
          </div>

          <div className="row mb-6">
            <ValidationField
              className="col-md-5"
              label="City"
              isRequired
              name="city"
              type="text"
              placeholder="Enter city"
              formik={formik}
            />
            <ValidationField
              className="col-md-5"
              label="State"
              isRequired
              name="state"
              type="text"
              placeholder="Enter state"
              formik={formik}
            />
          </div>

          <div className="row mb-6">
            <ValidationField
              className="col-md-5"
              label="Postal code"
              isRequired
              name="zipCode"
              type="text"
              placeholder="Enter postal code"
              formik={formik}
            />
            <ValidationField
              className="col-md-5"
              label="Phone number"
              isRequired
              name="phoneNumber"
              type="text"
              placeholder="Enter phone number"
              formik={formik}
            />
          </div>

          <div className="row mb-6">
            <ValidationField
              className="col-md-5"
              label="Email"
              isRequired
              name="supportEmail"
              type="text"
              placeholder="Enter email"
              formik={formik}
            />
            <ValidationField
              className="col-md-5"
              label="Alternate number"
              
              name="alternateNumber"
              type="text"
              placeholder="Enter alternate number"
              formik={formik}
            />
          </div>
          <div className="row mb-6">
            {" "}
            <ValidationField
              className="col-md-5"
              label="Website"
            
              name="website"
              type="text"
              placeholder="Enter website url"
              formik={formik}
            />
          </div>
          <div className="row">
            <div className="col-md-5">
              <label className="fw-bold fs-6 mb-2">Logo</label>
              {isAdd == true || isEdit == true ? (
                <div>
                  <DragAndDropFileUpload
                    appId=""
                    onUpload={(files: any) => setAttachmentIds(files)}
                    allowedTypes={allowedTypes}
                    url={`api/fileupload/locations`}
                  />
                  <ul className="fs-7 ">
                    <li>Max size: 2 MB</li>
                    <li>Types supported: PNG, JPG/JPEG</li>
                  </ul>
                  {isAdd== false&& <button
                        type="button"
                        className="btn btn-sm btn-theme my-7"
                        onClick={() => {
                          setIsEdit(false);
                          setAttachmentIds([]);
                        }}
                      >
                        Cancel
                      </button>}
                </div>
                
              ) : (
                <div>
                  {profile.logoUrl && (
                    <div>
                      <div>
                      <img
                        src={API_URL +encodeURI(profile.logoUrl)}
                        alt="Logo"
                        width={250}
                        height={250}
                        style={{ objectFit: "contain" }}
                      /></div>
                       <div>
                      <button
                        type="button"
                        className="btn btn-sm btn-theme my-7"
                        onClick={() => setIsEdit(true)}
                      >
                        Edit
                      </button>
                      </div>
                      {/* <button type="button">
                      Remove
                    </button> */}
                    </div>
                  )}
                </div>
              )}
            </div>
          </div>
          {/* Footer */}
          <div className="card-footer d-flex justify-content-center py-6 px-9">
            <button type="submit" className="btn btn-theme" disabled={loading}>
              <span className="indicator-label">Submit</span>
              {formik.isSubmitting && (
                <span className="indicator-progress">
                  Please wait...{" "}
                  <span className="spinner-border spinner-border-sm align-middle ms-2"></span>
                </span>
              )}
            </button>
            <Link to={"/locations"} className=" btn btn-light mx-3">
              Cancel
            </Link>
          </div>
        </div>
      </div>
    </form>
  );
};

export default LocationProfileForm;
