using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PanelHand : MonoBehaviour {

	public GameObject[] Cards;

	private AvatarModel _AvatarModel;

	public AvatarModel AvatarModel {
		set { _AvatarModel = value;  }
	}

	void Update() {
		if (_AvatarModel != null && _AvatarModel.Hand.Count > 0) {
			for (int i = 0; i < Cards.Length; i++) {
				Cards[i].SetActive(_AvatarModel.Hand.Count > i);
				if (_AvatarModel.Hand.Count > i) {
					Cards[i].GetComponent<PanelCardInHand>().Card = _AvatarModel.Hand[i];
				}
			}
		}
	}
}
