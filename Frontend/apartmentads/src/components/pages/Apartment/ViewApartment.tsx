import React, { useEffect, useState } from "react";
import { Link, useParams } from "react-router-dom";
import RefreshAccessToken from "../../Auth";
import axios from "axios";
import { API_URL } from "../../../config";
import RoomModel from "../../../type/RoomModel";
import Swal from "sweetalert2";
import withReactContent from "sweetalert2-react-content";
import Room from "../Room/Room";
import { Button } from "react-bootstrap";


const MySwal = withReactContent(Swal);

function ViewApartment() {
    const id = useParams().id;

    const [adId, setAdId] = React.useState(-1);
    const [rooms, setRooms] = useState<RoomModel[]>([]);

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

        axios.get(`${API_URL}/api/apartments/${id}`, {
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

        axios.get(`${API_URL}/api/apartments/${id}/rooms`, {
            headers: { 
                Authorization: `Bearer ${localStorage.getItem("accessToken")}` 
            }
        }).then((response) => {
            const updatedRooms = response.data.map((room: RoomModel) => {
                return {
                    ...room,
                    apartmentId: Number(id)
                };
            });
            setRooms(updatedRooms);
        }).catch((error) => {
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

        axios.post(`${API_URL}/api/ads/exists`, {
            apartmentId: Number(id)
        }, {
            headers: { 
                Authorization: `Bearer ${localStorage.getItem("accessToken")}` 
            }
        }).then((response) => {
            setAdId(response.data.id);
        }).catch((error) => {
            setAdId(-1);
        });
    }, []);

    function handleAdDelete(event: any) {
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
                axios.delete(`${API_URL}/api/ads/${event.target.value}`, {
                    headers: {
                        Authorization: `Bearer ${localStorage.getItem('accessToken')}`
                    }
                })
                .then((response) => {
                    MySwal.fire({
                        title: "Success",
                        text: `Advertisement deleted.`,
                        icon: "success",
                        confirmButtonText: "Ok",
                    })
                    .then(() => {
                        window.location.href = `/apartments/${id}`;
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
                        window.location.href = `/apartments/${id}`;
                    });
                });
            }
        });
    
    }

    async function handlePublish() {
        const { value: formValues } = await MySwal.fire({
            title: "Advertisement",
            html: `
                <input type="text" class="swal2-input" id="title" placeholder="Title"/>
                <input type="textarea" class="swal2-input" id="description" placeholder="Description"/>
                <input type="number" class="swal2-input" id="price" placeholder="Price"/>
            `,
            focusConfirm: false,
            preConfirm: () => {
                return [
                    (document.getElementById("title") as HTMLInputElement).value,
                    (document.getElementById("description") as HTMLInputElement).value,
                    (document.getElementById("price") as HTMLInputElement).value
                ]
            }
        })

        if (formValues) {
            RefreshAccessToken();

            axios.post(`${API_URL}/api/ads`, {
                title: formValues[0],
                description: formValues[1],
                price: Number(formValues[2]),
                apartmentId: id
            }, { 
                headers: {
                    Authorization: `Bearer ${localStorage.getItem('accessToken')}`
                }
            })
            .then((response) => {
                MySwal.fire({
                    title: "Success",
                    text: `Advertisement created.`,
                    icon: "success",
                    confirmButtonText: "Ok",
                })
                .then(() => {
                    window.location.href = `/apartments/${id}`;
                });
            })
            .catch((error) => {
                if (error.response.status === 409) {
                    MySwal.fire({
                        title: "Request failed",
                        text: `Apartment already has an advertisement.`,
                        icon: "error",
                        confirmButtonText: "Ok",
                    })
                    .then(() => {
                        window.location.href = `/apartments/${id}`;
                    });
                }
                else {
                    MySwal.fire({
                        title: "Request failed",
                        text: `Something went wrong.`,
                        icon: "error",
                        confirmButtonText: "Ok",
                    })
                    .then(() => {
                        window.location.href = `/apartments/${id}`;
                    });
                }
            });
        }
    }

    return (
    <div>
        <div className="h1 text-center">Apartment</div>
        <hr />
        <div className="container pb-2">
            <Link to="/apartments" className="btn btn-secondary">Back</Link>
        </div>
        <div className="container pb-5">
            <fieldset>
                <div className="input-group mb-3">
                    <span className="input-group-text">{">"}</span>
                    <div className="form-floating">
                        <input type="text" className="form-control" id="address" placeholder="Address" value={address} onChange={handleAddressChange} readOnly/>
                        <label htmlFor="address">Address</label>
                    </div>
                </div>
                <div className="input-group mb-3">
                    <span className="input-group-text">{">"}</span>
                    <div className="form-floating">
                        <input type="number" className="form-control" id="floor" placeholder="Floor" value={floor} onChange={handleFloorChange} readOnly/>
                        <label htmlFor="floor">Floor</label>
                    </div>
                </div>
                <div className="input-group mb-3">
                    <span className="input-group-text">{">"}</span>
                    <div className="form-floating">
                        <input type="number" className="form-control" id="number" placeholder="Number" value={number} onChange={handleNumberChange} readOnly/>
                        <label htmlFor="number">Number</label>
                    </div>
                </div>
                <div className="input-group mb-3">
                    <span className="input-group-text">{">"}</span>
                    <div className="form-floating">
                        <input type="number" className="form-control" id="area" placeholder="Area" value={area} onChange={handleAreaChange} readOnly/>
                        <label htmlFor="area">Area</label>
                    </div>
                </div>
                <div className="input-group mb-3">
                    <span className="input-group-text">{">"}</span>
                    <div className="form-floating">
                        <input type="number" className="form-control" id="rating" placeholder="Rating" value={rating} onChange={handleRatingChange} readOnly/>
                        <label htmlFor="rating">Rating</label>
                    </div>
                </div>
            </fieldset>
            <div className="aligned-right">
                <Button className="btn btn-primary" onClick={handlePublish}>Create ad</Button>
            </div>
            {
                adId !== -1 ? 
                <div className="aligned-right">
                    <Button className="btn btn-danger" value={adId} onClick={handleAdDelete}>Delete ad</Button>
                </div>
                : null
            }
            <hr />
            <div className="h1 text-center">Rooms</div>
            <hr />
            <div className="aligned-right">
                <Link className="btn btn-success" to={`/apartments/${id}/create-room`}>Create</Link>
            </div>
            <table className="table table-responsive">
                <thead>
                    <tr>
                        <td className="col-6">Name</td>
                        <td className="col-2">Grade</td>
                        <td className="col-4">Action</td>
                    </tr>
                </thead>
                <tbody>
                    {
                        rooms.map((room: RoomModel) => (
                            <Room key={room.id} {...room} />
                        ))
                    }
                </tbody>
            </table>
        </div>
    </div>)
}

export default ViewApartment;