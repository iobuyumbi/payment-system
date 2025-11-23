import { createSlice } from '@reduxjs/toolkit'
import { cooperativeInitValues } from '../../_models/cooperative-model';

export const cooperativeSlice = createSlice({
    name: 'cooperatives',
    initialState: cooperativeInitValues,
    reducers: {
        addToCooperatives: (state, action: any) => {
            Object.assign(state, action.payload);
            return state;
        },
        resetCooperativeState: (state) => {
            state = { ...cooperativeInitValues };
            return cooperativeInitValues;
        },
    }
})

export const { addToCooperatives, resetCooperativeState } = cooperativeSlice.actions

export default cooperativeSlice.reducer