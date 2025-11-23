import { useState, useEffect } from 'react';
import CooperativeService from '../../services/CooperativeService';

const cooperativeService = new CooperativeService();

type CooperativeArr = {
  id: number;
  name: string;
};

export default function CooperativeDropdown(props: any) {
  const { formik,  isRequired,isDisabled } = props
  const [cooperative, setCooperative] = useState<CooperativeArr[]>([]);

  const bindCooperatives = async () => {
    const data = {
      pageNumber: 1,
      pageSize: 10,
      countryId:formik.values.countryId !==null && formik.values.countryId !==undefined ? formik.values.countryId : ""
    };
    var response = await cooperativeService.getCooperativeData(data); 
       
    // Assuming setProjects is a function that sets state or performs some action
    setCooperative(response);
  };

  useEffect(() => {
    bindCooperatives();
  }, []);
  useEffect(() => {
    bindCooperatives();
  }, [formik.values.countryId]);

  return (
    < div className="">
      <label className='fs-6 fw-bold mb-3'>Cooperatives {isRequired ? <span className='text-danger'>&nbsp;*</span> : null}</label>
      <select
        data-placeholder='Select cooperative'
        name='cooperativeId'
        className='form-select mb-3 mb-lg-0'
        disabled = {isDisabled}
        {...formik.getFieldProps('cooperativeId')}
      >

        {cooperative.map((item) => (
          <option key={item.id} value={item.id}>
            {item.name}
          </option>
        ))}
      </select>
      {formik.touched['cooperativeId'] && formik.errors['cooperativeId'] && (
        <div className='fv-plugins-message-container'>
          <div className='fv-help-block'>
            <span role='alert'>{formik.errors['cooperativeId']}</span>
          </div>
        </div>
      )}
    </div>
  )
}
