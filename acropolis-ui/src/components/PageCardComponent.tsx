import { Props } from "../helpers/component-props.helper";
import { Page } from "../models/resource";

export const PageCardComponent: React.FC<Props<{ page: Page }>> = ({ page, className, ...rest }) => {
  return (
    <div {...rest} className={className + " border rounded-lg border-slate-600 p-2"}>
      <div className="mb-3">
        <p className="font-bold truncate ..." title={page.title!}>
          {page.title}
        </p>
        <span className="text-xs text-blue-400">
          <a href={page.url} target="_blank">
            {page.source}
          </a>
        </span>
      </div>

      <p className="text-sm">
        {page.description}
      </p>

    </div>
  );
}
export default PageCardComponent;