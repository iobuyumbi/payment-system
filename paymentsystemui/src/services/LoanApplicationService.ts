import BaseService from "./BaseService";

export default class LoanApplicationService extends BaseService {


  constructor() {
    super();
  }

  getLoanApplicationData = async (data: any): Promise<any[] | null> => {

    const response = await this.post(
      this.getURL("api/LoanApplication/Search"),
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

  getApplicationStatusLog = async (id: any): Promise<any[] | null> => {

    const response = await this.get(
      this.getURL(`api/LoanApplication/StatusLog/${id}`)
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

  getApplicationDocumentsData = async (id: any): Promise<any[] | null> => {

    const response = await this.get(
      this.getURL(`api/LoanApplication/GetDocument/${id}`)
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

  getFarmerLoanApplications = async (farmerId: any): Promise<any[] | []> => {

    const response = await this.get(
      this.getURL(`api/LoanApplication/farmer/${farmerId}`)
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

  postLoanApplicationData = async (data: any): Promise<any | null> => {

    const response = await this.post(this.getURL("api/LoanApplication/add"),
      data
    );

    if (response && this.isSuccessResponse(response)) {
      if (response.data) {
        const data: any = response.data;
        if (data && data.result) {
          return data.result as any;
        }
      }
    }
    return response.data;
  };

  putLoanApplicationData = async (data: any, id: any): Promise<any | null> => {
    const response = await this.put(this.getURL(`api/LoanApplication/${data.id}`), data);
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

  putLoanApplicationStatusData = async (id: any, data: any,): Promise<any | null> => {

    const response = await this.put(this.getURL(`api/LoanApplication/StatusUpdate/${id}`), data);
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

  getEmiSchedule = async (loanApplicationId: any): Promise<any | null> => {

    const response = await this.get(
      this.getURL(`api/LoanApplication/emi-schedule/${loanApplicationId}`),
    );

    if (response && this.isSuccessResponse(response)) {
      if (response.data) {
        const data: any = response.data;
        if (data && data.result) {
          return data.result as any;
        }
      }
    }
    return null;
  };

  calculateInterest = async (loanApplicationId: any): Promise<any | null> => {

    const response = await this.get(
      this.getURL(`api/LoanApplication/calculate-interest/${loanApplicationId}`),
    );

    if (response && this.isSuccessResponse(response)) {
      if (response.data) {
        const data: any = response.data;
        if (data && data.result) {
          return data.result as any;
        }
      }
    }
    return null;
  };

  getImportSummary = async (batchId: any): Promise<any | null> => {

    const response = await this.get(
      this.getURL(`api/LoanApplication/import-summary/${batchId}`),
    );

    if (response && this.isSuccessResponse(response)) {
      if (response.data) {
        const data: any = response.data;
        if (data && data.result) {
          return data.result as any;
        }
      }
    }
    return null;
  };

  getImportHistory = async (batchId: any): Promise<any | null> => {

    const response = await this.get(
      this.getURL(`api/LoanApplication/import-history/${batchId}`),
    );

    if (response && this.isSuccessResponse(response)) {
      if (response.data) {
        const data: any = response.data;
        if (data && data.result) {
          return data.result as any;
        }
      }
    }
    return null;
  };

getLoanSchedule = async (id: any): Promise<any[] | []> => {

    const response = await this.get(
      this.getURL(`api/LoanApplication/Schedule/${id}`)
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
