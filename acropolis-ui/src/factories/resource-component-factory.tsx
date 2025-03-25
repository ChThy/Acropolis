import { ReactElement } from "react";
import PageCardComponent from "../components/PageCardComponent";
import VideoCardComponent from "../components/VideoCardComponent";
import { ElementProps } from "../shared/helpers/component-props.helper";
import { Page, ResourceModel, Video } from "../models/resource";

const isPage = (resource: ResourceModel): resource is Page => resource.type === 'page';
const isVideo = (resource: ResourceModel): resource is Video => resource.type === `video`;

export default function create(resource: ResourceModel, props?: ElementProps): ReactElement {
  if (isPage(resource)) {
    return <PageCardComponent key={resource.id} page={resource} {...props} />
  }
  else if (isVideo(resource)) {
    return <VideoCardComponent key={resource.id} video={resource} {...props} />
  }
  console.error(`Invalid ResourceModel: ${typeof resource} `)
  return <></>;
}