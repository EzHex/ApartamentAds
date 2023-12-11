import React, { useEffect } from "react";
import { Button } from "react-bootstrap";
import ObjectModel from "../../../type/ObjectModel";
import CommentModel from "../../../type/CommentModel";
import axios from "axios";
import { API_URL } from "../../../config";
import { useParams } from "react-router-dom";
import Swal from "sweetalert2";
import withReactContent from "sweetalert2-react-content";
import Comment from "../Comment/Comment";
import RefreshAccessToken from "../../Auth";

const MySwal = withReactContent(Swal);

function ViewObject() {
    const apartmentId = useParams().apartmentId;
    const roomId = useParams().roomId;
    const objectId = useParams().id;

    const [comments, setComments] = React.useState<CommentModel[]>([]);
    const [newComment, setNewComment] = React.useState("");

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

    const handleCommentChange = (event: any) => {
        setNewComment(event.target.value);
    };

    useEffect(() => {
        RefreshAccessToken();

        axios.get(`${API_URL}/api/apartments/${apartmentId}/rooms/${roomId}/objects/${objectId}`, {
            headers: {
                Authorization: `Bearer ${localStorage.getItem('accessToken')}`
            } 
        })
        .then((response) => {
            setName(response.data.name);
            setDescription(response.data.description);
            setImage(response.data.image);
            setGrade(response.data.grade);
        }) 
        .catch((error) => {
            MySwal.fire({
                title: "Request failed",
                text: `Something went wrong.`,
                icon: "error",
                confirmButtonText: "Ok",
            })
            .then(() => {
                window.location.href = `/apartments/${apartmentId}/rooms/${roomId}`;
            })
        })

        axios.get(`${API_URL}/api/apartments/${apartmentId}/rooms/${roomId}/objects/${objectId}/comments`, {
            headers: {
                Authorization: `Bearer ${localStorage.getItem('accessToken')}`
            } 
        })
        .then((response) => {
            const updatedComments = response.data.map((cmt: CommentModel) => {
                return {
                    ...cmt,
                    apartmentId: apartmentId,
                    roomId: roomId,
                    objectId: objectId
                };
            });
            setComments(updatedComments);
        })
        .catch((error) => {
            MySwal.fire({
                title: "Request failed",
                text: `Something went wrong.`,
                icon: "error",
                confirmButtonText: "Ok",
            })
            .then(() => {
                window.location.href = `/apartments/${apartmentId}/rooms/${roomId}`;
            })
        })
    }, [])


    function handleEdit() {
        RefreshAccessToken();

        axios.put(`${API_URL}/api/apartments/${apartmentId}/rooms/${roomId}/objects/${objectId}`, {
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
                text: `Object edited successfully.`,
                icon: "success",
                confirmButtonText: "Ok",
            })
            .then(() => {
                window.location.reload();
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
                window.location.href = `/apartments/${apartmentId}/rooms/${roomId}`;
            })
        })
    }

    function handleSend() {
        RefreshAccessToken();

        axios.post(`${API_URL}/api/apartments/${apartmentId}/rooms/${roomId}/objects/${objectId}/comments`, {
            content: newComment
        }, {
            headers: {
                Authorization: `Bearer ${localStorage.getItem('accessToken')}`
            } 
        })
        .then((response) => {
            MySwal.fire({
                title: "Success",
                text: `Comment sent successfully.`,
                icon: "success",
                confirmButtonText: "Ok",
            })
            .then(() => {
                window.location.reload();
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
                window.location.reload();
            })
        })
    }

    return (
        <div>
            <div className="h1 text-center">Object</div>
            <hr />
            <div className="container pb-5">
                <fieldset>
                    <div className="input-group mb-3">
                        <span className="input-group-text">{">"}</span>
                        <div className="form-floating">
                            <input type="text" className="form-control" id="name" placeholder="Name" value={name} onChange={handleNameChange} />
                            <label htmlFor="name">Name</label>
                        </div>
                    </div>
                    <div className="input-group mb-3">
                        <span className="input-group-text">{">"}</span>
                        <div className="form-floating">
                            <textarea className="form-control" placeholder="Description" id="description" value={description} onChange={handleDescriptionChange}></textarea>
                            <label htmlFor="description">Description</label>
                        </div>
                    </div>
                    <div className="input-group mb-3">
                        <span className="input-group-text">{">"}</span>
                        <div className="form-floating">
                            <input type="text" className="form-control" id="image" placeholder="Image" value={image} onChange={handleImageChange} />
                            <label htmlFor="image">Image</label>
                        </div>
                    </div>
                    <div className="input-group mb-3">
                        <span className="input-group-text">{">"}</span>
                        <div className="form-floating">
                            <input type="number" className="form-control" id="grade" placeholder="Grade" value={grade} onChange={handleGradeChange} />
                            <label htmlFor="grade">Grade</label>
                        </div>
                    </div>
                </fieldset>
                <div className="aligned-right">
                    <Button className="btn btn-primary" onClick={handleEdit}>Save</Button>
                </div>
                <hr />
                <div className="h1 text-center">Comments</div>
                <hr />
                {
                    comments.map((cmt: CommentModel) => (
                        <Comment key={cmt.id} {...cmt} />
                    ))
                }
                <hr />
                <fieldset>
                    <div className="input-group mb-3">
                        <span className="input-group-text">{">"}</span>
                        <div className="form-floating">
                            <textarea 
                                className="form-control" 
                                placeholder="Comment" 
                                id="newComment"
                                value={newComment}
                                onChange={handleCommentChange}></textarea>
                            <label htmlFor="comment">Comment</label>
                        </div>
                    </div>
                </fieldset>
                <div className="aligned-right">
                    <Button className="btn btn-primary" onClick={handleSend}>Send</Button>
                </div>
            </div>
        </div>
    )
}

export default ViewObject;