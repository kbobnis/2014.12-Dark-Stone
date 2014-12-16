using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class PanelTiles : MonoBehaviour {

	public GameObject PanelInformation;

	public Mode Mode = Mode.Ready;

	internal void PointerDownOn(PanelTile panelTile) {
		try {
			PanelAvatarModel pam = panelTile.PanelAvatar.GetComponent<PanelAvatar>().Model;
			Card c = pam.TopCard();

			Debug.Log("mode: " + Mode);
			switch (Mode) {
				case Mode.Ready:

					if (pam.Card.Params.ContainsKey(ParamType.HisMana) && c == null) {
						PanelInformation.GetComponent<PanelInformation>().SetText("No more spells available.");
					} else  if (c != null) {
						if (c.Cost > panelTile.PanelAvatar.GetComponent<PanelAvatar>().PanelMana.GetComponent<PanelValue>().ActualValue) {
							PanelInformation.GetComponent<PanelInformation>().SetText("You have not enough mana to cast this spell");
						} else if (c.Params.ContainsKey(ParamType.Distance)) {
							SetPlace(panelTile);
						} else if (!c.Params.ContainsKey(ParamType.Distance)) {

							//adding card params to actual card, event the after turn ones
							if (panelTile.PanelAvatar.GetComponent<PanelAvatar>().Model.Card != null) {
								Debug.Log("combining");
								panelTile.PanelInteraction.GetComponent<PanelInteraction>().Prepare(global::Mode.SpellPositioning, c, panelTile, panelTile, Side.None);
								CastSpell(panelTile);
							} else {
								throw new NotImplementedException("What is this case.");
							}

							
						} else {
							throw new Exception("What to do here. What is this case");
						}
					}
					break;
				case Mode.SpellDirectioning:
					if (panelTile.PanelInteraction.GetComponent<PanelInteraction>().IsInMode(Mode.SpellDirectioning)) {

						Side castingSide = panelTile.PanelInteraction.GetComponent<PanelInteraction>().CastingSide;
						panelTile.PanelInteraction.GetComponent<PanelInteraction>().CastingFrom.PanelAvatar.GetComponent<PanelAvatar>().SetDirection(castingSide);
						DisableCastingOnAll(Mode.All);
						Mode = Mode.Ready;
					} else {
						PanelInformation.GetComponent<PanelInformation>().SetText("Select direction");
					}
						
					break;
				case Mode.SpellPositioning:
					if (panelTile.PanelInteraction.GetComponent<PanelInteraction>().IsInMode(Mode.SpellPositioning)) {
						CastSpell(panelTile);
					} else {
						DisableCastingOnAll(Mode.All);
						Mode = Mode.Ready;
					} 
					break;
				default:
					Debug.Log("Default switch, what mode is it? " + Mode);
					break;
			}
			Debug.Log("after mode is: " + Mode);
		} catch (Exception e) {
			Debug.Log("exception: " + e);
		}

	}

	private void SetPlace(PanelTile panelTile){
		Card c = panelTile.PanelAvatar.GetComponent<PanelAvatar>().Model.TopCard();

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

	private void CastSpell(PanelTile panelTile) {
		PanelAvatar pa = panelTile.PanelInteraction.GetComponent<PanelInteraction>().WhoIsCasting.PanelAvatar.GetComponent<PanelAvatar>();
		pa.CastOn(panelTile);
		
		if (panelTile.PanelAvatar.GetComponent<PanelAvatar>().Model.Speed > 0) {
			DisableCastingOnAll(Mode.SpellPositioning);
			SetDirection(panelTile, panelTile.PanelInteraction.GetComponent<PanelInteraction>().WhoIsCasting);
			Mode = Mode.SpellDirectioning;
		} else {
			DisableCastingOnAll(Mode.All);
			Mode = Mode.Ready;
		}
	}

	private void SetDirection(PanelTile castingWhere, PanelTile castersTile) {
		PanelAvatarModel pam = castingWhere.PanelAvatar.GetComponent<PanelAvatar>().Model;
		Card c = pam.Card;

		bool foundAPlace = false;

		foreach (Side s in SideMethods.AllSides()) {
			if (castingWhere.SetInteractionForMode(Mode.SpellDirectioning, c, pam.Speed, s, castersTile, castingWhere)) {
				foundAPlace = true;
			}
		}

		if (!foundAPlace) {
			Debug.Log("There is no place where it can be cast.");
			Mode = Mode.Ready;
		}
	}
}

public enum Mode {
	Ready, SpellDirectioning, SpellPositioning, All
}
