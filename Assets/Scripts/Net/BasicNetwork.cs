using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Net.Sockets;
using System.Threading;

public enum message
{
    UNO,
    DOS
}
public class BasicNetwork : MonoBehaviour
{

	public delegate void clientConnectHandler();
	public clientConnectHandler onClientConnected;

    int _loser = -1;

    public GameObject player;
    public GameObject IA;
    public string connectToIP = "127.0.0.1";
    public int connectionPort = 25001;

	public ArrayList users = new ArrayList();
	public string nick = "player";
    
    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
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


    [RPC]
    void loadLevel()
    {
        if (Network.peerType == NetworkPeerType.Client)
        {
            Application.LoadLevel(1);
        }
    }

    [RPC]
    void startGame()
    {
        Debug.Log("voy a instanciar");
        Application.LoadLevel(1);
        networkView.RPC("loadLevel", RPCMode.Others);
    }

    void OnLevelWasLoaded(int level)
    {
        //Debug.Log("onlevelloadded");
        if (level == 1 && Network.peerType != NetworkPeerType.Server)
            Network.Instantiate(player, Vector3.zero, Quaternion.identity, 0);

        Debug.Log("map loaded");

        if (level == 1 && Network.peerType == NetworkPeerType.Server)
        {
            for( int i = 0 ; i < Network.connections.Length ; ++i)
            {
                Network.Instantiate(IA, Vector3.zero, Quaternion.identity, 0);
                Network.Instantiate(IA, Vector3.zero, Quaternion.identity, 0);
                Network.Instantiate(IA, Vector3.zero, Quaternion.identity, 0);
            }

        }

    }

    void OnPlayerDisconnected(NetworkPlayer player)
    {
        Debug.Log("Clean up after player " + player);
        Network.RemoveRPCs(player);
        Network.DestroyPlayerObjects(player);
    }

    public void refresh()
    {

    }

    public void connect(string ip)
    {
        Network.Connect(ip, connectionPort);
    }

    public void server()
    {
        Network.InitializeServer(32, connectionPort, false);
    }

    public void start()
    {
        networkView.RPC("startGame", RPCMode.Server);
    }
	

	public void OnConnectedToServer()
	{
		networkView.RPC("initClient", RPCMode.Others, nick);
	}
	
	[RPC]
	public void initClient(string player)
	{
		users.Add(player);
		networkView.RPC ("OnClientConnected", RPCMode.Others, users);
	}

    [RPC]
	public void OnClientConnected(string[] players)
	{
		if (onClientConnected != null)
			onClientConnected ();
	}

    [RPC]
    public void FinishParty(int teamLoser)
    {
        _loser = teamLoser;

        Application.LoadLevel(0);
    }

    public int getLoser()
    {
        return _loser;
    }
    
}
