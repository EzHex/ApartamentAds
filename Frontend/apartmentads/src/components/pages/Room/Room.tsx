import React from "react";
import RoomModel from "../../../type/RoomModel";
import { Button } from "react-bootstrap";
import DeleteRoom from "./DeleteRoom";
import { Link } from "react-router-dom";

function Room(room : RoomModel) {
    return (
    <tr>
        <td className="col-6">{room.name}</td>
        <td className="col-2">{room.grade}</td>
        <td className="col-4">
            <Link className="btn btn-primary" to={`/apartments/${room.apartmentId}/rooms/${room.id}`}>View</Link>
            <DeleteRoom key={room.id} {...room} />
        </td>
    </tr>)
}

export default Room;