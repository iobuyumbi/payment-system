import BaseService from "./BaseService";

export default class AssociateService extends BaseService {
  constructor() {
    super();
  }

  getAssociateData = async (data: any): Promise<any[] | []> => {
    const response = await this.post(
      this.getURL("api/Associate/Search"),
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

  postAssociateData = async (data: any): Promise<any | null> => {

    const response = await this.post(
      this.getURL("api/Associate/addRange"),
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

  putAssociateData = async (data: any,id:any): Promise<any | null> => {

    const response = await this.put(
      this.getURL(`api/Associate/${id}`),
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