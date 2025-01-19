import { ReactElement } from "react";
import PageComponent from "../components/PageComponent";
import VideoComponent from "../components/VideoComponent";
import { ElementProps } from "../helpers/component-props.helper";
import { Page, ResourceModel, Video } from "../models/resource";

const isPage = (resource: ResourceModel): resource is Page => resource.type === 'page';
const isVideo = (resource: ResourceModel): resource is Video => resource.type === `video`;

export default function create(resource: ResourceModel, props?: ElementProps): ReactElement {
  if (isPage(resource)) {
    return <PageComponent key={resource.id} page={resource} {...props} />
  }
  else if (isVideo(resource)) {
    return <VideoComponent key={resource.id} video={resource} {...props} />
  }
  console.error(`Invalid ResourceModel: ${typeof resource} `)
  return <></>;
}