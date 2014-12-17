using UnityEngine;
using System.Collections;

public class PanelBottom : MonoBehaviour {

	public GameObject PanelMana, PanelHand;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	internal void Prepare(AvatarModel lasiaModel) {
		PanelMana.GetComponent<PanelMana>().AvatarModel = lasiaModel;
		PanelHand.GetComponent<PanelHand>().AvatarModel = lasiaModel;
	}
}
