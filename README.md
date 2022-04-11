# ChatSignalR-dotNet
 .NET Core project creating a chat server using SignalR.

**Configuration:**

Open `Properties\launchSettings.json` and set the machine local IP or localhost on `applicationUrl`

**Client:**

Using NodeJS: https://github.com/CAVAh/ChatSignalR-NodeJS

Using React Native: https://github.com/CAVAh/ChatSignalR-React-Native

**Postman Request:**

Call Postman Request (request file inside Postman folder) POST `/chat/signalr/negotiate/` this request will return a `connectionId` to use to connect to the websocket.

On Postman create a WebSocket Request to call the WebSocket Server. URL: `wss://192.168.68.135:7121/chat/signalr?id={{signalRconnectionId}}` 

After connect needs to send a message `{"protocol":"json","version":1}`

The response message should be `{}`

After this message you can send the real message: 

`{"arguments":["user","msg"],"invocationId":"0","target":"SendMessage","type":1}`

Remember to use 0x1e char at the end of JSON (line).
