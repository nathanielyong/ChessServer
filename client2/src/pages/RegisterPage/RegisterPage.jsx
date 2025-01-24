import { TextField, Button, FormLabel, Paper, FormControl } from '@mui/material';
import './RegisterPage.css';
import { useState } from 'react';
import axios from 'axios';
import { useNavigate } from 'react-router-dom';

function RegisterPage() {
  const [username, setUsername] = useState('');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [confirmPassword, setConfirmPassword] = useState('');
  const [error, setError] = useState('');
  const navigate = useNavigate();

  const handleRegister = async (e) => {
    e.preventDefault();
    if (password !== confirmPassword) {
      setError("Passwords do not match")
    } else {
      try {
        const response = await axios.post("/Auth/register", {
          username: username,
          email: email,
          password: password
        })
        console.log(response)
        navigate('/login')
      } catch (err) {
        console.log(err)
        setError(JSON.stringify(err.response.data));
      }
    }
  }

  return (
    <>
      <div className="signup-container">
        <h1>Sign Up with Email</h1>
        <Paper elevation={5} sx={{ padding: '50px 70px' }}>
          <form onSubmit={e => handleRegister(e)} id="signup-form">
            <FormControl>
              <FormLabel>Username</FormLabel>
              <TextField required variant="standard" id="username-field"
                onChange={e => setUsername(e.target.value)}
                sx={{ width: "200px" }}
              />
            </FormControl>
            <FormControl>
              <FormLabel>Email Address</FormLabel>
              <TextField required variant="standard" onChange={e => setEmail(e.target.value)}
                sx={{ width: '200px' }}
              />
            </FormControl>
            <FormControl>
              <FormLabel>Password</FormLabel>
              <TextField required variant="standard" type="password"
                onChange={e => setPassword(e.target.value)} sx={{ width: "200px" }} />
            </FormControl>
            <FormControl>
              <FormLabel>Confirm Password</FormLabel>
              <TextField required variant="standard" type="password"
                onChange={e => setConfirmPassword(e.target.value)} sx={{ width: "200px" }} />
            </FormControl>
            <Button color="secondary" variant="contained" type="submit" htmlFor="signup-form" className="signup-button" size="large">Sign Up</Button>
            {error && <div style={{color: 'red'}}>{error}</div>}
          </form>
        </Paper>
      </div>
    </>
  )
}

export default RegisterPage