import React from "react";
import { Button } from "react-bootstrap";
import { useParams } from "react-router-dom";
import RoomModel from "../../../type/RoomModel";
import Swal from "sweetalert2";
import withReactContent from "sweetalert2-react-content";
import { API_URL } from "../../../config";
import axios from "axios";

const MySwal = withReactContent(Swal);

function DeleteRoom(room : RoomModel) {
    function handleDelete(event : any) {
        MySwal.fire({
            title: "Are you sure?",
            text: "You won't be able to revert this!",
            icon: "warning",
            showCancelButton: true,
            confirmButtonText: "Yes, delete it!",
        })
        .then((result) => {
            if (result.isConfirmed) {
                axios.delete(`${API_URL}/api/apartments/${room.apartmentId}/rooms/${event.target.value}`, {
                    method: "DELETE",
                    headers: {
                        "Content-Type": "application/json",
                        Authorization: `Bearer ${localStorage.getItem('accessToken')}`
                    }
                })
                .then((response) => {
                    MySwal.fire({
                        title: "Success!",
                        text: "You have successfully deleted a room.",
                        icon: "success",
                        confirmButtonText: "Ok",
                    }).then(() => {
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
                        window.location.href = "/";
                    });
                });
            }
        });
    }

    return (
        <Button className="btn btn-danger" value={room.id} onClick={handleDelete}>Delete</Button>
    )
}

export default DeleteRoom;