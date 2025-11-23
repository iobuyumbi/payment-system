import BaseService from "./BaseService";

export default class FacilitationPaymentService extends BaseService {
  constructor() {
    super();
  }

  getFacilitationPayments = async (data: any): Promise<any[] | null> => {
    const response = await this.post(this.getURL("api/Facilitation/Search"), data);

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

  postFacilitationPayments = async (data: any): Promise<any | null> => {
    const response = await this.post(this.getURL("api/Facilitation/add"), data);

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

  putFacilitationPayments = async (data: any, id: any): Promise<any | null> => {
    const response = await this.put(this.getURL(`api/Facilitation/${data.id}`), data);
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