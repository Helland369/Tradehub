import { Routes, Route } from "react-router-dom";
import NavBar from "./components/NavBar";
import Home from "./pages/Home";
import Login from "./pages/Login";
import Registration from "./pages/Registration";
import EditUser from "./pages/EditUser";
import CreateListing from "./pages/CreateListing";
import ItemPage from "./pages/ItemPage";

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
          <Route path="/item/:id" element={<ItemPage />} />
        </Routes>
      </main>
    </div>
  );
}

export default App;
