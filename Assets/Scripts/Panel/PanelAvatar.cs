using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class PanelAvatar : MonoBehaviour {

	public GameObject PanelHealth;

	public void Prepare(Card card) {

		GetComponent<Image>().enabled = card != null;
		PanelHealth.SetActive(false);
		if (card != null) {
			if (!card.Animations.ContainsKey(AnimationType.OnBoard)) {
				throw new Exception("There is no onBoard animation for card: " + card.Name);
			}
			GetComponent<Image>().sprite = card.Animations[AnimationType.OnBoard];
			PanelHealth.SetActive(card.Params.ContainsKey(ParamType.Health));
			if (card.Params.ContainsKey(ParamType.Health)) {
				PanelHealth.GetComponent<PanelValue>().Value = (int)card.Params[ParamType.Health];
			}
		}
	}
}
