import BaseService from "./BaseService";

export default class CooperativeService extends BaseService {
  constructor() {
    super();
  }

  getCooperativeData = async (data: any): Promise<any[] | []> => {
    const response = await this.post(
      this.getURL("api/Cooperative/Search"),
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

  postCooperativeData = async (data: any): Promise<any | null> => {

    const response = await this.post(
      this.getURL("api/Cooperative/add"),
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
    return null;
  };

  putCooperativeData = async (data: any,id:any): Promise<any | null> => {

    const response = await this.put(
      this.getURL(`api/Cooperative/${id}`),
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
    return null;
  };
}
