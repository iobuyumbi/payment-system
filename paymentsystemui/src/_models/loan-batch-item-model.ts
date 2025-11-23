import { OptionType } from "./option-type";

export interface CreateLoanBatchItemModel {
    loanBatch: OptionType;
    loanItem: OptionType;
    supplierDetails: string;
    quantity: number;
    unit: OptionType;
    unitPrice: number | null;
    isFree : boolean;
}

export const loanBatchItemInit: CreateLoanBatchItemModel = {
    loanBatch: {
        value: "",
        label: ""
    },
    loanItem: {
        value: "",
        label: ""
    },
    supplierDetails: "",
    quantity: 1,
    isFree:false,
    unit: {
        value: "",
        label: ""
    },
    unitPrice: null
}