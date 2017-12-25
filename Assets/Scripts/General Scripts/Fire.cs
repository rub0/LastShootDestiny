using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour {

	public GameObject bullet;

	public int bulletVelocity = 10;

	public Transform bulletSourceTransformRigth;

	public Transform bulletSourceTransformLeft;

	public GameObject UpSpriteGo;

	public GameObject DownSpriteGo;

	private SpriteRenderer UpSprite;

	private SpriteRenderer DownSprite;

	private Transform currentSourceTransform;

	private float nextFire = 0.0f;

	private bool rigth = true;

	public bool useServerData = true;

    CharaterAnimatorManager[] temp;

	// Use this for initialization
	void Start ()
	{
        temp = GetComponentsInChildren<CharaterAnimatorManager>();
	
		currentSourceTransform = rigth ? bulletSourceTransformRigth : bulletSourceTransformLeft;

		UpSprite = UpSpriteGo.gameObject.GetComponent<SpriteRenderer> ();

        if (Network.peerType == NetworkPeerType.Client && !networkView.isMine)
        {
            Destroy(this);
            return;
        }
		DownSprite = DownSpriteGo.gameObject.GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		Physics.IgnoreLayerCollision(9, 11, (this.gameObject.GetComponent<CharacterController>().velocity.y > 0.0f));


		//if (Input.GetButtonDown ("Horizontal")) 
		//{
		if (Input.GetAxis ("Horizontal") > 0) //rigth
		{
			rigth = false;
		}
		else if (Input.GetAxis ("Horizontal") < 0)//left
		{
			rigth = true;
		}

		if (Input.GetButtonDown ("Fire1") && Time.time > nextFire && (networkView.isMine || Network.peerType == NetworkPeerType.Disconnected)) 
		{
			if (Network.peerType == NetworkPeerType.Disconnected)
				shoot(rigth);
			else
				networkView.RPC("shoot", RPCMode.Server, rigth);
		}

		//this.gameObject.transform.localScale.Set (rigth ? 1 : -1, 1, 1);
        if(networkView.isMine || Network.peerType == NetworkPeerType.Disconnected)
		    this.gameObject.transform.localScale = new Vector3 (rigth ? -1 : 1, 1, 1);

		//material.SetTextureScale ("_MainTex", Vector2 (rigth ? -1 : 1, 1));

		//DownSprite.material.SetTextureScale ("_MainTex", Vector2 (rigth ? -1 : 1, 1));

		currentSourceTransform =  /*rigth ? bulletSourceTransformRigth :*/ bulletSourceTransformLeft;
			
			//currentSourceTransform = 
//			iTween.RotateBy(gameObject, iTween.Hash("y", .25, "easeType", "easeOutCubic", "loopType", "once", "delay", .1));


			//transform.rotation = Vector3.Normalize(new Vector3(Input.GetAxis("Horizontal"), 0f, 0f));
		//}

	}



	[ContextMenu ("Shoot")]
    [RPC]
	public void shoot (bool shootToRight)
	{

		GameObject cloneBullet = (Network.peerType == NetworkPeerType.Disconnected) ? Instantiate(bullet, currentSourceTransform.position, Quaternion.identity) as GameObject
			: Network.Instantiate(bullet, currentSourceTransform.position, Quaternion.identity, 0) as GameObject ;

		Bullet bulletObject = cloneBullet.gameObject.GetComponent<Bullet>();

		bulletObject.velocity = bulletVelocity;

        bulletObject.owner = this.gameObject.GetComponent<Life>().getTeam();

		bulletObject.toRight = shootToRight;

        foreach (CharaterAnimatorManager anim in temp)
            anim.onShoot();

        networkView.RPC("onShoot",RPCMode.Others);

        bulletObject.networkView.RPC("setOwner", RPCMode.Server, bulletObject.owner);
	}
}
