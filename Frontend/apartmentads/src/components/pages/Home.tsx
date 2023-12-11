import React from "react";
import apartmentSvg from "../../apartment.svg";
import "./Home.css";

export const Home = () => {
  return <div>
      <div className="h1 text-center">Home</div>
      <hr />
      <div className="container text-center">
        <img src={apartmentSvg} alt="Apartment SVG Here" />
      </div>
    </div>;
};