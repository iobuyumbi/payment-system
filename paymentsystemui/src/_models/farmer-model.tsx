export interface FarmerModel {
  firstName: string;
  otherNames: string;
  mobile: string;
  alternateContactNumber: string;
  email: string;
  systemId: string;
  participantId: string;
  enumerationDate: Date | null;
  cooperativeId: string | null;
  hasDisability: boolean;
  accessToMobile: boolean;
  paymentPhoneNumber: string;
  isFarmerPhoneOwner: boolean;
  phoneOwnerName: string;
  phoneOwnerNationalId: string;
  phoneOwnerRelationWithFarmer: string;
  phoneOwnerAddress: string;
  countryId: string;
  adminLevel1Id: string;
  adminLevel2Id: string;
  adminLevel3Id: string;
  village: string;
  gender: number;
  birthMonth: number;
  birthYear: number;
  cooperative: any;
  documentTypeId : any;
  documentType : any;
}

export const farmInitValues: FarmerModel = {
  firstName: "",
  otherNames: "",
  mobile: "",
  alternateContactNumber: "",
  email: "",
  systemId: "",
  participantId: "",
  enumerationDate: null,
  cooperativeId: "",
  hasDisability: false,
  accessToMobile: false,
  paymentPhoneNumber: "",
  isFarmerPhoneOwner: true,
  phoneOwnerName: "",
  phoneOwnerNationalId: "",
  phoneOwnerRelationWithFarmer: "",
  phoneOwnerAddress: "",
  countryId: "",
  gender: 0,
  adminLevel1Id: "",
  adminLevel2Id: "",
  adminLevel3Id: "",
  village: "",
  birthMonth: 0,
  birthYear: 0,
  cooperative: null,
  documentType:null,
  documentTypeId:""
};
