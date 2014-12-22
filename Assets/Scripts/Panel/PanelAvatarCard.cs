using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class PanelAvatarCard : MonoBehaviour {

	public GameObject PanelCost, PanelAttack, PanelHealth;


	internal void Prepare(AvatarModel avatarModel, Card Card, bool enableCostColor) {
		if (Card == null) {
			throw new Exception("Not nice to prepare with empty card");
		}

		GetComponent<Image>().sprite = Card.Animation;
		if (enableCostColor) {
			GetComponent<Image>().color = avatarModel.ActualMana >= Card.Cost ? Color.white : Color.black;
		}

		PanelCost.GetComponent<PanelValue>().Prepare(Card.Cost);
		PanelAttack.GetComponent<PanelValue>().Prepare(Card.Params.ContainsKey(ParamType.Attack) ? Card.Params[ParamType.Attack] : 0);
		PanelHealth.GetComponent<PanelValue>().Prepare(Card.Params.ContainsKey(ParamType.Health) ? Card.Params[ParamType.Health] : 0);
	}
}
