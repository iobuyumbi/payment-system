import BaseService from "./BaseService";

export default class FarmingInputService extends BaseService {

  constructor() {
    super();
  }

  getFarmingInputData = async (data: any): Promise<any[] | []> => {
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
    return [];
  };

  postFarmingInputData = async (data: any): Promise<any | null> => {
    const response = await this.post(
      this.getURL("api/MasterLoanItem"),
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

}
