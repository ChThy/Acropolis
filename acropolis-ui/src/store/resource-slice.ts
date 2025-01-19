import { createSlice } from "@reduxjs/toolkit"
import { Page, Video } from "../models/resource"
import { fetchPages, fetchVideos } from "./actions";

interface PagesState {
  pages: Page[]
  activeFetchRequestId: string | null
}

interface VideoState {
  videos: Video[]
  activeFetchRequestId: string | null
}

interface ResourcesState {
  pages: PagesState,
  videos: VideoState
}

const initalState: ResourcesState = {
  pages: {
    activeFetchRequestId: null,
    pages: []
  },
  videos: {
    activeFetchRequestId: null,
    videos: []
  }
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
    });
    builder.addCase(fetchVideos.pending, (state, action) => {
      state.videos.activeFetchRequestId ??= action.meta.requestId;
    });
    builder.addCase(fetchVideos.fulfilled, (state, action) => {
      state.videos.videos = action.payload;
      if (state.videos.activeFetchRequestId === action.meta.requestId) {
        state.videos.activeFetchRequestId = null;
      }
    });
    builder.addCase(fetchVideos.rejected, (state, action) => {
      if (state.videos.activeFetchRequestId === action.meta.requestId) {
        state.videos.activeFetchRequestId = null;
      }
    });
  }
});

