import React, { useState, ChangeEvent } from 'react';
import { useNavigate } from 'react-router-dom';
//import logo from '../logo.svg';
import '../App.css';

const apiUrl = 'https://landlordcardgameapi-d4fkbke4ewdjbqcw.canadacentral-01.azurewebsites.net/session/createsession'

function Home() {
  const [username, setUsername] = useState<string>('Guest');
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [roomId, setRoomId] = useState<string>('');
  const navigate = useNavigate();

  const getSessionID = () => {
    setIsLoading(true);
    fetch(`${apiUrl}?userName=${username}`, {
      method: 'POST',
    })
      .then(response => {
        if (!response.ok) {
          if (response.status === 404) {
            throw new Error('URL not found');
          } else if (response.status === 500) {
            throw new Error('Server error');
          }
          else {
            throw new Error('Network error');
          }
        }
        return response.json()
      })
      .then(data => {
        const newRoomId = data.roomId
        setRoomId(newRoomId)
        joinRoom(newRoomId)
      })
      .catch(error => {
        console.error('Error: ', error)
      })
      .finally(() => setIsLoading(false));
  }

  const joinRoom = (roomCode: string = roomId) => {
    if (roomCode?.length >= 4) {
      navigate(`/room/${roomCode}`);
    }
  }

  return (
    <div className="home">
      <header className="app-header">
        <p>LANDLORD!!!</p>
      </header>
      <div className="room-sections">
        <div className="room-section">
          <p>CREATE ROOM</p>
          <input
            placeholder="Username"
            onChange={e => setUsername(e.target.value)}
          />
          <button
            disabled={isLoading}
            onClick={getSessionID}>
            {isLoading ? 'Creating session...' : 'Create new session'}
          </button>
        </div>
        <div className="room-section">
          <p>JOIN ROOM</p>
          <input
            placeholder="Room code"
            maxLength={4}
            onChange={e => setRoomId(e.target.value)}
          />
          <button
            onClick={() => joinRoom()}
          >
            Enter room code
          </button>
        </div>
      </div>
    </div>
  );
}

export default Home;