import React from "react";
import { Link, useLocation } from "react-router-dom";

import "./Footer.css";

export const Footer = () => {
    const handleScrollToTop = () => {
        window.scrollTo({ top: 0, behavior: "smooth" });
    };

    const location = useLocation();

    return (
        <footer>
            <Link onClick={handleScrollToTop} to={location.pathname}>
                Top of the page
            </Link>
        </footer>
    );
};