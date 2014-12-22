using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public static class CardMethods {
	
	public static string Describe(this Card Card, bool shortV=false){
		string text = "";
		foreach (KeyValuePair<ParamType, int> kvp in Card.Params) {
			if ((shortV || (kvp.Key != ParamType.Attack && kvp.Key != ParamType.Health)) && (kvp.Key != ParamType.Speed || kvp.Value != 1)) {
				text += kvp.Key + ": " + kvp.Value + ", ";
			}
		}
		if (!shortV) {
			text += "\n\n";
		}
		if (Card.CardTarget != CardTarget.JustThrow && Card.CardTarget != CardTarget.Self && Card.CardTarget != CardTarget.Empty) {
			text += "Cast on: " + Card.CardTarget + ",\n";
		}
		if (Card.IsCastOn != IsCastOn.Target && Card.CardTarget != CardTarget.Empty) {
			text += "Affects: " + Card.IsCastOn + ",\n";
		}
		
		foreach (KeyValuePair<Effect, Card> kvp in Card.Effects) {
			if (kvp.Key != Effect.WhileAlive) {
				text += kvp.Key + ": ";
			}
			text += kvp.Value.Describe(true) + ", ";
		}
		if (!shortV) {
			text += "\n";
		}


		if (Card.CardPersistency != CardPersistency.EveryActionRevalidate && Card.CardPersistency != CardPersistency.WhileHolderAlive && Card.CardPersistency != CardPersistency.Instant) {
			text += Card.CardPersistency + ",";
		}
		return text;
	}
}

public class PanelCardPreview : MonoBehaviour {

	public GameObject PanelAvatar, PanelDetails;

	internal void Preview(AvatarModel avatarModel, Card Card, bool showCost) {
		PanelAvatar.GetComponent<PanelAvatarCard>().Prepare(avatarModel, Card, showCost);
		PanelDetails.GetComponentInChildren<Text>().text = Card.Name + "\n" + Card.Describe();
	}
}
