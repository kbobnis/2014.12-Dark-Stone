using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class PanelAvatarCard : MonoBehaviour {

	public GameObject PanelCost, PanelAttack, PanelHealth;


	internal void Prepare(Card Card) {
		if (Card == null) {
			throw new Exception("Not nice to prepare with empty card");
		}
		PanelCost.GetComponent<PanelValue>().Prepare(Card.Cost);
		GetComponent<Image>().sprite = Card.Animations[AnimationType.Icon];
		PanelAttack.GetComponent<PanelValue>().Prepare(Card.Params.ContainsKey(ParamType.Damage) ? Card.Params[ParamType.Damage] : 0);
		PanelHealth.GetComponent<PanelValue>().Prepare(Card.Params.ContainsKey(ParamType.Health) ? Card.Params[ParamType.Health] : 0);
	}
}
