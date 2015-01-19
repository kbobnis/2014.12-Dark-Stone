using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;
using System.Security.Cryptography;


public static class Methods {

	public static void Shuffle<T>(this IList<T> list) {
		RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
		int n = list.Count;
		while (n > 1) {
			byte[] box = new byte[1];
			do provider.GetBytes(box);
			while (!(box[0] < n * (Byte.MaxValue / n)));
			int k = (box[0] % n);
			n--;
			T value = list[k];
			list[k] = list[n];
			list[n] = value;
		}
	}
}

public class AnimationRequestStruct {
	public AnimationRequest AnimationRequest;
	public object Param;
	public AvatarModel AvatarModel;

	public AnimationRequestStruct(AvatarModel avatarModel, global::AnimationRequest animationRequest, object param=null) {
		AnimationRequest = animationRequest;
		AvatarModel = avatarModel;
		Param = param;
	}

}

public class PanelMinigame : MonoBehaviour {

	public GameObject PanelBoardFront, PanelBoardBack, PanelInformation, PanelCardPreview, PanelMyHeroStatus, ButtonEndTurn, PanelEnemyStatus;

	public GameObject AvatarCardPrefab;

	public AvatarModel MyModel, EnemysModel;
	public AvatarModel ActualTurnModel;
	//to be visible in inspector
	public Mode Mode = Mode.Ready;
	public static PanelMinigame Me;

	public List<AnimationRequestStruct> AnimationRequests = new List<AnimationRequestStruct>();
	private bool AnimatingNow = false;
	private float AnimationStart;

