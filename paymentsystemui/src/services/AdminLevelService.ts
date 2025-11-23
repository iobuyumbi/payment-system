import BaseService from "./BaseService";

export default class AdminLevelService extends BaseService {

  constructor() {
    super();
  }

  // Level 1 (County/Region)
  getAdminLevel1Data = async (data: any) => {
   
    const response = await this.post(this.getURL(`api/AdminLevel1/Search`),  data );
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

  postAdminLevel1Data = async (data: any): Promise<any | null> => {
    const response = await this.post(this.getURL("api/AdminLevel1"), data);

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



  putAdminLevel1Data = async (data: any, id: any): Promise<any | null> => {
    const response = await this.put(this.getURL(`api/AdminLevel1/${id}`), data);
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

 


  // Level 2 (Sub-County)
  getAdminLevel2Data = async (data: any) => {
    const response = await this.post(this.getURL("api/AdminLevel2/Search"), data);

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
  


  postAdminLevel2Data = async (data: any): Promise<any | null> => {
    const response = await this.post(this.getURL("api/AdminLevel2"), data);

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

  putAdminLevel2Data = async (data: any, id: any): Promise<any | null> => {
    const response = await this.put(this.getURL(`api/AdminLevel2/${data.id}`), data);
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



 // Level 3 (Ward)
 getAdminLevel3Data = async (data: any): Promise<any[]> => {
    const response = await this.post(this.getURL("api/AdminLevel3/Search"), data);

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

  postAdminLevel3Data = async (data:any): Promise<any | null> => {
    const response = await this.post(this.getURL("api/AdminLevel3"), data);

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

  putAdminLevel3Data = async (data: any, id: any): Promise<any | null> => {
    const response = await this.put(this.getURL(`api/AdminLevel3/${data.id}`), data);
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
   // Level 4 (Village)
   getAdminLevel4Data = async (data: any): Promise<any[]> => {
    const response = await this.post(this.getURL("api/AdminLevel4/Search"), data);

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

  postAdminLevel4Data = async (data: any): Promise<any | null> => {
    const response = await this.post(this.getURL("api/AdminLevel4"), data);

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

  putAdminLevel4Data = async (data: any, id: any): Promise<any | null> => {
    const response = await this.put(this.getURL(`api/AdminLevel4/${data.id}`), data);
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

  


