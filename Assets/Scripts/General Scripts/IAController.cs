using UnityEngine;
using System.Collections;

public class IAController : InputManager
{

    private GameObject[] players;
    private Vector3 movement = Vector3.zero;

    float actualDistance;
    // Use this for initialization
    void Start()
    {
        if (Network.peerType != NetworkPeerType.Server && Network.peerType != NetworkPeerType.Disconnected)
        {
            Destroy(GetComponent<MovementController>());
            Destroy(this);
            return;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        float distance = 999f;
        Vector3 target = Vector3.zero;
        foreach ( GameObject player in players)
        {
            float d = Vector3.Distance(player.transform.position, transform.position);
            if (distance > d)
            {
                target = player.transform.position;
                distance = d;
            }
        }
        
        actualDistance = distance;

        target = target - transform.position;
        movement = Vector3.zero;
        if (target.y > 2)
            movement.y = 1;
        if (target.x > 1)
            movement.x = 1;
        else if (target.x < -1)
            movement.x = -1;
    }

    public Vector3 updateMovement()
    {

        return movement;
    }

    public bool getButtonJumpUp()
    {
        return movement.y > 0;
    }

    public bool startButton()
    {
        return false;
    }

    public int getNumJoysticks()
    {
        return Input.GetJoystickNames().Length;
    }

    public bool getOKButton()
    {
        return false;
    }

    public bool getCancelButton()
    {
        return false;
    }

    public bool getFireEnabled()
    {
        return actualDistance < 8f;
    }
}


