using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

public class PanelSpell : MonoBehaviour {

	public GameObject ImageSpell;

	public List<Card> CardStack = new List<Card>();
	public List<PanelSpell> CardFeeders = new List<PanelSpell>();

	void Awake() {
		UpdateImage();
	}

	void Update() {
		if (CardStack.Count == 0 && CardFeeders.Count > 0) {
			foreach (PanelSpell ps in CardFeeders) {
				Card c = ps.TryToPullCard();
				if (c != null) {
					AddCard(c);
					break;
				}
			}
			UpdateImage();
		}
	}

	public Card TopCard() {
		return CardStack.Count>0?CardStack[0]:null;
	}

	private Card TryToPullCard() {
		Card c = null;
		if (CardStack.Count > 0) {
			c = CardStack[0];
			CardStack.Remove(c);
			UpdateImage();
		}
		return c;
	}

	internal void AddCard(Card card) {
		CardStack.Add(card);
		UpdateImage();
	}

	private void UpdateImage() {
		GetComponent<Image>().enabled = false;
		ImageSpell.SetActive(false);
		if (CardStack.Count > 0) {
			GetComponent<Image>().enabled = true;
			ImageSpell.SetActive(true);
			if (!CardStack[0].Animations.ContainsKey(AnimationType.Icon)) {
				throw new Exception("Card: " + CardStack[0].Name + ", has no icon animation.");
			}
			ImageSpell.GetComponent<Image>().sprite = CardStack[0].Animations[AnimationType.Icon];
		}
	}

	internal void CastOn(PanelTile panelTile) {
		Card c = TryToPullCard();
		if (c == null) {
			throw new Exception("Why you cast when there is no card");
		}
		CardFeeders.Add(panelTile.GetComponent<PanelTile>().PanelSpell.GetComponent<PanelSpell>());
		panelTile.Prepare(c);
	}
}
