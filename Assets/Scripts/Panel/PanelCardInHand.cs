using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class PanelCardInHand : MonoBehaviour {

	public GameObject PanelCardPreview, PanelAvatar, PanelMinigame;
	public Card Card;


	void Update() {
		PanelAvatar.SetActive(false);

		if (Card != null) {
			PanelAvatar.SetActive(true);
			PanelAvatar.GetComponent<PanelAvatarCard>().Prepare(Card);
		}
	}

	public void ShowPreview() {
		Debug.Log("Showing preview for card: " + Card.Name);
		try {
			PanelCardPreview.GetComponent<PanelCardPreview>().Preview(Card);
			PanelMinigame.GetComponent<PanelMinigame>().CardInHandSelected(Card);
		} catch (Exception e) {
			Debug.Log("Exception: " + e);
		}
	}

}
