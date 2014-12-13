using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class PanelTile : MonoBehaviour {

	public GameObject PanelCardOnBoard;

	private WhatNow WhatNow = WhatNow.Nothing;
	private PanelCardOnBoard SpellCasterForThisTile;

	public Dictionary<Side, PanelTile> Neighbours = new Dictionary<Side, PanelTile>();

	internal void CastFromTemplate(TileTemplate tt, float pan) {
		
		PanelCardOnBoard.GetComponent<PanelCardOnBoard>().Prepare(tt.Card, null);
		
	}

	public void PointerDown(BaseEventData bed) {

		Card c = PanelCardOnBoard.GetComponent<PanelCardOnBoard>().PanelSpell.GetComponent<PanelSpell>().TopCard();

		Debug.Log("clicked on: " + (c!=null?c.Name:"no card"));

		if (c != null) {
			if (!c.Params.ContainsKey(ParamType.Speed)) {
				throw new Exception("What to do when card has no speed? Figure it out, implement it.");
			}
			WhatNow = WhatNow.ThrowingSpell;
			
			PrepareNeighboursToPlaceSpell(c.Params[ParamType.Speed], c);
		}
	}

	private void PrepareNeighboursToPlaceSpell(int distance, Card c) {

		if (distance > 1) {
			if (Neighbours[Side.Up] != null) {
				Neighbours[Side.Up].GetComponent<PanelTile>().PrepareNeighboursToPlaceSpell(distance - 1, c);
			}

		} else {
			WhatNow = global::WhatNow.ReadyToReceiveSpell;
			SpellCasterForThisTile = PanelCardOnBoard.GetComponent<PanelCardOnBoard>();
		}
		
	}

}

public enum WhatNow {
	Nothing, ThrowingSpell, ReadyToReceiveSpell
}
