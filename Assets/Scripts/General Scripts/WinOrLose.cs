using UnityEngine;
using System.Collections;

public class WinOrLose : MonoBehaviour {


	public GameObject losePanel;
    public GameObject winPanel;

	public BasicNetwork network;

	private int team = -1;

    // Use this for initialization
    void OnLevelWasLoaded(int level)
    {
		
        if (network.getLoser() != -1)// Not First time
        {
			for(int i = 0; i <= network.users.Count; i++)
			{
				if ( network.users[i].ToString() == network.nick)
				{
					team =  (i <= 1)? 1 : 2;
				}
			}

			if (network.getLoser () == team) 
			{
				losePanel.gameObject.SetActive(true);
				Invoke("DisableThis", 3.0f);
			} else {
				winPanel.gameObject.SetActive(true);
				Invoke("DisableThis", 3.0f);
			}

		}



    }



	

	public void DisableThis(GameObject GO)
	{
		winPanel.SetActive(false);
		losePanel.SetActive(false);
	}

}
