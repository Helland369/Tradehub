import { Routes, Route } from "react-router-dom";
import NavBar from "./components/NavBar";
import Home from "./pages/Home";
import Login from "./pages/Login";
import Registration from "./pages/Registration";
import EditUser from "./pages/EditUser";
import CreateListing from "./pages/CreateListing";

function App() {
  return (
    <div>
      <NavBar />
      <main className="main-content">
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/login" element={<Login />} />
          <Route path="/registration" element={<Registration />} />
          <Route path="/edit" element={<EditUser />} />
          <Route path="createListing" element={<CreateListing />} />
        </Routes>
      </main>
    </div>
  );
}

export default App;
