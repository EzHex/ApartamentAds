import React from "react";
import { Button, Form } from "react-bootstrap";
import RefreshAccessToken from "../../Auth";
import axios from "axios";
import { API_URL } from "../../../config";
import { useParams } from "react-router-dom";
import withReactContent from "sweetalert2-react-content";
import Swal from "sweetalert2";

const MySwal = withReactContent(Swal);

function CreateObject() {
    const apartmentId = useParams().apartmentId;
    const roomId = useParams().roomId;

    const [name, setName] = React.useState("");
    const [description, setDescription] = React.useState("");
    const [image, setImage] = React.useState("");
    const [grade, setGrade] = React.useState(0);

    const handleNameChange = (event: any) => {
        setName(event.target.value);
    };

    const handleDescriptionChange = (event: any) => {
        setDescription(event.target.value);
    };

    const handleImageChange = (event: any) => {
        setImage(event.target.value);
    };

    const handleGradeChange = (event: any) => {
        setGrade(event.target.value);
    };

    function handleCreate() {
        RefreshAccessToken();

        axios.post(`${API_URL}/api/apartments/${apartmentId}/rooms/${roomId}/objects`, {
            name: name,
            description: description,
            image: image,
            grade: grade
        }, {
            headers: {
                Authorization: `Bearer ${localStorage.getItem('accessToken')}`
            } 
        })
        .then((response) => {
            MySwal.fire({
                title: "Success",
                text: `Object created successfully.`,
                icon: "success",
                confirmButtonText: "Ok",
            }).then(() => {
                window.location.href = `/apartments/${apartmentId}/rooms/${roomId}`;
            })
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
            })
        })
    }

    return (
        <div>
            <div className="h1 text-center">Create object</div>
            <div className="container">
                <Form>
                    <Form.Group className="mb-3" controlId="formBasicName">
                        <Form.Label>Name</Form.Label>
                        <Form.Control 
                            type="text" 
                            placeholder="Enter name" 
                            value={name}
                            onChange={handleNameChange}    
                        />
                    </Form.Group>
                    <Form.Group className="mb-3" controlId="formBasicDescription">
                        <Form.Label>Description</Form.Label>
                        <Form.Control 
                            type="text" 
                            placeholder="Enter description" 
                            value={description}
                            onChange={handleDescriptionChange}
                        />
                    </Form.Group>
                    <Form.Group className="mb-3" controlId="formBasicImage">
                        <Form.Label>Image</Form.Label>
                        <Form.Control 
                            type="text" 
                            placeholder="Enter image link" 
                            value={image}
                            onChange={handleImageChange}
                        />
                    </Form.Group>
                    <Form.Group className="mb-3" controlId="formBasicGrade">
                        <Form.Label>Grade</Form.Label>
                        <Form.Control 
                            type="text" 
                            placeholder="Enter grade" 
                            value={grade}
                            onChange={handleGradeChange}
                        />
                    </Form.Group>
                    <div className="aligned-right">
                        <Button className="btn btn-success" onClick={handleCreate}>Create</Button>
                    </div>
                </Form>
            </div>
        </div>
    )
}

export default CreateObject;