import axios from "axios";
import { Button } from "react-bootstrap";
import { API_URL } from "../../../config";
import Swal from "sweetalert2";
import withReactContent from "sweetalert2-react-content";
import RefreshAccessToken from "../../Auth";
import ApartmentModel from "../../../type/ApartmentModel";

const MySwal = withReactContent(Swal);

function DeleteApartment(apartment: ApartmentModel) {
    const handleDelete = (event : any) => {
        
        MySwal.fire({
            title: "Are you sure?",
            text: "You won't be able to revert this!",
            icon: "warning",
            showCancelButton: true,
            confirmButtonText: "Yes, delete it!",
        })
        .then((result) => {
            if (result.isConfirmed) {
                RefreshAccessToken();

                axios.delete(`${API_URL}/api/apartments/${event.target.value}`, {
                    headers: {
                        Authorization: `Bearer ${localStorage.getItem('accessToken')}`
                    } 
                })
                .then((response) => {
                    MySwal.fire({
                        title: "Success!",
                        text: "You have successfully deleted an apartment.",
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
    };

    return (
        <Button className="btn btn-danger" value={apartment.id} onClick={handleDelete}>Delete</Button>
    );
}

export default DeleteApartment;