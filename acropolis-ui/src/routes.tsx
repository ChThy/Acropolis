import { RouteObject } from "react-router";
import App from "./App";
import { Overview } from "./components/overview/Overview";

export const routes: RouteObject[] = [
  {
    path: '',
    element: <App />,
    children: [
      {
        path: '',
        element: <Overview />
      }
    ]
  },
  {
    path: '*',
    element: <>Not Found</>
  }
];