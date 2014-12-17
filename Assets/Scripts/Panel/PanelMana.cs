using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PanelMana : MonoBehaviour {

	public GameObject[] Images;
	public AvatarModel AvatarModel;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (AvatarModel != null){
			for (int i = 0; i < Images.Length; i++) {
				Images[i].SetActive(i < AvatarModel.MaxMana);
				Images[i].GetComponent<Image>().sprite = (i < AvatarModel.ActualMana) ? SpriteManager.ManaCrystallFull : SpriteManager.ManaCrystalEmpty;
			}
		}
	
	}
}
