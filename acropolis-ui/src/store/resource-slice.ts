import { createSlice } from "@reduxjs/toolkit"
import { Page, PendingResource, Video } from "../models/resource"
import { fetchPages, fetchPendingPages, fetchPendingVideos, fetchVideos } from "./thunks";

interface PagesState {
  pages: Page[];
  pendingPages: PendingResource[];
  activeFetchRequestId: string | null;
}

interface VideoState {
  videos: Video[];
  pendingVideos: PendingResource[];
  activeFetchRequestId: string | null;
}

interface ResourcesState {
  pages: PagesState,
  videos: VideoState
}

const initalState: ResourcesState = {
  pages: {
    activeFetchRequestId: null,
    pages: [],
    pendingPages: []
  },
  videos: {
    activeFetchRequestId: null,
    videos: [],
    pendingVideos: []
  }
}

export const resourcesSlice = createSlice({
  name: 'resources',
  initialState: initalState,
  reducers: {

  },
  extraReducers: builder => {
    builder.addCase(fetchPendingVideos.fulfilled, (state, action) => {
      state.videos.pendingVideos = action.payload
    })
    .addCase(fetchPendingPages.fulfilled, (state, action) => {
      state.pages.pendingPages = action.payload;
    })

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

