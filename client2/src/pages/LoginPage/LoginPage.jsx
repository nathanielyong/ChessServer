import { useState } from 'react'
import axios from 'axios'
import { TextField, Button, FormLabel, Paper, FormControl } from '@mui/material';
import './LoginPage.css'
import { useNavigate } from 'react-router-dom';

function LoginPage() {
    const [username, setUsername] = useState('');
    const [password, setPassword] = useState('');
    const [error, setError] = useState('');
    const navigate = useNavigate();

    const handleLogin = async (e) => {
        e.preventDefault();
        try {
            const response = await axios.post("/Auth/login", {
                username: username,
                password: password
            })
            console.log(response)
            const token = response.data
            localStorage.setItem('jwtToken', token)
            window.dispatchEvent(new CustomEvent('login', {
                detail: {
                    storage: localStorage.getItem('jwtToken')
                }
            }));
            navigate('/')
        } catch (err) {
            console.log(err)
            setError(JSON.stringify(err.response.data));
        }
    }

    return (
        <div className="login-container">
            <h1>Login</h1>
            <Paper elevation={5} sx={{ padding: '50px 70px' }}>
                <form onSubmit={e => handleLogin(e)} id="login-form">
                    <FormControl>
                        <FormLabel>Username</FormLabel>
                        <TextField required variant="standard" onChange={e => setUsername(e.target.value)} onFocus={() => setError('')} />
                    </FormControl>
                    <FormControl>
                        <FormLabel>Password</FormLabel>
                        <TextField required variant="standard" type="password" onChange={e => setPassword(e.target.value)} onFocus={() => setError('')} />
                    </FormControl>
                    <Button color="secondary" variant="contained" type="submit" htmlFor="login-form" className="login-button" size="large">Login</Button>
                    {error && <div style={{ color: 'red' }}>{error}</div>}
                </form>
            </Paper>
        </div>
    )
}

export default LoginPage