import { BrowserRouter as Router, Routes, Route } from "react-router-dom";
import { Login } from "./screens/Login";
import { Home } from "./screens/Home";


export default function AppRoutes() {
  return (
    <Router basename={import.meta.env.BASE_URL}>
      <Routes>
        <Route path="/" element={<Login />} />
        <Route path="/home" element={<Home />} />
      </Routes>
    </Router>
  );
}
