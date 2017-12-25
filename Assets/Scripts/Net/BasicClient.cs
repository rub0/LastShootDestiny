using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Net.Sockets;
using System.Threading;

public class BasicClient : MonoBehaviour {

    /**
     * Asynchronous class
     */
    private string nick = "nickdeprueba";
    public string connectToIP = "127.0.0.1";
    public int connectionPort = 25001;
    public GameObject manager;


    // Use this for initialization
    void Start()
    {
    }

    public void OnGUI()
    {
        if (Network.peerType == NetworkPeerType.Disconnected)
        {
            //We are currently disconnected: Not a client or host
            GUILayout.Label("Connection status: Disconnected");

            connectToIP = GUILayout.TextField(connectToIP, GUILayout.MinWidth(100));

            GUILayout.BeginVertical();
            if (GUILayout.Button("Connect as client"))
            {
                //Connect to the "connectToIP" and "connectPort" as entered via the GUI
                Debug.Log(Network.Connect(connectToIP, connectionPort));
                Debug.Log(Network.peerType);
            }

            /* if (GUILayout.Button("Start Server"))
             {
                 //Start a server for 32 clients using the "connectPort" given via the GUI
                 Network.InitializeServer(32, connectionPort, false);
             }*/
            GUILayout.EndVertical();


        }
        else
        {

            if (Network.peerType == NetworkPeerType.Connecting)
            {

                GUILayout.Label("Connection status: Connecting");
            }
            else if (Network.peerType == NetworkPeerType.Client)
            {

                GUILayout.Label("Connection status: Client!");
                GUILayout.Label("Ping to server: " + Network.GetAveragePing(Network.connections[0]));
                GUILayout.Label("mi networkid " + networkView.viewID);
            }
            else if (Network.peerType == NetworkPeerType.Server)
            {

                GUILayout.Label("Connection status: Server!");
                GUILayout.Label("Connections: " + Network.connections.Length);
                if (Network.connections.Length >= 1)
                {
                    GUILayout.Label("Ping to first player: " + Network.GetAveragePing(Network.connections[0]));
                }
            }

            if (GUILayout.Button("start game"))
            {
                networkView.RPC("startGame", RPCMode.Server);
            }

            if (GUILayout.Button("Disconnect"))
            {
                Network.Disconnect(200);
            }
        }
    }

    public string LocalIPAddress()
    {
        IPHostEntry host;
        string localIP = "";
        host = Dns.GetHostEntry(Dns.GetHostName());
        foreach (IPAddress ip in host.AddressList)
        {
            if (ip.AddressFamily == AddressFamily.InterNetwork)
            {
                localIP = ip.ToString();
            }
        }
        return localIP;
    }

    void OnConnectedToServer()
    {
        networkView.RPC("initClient", RPCMode.Server, nick);
    }


    [RPC]
    void initClient(string nick)
    {
    }

    [RPC]
    void startGame()
    {
    }
    [RPC]
    void loadMap()
    {
        Application.Quit();
    }
}
