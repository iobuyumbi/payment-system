import clsx from 'clsx'
import React, { useState } from 'react'


interface CounterProps {
  text: string
  count: string
  link: string
  boxColor: string
  color: string
}

export const Counter: React.FC<CounterProps> = ({ text, count, boxColor, link, color }) => {
  return (
    <div className={`text-center ${boxColor}`}>
      <h6 className={`fs-2 fw-bolder ${boxColor}`}>{count}</h6>
      <a href={link} className={`${color} fw-semibold fs-6 mt-2`}>
        {text}
      </a>
    </div>
  )
}

export default function MultiSelect(props: any) {
  const [selectedFlavors, setSelectedFlavors] = useState<any>([])

  const handleSelect = function (selectedItems: any) {
    const flavors: any = []
    for (let i = 0; i < selectedItems.length; i++) {
      flavors.push(selectedItems[i].value)
    }
    setSelectedFlavors(flavors)
  }

  return (
    <form>
      <select
        multiple={true}
        value={selectedFlavors}
        onChange={(e) => {
          handleSelect(e.target.selectedOptions)
        }}
      >
        {props.options.map((option: any) => (
          <option key={option.value} value={option.value}>
            {option.label}
          </option>
        ))}
      </select>
    </form>
  )
}

export const MultiCheckBox = (props: any) => {
  const { options, name, label, ...rest } = props
  return (
    <select
      className='form-select  fw-bolder'
      data-kt-select2='true'
      data-placeholder='Select option'
      data-allow-clear='true'
      data-kt-user-table-filter='status'
      data-hide-search='true'
      multiple={true}
      value={props.value}
      onChange={props.handleChange}
    >
      {options.map((option: any) => (
        <option key={option.value} value={option.value}>
          {option.label}
        </option>
      ))}
    </select>
  )
}

export const Loader = () => {
  return (
    <div className='loader'>
      <span></span>
    </div>
  )
}

export const RequiredMarker = () => {
  return (
    <div className='text-gray-600 mb-5 fs-6'>
      Required fields are marked with an asterisk <span className='text-danger'>*</span>
    </div>
  )
}

export const convertAmountToWords = (amount: any) => {
  var words = new Array()
  words[0] = ''
  words[1] = 'One'
  words[2] = 'Two'
  words[3] = 'Three'
  words[4] = 'Four'
  words[5] = 'Five'
  words[6] = 'Six'
  words[7] = 'Seven'
  words[8] = 'Eight'
  words[9] = 'Nine'
  words[10] = 'Ten'
  words[11] = 'Eleven'
  words[12] = 'Twelve'
  words[13] = 'Thirteen'
  words[14] = 'Fourteen'
  words[15] = 'Fifteen'
  words[16] = 'Sixteen'
  words[17] = 'Seventeen'
  words[18] = 'Eighteen'
  words[19] = 'Nineteen'
  words[20] = 'Twenty'
  words[30] = 'Thirty'
  words[40] = 'Forty'
  words[50] = 'Fifty'
  words[60] = 'Sixty'
  words[70] = 'Seventy'
  words[80] = 'Eighty'
  words[90] = 'Ninety'
  amount = amount.toString()
  var atemp = amount.split('.')
  var number = atemp[0].split(',').join('')
  var n_length = number.length
  var words_string = ''
  if (n_length <= 9) {
    var n_array: any = new Array(0, 0, 0, 0, 0, 0, 0, 0, 0)
    var received_n_array: any = new Array()
    for (var i = 0; i < n_length; i++) {
      received_n_array[i] = number.substr(i, 1)
    }
    for (var i = 9 - n_length, j = 0; i < 9; i++, j++) {
      n_array[i] = received_n_array[j]
    }
    for (var i = 0, j = 1; i < 9; i++, j++) {
      if (i == 0 || i == 2 || i == 4 || i == 7) {
        if (n_array[i] == 1) {
          n_array[j] = 10 + parseInt(n_array[j])
          n_array[i] = 0
        } else if (n_array[i] == 0) {
          n_array[j] = parseInt(n_array[j])
        } else {
          n_array[j] = parseInt(n_array[j]) + 10
        }
      }
    }
    let value: any
    for (var i = 0; i < 9; i++) {
      if (i == 0 || i == 2 || i == 4 || i == 7) {
        value = n_array[i] * 10
      } else {
        value = n_array[i]
      }
      if (value != 0) {
        words_string += words[value] + ' '
      }
      if ((i == 1 && value != 0) || (i == 0 && value != 0 && n_array[i + 1] == 0)) {
        words_string += 'Crores '
      }
      if ((i == 3 && value != 0) || (i == 2 && value != 0 && n_array[i + 1] == 0)) {
        words_string += 'Lakhs '
      }
      if ((i == 5 && value != 0) || (i == 4 && value != 0 && n_array[i + 1] == 0)) {
        words_string += 'Thousand '
      }
      if (i == 6 && value != 0 && n_array[i + 1] != 0 && n_array[i + 2] != 0) {
        words_string += 'Hundred and '
      } else if (i == 6 && value != 0) {
        words_string += 'Hundred '
      }
    }
    words_string = words_string.split('  ').join(' ')
  }
  return words_string
}