	internal void Prepare() {

		AvatarModel am = new AvatarModel(Card.Druid, false, null);

		Me = this;
		PanelInformation.SetActive(true);

		List<List<TileTemplate>> templates = new List<List<TileTemplate>>();
		for (int i = 0; i < 3; i++) {
			List<TileTemplate> col = new List<TileTemplate>();
			for (int j = 0; j < 4; j++) {
				TileTemplate tt = new TileTemplate();
				col.Add(tt);
			}
			templates.Add(col);
		}

		PanelBoardBack.GetComponent<ScrollableList>().Build(templates);

		templates[2][2].AddTemplate(Card.Druid);
		templates[0][1].AddTemplate(Card.Shaman);

		PanelBoardFront.GetComponent<ScrollableList>().Build(templates);
		
		PanelAvatar lasiaAvatar = PanelBoardFront.GetComponent<ScrollableList>().ElementsToPut[2 * 4 + 2].GetComponent<PanelTile>().PanelAvatar.GetComponent<PanelAvatar>();
		AvatarModel lasiaModel = lasiaAvatar.Model;

		List<Card> all = new List<Card>();
		all.Add(Card.AncestralHealing);
		all.Add(Card.StonetuskBoar);
		all.Add(Card.RiverCrocolisk);
		all.Add(Card.Wolfrider);
		all.Add(Card.StormwindKnight);
		all.Add(Card.StormpikeCommando);
		all.Add(Card.RecklessRocketeer);
		all.Add(Card.WarGolem);
		all.Add(Card.Nightblade);
		all.Add(Card.LordOfTheArena);
		all.Add(Card.Archmage);
		all.Add(Card.GurubashiBerserker);
		all.Add(Card.DarkscaleHealer);
		all.Add(Card.BootyBayBodyguard);
		all.Add(Card.OgreMagi);
		all.Add(Card.OasisSnapjaw);
		all.Add(Card.DragonlingMechanic);
		all.Add(Card.SilverbackPatriarch);
		all.Add(Card.RaidLeader);
		all.Add(Card.MagmaRager);
		all.Add(Card.IronforgeRifleman);
		all.Add(Card.NoviceEngineer);
		all.Add(Card.Moonfire);
		all.Add(Card.DalaranMage);
		all.Add(Card.FrostwolfGrunt);
		all.Add(Card.GoldshireFootman);
		all.Add(Card.VoodooDoctor);
		all.Add(Card.ElvenArcher);
		all.Add(Card.MurlocTidehunter);
		all.Add(Card.HealingTouch);
		all.Add(Card.Innervate);
		all.Add(Card.MarkOfTheWild);
		all.Add(Card.IronfurGrizzly);
		all.Add(Card.SavageRoar);
		all.Add(Card.Claw);
		all.Add(Card.GnomishInventor);
		all.Add(Card.ChillwindYeti);
		all.Add(Card.Sprint);
		all.Add(Card.Flamestrike);
		all.Add(Card.CoreHound);
		all.Add(Card.KoboldGeomancer);
		all.Add(Card.RockbiterWeapon);
		all.Add(Card.Swipe);
		all.Add(Card.MindControl);
		all.Add(Card.Starfire);
		all.Add(Card.FlametongueTotem);
		all.Add(Card.RazorfenHunter);
		all.Add(Card.SenjinShieldmasta);
		all.Add(Card.Bloodlust);
		all.Add(Card.BoulderfishOgre);
		all.Add(Card.FireElemental);
		all.Add(Card.FrostwolfWarlord);
		all.Add(Card.Hex);
		all.Add(Card.StormwindChampion);
		all.Add(Card.ShatteredSunCleric);
		all.Add(Card.BloodfenRaptor);
		all.Add(Card.Thrallmar);
		all.Add(Card.Wisp);
		all.Add(Card.IronbarkProtector);

		all.Add(Card.KillTheBullies);
		all.Add(Card.BubblingVolcano);
		all.Add(Card.MagicArrow);

		all.RemoveAll(x => x.Cost > 5);
		 
		all.Shuffle();

		lasiaModel.Deck = all.GetRange(0, 15);
		lasiaModel.Hand.Add(Card.MagicArrow);
		lasiaModel.PullCardFromDeck();
		lasiaModel.PullCardFromDeck();

		PanelAvatar dementorAvatar = PanelBoardFront.GetComponent<ScrollableList>().ElementsToPut[1].GetComponent<PanelTile>().PanelAvatar.GetComponent<PanelAvatar>();
		AvatarModel dementorModel = dementorAvatar.Model;

		all.Shuffle();
		dementorModel.Deck = all.GetRange(0, 15);
		dementorModel.Hand.Add(Card.BubblingVolcano);
		dementorModel.PullCardFromDeck();
		dementorModel.PullCardFromDeck();
		dementorModel.PullCardFromDeck();

		MyModel = lasiaModel;
		EnemysModel = dementorModel;
		
		ActualTurnModel = EnemysModel;
		EndTurn();
		RevalidateEffects();

	}

	void Update() {
		if (MyModel != null && EnemysModel != null && (MyModel.ActualHealth <= 0 || EnemysModel.ActualHealth <= 0)) {
			PanelInformation.GetComponent<PanelInformation>().SetText((MyModel.ActualHealth > 0 ? "You won" : "You lost"));
		}

	}

	public void EndTurn() {
		if (Mode != Mode.Ready) {
			PanelInformation.GetComponent<PanelInformation>().SetText("Finish " + Mode + " first.");
		} else {
			StartCoroutine(EndTurnCoroutine());
		}
	}

	private IEnumerator EndTurnCoroutine() {

		ActualTurnModel.EndOfATurn();

		ActualTurnModel = ActualTurnModel == MyModel?EnemysModel:MyModel;
		ActualTurnModel.StartOfATurn();
		RevalidateEffects();

		ButtonEndTurn.GetComponentInChildren<Text>().text = ActualTurnModel == MyModel?"End turn":"Enemys turn";

		yield return null;
	}


