export interface masterTermsModel {
    descriptiveName: string;
    interestRateType: string;
    interestRate: number;
    interestApplication: string;
    tenure: number;
    gracePeriod: number;
    hasAdditionalFee : boolean;
    additionalFee: any;

}
export const masterTermsInitValues: masterTermsModel = {

    descriptiveName: "",
    interestRate: 0,
    interestRateType: "",
    interestApplication: "",
    tenure: 0,
    gracePeriod: 0,
    hasAdditionalFee:false,
    additionalFee: [
        {id : '', feeName: '', feeType: '', value: undefined },
    ],
}