import { createAsyncThunk, createSelector } from "@reduxjs/toolkit";
import { PagesClient } from "../clients/clients";
import axios from "axios";
import { Page } from "../models/resource";
import { RootState } from "./store";
import { resourcesSelector } from "./selectors";

const pagesClient = new PagesClient('http://localhost:5092', axios.create());

export const fetchPages = createAsyncThunk(
  'pages/fetch',
  async (_, thunkApi) => {
    const activeFetchRequestId = resourcesSelector(thunkApi.getState() as RootState).pages.activeFetchRequestId;
    if (activeFetchRequestId && activeFetchRequestId !== thunkApi.requestId) {
      return thunkApi.rejectWithValue("Still fetching previous request.");
    }    

    const pages = await pagesClient.pages(false);
    return pages.map<Page>(e => ({ title: e.title ?? "", url: e.url ?? "" }));
  }
);