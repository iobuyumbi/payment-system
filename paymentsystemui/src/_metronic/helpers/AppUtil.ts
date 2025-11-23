// Service Imports
const storageService = new StorageService();

// Other Imports
import config from "../../environments/config";
import StorageService from "../../services/StorageService";
import { StorageKeys } from "./crud-helper/consts";


export const getAccessToken = (): string | null => {
  const bearerToken = JSON.parse(localStorage.getItem(StorageKeys.ACCESS_TOKEN) || '');
  //const bearerToken = storageService.getStringStorage(StorageKeys.ACCESS_TOKEN);
  return bearerToken ? bearerToken.api_token : null;
};

export const getAssetUrl = (path: string): string => {
  //return `${process.env.PUBLIC_URL}${path}`;
  return `${config.subdirectory}${path}`;
};

export const getRoles = (): any[] => {
  const bearerToken = JSON.parse(localStorage.getItem(StorageKeys.ACCESS_TOKEN) || '');
  //const bearerToken = storageService.getStringStorage(StorageKeys.ACCESS_TOKEN);
  return bearerToken ? bearerToken.roles : [];
};

export const selectedEnterpriseImageURL = (imageURL: string) => {
  const name = imageURL.substring(imageURL.lastIndexOf("/") + 1);
  const selectedName = `${name.substring(
    0,
    name.lastIndexOf(".")
  )}-green${name.substring(name.lastIndexOf("."))}`;
  return `/assets/Icons/${selectedName}`;
};

export const unselectedEnterpriseImageURL = (imageURL: string) => {
  const name = imageURL.substring(imageURL.lastIndexOf("/") + 1);
  const selectedName = `${name.substring(
    0,
    name.lastIndexOf("-")
  )}${name.substring(name.lastIndexOf("."))}`;
  return `/assets/Icons/${selectedName}`;
};

const SELECTED_COUNTRY_CODE_KEY = 'selected_country_code'

export const getSelectedCountryCode = (): string | null => {
  if (typeof localStorage === 'undefined') return null
  return localStorage.getItem(SELECTED_COUNTRY_CODE_KEY)
}

export const setSelectedCountryCode = (code: string): void => {
  if (typeof localStorage === 'undefined') return
  localStorage.setItem(SELECTED_COUNTRY_CODE_KEY, code)
}

export const clearSelectedCountryCode = (): void => {
  if (typeof localStorage === 'undefined') return
  localStorage.removeItem(SELECTED_COUNTRY_CODE_KEY)
}
