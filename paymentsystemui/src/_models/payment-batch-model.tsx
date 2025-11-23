export interface PaymentBatchModel {
    loanBatchIds: any
    projectIds: any,
    batchName: string,
    paymentModule : number,
}

export const paymentBatchInitValues: PaymentBatchModel = {
    batchName: '',
    loanBatchIds: [],
    projectIds: [],
    paymentModule :0
}