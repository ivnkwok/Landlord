import React, {useState} from 'react';
import { useParams } from 'react-router-dom';
import '../App.css';

function Lobby() {

    const {roomCode} = useParams<{roomCode: string}>();
    const [muted, setMuted] = useState<boolean>(false)
    const [volume, setVolume] = useState<number>(50)
    const toggleMute = () => setMuted(value => !value);

    return (
        <div className='lobby'>
            <div className='top-bar'>
                <div className='room-info'>
                <span>LANDLORD</span>
                <span>/room/{roomCode}</span>
                </div>
                <div className='volume'>
                    <button className='toggleMute' onClick={e => toggleMute()}>{muted || volume == 0 ? '🔇' : '🔊'}</button>
                    <input type='range' min='0' max='100' step='5' onChange={(e: React.ChangeEvent<HTMLInputElement>) => setVolume(Number(e.target.value))}></input>
                </div>
            </div>
            <div className='main-content'>
              <div className='canvas-area'>
                  <canvas className='game-board'></canvas>
              </div>
              <div className='chat-area'></div>
            </div>
        </div>
    )
}

export default Lobby;