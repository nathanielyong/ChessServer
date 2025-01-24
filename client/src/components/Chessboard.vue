<template>
  <div>
    <div class="chess-container">
      <TheChessboard :style="{ margin: '0' }" @move="makeMove" :player-color="playerColor" :board-config="boardConfig"
        @board-created="onBoardCreated" reactive-config>
      </TheChessboard>
      <div class="timers">
        <div>{{ this.isPlayerWhite ? this.blackPlayerUsername : this.whitePlayerUsername }}</div>
        <div class="timer">{{ this.isPlayerWhite ? formattedBlackTime : formattedWhiteTime }}</div>
        <div class="info">
          <div>{{ this.result !== "" ? this.gameEndReason + " " + this.result : "" }}</div>
          <button class="resign" @click="resignGame">Resign</button>
        </div>
        <div class="timer">{{ this.isPlayerWhite ? formattedWhiteTime : formattedBlackTime }}</div>
        <div>{{ this.isPlayerWhite ? this.whitePlayerUsername : this.blackPlayerUsername }}</div>
      </div>
    </div>
  </div>
</template>
<script>
import { reactive } from "vue";
import { TheChessboard } from "vue3-chessboard";
import "vue3-chessboard/style.css";
import axiosInstance from "@/axiosConfig";
import * as signalR from '@microsoft/signalr';
import { jwtDecode } from "jwt-decode";

