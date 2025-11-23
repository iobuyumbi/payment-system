import { Navigate, Outlet } from 'react-router-dom';
import { isAllowed } from '../../_metronic/helpers/ApiUtil';


const ProtectedRoute = ({ permission, redirectPath = "/not-allowed" }: { permission: any, redirectPath: string }) => {
  return !isAllowed(permission) ? <Navigate to={redirectPath} /> : <Outlet />;
};

export default ProtectedRoute;
