import { Page } from "../models/resource";
import ResourceListItem from "./ResourceListItem";

export const Pages: React.FC<{ pages: Page[] }> = ({ pages }) => {
  return (
    <div className="p-5">
      <p className="pb-3 text-xl font-bold">Total pages scraped: {pages.length}</p>
      <div>
        {pages.map(page => (
            <ResourceListItem key={page.id} resource={page} className="mb-3" />
        ))}
      </div>
    </div>
  );
}

export default Pages;