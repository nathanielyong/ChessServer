import { AppBar, Toolbar, IconButton, Button, Tooltip, Avatar, Box, Divider, Menu, MenuItem, Typography } from '@mui/material'
import { Home } from '@mui/icons-material'
import { Link } from 'react-router-dom'
import { useState } from 'react'
import './Navbar.css'

function Navbar(props) {

  const handleLogout = async () => {
    setAnchorElUser(null);
  };

  const [anchorElUser, setAnchorElUser] = useState(null);

  const handleOpenUserMenu = (event) => {
    setAnchorElUser(event.currentTarget);
  };

  const handleCloseUserMenu = () => {
    setAnchorElUser(null);
  };

  return (
    <>
      <AppBar position="fixed">
        {props.isLoggedIn ?
          <Toolbar className="navbar">
            <IconButton component={Link} to={"/"} className="button">
              <Home />
            </IconButton>
            <Button component={Link} to={"/play"} className="button" color="inherit">
              Play
            </Button>
            <Box style={{ marginLeft: "auto" }} sx={{ flexGrow: 0 }}>
              <Tooltip title="Open settings">
                <IconButton onClick={handleOpenUserMenu} sx={{ p: 0 }}>
                  <Avatar></Avatar>
                </IconButton>
              </Tooltip>
              <Menu
                sx={{ mt: '45px' }}
                id="menu-appbar"
                anchorEl={anchorElUser}
                anchorOrigin={{
                  vertical: 'top',
                  horizontal: 'right',
                }}
                keepMounted
                transformOrigin={{
                  vertical: 'top',
                  horizontal: 'right',
                }}
                open={Boolean(anchorElUser)}
                onClose={handleCloseUserMenu}
              >
                <Typography sx={{ padding: '4px 16px', fontWeight: 'bold' }}>
                  {props.name}
                </Typography>
                <Divider />
                <MenuItem key="Profile" onClick={handleCloseUserMenu}>
                  <Typography sx={{ textAlign: 'center' }}>Profile</Typography>
                </MenuItem>
                <MenuItem key="Settings" onClick={handleCloseUserMenu}>
                  <Typography sx={{ textAlign: 'center' }}>Settings</Typography>
                </MenuItem>
                <MenuItem key="Logout" onClick={handleLogout}>
                  <Typography sx={{ textAlign: 'center' }}>Logout</Typography>
                </MenuItem>
              </Menu>
            </Box>
          </Toolbar>
          :
          <Toolbar className="navbar">
            <IconButton component={Link} to={"/"} className="button">
              <Home />
            </IconButton>
            <Button component={Link} to={"/login"} className="button" color="inherit">
              Login
            </Button>
            <Button component={Link} to={"/register"} className="button" color="inherit">
              Register
            </Button>
          </Toolbar>
        }
      </AppBar >
    </>
  )
}

export default Navbar
