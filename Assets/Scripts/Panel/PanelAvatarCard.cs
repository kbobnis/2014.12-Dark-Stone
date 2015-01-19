using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PanelAvatarCard : MonoBehaviour {

	public GameObject PanelCrystal, PanelAttack, PanelHealth, PanelArmor, CardImage, ImageOval, ImageColor, ImageBorder, ImageTaunt, ImageSticky;

	public AvatarModel HeroModel;
	public Card Card;
	public WhereAmI WhereAmI = WhereAmI.TopInfo;

	public void Prepare(AvatarModel heroModel , Card card, WhereAmI whereAmI){
		HeroModel = heroModel;
		Card = card;

		CardImage.SetActive(Card != null);
		ImageColor.SetActive(Card != null);
		ImageOval.SetActive(Card != null);
		PanelCrystal.SetActive(Card != null);
		PanelAttack.SetActive(Card != null);
		PanelHealth.SetActive(Card != null);
		ImageTaunt.SetActive(false);
		ImageSticky.SetActive(false);
		//minion has green background
		ImageColor.GetComponent<Image>().color = (card!=null&&card.CardPersistency.IsCharacter()) ? Color.blue : Color.red;
		//minion has oval
		//ImageOval.SetActive(card!=null && Card.CardPersistency.IsCharacter());

		CardImage.GetComponent<Image>().sprite = card != null?card.Animation:null;

		ImageBorder.SetActive(whereAmI == global::WhereAmI.Hand || whereAmI == global::WhereAmI.SpecialPower || whereAmI == global::WhereAmI.Board);
	}

	public void Touched(BaseEventData bed) {
		try {
			switch (WhereAmI) {
				case global::WhereAmI.Board:
					PanelMinigame.Me.GetComponent<PanelMinigame>().PointerDownOn(gameObject.transform.parent.gameObject.transform.parent.gameObject.GetComponent<PanelTile>());
					break;
				case global::WhereAmI.Hand:
					PanelMinigame.Me.GetComponent<PanelMinigame>().CardInHandSelected(Card, false);
					break;
				case global::WhereAmI.SpecialPower:

					if (PanelMinigame.Me.Mode != Mode.CastingSpellNoCancel ) {
						PanelMinigame.Me.PanelCardPreview.GetComponent<PanelCardPreview>().PreviewCard(PanelMinigame.Me.ActualTurnModel, Card, global::WhereAmI.TopInfo);

						if (HeroModel == PanelMinigame.Me.ActualTurnModel && HeroModel.ActualMana >= Card.Cost && !HeroModel.AlreadyUsedPower) {

							bool anyActionDone = false;

							PanelMinigame.Me.CardInHandSelected(Card, true);
							if (Card.CardTarget == CardTarget.Self) {
								PanelTile actualTurnHerosTile = PanelMinigame.Me.PanelBoardFront.GetComponent<PanelTiles>().FindTileForModel(PanelMinigame.Me.ActualTurnModel);
								anyActionDone = PanelMinigame.Me.PointerDownOn(actualTurnHerosTile);
							}
							if (Card.CardTarget == CardTarget.EnemyHero) {
								PanelTile actualTurnHerosTile = PanelMinigame.Me.PanelBoardFront.GetComponent<PanelTiles>().FindTileForModel(PanelMinigame.Me.ActualTurnModel == PanelMinigame.Me.MyModel ? PanelMinigame.Me.EnemysModel : PanelMinigame.Me.MyModel);
								anyActionDone = PanelMinigame.Me.PointerDownOn(actualTurnHerosTile);
							}
							//mana deduction has to be after pointer down on, because the spell will no select
							if (anyActionDone) {
								HeroModel.AlreadyUsedPower = true;
								HeroModel._ActualMana -= Card.Cost;
								PanelMinigame.Me.RevalidateEffects();
							} else {
								PanelMinigame.Me.PanelInformation.GetComponent<PanelInformation>().SetText("There is no place or target for special power");
							}
						}
					}
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

	public void PreviewCardHand(AvatarModel heroModel, Card card, WhereAmI whereAmI) {

		Prepare(heroModel, card, whereAmI);
		
		PanelCrystal.GetComponent<PanelValue>().Prepare(Card!=null? Card.Cost:0);
		PanelAttack.GetComponent<PanelValue>().Prepare(Card!=null && Card.Params.ContainsKey(ParamType.Attack) ? Card.Params[ParamType.Attack] : 0);
		PanelHealth.GetComponent<PanelValue>().Prepare(Card != null && Card.Params.ContainsKey(ParamType.Health) ? Card.Params[ParamType.Health] : 0);

		ImageBorder.GetComponent<Image>().color = card != null && heroModel != null && card.Cost > heroModel.ActualMana ? Color.black : (whereAmI == global::WhereAmI.SpecialPower && heroModel.AlreadyUsedPower ? Color.black : Color.green );
		if (whereAmI == global::WhereAmI.TopInfo) {
			ImageBorder.GetComponent<Image>().color = Color.white;
		}
		//PanelCrystal.GetComponent<Image>().color = card != null && card.Cost > heroModel.ActualMana ? Color.black : Color.white;
		//PanelCrystal.GetComponent<PanelValue>().Text.GetComponent<Text>().color = card != null && heroModel != null && card.Cost > heroModel.ActualMana ? Color.black : Color.white;
	}

	internal void PreviewModel(AvatarModel heroModel, AvatarModel target, bool friendly) {
		Card c = target != null ? target.Card : null;
		Prepare(heroModel, c, global::WhereAmI.Board);

		//on board there is no color
		ImageColor.SetActive(false);
		PanelAttack.GetComponent<PanelValue>().Prepare(target!=null ? target.ActualAttack:0);
		PanelHealth.GetComponent<PanelValue>().Prepare(target!=null ? target.ActualHealth:0);
		PanelArmor.GetComponent<PanelValue>().Prepare(target != null ? target.Armor : 0);

		ImageBorder.SetActive(target != null);
		if (target != null) {
			ImageBorder.GetComponent<Image>().color = target.MovesLeft > 0 ? Color.green : Color.black;
			ImageBorder.GetComponent<Image>().material = Image.defaultGraphicMaterial;

			if (!friendly) {
				ImageBorder.GetComponent<Image>().color = Color.red;
				ImageBorder.GetComponent<Image>().material = SpriteManager.Font.material;
			}
		}
		bool hasTaunt = false;
		bool hasSticky = false;
		if (target != null) {
			foreach (CastedCard cc in target.Effects) {
				if (cc.Params.ContainsKey(CastedCardParamType.Taunt)) {
					hasTaunt = true;
				}
				if (cc.Params.ContainsKey(CastedCardParamType.Sticky)){
					hasSticky = true;
				}
			}
		}
		ImageTaunt.SetActive(hasTaunt);
		ImageSticky.SetActive(hasSticky);

	}
}
