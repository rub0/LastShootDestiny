using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Net.Sockets;
using System.Threading;

public class BasicServer : MonoBehaviour {

    public string connectToIP = "127.0.0.1";
    public int connectionPort = 25001;
    public GameObject manager;

    private ArrayList players = new ArrayList();

    public void OnGUI()
    {
        if (Network.peerType == NetworkPeerType.Disconnected)
        {
            //We are currently disconnected: Not a client or host
            GUILayout.Label("Connection status: Disconnected");

            connectToIP = GUILayout.TextField(connectToIP, GUILayout.MinWidth(100));

            GUILayout.BeginVertical();

            if (GUILayout.Button("Start Server"))
            {
                //Start a server for 32 clients using the "connectPort" given via the GUI
                Network.InitializeServer(32, connectionPort, false);
            }
            GUILayout.EndVertical();

        }
        else
        {
            if (Network.peerType == NetworkPeerType.Connecting)
            {

                GUILayout.Label("Connection status: Connecting");

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

    void OnPlayerConnected(NetworkPlayer player)
    {

    }

    [RPC]
    void initClient(string nick)
    {
        players.Add(nick);
        Debug.Log("cliente iniciado");
    }

    [RPC]
    void startGame()
    {
        DontDestroyOnLoad(this.gameObject);
        Debug.Log("voy a instanciar");
        NetworkView.Instantiate(manager);
        Debug.Log("termino de instanciar");
    }
}
