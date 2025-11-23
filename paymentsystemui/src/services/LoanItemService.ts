import BaseService from "./BaseService";

export default class LoanItemService extends BaseService {
  constructor() {
    super();
  }

  getLoanItemData = async (data: any): Promise<any[] | null> => {
    const response = await this.post(
      this.getURL("api/LoanItem/Search"),
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
    return null;
  };


  postLoanItemData = async (data: any): Promise<any | null> => {
    const response = await this.post(
      this.getURL("api/LoanItem"),
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

  putLoanItemData = async (data: any, id: any): Promise<any | null> => {
    const response = await this.put(this.getURL(`api/LoanItem/${data.id}`), data);
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

  getMasterLoanItemData = async (data: any): Promise<any[] | null> => {
    const response = await this.post(
      this.getURL("api/MasterLoanItem/Search"),
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
    return null;
  };

  postMasterLoanItemData = async (data: any): Promise<any | null> => {
    const response = await this.post(
      this.getURL("api/MasterLoanItem"),
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

  putMasterLoanItemData = async (data: any, id: any): Promise<any | null> => {
    const response = await this.put(this.getURL(`api/MasterLoanItem/${data.id}`), data);
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
