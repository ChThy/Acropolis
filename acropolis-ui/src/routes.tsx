import { RouteObject } from "react-router";
import App from "./App";
import { Overview } from "./components/overview/Overview";
import { Video } from "./components/videos/Video";

export const routes: RouteObject[] = [
  {
    path: '',
    element: <App />,
    children: [
      {
        path: '',
        element: <Overview />
      },
      {
        path: 'videos/:id',
        element: <Video />
      }
    ]
  },
  {
    path: '*',
    element: <>Not Found</>
  }
];