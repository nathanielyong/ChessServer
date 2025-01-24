<template>
    <div class="container">
        <h1>User Profile</h1>
        <div class="profile">
            <div>Username: {{ this.username }}</div>
            <div>Email: {{ this.email }}</div>
            <div>Dated Joined: {{ this.dateJoined.split('.')[0].replace('T', ' ') }}</div>
            <div>Rating: {{ this.rating }}</div>
            <div>Status: {{ isPlaying ? "Currently in game" : "Not in game" }}</div>
            <div>Current Game ID: {{ this.currentGameId ? currentGameId : "" }}</div>
            <div>Total Games Played: {{ this.numGamesPlayed }}</div>
            <div>Wins: {{ this.wins }}</div>
            <div>Losses: {{ this.losses }}</div>
            <div>Draws: {{ this.draws }}</div>
        </div>
        <h2>Chess Games of "{{ this.username }}"</h2>
        <table class="striped-table">
            <thead>
                <tr>
                    <th>ID</th>
                    <th>White Player</th>
                    <th>Black Player</th>
                    <th>Result</th>
                    <th>Time Control</th>
                    <th>PGN</th>
                    <th>Moves</th>
                    <th>Date Started</th>
                    <th>Date Finished</th>
                </tr>
            </thead>
            <tbody>
                <tr class="game" v-for="(game, index) in games" :key="index">
                    <td>{{ game.id }}</td>
                    <td>{{ game.whitePlayerUsername }}</td>
                    <td>{{ game.blackPlayerUsername }}</td>
                    <td :class="{ 'text-green': (this.username === game.whitePlayerUsername && game.result === '1-0' || this.username === game.blackPlayerUsername && game.result === '0-1'),
                                'text-red': (this.username === game.whitePlayerUsername && game.result === '0-1' || this.username === game.blackPlayerUsername && game.result === '1-0') }">{{ game.gameEndReason + " " + game.result }}</td>
                    <td>{{ game.startTime + " + " + game.increment }}</td>
                    <td>{{ game.pgn }}</td>
                    <td>{{ game.moves }}</td>
                    <td>{{ game.dateStarted.split('.')[0].replace('T', ' ') }}</td>
                    <td>{{ game.dateFinished.split('.')[0].replace('T', ' ') }}</td>
                </tr>
            </tbody>
        </table>
    </div>
</template>
<script>
import axiosInstance from "@/axiosConfig";
export default {
    data() {
        return {
            username: "",
            email: "",
            dateJoined: "",
            isPlaying: false,
            currentGameId: null,
            rating: null,
            numGamesPlayed: 0,
            wins: 0,
            draws: 0,
            losses: 0,
            games: {}
        }
    },
    mounted() {
        this.fetchProfile()
        this.fetchGames()
    },
    methods: {
        async fetchProfile() {
            try {
                const response = await axiosInstance.get(
                    "/api/User/profile"
                )
                console.log(response)
                const data = response.data
                this.username = data.username
                this.email = data.email
                this.isPlaying = data.isPlaying
                this.dateJoined = data.datedJoined
                this.currentGameId = data.liveChessGameId
                this.rating = data.rating
                this.numGamesPlayed = data.numGamesPlayed
                this.wins = data.wins
                this.losses = data.losses
                this.draws = data.draws
            } catch (err) {
                this.error = err;
            }
        },
        async fetchGames() {
            try {
                const response = await axiosInstance.get(
                    "/api/ChessGame/myGames"
                )
                console.log(response)
                this.games = response.data
            } catch (err) {
                this.error = err;
            }
        }
    }
}
</script>
<style>
.container {
    display: flex;
    flex-direction: column;
    align-items: center;
}

.profile {
    width: 400px;
    border: 1px solid black;
    border-radius: 5px;
    padding: 10px;
    display: flex;
    flex-direction: column;
    align-items: flex-start;
    gap: 5px;
}

.game {
    list-style-type: none;
}

.game td {
    max-width: 250px;
}

.striped-table {
    border-collapse: collapse;
}

.striped-table th,
.striped-table td {
    border: 1px solid #ddd;
    padding: 8px;
}

.striped-table tr:nth-child(even) {
    background-color: #f2f2f2;
}

.striped-table tr:hover {
    background-color: #ddd;
}
.text-green {
  color: green;
}

.text-red {
  color: red;
}
</style>