export class N2WIndian {
  private static readonly zeroTo99: string[] = []
  private static readonly place: string[] = 'Thousand|Lakh|Crore|Arab|Kharab|Nil'.split('|')

  static {
    const ones: string[] =
      '|One|Two|Three|Four|Five|Six|Seven|Eight|Nine|Ten|Eleven|Twelve|Thirteen|Fourteen|Fifteen|Sixteen|Seventeen|Eighteen|Nineteen'.split(
        '|'
      )

    const tens: string[] = '||Twenty|Thirty|Forty|Fifty|Sixty|Seventy|Eighty|Ninety'.split('|')

    for (let i = 0; i < 100; i++) {
      const t: number = Math.floor(i / 10)
      const o: number = i % 10
      N2WIndian.zeroTo99.push(t < 2 ? ones[i] : tens[t] + (o ? ' ' + ones[o] : ''))
    }
  }

  public static convert(x: string): string {
    let n: number = x.length
    x = n === 0 ? '00' : n === 1 || n % 2 === 0 ? '0' + x : x
    n = x.length
    let r = N2WIndian.zeroTo99[x.charCodeAt((n -= 2)) * 10 + x.charCodeAt(n + 1) - 528]
    if (n >= 1) {
      const v: string = N2WIndian.zeroTo99[x.charCodeAt((n -= 1)) - 48]
      if (v) r = v + ' Hundred' + (r ? ' ' + r : '')
    }
    for (let i = 0; n > 0; i++) {
      const v: string = N2WIndian.zeroTo99[x.charCodeAt((n -= 2)) * 10 + x.charCodeAt(n + 1) - 528]
      if (v) r = v + ' ' + N2WIndian.place[i] + (r ? ' ' + r : '')
    }
    return r || 'Zero'
  }
}

// loading stages of any page
export const LOADINGSTAGES = {
  IDLE: 0,
  LOADING: 1,
  POSITIVE: 3,
  NEGATIVE: 4,
  ERROR: 5,
  EMPTY: 6,
}

export const MyLgSixField = (props: any) => {
  const { formik } = props
  return (
    <div className='col-lg-5 my-2 mx-3'>
      <label className='form-label fw-bold fs-6 mt-3'>{props.label}</label>
      {props.isRequired ? <span className='text-danger'>*</span> : null}
      <input
        type='text'
        className='form-control form-control-lg '
        name={props.name}
        value={formik.values[props.name]}
        onChange={formik.handleChange}
        onBlur={formik.handleBlur}
      />
      {formik.touched[props.name] && formik.errors[props.name] ? (
        <div className='fv-plugins-message-container'>
          <div className='fv-help-block'>{formik.errors[props.name]}</div>
        </div>
      ) : null}
    </div>
  )
}

export const MyLgFourField = (props: any) => {
  const { formik } = props
  return (
    <div className='col-lg-4'>
      <label className='form-label fw-bold fs-6'>{props.label}</label>
      {props.isRequired ? <span className='text-danger'>*</span> : null}
      <input
        type='text'
        className='form-control form-control-lg '
        name={props.name}
        value={formik.values[props.name]}
        onChange={formik.handleChange}
        onBlur={formik.handleBlur}
      />
      {formik.touched[props.name] && formik.errors[props.name] ? (
        <div className='fv-plugins-message-container'>
          <div className='fv-help-block'>{formik.errors[props.name]}</div>
        </div>
      ) : null}
    </div>
  )
}

export const MyLgTenField = (props: any) => {
  const { formik } = props
  return (
    <div className='col-lg-10'>
      <label className='form-label fw-bold fs-6'>{props.label}</label>
      {props.isRequired ? <span className='text-danger'>*</span> : null}
      <input
        type='text'
        className='form-control  form-control-lg '
        name={props.name}
        value={formik.values[props.name]}
        onChange={formik.handleChange}
        onBlur={formik.handleBlur}
      />
      {formik.touched[props.name] && formik.errors[props.name] ? (
        <div className='fv-plugins-message-container'>
          <div className='fv-help-block'>{formik.errors[props.name]}</div>
        </div>
      ) : null}
    </div>
  )
}

