import { Link } from "react-router-dom";
import "../styles/NavBar.css";

function NavBar() {
  const token = localStorage.getItem("token");

  return (
    <nav className="navBar">
      <div className="navBar-logo">
        <Link to="/">Tradehub</Link>
      </div>
      <div className="navBar-links">
        <Link to="/" className="nav-link">
          Home
        </Link>

        {!token && (
          <>
            <Link to="/login" className="nav-link">
              Login
            </Link>
            <Link to="/registration" className="nav-link">
              Registration
            </Link>
          </>
        )}

        {token && (
          <>
            <Link to="/edit" className="nav-link">
              Edit
            </Link>
            <Link
              to="/"
              className="nav-link"
              onClick={() => {
                localStorage.removeItem("token");
                localStorage.removeItem("id");
                window.location.reload();
              }}
            >
              Logout
            </Link>
          </>
        )}
      </div>
    </nav>
  );
}

export default NavBar;
