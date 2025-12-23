import axios from "axios";
import { AuthModel, UserModel } from "./_models";
import { getAPIBaseUrl } from "../../../../_metronic/helpers/ApiUtil";
import { getSelectedCountryCode, setSelectedCountryCode } from "../../../../_metronic/helpers/AppUtil";

const API_URL = getAPIBaseUrl(); //import.meta.env.VITE_APP_API_URL;

export const GET_USER_BY_ACCESSTOKEN_URL = `${API_URL}api/token/verify_token`;
export const LOGIN_URL = `${API_URL}/login`;
export const REGISTER_URL = `${API_URL}/register`;
export const REQUEST_PASSWORD_URL = `${API_URL}api/user/ForgotPassword`;

const buildHeaders = () => {
  let code = getSelectedCountryCode();
  if (!code) {
    code = "KE";
    setSelectedCountryCode(code);
  }
  return {
    Accept: "application/json",
    "Content-Type": "application/json",
    "X-Country-Code": code,
  };
};

// Server should return AuthModel
export function login(email: string, password: string) {
  return axios.post<AuthModel>(
    LOGIN_URL,
    {
      email,
      password,
    },
    { headers: buildHeaders() }
  );
}

// Server should return AuthModel
export function register(
  email: string,
  firstname: string,
  lastname: string,
  password: string,
  password_confirmation: string
) {
  return axios.post(
    REGISTER_URL,
    {
      email,
      first_name: firstname,
      last_name: lastname,
      password,
      password_confirmation,
    },
    { headers: buildHeaders() }
  );
}

// Server should return object => { result: boolean } (Is Email in DB)
export function requestPassword(email: string) {
  return axios.post<{ result: boolean }>(
    REQUEST_PASSWORD_URL,
    {
      email,
    },
    { headers: buildHeaders() }
  );
}

export function getUserByToken(token: string) {
  return axios.post<UserModel>(
    GET_USER_BY_ACCESSTOKEN_URL,
    {
      api_token: token,
    },
    { headers: buildHeaders() }
  );
}
