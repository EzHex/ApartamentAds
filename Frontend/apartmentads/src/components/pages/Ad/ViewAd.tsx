import React, { useEffect } from "react";
import AdModel from "../../../type/AdModel";
import { Link, useParams } from "react-router-dom";
import Swal from "sweetalert2";
import withReactContent from "sweetalert2-react-content";
import RefreshAccessToken from "../../Auth";
import axios from "axios";
import { API_URL } from "../../../config";

const MySwal = withReactContent(Swal);

function ViewAd() {
    const adId = useParams().id;

    const [ad, setAd] = React.useState<AdModel>(
        {
            id : 0,
            title : "",
            description : "",
            date : "",
            price : 0,
        }
    );

    useEffect(() => {
        if (localStorage.getItem('refreshToken'))
        {
            RefreshAccessToken();
            axios.get(`${API_URL}/api/ads/${adId}`, {
                headers: {
                    Authorization: `Bearer ${localStorage.getItem('accessToken')}`
                }
            })
            .then((response) => {
                setAd(response.data);
            })
            .catch((error) => {
                MySwal.fire({
                    title: "Request failed",
                    text: `Something went wrong.`,
                    icon: "error",
                    confirmButtonText: "Ok",
                })
                .then(() => {
                    window.location.href = `/ads`;
                })
            });
        }
        else
        {
            axios.get(`${API_URL}/api/ads/${adId}`)
            .then((response) => {
                setAd(response.data);
            })
            .catch((error) => {
                MySwal.fire({
                    title: "Request failed",
                    text: `Something went wrong.`,
                    icon: "error",
                    confirmButtonText: "Ok",
                })
                .then(() => {
                    window.location.href = `/ads`;
                })
            });
        }
    }, []);


    return (
        <div>
            <div className="h1 text-center">Ad</div>
            <hr />
            <div className="container pb-2">
                <Link to="/ads" className="btn btn-secondary">Back</Link>
            </div>
            <div className="container pb-5">
                <fieldset>
                    <div className="input-group mb-3">
                        <span className="input-group-text">{">"}</span>
                        <div className="form-floating">
                            <input type="text" className="form-control" id="date" placeholder="Date" value={new Date(ad.date).toLocaleDateString('lt-LT')}  readOnly/>
                            <label htmlFor="name">Date</label>
                        </div>
                    </div>
                    <div className="input-group mb-3">
                        <span className="input-group-text">{">"}</span>
                        <div className="form-floating">
                            <input type="text" className="form-control" id="title" placeholder="Title" value={ad.title} readOnly />
                            <label htmlFor="name">Title</label>
                        </div>
                    </div>
                    <div className="input-group mb-3">
                        <span className="input-group-text">{">"}</span>
                        <div className="form-floating">
                            <input type="text" className="form-control" id="description" placeholder="Description" value={ad.description} readOnly />
                            <label htmlFor="name">Description</label>
                        </div>
                    </div>
                    <div className="input-group mb-3">
                        <span className="input-group-text">{">"}</span>
                        <div className="form-floating">
                            <input type="text" className="form-control" id="price" placeholder="Price" value={ad.price} readOnly />
                            <label htmlFor="name">Price</label>
                        </div>
                    </div>
                    {
                        ad.email !== undefined && ad.email !== null && ad.email !== "" ? (
                            <div className="input-group mb-3">
                                <span className="input-group-text">{">"}</span>
                                <div className="form-floating">
                                    <input type="text" className="form-control" id="email" placeholder="Email" value={ad.email} readOnly />
                                    <label htmlFor="name">Owner email</label>
                                </div>
                            </div>
                        ) : (
                            <div className="input-group mb-3">
                                <span className="input-group-text">{">"}</span>
                                <div className="form-floating">
                                    <input type="text" className="form-control text-danger" id="notLoggedIn" placeholder="Email" value="Login to access" readOnly />
                                    <label htmlFor="name">Owner email</label>
                                </div>
                            </div>
                        )
                    }
                </fieldset>
            </div>
        </div>
    );
}

export default ViewAd;
