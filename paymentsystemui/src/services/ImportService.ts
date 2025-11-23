

/* eslint-disable @typescript-eslint/no-explicit-any */
// Base Imports

// Model Imports

// Other Imports

import BaseService from "./BaseService";
const defaultCountryId = "47E73F3C-3846-4052-960F-1D9A211A8651";

export default class ImportService extends BaseService {

    // eslint-disable-next-line @typescript-eslint/no-useless-constructor
    constructor() {
        super('', true);
    }


    getImportHistory = async (searchTerm: any): Promise<any | null> => {
        try {
            const response = await this.get(
                this.getURL(`api/ExcelImport`)
            );

            if (response && this.isSuccessResponse(response)) {
                if (response.data) {
                    const data: any = response.data;
                    if (data && data.result) {
                        return data.result as any[];
                    }
                }
            }
        }
        catch (ex) {
            console.log(ex);
        }


        return null;
    };

    getImportDetail = async (id: any): Promise<any[] | []> => {
        try {
            const response = await this.get(
                this.getURL(`api/ExcelImport/detail/${id}`)
            );

            if (response && this.isSuccessResponse(response)) {
                if (response.data) {
                    const data: any = response.data;
                    if (data && data.result) {
                        return data.result as any[];
                    }
                }
            }
        }
        catch (ex) {
            console.log(ex);
        }


        return [];
    };


    getExcelImportData = async (data: any): Promise<any[] | null> => {
        const response = await this.post(this.getURL("api/ExcelImport/Search"), data);

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

    getImportSummaryByPaymentBatch = async (paymentBatchId: any): Promise<any[] | []> => {
        var url = `api/ExcelImport/payment-batch/${paymentBatchId}/import-summary`;
        const response = await this.get(this.getURL(url));

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

     getImportDetailsByPaymentBatch = async (paymentBatchId: any): Promise<any[] | []> => {
        var url = `api/ExcelImport/payment-batch/${paymentBatchId}/import-details`;
        const response = await this.get(this.getURL(url));

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
