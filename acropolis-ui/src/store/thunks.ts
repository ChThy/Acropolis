import { createAsyncThunk } from "@reduxjs/toolkit";
import { Page, PendingResource, Video } from "../models/resource";
import { RootState } from "./store";
import { activeFetchPagesRequestIdSelector, activeFetchVideosRequestIdSelector } from "./selectors";
import parseUrl from "parse-url";
import { Configuration, DownloadedVideo, PagesApi, VideosApi } from "../clients/acropolis";
import { map } from "../shared/models/paged-result";
import { Filter, SieveFilterService } from "sieve-ts";

const baseUrl = 'http://localhost:5092';

const pagesApi = new PagesApi(new Configuration({ basePath: baseUrl }));
const videosApi = new VideosApi(new Configuration({ basePath: baseUrl }));
const sieveService = new SieveFilterService();

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

    const pages = await pagesApi.pages();
    return pages.data.map<Page>(e => {
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

export const fetchPendingPages = createAsyncThunk(
  'pages/fetchPendingPages',
  async () => {
    const pendingPages = await pagesApi.requestedPages();

    return pendingPages.data.map<PendingResource>(e => ({
      id: e.correlationId!,
      url: e.url!,
      requestedTimestamp: e.requestedTimestamp!,
      currentState: e.currentState!,
      error: e.errorMessage
        ? {
          errorMessage: e.errorMessage!,
          errorTimestamp: e.errorTimestamp!
        }
        : undefined
    }));
  }
);

export const retryAllPendingPages = createAsyncThunk(
  'pages/retryAllPendingPages',
  async () => {
    await pagesApi.retryFailedScrapes();
  }
);

export const retryPendingPage = createAsyncThunk(
  'pages/retryPendingPage',
  async (id: string) => {
    await pagesApi.retryFailedScrapes();  //TODO specific retry
  }
);

export const fetchVideos = createAsyncThunk(
  'videos/fetch',
  async (filter: Filter, thunkApi) => {
    const activeFetchRequestId = activeFetchVideosRequestIdSelector(thunkApi.getState() as RootState);
    if (activeFetchRequestId && activeFetchRequestId !== thunkApi.requestId) {
      return rejectRequestBusy(thunkApi)
    }

    const x = sieveService.getFilterValue(filter);
    console.log(x);

    const pages = await videosApi.videos({});
    return map(pages.data, mapVideo);
  }
);

export const fetchPendingVideos = createAsyncThunk(
  'videos/fetchPendingVideos',
  async () => {
    const pendingVideos = await videosApi.requestedVideos();

    return pendingVideos.data.map<PendingResource>(e => ({
      id: e.correlationId!,
      url: e.url!,
      requestedTimestamp: e.requestedTimestamp!,
      currentState: e.currentState!,
      error: e.errorMessage
        ? {
          errorMessage: e.errorMessage!,
          errorTimestamp: e.errorTimestamp!
        }
        : undefined
    }));
  }
);

export const retryAllPendingVideos = createAsyncThunk(
  'videos/retryAllPendingVideos',
  async () => {
    await videosApi.retryAllFailedVideos();
  }
);

export const retryPendingVideo = createAsyncThunk(
  'videos/retryPendingVideo',
  async (id: string) => {
    await videosApi.retryFailedVideo({ id });
  }
);

function mapVideo(e: DownloadedVideo): Video {
  const parsedUrl = parseUrl(e.url!);

  return ({
    type: 'video',
    id: e.id!,
    title: e.metaData?.videoTitle ?? "",
    url: e.url ?? "",
    source: parsedUrl.resource,
    description: parsedUrl.pathname.replace(/[\W_]/g, " ").trim(),
    viewed: false
  });
}
