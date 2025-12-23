/* eslint-disable @typescript-eslint/no-explicit-any */
// Base Imports

// Model Imports

// Other Imports

import BaseService from "./BaseService";

export default class ReportService extends BaseService {

  // eslint-disable-next-line @typescript-eslint/no-useless-constructor
  constructor() {
    super('', true);
  }

  getStats = async (selectedYear: any): Promise<any | null> => {
    try {
      const response = await this.get(
        this.getURL(`api/reports/stats/${selectedYear}`)
      );

      if (response && this.isSuccessResponse(response)) {
        if (response.data) {
          const data: any = response.data;
          if (data && data.result) {
            return data.result as any;
          }
        }
      }
    }
    catch (ex) {
      console.log(ex);
    }
    return null;
  };

  getPaymentStats = async (data: any): Promise<any[] | []> => {
    try {
      const response = await this.post(
        this.getURL(`api/reports/paymentStats`),
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
    }
    catch (ex) {
      console.log(ex);
    }
    return [];
  };

  getPaymentChartData = async (): Promise<any[] | []> => {
    try {
      const response = await this.get(
        this.getURL(`api/reports/payments-chart`)
      );

      if (response && this.isSuccessResponse(response)) {
        if (response.data) {
          const data: any = response.data;
          if (data && data.result) {
            return data.result as any;
          }
        }
      }
    }
    catch (ex) {
      console.log(ex);
    }
    return [];
  };

  getJobExecutionLog = async (): Promise<any[] | []> => {
    const response = await this.get(
      this.getURL("api/reports/job-execution-log")
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


  getPaymentConfirmationStats = async (data: any): Promise<any | []> => {
    try { 
      const response = await this.post(
        this.getURL(`api/reports/GetPaymentConfirmationReport`),
        data
      );

      if (response && this.isSuccessResponse(response)) {
        if (response.data) {
          const data: any = response.data.data;
          if (data && data.result) {
            return data.result as any;
          }
        }
      }
    }
    catch (ex) {
      console.log(ex);
    }
    return [];
  };

  getLoanAccountReports = async (data: any): Promise<any | []> => {
    try { 
      
      const response = await this.post(
        this.getURL(`api/reports/GetLoanAccountReports`),
        data
      );

      if (response && this.isSuccessResponse(response)) {
        if (response.data) {
          const data: any = response.data.data;
          if (data && data.result) {
            return data.result as any;
          }
        }
      }
    }
    catch (ex) {
      console.log(ex);
    }
    return [];
  };

  getLoanBatchReports = async (data: any): Promise<any | []> => {
    try { 
      
      const response = await this.post(
        this.getURL(`api/reports/GetLoanBatchReports`),
        data
      );

      if (response && this.isSuccessResponse(response)) {
        if (response.data) {
          const data: any = response.data.data;
          if (data && data.result) {
            return data.result as any;
          }
        }
      }
    }
    catch (ex) {
      console.log(ex);
    }
    return [];
  };

  getRepaymentMonthlyTrends = async (): Promise<any | []> => {
    try { 
      
      const response = await this.get(
        this.getURL(`api/reports/RepaymentMonthlyTrends`)
        
      );

      if (response && this.isSuccessResponse(response)) {
        if (response.data) {
          const data: any = response.data;
          if (data && data.result) {
            return data.result as any;
          }
        }
      }
    }
    catch (ex) {
      console.log(ex);
    }
    return [];
  };


getApplicationReports = async (props : any): Promise<any | []> => {
    try { 
      
      const response = await this.post(
        this.getURL(`api/reports/GetLoanApplicationReports`),
        props
      );


      if (response && this.isSuccessResponse(response)) {
        if (response.data) {
          const data: any = response.data;
          if (data && data.result) {
            return data.result as any;
          }
        }
      }
    }
    catch (ex) {
      console.log(ex);
    }
    return [];
  };


  getCountryApplicationReports = async (props : any): Promise<any | []> => {
    try { 
      
      const response = await this.post(
        this.getURL(`api/reports/GetCountryLoanApplicationReports`),
        props
      );

      if (response && this.isSuccessResponse(response)) {
        if (response.data) {
          const data: any = response.data;
          if (data && data.result) {
            return data.result as any;
          }
        }
      }
    }
    catch (ex) {
      console.log(ex);
    }
    return [];
  };

 getGlobalLoanPortfolioReports = async (props : any): Promise<any | []> => {
    try { 
      
      const response = await this.post(
        this.getURL(`api/reports/GlobalLoanPortfolioReports`),
        props
      );

      if (response && this.isSuccessResponse(response)) {
        if (response.data) {
          const data: any = response.data;
          if (data && data.result) {
            return data.result as any;
          }
        }
      }
    }
    catch (ex) {
      console.log(ex);
    }
    return [];
  };



 getGlobalLoanApplicationReports = async (props : any): Promise<any | []> => {
    try { 
      
      const response = await this.post(
        this.getURL(`api/reports/GlobalLoanApplicationReports`),
        props
      );

      if (response && this.isSuccessResponse(response)) {
        if (response.data) {
          const data: any = response.data;
          if (data && data.result) {
            return data.result as any;
          }
        }
      }
    }
    catch (ex) {
      console.log(ex);
    }
    return [];
  };


  getDisbursedLoanReports = async (props : any): Promise<any | []> => {
    try { 
      
      const response = await this.post(
        this.getURL(`api/reports/GetDisbursedLoanReports`),
        props
      );


      if (response && this.isSuccessResponse(response)) {
        if (response.data) {
          const data: any = response.data;
          if (data && data.result) {
            return data.result as any;
          }
        }
      }
    }
    catch (ex) {
      console.log(ex);
    }
    return [];
  };


  getPaymentReports = async (props : any): Promise<any | []> => {
    try { 

      const response = await this.post(
        this.getURL(`api/reports/GetPaymentReports`),
        props
      );


      if (response && this.isSuccessResponse(response)) {
        if (response.data) {
          const data: any = response.data;
          if (data && data.result) {
            return data.result as any;
          }
        }
      }
    }
    catch (ex) {
      console.log(ex);
    }
    return [];
  };















}
