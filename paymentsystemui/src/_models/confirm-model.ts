export interface IConfirmModel {
    title: string;
    btnText: string;
    message: string;
    deleteUrl: string;
    params?: any
}

export interface IStatusChangeModel {
    leadId: number;
    title: string;
    btnText: string;
    options: any
}