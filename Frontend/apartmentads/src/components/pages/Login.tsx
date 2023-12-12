import React, { useEffect } from "react";
import { API_URL } from "../../config";
import Form from "react-bootstrap/Form";
import Button from 'react-bootstrap/Button';
import axios from "axios";
import Swal from "sweetalert2";
import withReactContent from "sweetalert2-react-content";

const MySwal = withReactContent(Swal);

export const Login = () => {
  const [username, setUsername] = React.useState("");
  const [password, setPassword] = React.useState("");
  
  const handleUsernameChange = (event : any) => {
    setUsername(event.target.value);
  };

  const handlePasswordChange = (event : any) => {
    setPassword(event.target.value);
  };

  const handleLogin = () => {
    axios
      .post(`${API_URL}/api/login`, {
        username: username,
        password: password,
      })
      .then((response) => {
        localStorage.setItem("accessToken", response.data.accessToken);
        localStorage.setItem("refreshToken", response.data.refreshToken);
        
        MySwal.fire({
          title: "Success!",
          text: "You have successfully logged in.",
          icon: "success",
          confirmButtonText: "Ok",
        }).then(() => {
          window.location.href = "/";
        });
      })
      .catch((error) => {
        MySwal.fire({
          title: "Login failed",
          text: "Something went wrong.",
          icon: "error",
          confirmButtonText: "Ok",
        });
      });
  };

  return <div>
      <div className="h1 text-center">Login</div>
      <Form className="container pb-5">
        <Form.Group className="mb-3" controlId="username">
          <Form.Label>Username</Form.Label>
          <Form.Control 
            type="text" 
            placeholder="Username"
            value={username}
            onChange={handleUsernameChange}
          />
        </Form.Group>
        
        <Form.Group className="mb-3" controlId="formBasicPassword">
          <Form.Label>Password</Form.Label>
          <Form.Control 
            type="password"
            placeholder="Password"
            value={password}
            onChange={handlePasswordChange}
          />
        </Form.Group>
        <Button variant="primary" onClick={handleLogin}>
          Submit
        </Button>
      </Form>
      </div>;
};