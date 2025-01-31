<template>
  <div class="request-form">
    <h2>Create New Challenge</h2>
    <form @submit.prevent="handleNewGameRequest">
      <div class="form-group">
        <label for="colour">Piece Colour</label>
        <select v-model="colour" required>
          <option value="White">White</option>
          <option value="Black">Black</option>
        </select>
      </div>
      <div class="form-group">
        <label>Opponent Username</label>
        <input type="text" v-model="opponentUsername">
      </div>
      <h3>Time Control</h3>
      <div class="form-group">
        <label>Start Time (minutes)</label>
        <input type="number" v-model="startTime">
      </div>
      <div class="form-group">
        <label>Increment (seconds)</label>
        <input type="number" v-model="increment">
      </div>
      <div v-if="error" class="error-message">{{ error }}</div>
      <button type="submit">Send Challenge</button>
    </form>
  </div>
</template>
<script>
import axiosInstance from '@/axiosConfig'

export default {
  data() {
    return {
      colour: '',
      opponentUsername: '',
      startTime: 10,
      increment: 0,
      error: ''
    }
  },
  methods: {
    async handleNewGameRequest() {
      try {
        const response = await axiosInstance.post("/api/LiveChessGame/newGame", {
          colour: this.colour,
          opponentUsername: this.opponentUsername,
          startTime: this.startTime,
          increment: this.increment,
        })
        console.log(response)
        this.$router.push('/play')
      } catch (err) {
        console.log(err.message)
        this.error = err.response.data
      }
    }
  }
}
</script>
<style>
.request-form {
  max-width: 400px;
  margin: 0 auto;
  padding: 1em;
  border: 1px solid #ccc;
  border-radius: 5px;
}

.form-group {
  margin-bottom: 1em;
  display: flex;
  align-items: center;
}

.request-form select {
  flex: 1;
  padding: 0.5em;
}

.request-form label {
  margin-right: .5em;
  color: #333333;
  display: block;
}

.request-form input[type="text"] {
  border: 1px solid #bbbbbb;
  padding: .5em;
  font-size: 1em;
  flex: 1;
  box-sizing: border-box;
  border-radius: 5px;
}

.request-form button {
  padding: 0.7em;
  color: #fff;
  background-color: #007BFF;
  border: none;
  border-radius: 5px;
  cursor: pointer;
}

.request-form button:hover {
  background-color: #0056b3;
}

input[type="number"] {
  width: 50%;
  padding: 0.5em;
  border-radius: 5px;
}

.error-message {
  color: #ff0000;
  margin-top: 0.5em;
}
</style>