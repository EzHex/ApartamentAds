import React from "react";
import ObjectModel from "../../../type/ObjectModel";
import { Link } from "react-router-dom";
import DeleteObject from "./DeleteObject";

function Object(object : ObjectModel) {
    return (
        <tr>
            <td className="col-2">{object.name}</td>
            <td className="col-3">{object.description}</td>
            <td className="col-4"><img src={object.image} className="img-fluid" alt="Image failed to load" /></td>
            <td className="col-1">{object.grade}</td>
            <td className="col-2">
                <Link className="btn btn-primary" to={`/apartments/${object.apartmentId}/rooms/${object.roomId}/objects/${object.id}`}>View</Link>
                <DeleteObject key={object.id} {...object} />
            </td>
        </tr>
    )
}

export default Object;