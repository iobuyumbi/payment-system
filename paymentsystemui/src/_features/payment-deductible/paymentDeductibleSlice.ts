import { createSlice } from '@reduxjs/toolkit'
import { paymentDeductibleInitValues } from '../../_models/deductible-model';


export const paymentDeductibleSlice = createSlice({
    name: 'paymentBatch',
    initialState: paymentDeductibleInitValues,
    reducers: {
        addToPaymentDeductible: (state, action: any) => {
            Object.assign(state, action.payload);
            return state;
        },
        resetPaymentDeductibleState: (state) => {
            state = { ...paymentDeductibleInitValues };
            return paymentDeductibleInitValues;
        },
    }
})

export const { addToPaymentDeductible, resetPaymentDeductibleState } = paymentDeductibleSlice.actions

export default paymentDeductibleSlice.reducer