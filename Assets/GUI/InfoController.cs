using UnityEngine;
using System.Collections;



public class InfoController : MonoBehaviour {

	private UILabel nameLabel;
	private UISlider LifeBar;

	private UIFilledSprite LifeBarSprite;

	public GameObject nameLabelGO;
	public GameObject LifeBarGO;


	// Use this for initialization
	void Start () {
		nameLabel = nameLabelGO.GetComponent<UILabel> ();
		nameLabel.text = GameObject.Find ("manager").GetComponent<BasicNetwork> ().nick;;

		LifeBar = LifeBarGO.GetComponent<UISlider> ();

		LifeBarSprite = LifeBarGO.GetComponent<UIFilledSprite> ();
	}
	

	public void setLife(int life)
	{
		if (LifeBar != null)
			LifeBar.sliderValue = (float)life / 100.0f;
	}

}
