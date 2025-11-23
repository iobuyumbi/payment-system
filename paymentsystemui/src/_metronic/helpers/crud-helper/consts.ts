import { profileDetailsInitValues } from "../../../app/modules/accounts/components/settings/SettingsModel"

const QUERIES = {
  USERS_LIST: 'users-list',
}

export {QUERIES}

export const  TableColumns = {
  projects: [
    { label: "", sortLabel: null, sortVisibleFlag: false, sortDir: 0 },
    { label: "Project name", sortLabel: "projectName", sortVisibleFlag: false, sortDir: 0 },
    { label: "Country", info: true, infoData: "This is dummy info", sortLabel: "countrtName", sortVisibleFlag: false, sortDir: 0 },
    { label: "Actions", sortLabel: null, editMultipleVisibleFlag: false, sortVisibleFlag: false, sortDir: 0 },
  ],
  loanbatches:[
    { label: "", sortLabel: null, sortVisibleFlag: false, sortDir: 0 },
    { label: "Name", sortLabel: "projectName", sortVisibleFlag: false, sortDir: 0 },
    { label: "Start/End Date", info: true, infoData: "This is dummy info", sortLabel: "countrtName", sortVisibleFlag: false, sortDir: 0 },
    { label: "Items", info: true, infoData: "This is dummy info", sortLabel: "countrtName", sortVisibleFlag: false, sortDir: 0 },
    { label: "Project Manager", info: true, infoData: "This is dummy info", sortLabel: "countrtName", sortVisibleFlag: false, sortDir: 0 },
    { label: "Supplier", info: true, infoData: "This is dummy info", sortLabel: "countrtName", sortVisibleFlag: false, sortDir: 0 },
    { label: "Actions", sortLabel: null, editMultipleVisibleFlag: false, sortVisibleFlag: false, sortDir: 0 },
  ],
  loans:[
    { label: "", sortLabel: null, sortVisibleFlag: false, sortDir: 0 },
    { label: "Loan name", sortLabel: "projectName", sortVisibleFlag: false, sortDir: 0 },
    { label: "Loan Amount", info: true, infoData: "This is dummy info", sortLabel: "countrtName", sortVisibleFlag: false, sortDir: 0 },
    { label: "Loan Term", info: true, infoData: "This is dummy info", sortLabel: "countrtName", sortVisibleFlag: false, sortDir: 0 },
    { label: "Interest Rate", info: true, infoData: "This is dummy info", sortLabel: "countrtName", sortVisibleFlag: false, sortDir: 0 },
    { label: "Created On", info: true, infoData: "This is dummy info", sortLabel: "countrtName", sortVisibleFlag: false, sortDir: 0 },
    { label: "Actions", sortLabel: null, editMultipleVisibleFlag: false, sortVisibleFlag: false, sortDir: 0 },
  ],
  inputs: [
    { label: "", sortLabel: null, sortVisibleFlag: false, sortDir: 0 },
    { label: "Input name", sortLabel: "inputName", sortVisibleFlag: false, sortDir: 0 },
    { label: "Category", sortLabel: "categoryName", sortVisibleFlag: false, sortDir: 0 },
    { label: "Actions", sortLabel: null, editMultipleVisibleFlag: false, sortVisibleFlag: false, sortDir: 0 },
  ],
  farmers: [
    { label: "", sortLabel: null, sortVisibleFlag: false, sortDir: 0 },
    { label: "Name", sortLabel: "farmerName", sortVisibleFlag: false, sortDir: 0 },
    { label: "Location", sortLabel: "location", sortVisibleFlag: false, sortDir: 0 },
    { label: "Mobile", sortLabel: "mobile", sortVisibleFlag: false, sortDir: 0 },
    { label: "Age", sortLabel: "age", sortVisibleFlag: false, sortDir: 0 },
    { label: "Farming since", sortLabel: "farmingSince", sortVisibleFlag: false, sortDir: 0 },
    { label: "Actions", sortLabel: null, editMultipleVisibleFlag: false, sortVisibleFlag: false, sortDir: 0 },
  ],
  users: [
    { label: "", sortLabel: null, sortVisibleFlag: false, sortDir: 0 },
    { label: "Name", sortLabel: "name", sortVisibleFlag: false, sortDir: 0 },
    { label: "Username", sortLabel: "username", sortVisibleFlag: false, sortDir: 0 },
    { label: "Role", sortLabel: "role", sortVisibleFlag: false, sortDir: 0 },
    { label: "Project", sortLabel: "project", sortVisibleFlag: false, sortDir: 0 },
    { label: "Country", sortLabel: "country", sortVisibleFlag: false, sortDir: 0 },
    { label: "Actions", sortLabel: null, editMultipleVisibleFlag: false, sortVisibleFlag: false, sortDir: 0 },
  ]
};


export enum StorageKeys {
  USER = "@@storageKeys/USER",
  ACCESS_TOKEN = "kt-auth-react-v",
  COOKIES_ACCEPTED = "@@storageKeys/COOKIES_ACCEPTED",
}

export enum ErrorType {
  SUCCESS = "Success",
  INFO = "Info",
  WARNING = "Warning",
  ERROR = "Error",
  // DELETE = "Delete",
}

export const PAGE_COUNT_ARR = [
  { value: "5", label: "5" },
  { value: "10", label: "10" },
  { value: "20", label: "20" },
  { value: "50", label: "50" },
  { value: "100", label: "100" },
];

export const statusTextMap:
  {
    [key: string]: string
  } =
{
  "Approved": "success",
  "Initiated": "dark",
  "Under Review": "primary",
  "Review Rejected": "danger",
  "Pending Approval": "warning",
  "Rejected": "danger",
};
