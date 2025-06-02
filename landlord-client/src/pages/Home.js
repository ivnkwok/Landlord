import React, {useState} from 'react';
import { useNavigate } from 'react-router-dom';
import logo from '../logo.svg';
import '../App.css';

const apiUrl = 'https://landlordcardgameapi-d4fkbke4ewdjbqcw.canadacentral-01.azurewebsites.net/session/createsession'

function Home() {
  const [username, setUsername] = useState('Guest')
  const navigate = useNavigate();

  const getSessionID = () => {
    fetch(`${apiUrl}?userName=${username}`, {
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
      const roomId = data.roomId
      navigate(`/${roomId}`)
    })
    .catch(error => {
      console.error('Error: ', error)
    })
  }

  return (
    <div className="Home">
      <header className="App-header">
        <img src={logo} className="App-logo" alt="logo" />
        <p>LANDLORD!!!</p>
        <input 
          value={username}
          onChange={e => setUsername(e.target.value)}
        />
        <button onClick={getSessionID}>
          Create new session
        </button>
      </header>
    </div>
  );
}

export default Home;
