<template>
  <div>
    <div class="chess-container">
      <div class="chat">
        <div class="chat-messages">
          <div v-for="(message, index) in messages" :key="index" class="chat-message">
            <div v-if="message.type === 'chat'">
              <span class="username">{{ message.username }}:</span> {{ message.text }}
            </div>
            <div v-else>
              <span class="special-message">{{ message.username }} {{ message.text }}</span>
            </div>
          </div>
        </div>
        <div class="chat-input">
          <input type="text" v-model="newMessage" :maxlength="200" placeholder="Type a message..."
            @keydown.enter="sendMessage" />
          <button @click="sendMessage">Send</button>
        </div>
      </div>
      <TheChessboard :style="{ margin: '0' }" @move="makeMove" :player-color="playerColor" :board-config="boardConfig"
        @board-created="onBoardCreated" reactive-config>
      </TheChessboard>
      <div class="timers">
        <div>{{ this.isPlayerWhite ? this.blackPlayerUsername : this.whitePlayerUsername }}</div>
        <div class="timer">{{ this.isPlayerWhite ? formattedBlackTime : formattedWhiteTime }}</div>
        <div class="info" v-if="result">
          <div>{{ this.result !== "" ? this.gameEndReason + " " + this.result : "" }}</div>
        </div>
        <div class="info" v-else>
          <div v-if="drawOffer" class="drawButtons">
            <button class="accept" @click="acceptDraw">Accept</button>
            <button class="decline" @click="declineDraw">Decline</button>
          </div>
          <button v-else class="draw" @click="offerDraw">Draw</button>
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
import { mapGetters, mapActions } from 'vuex';

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
      messages: [],
      newMessage: "",
      drawOffer: false,
      offerSent: false
    };
  },
  computed: {
    ...mapGetters(['getToken']),
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
    const token = this.getToken;
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
        const gameState = response.gameState;
        console.log(gameState)
        console.log(`${!gameState.isWhiteTurn ? "White player" : "Black player"} played move:`, gameState.prevMove)
        if ((gameState.isWhiteTurn && this.isPlayerWhite) || (!gameState.isWhiteTurn && !this.isPlayerWhite)) {
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

      this.connection.on('SendMessage', (username, message) => {
        console.log(username + ": " + message)

        this.messages.push({
          username: username,
          text: message,
          type: 'chat'
        });

        this.$nextTick(() => {
          const chatMessages = this.$el.querySelector(".chat-messages");
          chatMessages.scrollTop = chatMessages.scrollHeight;
        });
      })

      this.connection.on('ResignGame', (response, username) => {
        const gameState = response.gameState;
        console.log(gameState)
        this.whiteTime = gameState.whiteTimeRemaining
        this.blackTime = gameState.blackTimeRemaining
        this.result = gameState.result
        this.gameEndReason = gameState.gameEndReason
        this.boardConfig.viewOnly = this.result !== ""

        this.messages.push({
          username: username,
          text: "resigned",
          type: 'resignation'
        })

        this.$nextTick(() => {
          const chatMessages = this.$el.querySelector(".chat-messages");
          chatMessages.scrollTop = chatMessages.scrollHeight;
        });
      })

      this.connection.on('DrawOffer', (username) => {
        this.drawOffer = true;

        this.messages.push({
          username: username,
          text: "offers a draw",
          type: 'drawOffer'
        });

        this.$nextTick(() => {
          const chatMessages = this.$el.querySelector(".chat-messages");
          chatMessages.scrollTop = chatMessages.scrollHeight;
        });
      })

      this.connection.on('DeclineDrawOffer', (username) => {
        this.offerSent = false;

        this.messages.push({
          username: username,
          text: "declines a draw",
          type: 'drawOffer'
        });

        this.$nextTick(() => {
          const chatMessages = this.$el.querySelector(".chat-messages");
          chatMessages.scrollTop = chatMessages.scrollHeight;
        });
      })

      this.connection.on('AcceptDrawOffer', (username, response) => {
        const gameState = response.gameState;
        console.log(gameState)
        this.whiteTime = gameState.whiteTimeRemaining
        this.blackTime = gameState.blackTimeRemaining
        this.result = gameState.result
        this.gameEndReason = gameState.gameEndReason
        this.boardConfig.viewOnly = this.result !== ""

        this.messages.push({
          username: username,
          text: "accepts a draw",
          type: 'drawOffer'
        });

        this.$nextTick(() => {
          const chatMessages = this.$el.querySelector(".chat-messages");
          chatMessages.scrollTop = chatMessages.scrollHeight;
        });
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
  beforeUnmount() {
    if (this.connection) {
      this.connection.stop().then(() => {
        console.log('SignalR connection stopped');
      }).catch((err) => {
        console.error('Error stopping SignalR connection:', err);
      });
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
        console.log(err)
        this.error = err;
      }
    },
    async makeMove(move) {
      if ((this.isWhiteTurn && this.isPlayerWhite) || (!this.isWhiteTurn && !this.isPlayerWhite)) {
        if (this.drawOffer) {
          this.drawOffer = false
          this.declineDraw()
        }
        this.offerSent = false
        const moveString = move.from + '-' + move.to;
        try {
          await this.connection.invoke('MakeMove', moveString)
        } catch (err) {
          console.log(err)
        }
      }
    },
    async resignGame() {
      try {
        await this.connection.invoke('ResignGame')
      } catch (err) {
        console.log(err)
      }
    }, async offerDraw() {
      if (!this.offerSent) {
        this.messages.push({
          username: this.username,
          text: "offers a draw",
          type: 'draw offer'
        });

        this.$nextTick(() => {
          const chatMessages = this.$el.querySelector(".chat-messages");
          chatMessages.scrollTop = chatMessages.scrollHeight;
        });
        try {
          await this.connection.invoke('DrawOffer')
        } catch (err) {
          this.error = err
        }
      }
      this.offerSent = true;
    }, async declineDraw() {
      this.drawOffer = false;
      this.messages.push({
        username: this.username,
        text: "declines a draw",
        type: 'draw offer'
      });

      this.$nextTick(() => {
        const chatMessages = this.$el.querySelector(".chat-messages");
        chatMessages.scrollTop = chatMessages.scrollHeight;
      });
      try {
        await this.connection.invoke('ReplyToDrawOffer', 'Decline')
      } catch (err) {
        this.error = err
      }
    }, async acceptDraw() {
      this.drawOffer = false;

      try {
        await this.connection.invoke('ReplyToDrawOffer', 'Accept')
      } catch (err) {
        this.error = err
      }
    },
    onBoardCreated(api) {
      this.boardAPI = api;
    }, async sendMessage() {
      if (this.newMessage.trim() === "" || this.newMessage.length > 200) return;

      this.messages.push({
        username: this.username,
        text: this.newMessage,
        type: 'chat'
      });

      this.$nextTick(() => {
        const chatMessages = this.$el.querySelector(".chat-messages");
        chatMessages.scrollTop = chatMessages.scrollHeight;
      });

      await this.connection.invoke('SendMessage', this.newMessage);
      this.newMessage = "";
    },
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

.side-panel {
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 20px;
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
  justify-content: flex-end;
  align-items: center;
  border: 1px solid #ccc;
  border-radius: 5px;
  padding: 10px;
  width: 200px;
  height: 300px;
  text-align: center;
  gap: 5px;
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

.draw {
  padding: 0.7em;
  color: #fff;
  background-color: blue;
  border: none;
  width: 60%;
  border-radius: 4px;
  cursor: pointer;
}

.draw:hover {
  background-color: #0000cc;
}

.drawButtons {
  display: flex;
  flex-direction: row;
  align-items: center;
  justify-content: center;
  gap: 10px;
  width: 60%;
}

.accept {
  background-color: #00cc00;
  color: #fff;
  border-radius: 4px;
  border: none;
  padding: 0.7em;
  cursor: pointer;
}

.accept:hover {
  background-color: green;
}

.decline {
  background-color: red;
  color: #fff;
  border-radius: 4px;
  border: none;
  padding: 0.7em;
  cursor: pointer;
}

.decline:hover {
  background-color: #cc0000;
}

.chat {
  width: 250px;
  height: 350px;
  border: 1px solid #ccc;
  border-radius: 8px;
  overflow: hidden;
  display: flex;
  flex-direction: column;
  background-color: #e6e6fa;
  box-shadow: 0 4px 8px rgba(0, 0, 0, 0.1);
}

.chat-messages {
  flex: 1;
  overflow-y: auto;
  padding: 10px;
  background-color: #f8f8ff;
  color: #333;
  word-wrap: break-word;
}

.chat-message {
  margin-bottom: 10px;
  padding: 5px;
  border-radius: 5px;
  background-color: #fff;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
}

.username {
  font-weight: bold;
  color: #007bff;
  margin-bottom: 3px;
}

.chat-input {
  display: flex;
  gap: 10px;
  padding: 10px;
  background-color: #e6e6fa;
}

.chat-input input {
  flex: 1;
  padding: 8px;
  border: 1px solid #ccc;
  border-radius: 5px;
}

.chat-input button {
  padding: 8px 12px;
  color: white;
  background-color: #007bff;
  border: none;
  border-radius: 5px;
  cursor: pointer;
}

.chat-input button:hover {
  background-color: #0056b3;
}

.special-message {
  font-weight: bold
}
</style>