import BaseService from "./BaseService";

export default class UserService extends BaseService {


  constructor() {
    super();
  }

  getUserData = async (): Promise<any[]> => {
    const response = await this.get(
      this.getURL("api/User"),
    );

    if (response && this.isSuccessResponse(response)) {
      if (response.data) {
        const data: any = response.data;
        if (data && data.result) {
          return data.result as any[];
        }
      }
    }
    return [];
  };


  postUserData = async (data: any): Promise<any | null> => {
    const response = await this.post(
      this.getURL("api/User"),
      data
    );

    if (response && this.isSuccessResponse(response) && response.data) {
      const data: any = response.data;
      if (data && data.result) {
        return data.result as any[];
      }
    }
    return response.data;
  };

  putUserData = async (data: any, id: any): Promise<any | null> => {
    const response = await this.put(
      this.getURL(`api/User/${id}`),
      data
    );
    if (response && this.isSuccessResponse(response) && response.data) {
      const data: any = response.data;
      if (data && data.result) {
        return data.result as any[];
      }
    }
    return null;
  };

  putUserPassword = async (data: any): Promise<any | null> => {

    const response = await this.put(
      this.getURL(`api/User/changePassword`),
      data
    );
    if (response && this.isSuccessResponse(response) && response.data) {
      const data: any = response.data;
      if (data && data.result) {
        return data.result as any[];
      }
    }
    return response.data;
  };

  getUserCountries = async (id: any): Promise<any[] | []> => {
    const response = await this.get(this.getURL(`api/User/countries/${id}`));
    if (response && this.isSuccessResponse(response)) {
      if (response.data) {
        const data: any = response.data;
        if (data && data.result) {
          return data.result as any[];
        }
      }
    }
    return [];
  };

  getUserRoles = async (id: any): Promise<any[] | []> => {
    const response = await this.get(this.getURL(`api/User/roles/${id}`));
    if (response && this.isSuccessResponse(response)) {
      if (response.data) {
        const data: any = response.data;
        if (data && data.result) {
          return data.result as any[];
        }
      }
    }
    return [];
  };

  getUsersByPermissions = async (permission: string): Promise<any[] | []> => {

    const response = await this.get(this.getURL(`api/User/GetAllAuthorised?permission=${permission}`));

    if (response && this.isSuccessResponse(response)) {
      const data: any = response.data;
      if (data && data.result) {
        return data.result as any[];
      }
    }

    return [];
  };

  getOfficers = async (): Promise<any[] | []> => {

    const response = await this.get(this.getURL(`api/User/GetOfficerList`));

    if (response && this.isSuccessResponse(response)) {
      const data: any = response.data;
      if (data && data.result) {
        return data.result as any[];
      }
    }

    return [];
  };

  getAccessLog = async (data: any): Promise<any | null> => {
    try {

      const response = await this.post(
        this.getURL("api/Account/access-log"),
        data
      );
      console.log(response)
      if (response && this.isSuccessResponse(response) && response.data) {
        const data: any = response.data.data;
        if (data && data.result) {
          return data.result as any;
        }
      }
    }
    catch (ex) {
      console.log(ex);
    }

    return null;
  };







}
