import React, {useState} from 'react';
import { useParams } from 'react-router-dom';

function Lobby() {

    const {roomCode} = useParams();
    function EndSession() {

    }

    return (
        <div>
            <button>End Session</button>
            <p>{roomCode}</p>
        </div>
    )
}

export default Lobby;