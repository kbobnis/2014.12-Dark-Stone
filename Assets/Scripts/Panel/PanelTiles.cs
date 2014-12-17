using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class PanelTiles : MonoBehaviour {

	public GameObject PanelInformation;

	public Mode Mode = Mode.Ready;

	internal void PointerDownOn(PanelTile panelTile) {
		Debug.Log("pointer down");
	}

	private void SetPlace(PanelTile panelTile){
		Card c = null; 

		bool foundAPlace = false;

		foreach (Side s in SideMethods.AllSides()) {
			if (panelTile.SetInteractionForMode(Mode.SpellPositioning, c, c.Params[ParamType.Distance], s, panelTile, panelTile)) {
				foundAPlace = true;
			}
		}

		if (foundAPlace) {
			Mode = Mode.SpellPositioning;
		} else {
			PanelInformation.GetComponent<PanelInformation>().SetText("There is no place to cast this spell. All are taken.");
			DisableCastingOnAll(Mode.All);
			Mode = Mode.Ready;
		}
	}

	private void DisableCastingOnAll(Mode disableWhat) {
		foreach (GameObject tile in gameObject.GetComponent<ScrollableList>().ElementsToPut) {
			tile.GetComponent<PanelTile>().PanelInteraction.GetComponent<PanelInteraction>().DisableWhat(disableWhat);
		}
	}

	private void CastSpell(Card c, PanelTile panelTile) {
		PanelAvatar pa = panelTile.PanelInteraction.GetComponent<PanelInteraction>().WhoIsCasting.PanelAvatar.GetComponent<PanelAvatar>();
		pa.CastOn(c, panelTile);
		
		if (panelTile.PanelAvatar.GetComponent<PanelAvatar>().Model.Speed > 0 || panelTile.PanelAvatar.GetComponent<PanelAvatar>().Model.MovesLeft > 0) {
			DisableCastingOnAll(Mode.SpellPositioning);
			SetDirection(panelTile, panelTile.PanelInteraction.GetComponent<PanelInteraction>().WhoIsCasting);
			Mode = Mode.SpellDirectioning;
		} else {
			DisableCastingOnAll(Mode.All);
			Mode = Mode.Ready;
		}
	}

	private void SetDirection(PanelTile castingWhere, PanelTile castersTile) {
		AvatarModel pam = castingWhere.PanelAvatar.GetComponent<PanelAvatar>().Model;
		Card c = pam.Card;

		bool foundAPlace = false;

		foreach (Side s in SideMethods.AllSides()) {
			if (castingWhere.SetInteractionForMode(Mode.SpellDirectioning, c, 1, s, castersTile, castingWhere)) {
				foundAPlace = true;
			}
		}

		castingWhere.SetInteractionForMode(Mode.SpellDirectioning, c, 0, Side.None, castingWhere, castingWhere);

		if (!foundAPlace) {
			Mode = Mode.Ready;
		}
	}
}

public enum Mode {
	Ready, SpellDirectioning, SpellPositioning, All
}