export default {
  data() {
    return {
      username: "",
      boardAPI: null,
      connection: null,
      liveChessGameId: null,
      intervalId: null,
      intervalId2: null,
      whitePlayerUsername: "",
      blackPlayerUsername: "",
      isPlayerWhite: true,
      isWhiteTurn: true,
      whiteTime: 0,
      blackTime: 0,
      moveCount: 0,
      result: "",
      gameEndReason: "",
      error: "",
      boardConfig: reactive({
        coordinates: true
      }),
      playerColor: '',
    };
  },
  computed: {
    formattedWhiteTime() {
      const minutes = Math.max(0, Math.floor(this.whiteTime / 60000)).toString().padStart(2, '0')
      const seconds = Math.max(0, Math.floor(this.whiteTime % 60000 / 1000)).toString().padStart(2, '0')
      return `${minutes}:${seconds}`
    },
    formattedBlackTime() {
      const minutes = Math.max(0, Math.floor(this.blackTime / 60000)).toString().padStart(2, '0')
      const seconds = Math.max(0, Math.floor(this.blackTime % 60000 / 1000)).toString().padStart(2, '0')
      return `${minutes}:${seconds}`
    }
  },
  async mounted() {
    const token = localStorage.getItem("jwtToken");
    if (token) {
      const decodedToken = jwtDecode(token);
      this.username = decodedToken["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"];
    }

    await this.fetchGame()

    if (this.liveChessGameId) {
      this.startTimer()

      this.connection = new signalR.HubConnectionBuilder()
        .withUrl("/chess", {
          accessTokenFactory: () => token
        })
        .withAutomaticReconnect()
        .configureLogging(signalR.LogLevel.Information)
        .build();

      this.connection.on('JoinGame', (message) => {
        console.log(message)
      })

      this.connection.on('UserDisconnected', (message) => {
        console.log(message)
      })

      this.connection.on('MakeMoveError', (message) => {
        console.log(message)
      })

      this.connection.on('MakeMove', (response) => {
        console.log("Move played", response)
        const gameState = response.gameState;
        console.log(gameState)
        console.log("boardAPI: ", this.boardAPI)
        console.log("new move", gameState.prevMove)
        // If it's not player's move and MakeMove fired, then opponent made move.
        if ((this.isWhiteTurn && !this.isPlayerWhite) || (!this.isWhiteTurn && this.isPlayerWhite)) {
          this.boardAPI.move(gameState.prevMove)
        }
        this.isWhiteTurn = gameState.isWhiteTurn
        this.whiteTime = gameState.whiteTimeRemaining
        this.blackTime = gameState.blackTimeRemaining
        this.moveCount = gameState.moveCount
        this.result = gameState.result
        this.gameEndReason = gameState.gameEndReason
        this.boardConfig.viewOnly = this.result !== ""
      })

      if (this.connection.state === signalR.HubConnectionState.Disconnected) {
        try {
          await this.connection.start();
          console.log("Connected to SignalR");
          await this.connection.invoke('JoinGame', this.liveChessGameId.toString());
        } catch (err) {
          console.error(err);
        }
      } else {
        console.log("Already connected to SignalR")
      }
    } else {
      console.log("No live chess game found. Make a challenge with someone to start playing!")
    }
  },
  methods: {
    startTimer() {
      if (this.intervalId) {
        clearInterval(this.intervalId)
      }
      this.intervalId = setInterval(() => {
        if (this.moveCount > 1 && this.result === "") {
          if (this.isWhiteTurn) {
            if (this.whiteTime > 0) {
              this.whiteTime -= 1000
            } else {
              clearInterval(this.intervalId)
            }
          } else {
            if (this.blackTime > 0) {
              this.blackTime -= 1000
            } else {
              clearInterval(this.intervalId)
            }
          }
        }
      }, 1000)
    },
    async fetchGame() {
      try {
        const response = await axiosInstance.get(
          "/api/LiveChessGame/getCurrentGame"
        );
        console.log(response);
        const gameState = response.data.gameState;
        this.liveChessGameId = gameState.id
        this.whitePlayerUsername = gameState.whitePlayerUsername
        this.blackPlayerUsername = gameState.blackPlayerUsername
        this.isWhiteTurn = gameState.isWhiteTurn
        this.isPlayerWhite = this.username === this.whitePlayerUsername
        this.whiteTime = gameState.whiteTimeRemaining
        this.blackTime = gameState.blackTimeRemaining
        this.moveCount = gameState.moveCount
        this.result = gameState.result
        this.gameEndReason = gameState.gameEndReason
        this.boardConfig.fen = gameState.currentPositionFEN
        this.boardConfig.orientation = this.isPlayerWhite ? 'white' : 'black'
        this.boardConfig.viewOnly = this.result !== ""
        this.playerColor = this.isPlayerWhite ? 'white' : 'black'
      } catch (err) {
        this.error = err;
      }
    },
    async makeMove(move) {
      const moveString = move.from + '-' + move.to;
      console.log(moveString)
      try {
        await this.connection.invoke('MakeMove', moveString)
      } catch (err) {
        this.error = err;
      }
    },
    async resignGame() {
      try {
        const response = await axiosInstance.post(
          "/api/LiveChessGame/resignGame"
        )
        console.log(response)
        const gameState = response.data.gameState
        this.whitetime = gameState.whiteTimeRemaining
        this.blackTime = gameState.blackTimeRemaining
        this.result = gameState.result
        this.gameEndReason = gameState.gameEndReason
        this.boardConfig.viewOnly = true
      } catch (err) {
        this.error = err
      }
    }, onBoardCreated(api) {
      this.boardAPI = api;
    }

  },
  components: {
    TheChessboard,
  },
};
</script>
<style scoped>
.chess-container {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 30px;
}

.timers {
  width: 200px;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  gap: 20px;
}

.timer {
  background-color: #f0f0f0;
  border: 1px solid #ccc;
  border-radius: 5px;
  padding: 10px;
  text-align: center;
  width: 100px;
}

.info {
  display: flex;
  flex-direction: column;
  justify-content: space-between;
  align-items: center;
  border: 1px solid #ccc;
  border-radius: 5px;
  padding: 10px;
  width: 200px;
  height: 300px;
  text-align: center;
}

.resign {
  padding: 0.7em;
  color: #fff;
  background-color: red;
  border: none;
  width: 60%;
  border-radius: 4px;
  cursor: pointer;
}

.resign:hover {
  background-color: #cc0000;
}
</style>