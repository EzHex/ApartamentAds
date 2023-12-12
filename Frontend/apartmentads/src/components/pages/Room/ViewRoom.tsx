import React, { useEffect } from "react";
import { Button } from "react-bootstrap";
import { Link, useParams } from "react-router-dom";
import RefreshAccessToken from "../../Auth";
import axios from "axios";
import { API_URL } from "../../../config";
import withReactContent from "sweetalert2-react-content";
import Swal from "sweetalert2";
import ObjectModel from "../../../type/ObjectModel";
import Object from "../Object/Object";

const MySwal = withReactContent(Swal);

function ViewRoom() {
    const apartmentId = useParams().apartmentId;
    const roomId = useParams().id;

    const [objects, setObjects] = React.useState<ObjectModel[]>([]);

    const [name, setName] = React.useState("");
    const [grade, setGrade] = React.useState(0);

    const handleNameChange = (event: any) => {
        setName(event.target.value);
    };

    const handleGradeChange = (event: any) => {
        setGrade(event.target.value);
    };

    function handleEdit() {
        RefreshAccessToken();

        axios.put(`${API_URL}/api/apartments/${apartmentId}/rooms/${roomId}`, {
            name: name,
            grade: grade
        }, {
            headers: {
                Authorization: `Bearer ${localStorage.getItem('accessToken')}`
            } 
        })
        .then((response) => {
            MySwal.fire({
                title: "Success",
                text: `Room edited successfully.`,
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
                window.location.href = "/";
            });
        })
    }

    useEffect(() => {
        RefreshAccessToken();

        axios.get(`${API_URL}/api/apartments/${apartmentId}/rooms/${roomId}`, {
            headers: {
                Authorization: `Bearer ${localStorage.getItem('accessToken')}`
            } 
        })
        .then((response) => {
            setName(response.data.name);
            setGrade(response.data.grade);
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
        })

        axios.get(`${API_URL}/api/apartments/${apartmentId}/rooms/${roomId}/objects`, {
            headers: {
                Authorization: `Bearer ${localStorage.getItem('accessToken')}`
            } 
        }).then((response) => {
            const updatedObjects = response.data.map((object: ObjectModel) => {
                return {
                    ...object,
                    apartmentId: Number(apartmentId),
                    roomId: Number(roomId)
                };
            });
            setObjects(updatedObjects);
        })

    }, [])


    return (
    <div>
        <div className="h1 text-center">Room</div>
        <hr />
        <div className="container pb-2">
            <Link to={`/apartments/${apartmentId}`} className="btn btn-secondary">Back</Link>
        </div>
        <div className="container pb-5">
            <fieldset>
                <div className="input-group mb-3">
                    <span className="input-group-text">{">"}</span>
                    <div className="form-floating">
                        <input type="text" className="form-control" id="name" placeholder="Address" value={name} onChange={handleNameChange} />
                        <label htmlFor="address">Name</label>
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
            <div className="h1 text-center">Objects</div>
            <hr />
            <div className="aligned-right">
                <Link className="btn btn-success" to={`/apartments/${apartmentId}/rooms/${roomId}/create-object`}>Create</Link>
            </div>
            <table className="table table-responsive">
                <thead>
                    <tr>
                        <th className="col-2">Name</th>
                        <th className="col-3">Description</th>
                        <th className="col-4">Image</th>
                        <th className="col-1">Grade</th>
                        <th className="col-2">Actions</th>
                    </tr>
                </thead>
                <tbody>
                    {
                        objects.map((object: ObjectModel) => (
                            <Object key={object.id} {...object} />
                        ))  
                    }
                </tbody>
            </table>
        </div>
    </div>)
}

export default ViewRoom;