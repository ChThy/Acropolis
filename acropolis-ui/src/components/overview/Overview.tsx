import { FC, useEffect } from "react";
import { useAppDispatch } from "../../store/hooks";
import { fetchPendingVideos, retryAllPendingVideos, retryPendingVideo } from "../../store/thunks";
import { Button, Card, List } from "antd";
import { useSelector } from "react-redux";
import { pendingVideosSelector } from "../../store/selectors";

export const Overview: FC = () => {
  const dispatch = useAppDispatch();
  const pendingVideos = useSelector(pendingVideosSelector);

  useEffect(() => {
    dispatch(fetchPendingVideos())
  }, []);

  return (
    <div className="grid grid-cols-3">
    <Card>
      <Button size="small" onClick={() => dispatch(fetchPendingVideos())}>Refreh</Button>
      <Button size="small" onClick={() => dispatch(retryAllPendingVideos())}>Retry All</Button>
      <List size="small" dataSource={pendingVideos} renderItem={(item) => <List.Item>
        <div className="flex flex-row w-full items-center">
          <div className="flex-auto overflow-hidden">
            <p title={item.error?.errorMessage} className="truncate">{item.error?.errorMessage}</p>
          </div>
          <div className="pl-4">
            <Button size="small" disabled={!item.error} onClick={() => dispatch(retryPendingVideo(item.id))}>retry</Button>
          </div>
        </div>
      </List.Item>} />
    </Card>

    </div>
  );
}