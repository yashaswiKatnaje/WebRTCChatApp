// URL for the SignalR chat hub (replace with your actual URL)
const signalingServerUrl = "https://localhost:44342/SignalRHub";
const connection = new signalR.HubConnectionBuilder().withUrl(signalingServerUrl).build();

// WebRTC variables
let localStream;
let peerConnection;
let remoteUserId;
remoteUserId = '31121994';
const configuration = {
    iceServers: [{ urls: "stun:stun.l.google.com:19302" }]  // Free STUN server to allow NAT traversal
};

// Get HTML elements for the video streams
const localVideo = document.getElementById("localVideo");
const remoteVideo = document.getElementById("remoteVideo");

// Function to access the camera and microphone
async function startMedia() {
    localStream = await navigator.mediaDevices.getUserMedia({ video: true, audio: true });
    localVideo.srcObject = localStream;
}

// Function to create a new peer-to-peer connection
function createPeerConnection() {
    peerConnection = new RTCPeerConnection(configuration);

    // Add local stream tracks (video/audio) to the peer connection
    localStream.getTracks().forEach(track => peerConnection.addTrack(track, localStream));

    // Listen for remote stream and add it to the remote video element
    peerConnection.ontrack = event => {
        const [stream] = event.streams;
        remoteVideo.srcObject = stream;
    };

    // Send any new ICE candidates to the signaling server
    peerConnection.onicecandidate = event => {
        if (event.candidate) {
            connection.invoke("SendIceCandidate", remoteUserId, JSON.stringify(event.candidate));
        }
    };
}

// Start the call when the user clicks "Start Call"
document.getElementById("startCall").addEventListener("click", async () => {
    await startMedia();  // Get the local media stream
    createPeerConnection();  // Set up the WebRTC peer connection

    // Create an SDP offer and set it as the local description
    const offer = await peerConnection.createOffer();
    await peerConnection.setLocalDescription(offer);

    // Send the offer via SignalR to the other peer
    connection.invoke("SendOffer", remoteUserId, JSON.stringify(offer));
});

// End the call when the user clicks "End Call"
document.getElementById("endCall").addEventListener("click", () => {
    peerConnection.close();  // Close the WebRTC connection
    localVideo.srcObject = null;
    remoteVideo.srcObject = null;
});

// Handle incoming SignalR messages for signaling
connection.on("ReceiveOffer", async offer => {
    if (!peerConnection) createPeerConnection();
    await peerConnection.setRemoteDescription(new RTCSessionDescription(JSON.parse(offer)));

    // Create an SDP answer and send it to the other peer
    const answer = await peerConnection.createAnswer();
    await peerConnection.setLocalDescription(answer);
    connection.invoke("SendAnswer", remoteUserId, JSON.stringify(answer));
});

connection.on("ReceiveAnswer", async answer => {
    await peerConnection.setRemoteDescription(new RTCSessionDescription(JSON.parse(answer)));
});

connection.on("ReceiveIceCandidate", async candidate => {
    await peerConnection.addIceCandidate(new RTCIceCandidate(JSON.parse(candidate)));
});

// Start the SignalR connection
connection.start().catch(console.error);