export const MyTextArea = (props: any) => {
  const { formik } = props
  return (
    <div>
      <label className='form-label fw-bold fs-6'>{props.label}</label>
      {props.isRequired ? <span className='text-danger'>*</span> : null}
      <textarea
        className='form-control form-control-lg '
        name={props.name}
        value={formik.values[props.name]}
        onChange={formik.handleChange}
        onBlur={formik.handleBlur}
        rows={props.rows}
      />
      {formik.touched[props.name] && formik.errors[props.name] ? (
        <div className='fv-plugins-message-container'>
          <div className='fv-help-block'>{formik.errors[props.name]}</div>
        </div>
      ) : null}
    </div>
  )
}

export const FieldText = (props: any) => {
  const { formik } = props
  return (
    <div className='col-lg-6'>
      {props.isRequired ? <span className='text-danger'>*</span> : null}
      <input
        type='text'
        className='form-control  mb-3 mb-lg-0'
        name={props.name}
        value={formik.values[props.name]}
        onChange={formik.handleChange}
        onBlur={formik.handleBlur}
      />
      {formik.touched[props.name] && formik.errors[props.name] ? (
        <div className='fv-plugins-message-container'>
          <div className='fv-help-block'>{formik.errors[props.name]}</div>
        </div>
      ) : null}
    </div>
  )
}

export const ValidationField = (props: any) => {
  const { formik, className, isRequired, type, ...rest } = props
  // check if the type is Date
  // if yes then use the datepicker
  // else use the normal input field

  return (
    <div className={className}>
      <label className='fw-bold fs-6 mb-2'>{props.label}</label>
      {isRequired ? <span className='text-danger'>&nbsp;*</span> : null}
      <input
        {...rest}
        placeholder={props.placeholder}
        {...formik.getFieldProps(props.name)}
        type={type}
        name={props.name}
        className={clsx(
          'form-control  mb-3 mb-lg-0',
          { 'is-invalid': formik.touched[props.name] && formik.errors[props.name] }
          // {
          //     'is-valid': formik.touched[props.name] && !formik.errors[props.name],
          // }
        )}
        autoComplete='off'
        //disabled={formik.isSubmitting}
      />
      {formik.touched[props.name] && formik.errors[props.name] && (
        <div className='fv-plugins-message-container'>
          <div className='fv-help-block'>
            <span role='alert'>{formik.errors[props.name]}</span>
          </div>
        </div>
      )}
    </div>
  )
}

export const ValidationTextArea = (props: any) => {
  const { formik, className, isRequired, name, placeholder, label,rows, ...rest } = props
  return (
    <div className={className}>
      <label className='fw-bold fs-6 mb-2'>{label}</label>
      {isRequired ? <span className='text-danger'>&nbsp;*</span> : null}

      <textarea
        {...rest}
        placeholder={placeholder}
        {...formik.getFieldProps(name)}
        name={name}
        rows={rows}

        className={clsx(
          'form-control  mb-3 mb-lg-0',
          { 'is-invalid': formik.touched[name] && formik.errors[name] }
          //{ 'is-valid': formik.touched[name] && !formik.errors[name] },
        )}
        autoComplete='off'
        //disabled={formik.isSubmitting}
      />
      {formik.touched[name] && formik.errors[name] && (
        <div className='fv-plugins-message-container'>
          <div className='fv-help-block'>
            <span role='alert'>{formik.errors[name]}</span>
          </div>
        </div>
      )}
    </div>
  )
}

export const ValidationSelect = (props: any) => {
  const { formik, className, isRequired, name, placeholder, defaultValue,label, text, multiple, ...rest } = props
  return (
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
          // { 'is-valid': formik.touched[name] && !formik.errors[name] },
        )}
        autoComplete='off'
        //disabled={formik.isSubmitting}
      >
        <option value="" hidden>{text}</option>
        {props.options.length > 0 && props.options.map((item: any) => (
          <option key={item.id} value={item.id}>
            {/* {item[`${name}Name`]} */}
            {item.name}
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
  )
}

export const FilterSelect = (props: any) => {
  const { className, name, placeholder, label, ...rest } = props
  return (
    <div className={className}>
      <label className='fw-bold fs-6 mb-2'>{label}</label>
      <select
        {...rest}
        placeholder={placeholder}
        onChange={props.onChange}
        value={props.value}
        name={name}
        className={clsx('form-select  mb-3 mb-lg-0')}
      >
        {props.options.map((item: any) => (
          <option key={item.id} value={item.id}>
            {item.name}
          </option>
        ))}
      </select>
    </div>
  )
}

export const FilterInput = (props: any) => {
  const { className, name, placeholder, label, ...rest } = props
  return (
    <div className={className}>
      <label className='fw-bold fs-6 mb-2'>{label}</label>
      <input
        {...rest}
        placeholder={placeholder}
        onChange={props.onChange}
        value={props.value}
        name={name}
        className={clsx('form-control  mb-3 mb-lg-0')}
      />
    </div>
  )
}
