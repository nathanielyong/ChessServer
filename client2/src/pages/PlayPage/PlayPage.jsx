import axiosInstance from '../../axiosConfig'
import { Chessboard } from 'react-chessboard';
import React, { useState, useEffect, useRef } from 'react';
import './PlayPage.css'

function PlayPage(props) {
    const [whitePlayerUsername, setWhitePlayerUsername] = useState('');
    const [blackPlayerUsername, setBlackPlayerUsername] = useState('');
    const [isPlayerWhite, setIsPlayerWhite] = useState(true);
    const [isWhiteTurn, setIsWhiteTurn] = useState(true);
    const [whiteTime, setWhiteTime] = useState(0);
    const [blackTime, setBlackTime] = useState(0);
    const [moveCount, setMoveCount] = useState(0);
    const [result, setResult] = useState('');
    const [gameEndReason, setGameEndReason] = useState('');
    const [error, setError] = useState('');
    const [fen, setFen] = useState('rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1');
    const [playerColor, setPlayerColor] = useState('');
    const [intervalId, setIntervalId] = useState(null);
    const [intervalId2, setIntervalId2] = useState(null);

    const chessboardRef = useRef(null);

    const formattedWhiteTime = () => {
        const minutes = Math.max(0, Math.floor(whiteTime / 60000)).toString().padStart(2, '0');
        const seconds = Math.max(0, Math.floor(whiteTime % 60000 / 1000)).toString().padStart(2, '0');
        return `${minutes}:${seconds}`;
    };

    const formattedBlackTime = () => {
        const minutes = Math.max(0, Math.floor(blackTime / 60000)).toString().padStart(2, '0');
        const seconds = Math.max(0, Math.floor(blackTime % 60000 / 1000)).toString().padStart(2, '0');
        return `${minutes}:${seconds}`;
    };

    const startTimer = () => {
        if (intervalId) {
            clearInterval(intervalId);
        }
        const id = setInterval(() => {
            if (moveCount > 1 && result === '') {
                if (isWhiteTurn) {
                    if (whiteTime > 0) {
                        setWhiteTime(prev => prev - 1000);
                    } else {
                        clearInterval(id);
                    }
                } else {
                    if (blackTime > 0) {
                        setBlackTime(prev => prev - 1000);
                    } else {
                        clearInterval(id);
                    }
                }
            }
        }, 1000);
        setIntervalId(id);
    };

    const refreshGameInterval = () => {
        if (intervalId2) {
            clearInterval(intervalId2);
        }
        const id2 = setInterval(() => {
            if (isPlayerWhite !== isWhiteTurn && result === '') {
                fetchGame();
            }
        }, 5000);
        setIntervalId2(id2);
    };

    const fetchGame = async () => {
        try {
            const response = await axiosInstance.get('/api/LiveChessGame/getCurrentGame');
            const gameState = response.data.gameState;
            setWhitePlayerUsername(gameState.whitePlayerUsername);
            setBlackPlayerUsername(gameState.blackPlayerUsername);
            setIsWhiteTurn(gameState.isWhiteTurn);
            setIsPlayerWhite(username === gameState.whitePlayerUsername);
            setWhiteTime(gameState.whiteTimeRemaining);
            setBlackTime(gameState.blackTimeRemaining);
            setMoveCount(gameState.moveCount);
            setResult(gameState.result);
            setGameEndReason(gameState.gameEndReason);
            setFen(gameState.currentPositionFEN);
            setPlayerColor(isPlayerWhite ? 'white' : 'black');
        } catch (err) {
            setError(err);
        }
    };

    const makeMove = async () => {

    }

    const resignGame = async () => {

    }

    useEffect(() => {
        // fetchGame();
        startTimer();
        refreshGameInterval();
    }, []);

    return (
        <div className="chess-container">
            <div style={{ width: "600px" }}>
                <Chessboard id="chessboard"
                    position={fen}
                    boardOrientation={isWhiteTurn ? 'white' : 'black'}
                    arePiecesDraggable={false}
                />
            </div>
            <div className="timers">
                <div>{isPlayerWhite ? blackPlayerUsername : whitePlayerUsername}</div>
                <div className="timer">
                    {isPlayerWhite ? formattedBlackTime() : formattedWhiteTime()}
                </div>
                <div className="info">
                    <div>{result ? `${gameEndReason} ${result}` : ''}</div>
                    <button className="resign" onClick={resignGame}>
                        Resign
                    </button>
                </div>
                <div className="timer">
                    {isPlayerWhite ? formattedWhiteTime() : formattedBlackTime()}
                </div>
                <div>{isPlayerWhite ? whitePlayerUsername : blackPlayerUsername}</div>
            </div>
        </div>
    )
}

export default PlayPage