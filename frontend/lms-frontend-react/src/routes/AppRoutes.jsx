import { useRoutes } from "react-router-dom";
import Login from "../pages/Login";
import Register from "../pages/Register";
import Home from "../pages/Home";
import CourseList from "../pages/CourseList";
import PrivateRoute from "./PrivateRoute";

export default function AppRoutes() {
  const routes = useRoutes([
    { path: "/", element: <Home /> },
    { path: "/login", element: <Login /> },
    { path: "/register", element: <Register /> },
    {
      path: "/courses",
      element: (
        <PrivateRoute>
          <CourseList />
        </PrivateRoute>
      ),
    },
  ]);

  return routes;
}
