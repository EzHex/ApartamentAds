import React from "react";
import ApartmentModel from "../../../type/ApartmentModel";
import { Button } from "react-bootstrap";
import { Link } from "react-router-dom";
import DeleteApartment from "./DeleteApartment";

function Apartment(apartment: ApartmentModel) {
    return <tr>
        <td className="col-2">{apartment.address}</td>
        <td className="col-2">{apartment.floor}</td>
        <td className="col-2">{apartment.number}</td>
        <td className="col-2">{apartment.area}</td>
        <td className="col-2">{apartment.rating}</td>
        <td className="col-2">
            <Link className="btn btn-primary"  to={`/apartments/${apartment.id}`}>View</Link>
            <Link className="btn btn-secondary" to={`/edit-apartment/${apartment.id}`}>Edit</Link>
            <DeleteApartment key={apartment.id} {...apartment} />
        </td>
    </tr>
}

export default Apartment;