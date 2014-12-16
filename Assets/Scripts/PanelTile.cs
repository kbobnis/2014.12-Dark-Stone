using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class PanelTile : MonoBehaviour {

	public GameObject PanelTiles, PanelAvatar, PanelInteraction;

	public Dictionary<Side, PanelTile> Neighbours = new Dictionary<Side, PanelTile>();

	public void Touched(BaseEventData bed) {
		PanelTiles.GetComponent<PanelTiles>().PointerDownOn(this);
	}

	internal bool SetInteractionForMode(Mode mode, Card c, int distance, Side s, PanelTile whoIsCasting, PanelTile castingFrom) {
		bool anyPlaceToCast = false;
		if (distance > 0) {
			if (Neighbours.ContainsKey(s)) {
				if (Neighbours[s].SetInteractionForMode(mode, c, distance - 1, s, whoIsCasting, castingFrom)) {
					anyPlaceToCast = true;
				}
			}
		} else if (!PanelAvatar.GetComponent<PanelAvatar>().HasSpell() || (mode == Mode.SpellDirectioning)) {
			PanelInteraction.GetComponent<PanelInteraction>().Prepare(mode, c, whoIsCasting, castingFrom, s);
			anyPlaceToCast = true;
		}
		return anyPlaceToCast;
	}


}
