import BaseService from "./BaseService";

export default class ExcelExportService extends BaseService {
  constructor() {
    super();
  }

  getDeductiblePayments = async (model: any): Promise<any[] | []> => {
    const response = await this.post(this.getURL(`api/ExcelExport/PaymentHistory`), model);
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

  getAllDeductiblePayments = async (model: any): Promise<any| []> => {
    const response = await this.post(this.getURL(`api/ExcelExport/GetAllDeductiblePayments`), model);
    if (response && this.isSuccessResponse(response)) {
      if (response.data) {
        const data: any = response.data.data;
        if (data && data.result) {
          return data.result as any;
        }
      }
    }
    return [];
  };
  
}