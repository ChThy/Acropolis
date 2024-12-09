import { createAsyncThunk } from "@reduxjs/toolkit";
import { PagesClient } from "../clients/clients";
import axios from "axios";

const pagesClient = new PagesClient('http://localhost:5092/', axios.create({}));

export const fetchPages = createAsyncThunk(
  'pages/fetch',
  async ({} , thunkApi) => {
    console.log('fetching');
    const pages = await pagesClient.pages();
    console.log('fetched');
    return pages;
  }
)