import React from "react";
import AdModel from "../../../type/AdModel";
import { Button } from "react-bootstrap";
import { Link } from "react-router-dom";

function Ad(ad : AdModel) {
    return <tr className="table-row">
        <td className="col-2">{new Date(ad.date).toLocaleDateString('lt-LT')}</td>
        <td className="col-3">{ad.title}</td>
        <td className="col-3">{ad.description}</td>
        <td className="col-2">{ad.price}</td>
        <td className="col-2">
            <Link className="btn btn-primary" to={`/ads/${ad.id}`}>View</Link>
        </td>
    </tr>;
}

export default Ad;