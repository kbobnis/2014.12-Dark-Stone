using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PanelInteraction : MonoBehaviour {

	public PanelTile WhatWantsToMoveHere;

	void Awake() {
		UpdateImage();
	}

	private void UpdateImage() {
		GetComponent<Image>().enabled = false;
		GetComponentInChildren<Text>().enabled = false;

		if (WhatWantsToMoveHere != null) {
			GetComponent<Image>().enabled = true;
			GetComponentInChildren<Text>().enabled = true;
		}
	}


	internal void CanMoveHere(PanelTile panelTile) {
		WhatWantsToMoveHere = panelTile;

		GetComponentInChildren<Text>().text = "Move here";
		UpdateImage();
	}

	internal void Clear() {
		WhatWantsToMoveHere = null;
		UpdateImage();
	}
}