	internal void CardInHandSelected(Card card, bool alreadyPaidFor) {

		if (Mode != global::Mode.CastingSpellNoCancel) {
			AnimationRequests.Add(new AnimationRequestStruct(ActualTurnModel, AnimationRequest.CardInHandSelected, card));
			PanelCardPreview.GetComponent<PanelCardPreview>().PreviewCard(ActualTurnModel, card, WhereAmI.TopInfo);

			DisableAllPanelsInteraction();
			Mode = global::Mode.Ready;

			if (card.Cost <= ActualTurnModel.ActualMana) {
				//getting your main character
				PanelTile panelTile = PanelBoardFront.GetComponent<PanelTiles>().FindTileForModel(ActualTurnModel);
				CastSpell(panelTile, card, panelTile, panelTile, false, card.Cost, alreadyPaidFor);
			}
		}
	}

	/* if finished in action */
	internal bool PointerDownOn(PanelTile panelTile) {
		bool actionWithBattlecryDone = false;
		Debug.Log("pointer down on: " + panelTile.gameObject.name + ", mode: " + Mode);
		PanelInteraction pi = panelTile.PanelInteraction.GetComponent<PanelInteraction>();
		AvatarModel heroModel = panelTile.PanelAvatar.GetComponent<PanelAvatar>().PanelAvatarCard.GetComponent<PanelAvatarCard>().HeroModel;
		AvatarModel targetModel = panelTile.PanelAvatar.GetComponent<PanelAvatar>().Model;

		if (Mode != global::Mode.CastingSpellNoCancel) {
			PanelCardPreview.GetComponent<PanelCardPreview>().Preview(heroModel, targetModel, WhereAmI.TopInfo);
		}

		bool isYourCharacter = ActualTurnModel.IsItYourMinion(targetModel) || ActualTurnModel == heroModel;

		switch (Mode) {
			case global::Mode.Ready: {

					if (isYourCharacter && targetModel != null && targetModel.MovesLeft > 0 && panelTile.SetInteractionToMoveAround()) {
						Mode = global::Mode.MovingOrAttacking;
						actionWithBattlecryDone = true;
					}
					break;
				}
			case global::Mode.MovingOrAttacking: {

				int movesLeft = 0;
				PanelTile whatWantsToMoveOrAttackHere = pi.WhatMoveOrAttack;
				PanelInteractionMode mode = pi.Mode;
				bool hasMoved = false;
				if (pi.Mode == PanelInteractionMode.Moving || pi.Mode == PanelInteractionMode.Attacking) {
					whatWantsToMoveOrAttackHere = pi.WhatMoveOrAttack;
					if (!isYourCharacter) {
						whatWantsToMoveOrAttackHere.PanelAvatar.GetComponent<PanelAvatar>().Model.MovesLeft--;
						movesLeft = whatWantsToMoveOrAttackHere.PanelAvatar.GetComponent<PanelAvatar>().Model.MovesLeft;
						actionWithBattlecryDone = true;
					}
					//is here something?
					if (heroModel != null && !isYourCharacter) {
						//battle
						whatWantsToMoveOrAttackHere.PanelAvatar.GetComponent<PanelAvatar>().BattleOutWith(panelTile.PanelAvatar.GetComponent<PanelAvatar>());
						if (panelTile.PanelAvatar.GetComponent<PanelAvatar>().Model == null) {
							//move if killed
							panelTile.PanelAvatar.GetComponent<PanelAvatar>().Model = whatWantsToMoveOrAttackHere.PanelAvatar.GetComponent<PanelAvatar>().Model;
							whatWantsToMoveOrAttackHere.PanelAvatar.GetComponent<PanelAvatar>().Model = null;
							hasMoved = true;
						}
					} else {
						//move
						panelTile.PanelAvatar.GetComponent<PanelAvatar>().Model = whatWantsToMoveOrAttackHere.PanelAvatar.GetComponent<PanelAvatar>().Model;
						whatWantsToMoveOrAttackHere.PanelAvatar.GetComponent<PanelAvatar>().Model = null;
						hasMoved = true;
					}
					
				}
				DisableAllPanelsInteraction();
				Mode = global::Mode.Ready;
				//automatically enter next move interaction if moves are left
				if (movesLeft > 0) {
					Debug.Log("Moves left: " + movesLeft);
					if (panelTile.PanelAvatar.GetComponent<PanelAvatar>().Model != null && hasMoved) {
						PointerDownOn(panelTile);
					} else {
						PointerDownOn(whatWantsToMoveOrAttackHere);
					}
				}			

				break;
			}
			case global::Mode.CastingSpellNoCancel: {
					if (pi.Mode == PanelInteractionMode.Casting) {
						actionWithBattlecryDone = CastSpell(panelTile, pi.CastersCard, pi.Caster, pi.Caster, true, 0, true); //if no cancel, then it is already paid (hero power or battlecry)
						DisableAllPanelsInteraction();
						Mode = global::Mode.Ready;
						actionWithBattlecryDone = CastEffects(panelTile);
					}
				break;
			}
			case global::Mode.CastingSpell: {

				PanelInteractionMode pim = pi.Mode;
				if (pim == PanelInteractionMode.Casting) {
					actionWithBattlecryDone = CastSpell(panelTile, pi.CastersCard, pi.Caster, pi.Caster, true, pi.CastersCard.Cost, false);
				}
				DisableAllPanelsInteraction();
				Mode = global::Mode.Ready;
				if (pim == PanelInteractionMode.Casting && pi.CastersCard != null) {
					actionWithBattlecryDone = CastEffects(panelTile);
				}
				break;
			}
			default: throw new NotImplementedException("Implement working with mode: " + Mode);
		}

		//revalidate effects
		RevalidateEffects();

		return actionWithBattlecryDone;
	}

