import { createSlice } from '@reduxjs/toolkit'
import { farmInitValues } from '../../_models/farmer-model';

export const farmerSlice = createSlice({
    name: 'farmers',
    initialState: farmInitValues,
    reducers: {
        addTofarmers: (state, action: any) => {
            Object.assign(state, action.payload);
            return state;
        },
        resetFarmerState: (state) => {
            state = { ...farmInitValues };
            return farmInitValues;
        },
    }
})

export const { addTofarmers, resetFarmerState } = farmerSlice.actions

export default farmerSlice.reducer