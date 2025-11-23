// Base Imports

// Model Imports

// Other Imports

import BaseService from "./BaseService";

export default class CountryService extends BaseService {

  // eslint-disable-next-line @typescript-eslint/no-useless-constructor
  constructor() {
    super();
  }

  getCountryData = async (isActive:any): Promise<any[] | null> => {

    const response = await this.get(
      this.getURL("api/Country"),isActive
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

  getSelectedCountryData = async (isActive:any): Promise<any[] | null> => {

    const response = await this.get(
      this.getURL("api/Country/GetSelectedCountry"),isActive
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
