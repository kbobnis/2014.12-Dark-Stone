using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class PanelAvatar : MonoBehaviour {

	public GameObject PanelHealth, PanelMana, PanelSpell;

	private Card Card;

	public void Prepare(Card card) {

		GetComponent<Image>().enabled = card != null;
		PanelHealth.SetActive(false);
		PanelMana.SetActive(false);
		if (card != null) {
			if (!card.Animations.ContainsKey(AnimationType.OnBoard)) {
				throw new Exception("There is no onBoard animation for card: " + card.Name);
			}
			Card = card;
			GetComponent<Image>().sprite = card.Animations[AnimationType.OnBoard];
			if (card.Params.ContainsKey(ParamType.Health)) {
				PanelHealth.SetActive(true);
				PanelHealth.GetComponent<PanelValue>().Value = (int)card.Params[ParamType.Health];
			}
			
			if (card.Params.ContainsKey(ParamType.HisMana)) {
				PanelMana.SetActive(true);
				PanelMana.GetComponent<PanelValue>().Value = card.Params[ParamType.HisMana];
			}
		}
	}

	public bool HasSpell() {
		return Card != null;
	}


	internal void CastOn(PanelTile panelTile) {

		PanelMana.GetComponent<PanelValue>().Value -= panelTile.PanelInteraction.GetComponent<PanelInteraction>().Card.Cost;
		PanelSpell.GetComponent<PanelSpell>().CastOn(panelTile);
	}
}
