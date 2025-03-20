import { FC } from "react";
import { PendingResource } from "../models/resource";
import { Button, Card, List } from "antd";

export type PendingResourcesListProps = {
  pendingItems: PendingResource[];
  onRefresh: () => void;
  onRetryPendingResources: (id: string) => void;
  onRetryAllPendingResources: () => void;
}

export const PendingResources: FC<PendingResourcesListProps> = ({ pendingItems, onRefresh, onRetryPendingResources, onRetryAllPendingResources }) => {
  return (
    <Card>
      <Button size="small" onClick={() => onRefresh()}>Refreh</Button>
      <Button size="small" onClick={() => onRetryAllPendingResources()}>Retry All</Button>
      <List size="small" dataSource={pendingItems} renderItem={(item) => <List.Item>
        <div className="flex flex-row w-full items-center">
          <div className="flex-auto overflow-hidden">
            <p title={item.error?.errorMessage} className="truncate">{item.error?.errorMessage}</p>
          </div>
          <div className="pl-4">
            <Button size="small" disabled={!item.error} onClick={() => onRetryPendingResources(item.id)}>retry</Button>
          </div>
        </div>
      </List.Item>} />
    </Card>
  );
}

export default PendingResources;