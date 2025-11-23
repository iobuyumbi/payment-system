import React, { useState } from 'react';
import clsx from 'clsx';

interface Year {
  value: number;
  label: string;
}

const currentYear = new Date().getFullYear();
const maxYear = currentYear - 18;

const years: Year[] = [];

for (let year = maxYear; year >= 1900; year--) {
  years.push({ value: year, label: year.toString() });
}


const YearDropdown = (props: any) => {
  const { formik, className, isRequired, name, placeholder, defaultValue, label, text, ...rest } = props;

  return (<>
    <div className={className}>
      <label className='fw-bold fs-6 mb-2'>{label}</label>
      {isRequired ? <span className='text-danger'>&nbsp;*</span> : null}
      <select
        {...rest}
        {...formik.getFieldProps(name)}
        name={name}
        text={text}
        defaultValue={defaultValue}
        className={clsx(
          'form-select  mb-3 mb-lg-0',
          { 'is-invalid': formik.touched[name] && formik.errors[name] },
        )}
        autoComplete='off'
        disabled={formik.isSubmitting}
      >
        <option value="" hidden>{text}</option>
        {years.length > 0 && years.map((item: any) => (
          <option key={item.value} value={item.value}>
            {item.label}
          </option>
        ))}
      </select>
      {formik.touched[name] && formik.errors[name] && (
        <div className='fv-plugins-message-container'>
          <div className='fv-help-block'>
            <span role='alert'>{formik.errors[name]}</span>
          </div>
        </div>
      )}
    </div>
  </>

  );
};

export default YearDropdown;