import { createAsyncThunk, createSelector } from "@reduxjs/toolkit";
import { PagesClient } from "../clients/clients";
import axios from "axios";
import { Page } from "../models/resource";
import { RootState } from "./store";

const pagesClient = new PagesClient('http://localhost:5092', axios.create());

export const fetchPages = createAsyncThunk(
  'pages/fetch',
  async (_, thunkApi) => {
        
    const pages = await pagesClient.pages();    
    return pages.map<Page>(e => ({ title: e.title ?? "", url: e.url ?? "" }));
  }
)


//TODO: move to other files
const rootStateSelector = (state: RootState) => state;
const resourcesSelector = createSelector(rootStateSelector, root => root.resources);