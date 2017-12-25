using UnityEngine;
using System.Collections;

public class NetServerManager : MonoBehaviour
{
    private string nick;
    public GameObject player;
    public string connectToIP = "127.0.0.1";
    public int connectionPort = 25001;
    private ArrayList players;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    [RPC]
    public void loadLevel()
    {
        if (Network.peerType == NetworkPeerType.Client)
        {
            Application.LoadLevel(1);
        }
    }

    [RPC]
    public void startGame()
    {
        Debug.Log("voy a instanciar");
        Application.LoadLevel(1);
        networkView.RPC("loadLevel", RPCMode.Others);
    }

    public void OnLevelWasLoaded(int level)
    {
        //Debug.Log("onlevelloadded");
        if (level == 1 && Network.peerType != NetworkPeerType.Server)
            Network.Instantiate(player, Vector3.zero, Quaternion.identity, 0);

    }

    public void OnPlayerDisconnected(NetworkPlayer player)
    {
        Debug.Log("Clean up after player " + player);
        Network.RemoveRPCs(player);
        Network.DestroyPlayerObjects(player);
    }

    public void OnConnectedToServer()
    {
        networkView.RPC("initClient", RPCMode.Others, nick);
    }

    [RPC]
    public void initClient(string player)
    {
        players.Add(player);
    }
}
