using UnityEngine;
using System.Collections;

public class IAFire : MonoBehaviour {
    public IAController input;

    public int bulletVelocity = 10;

    public GameObject bullet;

    public float nextFire = 0.25f;

    private float coolDown = 0.0f;

    private bool rigth = true;

    public Transform bulletSourceTransformRigth;

    public Transform bulletSourceTransformLeft;

    private Transform currentSourceTransform;
	// Use this for initialization
	void Start () {

        currentSourceTransform = /*rigth ? bulletSourceTransformRigth :*/ bulletSourceTransformLeft;

        if (!input)
            input = GetComponent<IAController>();
	}
	
	// Update is called once per frame
	void Update () {

        coolDown += Time.deltaTime;


        Physics.IgnoreLayerCollision(0, 11, (this.gameObject.GetComponent<CharacterController>().velocity.y > 0.0f));
        if (input.getFireEnabled() && coolDown > nextFire)
        {
            coolDown = 0.0f;
            if (input.updateMovement().x > 0) //rigth
            {
                rigth = false;
            }
            else if (input.updateMovement().x < 0)//left
            {
                rigth = true;
            }


            this.gameObject.transform.localScale = new Vector3(rigth ? -1 : 1, 1, 1);
            GameObject cloneBullet = (Network.peerType == NetworkPeerType.Disconnected) ? Instantiate(bullet, currentSourceTransform.position, Quaternion.identity) as GameObject
            : Network.Instantiate(bullet, currentSourceTransform.position, Quaternion.identity, 0) as GameObject;

            Bullet bulletObject = cloneBullet.gameObject.GetComponent<Bullet>();

            bulletObject.velocity = bulletVelocity;


            bulletObject.owner = this.gameObject.GetComponent<Life>().getTeam();

            bulletObject.toRight = rigth;

            
        }
	}
}
