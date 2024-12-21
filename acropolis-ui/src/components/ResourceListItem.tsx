import { ResourceModel } from "../models/resource";
import { Props } from "../helpers/component-props.helper";
import truncate from "./../helpers/string-extensions";

export const ResourceListItem: React.FC<Props<{ resource: ResourceModel }>> = ({ resource, className, ...rest }) => {

  return (
    <div {...rest} className={className + " border rounded-lg border-slate-600 p-2"}>
      <div className="mb-3">
        <p className="font-bold" title={resource.title!}>
          {truncate(resource.title, 128)}
        </p>
        <span className="text-xs text-blue-400">
          <a href={resource.url}>
            {resource.source}
          </a> 
        </span>
      </div>

      <p className="text-sm">
        {resource.description}
      </p>

    </div>
  );
}

export default ResourceListItem;