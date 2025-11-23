export interface UserModel {
    countryId: any;
    username: string;
    firstName: string,
    lastName: string,
    email: string,
    phoneNumber: string,
    roleId: string,
    roleName: string,
    country: any,
    project: string,
    projectManagerId: any,
    isLoginEnabled: boolean
}

export const userInitValues: UserModel = {
    firstName: '',
    lastName: '',
    email: '',
    phoneNumber: '',
    roleId: '',
    country: [],
    project: '',
    projectManagerId: null,
    username: "",
    roleName: "",
    countryId: undefined,
    isLoginEnabled: false
}