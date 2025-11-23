import axios from 'axios';
import { baseURL } from "./BaseUrl";
import { baseUrlMobile } from './BaseUrl';
import { AUTH_LOCAL_STORAGE_KEY } from "../app/modules/auth/core/AuthHelpers";
var Environment = {
  //mobile or desktop compatible event name, to be used with '.on' function
  TOUCH_DOWN_EVENT_NAME: 'mousedown touchstart',
  TOUCH_UP_EVENT_NAME: 'mouseup touchend',
  TOUCH_MOVE_EVENT_NAME: 'mousemove touchmove',
  TOUCH_DOUBLE_TAB_EVENT_NAME: 'dblclick dbltap',

  isAndroid: function() {
      return navigator.userAgent.match(/Android/i);
  },
  isBlackBerry: function() {
      return navigator.userAgent.match(/BlackBerry/i);
  },
  isIOS: function() {
      return navigator.userAgent.match(/iPhone|iPad|iPod/i);
  },
  isOpera: function() {
      return navigator.userAgent.match(/Opera Mini/i);
  },
  isWindows: function() {
      return navigator.userAgent.match(/IEMobile/i);
  },
  isMobile: function() {
      return (Environment.isAndroid() || Environment.isBlackBerry() || Environment.isIOS() || Environment.isOpera() || Environment.isWindows());
  }
};

let token = '';
const aware_user = JSON.parse(localStorage.AUTH_LOCAL_STORAGE_KEY);
if (aware_user) {
  token = aware_user?.api_token;
}
const apiUrl = Environment.isMobile() ? baseUrlMobile : baseURL;

const axiosInstance = axios.create({
  baseURL:apiUrl ,
  headers: {
    'Content-Type': 'application/json',
    'Access-Control-Allow-Origin': '*',
    'Access-Control-Allow-Methods': 'POST, GET, OPTIONS',
    Authorization: 'Bearer ' + token,
  }
});

// axiosInstance.interceptors.request.use((config) => {
//     // const state = store.getState();
//     // const token = state.auth.auth.idToken;
//     // config.params = config.params || {};
//     // config.params['auth'] = token;
//    if(config.status===401){alert('hi')}
//     return config;
// });

axiosInstance.interceptors.response.use((response) => response, (error) => {
  if (error.response.status === 401) {
    localStorage.removeItem(AUTH_LOCAL_STORAGE_KEY) 
    window.location.reload();
    //window.location = '/auth/login';
  }
});
export default axiosInstance;
