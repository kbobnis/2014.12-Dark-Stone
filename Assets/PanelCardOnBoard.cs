using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class PanelCardOnBoard : MonoBehaviour {

	public GameObject ImageHealth, PanelSpell;
	
	public Card TemplateCard;
	public PanelCardOnBoard Caster;

	public void Prepare(Card card, PanelCardOnBoard caster) {
		if (TemplateCard != null) {
			throw new System.Exception("Card already on this tile. Implement it");
		}
		Caster = caster;
		TemplateCard = card;

		GetComponent<Image>().enabled = card != null;
		ImageHealth.SetActive(card != null);
		if (card != null) {
			ImageHealth.SetActive(card.Params.ContainsKey(ParamType.Health));
			if (card != null) {
				if (!card.Animations.ContainsKey(AnimationType.OnBoard)) {
					throw new Exception("There is no onBoard animation for card: " + card.Name);
				}
				GetComponent<Image>().sprite = card.Animations[AnimationType.OnBoard];
				if (card.Params.ContainsKey(ParamType.Health)) {
					ImageHealth.GetComponent<PanelValue>().Value = (int) card.Params[ParamType.Health];
				}
			}
		}
	}

	internal void Cast(Card card, PanelCardOnBoard where) {
		if (TemplateCard == null ) {
			throw new Exception("You can not cast from empty tile");
		}
		where.Prepare(card, this);
		PanelSpell.GetComponent<PanelSpell>().CardFeeders.Add(where.PanelSpell.GetComponent<PanelSpell>());

	}

	internal void AddCardTemplate(Card card) {
		PanelSpell.GetComponent<PanelSpell>().AddCard(card);
	}
}
