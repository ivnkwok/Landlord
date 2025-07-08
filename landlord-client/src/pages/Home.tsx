import React, {useState} from 'react';
import { useNavigate } from 'react-router-dom';
import '../App.css';
import { error } from 'console';

const apiUrl = process.env.REACT_APP_API_URL

function Home() {
  const [username, setUsername] = useState('Guest')
  const [isLoading, setIsLoading] = useState<boolean>(false);
  const [roomId, setRoomId] = useState<string>('');
  const navigate = useNavigate();
  
  const createSession = () => {
    setIsLoading(true);
    fetch(`${apiUrl}createsession?userName=${username}`, {
      method: 'POST',
    })
    .then(response => {
      if (!response.ok) {
        if (response.status == 404) {
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
      const newRoomId = data.roomId;
      setRoomId(newRoomId);
      navigateToRoom(data);
    })
    .catch(error => {
      console.error('Error: ', error)
    })
    .finally(() => {
      setIsLoading(false);
    })
  }

  const joinRoom = async () => {
    setIsLoading(true);
    fetch(`${apiUrl}joinsession?roomId=${roomId}&userName=${username}`, {
      method: 'POST',
    })
    .then(response => {
      if (!response.ok) {
        if (response.status == 404) {
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
      navigateToRoom(data);
    })
  }
  
  const navigateToRoom = (roomInfo: any) => {
    let roomCode = roomInfo.roomId;
    if (roomCode?.length >= 4) {
      navigate(`/room/${roomCode}`, {state: roomInfo});
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
            onClick={createSession}>
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
            disabled={isLoading}
            onClick={joinRoom}
          >
            Enter room code
          </button>
        </div>
      </div>
    </div>
  );
}

export default Home;
