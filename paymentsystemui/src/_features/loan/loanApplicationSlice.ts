import { createSlice } from '@reduxjs/toolkit'
import { initLoanApplicationValues } from '../../_models/loan-application-model';

export const loanApplicationSlice = createSlice({
    name: 'loan',
    initialState: initLoanApplicationValues,
    reducers: {
        addToLoanApplication: (state, action: any) => {
            Object.assign(state, action.payload);
            return state;
        },
        resetLoanApplicationState: (state) => {
            state = { ...initLoanApplicationValues };
            return initLoanApplicationValues;
        },
    }
})

export const { addToLoanApplication, resetLoanApplicationState } = loanApplicationSlice.actions

export default loanApplicationSlice.reducer