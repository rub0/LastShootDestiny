using UnityEngine;
using System.Collections;

public class SpawnManager : MonoBehaviour {

    public GameObject[] spawnPointsIzq;
    public GameObject[] spawnPointsDer;
    public GameObject[] spawnPointsIA;

    // Use this for initialization
    void Start()
    {
        if (Network.peerType == NetworkPeerType.Client)
        {
            Destroy(this.gameObject);
            return;
        }
    }

    public Vector3 giveRespawnIzq()
    {
        int i = 0;
        for (i = 0; i < spawnPointsIzq.Length - 1; ++i)
        {
            if ( Physics.OverlapSphere(spawnPointsIzq[i].transform.position, 1.0f).Length == 0)
                return spawnPointsIzq[i].transform.position; ;
        }

        return spawnPointsIzq[i].transform.position;
    }

    public Vector3 giveRespawnDer()
    {
        int i = 0;
        for (i = 0; i < spawnPointsDer.Length - 1; ++i)
        {
            if (Physics.OverlapSphere(spawnPointsDer[i].transform.position, 1.0f).Length == 0)
                return spawnPointsDer[i].transform.position; ;
        }

        return spawnPointsDer[i].transform.position;
    }

    public Vector3 giveRespawnIA()
    {
        return spawnPointsIA[Random.Range(0, spawnPointsIA.Length)].transform.position;
    }
}
