import { createAsyncThunk } from "@reduxjs/toolkit";
import { PagesClient, VideosClient } from "../clients/clients";
import axios from "axios";
import { Page, PendingResource, Video } from "../models/resource";
import { RootState } from "./store";
import { activeFetchPagesRequestIdSelector, activeFetchVideosRequestIdSelector } from "./selectors";
import parseUrl from "parse-url";

const baseUrl = 'http://localhost:5092';

const pagesClient = new PagesClient(baseUrl, axios.create());
const videosClient = new VideosClient(baseUrl);

function rejectRequestBusy(thunkApi: any) {
  return thunkApi.rejectWithValue("Still fetching previous request.");
}

export const fetchPages = createAsyncThunk(
  'pages/fetch',
  async (arg, thunkApi) => {
    const activeFetchRequestId = activeFetchPagesRequestIdSelector(thunkApi.getState() as RootState);
    if (activeFetchRequestId && activeFetchRequestId !== thunkApi.requestId) {
      // return thunkApi.rejectWithValue("Still fetching previous request.");
      return rejectRequestBusy(thunkApi)
    }

    const pages = await pagesClient.pages();
    return pages.map<Page>(e => {
      const parsedUrl = parseUrl(e.url!);

      return ({
        type: 'page',
        id: e.id!,
        title: e.metaData?.pageTitle ?? "",
        url: e.url ?? "",
        source: parsedUrl.resource,
        description: parsedUrl.pathname.replace(/[\W_]/g, " ").trim(),
        viewed: false
      })
    });
  }
);

export const fetchVideos = createAsyncThunk(
  'videos/fetch',
  async (_, thunkApi) => {
    const activeFetchRequestId = activeFetchVideosRequestIdSelector(thunkApi.getState() as RootState);
    if (activeFetchRequestId && activeFetchRequestId !== thunkApi.requestId) {
      return rejectRequestBusy(thunkApi)
    }

    const pages = await videosClient.videos();
    return pages.map<Video>(e => {
      const parsedUrl = parseUrl(e.url!);

      return ({
        type: 'video',
        id: e.id!,
        title: e.metaData?.videoTitle ?? "",
        url: e.url ?? "",
        source: parsedUrl.resource,
        description: parsedUrl.pathname.replace(/[\W_]/g, " ").trim(),
        viewed: false
      })
    });
  }
);

export const fetchPendingVideos = createAsyncThunk(
  'videos/fetchPendingVideos',
  async () => {
    const pendingVideos = await videosClient.requestedVideos();

    return pendingVideos.map<PendingResource>(e => ({
      id: e.correlationId!,
      url: e.url!,
      requestedTimestamp: e.requestedTimestamp!,
      currentState: e.currentState!,
      error: e.errorMessage
        ? {
          errorMessage: e.errorMessage,
          errorTimestamp: e.errorTimestamp
        }
        : undefined
    }));
  }
);

export const retryAllPendingVideos = createAsyncThunk(
  'videos/retryAllPendingVideos',
  async () => {
    await videosClient.retryAllFailedVideos();
  }
);

export const retryPendingVideo = createAsyncThunk(
  'videos/retryPendingVideo',
  async (id: string) => {
    await videosClient.retryFailedVideo(id);
  }
);