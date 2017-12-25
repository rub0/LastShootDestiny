using UnityEngine;
using System.Collections;

public class InputManager : MonoBehaviour, IMovementCommands {

    private PlayerState state;
    public float _errorRange = 0.1f;

    void Start()
    {
        state = GetComponent<PlayerState>();

        if (state == null)
            state = gameObject.AddComponent<PlayerState>();
    }

    public Vector3 updateMovement() 
    {
        Vector3 displ = Vector3.zero;

        if (Input.GetAxis("Horizontal") < -_errorRange)
            displ += Vector3.left;
        else if (Input.GetAxis("Horizontal") > _errorRange)
            displ += Vector3.right;

        if(Input.GetButton("Jump"))
            displ.y = 1.0f;

        return displ;
    }

    public Vector3 updatePause()
    {
        Vector3 displ = Vector3.zero;

        if (Input.GetAxisRaw("Horizontal_" + state.playerNumber) < -_errorRange)
            displ += Vector3.left;
        else if (Input.GetAxisRaw("Horizontal_" + state.playerNumber) > _errorRange)
            displ += Vector3.right;

        if (Input.GetAxisRaw("Vertical_" + state.playerNumber) > _errorRange)
            displ.y = 1.0f;
        else if (Input.GetAxisRaw("Vertical_" + state.playerNumber) < -_errorRange)
            displ.y = -1.0f;

        return displ;
    }

    public bool getButtonJumpUp()
    {
        return Input.GetButtonDown("Jump");
    }

    public bool startButton()
    {
        return Input.GetButtonDown("Start");
    }

    public int getNumJoysticks()
    {
        return Input.GetJoystickNames().Length;
    }

    public bool getOKButton()
    {
        return Input.GetButton("Jump_general");
    }

    public bool getCancelButton()
    {
        return Input.GetButton("Teleport_general");
    }
}
