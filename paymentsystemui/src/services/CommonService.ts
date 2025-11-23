

/* eslint-disable @typescript-eslint/no-explicit-any */
// Base Imports

// Model Imports

// Other Imports

import BaseService from "./BaseService";

export default class CommonService extends BaseService {

  // eslint-disable-next-line @typescript-eslint/no-useless-constructor
  constructor() {
    super('', true);
  }

  getItemUnits = async (): Promise<any[] | []> => {
    const response = await this.get(
      this.getURL("api/master/item-units")
    );

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

  deleteEntity = async (url: any): Promise<any | null> => {
    try {
      const response = await this.delete(
        this.getURL(url)
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

  getDocumentTypes = async (): Promise<any[] | []> => {
    const response = await this.get(
      this.getURL("api/DocumentType")
    );

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



