import { createSlice } from '@reduxjs/toolkit'
import { projectInitValues } from '../../_models/project-model';

export const projectSlice = createSlice({
    name: 'projects',
    initialState: projectInitValues,
    reducers: {
        addToProjects: (state, action: any) => {
            Object.assign(state, action.payload);
            return state;
        },
        resetProjectState: (state) => {
            state = { ...projectInitValues };
            return projectInitValues;
        },
    }
})

export const { addToProjects, resetProjectState } = projectSlice.actions

export default projectSlice.reducer