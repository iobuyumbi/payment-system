import BaseService from "./BaseService";

export default class ProjectService extends BaseService {
  constructor() {
    super();
  }

  getProjectData = async (data: any): Promise<any[] | []> => {
    const response = await this.post(
      this.getURL("api/Project/Search"),
      data
    );
    if (response && this.isSuccessResponse(response)) {
      if (response.data) {
        const data: any = response.data.data;
        if (data && data.result) {
          return data.result as any[];
        }
      }
    }
    return [];
  };

  getProjectByCountryData = async (data: any): Promise<any[] | []> => {
    const response = await this.post(
      this.getURL("api/Project/GetByCountryId"),
      data
    );
    if (response && this.isSuccessResponse(response)) {
      if (response.data) {
        const data: any = response.data.data;
        if (data && data.result) {
          return data.result as any[];
        }
      }
    }
    return [];
  };

  addProject = async (data: any): Promise<any | null> => {
    const response = await this.post(
      this.getURL("api/Project"),
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
  };

  updateProject = async (data: any,id:any): Promise<any | null> => {
    const response = await this.put(
      this.getURL(`api/Project/${id}`),
      data,id
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
  };
}
