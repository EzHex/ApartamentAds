import React from "react";
import AdModel from "../../../type/AdModel";
import { Button } from "react-bootstrap";

function Ad(ad : AdModel) {
    return <tr className="table-row">
        <td className="col-2">{new Date(ad.date).toLocaleDateString('lt-LT')}</td>
        <td className="col-3">{ad.title}</td>
        <td className="col-3">{ad.description}</td>
        <td className="col-2">{ad.price}</td>
        <td className="col-2">
            <Button variant="primary" value={ad.id}>View</Button>
        </td>
    </tr>;
}

export default Ad;