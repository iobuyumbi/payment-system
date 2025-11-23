import BaseService from "./BaseService";

export default class LoanRepaymentService extends BaseService {
  constructor() {
    super();
  }

  getLoanRepaymentHistory = async (loanApplicationId: any): Promise<any[] | []> => {
    const response = await this.get(
      this.getURL(`api/LoanRepayments/${loanApplicationId}`),

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

  saveLoanRepayment = async (data: any): Promise<any | null> => {
    const response = await this.post(
      this.getURL("api/LoanRepayments"),
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

  applyPayment = async (loanApplicationId: any, data: any): Promise<any | null> => {
    const response = await this.put(
      this.getURL(`api/LoanRepayments/apply-payment/${loanApplicationId}`),
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


generateLoanStatement = async (data: any): Promise<any | null> => {
    const response = await this.post(
      this.getURL(`api/LoanRepayments/GenerateStatement/${data}`),
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

  getLoanStatementHistory = async (loanApplicationId: any): Promise<any[] | []> => {
    const response = await this.get(
      this.getURL(`api/LoanRepayments/GetApplicationStatements/${loanApplicationId}`),

    );

    if (response && this.isSuccessResponse(response)) {
      if (response.data) {
        const data: any = response.data.result;
        if (data && data.result) {
          return data.result as any[];
        }
      }
    }
    return [];
  };



}
