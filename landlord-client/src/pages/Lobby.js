import React, {useState} from 'react';
import { useParams } from 'react-router-dom';
import '../App.css';

function Lobby() {

    const {roomCode} = useParams();
    const [muted, setMuted] = useState(false)
    const [volume, setVolume] = useState(50)
    const toggleMute = () => setMuted(value => !value);

    return (
        <div className="top-bar">
            <div className='room-info'>
               <span>LANDLORD</span>
               <span>/room/{roomCode}</span>
            </div>
            <div className='volume'>
                <button className='toggleMute' onClick={e => toggleMute()}>{muted || volume == 0 ? '🔇' : '🔊'}</button>
                <input type='range' min='0' max='100' step='5' onChange={e => setVolume(e.target.value)}></input>
            </div>
        </div>
    )
}

export default Lobby;