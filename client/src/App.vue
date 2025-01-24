<template>
  <nav>
    <router-link to="/home">Home</router-link> |
    <router-link to="/play">Play</router-link> |
    <template v-if="isLoggedIn">
      <router-link to="/profile">Profile</router-link> |
      <router-link to="/logout">Logout</router-link>
    </template>
    <template v-else>
      <router-link to="/login">Login</router-link> |
      <router-link to="/register">Register</router-link>
    </template>
  </nav>
  <router-view />
</template>
<script>
export default {
  data() {
    return {
      isLoggedIn: false
    }
  },
  mounted() {
    this.isLoggedIn = localStorage.getItem('jwtToken') != null
    window.addEventListener('login', (event) => {
      this.isLoggedIn = event.detail.storage != null;
    })
  },
}
</script>
<style>
#app {
  font-family: Avenir, Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  text-align: center;
  color: #2c3e50;
}

nav {
  padding: 30px;
}

nav a {
  font-weight: bold;
  color: #2c3e50;
}

nav a.router-link-exact-active {
  color: #42b983;
}
</style>
