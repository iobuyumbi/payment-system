/* eslint-disable prefer-const */
/* eslint-disable @typescript-eslint/no-explicit-any */
/* eslint-disable @typescript-eslint/no-unused-vars */

import axios from 'axios';
//import { baseURL } from "./BaseUrl";
import { AUTH_LOCAL_STORAGE_KEY } from "../app/modules/auth/core/AuthHelpers";
import { getAPIBaseUrl } from "../_metronic/helpers/ApiUtil";

const axiosInstance = axios.create();

export function uploadExcel(files: any, module: any, paymentBatchId: any, countryId: any) {
    let token = '';
    const aware_user = JSON.parse(localStorage.getItem(AUTH_LOCAL_STORAGE_KEY) || "");
    if (aware_user) {
        token = aware_user?.api_token;
    }

    const apiUrl = paymentBatchId
        ? `api/ExcelImport/ImportData/${module}/${countryId}/${paymentBatchId}`
        : `api/ExcelImport/ImportData/${module}/${countryId}`;
    const response = axiosInstance.post(apiUrl, files,

        {
            baseURL: getAPIBaseUrl(),
            headers: {
                'Content-Type': 'multipart/form-data',
                'Access-Control-Allow-Origin': '*',
                'Access-Control-Allow-Methods': 'POST, GET, OPTIONS',
                Authorization: 'Bearer ' + token,
            },
        });

    return response;
}

export function getExcelTemplate(downloadUrl: any) {
    let token = '';
    const aware_user = JSON.parse(localStorage.getItem(AUTH_LOCAL_STORAGE_KEY) || "");
    if (aware_user) {
        token = aware_user?.api_token;
    }
    let headers = new Headers();
    const response = axiosInstance.get('excelImport/importData', {
        baseURL: getAPIBaseUrl(),
        headers: {
            'Content-Type': 'multipart/form-data',
            'Access-Control-Allow-Origin': '*',
            'Access-Control-Allow-Methods': 'POST, GET, OPTIONS',
            Authorization: 'Bearer ' + token,
        },
    });

    return response;
}