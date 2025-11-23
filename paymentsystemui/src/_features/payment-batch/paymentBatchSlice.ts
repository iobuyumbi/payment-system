import { createSlice } from '@reduxjs/toolkit'
import { paymentBatchInitValues } from '../../_models/payment-batch-model';

export const paymentBatchSlice = createSlice({
    name: 'paymentBatch',
    initialState: paymentBatchInitValues,
    reducers: {
        addToPaymentBatch: (state, action: any) => {
            Object.assign(state, action.payload);
            return state;
        },
        resetPaymentBatchState: (state) => {
            state = { ...paymentBatchInitValues };
            return paymentBatchInitValues;
        },
    }
})

export const { addToPaymentBatch, resetPaymentBatchState } = paymentBatchSlice.actions

export default paymentBatchSlice.reducer