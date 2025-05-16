import React, {useState} from 'react';
import logo from './logo.svg';
import './App.css';

const apiUrl = 'https://landlordcardgameapi-d4fkbke4ewdjbqcw.canadacentral-01.azurewebsites.net/session/createsession'

function App() {
  const [sessionID, setSessionID] = useState('');

  const getSessionID = () => {
    fetch(apiUrl).then(response => {
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
      return response.text()
    })
    .then(data => {
      setSessionID(data)
    })
    .catch(error => {
      console.error('Error: ', error)
      setSessionID('Error: ', error)
    })
  } 


  return (
    <div className="App">
      <header className="App-header">
        <img src={logo} className="App-logo" alt="logo" />
        <p>{sessionID || 'no session yet'}</p>
        <button onClick={getSessionID}>
          Create new session
        </button>
      </header>
    </div>
  );
}

export default App;
