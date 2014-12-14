using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class PanelTiles : MonoBehaviour {

	public GameObject PanelInformation;

	public Mode Mode = Mode.Ready;

	internal void PointerDownOn(PanelTile panelTile) {
		try {
			PanelSpell sp = panelTile.PanelAvatar.GetComponent<PanelAvatar>().PanelSpell.GetComponent<PanelSpell>();
			Card c = sp.TopCard();

			Debug.Log("mode: " + Mode);
			switch (Mode) {
				case Mode.Ready:

					if (panelTile.PanelAvatar.GetComponent<PanelAvatar>().Card.Params.ContainsKey(ParamType.HisMana) && panelTile.PanelAvatar.GetComponent<PanelAvatar>().PanelSpell.GetComponent<PanelSpell>().TopCard() == null) {
						PanelInformation.GetComponent<PanelInformation>().SetText("No more spells available.");
					} else  if (c != null) {
						if (c.Cost > panelTile.PanelAvatar.GetComponent<PanelAvatar>().PanelMana.GetComponent<PanelValue>().Value) {
							PanelInformation.GetComponent<PanelInformation>().SetText("You have not enough mana to cast this spell");
						} else if (c.Params.ContainsKey(ParamType.Distance)) {
							SetPlace(panelTile);
						} else if (!c.Params.ContainsKey(ParamType.Distance)) {
							throw new Exception("Spell must have distance!");
						} else {
							throw new Exception("What to do here. What is this case");
						}
					}
					break;
				case Mode.SpellDirectioning:
					if (panelTile.PanelInteraction.GetComponent<PanelInteraction>().IsInMode(Mode.SpellDirectioning)) {
						panelTile.PanelInteraction.GetComponent<PanelInteraction>().CastingFrom.PanelAvatar.GetComponent<PanelAvatar>().PanelDirection.GetComponent<PanelDirection>().Prepare(panelTile.PanelInteraction.GetComponent<PanelInteraction>().CastingSide);
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
						Mode = global::Mode.Ready;
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
		PanelSpell sp = panelTile.PanelAvatar.GetComponent<PanelAvatar>().PanelSpell.GetComponent<PanelSpell>();
		Card c = sp.TopCard();

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
		
		Card c = panelTile.PanelInteraction.GetComponent<PanelInteraction>().Card;
		if (c.Params.ContainsKey(ParamType.Speed)) {
			DisableCastingOnAll(Mode.SpellPositioning);
			SetDirection(panelTile, panelTile.PanelInteraction.GetComponent<PanelInteraction>().WhoIsCasting);
			Mode = Mode.SpellDirectioning;
		} else {
			DisableCastingOnAll(Mode.All);
			Mode = Mode.Ready;
		}
	}

	private void SetDirection(PanelTile castingWhere, PanelTile castersTile) {
		Card c = castingWhere.PanelAvatar.GetComponent<PanelAvatar>().Card;
		Debug.Log("Setting direction for card: " + c.Name);

		bool foundAPlace = false;

		foreach (Side s in SideMethods.AllSides()) {
			if (castingWhere.SetInteractionForMode(Mode.SpellDirectioning, c, c.Params[ParamType.Speed], s, castersTile, castingWhere)) {
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
