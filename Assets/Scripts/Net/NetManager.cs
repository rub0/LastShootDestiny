using UnityEngine;
using System.Collections;

public class NetManager : MonoBehaviour {

    public GameObject[] prefabs;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    [RPC]
    public void createEntity(int prefab, Vector3 position, Quaternion rotation, NetworkViewID viewID)
    {
        message entityType = (message)prefab;

        GameObject gameObject = Instantiate(prefabs[prefab], position, rotation) as GameObject;
        gameObject.GetComponent<NetworkView>().viewID = viewID;
    }

    void OnConnectedToServer()
    {
        Debug.Log("Connected to server");

        NetworkViewID viewID = Network.AllocateViewID();
        networkView.RPC("createEntity", RPCMode.AllBuffered, 0, Vector3.zero, Quaternion.identity, viewID);

    }
}
