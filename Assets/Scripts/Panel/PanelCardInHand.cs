using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class PanelCardInHand : MonoBehaviour {

	public GameObject PanelCardPreview, PanelAvatar, PanelMinigame;

	private AvatarModel AvatarModel;
	public Card Card;

	//this is linked in inspector as pointer down
	public void PointerDown() {
		Debug.Log("Showing preview for card: " + Card.Name);
		try {
			PanelCardPreview.GetComponent<PanelCardPreview>().Preview(AvatarModel, Card, false);
			PanelMinigame.GetComponent<PanelMinigame>().CardInHandSelected(Card);
		} catch (Exception e) {
			Debug.Log("Exception: " + e);
		}
	}


	internal void Prepare(AvatarModel avatarModel, global::Card card) {
		AvatarModel = avatarModel;
		Card = card;

		PanelAvatar.SetActive(false);

		if (Card != null) {
			PanelAvatar.SetActive(true);
			PanelAvatar.GetComponent<PanelAvatarCard>().Prepare(AvatarModel, Card, true);
		}
	}
}
