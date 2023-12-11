import axios from "axios";
import React from "react";
import { API_URL } from "../config";
import Swal from "sweetalert2";
import withReactContent from "sweetalert2-react-content";

const MySwal = withReactContent(Swal);

function RefreshAccessToken() {
    axios.post(`${API_URL}/api/accessToken`,
            {
                refreshToken: localStorage.getItem('refreshToken')
            })
            .then((response) => {
                localStorage.setItem("accessToken", response.data.accessToken);
                localStorage.setItem("refreshToken", response.data.refreshToken);
            })
            .catch((error) => {
                MySwal.fire({
                    title: "Request failed",
                    text: "Please login again.",
                    icon: "error",
                    confirmButtonText: "Ok",
                }).then(() => {
                    window.location.href = "/login";
                });
            });
}

export default RefreshAccessToken;