export interface loanBatchModel {
    name: string;
    initiatedBy: string;
    initiationDate: Date | null;
    projectId: string;
    statusId: number | null;
    interestRate: number;
    rateType: string;
    calculationTimeframe: string;
    tenure: number;
    gracePeriod: number;
    processingFee: number;
    effectiveDate: Date | null;
    processingFees: any;
    stageText: string;
}
export const loanBatchInitValues: loanBatchModel = {
    name: "",
    initiatedBy: "",
    initiationDate: null,
    projectId: "",
    statusId: null,
    interestRate: 0,
    rateType: "",
    calculationTimeframe: "",
    tenure: 0,
    gracePeriod: 0,
    processingFee: 0,
    effectiveDate: null,
    processingFees: [
        { feeName: '', feeType: '', amount: undefined },
    ],
    stageText: ""
}