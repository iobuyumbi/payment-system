import { useEffect, useState } from "react";
import { toast } from "react-toastify";
import * as Yup from "yup";
import { useFormik } from "formik";
import { PageTitleWrapper } from "../../../_metronic/layout/components/toolbar/page-title";
import { Link, useNavigate, useParams } from "react-router-dom";
import { PageTitle, PageLink } from "../../../_metronic/layout/core";
import {
  RequiredMarker,
  ValidationField,
  ValidationSelect,
  ValidationTextArea,
} from "../../../_shared/components";
import FarmerService from "../../../services/FarmerService";
import { Content } from "../../../_metronic/layout/components/content";
import CountryService from "../../../services/CountryService";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import { useDispatch, useSelector } from "react-redux";
import { resetFarmerState } from "../../../_features/farmers/farmerSlice";
import { toastNotify } from "../../../services/NotifyService";
import CountryDropdown from "../../components/CountryDropdown";
import AdminLevelService from "../../../services/AdminLevelService";
import CustomComboBox from "../../../_shared/CustomComboBox/Index";
import CooperativeService from "../../../services/CooperativeService";
import { OptionType } from "../../../_models/option-type";
import MonthDropdown from "../../../_shared/CustomDatePickers/MonthDropdown";
import YearDropdown from "../../../_shared/CustomDatePickers/YearDropdown";
import { isAllowed } from "../../../_metronic/helpers/ApiUtil";
import { Error401 } from "../errors/components/Error401";
import CommonService from "../../../services/CommonService";
import ProjectService from "../../../services/ProjectService";
import FormErrorAlert from "../../../_shared/FormErrorAlert/Index";
import { getSelectedCountryCode } from "../../../_metronic/helpers/AppUtil";

const cooperativeService = new CooperativeService();
const countryData = new CountryService();
const adminLevelService = new AdminLevelService();
const commonService = new CommonService();
const projectService = new ProjectService();

const breadCrumbs: Array<PageLink> = [
  {
    title: 'Dashboard',
    path: '/dashboard',
    isSeparator: false,
    isActive: true,
  },
  {
    title: '',
    path: '',
    isSeparator: true,
    isActive: true,
  },
  {
    title: 'Farmers',
    path: '/farmers',
    isSeparator: false,
    isActive: true,
  },
  {
    title: '',
    path: '',
    isSeparator: true,
    isActive: true,
  },
];

const farmerSchema = Yup.object().shape({
  firstName: Yup.string()
    .required("First Name is required")
    .max(50, "Cannot exceed 50 characters"),
  otherNames: Yup.string().required("Other Names is required").max(100, "Cannot exceed 100 characters"),
  gender: Yup.string().required("Gender is required"),
  hasDisability: Yup.string().required("Disability field is required"),
  countryId: Yup.string().required("Required"),
  adminLevel1Id: Yup.string().required("Required"),
  adminLevel2Id: Yup.string().required("Required"),
  adminLevel3Id: Yup.string().required("Required"),
  village: Yup.string().required("Required"),
  email: Yup.string()
    .email("Invalid email address")
    .max(150, "Cannot exceed 150 characters")
    .notRequired(),
  systemId: Yup.string().required("Uwanjani system ID is required"),
  participantId: Yup.string().required("Participant ID is required"),
  paymentPhoneNumber: Yup.string()
    .required("Payment phone number is required")
    .max(20, "Cannot exceed 20 characters"),
  isFarmerPhoneOwner: Yup.boolean(),
  mobile: Yup.string()
    .required("Mobile is required")
    .max(15, "Cannot exceed 15 characters"),
  cooperative: Yup.array().required('Cooperative is required'),
  projectIds: Yup.array(),
  // cooperative: Yup.object({
  //   value: Yup.string().required('Select a cooperative'),
  // }).nullable()
  //   .required('Select a cooperative'), // Add this to ensure the object itself is required

  // phoneOwnerFullName: Yup.string().when('isFarmerPhoneOwner', (isFarmerPhoneOwner, schema) => {
  //   return isFarmerPhoneOwner? schema
  //     .required('Phone owner’s full name is required').max(150, 'Cannot exceed 150 characters') : schema;
  // }),
  // participantID: Yup.string().required('Participant ID is required'),
  // nationalID: Yup.string().required('National ID is required'),

  // phoneOwnerFullName: Yup.string().when('isFarmerPhoneOwner', {
  //   is: 'No',
  //   then: Yup.string().required('Phone owner’s full name is required').max(150, 'Cannot exceed 150 characters'),
  // }),
  // phoneOwnerNationalID: Yup.string().when('isFarmerPhoneOwner', {
  //   is: 'No',
  //   then: Yup.string().required('Phone owner’s national ID is required').max(50, 'Cannot exceed 50 characters'),
  // }),
  // phoneOwnerRelationshipWithFarmer: Yup.string().when('isFarmerPhoneOwner', {
  //   is: 'No',
  //   then: Yup.string().required('Phone owner’s relationship with farmer is required').max(50, 'Cannot exceed 50 characters'),
  // }),
  // phoneOwnerAddress: Yup.string().when('isFarmerPhoneOwner', {
  //   is: 'No',
  //   then: Yup.string().required('Phone owner’s address is required').max(500, 'Cannot exceed 500 characters'),
  // }),
});

