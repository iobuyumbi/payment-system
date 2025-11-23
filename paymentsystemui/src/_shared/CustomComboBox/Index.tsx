import Select from 'react-select';
import { OptionType } from '../../_models/option-type';

export default function CustomComboBox(props: any) {
    const { formik, isRequired, label, name, options, isMulti = false,disabled } = props

    return (
        <div className="">
            <label className='fs-6 fw-bold mb-1'>{label} {isRequired ? <span className='text-danger'>&nbsp;*</span> : null}</label>
            <Select
                name="sponsor"
                options={options}
                value={formik.values[name]}
                onChange={(option: OptionType | null) => formik.setFieldValue(name, option)}
                onBlur={formik.handleBlur}
                isMulti={isMulti}
                isDisabled={disabled}
            
            />
            {formik.touched[name] && formik.errors[name] ? (
                <div className='fv-plugins-message-container'>
                    <div className='fv-help-block'>
                        {formik.errors[name].value || formik.errors[name]}
                    </div>
                </div>
            ) : null}
        </div>
    )
}
