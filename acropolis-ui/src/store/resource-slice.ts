import { createSlice } from "@reduxjs/toolkit"
import { Page, Videos } from "../models/resource"
import { fetchPages } from "./actions";

interface PagesState {
  pages: Page[]
}

interface ResourcesState {
  pages: PagesState,
  videos: Videos[]
}

const initalState: ResourcesState = {
  pages: {
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
    builder.addCase(fetchPages.fulfilled, (state, action) => {
      state.pages.pages = action.payload;
    });
  }
});

