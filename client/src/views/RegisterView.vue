<template>
    <div class="register">
        <h2>Register</h2>
        <form @submit.prevent="handleRegister">
            <div>
                <label for="username">Username:</label>
                <input type="text" id="username" v-model="username" required>
            </div>
            <div>
                <label for="email">Email:</label>
                <input type="email" id="email" v-model="email" required>
            </div>
            <div>
                <label for="password">Password:</label>
                <input type="password" id="password" v-model="password" required>
            </div>
            <div>
                <label for="confirmPassword">Confirm Password:</label>
                <input type="password" id="confirmPassword" v-model="confirmPassword" required>
            </div>
            <div v-if="errorMessage" class="error-message">{{ errorMessage }}</div>
            <button type="submit">Register</button>
        </form>
    </div>
</template>
  
<script>
import axios from 'axios'
export default {
    data() {
        return {
            username: '',
            email: '',
            password: '',
            confirmPassword: '',
            errorMessage: ''
        }
    },
    methods: {
        async handleRegister() {
            if (this.password !== this.confirmPassword) {
                this.errorMessage = "Passwords do not match"
            } else {
                try {
                    const response = await axios.post("/Auth/register", {
                        username: this.username,
                        email: this.email,
                        password: this.password
                    })
                    console.log(response)
                    this.$router.push('/login')
                } catch (err) {
                    console.log(err)
                    this.errorMessage = err.response.data;
                }
            }
        }
    }
}
</script>
  
<style scoped>
.register {
    max-width: 400px;
    margin: 0 auto;
    padding: 1em;
    border: 1px solid #ccc;
    border-radius: 4px;
}

.register div {
    margin-bottom: 1em;
}

.register label {
    margin-bottom: .5em;
    color: #333333;
    display: block;
}

.register input {
    border: 1px solid #CCCCCC;
    padding: .5em;
    font-size: 1em;
    width: 100%;
    box-sizing: border-box;
}

.register button {
    padding: 0.7em;
    color: #fff;
    background-color: #007BFF;
    border: none;
    border-radius: 4px;
    cursor: pointer;
}

.register button:hover {
    background-color: #0056b3;
}

.error-message {
    color: #ff0000;
    margin-top: 0.5em;
}
</style>