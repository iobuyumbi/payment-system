import BaseService from "./BaseService";
export default class PermissionService extends BaseService {
  // eslint-disable-next-line @typescript-eslint/no-useless-constructor
  constructor() {
    super("", true);
  }

  saveRolePermission = async (
    roleId: string,
    data: any
  ): Promise<any | null> => {
    try {
      const response = await this.put(
        this.getURL(`api/RolePermissions/save-permissions/${roleId}`),
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
    } catch (ex) {
      console.log(ex);
    }

    return null;
  };

  getRolePermissions = async (roleId: string): Promise<any | null> => {
    try {
      const response = await this.get(
        this.getURL(`api/RolePermissions/permissions?roleId=${roleId}`)
      );

      if (response && this.isSuccessResponse(response)) {
        if (response.data) {
          const data: any = response.data;
          if (data && data.result) {
            return data.result as any[];
          }
        }
      }
    } catch (ex) {
      console.log(ex);
    }

    return null;
  };

  GetPermissions = async (username : any): Promise<any | null> => {
    try {
     
      const response = await this.post(
        this.getURL("api/Account/GetPermissions"), username
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

