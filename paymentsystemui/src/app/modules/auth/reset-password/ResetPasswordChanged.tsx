import { Link } from 'react-router-dom';
import { toAbsoluteUrl } from '../../../../_metronic/helpers';


const ResetPasswordChanged = () => {

    return (
        <div className="card w-full text-center">
            <div className="card-body p-10">

                <h3 className="text-lg font-medium text-gray-900 text-center mb-4">
                    Your password is changed
                </h3>
                <div className="fs-4 text-center text-gray-700 my-10">
                    Your password has been successfully updated.
                   
                </div>

                <div className="flex justify-center">
                    <Link
                        to={'/auth/login'}
                        className="btn btn-primary"
                    >
                        Sign in
                    </Link>
                </div>
            </div>
        </div>
    );
};

export { ResetPasswordChanged };
