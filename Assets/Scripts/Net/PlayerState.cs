using UnityEngine;
using System.Collections;

public class PlayerState : MonoBehaviour {

    public enum PlayerNumber { P1, P2, P3, P4 }
    public PlayerNumber playerNumber = PlayerNumber.P1;

    public enum State { Idle, Running, Jumping, Falling, Attacking, Teleporting, Dead, Ghost }
    public State state = State.Idle;

    public enum Orientation { Left, Right }
    public Orientation orientation = Orientation.Right;

    private Vector3 prevMomentum = Vector3.zero;
    private Vector3 currentMomentum = Vector3.zero;
    public Vector3 Momentum
    {
        get { return currentMomentum; }
        set
        {
            currentMomentum = value;
            if (prevMomentum.x != currentMomentum.x)
            {
                if (currentMomentum.x > 0)
                    orientation = Orientation.Right;
                else if (currentMomentum.x < 0)
                    orientation = Orientation.Left;
            }

            if (state != State.Attacking && state != State.Dead && state != State.Ghost)
            {
                if (currentMomentum.x == 0.0f)
                    state = State.Idle;
                else
                    state = State.Running;

                if (currentMomentum.y > 0)
                    state = State.Jumping;
                else if (currentMomentum.y < 0)
                    state = State.Falling;
            }
        }
    }
}
