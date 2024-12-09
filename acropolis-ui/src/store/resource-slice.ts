import { createSlice } from "@reduxjs/toolkit"
import { Page, Videos } from "../models/resource"

interface ResourcesState {
  pages: Page[],
  videos: Videos[]
}

const initalState: ResourcesState = {
  pages: [],
  videos: []
}

export const resourcesSlice = createSlice({
  name: 'resources',
  initialState: initalState,
  reducers: {
    
  }
});

