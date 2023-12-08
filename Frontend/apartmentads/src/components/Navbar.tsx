import React, {useState} from "react";
import { Link, NavLink } from "react-router-dom";

import "./Navbar.css";
import { Logout } from "./Logout";

export const Navbar = () => {
    const [showMenu, setShowMenu] = useState(false);
    const [loggedIn, setLoggedIn] = useState(localStorage.getItem("refreshToken") !== null);

  return <nav>
    <Link to={"/"} className="title">
        Logo
    </Link>
    <div className="menu" 
        onClick={ () => {setShowMenu(!showMenu);}}>
        <span></span>
        <span></span>
        <span></span>
    </div>
    <ul className={showMenu ? "open" : ""}>
        <li>
            <NavLink to={"/ads"}>Ads</NavLink>
        </li>
        <li className={loggedIn ? "hide" : ""}>
            <NavLink to={"/login"}>Login</NavLink>
        </li>
        <li className={loggedIn ? "hide" : ""}>
            <NavLink to={"/register"}>Register</NavLink>
        </li>
        <li className={loggedIn ? "" : "hide"}>
            <Logout />
        </li>
    </ul>
  </nav>
};