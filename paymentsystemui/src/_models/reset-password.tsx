
export interface ResetModel {
    code: string,
    password: string,
    changepassword: string,
}

export const resetInitValues: ResetModel = {
    code: '',
    password: '',
    changepassword: '',
}