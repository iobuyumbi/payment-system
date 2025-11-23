
import { object, string, ref as yupRef } from "yup";
import { useState } from "react";
import FormErrorAlert from "../../../../_shared/FormErrorAlert/Index";
import { AUTH_LOCAL_STORAGE_KEY } from "../core/AuthHelpers";
import UserService from "../../../../services/UserService";
import { ValidationField } from "../../../../_shared/components";
import { useFormik } from "formik";
import { toast } from "react-toastify";

const userService = new UserService();

export default function ForceChangePassword() {
  const [id, setId] = useState<any>(null);
  const [isLoading, setIsLoading] = useState(false);
  const [errors, setErrors] = useState<any>([]);

  // 1. Define your form validation schema using Yup
  const validationSchema = object({
    oldPassword: string()
      .min(8, "Password should be minimum of 8 characters")
      .required("Old password is required"),
    newPassword: string()
      .min(8, "Password should be minimum of 8 characters")
      .matches(/[a-zA-Z]/, "Password should contain at least one letter")
      .matches(/[0-9]/, "Password should contain at least one number")
      .matches(
        /[^a-zA-Z0-9]/,
        "Password should contain at least one special character"
      )
      .required("New password is required"),
    confirmPassword: string()
      .oneOf([yupRef("newPassword")], "Passwords must match")
      .required("Confirm password is required"),
  });

  // 2. Define your form
  const formik = useFormik({
    initialValues: {
      oldPassword: '',
      newPassword: '',
      confirmPassword: '',
    },
    validationSchema: validationSchema,
    onSubmit: async (values) => {
      setErrors([]);
      setIsLoading(true);
      try {
        const response = await userService.putUserPassword(values);
        if (response.id) {
          // Update requiresPasswordChange in local storage
          const storedUser = localStorage.getItem(AUTH_LOCAL_STORAGE_KEY);
          if (storedUser) {
            const userData = JSON.parse(storedUser);
            userData.requiresPasswordChange = false;
            localStorage.setItem(
              AUTH_LOCAL_STORAGE_KEY,
              JSON.stringify(userData)
            );
          }
          toast.success('Password updated successfully');

          setTimeout(() => {
            window.location.reload();
          }, 2000);
        } else if (
          (response.errors && response.errors.length > 0) ||
          (response.Errors && response.Errors.length > 0)
        ) {
          console.log(response)
          // Handle the errors accordingly
          setErrors(response.errors || response.Errors)
          // formik.setErrors({
          //   ...response.errors || response.Errors,
          // });
        }
      } catch (error) {
        console.log(error);
      } finally {
        setIsLoading(false);
      }
    },
  });


  return (
    <div className="form-container" >
      <form onSubmit={formik.handleSubmit} noValidate className="form">
        {/* Errors */}
        <FormErrorAlert errors={errors} />

        <div className=" mb-5" >
          {/* Old password */}
          <ValidationField
            isRequired
            formik={formik}
            name="oldPassword"
            label="Old Password"
            placeholder="Enter old password"
            type="password"
          />
        </div>
        <p style={{ fontSize: "0.875rem", color: "#6c757d" }}>
          <ul>
            <li>Password should be a minimum of 8 characters</li>
            <li>It should include an alphabet, a digit, and a special character.</li>
          </ul>
        </p>

        <div className=" mb-5" >
          {/* New password */}
          <ValidationField
            isRequired
            formik={formik}
            name="newPassword"
            label="New Password"
            placeholder="Enter new password"
            type="password"
          />
        </div>

        <div className=" mb-5" >
          {/* Confirm password */}
          <ValidationField
            isRequired
            formik={formik}
            name="confirmPassword"
            label="Confirm Password"
            placeholder="Confirm password"
            type="password"
          />
        </div>

        <button
          type="submit"
          className="btn btn-theme"
          style={{
            width: "100%",
          }}
        >
          {!isLoading && <span className='indicator-label'>Submit</span>}
          {isLoading && (
            <span className='indicator-progress' style={{ display: 'block' }}>
              Please wait...
              <span className='spinner-border spinner-border-sm align-middle ms-2'></span>
            </span>
          )}
        </button>
      </form>
    </div>

  );
}
