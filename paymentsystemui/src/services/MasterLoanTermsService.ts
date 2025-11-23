import BaseService from "./BaseService";

export default class MasterLoanTermService extends BaseService {
  constructor() {
    super();
  }

  getMasterLoanTermData = async (data:any): Promise<any[] | null> => {
    const response = await this.post(this.getURL("api/MasterLoanTerm/Search"), data);

    if (response && this.isSuccessResponse(response)) {
      if (response.data) {
        
        const data: any = response.data.data;
        
          return data as any[];
        
      }
    }
    return null;
  };
  
  getSingleMasterLoanTermData = async (id : any): Promise<any[] | null> => {
    const response = await this.get(this.getURL(`api/MasterLoanTerm/${id}`));

    if (response && this.isSuccessResponse(response)) {
      if (response.data) {
        
        const data: any = response.data.result;
        
          return data as any[];
        
      }
    }
    return null;
  };

 
  postMasterLoanTermData = async (data: any): Promise<any | null> => {
    const response = await this.post(this.getURL("api/MasterLoanTerm"), data);
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

  putMasterLoanTermData = async (data: any, id: any): Promise<any | null> => {
    const response = await this.put(this.getURL(`api/MasterLoanTerm/${data.id}`), data);
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
