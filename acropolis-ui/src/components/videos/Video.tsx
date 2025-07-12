import { FC, useEffect, useRef } from "react";
import { useParams } from "react-router";
import { useAppDispatch } from "../../store/hooks";
import { fetchVideo } from "../../store/thunks";
import { useSelector } from "react-redux";
import { videoSelector } from "../../store/selectors";

export const Video: FC = () => {
  const params = useParams();
  const dispatch = useAppDispatch();
  const id = params['id'];
  
  const video = useSelector(videoSelector(id!));

  useEffect(() => {
    if (!id) {
      return;
    }

    dispatch(fetchVideo(id));
  }, []);



  return (
    <>
      <h1 className="text-xl font-bold">{video?.title}</h1>
      <a href={video?.url} target="_blank" className="hover:text-cyan-500">{video?.url}</a>

      <div className="mt-8">
        <video controls>
          <source src=" http://localhost:5092/videos/01947e7f-67b4-7452-b147-4c25e7028c12/01947e7f-67b5-7615-b6b7-dd175ddf2641"
            type="video/mp4"
          />
          Your browser does not support the video tag.
        </video>
      </div>
    </>
  );
}