import React from "react";
import CommentModel from "../../../type/CommentModel";
import { Button } from "react-bootstrap";
import Swal from "sweetalert2";
import withReactContent from "sweetalert2-react-content";
import axios from "axios";
import { API_URL } from "../../../config";
import RefreshAccessToken from "../../Auth";

const MySwal = withReactContent(Swal);

function Comment(comment : CommentModel) {

    function handleDelete(event : any) {
        RefreshAccessToken();
        
        MySwal.fire({
            title: "Are you sure?",
            text: "You won't be able to revert this!",
            icon: "warning",
            showCancelButton: true,
            confirmButtonText: "Yes, delete it!",
            cancelButtonText: "No, keep it",
        })
        .then((result) => {
            if (result.isConfirmed) {
                axios.delete(`${API_URL}/api/apartments/${comment.apartmentId}/rooms/${comment.roomId}/objects/${comment.objectId}/comments/${event.target.value}`, {
                    headers: {
                        Authorization: `Bearer ${localStorage.getItem('accessToken')}`
                    }
                })
                .then((response) => {
                    MySwal.fire({
                        title: "Success",
                        text: `Comment deleted successfully.`,
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
        })
    }

    return (
        <div>
            <div className="input-group mb-3">
            <span className="form-control">{comment.content}</span>
            <Button className="btn btn-danger" value={comment.id} onClick={handleDelete}>Delete</Button>
            </div>
        </div>
    )
}

export default Comment;