	private bool CastEffects(PanelTile panelTile) {
		bool allEffectDone = false;
		bool allEffectsOnceSetTrue = false;

		PanelInteraction pi = panelTile.PanelInteraction.GetComponent<PanelInteraction>();
		AvatarModel heroModel = panelTile.PanelAvatar.GetComponent<PanelAvatar>().PanelAvatarCard.GetComponent<PanelAvatarCard>().HeroModel;
		AvatarModel targetModel = panelTile.PanelAvatar.GetComponent<PanelAvatar>().Model;
		PanelInteractionMode whatMode = pi.Mode;
		Card whatCard = pi.CastersCard;

		List<Card> cardsToCast = new List<Card>();

		if (whatCard.Effects.ContainsKey(Effect.BattlecryNonExistantRandom)) {
			Card c = null;
			int random = new System.Random().Next(whatCard.Effects[Effect.BattlecryNonExistantRandom].Length);
			int count = 0;
			do {
				random++;
				count++;
				if (count > whatCard.Effects[Effect.BattlecryNonExistantRandom].Length) {
					c = null;
					break;
				}
				if (random >= whatCard.Effects[Effect.BattlecryNonExistantRandom].Length) {
					random = 0;
				}
				c = whatCard.Effects[Effect.BattlecryNonExistantRandom][random];
				//check if exists on board
			} while (PanelBoardFront.GetComponent<PanelTiles>().IsModelOfCard(c));
			if (c != null) {
				cardsToCast.Add(c);
			}
		}
		if (whatCard.Effects.ContainsKey(Effect.Battlecry)) {
			Debug.Log("There is battlecry for " + whatCard.Name);
			cardsToCast.AddRange(whatCard.Effects[Effect.Battlecry]);
			Card[] battlecryCards = whatCard.Effects[Effect.Battlecry];

		}
		if (whatCard.Effects.ContainsKey(Effect.WhileAlive) && whatCard.Effects[Effect.WhileAlive][0].CardPersistency == CardPersistency.WhileHolderAlive) {
			cardsToCast.AddRange(whatCard.Effects[Effect.WhileAlive]);
		}

		foreach (Card battlecryCard in cardsToCast) {
			bool explicitly = battlecryCard.CardTarget == CardTarget.Self || battlecryCard.CardTarget == CardTarget.EnemyHero;
			if (explicitly == false) {
				PanelCardPreview.GetComponent<PanelCardPreview>().PreviewCard(heroModel, battlecryCard, WhereAmI.TopInfo);
			}
			if (CastSpell(panelTile, battlecryCard, panelTile, pi.Caster, explicitly, 0, true) ) {
				if (!allEffectsOnceSetTrue) {
					allEffectDone = true;
					allEffectsOnceSetTrue = true;
				}
			} else {
				allEffectDone = false;
			}
		}
		return allEffectDone;
	}

