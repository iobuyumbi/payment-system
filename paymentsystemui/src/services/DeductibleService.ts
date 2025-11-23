
import BaseService from "./BaseService";

export default class DeductiblePaymentService extends BaseService {
  constructor() {
    super();
  }

  getSingleDeductiblePayment = async (id: any): Promise<any[] | null> => {
    const response = await this.get(this.getURL(`api/PaymentDeductible/single/${id}`));

    if (response && this.isSuccessResponse(response)) {
      if (response.data) {
        const data: any = response.data.data;
        if (data ) {
          return data as any[];
        }
      }
    }
    return null;
  };
  getDeductiblePaymentReport = async (): Promise<any[] | null> => {
    const response = await this.get(this.getURL(`api/PaymentDeductible/report`));

    if (response && this.isSuccessResponse(response)) {
      if (response.data) {
        const data: any = response.data.data;
        if (data ) {
          return data as any[];
        }
      }
    }
    return null;
  };
  postDeductiblePayments = async (data: any): Promise<any | null> => {
    const response = await this.post(this.getURL("api/PaymentDeductible/"), data);

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

  putDeductiblePayments = async (data: any, id: any): Promise<any | null> => {
    const response = await this.put(this.getURL(`api/PaymentDeductible/${data.id}`), data);
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

  getChartData = async (): Promise<any[] | null> => {
    const response = await this.get(this.getURL(`api/PaymentDeductible/GetPaymentSummary`));

    if (response && this.isSuccessResponse(response)) {
   
      if (response.data) {
        const data: any = response.data.result;
        if (data ) {
          return data as any[];
        }
      }
    }
    return null;
  };
  
}