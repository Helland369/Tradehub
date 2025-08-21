import { Link } from "react-router-dom";
import "../styles/NavBar.css";

function NavBar() {
  return (
    <nav className="navBar">
      <div className="navBar-logo">
        <Link to="/">Tradehub</Link>
      </div>
      <div className="navBar-links">
        <Link to="/" className="nav-link">
          Home
        </Link>
        <Link to="/login" className="nav-link">
          Login
        </Link>
      </div>
    </nav>
  );
}

export default NavBar;
