import { useState, useEffect } from 'react';

import CountryService from '../../services/CountryService';

const countryService = new CountryService();

type CountryArr = {
  id: number;
  name: string;
};

export default function CountryDropdown(props: any) {
  const { formik, isRequired, isSelected, disabled } = props
  const [country, setCountry] = useState<CountryArr[]>([]);

  const bindCountries = async () => {
    const data = {
      pageNumber: 1,
      pageSize: 10,
    };
    var response = isSelected === true ? await countryService.getSelectedCountryData(true) : await countryService.getCountryData(true);

    if (Array.isArray(response)) {

      var result = response.map(item => ({
        id: item.id,
        name: item.countryName
      }));

      result.unshift({ id: '0', name: 'Select country' });
    } else {
      result = [{ id: '0', name: 'Select country' }];
    }

    // Assuming setProjects is a function that sets state or performs some action
    setCountry(result);
  };

  useEffect(() => {
    bindCountries();
  }, []);

  return (
    < div className="">
      <label className='fs-6 fw-bold mb-3'>Country {isRequired ? <span className='text-danger'>&nbsp;*</span> : null}</label>
      <select
        data-placeholder='Select country'
        name='countryId'
        className='form-select mb-3 mb-lg-0'
        disabled={disabled}
        {...formik.getFieldProps('countryId')}
      >

        {country.map((item) => (
          <option key={item.id} value={item.id}>
            {item.name}
          </option>
        ))}
      </select>
      {formik.touched['countryId'] && formik.errors['countryId'] && (
        <div className='fv-plugins-message-container'>
          <div className='fv-help-block'>
            <span role='alert'>{formik.errors['countryId']}</span>
          </div>
        </div>
      )}
    </div>
  )
}
