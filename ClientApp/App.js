// src/App.js
import React, { useState } from 'react';
import Register from './Register';
import Login from './Login';
import Chat from './Chat'; // Assuming you have a Chat component

const App = () => {
    const [token, setToken] = useState(null);

    return (
        <div>
            <h1>WebRTC Chat App</h1>
            {!token ? (
                <div>
                    <Register />
                    <Login setToken={setToken} />
                </div>
            ) : (
                <Chat token={token} /> // Pass the token to the Chat component
            )}
        </div>
    );
};

export default App;
