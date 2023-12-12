import React, { useEffect } from "react";
import { API_URL } from "../../../config";
import ApartmentModel from "../../../type/ApartmentModel";
import axios from "axios";
import Swal from "sweetalert2";
import withReactContent from "sweetalert2-react-content";
import Apartment from "./Apartment";
import { Button } from "react-bootstrap";
import "./Apartments.css";
import RefreshAccessToken from "../../Auth";
import { Link } from "react-router-dom";

const MySwal = withReactContent(Swal);

export default function Apartments() {
    const [apartments, setApartments] = React.useState<ApartmentModel[]>([]);

    useEffect(() => {
        RefreshAccessToken();

        axios.get(`${API_URL}/api/apartments`, {
            headers: {
                Authorization: `Bearer ${localStorage.getItem('accessToken')}`
            }
        })
            .then((response) => {
                setApartments(response.data);
            })
            .catch((error) => {
                MySwal.fire({
                    title: "Request failed",
                    text: `Something went wrong.`,
                    icon: "error",
                    confirmButtonText: "Ok",
                })
                .then(() => {
                    window.location.href = "/";
                });
            });
    }, []);

  return <div>
        <div className="h1 text-center">Apartments</div>
        <hr />
        <div className="container pb-2">
            <Link to="/" className="btn btn-secondary">Back</Link>
        </div>
        <div className="container pb-5">
            <div className="aligned-right">
                <Link className="btn btn-success" to={"/create-apartment"}>Create</Link>
            </div>
            <table className="table table-responsive">
                <thead>
                    <tr>
                        <td className="col-2">Address</td>
                        <td className="col-2">Floor</td>
                        <td className="col-2">Number</td>
                        <td className="col-2">Area</td>
                        <td className="col-2">Rating</td>
                        <td className="col-2">Action</td>
                    </tr>
                </thead>
                <tbody>
                    {
                        apartments.map((apartment: ApartmentModel) => (
                            <Apartment key={apartment.id} {...apartment} />
                        ))
                    }
                </tbody>
            </table>
        </div>
    </div>;
}