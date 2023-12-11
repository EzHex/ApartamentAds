import { Link } from "react-router-dom";

export const Logout = () => {

    const handleLogout = () => {
        localStorage.removeItem("accessToken");
        localStorage.removeItem("refreshToken");
        window.location.href = "/";
    };
    
    return <Link to={"/"} onClick={handleLogout}>Logout</Link>
};

