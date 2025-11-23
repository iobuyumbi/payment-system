import BaseService from "./BaseService";

export default class PaymentBatchService extends BaseService {

  constructor() {
    super();
  }

  getPaymentBatchData = async (data: any): Promise<any[] | null> => {
    const response = await this.post(this.getURL("api/PaymentBatch/Search"), data);

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

 getPaymentBatchPagedData = async (data: any): Promise<any | null> => {
    const response = await this.post(this.getURL("api/PaymentBatch/Search"), data);
debugger
    if (response && this.isSuccessResponse(response)) {
      if (response.data) {
        const data: any = response.data.data;
        if (data && data.result) {
          return data as any;
        }
      }
    }
    return null;
  };
  getPaymentBatchStats = async (): Promise<any | null> => {
    const response = await this.get(this.getURL("api/PaymentBatch/Stats"));

    if (response && this.isSuccessResponse(response)) {
      if (response.data) {
        const data: any = response.data.data;
        if (data) {
          return data as any;
        }
      }
    }
    return null;
  };

  getSingle = async (paymentBatchId: any): Promise<any | null> => {
    const response = await this.get(this.getURL(`api/PaymentBatch/single/${paymentBatchId}`));

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

  postPaymentBatchData = async (data: any): Promise<any | null> => {
    const response = await this.post(this.getURL("api/PaymentBatch/add"), data);

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

  putPaymentBatchData = async (data: any, id: any): Promise<any | null> => {
    const response = await this.put(this.getURL(`api/PaymentBatch/${data.id}`), data);
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

  updatePaymentStage = async (data: any, id: any): Promise<any | null> => {
    const response = await this.put(this.getURL(`api/PaymentBatch/update-stage/${id}`), data);
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

  getHistory = async (id: any): Promise<any | null> => {
    const response = await this.get(this.getURL(`api/PaymentBatch/history/${id}`));
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

  sendEmail = async (paymentBatchId: any): Promise<any | null> => {
    const response = await this.get(this.getURL(`api/PaymentBatch/send-email/${paymentBatchId}`));

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

  // Api Request services
  getApiRequestData = async (data: any): Promise<any | null> => {
    const response = await this.post(this.getURL("api/ApiRequest/GetAll"), data);

    if (response && this.isSuccessResponse(response)) {
      if (response.data) {
        const data: any = response.data.data;
        if (data && data.result) {
          return data.result as any;
        }
      }
    }
    return null;
  };

  getSingleRequestBody = async (id: any): Promise<any | null> => {
    const response = await this.get(this.getURL(`api/ApiRequest/GetSingleRequestBody/${id}`));

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

  getSingleResponseBody = async (id: any): Promise<any | null> => {
    const response = await this.get(this.getURL(`api/ApiRequest/GetSingleResponseBody/${id}`));

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

  postPaymentProcessing = async (searchParams: any): Promise<any | null> => {
    const response = await this.post(this.getURL(`api/PaymentProcessing/multiple/paymentBatch`), searchParams);
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

  postVerifyMobile = async (searchParams: any): Promise<any | null> => {

    const response = await this.post(this.getURL(`api/PaymentProcessing/verifyContacts`), searchParams);
    if (response && this.isSuccessResponse(response)) {
      if (response.data) {
        const data: any = response.data;
        if (data && data.result) {
          return data as any;
        }
      }
    }
    return null;
  };

  payAllAsync = async (searchParams: any): Promise<any | null> => {

    const response = await this.post(this.getURL(`api/PaymentProcessing/payAllAsync`), searchParams);

    if (response && this.isSuccessResponse(response)) {
      if (response.data) {
        const data: any = response.data;
        if (data) {
          return data as any;
        }
      }
    }
    return null;
  };

  payAllBatchAsync = async (searchParams: any): Promise<any | null> => {

    const response = await this.post(this.getURL(`api/PaymentProcessing/payAllBatchAsync`), searchParams);

    if (response && this.isSuccessResponse(response)) {
      if (response.data) {
        const data: any = response.data;
        if (data) {
          return data as any;
        }
      }
    }
    return null;
  };

  listAllContacts = async (): Promise<any | null> => {
    const response = await this.get(this.getURL(`api/PaymentProcessing/verifyContactsSample`),);
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

  verifySampleContacts = async (): Promise<any | null> => {
    const response = await this.get(this.getURL(`api/PaymentProcessing/verifyContactsSample`),);
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

  updatePaymentManually = async (id: any): Promise<any | null> => {
    const response = await this.put(this.getURL(`api/PaymentBatch/process-manually/${id}`));
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

  getActionContext = async (id: any): Promise<any | null> => {
    const response = await this.get(this.getURL(`api/PaymentBatch/${id}/action-context`),);
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

  updateStage = async (data: { action: string; remarks: string; }, id: any): Promise<any | null> => {
    const response = await this.post(
      this.getURL(`api/PaymentBatch/${id}/transition`),
      data);

    if (response && this.isSuccessResponse(response)) {
      if (response.data) {
        const data: any = response.data;
        if (data && data.result) {
          return data.result as any;
        }
      }
    }
    return null;
  }
}