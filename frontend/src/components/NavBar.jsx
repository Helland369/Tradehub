import { Link } from "react-router-dom";
import { AuthContext } from "./AuthContext";
import "../styles/NavBar.css";
import { useContext } from "react";

function NavBar() {
  // const token = localStorage.getItem("token");
  const { user, logout } = useContext(AuthContext);

  return (
    <nav className="navBar">
      <div className="navBar-logo">
        <Link to="/">Tradehub</Link>
      </div>
      <div className="navBar-links">
        <Link to="/" className="nav-link">
          Home
        </Link>

        {user ? (
          <>
            <Link to="/edit" className="nav-link">
              Edit
            </Link>
            <Link to="/createListing" className="nav-link">
              Create listing
            </Link>
            <Link
              className="nav-link"
              onClick={logout}
              // to="/"
              // className="nav-link"
              // onClick={() => {
              //   localStorage.removeItem("token");
              //   localStorage.removeItem("id");
              //   window.location.reload();
              // }}
            >
              Logout
            </Link>
          </>
        ) : (
          <>
            <Link to="/login" className="nav-link">
              Login
            </Link>
            <Link to="/registration" className="nav-link">
              Registration
            </Link>
          </>
        )}
      </div>
    </nav>
  );
}

export default NavBar;
