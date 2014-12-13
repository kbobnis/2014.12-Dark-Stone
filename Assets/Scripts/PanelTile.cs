using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class PanelTile : MonoBehaviour {

	public GameObject PanelSpell, PanelTiles, PanelAvatar;

	public Card TemplateCard;

	public Dictionary<Side, PanelTile> Neighbours = new Dictionary<Side, PanelTile>();

	public void Touched(BaseEventData bed) {
		PanelTiles.GetComponent<PanelTiles>().PointerDownOn(this);
	}

	public void Prepare(Card card) {
		if (TemplateCard != null) {
			throw new System.Exception("Card already on this tile. What to do now");
		}
		TemplateCard = card;
		PanelAvatar.GetComponent<PanelAvatar>().Prepare(card);
	}


}
