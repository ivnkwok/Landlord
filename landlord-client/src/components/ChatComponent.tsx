// ChatComponent.tsx
import React, { useEffect, useState } from 'react';
import {
  ChatClient,
  ChatThreadClient,
  ChatMessage
} from '@azure/communication-chat';
import { AzureCommunicationTokenCredential } from '@azure/communication-common';

type ChatProps = {
  endpoint: string;
  userToken: string;
  threadId: string;
};

const ChatComponent = ({ endpoint, userToken, threadId }: ChatProps) => {
  const [chatThreadClient, setChatThreadClient] = useState<ChatThreadClient | null>(null);
  const [messages, setMessages] = useState<ChatMessage[]>([]);
  const [newMessage, setNewMessage] = useState('');

  useEffect(() => {
    const initChat = async () => {
      const credential = new AzureCommunicationTokenCredential(userToken);
      const chatClient = new ChatClient(endpoint, credential);
      const threadClient = await chatClient.getChatThreadClient(threadId);
      setChatThreadClient(threadClient);

      const messagesPage = await threadClient.listMessages().next();
      if (messagesPage.value) {
        setMessages([messagesPage.value]);
      }
    };

    initChat();
  }, [endpoint, userToken, threadId]);

  const sendMessage = async () => {
    if (chatThreadClient && newMessage.trim()) {
      await chatThreadClient.sendMessage({ content: newMessage });
      // Fetch latest messages after sending
      const messagesIterator = chatThreadClient.listMessages();
      const allMessages: ChatMessage[] = [];
      for await (const msg of messagesIterator) {
        allMessages.push(msg);
      }
      setMessages(allMessages);
      setNewMessage('');
    }
  };

  return (
    <div className="p-4 border rounded max-w-md mx-auto">
      <div className="h-64 overflow-y-auto border mb-2 p-2">
        {messages.map((msg, index) => (
          <div key={index} className="mb-1">
            {msg.content as string}
          </div>
        ))}
      </div>
      <input
        type="text"
        value={newMessage}
        onChange={(e) => setNewMessage(e.target.value)}
        className="border p-1 w-full mb-2"
        placeholder="Type a message"
      />
      <button
        onClick={sendMessage}
        className="bg-blue-500 text-white px-3 py-1 rounded"
      >
        Send
      </button>
    </div>
  );
};

export default ChatComponent;
