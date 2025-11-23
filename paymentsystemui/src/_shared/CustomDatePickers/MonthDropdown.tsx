import React, { useState } from 'react';
import clsx from 'clsx';

interface Month {
  value: number;
  label: string;
}

const months: Month[] = [
  { value: 1, label: 'January' },
  { value: 2, label: 'February' },
  { value: 3, label: 'March' },
  { value: 4, label: 'April' },
  { value: 5, label: 'May' },
  { value: 6, label: 'June' },
  { value: 7, label: 'July' },
  { value: 8, label: 'August' },
  { value: 9, label: 'September' },
  { value: 10, label: 'October' },
  { value: 11, label: 'November' },
  { value: 12, label: 'December' },
];

const MonthDropdown = (props: any) => {
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
        {months.length > 0 && months.map((item: any) => (
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

export default MonthDropdown;