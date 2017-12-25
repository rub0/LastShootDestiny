using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Net;
using System.Net.Sockets;
using System.Threading;


public class OtroManager : MonoBehaviour
{


    public string connectToIP = "127.0.0.1";
    public int connectionPort = 25001;
    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(this);
        
    }
    public void OnGUI()
    {
        if (Network.peerType == NetworkPeerType.Client)
        {
            GUILayout.Label("loading....!");
        }
    }
    /*// Update is called once per frame
    void Update () {
	
    }*/

    [RPC]
    void loadLevel()
    {
        if (Network.peerType == NetworkPeerType.Client)
        {
            Application.LoadLevel(2);
        }
    }

    void OnNetworkInstantiate(NetworkMessageInfo info)
    {
        Debug.Log("ayyyy");
        if (Network.peerType == NetworkPeerType.Server)
        {
            Debug.Log("voy a cargar nivel");
            networkView.RPC("loadLevel", RPCMode.Others);
        }
    }
}
