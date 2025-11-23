import { useFormik } from 'formik';
import * as Yup from 'yup';
import { useState } from 'react';
import clsx from 'clsx';
import { useNavigate, useSearchParams } from 'react-router-dom';
import AuthService from '../../../../services/AuthService';
import FormErrorAlert from '../../../../_shared/FormErrorAlert/Index';

const authService = new AuthService();

const passwordSchema = Yup.object().shape({
    newPassword: Yup.string()
        .min(6, 'Password must be at least 6 characters')
        .required('New password is required'),
    confirmPassword: Yup.string()
        .oneOf([Yup.ref('newPassword')], 'Passwords must match')
        .required('Please confirm your new password')
});

const ResetPasswordChange = () => {
    const navigate = useNavigate();
    // const { changePassword } = useAuthContext();
    const [searchParams] = useSearchParams();

    // const userId = searchParams.get('u');
    // const token = searchParams.get('t');
    const [errors, setErrors] = useState<any>([]);
    const [loading, setLoading] = useState(false);
    const [hasErrors, setHasErrors] = useState<boolean | undefined>(undefined);
    const [showNewPassword, setShowNewPassword] = useState(false);
    const [showNewPasswordConfirmation, setShowNewPasswordConfirmation] = useState(false);

    const formik = useFormik({
        initialValues: {
            newPassword: '',
            confirmPassword: ''
        },
        validationSchema: passwordSchema,
        onSubmit: async (values, { setStatus, setSubmitting }) => {
            setLoading(true);
            setHasErrors(undefined);

            const token = new URLSearchParams(window.location.search).get('t');
            const userId = new URLSearchParams(window.location.search).get('u');

            if (!token || !userId) {
                setHasErrors(true);
                setStatus('Token and user properties are required');
                setLoading(false);
                setSubmitting(false);
                return;
            }

            try {
                const data = {
                    userId, token,
                    password: values.newPassword,
                    confirmPassword: values.confirmPassword,
                };

                const response = await authService.changePassword(data);
                console.log(response);
                 debugger
                if (response && response.id) {
                    navigate('/auth/reset-password/changed');
                }
                else {
                    setErrors(response.errors || response.Errors);
                }
                setHasErrors(false);
            } catch {
                setHasErrors(true);
                setStatus('Password reset failed. Please try again.');
            } finally {
                setLoading(false);
                setSubmitting(false);
            }
        }
    });

    return (
        <div className="card max-w-[370px] w-full">
            <form
                className="card-body flex flex-col gap-5 p-10"
                onSubmit={formik.handleSubmit}
                noValidate
            >
                <div className='mb-10'>
                    <h3 className="text-lg font-medium text-gray-900">Reset Password</h3>
                    <span className="text-2sm text-gray-700">Enter your new password</span>
                </div>

                {/* {hasErrors && (
                    <div className="mb-4 alert alert-danger">
                        <div className="alert-text">There was an error. Please try again.</div>
                    </div>
                )} */}
                <FormErrorAlert errors={errors} />

                <div className="flex flex-col gap-1 mt-5">
                    <label className="form-label fw-bolder text-gray-900 fs-6">New Password</label>

                    <input
                        type={showNewPassword ? 'text' : 'password'}
                        placeholder="Enter a new password"
                        autoComplete="off"
                        {...formik.getFieldProps('newPassword')}
                        className={clsx(
                            'form-control bg-transparent',
                            { 'is-invalid': formik.touched.newPassword && formik.errors.newPassword },
                            { 'is-valid': formik.touched.newPassword && !formik.errors.newPassword }
                        )}
                    />

                    {formik.touched.newPassword && formik.errors.newPassword && (
                        <span role="alert" className="text-danger text-xs mt-2">
                            {formik.errors.newPassword}
                        </span>
                    )}
                </div>

                <div className="flex flex-col gap-1 mt-5">
                    <label className="form-label fw-bolder text-gray-900 fs-6">Confirm New Password</label>

                    <input
                        type={showNewPasswordConfirmation ? 'text' : 'password'}
                        placeholder="Re-enter a new Password"
                        autoComplete="off"
                        {...formik.getFieldProps('confirmPassword')}
                        className={clsx(
                            'form-control bg-transparent',
                            { 'is-invalid': formik.touched.confirmPassword && formik.errors.confirmPassword },
                            { 'is-valid': formik.touched.confirmPassword && !formik.errors.confirmPassword }
                        )}
                    />


                    {formik.touched.confirmPassword && formik.errors.confirmPassword && (
                        <span role="alert" className="text-danger text-xs mt-2">
                            {formik.errors.confirmPassword}
                        </span>
                    )}
                </div>
                <div className="mt-5">
                    <button
                        type="submit"
                        className="btn btn-primary flex justify-center grow"
                        disabled={loading}
                    >
                        {loading ? 'Please wait...' : 'Submit'}
                    </button>
                </div>

            </form>
        </div>
    );
};

export { ResetPasswordChange };
