import BaseService from "./BaseService";

export default class LoanBatchService extends BaseService {
  constructor() {
    super();
  }

  getLoanBatchData = async (data: any): Promise<any[] | []> => {
    const response = await this.post(this.getURL("api/LoanBatch/Search"), data);
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

getLoanBatchPagedData = async (data: any): Promise<any | null> => {
    const response = await this.post(this.getURL("api/LoanBatch/Search"), data);
   
    if (response && this.isSuccessResponse(response)) {
      if (response.data) {
        const data: any = response.data.data;
        if (data && data) {
          return data as any;
        }
      }
    }
    return [];
  };

  getSingle = async (loanBatchId: any): Promise<any | null> => {
    const response = await this.get(this.getURL(`api/LoanBatch/single/${loanBatchId}`));

    if (response && this.isSuccessResponse(response)) {
      if (response.data) {
        const data: any = response.data;
        if (data && data.data) {
          return data.data as any;
        }
      }
    }
    return null;
  };

  searchByProjects = async (projectIds: any): Promise<any | []> => {
    const response = await this.post(this.getURL("api/LoanBatch/SearchByProjects"), projectIds);
    if (response && this.isSuccessResponse(response)) {
      if (response.data) {
        const data: any = response.data.data;
        if (data) {
          return data as any;
        }
      }
    }
    return [];
  };

  postLoanBatchData = async (data: any): Promise<any | null> => {
    const response = await this.post(this.getURL("api/LoanBatch"), data);

    if (response && this.isSuccessResponse(response)) {
      if (response.data) {
        const data: any = response.data;

        if (data && data.result) {
          return data.result as any[];
        }
      }
    }
    return response.data;
  };

  putLoanBatchData = async (data: any, id: any): Promise<any | null> => {
    const response = await this.put(this.getURL(`api/LoanBatch/${data.id}`), data);
    if (response && this.isSuccessResponse(response)) {
      if (response.data) {
        const data: any = response.data;
        if (data && data.result) {
          return data.result as any[];
        }
      }
    }
    return response.data;
  };

  saveLoanBatchItem = async (data: any): Promise<any | null> => {
    const response = await this.post(this.getURL("api/LoanBatch/CreateLoanBatchItem"), data);

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

  putLoanBatchItem = async (data: any, id: any): Promise<any | null> => {

    const response = await this.put(this.getURL(`api/LoanBatch/UpdateLoanBatchItem/${id}`), data);

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

  getLoanBatchItems = async (loanBatchId: any): Promise<any[] | null> => {
    const response = await this.get(this.getURL(`api/LoanBatch/GetBatchItems/${loanBatchId}`));

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

  updateLoanBatchStage = async (data: any, id: any, excelImportId: any): Promise<any | null> => {
    const response = await this.put(this.getURL(`api/LoanBatch/update-stage/${id}/${excelImportId}`), data);
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


  getLoanBatchFiles = async (loanBatchId: any): Promise<any[] | null> => {
    const response = await this.get(this.getURL(`api/LoanBatch/GetBatchFiles/${loanBatchId}`));

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

  getValidLoanBatches = async (): Promise<any[]> => {
    const response = await this.get(this.getURL(`api/LoanBatch/valid`));
    if (response && this.isSuccessResponse(response)) {
      if (response.data) {
        const data: any = response.data;
        if (data && data.data) {
          return data.data as any[];
        }
      }
    }
    return [];
  };

}