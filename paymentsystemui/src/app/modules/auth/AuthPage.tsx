import {Route, Routes} from 'react-router-dom'
import {Registration} from './components/Registration'
import {ForgotPassword} from './components/ForgotPassword'
// import { ResetPassword } from './components/ResetPassword'
import {Login} from './components/Login'
import {AuthLayout} from './AuthLayout'
import { ResetPasswordChange } from './reset-password/ResetPasswordChange'
import { ResetPasswordChanged } from './reset-password/ResetPasswordChanged'

const AuthPage = () => (
  <Routes>
    <Route element={<AuthLayout />}>
      <Route path='login' element={<Login />} />
      <Route path='registration' element={<Registration />} />
      <Route path='forgot-password' element={<ForgotPassword />} />
      <Route path='reset-password/change' element={<ResetPasswordChange />} />
       <Route path='reset-password/changed' element={<ResetPasswordChanged/>} />
      <Route index element={<Login />} />
    </Route>
  </Routes>
)

export {AuthPage}
