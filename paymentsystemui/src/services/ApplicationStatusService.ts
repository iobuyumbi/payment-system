import BaseService from "./BaseService";

export default class ApplicationStatusService extends BaseService {

  constructor() {
    super();
  }

  // Level 1 (County/Region)
  getStatusData = async (data: any) => {
   
    const response = await this.post(this.getURL(`api/ApplicationStatus/Search`),  data );
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

  postStatusData = async (data: any): Promise<any | null> => {
    const response = await this.post(this.getURL("api/ApplicationStatus"), data);

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



  putStatusData = async (data: any, id: any): Promise<any | null> => {
    const response = await this.put(this.getURL(`api/ApplicationStatus/${id}`), data);
    if (response && this.isSuccessResponse(response)) {
      if (response.data) {
        const data: any = response.data;
        if (data && data.result) {
          return data.result as any[];
        }
    return [];
  };
}
  }
}