const farmerService = new FarmerService();

export function AddFarmer() {
  const navigate = useNavigate();
  const dispatch = useDispatch();

  var farmer = useSelector((state: any) => state?.farmers);

  const [farmerCoperatives, setFarmerCoperatives] = useState<any[]>([]);
  const [farmerProjects, setFarmerProjects] = useState<any[]>([]);
  const [projects, setProjects] = useState<any>([]);

  const [data, setData] = useState<any>({
    ...farmer,
    countryId: farmer?.country?.id,
    adminLevel1Id: farmer?.adminLevel1?.id,
    adminLevel2Id: farmer?.adminLevel2?.id,
    adminLevel3Id: farmer?.adminLevel3?.id,
    cooperative: farmerCoperatives,
    projectIds: farmerProjects
  });

  const { id } = useParams();
  const [title] = useState<any>(id == null ? "Add Farmer" : "Edit Farmer");
  const [countries, setCountries] = useState<any>();

  const [levelArr1, setLevelArr1] = useState<any[]>([]);
  const [levelArr2, setLevelArr2] = useState<any[]>([]);
  const [levelArr3, setLevelArr3] = useState<any[]>([]);
  const [errors, setErrors] = useState<any>([]);

  const [labels, setLabels] = useState({
    adminLevel1: "",
    adminLevel2: "",
    adminLevel3: "",
    adminLevel4: "Village",
  });

  const [loading, setLoading] = useState(false);
  const [showPhoneOwnerFields, setShowPhoneOwnerFields] = useState<boolean>(false);
  const [isDisabled, setIsDisabled] = useState<any>(true);

  const formik = useFormik<any>({
    enableReinitialize: true,
    initialValues: data,
    validationSchema: farmerSchema,
    onSubmit: (values) => {
      const toastId = toast.loading("Please wait...");
      setLoading(true);
      setTimeout(async () => {
        const updatedData = { ...data, ...values };
        const dis = values.hasDisability;
        setData(updatedData);

        const documentTypeOptions = documentTypes.map((c: any) => ({
          value: c.id,
          label: c.name
        }));

        const transformedProjectIds = values.projectIds.map((p: any) => p.value);

        const parsedValues = {
          ...updatedData,
          accessToMobile: values.accessToMobile === true,
          hasDisability: dis.toString() === "true",
          isFarmerPhoneOwner: values.isFarmerPhoneOwner.toString() === "true",
          gender: +values.gender,
          documentType: documentTypeOptions[0],
          projectIds: transformedProjectIds
        };

        if (
          parsedValues.documentType &&
          parsedValues.documentType.value === null &&
          parsedValues.documentType.label === "None"
        ) {
          parsedValues.documentType = null;
          parsedValues.documentTypeId = null;
        }

        if (title === "Add Farmer") {
          const result = await farmerService.postFarmerData(parsedValues);

          if (result && result.id) {
            setLoading(false);
            toastNotify(id, "Success");
            navigate("/farmers");
          } else {
            setErrors(result.errors || result.Errors);
          }

          toast.dismiss(toastId);
        } else {
          const result = await farmerService.putFarmerData(parsedValues, id);

          if (result && result.id) {
            toastNotify(id, "Success");
            setLoading(false);
            navigate("/farmers");
          } else {
            setErrors(result.errors || result.Errors);
          }
          toast.dismiss(toastId);

        }
        setLoading(false);
      }, 1000);
    },
  });

  const getProjects = async (countryId: any) => {
    const data: any = {
      pageNumber: 1,
      pageSize: 10,
      countryId: countryId
    };
    projectService.getProjectData(data).then((projects: any) => {

      const options: any[] = projects.map((opt: any) => ({
        value: opt.id,
        label: opt.projectName,
      }));

      setProjects(options);
    });
  };

  const getCountries = async () => {
    try {
      countryData.getCountryData(1).then((data: any) => {
        if (data && data.length > 0) {
          const list = data.map((country: any) => ({
            id: country.id,
            name: country.countryName,
            code: country.code
          }));
          setCountries(list);
        }
      });
    } catch (error) {
      console.error("Error fetching countries:", error);
    }
  };

  const getL1 = async (level0_Id: any) => {
    try {
      const pageData = {
        pageNumber: 1,
        pageSize: 10000,
        filter: "",
        countryId: level0_Id
      };

      var response = await adminLevelService.getAdminLevel1Data(pageData);
      const list = response.map((country: any) => ({
        id: country.id,
        name: country.countyName,
      }));
      setLevelArr1(list);
    } catch (error) {
      console.error("Error fetching admin level 1:", error);
    }
  };

  const getL2 = async (level1_Id: any) => {
    try {
      const pageData = {
        pageNumber: 1,
        pageSize: 10000,
        filter: "",
        countyId: level1_Id
      };
      var response = await adminLevelService.getAdminLevel2Data(pageData);
      const list = response.map((country: any) => ({
        id: country.id,
        name: country.subCountyName,
      }));
      setLevelArr2(list);
    } catch (error) {
      console.error("Error fetching admin level 1:", error);
    }
  };

  const getL3 = async (level3_Id: any) => {
    try {
      const pageData = {
        pageNumber: 1,
        pageSize: 1000,
        filter: "",
        subCountyId: level3_Id
      };
      var response = await adminLevelService.getAdminLevel3Data(pageData);
      const list = response.map((country: any) => ({
        id: country.id,
        name: country.wardName,
      }));
      setLevelArr3(list);
    } catch (error) {
      console.error("Error fetching admin level 1:", error);
    }
  };

  const [cooperative, setCooperative] = useState<OptionType[]>([]);

  const bindCooperatives = async () => {
    const data = {
      pageNumber: 1,
      pageSize: 10,
      countryId: formik.values.countryId !== null && formik.values.countryId !== undefined ? formik.values.countryId : ""
    };
    var response = await cooperativeService.getCooperativeData(data);

    // Assuming setProjects is a function that sets state or performs some action
    const options: any[] = response.map((opt: any) => ({
      value: opt.id,
      label: opt.name,
    }));
    setCooperative(options);
  };

  const [documentTypes, setDocumentTypes] = useState<any>([]);

  const bindDocumentTypes = async () => {
    const data = {
      pageNumber: 1,
      pageSize: 10,
      countryId: formik.values.countryId !== null && formik.values.countryId !== undefined ? formik.values.countryId : ""
    };
    var response = await commonService.getDocumentTypes();

    // Assuming setProjects is a function that sets state or performs some action
    const options: any[] = [
      { id: null, name: "None" }, // Default option
      ...response?.map((opt: { id: number; typeName: string }) => ({
        id: opt.id,
        name: opt.typeName,
      })) || [],
    ];
    setDocumentTypes(options);
  };

  const getFarmerCooperatives = async () => {
    var response = await farmerService.getFarmerCooperatives(farmer.id);
    var response2 = await farmerService.getFarmerProjects(farmer.id);

    if (response) {
      // Set the form data explicitly
      formik.setValues({
        ...farmer,
        cooperative: response,
        projectIds: response2,
        countryId: farmer?.country?.id,
        adminLevel1Id: farmer?.adminLevel1?.id,
        adminLevel2Id: farmer?.adminLevel2?.id,
        adminLevel3Id: farmer?.adminLevel3?.id,
      });

    }
  }

  useEffect(() => {
    if (id == undefined || id == null) {
      dispatch(resetFarmerState());
      formik.resetForm();
    }

    if (title === "Add Farmer") {
      document.title = "Add Farmer";
      dispatch(resetFarmerState());
    }
    else {
      document.title = "Edit Farmer";
      getFarmerCooperatives();
      // getFarmerProjects();
    }
    bindDocumentTypes();
    getCountries();
  }, []);

  useEffect(() => {
    // Destructure formik values for easier access
    const { countryId } = formik.values;

    // Determine labels based on countryId
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

    bindCooperatives();
    getProjects(countryId);
  }, [formik.values.countryId]);

  useEffect(() => {
    const isFarmerPhoneOwnerValue = formik.values.isFarmerPhoneOwner.toString();
    setShowPhoneOwnerFields(isFarmerPhoneOwnerValue === "false" ? true : false);

    if (!showPhoneOwnerFields) {
      formik.setFieldValue("phoneOwnerName", ""),
        formik.setFieldValue("phoneOwnerNationalId", ""),
        formik.setFieldValue("phoneOwnerRelationWithFarmer", ""),
        formik.setFieldValue("phoneOwnerAddress", "");
    }
  }, [formik.values.isFarmerPhoneOwner]);

  useEffect(() => {
    setIsDisabled(formik.values.countryId !== "0" || formik.values.countryId ? false : true)

    // get admin level 1
    getL1(formik.values.countryId);
  }, [formik.values.countryId]);

  useEffect(() => {
    // get admin level 2
    getL2(formik.values.adminLevel1Id);
  }, [formik.values.adminLevel1Id]);

  useEffect(() => {
    // get admin level 3
    getL3(formik.values.adminLevel2Id);
  }, [formik.values.adminLevel2Id]);

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
      {isAllowed('farmers.add') ? <><PageTitleWrapper />
        <PageTitle breadcrumbs={breadCrumbs}>{title}</PageTitle>
        <div className="pt-2">
          <div>
            <form onSubmit={formik.handleSubmit} noValidate className="form">
              <div className="card mb-5 mb-xl-10">
                <div className="card-body p-9">
                  <RequiredMarker />

                  <FormErrorAlert errors={errors} />

                  <div className="pt-3 pb-5">
                    <span className="fw-semibold text-gray-600 text-uppercase fs-6 ls-1">
                      Basic Information
                    </span>
                    <div className="separator mt-2 mb-3"></div>
                  </div>
                  {/* 1 */}
                  <div className="row mb-6">
                    <ValidationField
                      className="col-md-5"
                      label="First Name"
                      isRequired
                      name="firstName"
                      type="text"
                      placeholder="Enter first name"
                      formik={formik}
                    />
                    <ValidationField
                      className="col-md-5 offset-md-1"
                      label="Other Names"
                      isRequired
                      name="otherNames"
                      type="text"
                      placeholder="Enter other name"
                      formik={formik}
                    />
                  </div>
                  <div className="row mb-6">
                    <ValidationSelect
                      className="col-md-5"
                      label="Document type"
                      name="documentTypeId"
                      type="text"
                      options={documentTypes}
                      formik={formik}
                    />
                    <ValidationField
                      className="col-md-5  offset-md-1"
                      label="Document ID (Optional)"
                      name="beneficiaryId"
                      type="text"
                      placeholder="Enter national ID"
                      formik={formik}
                    />
                  </div>
                  {/* 2 */}
                  <div className="row mb-6 d-flex ">
                    <div className="col-md-6">
                      <ValidationSelect
                        className="col-md-10"
                        label="Gender"
                        isRequired
                        name="gender"
                        type="number"
                        options={[
                          { id: null, name: "Select gender" },
                          { id: 1, name: "Male" },
                          { id: 2, name: "Female" },
                        ]}
                        formik={formik}
                      />
                    </div>
                    <div className="col-md-6 ">
                      <div className="row">
                        <div className="col-md-5 mt-2">
                          <MonthDropdown
                            className="col-md-12"
                            label="Month/Year of birth"
                            name="birthMonth"
                            formik={formik}
                          />
                          {/* <DatePicker
                          selected={formik.values.dateOfBirth}
                          className="form-control"
                          name="DOB"
                          onChange={(date) =>
                            formik.setFieldValue("dateOfBirth", date)
                          }
                        /> */}
                        </div>
                        <div className="col-md-5 mt-2">
                          <YearDropdown
                            className="col-md-12"
                            label="&nbsp;"
                            name="birthYear"
                            formik={formik}
                          />
                        </div>
                      </div>
                    </div>
                  </div>

                  {/* 3 */}
                  <div className="row mb-6">
                    <ValidationSelect
                      className="col-md-5"
                      label="Has disability"
                      isRequired
                      name="hasDisability"
                      options={[
                        { id: false, name: "No" },
                        { id: true, name: "Yes" },
                      ]}
                      formik={formik}
                    />
                    <div className="col-md-5 offset-md-1">
                      <CountryDropdown formik={formik} isRequired={true} isSelected={true} disabled={true} />
                    </div>
                  </div>

                  {/* Separator */}
                  <div className="py-5">
                    <span className="fw-semibold text-gray-600 text-uppercase fs-6 ls-1">
                      Contact Information
                    </span>
                    <div className="separator mt-2 mb-3"></div>
                  </div>

                  {/* 4 */}
                  <div className="row mb-6">
                    <ValidationField
                      className="col-md-5 "
                      label="Mobile"
                      isRequired
                      name="mobile"
                      type="text"
                      placeholder="Enter mobile"
                      formik={formik}
                    />

                    <ValidationField
                      className="col-md-5 offset-md-1"
                      label="Alternate contact number"
                      name="alternateContactNumber"
                      type="text"
                      placeholder="Enter alternate contact number"
                      formik={formik}
                    />
                  </div>

                  {/* 5 */}
                  <div className="row mb-6">
                    <ValidationField
                      className="col-md-5"
                      label="Email (Optional)"
                      name="email"
                      type="email"
                      placeholder="Enter email"
                      formik={formik}
                    />
                  </div>

                  {/* Separator */}
                  <div className="py-5">
                    <span className="fw-semibold text-gray-600 text-uppercase fs-6 ls-1">
                      Location
                    </span>
                    <div className="separator mt-2 mb-3"></div>
                  </div>

                  {/* 6 */}
                  <div className="row mb-6">

                    <ValidationSelect
                      className="col-md-5 "
                      label={labels.adminLevel1}
                      isRequired
                      name="adminLevel1Id"
                      type="text"
                      placeholder={`Enter ${labels.adminLevel1.toLowerCase()}`}
                      formik={formik}
                      options={levelArr1}
                    />
                    <ValidationSelect
                      className="col-md-5 offset-md-1"
                      label={labels.adminLevel2}
                      isRequired
                      name="adminLevel2Id"
                      type="text"
                      placeholder={`Enter ${labels.adminLevel2.toLowerCase()}`}
                      formik={formik}
                      options={levelArr2}
                    />
                  </div>

                  {/* 7 */}
                  <div className="row mb-6">
                    <ValidationSelect
                      className="col-md-5 "
                      label={labels.adminLevel3}
                      isRequired
                      name="adminLevel3Id"
                      type="text"
                      placeholder={`Enter ${labels.adminLevel3.toLowerCase()}`}
                      formik={formik}
                      options={levelArr3}
                    />
                    <ValidationField
                      className="col-md-5 offset-md-1"
                      label={labels.adminLevel4}
                      isRequired
                      name="village"
                      type="text"
                      placeholder={`Enter ${labels.adminLevel4.toLowerCase()}`}
                      formik={formik}
                    />
                  </div>

                  {/* Separator */}
                  <div className="py-5">
                    <span className="fw-semibold text-gray-600 text-uppercase fs-6 ls-1">
                      Other
                    </span>
                    <div className="separator mt-2 mb-3"></div>
                  </div>

                  {/* 9 */}
                  <div className="row mb-6 ">
                    <div className="col-md-5 ">
                      <CustomComboBox
                        label="Cooperative(s)"
                        name="cooperative"
                        formik={formik}
                        options={cooperative}
                        isMulti={true}
                      />
                      {/* <CooperativeDropdown formik={formik} isRequired={true} isDisabled={isDisabled} countryId={formik.values.countryId} /> */}
                    </div>

                    <div className="col-md-5 offset-md-1">
                      <CustomComboBox
                        label="Project"
                        name="projectIds"
                        formik={formik}
                        options={projects}
                        isMulti={true}
                      />
                      {/* <CooperativeDropdown formik={formik} isRequired={true} isDisabled={isDisabled} countryId={formik.values.countryId} /> */}
                    </div>
                  </div>

                  {/* 10 */}
                  <div className="row mb-6">
                    <div className="col-md-5">
                      <ValidationField
                        label="Uwanjani system ID (Mandatory)"
                        isRequired
                        name="systemId"
                        type="text"
                        placeholder="Enter system ID"
                        formik={formik}
                      />
                      <p className="text-gray-500 fs-7 py-2">
                        This is Solidaridad's auto-generated unique ID
                      </p>
                    </div>
                    <div className="col-md-5 offset-md-1">
                      <ValidationField
                        label="Solidaridad Participant ID"
                        isRequired
                        name="participantId"
                        type="text"
                        placeholder="Enter participant ID"
                        formik={formik}
                      />
                      <p className="text-gray-500 fs-7 py-2">
                        As shown in participant card
                      </p>
                    </div>

                  </div>

                  {/* 11 */}

                  <div className="row mb-6">
                    <div className="col-md-5">
                      <label className="fs-6 fw-bold">Enumeration date</label>
                      <div className="col-12 mt-2">
                        <DatePicker
                          selected={formik.values.enumerationDate ?? new Date()}
                          className="form-control"
                          name="enumerationDate"
                          onChange={(date) =>
                            formik.setFieldValue("enumerationDate", date)
                          }
                        />
                      </div>
                    </div>
                  </div>

                  {/* Separator */}
                  <div className="py-5">
                    <span className="fw-semibold text-gray-600 text-uppercase fs-6 ls-1">
                      Payment Information
                    </span>
                    <div className="separator mt-2 mb-3"></div>
                  </div>

                  {/* 12 */}
                  <div className="row mb-6">
                    <ValidationField
                      className="col-md-5"
                      label="Payment Phone Number"
                      isRequired
                      name="paymentPhoneNumber"
                      type="text"
                      placeholder="Enter payment phone number"
                      formik={formik}
                    />

                    <ValidationSelect
                      className="col-md-5 offset-md-1"
                      label="Is the farmer the phone owner?"
                      isRequired
                      name="isFarmerPhoneOwner"
                      options={[
                        { id: true, name: "Yes" },
                        { id: false, name: "No" },
                      ]}
                      formik={formik}
                    />
                  </div>

                  {/* 13 */}
                  <div className="row mb-6">
                    {showPhoneOwnerFields && (
                      <>
                        <div className="row mb-6">
                          <div className="col-md-5">
                            <ValidationField
                              label="Payment nominee full name"
                              isRequired
                              name="phoneOwnerName"
                              type="text"
                              placeholder="Enter payment nominee name"
                              formik={formik}
                            />
                            <p className="text-gray-500 fs-7 py-2">
                              As appears in the national ID
                            </p>
                          </div>
                          <div className="col-md-5 offset-md-1">
                            <ValidationField
                              label="Payment nominee national ID"
                              isRequired
                              name="phoneOwnerNationalId"
                              type="text"
                              placeholder="Enter Payment nominee national ID"
                              formik={formik}
                            />
                          </div>
                        </div>

                        <div className="row mb-6">
                          <ValidationField
                            className="col-md-5"
                            label="Payment nominee's relationship with farmer"
                            isRequired
                            name="phoneOwnerRelationWithFarmer"
                            type="text"
                            placeholder="Enter Payment nominee's relationship with farmer"
                            formik={formik}
                          />
                          <ValidationTextArea
                            className="col-md-5 offset-md-1"
                            label="Payment nominee address"
                            isRequired
                            name="phoneOwnerAddress"
                            placeholder="Enter payment nominee address"
                            formik={formik}
                          />
                        </div>
                      </>
                    )}
                  </div>

                  {/* Footer */}
                  <div className="card-footer d-flex justify-content-center py-6 px-9">
                    <button
                      type="submit"
                      className="btn btn-theme"
                      disabled={loading}
                    >
                      <span className="indicator-label">Submit</span>
                      {formik.isSubmitting && (
                        <span className="indicator-progress">
                          Please wait...{" "}
                          <span className="spinner-border spinner-border-sm align-middle ms-2"></span>
                        </span>
                      )}
                    </button>
                    <Link to={"/farmers"} className=" btn btn-light mx-3">
                      Cancel
                    </Link>
                  </div>
                </div>
              </div>
            </form>
          </div>
        </div>
      </> : <Error401 />}
    </Content>
  );
}
