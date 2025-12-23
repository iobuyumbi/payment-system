/* eslint-disable no-var */
/* eslint-disable @typescript-eslint/no-unused-vars */

import { useState } from 'react'
import * as Yup from 'yup'
import clsx from 'clsx'
import { Link, useNavigate } from 'react-router-dom'
import { useFormik } from 'formik'
// import {getUserByToken, login} from '../core/_requests'
import { toAbsoluteUrl } from '../../../../_metronic/helpers'
import { useAuth } from '../core/Auth'
import AuthService from '../../../../services/AuthService';
import { getUserByToken, login } from '../core/_requests'
import { AUTH_LOCAL_STORAGE_KEY } from '../core/AuthHelpers'
import PermissionService from '../../../../services/PermissionService'
import { setSelectedCountryCode } from '../../../../_metronic/helpers/AppUtil'
import FormErrorAlert from '../../../../_shared/FormErrorAlert/Index'
import { FaEye, FaEyeSlash } from 'react-icons/fa'

const permissionService = new PermissionService();

const loginSchema = Yup.object().shape({
  username: Yup.string()
    //.email('Wrong email format')
    .min(3, 'Minimum 3 symbols')
    .max(50, 'Maximum 50 symbols')
    .required('Username is required'),
  password: Yup.string()
    .min(3, 'Minimum 3 symbols')
    .max(50, 'Maximum 50 symbols')
    .required('Password is required'),
})

const initialValues = {
  username: '',
  password: '',
  // username: 'LoanOfficer',
  // password: 'Password123!',
}

/*
  Formik+YUP+Typescript:
  https://jaredpalmer.com/formik/docs/tutorial#getfieldprops
  https://medium.com/@maurice.de.beijer/yup-validation-and-typescript-and-formik-6c342578a20e
*/
const authService = new AuthService();

