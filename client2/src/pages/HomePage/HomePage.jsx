import './HomePage.css'
import React, { useState } from 'react';
import { TextField, MenuItem, Select, FormControl, InputLabel, Button, FormHelperText, Typography, Paper } from '@mui/material';
import { useNavigate } from 'react-router-dom'
import axiosInstance from '../../axiosConfig'

function HomePage(props) {
  const [colour, setColour] = useState('White');
  const [opponentUsername, setOpponentUsername] = useState('');
  const [startTime, setStartTime] = useState('10');
  const [increment, setIncrement] = useState('5');
  const [error, setError] = useState('');
  const navigate = useNavigate();

  const handleNewGameRequest = async (e) => {
    e.preventDefault();
    try {
      const response = await axiosInstance.post("/api/LiveChessGame/newGame", {
        colour: colour,
        opponentUsername: opponentUsername,
        startTime: startTime,
        increment: increment,
      })
      console.log(response)
      navigate('/play')
    } catch (err) {
      console.log(err.message)
      setError(JSON.stringify(err.response.data))
    }
  };

  if (props.isLoggedIn)
    return (
      <>
        <Paper sx={{ padding: '20px' }}>
          <Typography variant="h5" gutterBottom>
            Create New Game
          </Typography>
          <form onSubmit={handleNewGameRequest}>
            <FormControl fullWidth margin="dense" required>
              <InputLabel>Piece Colour</InputLabel>
              <Select
                value={colour}
                onChange={(e) => setColour(e.target.value)}
                label="Piece Colour"
              >
                <MenuItem value="White">White</MenuItem>
                <MenuItem value="Black">Black</MenuItem>
              </Select>
            </FormControl>

            <FormControl fullWidth margin="dense">
              <TextField
                label="Opponent Username"
                variant="outlined"
                value={opponentUsername}
                onChange={(e) => setOpponentUsername(e.target.value)}
                required
              />
            </FormControl>

            <Typography variant="h6" gutterBottom>
              Time Control
            </Typography>

            <FormControl fullWidth margin="dense" required>
              <TextField
                label="Start Time (minutes)"
                type="number"
                variant="outlined"
                value={startTime}
                onChange={(e) => setStartTime(e.target.value)}
              />
            </FormControl>

            <FormControl fullWidth margin="dense" required>
              <TextField
                label="Increment (seconds)"
                type="number"
                variant="outlined"
                value={increment}
                onChange={(e) => setIncrement(e.target.value)}
              />
            </FormControl>

            {error && (
              <FormHelperText error>{error}</FormHelperText>
            )}

            <Button variant="contained" color="primary" type="submit" fullWidth sx={{ marginTop: '20px' }}>
              Create Game
            </Button>
          </form>
        </Paper>
      </>
    )
  else {
    return (
      <h1 className="container">
        Welcome to Chess Arena! Please login to begin playing
      </h1 >
    )
  }
}

export default HomePage