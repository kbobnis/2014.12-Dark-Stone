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
		for (int i = 0; i < 4; i++) {
			List<TileTemplate> col = new List<TileTemplate>();
			for (int j = 0; j < 5; j++) {
				TileTemplate tt = new TileTemplate();
				col.Add(tt);
			}
			templates.Add(col);
		}

		PanelBoardBack.GetComponent<ScrollableList>().Build(templates);

		templates[3][2].AddTemplate(Card.Druid);
		templates[0][2].AddTemplate(Card.Shaman);

		PanelBoardFront.GetComponent<ScrollableList>().Build(templates);
		
		PanelAvatar lasiaAvatar = PanelBoardFront.GetComponent<ScrollableList>().ElementsToPut[3 * 5 + 2].GetComponent<PanelTile>().PanelAvatar.GetComponent<PanelAvatar>();
		AvatarModel lasiaModel = lasiaAvatar.Model;
		lasiaModel.Deck.Add(Card.AncestralHealing);
		lasiaModel.Deck.Add(Card.StonetuskBoar);
		lasiaModel.Deck.Add(Card.RiverCrocolisk);
		lasiaModel.Deck.Add(Card.Wolfrider);
		lasiaModel.Deck.Add(Card.StormwindKnight);
		lasiaModel.Deck.Add(Card.StormpikeCommando);
		lasiaModel.Deck.Add(Card.RecklessRocketeer);
		lasiaModel.Deck.Add(Card.WarGolem);
		lasiaModel.Deck.Add(Card.Nightblade);
		lasiaModel.Deck.Add(Card.LordOfTheArena);
		lasiaModel.Deck.Add(Card.Archmage);
		lasiaModel.Deck.Add(Card.GurubashiBerserker);
		lasiaModel.Deck.Add(Card.DarkscaleHealer);
		lasiaModel.Deck.Add(Card.BootyBayBodyguard);
		lasiaModel.Deck.Add(Card.OgreMagi);
		lasiaModel.Deck.Add(Card.OasisSnapjaw);
		lasiaModel.Deck.Add(Card.DragonlingMechanic);
		lasiaModel.Deck.Add(Card.SilverbackPatriarch);
		lasiaModel.Deck.Add(Card.RaidLeader);
		lasiaModel.Deck.Add(Card.MagmaRager);
		lasiaModel.Deck.Add(Card.IronforgeRifleman);
		lasiaModel.Deck.Add(Card.NoviceEngineer);
		lasiaModel.Deck.Add(Card.Moonfire);
		lasiaModel.Deck.Add(Card.DalaranMage);
		lasiaModel.Deck.Add(Card.FrostwolfGrunt);
		lasiaModel.Deck.Add(Card.GoldshireFootman);
		lasiaModel.Deck.Add(Card.VoodooDoctor);
		lasiaModel.Deck.Add(Card.ElvenArcher);
		lasiaModel.Deck.Add(Card.MurlocTidehunter);
		lasiaModel.Deck.Add(Card.HealingTouch);
		lasiaModel.Deck.Add(Card.Innervate);
		lasiaModel.Deck.Add(Card.MarkOfTheWild);
		lasiaModel.Deck.Add(Card.IronfurGrizzly);
		lasiaModel.Deck.Add(Card.SavageRoar);
		lasiaModel.Deck.Add(Card.Claw);
		lasiaModel.Deck.Add(Card.GnomishInventor);
		lasiaModel.Deck.Add(Card.ChillwindYeti);
		lasiaModel.Deck.Add(Card.Sprint);
		lasiaModel.Deck.Add(Card.Flamestrike);
		lasiaModel.Deck.Add(Card.CoreHound);
		lasiaModel.Deck.Add(Card.KoboldGeomancer);
		lasiaModel.Deck.Add(Card.RockbiterWeapon);
		lasiaModel.Deck.Add(Card.Swipe);
		lasiaModel.Deck.Add(Card.MindControl);
		lasiaModel.Deck.Add(Card.Starfire);
		lasiaModel.Deck.Add(Card.FlametongueTotem);
		lasiaModel.Deck.Add(Card.RazorfenHunter);
		lasiaModel.Deck.Add(Card.SenjinShieldmasta);
		lasiaModel.Deck.Add(Card.Bloodlust);
		lasiaModel.Deck.Add(Card.BoulderfishOgre);
		lasiaModel.Deck.Add(Card.FireElemental);
		lasiaModel.Deck.Add(Card.FrostwolfWarlord);
		lasiaModel.Deck.Add(Card.Hex);
		lasiaModel.Deck.Add(Card.StormwindChampion);
		lasiaModel.Deck.Add(Card.ShatteredSunCleric);
		lasiaModel.Deck.Add(Card.BloodfenRaptor);
		lasiaModel.Deck.Add(Card.Thrallmar);
		lasiaModel.Deck.Add(Card.Wisp);
		lasiaModel.Deck.Add(Card.IronbarkProtector);
		 
		lasiaModel.Deck.Shuffle();
		lasiaModel.PullCardFromDeck();
		lasiaModel.PullCardFromDeck();
		lasiaModel.PullCardFromDeck();
		lasiaModel.PullCardFromDeck();

		PanelAvatar dementorAvatar = PanelBoardFront.GetComponent<ScrollableList>().ElementsToPut[2].GetComponent<PanelTile>().PanelAvatar.GetComponent<PanelAvatar>();
		AvatarModel dementorModel = dementorAvatar.Model;
		foreach (Card c in lasiaModel.Deck) {
			dementorModel.Deck.Add(c);
		}
		dementorModel.Deck.Shuffle();
		dementorModel.PullCardFromDeck();
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
		//Debug.Log("Card in hand selected: " + (card!=null?card.Name:"empty"));

		if (Mode != global::Mode.CastingSpellNoCancel) {
			AnimationRequests.Add(new AnimationRequestStruct(ActualTurnModel, AnimationRequest.CardInHandSelected, card));
			PanelCardPreview.GetComponent<PanelCardPreview>().PreviewCard(ActualTurnModel, card, WhereAmI.TopInfo);

			DisableAllPanelsInteraction();
			Mode = global::Mode.Ready;

			if (card.Cost <= ActualTurnModel.ActualMana) {
				//getting your main character
				PanelTile panelTile = PanelBoardFront.GetComponent<PanelTiles>().FindTileForModel(ActualTurnModel);
				//Debug.Log("Card in hand selected, actual hero tile: " + panelTile.PanelAvatar.GetComponent<PanelAvatar>().Model.Card.Name);
				CastSpell(panelTile, card, panelTile, panelTile, false, card.Cost, alreadyPaidFor);
			}
		}
	}

	internal void PointerDownOn(PanelTile panelTile) {
		Debug.Log("pointer down on: " + panelTile.gameObject.name + ", mode: " + Mode);
		PanelInteraction pi = panelTile.PanelInteraction.GetComponent<PanelInteraction>();
		AvatarModel heroModel = panelTile.PanelAvatar.GetComponent<PanelAvatar>().PanelAvatarCard.GetComponent<PanelAvatarCard>().HeroModel;
		AvatarModel targetModel = panelTile.PanelAvatar.GetComponent<PanelAvatar>().Model;

		if (Mode != global::Mode.CastingSpellNoCancel) {
			PanelCardPreview.GetComponent<PanelCardPreview>().Preview(heroModel, targetModel, WhereAmI.TopInfo);
		}

		bool isYourCharacter = ActualTurnModel.IsItYourMinion(targetModel) || ActualTurnModel == heroModel;
		if (heroModel != null) {
			string effects = "";
			foreach (CastedCard cc in heroModel.Effects) {
				string paramT = "(";
				foreach(KeyValuePair<CastedCardParamType, int> kvp in cc.Params){
					paramT += kvp.Key + ": " + kvp.Value + ";";
				}
				paramT += ")";
				effects += cc.Card.Name + paramT + ", ";
			}

			//Debug.Log(heroModel.Card.Name + " (hero: " + heroModel.GetMyHero().Card.Name + ") " + heroModel.MovesLeft + " moves left, max health: " + heroModel.MaxHealth + ", effects: " + effects);
		}

		switch (Mode) {
			case global::Mode.Ready: {

					if (!isYourCharacter && targetModel != null) {
						//PanelInformation.GetComponent<PanelInformation>().SetText("This is your enemys minion");
					}
					if (isYourCharacter) {
						//has no moves
						if (targetModel.MovesLeft <= 0) {
							//PanelInformation.GetComponent<PanelInformation>().SetText("No moves left");
						} else {

							if (panelTile.SetInteractionToMoveAround()) {
								Mode = global::Mode.MovingOrAttacking;
							}
						}
					}
					break;
				}
			case global::Mode.MovingOrAttacking: {

				int movesLeft = 0;
				PanelTile whatWantsToMoveOrAttackHere = pi.WhatMoveOrAttack;
				PanelInteractionMode mode = pi.Mode;
				if (pi.Mode == PanelInteractionMode.Moving || pi.Mode == PanelInteractionMode.Attacking) {
					whatWantsToMoveOrAttackHere = pi.WhatMoveOrAttack;
					if (!isYourCharacter) {
						whatWantsToMoveOrAttackHere.PanelAvatar.GetComponent<PanelAvatar>().Model.MovesLeft--;
						movesLeft = whatWantsToMoveOrAttackHere.PanelAvatar.GetComponent<PanelAvatar>().Model.MovesLeft;
					}
					//is here something?
					if (heroModel != null) {
						if ( isYourCharacter) {
							//can not move on your mininion
							PanelInformation.GetComponent<PanelInformation>().SetText("Can not attack your own minion");
						} else {
							//battle
							whatWantsToMoveOrAttackHere.PanelAvatar.GetComponent<PanelAvatar>().BattleOutWith(panelTile.PanelAvatar.GetComponent<PanelAvatar>());
							if (panelTile.PanelAvatar.GetComponent<PanelAvatar>().Model == null) {
								//move if killed
								panelTile.PanelAvatar.GetComponent<PanelAvatar>().Model = whatWantsToMoveOrAttackHere.PanelAvatar.GetComponent<PanelAvatar>().Model;
								whatWantsToMoveOrAttackHere.PanelAvatar.GetComponent<PanelAvatar>().Model = null;
							}
						}
					} else {
						//move
						panelTile.PanelAvatar.GetComponent<PanelAvatar>().Model = whatWantsToMoveOrAttackHere.PanelAvatar.GetComponent<PanelAvatar>().Model;
						whatWantsToMoveOrAttackHere.PanelAvatar.GetComponent<PanelAvatar>().Model = null;
					}
					
				}
				DisableAllPanelsInteraction();
				Mode = global::Mode.Ready;
				//automatically enter next move interaction if moves are left
				if (movesLeft > 0) {
					Debug.Log("Moves left: " + movesLeft);
					PointerDownOn(panelTile);
				}			

				break;
			}
			case global::Mode.CastingSpellNoCancel: {
					if (pi.Mode == PanelInteractionMode.Casting) {
						CastSpell(panelTile, pi.CastersCard, pi.Caster, pi.Caster, true, 0, true); //if no cancel, then it is already paid (hero power or battlecry)
						DisableAllPanelsInteraction();
						Mode = global::Mode.Ready;
						CastEffects(panelTile);
					}
				break;
			}
			case global::Mode.CastingSpell: {

				if (pi.Mode == PanelInteractionMode.Casting) {
					CastSpell(panelTile, pi.CastersCard, pi.Caster, pi.Caster, true, pi.CastersCard.Cost, false);
				}
				DisableAllPanelsInteraction();
				Mode = global::Mode.Ready;
				if (pi.CastersCard != null) {
					CastEffects(panelTile);
				}
				break;
			}
			default: throw new NotImplementedException("Implement working with mode: " + Mode);
		}

		//revalidate effects
		RevalidateEffects();
	}

	private void CastEffects(PanelTile panelTile) {

		PanelInteraction pi = panelTile.PanelInteraction.GetComponent<PanelInteraction>();
		AvatarModel heroModel = panelTile.PanelAvatar.GetComponent<PanelAvatar>().PanelAvatarCard.GetComponent<PanelAvatarCard>().HeroModel;
		AvatarModel targetModel = panelTile.PanelAvatar.GetComponent<PanelAvatar>().Model;
		PanelInteractionMode whatMode = pi.Mode;
		Card whatCard = pi.CastersCard;

		List<Card> cardsToCast = new List<Card>();

		if (whatCard.Effects.ContainsKey(Effect.BattlecryNonExistantRandom)) {
			Debug.Log("There is battlecry non existant random");
			Card c = null;
			int random = new System.Random().Next(whatCard.Effects[Effect.BattlecryNonExistantRandom].Length);
			int count = 0;
			do {
				random++;
				count++;
				if (count > whatCard.Effects[Effect.BattlecryNonExistantRandom].Length) {
					break;
				}
				if (random >= whatCard.Effects[Effect.BattlecryNonExistantRandom].Length) {
					random = 0;
				}
				c = whatCard.Effects[Effect.BattlecryNonExistantRandom][random];
				Debug.Log("Random totem found: " + c.Name);
				//check if exists on board
			} while (PanelBoardFront.GetComponent<PanelTiles>().IsModelOfCard(c));
			if (c != null) {
				Debug.Log("Adding totem to cards to cast: " + c);
				cardsToCast.Add(c);
			}
		}
		if (whatCard.Effects.ContainsKey(Effect.Battlecry)) {
			Debug.Log("There is battlecry for " + whatCard.Name);
			cardsToCast.AddRange(whatCard.Effects[Effect.Battlecry]);
			Card[] battlecryCards = whatCard.Effects[Effect.Battlecry];

		}
		if (whatCard.Effects.ContainsKey(Effect.WhileAlive) && whatCard.Effects[Effect.WhileAlive][0].CardPersistency == CardPersistency.WhileHolderAlive) {
			Debug.Log("There is while alive for " + whatCard.Name);
			cardsToCast.AddRange(whatCard.Effects[Effect.WhileAlive]);
		}

		foreach (Card battlecryCard in cardsToCast) {
			bool explicitly = battlecryCard.CardTarget == CardTarget.Self || battlecryCard.CardTarget == CardTarget.EnemyHero;
			if (explicitly == false) {
				PanelCardPreview.GetComponent<PanelCardPreview>().PreviewCard(heroModel, battlecryCard, WhereAmI.TopInfo);
			}
			CastSpell(panelTile, battlecryCard, panelTile, pi.Caster, explicitly, 0, true);
		}
	}

	public void CastSpell(PanelTile target, Card card, PanelTile caster, PanelTile castingHero, bool explicitlySelectedTile, int cost, bool alreadyPaidFor) {
		//Debug.Log("Casting spell " + card.Name + " by: " + caster.PanelAvatar.GetComponent<PanelAvatar>().Model.Card.Name + ", already paid? " + (alreadyPaidFor?"yes":"no"));

		if (explicitlySelectedTile) {
			foreach (PanelTile pt in PanelBoardFront.GetComponent<PanelTiles>().GetAllPanelTiles()) {
				if (castingHero.CanHeHaveThisSpell(target, pt, card)) {
					//Debug.Log(caster.PanelAvatar.GetComponent<PanelAvatar>().Model.Card.Name + " is casting on " + pt.gameObject.name + " card: " + card.Name);
					castingHero.PanelAvatar.GetComponent<PanelAvatar>().CastOn(pt.PanelAvatar.GetComponent<PanelAvatar>(), card, cost);
				}
			}
		} else {
			List<PanelTile> pts = PanelBoardFront.GetComponent<PanelTiles>().GetAllPanelTiles();
			foreach (PanelTile pt in pts) {
				if (CanBeCastedOn(caster, pt, card)) {
					pt.PanelInteraction.GetComponent<PanelInteraction>().CanCastHere(caster, card);
					Mode = alreadyPaidFor?global::Mode.CastingSpellNoCancel:global::Mode.CastingSpell;
				}
			}
		}
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
