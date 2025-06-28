import React, {useEffect, useState} from 'react';
import { useParams, useLocation } from 'react-router-dom';
import { CommandChatClient } from "../services/CommandChatClient";
import '../App.css';
import { ChatParticipant } from '@azure/communication-chat';

function Lobby() {

    const {roomCode} = useParams<{roomCode: string}>();
    const [userList, setUserList] = useState<ChatParticipant[]>([])
    const [muted, setMuted] = useState<boolean>(false)
    const [volume, setVolume] = useState<number>(50)
    const toggleMute = () => setMuted(value => !value);

    const location = useLocation();
    const roomInfo = location.state;
    let commandChatClient: CommandChatClient;

    useEffect(() => {
        const joinRoom = async (roomInfo : any) => {
            const userId = roomInfo.acsUser.userId;
            const token = roomInfo.acsUser.token;
            const endpointUrl = roomInfo.acsEndpoint;
            const threadId = roomInfo.acsConnectionId;
            commandChatClient = new CommandChatClient(endpointUrl, token, threadId, userId);
            await commandChatClient.initialize((receivedMessage, event) => {
                messageReceived(receivedMessage, event)
            });
            await sendJoinMessage();

            // window.setTimeout(async () => { 
            //     if (commandChatClient) {
            //     await commandChatClient.sendCommand("REBOOT_DEVICE_42");
            //     }
            // }, 5000);
        };
        
        if (roomInfo && !commandChatClient) {
            joinRoom(roomInfo);
        }
    }, []);

    const sendJoinMessage = async () => {
        if (commandChatClient) {
            await commandChatClient.sendCommand("ENTERED")

        }
    }

    const messageReceived = async (command : string, event : any) => {
        console.log("Received command:", command);
        // Perform logic
        if (command === "ENTERED") {
            let participants = await commandChatClient.getUserList()
            const newUserList = []
        for await (const participant of participants) {
            console.log(participant)
            newUserList.push(participant)
        }
        setUserList(newUserList)
            // const addParticipantRequest = {
            //     participants: [
            //         {
            //             id: { communicationUserId: roomInfo.acsUser.userId},
            //             displayName: roomInfo.acsUser.userName
            //         }
            //     ]
            // }
            // console.log(commandChatClient?.addUserToChat(addParticipantRequest))
        }
    };
    

    const getUserList = async () => {
        const participants = await commandChatClient.getUserList();
        const newUserList = []
        for await (const participant of participants) {
            console.log(participant)
            newUserList.push(participant)
        }
        setUserList(newUserList)
    }

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
              <div className='userlist'>{userList?.map((user, i) => (
                <p key={i}>{user.displayName}</p>
              ))}</div>
              <button onClick={getUserList}></button>
                  <canvas className='game-board'></canvas>
              </div>
              <div className='chat-area'></div>
            </div>
        </div>
    )
}

export default Lobby;