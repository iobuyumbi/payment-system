import { createSlice } from '@reduxjs/toolkit'
import { categoryInitValues } from '../../_models/category-model';


export const categorySlice = createSlice({
    name: 'categories',
    initialState: categoryInitValues,
    reducers: {
        addToCategories: (state, action: any) => {
            Object.assign(state, action.payload);
            return state;
        },
        resetCategoriesState: (state) => {
            state = { ...categoryInitValues };
            return categoryInitValues;
        },
    }
})

export const { addToCategories, resetCategoriesState } = categorySlice.actions

export default categorySlice.reducer