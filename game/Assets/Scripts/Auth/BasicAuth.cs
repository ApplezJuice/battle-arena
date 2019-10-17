﻿using UnityEngine;
using System.Collections;
using Mirror;
using PlayFab;

[AddComponentMenu("Network/Authenticators/BasicAuthenticator")]
    public class BasicAuth : NetworkAuthenticator
    {
        [Header("Custom Properties")]

        // set these in the inspector
        public string username;
        public string password;

        public class AuthRequestMessage : MessageBase
        {
            // use whatever credentials make sense for your game
            // for example, you might want to pass the accessToken if using oauth
            public string playFabID;
            public string sessionTicket;
        }

        public class AuthResponseMessage : MessageBase
        {
            public byte code;
            public string message;
        }

        public override void OnStartServer()
        {
            // register a handler for the authentication request we expect from client
            NetworkServer.RegisterHandler<AuthRequestMessage>(OnAuthRequestMessage, false);
        }

        public override void OnStartClient()
        {
            // register a handler for the authentication response we expect from server
            NetworkClient.RegisterHandler<AuthResponseMessage>(OnAuthResponseMessage, false);
        }

        public override void OnServerAuthenticate(NetworkConnection conn)
        {
            // do nothing...wait for AuthRequestMessage from client
        }

        public override void OnClientAuthenticate(NetworkConnection conn)
        {
            // use the params to call the authenticate session ticket

            AuthRequestMessage authRequestMessage = new AuthRequestMessage
            {
                playFabID = username,
                sessionTicket = password
            };

            NetworkClient.Send(authRequestMessage);
        }

        public void OnAuthRequestMessage(NetworkConnection conn, AuthRequestMessage msg)
        {
            Debug.LogFormat("Authentication Request: {0} {1}", msg.playFabID, msg.sessionTicket);

        // TESTING REMOVE
        msg.playFabID = username;
        msg.sessionTicket = password;
        // TESTING REMOVE\

        //NetworkManager.userAuthDetails.Add(msg.playFabID, conn);
        /*
         * 
         if (NetworkManager.singleton.PlayFabIDs.Contains(result.UserInfo.PlayFabID))
        {
            NetworkConnection conn = NetworkManager.singleton.PlayFabIDs(result.UserInfo.PlayFabID);
            NetworkManager.singleton.PlayFabIDs.Remove(result.UserInfo.PlayFabID);
        }

         */

        PlayFabServerAPI.AuthenticateSessionTicket(new PlayFab.ServerModels.AuthenticateSessionTicketRequest()
        {
            SessionTicket = msg.sessionTicket
        }, (result) =>
        {
            
            Debug.Log("The result of the result of the playfabID: " + result.UserInfo.PlayFabId);
            if (msg.playFabID == result.UserInfo.PlayFabId)
            {
                // this is a good request
                AuthResponseMessage authResponseMessage = new AuthResponseMessage
                {
                    code = 100,
                    message = "Success"
                };

                NetworkServer.SendToClient(conn.connectionId, authResponseMessage);

                // Invoke the event to complete a successful authentication
                base.OnServerAuthenticated.Invoke(conn);
            }
            else
            {
                Debug.Log(result.UserInfo.PlayFabId);
                // invalid token for playfabID
                AuthResponseMessage authResponseMessage = new AuthResponseMessage
                {
                    code = 200,
                    message = "Invalid Credentials"
                };

                NetworkServer.SendToClient(conn.connectionId, authResponseMessage);

                // must set NetworkConnection isAuthenticated = false
                conn.isAuthenticated = false;

                // disconnect the client after 1 second so that response message gets delivered
                Invoke(nameof(conn.Disconnect), 1);
            }

        }, (error) =>
        {
            // some error
            Debug.Log(error);
            AuthResponseMessage authResponseMessage = new AuthResponseMessage
            {
                code = 200,
                message = "Invalid Credentials"
            };

            NetworkServer.SendToClient(conn.connectionId, authResponseMessage);

            // must set NetworkConnection isAuthenticated = false
            conn.isAuthenticated = false;

            // disconnect the client after 1 second so that response message gets delivered
            Invoke(nameof(conn.Disconnect), 1);
        });

        // check the credentials by calling your web server, database table, playfab api, or any method appropriate.
        if (msg.playFabID == username && msg.sessionTicket == password)
            {
                // create and send msg to client so it knows to proceed
                AuthResponseMessage authResponseMessage = new AuthResponseMessage
                {
                    code = 100,
                    message = "Success"
                };

                NetworkServer.SendToClient(conn.connectionId, authResponseMessage);

                // Invoke the event to complete a successful authentication
                base.OnServerAuthenticated.Invoke(conn);
            }
            else
            {
                // create and send msg to client so it knows to disconnect
                AuthResponseMessage authResponseMessage = new AuthResponseMessage
                {
                    code = 200,
                    message = "Invalid Credentials"
                };

                NetworkServer.SendToClient(conn.connectionId, authResponseMessage);

                // must set NetworkConnection isAuthenticated = false
                conn.isAuthenticated = false;

                // disconnect the client after 1 second so that response message gets delivered
                Invoke(nameof(conn.Disconnect), 1);
            }
        }

        public void OnAuthResponseMessage(NetworkConnection conn, AuthResponseMessage msg)
        {
            if (msg.code == 100)
            {
                Debug.LogFormat("Authentication Response: {0}", msg.message);

                // Invoke the event to complete a successful authentication
                base.OnClientAuthenticated.Invoke(conn);
            }
            else
            {
                Debug.LogErrorFormat("Authentication Response: {0}", msg.message);

                // Set this on the client for local reference
                conn.isAuthenticated = false;

                // disconnect the client
                conn.Disconnect();
            }
        }
    }

