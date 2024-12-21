import { createSelector } from "@reduxjs/toolkit";
import { RootState } from "./store";

export const rootStateSelector = (state: RootState) => state;
export const resourcesSelector = createSelector(rootStateSelector, root => root.resources);
export const pagesSelector = createSelector(resourcesSelector, resources => resources.pages.pages);