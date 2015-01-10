using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class PanelCardPreview : MonoBehaviour {

	public GameObject PanelAvatar, TextDescription, TextName;


	public void PreviewCard(AvatarModel hero, Card card, WhereAmI whereAmI) {
		PanelAvatar.GetComponent<PanelAvatarCard>().PreviewCardHand(hero, card, whereAmI);
		TextName.GetComponent<Text>().text = card!=null?card.Name:"";
		TextDescription.GetComponent<Text>().text = card!=null?card.Describe(hero):"";
	}

	internal void Preview(AvatarModel hero, AvatarModel targetModel, WhereAmI whereAmI) {
		Card tmp = targetModel != null ? targetModel.Card : null;
		PreviewCard(hero, tmp, whereAmI);
		
		string effectsText = "";
		if (targetModel != null) {
			if (targetModel.Effects.Count > 0) {
				effectsText += "\n effects: ";
			}
			foreach (CastedCard cc in targetModel.Effects) {
				effectsText += "( ";
				foreach (KeyValuePair<CastedCardParamType, int> kvp in cc.Params) {
					effectsText += kvp.Key + ": " + kvp.Value + ",";
				}
				effectsText += "),";
			}
		}

		TextDescription.GetComponent<Text>().text = tmp!=null?(tmp.Describe(hero) + effectsText):"";
	}
}
