import { createSelector } from "@reduxjs/toolkit";
import { RootState } from "./store";
import { IResourcesStoreState } from "./resource-slice";

export const rootStateSelector = (state: RootState) => state;
// export const resourcesSelector = createSelector(rootStateSelector, root => root.resources);
export const sliceSelector = (state: IResourcesStoreState) => state;
export const resourcesSelector = createSelector(sliceSelector, state => state.resources);

export const activeFetchPagesRequestIdSelector = createSelector(resourcesSelector, resources => resources.pages.activeFetchRequestId);
export const pagesSelector = createSelector(resourcesSelector, resources => resources.pages.pages);

export const activeFetchVideosRequestIdSelector = createSelector(resourcesSelector, resources => resources.videos.activeFetchRequestId);
export const videosSelector = createSelector(resourcesSelector, resources => resources.videos.videos);