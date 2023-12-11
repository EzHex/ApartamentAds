import React, { useEffect } from "react";
import Swal from "sweetalert2";
import withReactContent from "sweetalert2-react-content";
import { API_URL } from "../../../config";
import axios from "axios";
import ApartmentModel from "../../../type/ApartmentModel";
import { useParams } from "react-router-dom";
import { Button, Form } from "react-bootstrap";
import RefreshAccessToken from "../../Auth";

const MySwal = withReactContent(Swal);

export default function EditApartment() {
    
    const [apartmentId, setApartmentId] = React.useState(useParams().id);

    const [address, setAddress] = React.useState("");
    const [floor, setFloor] = React.useState(0);
    const [number, setNumber] = React.useState(0);
    const [area, setArea] = React.useState(0);
    const [rating, setRating] = React.useState(0);

    const handleAddressChange = (event: any) => {
        setAddress(event.target.value);
    };

    const handleFloorChange = (event: any) => {
        setFloor(event.target.value);
    };

    const handleNumberChange = (event: any) => {
        setNumber(event.target.value);
    };

    const handleAreaChange = (event: any) => {
        setArea(event.target.value);
    };

    const handleRatingChange = (event: any) => {
        setRating(event.target.value);
    };

    useEffect(() => {
        RefreshAccessToken();

        axios.get(`${API_URL}/api/apartments/${apartmentId}`, {
            headers: {
                Authorization: `Bearer ${localStorage.getItem('accessToken')}`
            } 
        })
        .then((response) => {
            setAddress(response.data.address);
            setFloor(response.data.floor);
            setNumber(response.data.number);
            setArea(response.data.area);
            setRating(response.data.rating);
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

    const handleEdit = () => {
        axios.put(`${API_URL}/api/apartments/${apartmentId}`, {
            rating: rating
        }, {
            headers: {
                Authorization: `Bearer ${localStorage.getItem('accessToken')}`
            }
        })
            .then((response) => {
                MySwal.fire({
                    title: "Success!",
                    text: "You have successfully edited an apartment.",
                    icon: "success",
                    confirmButtonText: "Ok",
                }).then(() => {
                    window.location.href = "/apartments";
                });
            })
            .catch((error) => {
                MySwal.fire({
                    title: "Error!",
                    text: "You have failed to edit an apartment.",
                    icon: "error",
                    confirmButtonText: "Ok",
                })
                .then(() => {
                    window.location.href = `/edit-apartment/${apartmentId}`
                });
            });
    }
    
    return <div>
        <div className="h1 text-center">Edit apartment</div>
        <div className="container">
            <Form>
                <Form.Group controlId="formBasicAddress">
                    <Form.Label>Address</Form.Label>
                    <Form.Control 
                        type="text"
                        value={address}
                        onChange={handleAddressChange}
                        readOnly
                    />
                </Form.Group>

                <Form.Group controlId="formBasicFloor">
                    <Form.Label>Floor</Form.Label>
                    <Form.Control 
                        type="number"  
                        value={floor} 
                        onChange={handleFloorChange}
                        readOnly
                    />
                </Form.Group>

                <Form.Group controlId="formBasicNumber">
                    <Form.Label>Number</Form.Label>
                    <Form.Control 
                        type="number" 
                        value={number} 
                        onChange={handleNumberChange}
                        readOnly
                    />
                </Form.Group>

                <Form.Group controlId="formBasicArea">
                    <Form.Label>Area</Form.Label>
                    <Form.Control 
                        type="number" 
                        value={area} 
                        onChange={handleAreaChange}
                        readOnly
                    />
                </Form.Group>

                <Form.Group controlId="formBasicRating">
                    <Form.Label>Rating</Form.Label>
                    <Form.Control 
                        type="number" 
                        placeholder="Enter rating" 
                        value={rating} 
                        onChange={handleRatingChange} 
                    />
                </Form.Group>

                <Button variant="success" onClick={handleEdit}>Edit</Button>
            </Form>
        </div>
    </div>
}