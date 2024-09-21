import React, { useEffect, useState } from 'react';
import { io } from 'socket.io-client';

const Chat = () => {
    const [messages, setMessages] = useState([]);
    const [input, setInput] = useState('');
    const socket = io('http://localhost:5003/chathub'); // Change the URL to match your service

    useEffect(() => {
        socket.on('ReceiveMessage', (user, message) => {
            setMessages((prevMessages) => [...prevMessages, { user, message }]);
        });

        return () => {
            socket.off('ReceiveMessage');
        };
    }, [socket]);

    const sendMessage = () => {
        socket.emit('SendMessage', 'User1', input);
        setInput('');
    };

    return (
        <div>
            <h2>Chat Room</h2>
            <div>
                {messages.map((msg, index) => (
                    <div key={index}>
                        <strong>{msg.user}</strong>: {msg.message}
                    </div>
                ))}
            </div>
            <input
                type="text"
                value={input}
                onChange={(e) => setInput(e.target.value)}
            />
            <button onClick={sendMessage}>Send</button>
        </div>
    );
};

export default Chat;
