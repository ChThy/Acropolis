import { Props } from "../shared/helpers/component-props.helper";
import { Video } from "../models/resource";

export const VideoCardComponent: React.FC<Props<{ video: Video }>> = ({ video, className, ...rest }) => {

  return (
    <div {...rest} className={className + " border rounded-lg border-slate-600 p-2"}>
      <div className="mb-3">
        <p className="font-bold truncate ..." title={video.title!}>
          {video.title}
        </p>
        <span className="text-xs text-blue-400">
          <a href={video.url} target="_blank">
            {video.source}
          </a>
        </span>
      </div>

      <p className="text-sm">
        {video.description}
      </p>

    </div>
  );
}

export default VideoCardComponent;