using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class PanelTile : MonoBehaviour {

	public GameObject PanelMinigame, PanelAvatar, PanelInteraction;

	public Dictionary<Side, PanelTile> Neighbours = new Dictionary<Side, PanelTile>();
	public int Row;
	public int Column;

	public void Touched(BaseEventData bed) {
		try {
			PanelMinigame.GetComponent<PanelMinigame>().PointerDownOn(this);
		} catch (Exception e) {
			Debug.Log("Exception: " + e);
		}
	}


	internal int GetDistanceTo(PanelTile to) {
		int d = Mathf.Abs(to.Row - Row) + Mathf.Abs(to.Column - Column);
		if (to.Row != Row && to.Column != Column) {
			d--;
		}
		return d;
	}

	internal bool CanIHaveThisSpell(PanelTile caster, PanelTile onWhat, Card card) {

		AvatarModel myModel = PanelAvatar.GetComponent<PanelAvatar>().Model;
		AvatarModel onWhatModel = PanelAvatar.GetComponent<PanelAvatar>().Model;

		switch (card.IsCastOn) {
			case IsCastOn.Target: return onWhat == this; break;
			case IsCastOn.AllFriendlyMinions: return onWhatModel != null && onWhatModel.GetMyHero().IsItYourMinion(myModel); break;
		}
		throw new NotImplementedException("Implement case: " + card.IsCastOn);
	}
}
