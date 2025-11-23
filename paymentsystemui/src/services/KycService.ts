import BaseService from "./BaseService";

export default class KycService extends BaseService {
    constructor() {
        super();
    }

    revalidateMobileNumbers = async (): Promise<any | null> => {

        const response = await this.post(this.getURL(`api/PaymentProcessing/revalidateMobileNumbers`));
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
}