export function Login() {
  const navigate = useNavigate()
  const [loading, setLoading] = useState(false)
  const { saveAuth, setCurrentUser } = useAuth()
  const [errors, setErrors] = useState<any>([]);
  const [otp, setOtp] = useState('');
  const [userFound, setUserFound] = useState(false);
  const [userData, setUserData] = useState<any>(null);

  const formik = useFormik({
    initialValues,
    validationSchema: loginSchema,
    onSubmit: async (values, { setStatus, setSubmitting }) => {
      setLoading(true)
      setErrors([]);
      try {
        const result = await authService.authenticate(values);
        console.log(result)
        if (result.userId) {
          setUserFound(true);
          // const {data: auth} = await login(values.username, values.password)
          //const {data: auth} = result;
          setUserData(result);
         
        } else {
          setLoading(false)
          setSubmitting(false)
          saveAuth(undefined)
          setErrors(result.errors || result.Errors)
        }

      } catch (error: any) {
        console.error(error.response)
        saveAuth(undefined)
        //setStatus('The login details are incorrect')
        setSubmitting(false)
        setLoading(false)
      }

      finally {
        setLoading(false);
      }
    },
  })

  const updatePermissionsForCountry = async () => {
    setLoading(true); // show loader or message

    const stored = localStorage.getItem(AUTH_LOCAL_STORAGE_KEY);

    if (stored) {
      try {
        const parsed = JSON.parse(stored);

        let per = await permissionService.GetPermissions(parsed.username);

        parsed.permissions = per;
        localStorage.setItem(AUTH_LOCAL_STORAGE_KEY, JSON.stringify(parsed));

        setLoading(false); // hide loader before reload
        window.location.reload();
      } catch (err) {
        setLoading(false);
        console.error("Failed to update permissions in localStorage:", err);
        alert("Permission update failed. Please try again.");
      }
    } else {
      setLoading(false);
    }
  };


  const handleOtpChange = async () => {
    var data = {
      otp: otp,
      userId: userData?.userId ?? userData?.id
    }
    setLoading(true)
    setErrors([])
    try {
      let result = await authService.postOTP(data);
      console.log('[OTP] verify-otp response:', result);
      const succeeded = (result as any)?.succeeded ?? (result as any)?.Succeeded;
      const otpResult = (result as any)?.result ?? (result as any)?.Result;
      const isValid = Boolean(succeeded) && otpResult === true;
      
      if (isValid) {
        // Save auth data first
        saveAuth(userData) 
        
        // Set the selected country code from the login response
        if (userData && userData.countries && userData.countries.length > 0) {
          const firstCountryCode = userData.countries[0].code;
          setSelectedCountryCode(firstCountryCode);
        }
        
        // Try to get user data with permissions, but don't fail if it errors
        try {
          const { data: user } = await getUserByToken(userData.api_token)
          if (user) {
            setCurrentUser(user)
            // Update auth with permissions if available
            const stored = localStorage.getItem(AUTH_LOCAL_STORAGE_KEY);
            if (stored) {
              const parsed = JSON.parse(stored);
              const permissions = (user as any).permissions;
              if (permissions) {
                parsed.permissions = permissions;
                localStorage.setItem(AUTH_LOCAL_STORAGE_KEY, JSON.stringify(parsed));
              }
            }
          }
        } catch (tokenError: any) {
          // Continue - we'll get permissions separately
        }
        
        // Get permissions separately if not already set
        try {
          const stored = localStorage.getItem(AUTH_LOCAL_STORAGE_KEY);
          if (stored) {
            const parsed = JSON.parse(stored);
            if (!parsed.permissions || parsed.permissions.length === 0) {
              let per = await permissionService.GetPermissions(parsed.username || userData.username);
              if (per && Array.isArray(per)) {
                parsed.permissions = per;
                localStorage.setItem(AUTH_LOCAL_STORAGE_KEY, JSON.stringify(parsed));
              }
            }
          }
        } catch (permError) {
          // Continue anyway - user can still login
        }
        
        // Navigate to dashboard instead of reloading
        navigate('/');
      } else {
        setErrors(result?.errors || result?.Errors || ["Invalid OTP"]);
        setLoading(false);
      }
    } catch (error: any) {
      console.error("OTP verification failed:", error);
      setErrors(error.response?.data?.Errors || ["OTP verification failed."]);
      setLoading(false);
    }
  };

  const [showPassword, setShowPassword] = useState(false);

  const togglePasswordVisibility = () => {
    setShowPassword(prev => !prev);
  };

  return (
    <form
      className='form w-100'
      onSubmit={formik.handleSubmit}
      noValidate
      id='kt_login_signin_form'
    >
      {/* begin::Heading */}
      <div className='text-center mb-11'>
        <h1 className='text-gray-900 fw-bolder mb-3'>Sign In</h1>
        <div className='text-gray-500 fw-semibold fs-6'>Solidaridad portal</div>
      </div>
      {/* begin::Heading */}

      {formik.status ? (
        <div className='mb-lg-15 alert alert-danger'>
          <div className='alert-text font-weight-bold'>{formik.status}</div>
        </div>
      ) : (<>
      </>
        // <div className='mb-10 bg-light p-8 rounded'>
        //   <div className='text-theme'>
        //     Use account <strong>admin@demo.com</strong> and password <strong>demo</strong> to
        //     continue.
        //   </div>
        // </div>
      )}
      <FormErrorAlert errors={errors} />

      
    {!userFound && ( <><div className='fv-row mb-8'>
        <label className='form-label fs-6 fw-bolder text-gray-900'>Username</label>
        <input
          placeholder='username'
          {...formik.getFieldProps('username')}
          className={clsx(
            'form-control bg-transparent',
            { 'is-invalid': formik.touched.username && formik.errors.username },
            {
              'is-valid': formik.touched.username && !formik.errors.username,
            }
          )}
          type='text'
          name='username'
          autoComplete='off'
        />
        {formik.touched.username && formik.errors.username && (
          <div className='fv-plugins-message-container'>
            <span role='alert'>{formik.errors.username}</span>
          </div>
        )}
      </div>
      

     
      <div className='fv-row mb-3'>
        <label className='form-label fw-bolder text-gray-900 fs-6 mb-0'>Password</label>
        <div className="position-relative">
          <input
            type={showPassword ? 'text' : 'password'}
            autoComplete='off'
            {...formik.getFieldProps('password')}
            className={clsx(
              'form-control bg-transparent',
              {
                'is-invalid': formik.touched.password && formik.errors.password,
              },
              {
                'is-valid': formik.touched.password && !formik.errors.password,
              }
            )}
          />
          <span
            onClick={togglePasswordVisibility}
            className="position-absolute"
            style={{
              top: '50%',
              right: '10px',
              transform: 'translateY(-50%)',
              cursor: 'pointer',
              zIndex: 2,
            }}
          >
            {showPassword ? <FaEyeSlash /> : <FaEye />}
          </span>
        </div>
        
        {formik.touched.password && formik.errors.password && (
          <div className='fv-plugins-message-container'>
            <div className='fv-help-block'>
              <span role='alert'>{formik.errors.password}</span>
            </div>
          </div>
        )}
      </div>
      {/* end::Form group */}

      {/* begin::Wrapper */}
      <div className='d-flex flex-stack flex-wrap gap-3 fs-base fw-semibold mb-8'>
        <div />

        {/* begin::Link */}
        <Link to='/auth/forgot-password' className='link-primary'>
          Forgot Password ?
        </Link>
        {/* end::Link */}
      </div>
      {/* end::Wrapper */}

      {/* begin::Action */}
      <div className='d-grid mb-10'>
        <button
          type='submit'
          id='kt_sign_in_submit'
          className='btn btn-theme'
          disabled={formik.isSubmitting}
        >
          {!loading && <span className='indicator-label'>Continue</span>}
          {loading && (
            <span className='indicator-progress' style={{ display: 'block' }}>
              Please wait...
              <span className='spinner-border spinner-border-sm align-middle ms-2'></span>
            </span>
          )}
        </button>
      </div></>)}
      {/* end::Action */}

      {/* <div className='text-gray-500 text-center fw-semibold fs-6'>
        Not a Member yet?{' '}
        <Link to='/auth/registration' className='link-primary'>
          Sign up
        </Link>
      </div> */}

      {userFound && <div className='fv-row mb-8'>
        <label className='form-label fs-6 fw-bolder text-gray-900'>OTP</label>
        <label className='form-label fs-6 text-gray-900'>An OTP was sent to the registered email, please enter the OTP to continue</label>
        <input
          placeholder='OTP'
          value={otp}
          onChange={e => setOtp(e.target.value)}
          className={clsx(
            'form-control bg-transparent mb-5'
          )}
          type='text'
          name='otp'
          autoComplete='off'
        />

        <div className='d-grid mb-10'>
          
         <button
          type='button'
          id='kt_otp_submit'
          className='btn btn-theme'
          onClick={() => handleOtpChange()}
        >
          {!loading && <span className='indicator-label'>Submit</span>}
          {loading && (
            <span className='indicator-progress' style={{ display: 'block' }}>
              Please wait...
              <span className='spinner-border spinner-border-sm align-middle ms-2'></span>
            </span>
          )}
        </button>
        </div>
        {/* {formik.touched.otp && formik.errors.otp && (
          <div className='fv-plugins-message-container'>
            <span role='alert'>{formik.errors.otp}</span>
          </div>
        )} */}
      </div>}


      
    </form>
  )
}
