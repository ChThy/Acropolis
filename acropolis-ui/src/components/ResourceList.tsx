import React, {  } from "react";
import { Props } from "../helpers/component-props.helper";
import { Page, ResourceModel } from "../models/resource";
import create from "../factories/resource-component-factory";

export const ResourceList: React.FC<Props<{ items: ResourceModel[] }>> = ({ items }) => {
  return (
    <div className="p-5">
      <p className="pb-3 text-xl font-bold">Total: {items.length}</p>
      <div>
        {items.map(page => (
          create(page, { className: "mb-3" })
        ))}
      </div>
    </div>
  );
}

export default ResourceList;