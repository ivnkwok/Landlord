import React, {useEffect, useState} from 'react';
import { useParams, useLocation } from 'react-router-dom';
import { CommandChatClient } from "../services/CommandChatClient";
import '../App.css';

function Lobby() {

    const {roomCode} = useParams<{roomCode: string}>();
    const [muted, setMuted] = useState<boolean>(false)
    const [volume, setVolume] = useState<number>(50)
    const toggleMute = () => setMuted(value => !value);

    const location = useLocation();
    const roomInfo = location.state;
    let commandChatClient: CommandChatClient | null;

    useEffect(() => {
        const joinRoom = async (roomInfo : any) => {
            const userId = roomInfo.acsUser.userId;
            const token = roomInfo.acsUser.token;
            const endpointUrl = roomInfo.acsEndpoint;
            const threadId = roomInfo.acsConnectionId;
            commandChatClient = new CommandChatClient(endpointUrl, token, threadId, userId);
            await commandChatClient.initialize(messageReceived);

            window.setTimeout(async () => { 
                if (commandChatClient) {
                await commandChatClient.sendCommand("REBOOT_DEVICE_42");
                }
            }, 5000);
        };

        if (roomInfo && !commandChatClient) {
            joinRoom(roomInfo);
        }
    }, []);

    const messageReceived = (command : string, event : any) => {
        console.log("Received command:", command);
        // Perform logic
        if (command === "REBOOT_DEVICE_42") {
            console.log("🚀 Rebooting device...");
        }
    };
 
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