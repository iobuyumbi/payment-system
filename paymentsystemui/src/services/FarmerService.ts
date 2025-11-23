import BaseService from "./BaseService";

export default class FarmerService extends BaseService {
  constructor() {
    super();
  }

  getFarmerData = async (data: any): Promise<any | null> => {
    const response = await this.post(this.getURL("api/Farmer/Search"), data);

    if (response && this.isSuccessResponse(response)) {
      if (response.data) {
        const data: any = response.data.data;
        if (data && data.result) {
          return data.result as any[];
        }
      }
    }
    return null;
  };

  getFarmerPagedData = async (data: any): Promise<any | null> => {
    const response = await this.post(this.getURL("api/Farmer/Search"), data);
    if (response && this.isSuccessResponse(response)) {
      if (response.data) {
        const data: any = response.data;
        if (data && data.data.result) {
          return data.data as any;
        }
      }
    }
    return null;
  };

  postFarmerData = async (data: any): Promise<any | null> => {
    const response = await this.post(this.getURL("api/Farmer/add"), data);

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

  putFarmerData = async (data: any, id: any): Promise<any | null> => {
    const response = await this.put(this.getURL(`api/Farmer/${data.id}`), data);
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

  getFarmerCooperatives = async (id: any): Promise<any[] | []> => {
    const response = await this.get(this.getURL(`api/Farmer/cooperatives/${id}`));
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

  getFarmerProjects = async (id: any): Promise<any[] | []> => {
    const response = await this.get(this.getURL(`api/Farmer/projects/${id}`));
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
}
