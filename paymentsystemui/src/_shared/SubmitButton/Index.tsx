interface ButtonProps {
    loading: boolean;
    className?: string;
    formik: any
    children: React.ReactNode;
}

const SubmitButton = ({ loading, className, formik, children }: ButtonProps) => {
    return (
        <button
            type='submit'
            className={`btn btn-primary ${className}`}
            disabled={formik?.isSubmitting || !formik?.isValid}
        >
            {!loading && <span className='indicator-label'>
                {children}
            </span>}
            {loading && (
                <span className='indicator-progress' style={{ display: 'block' }}>
                    Please wait...
                    <span className='spinner-border spinner-border-sm align-middle ms-2'></span>
                </span>
            )}
        </button>
    )
}

export default SubmitButton