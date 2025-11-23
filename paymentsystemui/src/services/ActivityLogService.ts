

/* eslint-disable @typescript-eslint/no-explicit-any */
// Base Imports

// Model Imports

// Other Imports

import BaseService from "./BaseService";

export default class ActivityLogService extends BaseService {
    // eslint-disable-next-line @typescript-eslint/no-useless-constructor
    constructor() {
        super('', true);
    }

    getActivityLogs = async (): Promise<any | null> => {
        try {
            const response = await this.get(
                this.getURL(`api/ActivityLogs`)
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
}
