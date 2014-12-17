using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class PanelCardInHand : MonoBehaviour {

	public GameObject PanelCardPreview, PanelAvatar;
	private Card _Card;

	public Card Card {
		set { _Card = value; UpdateImage(); }
	}


	void Awake() {
		UpdateImage();
	}

	private void UpdateImage() {
		PanelAvatar.SetActive(false);

		if (_Card != null) {
			PanelAvatar.SetActive(true);
			PanelAvatar.GetComponent<PanelAvatar>().Prepare(null);
			PanelAvatar.GetComponent<PanelAvatar>().Prepare(_Card);
		}
	}

	public void ShowPreview() {
		Debug.Log("Showing preview for card: " + _Card.Name);
		PanelCardPreview.GetComponent<PanelCardPreview>().Preview(_Card);
	}

}
