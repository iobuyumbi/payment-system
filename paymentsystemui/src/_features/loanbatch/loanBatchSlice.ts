import { createSlice } from '@reduxjs/toolkit'
import { loanBatchInitValues } from '../../_models/loanbatch-model';

export const loanBatchSlice = createSlice({
    name: 'loanBatch',
    initialState: loanBatchInitValues,
    reducers: {
        addToLoanBatch: (state, action: any) => {
            Object.assign(state, action.payload);
            return state;
        },
        resetLoanBatchState: (state) => {
            state = { ...loanBatchInitValues };
            return loanBatchInitValues;
        },
    }
})

export const { addToLoanBatch, resetLoanBatchState } = loanBatchSlice.actions

export default loanBatchSlice.reducer