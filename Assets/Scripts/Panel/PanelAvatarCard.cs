using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PanelAvatarCard : MonoBehaviour {

	public GameObject PanelCrystal, PanelAttack, PanelHealth, PanelArmor, CardImage, ImageOval, ImageColor;

	public AvatarModel HeroModel;
	public Card Card;
	public WhereAmI WhereAmI = WhereAmI.TopInfo;

	public void Prepare(AvatarModel heroModel , Card card){
		HeroModel = heroModel;
		Card = card;

		CardImage.SetActive(Card != null);
		ImageColor.SetActive(Card != null);
		ImageOval.SetActive(Card != null);
		PanelCrystal.SetActive(Card != null);
		PanelAttack.SetActive(Card != null);
		PanelHealth.SetActive(Card != null);
		//minion has green background
		ImageColor.GetComponent<Image>().color = (card!=null&&card.CardPersistency.IsCharacter()) ? Color.green : Color.red;
		//minion has oval
		//ImageOval.SetActive(card!=null && Card.CardPersistency.IsCharacter());

		CardImage.GetComponent<Image>().sprite = card != null?card.Animation:null;
	}

	public void Touched(BaseEventData bed) {
		try {
			switch (WhereAmI) {
				case global::WhereAmI.Board:
					PanelMinigame.Me.GetComponent<PanelMinigame>().PointerDownOn(gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<PanelTile>());
					break;
				case global::WhereAmI.Hand:
					PanelMinigame.Me.GetComponent<PanelMinigame>().CardInHandSelected(Card);
					break;
				case global::WhereAmI.TopInfo:
					//nothing to do here;
					Debug.Log("Touched in top info");
					break;
				default:
					throw new NotImplementedException("Not implemented");
			}
		} catch (Exception e) {
			Debug.Log("Exception: " + e);
		}
	}

	public void PreviewCardHand(AvatarModel heroModel, Card card) {
		Prepare(heroModel, card);
		
		PanelCrystal.GetComponent<PanelValue>().Prepare(Card!=null? Card.Cost:0);
		PanelAttack.GetComponent<PanelValue>().Prepare(Card!=null && Card.Params.ContainsKey(ParamType.Attack) ? Card.Params[ParamType.Attack] : 0);
		PanelHealth.GetComponent<PanelValue>().Prepare(Card != null && Card.Params.ContainsKey(ParamType.Health) ? Card.Params[ParamType.Health] : 0);

		CardImage.GetComponent<Image>().color = card != null && heroModel != null && card.Cost > heroModel.ActualMana ? Color.black : Color.white;
		//PanelCrystal.GetComponent<Image>().color = card != null && card.Cost > heroModel.ActualMana ? Color.black : Color.white;
		//PanelCrystal.GetComponent<PanelValue>().Text.GetComponent<Text>().color = card != null && heroModel != null && card.Cost > heroModel.ActualMana ? Color.black : Color.white;
	}

	internal void PreviewModel(AvatarModel heroModel, AvatarModel target, bool friendly) {
		Card c = target != null ? target.Card : null;
		Prepare(heroModel, c);

		//on board there is no color
		ImageColor.SetActive(false);
		PanelAttack.GetComponent<PanelValue>().Prepare(target!=null ? target.ActualAttack:0);
		PanelHealth.GetComponent<PanelValue>().Prepare(target!=null ? target.ActualHealth:0);
		PanelArmor.GetComponent<PanelValue>().Prepare(target != null ? target.Armor : 0);

		if (target != null) {
			CardImage.GetComponent<Image>().color = target.MovesLeft > 0 ? Color.white : Color.black;
			CardImage.GetComponent<Image>().material = Image.defaultGraphicMaterial;

			if (!friendly) {
				CardImage.GetComponent<Image>().color = Color.red;
				CardImage.GetComponent<Image>().material = SpriteManager.Font.material;
			}
		}
	}
}