	public bool CastSpell(PanelTile target, Card card, PanelTile caster, PanelTile castingHero, bool explicitlySelectedTile, int cost, bool noCancel) {
		bool anyActionDone = false;
		//Debug.Log("Casting spell " + card.Name + " by: " + caster.PanelAvatar.GetComponent<PanelAvatar>().Model.Card.Name + ", already paid? " + (alreadyPaidFor?"yes":"no"));

		if (explicitlySelectedTile) {
			foreach (PanelTile pt in PanelBoardFront.GetComponent<PanelTiles>().GetAllPanelTiles()) {
				if (castingHero.CanHeHaveThisSpell(target, pt, card)) {
					Debug.Log(castingHero.PanelAvatar.GetComponent<PanelAvatar>().Model.Card.Name + " is casting on " + pt.gameObject.name + " card: " + card.Name);
					castingHero.PanelAvatar.GetComponent<PanelAvatar>().CastOn(pt.PanelAvatar.GetComponent<PanelAvatar>(), card);
					anyActionDone = true;
				}
			}
			if (anyActionDone) {
				castingHero.PanelAvatar.GetComponent<PanelAvatar>().Model._ActualMana -= cost;
			}
		} else {
			List<PanelTile> pts = PanelBoardFront.GetComponent<PanelTiles>().GetAllPanelTiles();
			foreach (PanelTile pt in pts) {
				if (CanBeCastedOn(caster, pt, card)) {
					anyActionDone = true;
					pt.PanelInteraction.GetComponent<PanelInteraction>().CanCastHere(caster, card);
					Mode = noCancel?global::Mode.CastingSpellNoCancel:global::Mode.CastingSpell;
				}
			}
		}
		return anyActionDone;
	}

	private bool CanBeCastedOn(PanelTile caster, PanelTile castedOn, Card card) {

		AvatarModel castersModel = caster.PanelAvatar.GetComponent<PanelAvatar>().Model;
		AvatarModel castedOnModel = castedOn.PanelAvatar.GetComponent<PanelAvatar>().Model;

		bool canCast = false;
		switch (card.CardTarget) {
			case CardTarget.Minion: 
				canCast = castedOnModel != null && castedOnModel.Card.CardPersistency != CardPersistency.Hero; 
				break;
			case CardTarget.Empty: 
				canCast = castedOnModel == null; 
				break;
			case CardTarget.FriendlyMinion: 
				canCast = castedOnModel != null && castedOnModel.Card.CardPersistency != CardPersistency.Hero && castersModel.GetMyHero().IsItYourMinion(castedOnModel); 
				break;
			case CardTarget.EnemyMinion:
				canCast = castedOnModel != null && castedOnModel.Card.CardPersistency == CardPersistency.Minion && castedOnModel.GetMyHero() != castersModel.GetMyHero();
				break;
			case CardTarget.Self: 
				canCast = castersModel == castedOnModel; 
				break;
			case CardTarget.Character:
				canCast = castedOnModel != null;
				break;
			case CardTarget.OtherFriendlyMinion: 
				canCast = castersModel != castedOnModel && castedOnModel != null && castedOnModel.Card.CardPersistency != CardPersistency.Hero && castersModel.GetMyHero().IsItYourMinion(castedOnModel); 
				break;
			case CardTarget.OtherCharacter:
				canCast = castersModel != castedOnModel && castedOnModel != null;
				break;
			case CardTarget.AnyOther:
				canCast = castersModel != castedOnModel;
				break;
			case CardTarget.Any:
				canCast = true;
				break;
			case CardTarget.EnemyCharacter:
				canCast = castedOnModel != null && !castersModel.GetMyHero().IsItYourMinion(castedOnModel) && castersModel.GetMyHero() != castedOnModel.GetMyHero();
				break;
			case CardTarget.EnemyHero:
				canCast = castedOnModel != null && !castersModel.IsFriendlyCharacter(castedOnModel) && castedOnModel.Card.CardPersistency == CardPersistency.Hero;
				break;
			default: 
				throw new NotImplementedException("Not implemented " + card.CardTarget);
		}

		//we have to check the distance
		if (canCast) {
			int distance = caster.GetDistanceTo(castedOn);
			//Debug.Log("Distance between " + castersModel.Card.Name + " and " + castedOn.Row + ", " + castedOn.Column + " is " + distance);
			canCast =  distance <= card.CastDistance;
		}

		return canCast;
	}


