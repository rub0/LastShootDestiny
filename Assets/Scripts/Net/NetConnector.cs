using UnityEngine;
using System.Collections;

public class NetConnector : MonoBehaviour {


	// Use this for initialization
	void Start () {
        
	}

    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {

        Vector3 position = Vector3.zero;
        Vector3 localScale = Vector3.zero;

        if (stream.isWriting)
        {
            position = transform.position;
            localScale = this.gameObject.transform.localScale;
            stream.Serialize(ref position);
            stream.Serialize(ref localScale);

            
        }
        else
        {
            stream.Serialize(ref position);
            stream.Serialize(ref localScale);
            transform.position = position;
            this.gameObject.transform.localScale = localScale;

            Debug.Log("local scale" + this.gameObject.transform.localScale);
        }
    }
	
	// Update is called once per frame
	void Update () {
	}

    [RPC]
	public void shoot(bool shootToRight)
    {
		GetComponent<Fire>().shoot(shootToRight);
    }

    [RPC]
    public void setLife(int percentage)
    {
        GameObject aux = GameObject.Find("Info");
        InfoController info;
        if (aux != null)
        {
            info = aux.gameObject.GetComponent<InfoController>();
            info = GameObject.Find("Info").gameObject.GetComponent<InfoController>();
            info.setLife(percentage);
        }
        
    }

    [RPC]
    public void onShoot()
    {
        CharaterAnimatorManager[] temp = GetComponentsInChildren<CharaterAnimatorManager>();

        foreach (CharaterAnimatorManager anim in temp)
            anim.onShoot();
    }

    [RPC]
    void respawn(bool enable)
    {
        CharacterController controller = GetComponent<CharacterController>();
        if (controller == null)
            return;
        MovementController movement = GetComponent<MovementController>();
        if (movement != null)
            movement.enabled = enable;
        else
        {
            IAMovement m = GetComponent<IAMovement>();
            if (m != null)
            {
                m.enabled = enable;
                GetComponent<IAFire>().enabled = enable;
            }
        }
        controller.detectCollisions = enable;
        controller.enabled = enable;
        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer render in renderers)
        {
            render.enabled = enable;
        }
    }

    [RPC]
    void setOwner(int team)
    {
        gameObject.GetComponent<Bullet>().owner = team;
    }
}
