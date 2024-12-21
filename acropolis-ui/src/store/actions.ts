import { createAsyncThunk, createSelector } from "@reduxjs/toolkit";
import { PagesClient } from "../clients/clients";
import axios from "axios";
import { Page } from "../models/resource";
import { RootState } from "./store";
import { resourcesSelector } from "./selectors";
import parseUrl from "parse-url";

const pagesClient = new PagesClient('http://localhost:5092', axios.create());

export const fetchPages = createAsyncThunk(
  'pages/fetch',
  async (_, thunkApi) => {
    const activeFetchRequestId = resourcesSelector(thunkApi.getState() as RootState).pages.activeFetchRequestId;
    if (activeFetchRequestId && activeFetchRequestId !== thunkApi.requestId) {
      return thunkApi.rejectWithValue("Still fetching previous request.");
    }

    const pages = await pagesClient.pages(false);
    return pages.map<Page>(e => {
      const parsedUrl = parseUrl(e.url!);

      return ({
      id: e.correlationId!,
      title: e.title ?? "",
      url: e.url ?? "",
      source: parsedUrl.resource,
      description: parsedUrl.pathname.replace(/[\W_]/g, " ").trim(),
      viewed: false
    })
  });
  }
);