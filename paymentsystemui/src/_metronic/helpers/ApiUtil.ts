
import { AUTH_LOCAL_STORAGE_KEY } from "../../app/modules/auth";
import config from "../../environments/config";
import environments from "../../environments/environments";


export const getAPIBaseUrl = (): string => {
    const runningEnv = config.runningEnv;
    if (runningEnv) {
        const environment = environments.find(
            (x: any) => x.type === runningEnv
        );
        if (environment) {
            return environment.baseAPIUrl;
        }
    }
    return "";
};

export const parseAPIResponse = (service: any, response: any) => {
    if (response && service.isSuccessResponse(response)) {
        if (response.data) {
            const data: any = response.data;
            if (data && data.data) {
                return data.data;
            }
        }
    }
    return null;
}
export const parseAPIResponseAll = (service: any, response: any) => {
    if (response && service.isSuccessResponse(response)) {
        if (response) {
            return response.data;
        }
    }
    return null;
}

export const isProduction = (): any => {
    return config.runningEnv.toLowerCase() === "production";
};

export const isDevOnly = (): any => {
    return config.runningEnv.toLowerCase() === "development";
};

export const isStagingOrDev = (): any => {
    return config.runningEnv.toLowerCase() === "development" || config.runningEnv.toLowerCase() === "staging";
};

export const getRetryDelay = () => {
    return config.retryDelay || 15;
}

export function isAllowed(perm: any) {
    const lsValue = localStorage.getItem(AUTH_LOCAL_STORAGE_KEY);
    if (!lsValue) {
        return false;
    }

    try {
        const aware_user = JSON.parse(lsValue);
        if (aware_user && Array.isArray(aware_user.permissions)) {
            return aware_user.permissions.includes(perm);
        }
    } catch (e) {
        console.error("Error parsing user auth data from local storage", e);
    }
    
    return false;
}

export function getRoleKeyFromRoleName(roles: string[]): 'approver' | 'reviewer' | 'initiator' {
    const role = roles.find(r =>
        r.toLowerCase().startsWith('approver')
        || r.toLowerCase().startsWith('reviewer')
        || r.toLowerCase().startsWith('initiator')
    );

    if (!role) return 'initiator'; // default/fallback

    if (role.toLowerCase().startsWith('approver')) return 'approver';
    if (role.toLowerCase().startsWith('reviewer')) return 'reviewer';
    if (role.toLowerCase().startsWith('initiator')) return 'initiator';

    return 'initiator';
}


export default {
    isDevOnly,
    isStagingOrDev,
    isProduction,
    parseAPIResponseAll,
    parseAPIResponse,
    getAPIBaseUrl,
    getRetryDelay,
    isAllowed
}
