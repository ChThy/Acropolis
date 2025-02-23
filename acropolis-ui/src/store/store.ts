import { configureStore } from "@reduxjs/toolkit";
import { IResourcesStoreState, resourcesSlice } from "./resource-slice";

interface ExpectedRootState extends IResourcesStoreState {
  
}

export const store = configureStore<ExpectedRootState>({
  reducer: {
    resources: resourcesSlice.reducer,
  } 
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;
