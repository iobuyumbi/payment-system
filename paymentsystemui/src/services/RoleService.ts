/* eslint-disable @typescript-eslint/no-explicit-any */
// Base Imports

// Model Imports

// Other Imports

import BaseService from "./BaseService";

export default class RoleService extends BaseService {

  // eslint-disable-next-line @typescript-eslint/no-useless-constructor
  constructor() {
    super('', true);
  }

  saveRole = async (data: any): Promise<any | null> => {
    try {
      const response = await this.post(
        this.getURL("api/Roles"),
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
    }
    catch (ex) {
      console.log(ex);
    }


    return null;
  };

  updateRole = async (id: any, data: any): Promise<any | null> => {
    try {
      const response = await this.put(
        this.getURL(`api/Roles/${id}`),
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
    }
    catch (ex) {
      console.log(ex);
    }


    return null;
  };

  getRoles = async (searchTerm: string): Promise<any | null> => {
    try {
      const response = await this.get(
        this.getURL(`api/Roles?keyword=${searchTerm}`)
      );

      if (response && this.isSuccessResponse(response)) {
        if (response.data) {
          const data: any = response.data;
          if (data && data.result) {
            return data.result as any[];
          }
        }
      }
    }
    catch (ex) {
      console.log(ex);
    }


    return null;
  };

  getSingle = async (id: any): Promise<any | null> => {
    try {
      const response = await this.get(
        this.getURL(`api/Roles/${id}`)
      );

      if (response && this.isSuccessResponse(response)) {
        if (response.data) {
          const data: any = response.data;
          if (data && data.result) {
            return data.result as any;
          }
        }
      }
    }
    catch (ex) {
      console.log(ex);
    }

    return null;
  };

  deleteRole = async (id: any): Promise<any | null> => {
    try {
      const response = await this.delete(
        this.getURL(`api/Roles/${id}`)
      );

      if (response && this.isSuccessResponse(response)) {
        if (response.data) {
          const data: any = response.data;
          if (data && data.result) {
            return data.result as any[];
          }
        }
      }
    }
    catch (ex) {
      console.log(ex);
    }


    return null;
  };

 

  getRolePermissions = async (roleId: string): Promise<any | null> => {
    try {
      const response = await this.get(
        this.getURL(`api/Permissions?roleId=${roleId}`)
      );

      if (response && this.isSuccessResponse(response)) {
        if (response.data) {
          const data: any = response.data;
          if (data && data.result) {
            return data.result as any[];
          }
        }
      }
    }
    catch (ex) {
      console.log(ex);
    }


    return null;
  };
}
