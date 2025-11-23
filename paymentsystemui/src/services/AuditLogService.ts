import BaseService from "./BaseService";

export default class AuditLogService extends BaseService {

    // eslint-disable-next-line @typescript-eslint/no-useless-constructor
    constructor() {
        super('', true);
    }

    getTopAuditLogs = async (exModule : string, limit : number): Promise<any[] | []> => {
        try {
            const response = await this.get(
                this.getURL(`api/AuditLog/top/${exModule}/${limit}`)
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

}
