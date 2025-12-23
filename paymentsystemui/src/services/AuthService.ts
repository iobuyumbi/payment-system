/* eslint-disable @typescript-eslint/no-explicit-any */
// Base Imports

// Model Imports

// Other Imports

import BaseService from "./BaseService";

export default class AuthService extends BaseService {

  // eslint-disable-next-line @typescript-eslint/no-useless-constructor
  constructor() {
    super('', false);
  }

  authenticate = async (data: any): Promise<any | null> => {
    try {
      const response = await this.post(
        this.getURL("api/Account/authenticate"),
        data
      );
     
      if (response && this.isSuccessResponse(response)) {
        if (response.data) {
          const data: any = response.data;
          if (data && data.result) {
            return data.result as any[];
          }
        }
      }
      return response.data;
    }
    catch (ex) {
      console.log('ex',ex);
      console.log(ex);
    }


    return null;
  };

  changePassword = async (data: any): Promise<any | null> => {
    try {
      const response = await this.post(
        this.getURL("api/user/ResetPassword"),
        data
      );

      if (response && this.isSuccessResponse(response)) {
        if (response.data) {
          const data: any = response.data;
          if (data && data.result) {
            return data.result as any[];
          }
        }
      }

      return response.data;
    }
    catch (ex: any) {
      return ex.data;
      console.log(ex);
    }


    return null;
  };



   postOTP = async (params: any): Promise<any | null> => {
    try {
      const response = await this.post(
        this.getURL("api/Account/verify-otp"),
        params
      );

      if (response && this.isSuccessResponse(response) && response.data) {
        const data: any = response.data;
        // Return the full ApiResult object so we can check succeeded and result
        return data;
      }
      return response?.data || null;
    } catch (ex: any) {
      console.error('OTP verification error:', ex);
      return { succeeded: false, result: false, errors: ex.response?.data?.Errors || ["OTP verification failed"] };
    }
  };

}
