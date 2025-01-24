import { useState, useEffect } from 'react'
import './App.css'
import Navbar from './components/Navbar/Navbar'
import LoginPage from './pages/LoginPage/LoginPage'
import RegisterPage from './pages/RegisterPage/RegisterPage'
import HomePage from './pages/HomePage/HomePage'
import PlayPage from './pages/PlayPage/PlayPage'
import { BrowserRouter, Route, Routes } from "react-router-dom"

function App() {
  const [isLoggedIn, setIsLoggedIn] = useState(false);

  useEffect(() => {
    setIsLoggedIn(localStorage.getItem('jwtToken') !== null);
    window.addEventListener('login', (event) => {
      setIsLoggedIn(true);
    })
  }, []);

  return (
    <BrowserRouter>
      <Navbar isLoggedIn={isLoggedIn} />
      <div className="content-container">
        <Routes >
          <Route path="/" element={<HomePage isLoggedIn={isLoggedIn} />} />
          <Route path="/login" element={<LoginPage />} />
          <Route path="/register" element={<RegisterPage />} />
          <Route path="/play" element={<PlayPage />} />
        </Routes>
      </div>
    </BrowserRouter>
  )
}

export default App
