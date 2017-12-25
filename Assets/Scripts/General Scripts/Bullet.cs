using UnityEngine;
using System.Collections;


public class Bullet : MonoBehaviour {

	public int velocity = 0;

	public int owner;

    public int damagePercentage;

	public bool toRight = true;

    bool niapa = false;

	public void OnEnable()
	{
        if (Network.peerType != NetworkPeerType.Server)
        {
            Destroy(this);
        }
	}

	public void OnTriggerEnter(Collider collision)
	{
        if (collision.gameObject == null)
        {
            Network.Destroy(this.gameObject);
            niapa = true;
            return;
        }

        Life lifeComponent = collision.gameObject.GetComponent<Life>();

        if (lifeComponent == null)
        {
            niapa = true;
            Network.Destroy(this.gameObject);
            return;
        }

        if (lifeComponent.team == owner) 
		{
            if (!niapa)
                Network.Destroy(this.gameObject);
            return;
		}
        
        switch (collision.gameObject.layer)
        {
            case 9:
                lifeComponent.Hurt(damagePercentage);
                break;
            case 12:
                lifeComponent.Hurt(damagePercentage/5);
                break;
            case 13:
                lifeComponent.Hurt(damagePercentage);
                break;
        }
        niapa = true;
        Network.Destroy(this.gameObject);
    }

    void Start()
    {
        Shooted();
    }

	// Use this for initialization
	public void Shooted ()
	{
		this.gameObject.rigidbody.AddRelativeForce ((toRight ? Vector3.left : Vector3.right) * velocity, ForceMode.Impulse);

		//Destroy (this.gameObject, 2.0f);

        Invoke("destroyBullet", 2.0f);
	}

    private void destroyBullet()
    {
        if (!niapa)
            Network.Destroy(this.gameObject);
    }

}
