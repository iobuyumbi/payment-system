import { useState, useEffect } from "react";
import * as Yup from "yup";
import clsx from "clsx";
import { Link } from "react-router-dom";
import { useFormik } from "formik";
import { PasswordMeterComponent } from '../../../../_metronic/assets/ts/components'

import {
  ResetModel,
  resetInitValues as initialValues,
} from "../../../../_models/reset-password";

const forgotPasswordSchema = Yup.object().shape({
  code: Yup.string()
    .min(3, "Minimum 3 characters")
    .max(50, "Maximum 50 characters")
    .required("Code is required"),
  password: Yup.string()
    .min(8, "password must contain 8 or more characters")
    .max(50, "Maximum 50 characters")
    .matches(/[0-9]/, "Your password must have at least one digit character")
    .matches(
      /[a-z]/,
      "Your password must have at least one lowercase character"
    )
    .matches(
      /[A-Z]/,
      "Your password must have at least one uppercase character"
    )
    .matches(
      /\W/,
      "Your password must have at least one symbol character"
    )
    .required("Password is required"),
  changepassword: Yup.string()
    .oneOf([Yup.ref("password")], "Password and Confirm Password didn't match"),
});

export function ResetPassword() {
  const [data, setData] = useState<ResetModel>(initialValues);
  const [loading, setLoading] = useState(false);
  const [hasErrors, setHasErrors] = useState<boolean | undefined>(undefined);
  const formik = useFormik<ResetModel>({
    initialValues,
    validationSchema: forgotPasswordSchema,
    onSubmit: (values, { setStatus, setSubmitting }) => {
      setLoading(true);
      setHasErrors(undefined);
      setTimeout(() => {
        values.code = String(values.code);
        values.password = String(values.password);
        values.changepassword = String(values.changepassword);

        const updatedData = Object.assign(data, values);
        setData(updatedData)
        //setStatus("Password Changed")
        // setSubmitting(false)
      }, 1000);
    },
  });
  useEffect(() => {
    PasswordMeterComponent.bootstrap()
  }, [])

  return (
    <form
      className="form w-100 fv-plugins-bootstrap5 fv-plugins-framework"
      noValidate
      id="kt_login_password_reset_form"
      onSubmit={formik.handleSubmit}
    >
      <div className="text-center mb-10">
        {/* begin::Title */}
        <h1 className="text-gray-900 fw-bolder mb-3">Reset Password</h1>
        <div className='text-gray-500 fw-semibold fs-6'>Solidaridad portal</div>
        {/* end::Title */}

        {/* begin::Link */}
        {/* <div className="text-gray-500 fw-semibold fs-6">
          Enter code sent on email to reset your password.
        </div> */}
        {/* end::Link */}
      </div>

      {/* begin::Title */}
      {hasErrors === true && (
        <div className="mb-lg-15 alert alert-danger">
          <div className="alert-text font-weight-bold">
            Sorry, looks like there are some errors detected, please try again.
          </div>
        </div>
      )}

      {hasErrors === false && (
        <div className="mb-10 bg-light-info p-8 rounded">
          <div className="text-info">Reset Password successful </div>
        </div>
      )}
      {/* end::Title */}

      {/* begin::Form group */}

      <div className="fv-row mb-8">
        <label className="form-label fw-bolder text-gray-900 fs-6">
          Reset code
        </label>
        <input
          type="text"
          placeholder="Enter code sent on email to reset your password."
          autoComplete="off"
          {...formik.getFieldProps("code")}
          className={clsx(
            "form-control bg-transparent",
            { "is-invalid": formik.touched.code && formik.errors.code },
            {
              "is-valid": formik.touched.code && !formik.errors.code,
            }
          )}
        />
        {formik.touched.code && formik.errors.code && (
          <div className="fv-plugins-message-container">
            <div className="fv-help-block">
              <span role="alert">{formik.errors.code}</span>
            </div>
          </div>
        )}
      </div>
      {/* end::Form group */}
      {/* begin::Form group Password */}
      <div className="fv-row mb-8" data-kt-password-meter="true">
        <div className="mb-1">
          <label className="form-label fw-bolder text-gray-900 fs-6">
            Password
          </label>
          <div className="position-relative mb-3">
            <input
              type="password"
              placeholder="Password"
              autoComplete="off"
              {...formik.getFieldProps("password")}
              className={clsx(
                "form-control bg-transparent",
                {
                  "is-invalid":
                    formik.touched.password && formik.errors.password,
                },
                {
                  "is-valid":
                    formik.touched.password && !formik.errors.password,
                }
              )}
            />
            {formik.touched.password && formik.errors.password && (
              <div className="fv-plugins-message-container">
                <div className="fv-help-block">
                  <span role="alert">{formik.errors.password}</span>
                </div>
              </div>
            )}
          </div>
          {/* begin::Meter */}
          <div
            className="d-flex align-items-center mb-3"
            data-kt-password-meter-control="highlight"
          >
            <div className="flex-grow-1 bg-secondary bg-active-success rounded h-5px me-2"></div>
            <div className="flex-grow-1 bg-secondary bg-active-success rounded h-5px me-2"></div>
            <div className="flex-grow-1 bg-secondary bg-active-success rounded h-5px me-2"></div>
            <div className="flex-grow-1 bg-secondary bg-active-success rounded h-5px"></div>
          </div>
          {/* end::Meter */}
        </div>
        <div className="text-muted">
          Use 8 or more characters with a mix of letters, numbers & symbols.
        </div>
      </div>
      {/* end::Form group */}

      {/* begin::Form group Confirm password */}
      <div className="fv-row mb-5">
        <label className="form-label fw-bolder text-gray-900 fs-6">
          Confirm Password
        </label>
        <input
          type="password"
          placeholder="Password confirmation"
          autoComplete="off"
          {...formik.getFieldProps("changepassword")}
          className={clsx(
            "form-control bg-transparent",
            {
              "is-invalid":
                formik.touched.changepassword && formik.errors.changepassword,
            },
            {
              "is-valid":
                formik.touched.changepassword && !formik.errors.changepassword,
            }
          )}
        />
        {formik.touched.changepassword && formik.errors.changepassword && (
          <div className="fv-plugins-message-container">
            <div className="fv-help-block">
              <span role="alert">{formik.errors.changepassword}</span>
            </div>
          </div>
        )}
      </div>
      {/* end::Form group */}

      {/* begin::Form group */}
      <div className="d-flex flex-wrap justify-content-center pb-lg-0">
        <button
          type="submit"
          id="kt_password_reset_submit"
          className="btn btn-theme"
        >
          <span className="indicator-label">Submit</span>
          {loading && (
            <span className="indicator-progress">
              Please wait...
              <span className="spinner-border spinner-border-sm align-middle ms-2"></span>
            </span>
          )}
        </button>
        <Link to="/auth/login">
          <button
            type="button"
            id="kt_login_password_reset_form_cancel_button"
            className="btn btn-light"
            disabled={formik.isSubmitting}
          >
            Cancel
          </button>
        </Link>{" "}
      </div>

      {/* end::Form group */}
    </form>
  );
}
