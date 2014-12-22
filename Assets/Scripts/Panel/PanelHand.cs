using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PanelHand : MonoBehaviour {

	public GameObject[] Cards;

	public AvatarModel AvatarModel;

	void Update() {

		if (AvatarModel != null ) {
			for (int i = 0; i < Cards.Length; i++) {
				Cards[i].SetActive(AvatarModel.Hand.Count > i);
				if (AvatarModel.Hand.Count > i) {
					Cards[i].GetComponent<PanelCardInHand>().Prepare(AvatarModel, AvatarModel.Hand[i]);
				}
			}
		}
	}
}
