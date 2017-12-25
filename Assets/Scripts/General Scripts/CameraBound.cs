using UnityEngine;
using System.Collections;

public class CameraBound : MonoBehaviour {

    Transform camera;
    public float maxX;
    public float minX;
    public float maxY;
    public float minY;

    public Transform target = null;

	// Use this for initialization
	void Start () {
        camera = GetComponent<Transform>();
	}
	
	// Update is called once per frame
	void Update () {

        if (target == null)
            return;

        Vector3 pos = target.position;
        pos.z = camera.position.z;

        if (pos.x > maxX)
            pos.x = maxX;
        if (pos.x < minX)
            pos.x = minX;
        if (pos.y > maxY)
            pos.y = maxY;
        if (pos.y < minY)
            pos.y = minY;

        camera.position = pos;
	}

    public void setPlayer(GameObject player)
    {
        target = player.transform;
    }
}
