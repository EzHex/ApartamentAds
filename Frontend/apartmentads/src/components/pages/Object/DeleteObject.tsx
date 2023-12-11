import React from "react";
import ObjectModel from "../../../type/ObjectModel";
import { Button } from "react-bootstrap";
import Swal from "sweetalert2";
import withReactContent from "sweetalert2-react-content";
import RefreshAccessToken from "../../Auth";
import axios from "axios";
import { API_URL } from "../../../config";

const MySwal = withReactContent(Swal);

function DeleteObject(object : ObjectModel) {
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
                axios.delete(`${API_URL}/api/apartments/${object.apartmentId}/rooms/${object.roomId}/objects/${event.target.value}`, {
                    headers: {
                        Authorization: `Bearer ${localStorage.getItem('accessToken')}`
                    }
                })
                .then((response) => {
                    MySwal.fire({
                        title: "Success",
                        text: `Object deleted successfully.`,
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
        <Button className="btn btn-danger" value={object.id} onClick={handleDelete}>Delete</Button>
    )
}

export default DeleteObject;