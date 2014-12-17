using UnityEngine;
using System.Collections;

public class PanelCardPreview : MonoBehaviour {

	public GameObject PanelAvatar;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	internal void Preview(Card Card) {
		PanelAvatar.GetComponent<PanelAvatar>().Model = new AvatarModel();
		PanelAvatar.GetComponent<PanelAvatar>().Prepare(Card);
	}

}
