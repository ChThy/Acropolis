import { createSlice } from "@reduxjs/toolkit"
import { Page, Videos } from "../models/resource"
import { fetchPages } from "./actions";

interface PagesState {
  pages: Page[]
  activeFetchRequestId: string | null
}

interface ResourcesState {
  pages: PagesState,
  videos: Videos[]
}

const initalState: ResourcesState = {
  pages: {
    activeFetchRequestId: null,
    pages: []
  },
  videos: []
}

export const resourcesSlice = createSlice({
  name: 'resources',
  initialState: initalState,
  reducers: {

  },
  extraReducers: builder => {
    builder.addCase(fetchPages.pending, (state, action) => {
      state.pages.activeFetchRequestId ??= action.meta.requestId;
    });
    builder.addCase(fetchPages.fulfilled, (state, action) => {
      state.pages.pages = action.payload;
      if (state.pages.activeFetchRequestId === action.meta.requestId) {
        state.pages.activeFetchRequestId = null;
      }
    });
    builder.addCase(fetchPages.rejected, (state, action) => {
      if (state.pages.activeFetchRequestId === action.meta.requestId) {
        state.pages.activeFetchRequestId = null;
      }
    })
  }
});

