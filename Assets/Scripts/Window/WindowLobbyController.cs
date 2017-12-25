using UnityEngine;
using System.Collections;

public class WindowLobbyController : MonoBehaviour {

	public GameObject labelC1;
	public GameObject labelC2;
	public GameObject labelC3;
	public GameObject labelC4;

	public GameObject labelIP;

	public GameObject clientButton;

	public GameObject ServerButton;

	public GameObject connectButton;

	public GameObject nameLabel;

	public GameObject networkGO;

	private BasicNetwork network;

	public void OnEnable()
	{
		network = networkGO.GetComponent<BasicNetwork> ();

		UIEventListener.Get (clientButton.gameObject).onClick += OnClickClient;
		UIEventListener.Get (connectButton.gameObject).onClick += OnClickConnect;
		UIEventListener.Get (ServerButton.gameObject).onClick += OnClickServer;

		network.onClientConnected += OnClientConnected;

	}

	public void OnDisable()
	{
		UIEventListener.Get (clientButton.gameObject).onClick -= OnClickClient;
		UIEventListener.Get (connectButton.gameObject).onClick -= OnClickConnect;
		UIEventListener.Get (ServerButton.gameObject).onClick -= OnClickServer;

		network.onClientConnected -= OnClientConnected;

	}

	void OnClickClient (GameObject go)
	{
		network.nick = nameLabel.GetComponent<UILabel>().text;
		clientButton.SetActive (false);
		connectButton.SetActive (true);
        network.connect(labelIP.gameObject.GetComponent<UILabel>().text);
	}

	void OnClickConnect (GameObject go)
	{
		network.start ();
	}

	void OnClickServer (GameObject go)
	{


		network.server ();
	}


	public void OnClientConnected()
	{

	}


	// Update is called once per frame
	void Update ()
	{
		if (Network.peerType == NetworkPeerType.Client) 
		{
				clientButton.SetActive (false);
				ServerButton.SetActive (false);
				connectButton.SetActive (true);
		} else 
			if (Network.peerType == NetworkPeerType.Server) 
		{
			clientButton.SetActive (false);
			ServerButton.SetActive (false);
			connectButton.SetActive (false);
		}
		else
		{
			clientButton.SetActive (true);
			ServerButton.SetActive (true);
			connectButton.SetActive (false);
		}

	
	}


}
