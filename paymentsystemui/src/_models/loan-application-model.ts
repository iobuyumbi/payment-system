export interface LoanApplicationModel {
    farmer: string; 
    witnessFullName?: string; 
    witnessNationalId?: string; 
    witnessPhoneNo?: string; 
    witnessRelation?: string; 
    dateOfWitness?: any; 
    enumeratorFullName?: string; 
    loanBatchId: string;
    principalAmount: number;
    officerId: string; // Added officerId to track the assigned loan officer
}

export const initLoanApplicationValues: LoanApplicationModel = {
    farmer: "",
    witnessFullName: "",
    witnessNationalId: "",
    witnessPhoneNo: "",
    witnessRelation: "",
    dateOfWitness:new Date().toISOString(),
    enumeratorFullName:"", 
    loanBatchId:"",
    principalAmount:0,
    officerId: "" // Initialize officerId
   
}

export interface Seedling {
    name: string;
    quantity: number;
    pricePerSeedling: number;
}