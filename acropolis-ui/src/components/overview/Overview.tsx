import { FC, useEffect } from "react";
import { useAppDispatch } from "../../store/hooks";
import { fetchPendingPages, fetchPendingVideos, fetchVideos, retryAllPendingPages, retryAllPendingVideos, retryPendingPage, retryPendingVideo } from "../../store/thunks";
import { useSelector } from "react-redux";
import { pagesCountSelector, pendingPagesSelector, pendingVideosSelector, videoCountSelector } from "../../store/selectors";
import PendingResources from "../PendingResources";
import { Card, Statistic } from "antd";
import { Filter } from "sieve-ts";

export const Overview: FC = () => {
  const dispatch = useAppDispatch();
  const numberOfVideos = useSelector(videoCountSelector);
  const numberOfPages = useSelector(pagesCountSelector);
  const pendingVideos = useSelector(pendingVideosSelector);
  const pendingPages = useSelector(pendingPagesSelector);

  useEffect(() => {
    dispatch(fetchVideos(new Filter({ pageSize: 0 })));
    dispatch(fetchPendingVideos())
    dispatch(fetchPendingPages());
  }, []);

  return (
    <div className="grid grid-cols-3 gap-4 w-full">

      <Card styles={{ body: { height: '100%' } }}>
        <div className="grid grid-cols-2 gap-4 place-content-center h-full">
          <Statistic title="Number of videos" value={numberOfVideos} />
          <Statistic title="Number of pending videos" value={pendingVideos.length} />
          <Statistic title="Number of pages" value={numberOfPages} />
          <Statistic title="Number of pending pages" value={pendingPages.length} />
        </div>
      </Card>

      <PendingResources
        pendingItems={pendingVideos}
        onRefresh={() => dispatch(fetchPendingVideos())}
        onRetryPendingResources={(id) => dispatch(retryPendingVideo(id))}
        onRetryAllPendingResources={() => dispatch(retryAllPendingVideos())}
      />
      <PendingResources
        pendingItems={pendingPages}
        onRefresh={() => dispatch(fetchPendingPages())}
        onRetryPendingResources={(id) => dispatch(retryPendingPage(id))}
        onRetryAllPendingResources={() => dispatch(retryAllPendingPages())}
      />

    </div>
  );
}