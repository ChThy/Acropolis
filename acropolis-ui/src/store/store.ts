import { configureStore } from "@reduxjs/toolkit";
import { resourcesSlice } from "./resource-slice";

export const store = configureStore({
  reducer: {
    resources: resourcesSlice.reducer
  }
});

export type RootState = ReturnType<typeof store.getState>;
export type AppDispatch = typeof store.dispatch;