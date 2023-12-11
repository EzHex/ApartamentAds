import React from "react";
import { Button, Form } from "react-bootstrap";
import Swal from "sweetalert2";
import withReactContent from "sweetalert2-react-content";
import { API_URL } from "../../../config";
import axios from "axios";
import RefreshAccessToken from "../../Auth";

const MySwal = withReactContent(Swal);

export default function CreateApartment() {

    const [address, setAddress] = React.useState("");
    const [floor, setFloor] = React.useState(0);
    const [number, setNumber] = React.useState(0);
    const [area, setArea] = React.useState(0);
    const [rating, setRating] = React.useState(0);

    const handleAddressChange = (event : any) => {
        setAddress(event.target.value);
    };

    const handleFloorChange = (event : any) => {
        setFloor(event.target.value);
    };

    const handleNumberChange = (event : any) => {
        setNumber(event.target.value);
    };

    const handleAreaChange = (event : any) => {
        setArea(event.target.value);
    };

    const handleRatingChange = (event : any) => {
        setRating(event.target.value);
    };

    const handleCreate = () => {
        RefreshAccessToken();

        axios.post(`${API_URL}/api/apartments`, {
            address: address,
            floor: floor,
            number: number,
            area: area,
            rating: rating,
        }, {
            headers: {
                Authorization: `Bearer ${localStorage.getItem('accessToken')}`
            }
        })
            .then((response) => {
                MySwal.fire({
                    title: "Success!",
                    text: "You have successfully created an apartment.",
                    icon: "success",
                    confirmButtonText: "Ok",
                }).then(() => {
                    window.location.href = "/apartments";
                })
                ;

        }).catch((error) => {
            MySwal.fire({
                title: "Request failed",
                text: `Something went wrong.`,
                icon: "error",
                confirmButtonText: "Ok",
                }).then(() => {
                    window.location.href = "/create-apartment";
                });
            });
    }

    return <div>
        <div className="h1 text-center">Create Apartment</div>
        <div className="container">
            <Form>
                <Form.Group className="mb-3" controlId="address">
                    <Form.Label>Address</Form.Label>
                    <Form.Control 
                        type="text" 
                        placeholder="Enter address"
                        value={address}
                        onChange={handleAddressChange}
                    />
                </Form.Group>
                <Form.Group className="mb-3" controlId="floor">
                    <Form.Label>Floor</Form.Label>
                    <Form.Control 
                        type="number" 
                        placeholder="Enter floor" 
                        value={floor}
                        onChange={handleFloorChange}
                    />
                </Form.Group>
                <Form.Group className="mb-3" controlId="number">
                    <Form.Label>Number</Form.Label>
                    <Form.Control 
                        type="number" 
                        placeholder="Enter number" 
                        value={number}
                        onChange={handleNumberChange}
                    />
                </Form.Group>
                <Form.Group className="mb-3" controlId="area">
                    <Form.Label>Area</Form.Label>
                    <Form.Control 
                        type="number" 
                        placeholder="Enter area" 
                        value={area}
                        onChange={handleAreaChange}
                    />
                </Form.Group>
                <Form.Group className="mb-3" controlId="rating">
                    <Form.Label>Rating</Form.Label>
                    <Form.Control 
                        type="number" 
                        placeholder="Enter rating" 
                        value={rating}
                        onChange={handleRatingChange}
                    />
                </Form.Group>
                <Button variant="success" onClick={handleCreate}>Create</Button>
            </Form>
        </div>
    </div>;
};