import React, {useState} from 'react';
import { useParams } from 'react-router-dom';
import '../App.css';

function Lobby() {

    const {roomCode} = useParams();
    function EndSession() {
        
    }

    return (
        <div className="top-bar">
            <p>LANDLORD {roomCode}</p>
            {/* <button>End Session</button> */}
        </div>
    )
}

export default Lobby;