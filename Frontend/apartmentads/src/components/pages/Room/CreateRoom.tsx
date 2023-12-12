import React from "react";
import { Button, Form } from "react-bootstrap";
import Swal from "sweetalert2";
import withReactContent from "sweetalert2-react-content";
import { API_URL } from "../../../config";
import axios from "axios";
import RefreshAccessToken from "../../Auth";
import { Link, useParams } from "react-router-dom";

const MySwal = withReactContent(Swal);

function CreateRoom() {
    const [apartmentId, setApartmentId] = React.useState(useParams().apartmentId);
    const [name, setName] = React.useState("");
    const [rating, setRating] = React.useState(0);

    const handleNumberChange = (event: any) => {
        setName(event.target.value);
    };

    const handleRatingChange = (event: any) => {
        setRating(event.target.value);
    };

    function handleCreate() {
        RefreshAccessToken();

        axios.post(`${API_URL}/api/apartments/${apartmentId}/rooms`, {
            name: name,
            grade: rating,
        }, {
            headers: {
                Authorization: `Bearer ${localStorage.getItem('accessToken')}`
            }
        })
            .then((response) => {
                MySwal.fire({
                    title: "Success!",
                    text: "You have successfully created a room.",
                    icon: "success",
                    confirmButtonText: "Ok",
                }).then(() => {
                    window.location.href = `/apartments/${apartmentId}`;
                });
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
    }

    return (
        <div>
            <div className="h1 text-center">Room</div>
            <hr />
            <div className="container pb-2">
                <Link to={`/apartments/${apartmentId}/`} className="btn btn-secondary">Back</Link>
            </div>
            <div className="container pb-5">
                <Form>
                    <Form.Group className="mb-3" controlId="formBasicName">
                        <Form.Label>Name</Form.Label>
                        <Form.Control 
                            type="text" 
                            placeholder="Enter name" 
                            value={name} 
                            onChange={handleNumberChange} />
                    </Form.Group>
                    <Form.Group className="mb-3" controlId="formBasicRating">
                        <Form.Label>Rating</Form.Label>
                        <Form.Control 
                            type="number" 
                            placeholder="Enter rating" 
                            value={rating} 
                            onChange={handleRatingChange} />
                    </Form.Group>
                    <Button variant="success" onClick={handleCreate}>Create</Button>
                </Form>
            </div>
        </div>
    )
}

export default CreateRoom;