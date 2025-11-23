import { createSlice } from '@reduxjs/toolkit'
import { userInitValues } from '../../_models/user-model';

export const userSlice = createSlice({
    name: 'users',
    initialState: userInitValues,
    reducers: {
        addToUsers: (state, action: any) => {
            Object.assign(state, action.payload);
            return state;
        },
        resetUserState: (state) => {
            state = { ...userInitValues };
            return userInitValues;
        },
    }
})

export const { addToUsers, resetUserState } = userSlice.actions

export default userSlice.reducer