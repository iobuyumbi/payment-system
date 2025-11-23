import { useState, useEffect } from 'react';
import UserService from '../../services/UserService';



const userService = new UserService();

type UserArr = {
  id: number;
  name: string;
};

export default function UserDropdown(props: any) {
  const { formik, isRequired, } = props
  const [user, setUser] = useState<UserArr[]>([]);

  const bindUsers = async () => {


    const response = await userService.getUserData();
   
    if (response && response.length > 0) {
      const userData = response.map((users) => ({
        id: users.id,
        name: users.username,
      }));
      userData.unshift({ id: "", name: 'Select ' });
      setUser(userData);
    }
  }


  useEffect(() => {
    bindUsers();
  }, []);

  return (
    < div className="">
      <label className='fs-6 fw-bold mb-3'>{props.label} {isRequired ? <span className='text-danger'>&nbsp;*</span> : null}</label>
      <select
        data-placeholder={'Select '[props.name]}
        name={props.name}
        className='form-select mb-3 mb-lg-0'
        {...formik.getFieldProps(props.name)}
      >

        {user.map((item) => (
          <option key={`${props.name}_${item.id}`} value={item.id}>
            {item.name}
          </option>
        ))}
      </select>
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
