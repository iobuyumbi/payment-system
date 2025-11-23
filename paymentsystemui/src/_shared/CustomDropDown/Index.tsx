import React from "react";
import clsx from "clsx";

export default function CustomDropDown(props: any) {
  const { className, name, label, isRequired, formik, data, disabled, ...rest } = props;
  return (
    <div className={className}>
      <label className="fw-bold fs-6 mb-2">{label}</label>
      {isRequired ? <span className="text-danger">&nbsp;*</span> : null}
      <select
        {...rest}
        {...formik.getFieldProps(props.fieldname)}
        name={props.fieldname}

        className={clsx("form-select mb-3 mb-lg-0", {
          "is-invalid": formik.touched[name] && formik.errors[name],
        })}
        autoComplete="off"
        disabled={disabled || formik.isSubmitting}
      >
        <option value="" hidden>
          Select {name}
        </option>
        {data.length > 0 &&
          data.map((item: any) => (
            <option key={item.id} value={item.id}>
              {/* {item.name} */}
              {item[`${name}Name`]}
            </option>
          ))}
      </select>
      {formik.touched[name] && formik.errors[name] && (
        <div className="fv-plugins-message-container">
          <div className="fv-help-block">
            <span role="alert">{formik.errors[name]}</span>
          </div>
        </div>
      )}
    </div>
  );
}