	public void RevalidateEffects() {
		PanelBoardFront.GetComponent<PanelTiles>().UpdateAdjacentModels();

		//mark to remove all every action revalidate effects
		List<AvatarModel> allModels = PanelBoardFront.GetComponent<PanelTiles>().GetAllAvatarModels();
		foreach (AvatarModel am in allModels) {

			foreach (CastedCard cc in am.Effects.ToArray()) {
				if (cc.Card.CardPersistency == CardPersistency.EveryActionRevalidate) {
					cc.MarkedToRemove = true;
				}
			}

		}
		
		//add all every action revalidate effects
		foreach (PanelTile pt in PanelBoardFront.GetComponent<PanelTiles>().GetAllPanelTiles()) {

			AvatarModel am = pt.PanelAvatar.GetComponent<PanelAvatar>().Model;
			if (am != null) {
				AvatarModel myHero = am.GetMyHero();

				foreach (KeyValuePair<Effect, Card[]> e in am.Card.Effects) {
					foreach (Card card in e.Value) {
						if (e.Key == Effect.WhileAlive && card.CardPersistency == CardPersistency.EveryActionRevalidate) {
							CastSpell(pt, card, pt, pt, true, 0, true);
						}
					}
				}
			}
		}

		//remove all effect which are still marked to remove
		foreach (PanelTile pt in PanelBoardFront.GetComponent<PanelTiles>().GetAllPanelTiles()) {

			AvatarModel am = pt.PanelAvatar.GetComponent<PanelAvatar>().Model;
			if (am != null) {
				foreach (CastedCard cc in am.Effects.ToArray()) {
					if (cc.Card.CardPersistency == CardPersistency.EveryActionRevalidate) {
						if (cc.MarkedToRemove) {
							am.Effects.Remove(cc);
						}
					}
				}
			}
			pt.PanelAvatar.GetComponent<PanelAvatar>().UpdateFromModel();
		}
		//to update the hand
		PanelMyHeroStatus.GetComponent<PanelHeroStatus>().HeroModel = ActualTurnModel;
		//update the top status
		PanelEnemyStatus.GetComponent<PanelHeroStatus>().HeroModel = MyModel == ActualTurnModel ? EnemysModel : MyModel;
	}

	private void DisableAllPanelsInteraction() {
		Debug.Log("clearing all tiles, meaining: also deselecting potencial card in hand");

		foreach (GameObject tile in PanelBoardFront.GetComponent<ScrollableList>().ElementsToPut) {
			tile.GetComponent<PanelTile>().PanelInteraction.GetComponent<PanelInteraction>().Clear();
		}
	}


}

public enum Mode {
	Ready, MovingOrAttacking,
	CastingSpell,
	CastingSpellNoCancel
}
