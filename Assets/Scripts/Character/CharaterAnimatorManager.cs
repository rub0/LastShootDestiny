using UnityEngine;
using System.Collections;

public class CharaterAnimatorManager : MonoBehaviour {

    Animator animator;
    //MovementController movement;

    public GameObject player;
    MovementController movement;

	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        movement = player.GetComponent<MovementController>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 momentum = movement.getMomentum();

        animator.SetFloat("horizontalSpeed", Mathf.Abs(momentum.x));
        //Debug.Log(momentum.y);
        animator.SetFloat("verticalSpeed", Mathf.Abs(momentum.y));
        animator.SetBool("ground", movement.isGrounded());
	}

    public void onShoot() {
        animator.SetTrigger("shoot");
    }
}
