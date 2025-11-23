import {
  combineReducers,
  configureStore,
} from '@reduxjs/toolkit';
import farmerReducer from '../_features/farmers/farmerSlice'
import cooperativeReducer from '../_features/cooperatives/cooperativeSlice'
import projectReducer from '../_features/projects/projectSlice'
import userReducer from '../_features/users/userSlice'
import loanBatchReducer from '../_features/loanbatch/loanBatchSlice'
import loanApplicationReducer from '../_features/loan/loanApplicationSlice'
import loanItemReducer from '../_features/loanitem/loanItemSlice'
import categoryReducer from '../_features/categories/categorySlice'
import paymentBatchReducer from '../_features/payment-batch/paymentBatchSlice'
import storage from 'redux-persist/lib/storage';
import { persistReducer, persistStore, FLUSH, REHYDRATE, PAUSE, PERSIST, PURGE, REGISTER } from 'redux-persist';
import autoMergeLevel2 from 'redux-persist/es/stateReconciler/autoMergeLevel2';

const persistConfig = {
  key: 'root',
  storage,
  stateReconciler: autoMergeLevel2
};

const rootReducer = combineReducers({
  farmers: farmerReducer,
  cooperatives:cooperativeReducer,
  projects:projectReducer,
  users:userReducer,
  loanBatches:loanBatchReducer,
  loanApplications: loanApplicationReducer,
  categories : categoryReducer,
  loanItems: loanItemReducer,
  paymentBatches : paymentBatchReducer
});

const persistedReducer = persistReducer<ReturnType<typeof rootReducer>>(
  persistConfig,
  rootReducer
);

export const store = configureStore({
  reducer: persistedReducer,
  middleware: (getDefaultMiddleware) =>
    getDefaultMiddleware({
      serializableCheck: {
        ignoredActions: [FLUSH, REHYDRATE, PAUSE, PERSIST, PURGE, REGISTER],
      },
    }),
},);

export const persistor = persistStore(store);

export const setupStore = (preloadedState?: Partial<RootState>) => {
  return store;
};

export const createPreloadedState = (
  customState: Partial<RootState>
): Partial<RootState> => {
  return {
    farmers: { ...store.getState().farmers, ...customState.farmers },
    cooperatives: { ...store.getState().cooperatives, ...customState.cooperatives },
    projects: { ...store.getState().projects, ...customState.projects },
    users:{...store.getState().users,...customState.users},
    loanBatches:{...store.getState().loanBatches,...customState.loanBatches},
    categories:{...store.getState().categories,...customState.categories},
    loanItems:{...store.getState().loanItems,...customState.loanItems},
    paymentBatches:{...store.getState().paymentBatches,...customState.paymentBatches},
    };
};
export type AppDispatch = typeof store.dispatch;
export type AppStore = ReturnType<typeof setupStore>;
export type RootState = ReturnType<typeof store.getState>;