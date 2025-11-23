/* eslint-disable @typescript-eslint/no-explicit-any */
// Base Imports

// Model Imports

// Other Imports

import BaseService from "./BaseService";

export default class EmailService extends BaseService {

  // eslint-disable-next-line @typescript-eslint/no-useless-constructor
  constructor() {
    super('', true);
  }

  saveEmailTemplate = async (data: any): Promise<any | null> => {
    try {
      const response = await this.post(
        this.getURL("api/EmailTemplates"),
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
    }
    catch (ex) {
      console.log(ex);
    }


    return null;
  };

  updateEmailTemplate = async (id: any, data: any): Promise<any | null> => {
    try {
      const response = await this.put(
        this.getURL(`api/EmailTemplates/${id}`),
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
    }
    catch (ex) {
      console.log(ex);
    }


    return null;
  };

  getTemplates = async (searchTerm: string): Promise<any[] | []> => {
    try {
      const response = await this.get(
        this.getURL(`api/EmailTemplates?keyword=${searchTerm}`)
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
    return [];
  };

  getTemplateByName = async (name: string): Promise<any | null> => {
    try {
      const response = await this.get(
        this.getURL(`api/EmailTemplates/${name}`)
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
}
