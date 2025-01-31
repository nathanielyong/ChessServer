<template>
  <div class="login">
    <h2>Login</h2>
    <form @submit.prevent="handleLogin">
      <div>
        <label for="username">Username:</label>
        <input type="text" id="username" v-model="username" required>
      </div>
      <div>
        <label for="password">Password:</label>
        <input type="password" id="password" v-model="password" required>
      </div>
      <div v-if="errorMessage" class="error-message">{{ errorMessage }}</div>
      <button type="submit">Login</button>
    </form>
  </div>
</template>

<script>
import axios from 'axios'
import { mapActions } from 'vuex'

export default {
  data() {
    return {
      username: '',
      password: '',
      errorMessage: ''
    }
  },
  methods: {
    ...mapActions(['login']),
    async handleLogin() {
      try {
        const response = await axios.post("/api/Auth/login", {
          username: this.username,
          password: this.password
        })
        console.log(response)
        const token = response.data
        this.login(token)
        console.log(token)
        this.$router.push('/')
      } catch (err) {
        console.log(err)
        this.errorMessage = err.response.data;
      }
    }
  }
}
</script>

<style>
.login {
  max-width: 400px;
  margin: 0 auto;
  padding: 1em;
  border: 1px solid #ccc;
  border-radius: 4px;
}

.login div {
  margin-bottom: 1em;
}

.login label {
  margin-bottom: .5em;
  color: #333333;
  display: block;
}

.login input {
  border: 1px solid #CCCCCC;
  padding: .5em;
  font-size: 1em;
  width: 100%;
  box-sizing: border-box;
}

.login button {
  padding: 0.7em;
  color: #fff;
  background-color: #007BFF;
  border: none;
  border-radius: 4px;
  cursor: pointer;
}

.login button:hover {
  background-color: #0056b3;
}

.error-message {
  color: #ff0000;
  margin-top: 0.5em;
}
</style>