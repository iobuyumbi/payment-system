export interface LoanItemModel {
    itemName: string;
    description: string;
    categoryId: string;
    cost: number | null;
}

export const loanItemInitValues: LoanItemModel = {
    itemName: "",
    description: "",
    categoryId: "",
    cost: null,
}