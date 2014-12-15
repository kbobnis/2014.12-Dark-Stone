using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class PanelAvatar : MonoBehaviour {

	public GameObject PanelHealth, PanelMana, PanelSpell, PanelAttack, PanelDirection;

	public Card Card;

	public bool IsEmpty() {
		return Card == null;
	}

	public void Prepare(Card card) {
		Card = card;
		if (card != null) {
			if (Card.Params.ContainsKey(ParamType.Health)) {
				PanelHealth.GetComponent<PanelValue>().Prepare(card.Params[ParamType.Health]);
			}
			if (Card.Params.ContainsKey(ParamType.HisMana)) {
				PanelMana.GetComponent<PanelValue>().Prepare(card.Params[ParamType.HisMana]);
			}
			if (Card.Params.ContainsKey(ParamType.Damage)) {
				PanelAttack.GetComponent<PanelValue>().Prepare(Card.Params[ParamType.Damage]);
			}
		}
		UpdateImage();
	}

	public void UpdateImage() {
		GetComponent<Image>().enabled = Card != null;
		if (Card != null) {
			if (!Card.Animations.ContainsKey(AnimationType.OnBoard)) {
				throw new Exception("There is no onBoard animation for card: " + Card.Name);
			}
			GetComponent<Image>().sprite = Card.Animations[AnimationType.OnBoard];
		}
	}

	internal void Clear() {
		Prepare(null);
		PanelDirection.GetComponent<PanelDirection>().Prepare(Side.None, 0);
	}

	public bool HasSpell() {
		return Card != null;
	}

	internal void CastOn(PanelTile panelTile) {
		PanelMana.GetComponent<PanelValue>().ActualValue -= panelTile.PanelInteraction.GetComponent<PanelInteraction>().Card.Cost;
		PanelSpell.GetComponent<PanelSpell>().CastOn(panelTile);
	}

}
