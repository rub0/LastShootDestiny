using UnityEngine;
using System.Collections;

public class Life : MonoBehaviour {

	public int percentage = 100;

    public int team = 1;

	private Color UpColor;
	private Color downColor;

	public GameObject UpColorGO;
	public GameObject downColorGO;


	private InfoController info;

	private MovementController player;

	// Use this for initialization
	void Start () 
	{
		if (Network.peerType != NetworkPeerType.Server)
		{
			Destroy(this);
			return;
		}

		Fire player = this.gameObject.GetComponent<Fire> ();
		//UpColor = player.DownSpriteGo.GetComponent<SpriteRenderer> ().color;
		//downColor = player.UpSpriteGo.GetComponent<SpriteRenderer> ().color;
	}

    public void OnEnable()
    {

		if (Network.peerType != NetworkPeerType.Server)
		{
			return;
		}

        setupPlayer(true);
        percentage = 100;

        GameObject aux = GameObject.Find("Info");

        if(aux != null) {
            info = aux.gameObject.GetComponent<InfoController>();
        }

		player = this.gameObject.GetComponent<MovementController> ();

		if (info != null && player != null)
			info.setLife (percentage);


        if (gameObject.layer == 12)
            return;

        switch (team)
        {
            case 1:
                transform.position = GameObject.FindGameObjectsWithTag("Respawn")[0].GetComponent<SpawnManager>().giveRespawnIzq();
                break;

            case 2:
                transform.position = GameObject.FindGameObjectsWithTag("Respawn")[0].GetComponent<SpawnManager>().giveRespawnDer();
                break;

            case 5:
                transform.position = GameObject.FindGameObjectsWithTag("Respawn")[0].GetComponent<SpawnManager>().giveRespawnIA();
                break;
        }
        ;
    }



	[ContextMenu ("Hurt")]
	public void Hurt (int damage)
	{
        Debug.Log("uuuuh me hacen daño y tengo " + gameObject);
		percentage -= damage;
        if (this.tag == "Player")
            this.gameObject.networkView.RPC("setLife", RPCMode.Others, percentage);
		//UpColorGO.gameObject.GetComponent<SpriteRenderer> ().material.color = Color.red;

        if (percentage <= 0)
        {
            setupPlayer(false);
            Debug.Log("voy a hacer setlife");
            if (this.tag == "Player")
                this.gameObject.networkView.RPC("setLife", RPCMode.Others, percentage);
            Debug.Log("termino setlife");
            Invoke("OnEnable", 5.0f);
        }

		if (info != null)
		{
			info = GameObject.Find ("Info").gameObject.GetComponent<InfoController>();
		}

		if (player != null && info != null) 
		{
			info.setLife (percentage);
		}
		/*

		Hashtable tweenParams = new Hashtable();
		tweenParams.Add("from", Color.red);
		tweenParams.Add("to", UpColor);
		tweenParams.Add("time", 4.0f);
		tweenParams.Add ("easetype", iTween.EaseType.easeInBack);
		tweenParams.Add ("includeChildren", true);

		iTween.ValueTo(player.DownSpriteGo, tweenParams);

		*/
	}

	void tweenOnUpdateCallBack( int newValue )
	{
		UpColor.a = newValue;
		Debug.Log( UpColor.a );
	}
	
	
	
	private void OnColorUpdated(Color color)
	{

		Debug.Log ("changing color");
		Fire player = this.gameObject.GetComponent<Fire> ();
		player.UpSpriteGo.GetComponent<SpriteRenderer>().color = color;
	}

    public int getTeam()
    {
        return team;
    }

    private void setupPlayer(bool enable)
    {
       // GameObject[]bases = GameObject.FindGameObjectsWithTag("base");
        //Debug.Log(enable + " setupplayer " + this.gameObject);
        if (!enable && this.gameObject.layer == 12)
        {
            //acabamos la puñetera partida xD
            Debug.Log("acabando la partida");
            GameObject.Find("manager").GetComponent<BasicNetwork>().networkView.RPC("FinishParty", RPCMode.All, this.team);

        }
        
        
        if (Network.peerType == NetworkPeerType.Server)
        {
            networkView.RPC("respawn", RPCMode.Others, enable);
        }

        percentage = 0;
    }
}
