import { createSlice } from '@reduxjs/toolkit'
import { loanItemInitValues } from '../../_models/loanitem-model';

export const loanItemsSlice = createSlice({
    name: 'loanItems',
    initialState: loanItemInitValues,
    reducers: {
        addToLoanItems: (state, action: any) => {
            Object.assign(state, action.payload);
            return state;
        },
        resetLoanItemsState: (state) => {
            state = { ...loanItemInitValues };
            return loanItemInitValues;
        },
    }
})

export const { addToLoanItems, resetLoanItemsState } = loanItemsSlice.actions

export default loanItemsSlice.reducer