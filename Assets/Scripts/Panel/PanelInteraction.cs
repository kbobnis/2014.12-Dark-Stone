using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PanelInteraction : MonoBehaviour {

	//moving
	public PanelTile WhatWantsToMoveHere;

	//casting spell
	public AvatarModel Caster;
	public Card CastersCard;


	void Awake() {
		UpdateImage();
	}

	private void UpdateImage() {
		GetComponent<Image>().enabled = false;
		GetComponentInChildren<Text>().enabled = false;

		if (WhatWantsToMoveHere != null || Caster != null) {
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
		Caster = null;
		CastersCard = null;
		UpdateImage();
	}

	internal void CanCastHere(AvatarModel caster, Card card) {
		Caster = caster;
		CastersCard = card;
		GetComponentInChildren<Text>().text = "Cast " + card.Name + " here";
		UpdateImage();
	}
}
