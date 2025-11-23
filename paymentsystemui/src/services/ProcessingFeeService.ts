import BaseService from "./BaseService";

export default class ProcessingFeeService extends BaseService {
  constructor() {
    super();
  }

  getProcessingFee = async (): Promise<any[] | []> => {
    const response = await this.get(
      this.getURL("api/MasterProcessingFee"),
    );
    if (response && this.isSuccessResponse(response)) {
      if (response.data) {
        const data: any = response.data;
        if (data && data.success) {
          return data.data as any[];
        }
      }
    }
    return [];
  };


  addProcessingFee = async (data: any): Promise<any | null> => {
    const response = await this.post(
      this.getURL("api/MasterProcessingFee"),
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



  updateProcessingFee = async (id: any, data: any): Promise<any | null> => {
 
    const response = await this.put(this.getURL(`api/MasterProcessingFee/${id}`), data);
    
    if (response && this.isSuccessResponse(response)) {
      if (response.data && response.data.result) {
        return response.data.result as any[];
      }
    }
    
    return null;
  };
}
