import { createSelector } from "@reduxjs/toolkit";
import { RootState } from "./store";

export const rootStateSelector = (state: RootState) => state;
export const resourcesSelector = createSelector(rootStateSelector, root => root.resources);

export const activeFetchPagesRequestIdSelector = createSelector(resourcesSelector, resources => resources.pages.activeFetchRequestId);
export const pagesSelector = createSelector(resourcesSelector, resources => resources.pages.pages);
export const pagesCountSelector = createSelector(resourcesSelector, resources => resources.pages.pages.length);
export const pendingPagesSelector = createSelector(resourcesSelector, resources => resources.pages.pendingPages);

export const activeFetchVideosRequestIdSelector = createSelector(resourcesSelector, resources => resources.videos.activeFetchRequestId);
export const videosSelector = createSelector(resourcesSelector, resources => resources.videos.videos);
export const videoCountSelector = createSelector(resourcesSelector, resources => resources.videos.videos.length);
export const pendingVideosSelector = createSelector(resourcesSelector, resources => resources.videos.pendingVideos);