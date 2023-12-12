import React from "react";
import Form from "react-bootstrap/Form";
import Button from 'react-bootstrap/Button';
import { API_URL } from "../../config";
import Swal from "sweetalert2";
import withReactContent from "sweetalert2-react-content";
import axios from "axios";
const MySwal = withReactContent(Swal);

export const Register = () => {
  const [username, setUsername] = React.useState("");
  const [password, setPassword] = React.useState("");
  const [email, setEmail] = React.useState("");

  const handleUsernameChange = (event : any) => {
    setUsername(event.target.value);
  };

  const handlePasswordChange = (event : any) => {
    setPassword(event.target.value);
  };

  const handleEmailChange = (event : any) => {
    setEmail(event.target.value);
  };

  const handleRegister = () => {
    axios
      .post(`${API_URL}/api/register`, {
        username: username,
        password: password,
        email: email,
      }).then((response) => {
        MySwal.fire({
          title: "Success!",
          text: "You have successfully registered.",
          icon: "success",
          confirmButtonText: "Ok",
        }).then(() => {
          window.location.href = "/login";
        })
      }).catch((error) => {
        MySwal.fire({
          title: "Registration failed",
          text: "Something went wrong.",
          icon: "error",
          confirmButtonText: "Ok",
          });
      });
  };

  return <div>
    <div className="h1 text-center">Registration</div>
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
        <Form.Group className="mb-3" controlId="email">
          <Form.Label>Email</Form.Label>
          <Form.Control 
            type="email" 
            placeholder="Email"
            value={email}
            onChange={handleEmailChange}
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
      <Button variant="primary" onClick={handleRegister}>
        Submit
      </Button>
      </Form>
  </div>;
};