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

public class PanelMinigame : MonoBehaviour {

	public GameObject PanelTiles, PanelTilesBg, PanelInformation, PanelCardPreview, PanelBottom, ButtonEndTurn;

	private AvatarModel MyModel, EnemysModel;
	public AvatarModel ActualTurnModel;
	//to be visible in inspector
	public Mode Mode = Mode.Ready;
	public static PanelMinigame Me;

	internal void Prepare() {

		AvatarModel am = new AvatarModel(Card.Lasia, false, null);

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

		PanelTilesBg.GetComponent<ScrollableList>().Build(templates);

		templates[3][2].AddTemplate(Card.Lasia);
		templates[0][2].AddTemplate(Card.Dementor);

		PanelTiles.GetComponent<ScrollableList>().Build(templates);
		
		PanelAvatar lasiaAvatar = PanelTiles.GetComponent<ScrollableList>().ElementsToPut[3 * 5 + 2].GetComponent<PanelTile>().PanelAvatar.GetComponent<PanelAvatar>();
		AvatarModel lasiaModel = lasiaAvatar.Model;
		lasiaModel.Deck.Add(Card.StonetuskBoar);
		lasiaModel.Deck.Add(Card.Moonfire);
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
		lasiaModel.Deck.Add(Card.StormwindChampion);
		lasiaModel.Deck.Add(Card.RazorfenHunter);
		lasiaModel.Deck.Add(Card.SenjinShieldmasta);
		lasiaModel.Deck.Add(Card.Bloodlust);
		lasiaModel.Deck.Add(Card.BoulderfishOgre);
		lasiaModel.Deck.Add(Card.FireElemental);
		lasiaModel.Deck.Add(Card.FrostwolfWarlord);
		lasiaModel.Deck.Add(Card.Hex);
		lasiaModel.Deck.Add(Card.ShatteredSunCleric);
		lasiaModel.Deck.Add(Card.BloodfenRaptor);
		lasiaModel.Deck.Add(Card.Thrallmar);
		lasiaModel.Deck.Add(Card.Wisp);
		lasiaModel.Deck.Add(Card.IronbarkProtector);
		//lasiaModel.Deck.Shuffle();

		PanelAvatar dementorAvatar = PanelTiles.GetComponent<ScrollableList>().ElementsToPut[2].GetComponent<PanelTile>().PanelAvatar.GetComponent<PanelAvatar>();
		AvatarModel dementorModel = dementorAvatar.Model;
		dementorModel.Deck.Add(Card.RockbiterWeapon);
		dementorModel.Deck.Add(Card.FlametongueTotem);
		dementorModel.Deck.Add(Card.RazorfenHunter);
		dementorModel.Deck.Add(Card.SenjinShieldmasta);
		dementorModel.Deck.Add(Card.Bloodlust);
		dementorModel.Deck.Add(Card.BoulderfishOgre);
		dementorModel.Deck.Add(Card.StormwindChampion);
		dementorModel.Deck.Add(Card.FireElemental);
		dementorModel.Deck.Add(Card.FrostwolfWarlord);
		dementorModel.Deck.Add(Card.GnomishInventor);
		dementorModel.Deck.Add(Card.ChillwindYeti);
		dementorModel.Deck.Add(Card.Hex);
		dementorModel.Deck.Add(Card.ShatteredSunCleric);
		dementorModel.Deck.Add(Card.MurlocTidehunter);
		dementorModel.Deck.Add(Card.BloodfenRaptor);
		dementorModel.Deck.Add(Card.Thrallmar);
		dementorModel.Deck.Add(Card.Wisp);
		dementorModel.Deck.Shuffle();

		MyModel = lasiaModel;
		EnemysModel = dementorModel;
		
		ActualTurnModel = EnemysModel;
		EndTurn();
		RevalidateEffects();

		PanelCardPreview.GetComponent<PanelCardPreview>().PreviewCard(lasiaModel, Card.GnomishInventor);
		
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


	internal void CardInHandSelected(Card card) {
		Debug.Log("Card in hand selected: " + (card!=null?card.Name:"empty"));
		PanelCardPreview.GetComponent<PanelCardPreview>().PreviewCard(ActualTurnModel, card);

		if (Mode != global::Mode.Ready) {
			DisableAllPanelsInteraction();
			Mode = global::Mode.Ready;
			//PanelInformation.GetComponent<PanelInformation>().SetText("Finish actual action before casting spells");
		} else {

			if (card.Cost > ActualTurnModel.ActualMana) {
				//PanelInformation.GetComponent<PanelInformation>().SetText("Not enough mana crystals.\nYou have " + ActualTurnModel.ActualMana + " mana crystals. And spell " + card.Name + " costs " + card.Cost + " mana crystals");
			} else {
				//getting your main character
				PanelTile panelTile = PanelTiles.GetComponent<PanelTiles>().FindTileForModel(ActualTurnModel);
				Debug.Log("Card in hand selected, actual hero tile: " + panelTile.PanelAvatar.GetComponent<PanelAvatar>().Model.Card.Name);
				CastSpell(panelTile, card, panelTile, panelTile, false, card.Cost);
			}
		}
	}



	internal void PointerDownOn(PanelTile panelTile) {
		Debug.Log("pointer down on: " + panelTile.gameObject.name);
		PanelInteraction pi = panelTile.PanelInteraction.GetComponent<PanelInteraction>();
		AvatarModel heroModel = panelTile.PanelAvatar.GetComponent<PanelAvatar>().PanelAvatarCard.GetComponent<PanelAvatarCard>().HeroModel;
		AvatarModel targetModel = panelTile.PanelAvatar.GetComponent<PanelAvatar>().Model;
		
		PanelCardPreview.GetComponent<PanelCardPreview>().Preview(heroModel, targetModel);

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

			Debug.Log(heroModel.Card.Name + " (hero: " + heroModel.GetMyHero().Card.Name + ") " + heroModel.MovesLeft + " moves left, max health: " + heroModel.MaxHealth + ", effects: " + effects);
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

					if (mode == PanelInteractionMode.Moving) {
						PointerDownOn(panelTile);
					} else {
						PointerDownOn(whatWantsToMoveOrAttackHere);
					}
				}			

				break;
			}
			case global::Mode.CastingSpell: {

				PanelInteractionMode whatMode = pi.Mode;
				Card whatCard = pi.CastersCard;

				if (pi.Mode == PanelInteractionMode.Casting) {
					Debug.Log("Casting spell " + whatCard.Name + " by: " + pi.Caster.PanelAvatar.GetComponent<PanelAvatar>().Model.Card.Name);
					CastSpell(panelTile, pi.CastersCard, pi.Caster, pi.Caster, true, pi.CastersCard.Cost);
				}
				DisableAllPanelsInteraction();
				Mode = global::Mode.Ready;

				if (whatMode == PanelInteractionMode.Casting && panelTile.PanelAvatar.GetComponent<PanelAvatar>().Model != null) {
					if (whatCard.Effects.ContainsKey(Effect.Battlecry)) {
						Debug.Log("There is battlecry for " + whatCard.Name);
						Card battlecryCard = whatCard.Effects[Effect.Battlecry];
						bool explicitly = battlecryCard.CardTarget == CardTarget.JustThrow || battlecryCard.CardTarget == CardTarget.Self;
						CastSpell(panelTile, battlecryCard, panelTile, pi.Caster, explicitly, 0);
					}
					if (whatCard.Effects.ContainsKey(Effect.WhileAlive) && whatCard.Effects[Effect.WhileAlive].CardPersistency == CardPersistency.WhileHolderAlive) {
						Debug.Log("There is while alive for " + whatCard.Name);
						Card whileAliveEffect = whatCard.Effects[Effect.WhileAlive];
						CastSpell(panelTile, whileAliveEffect, pi.Caster, pi.Caster, true, 0);
					}
				}
				break;
			}
			default: throw new NotImplementedException("Implement working with mode: " + Mode);
		}

		//revalidate effects
		RevalidateEffects();
	}

	private void CastSpell(PanelTile target, Card card, PanelTile caster, PanelTile castingHero, bool explicitlySelectedTile, int cost) {

		if (explicitlySelectedTile) {
			foreach (PanelTile pt in PanelTiles.GetComponent<PanelTiles>().GetAllPanelTiles()) {
				if (castingHero.CanHeHaveThisSpell(target, pt, card)) {
					Debug.Log(caster.PanelAvatar.GetComponent<PanelAvatar>().Model.Card.Name + " is casting on " + pt.gameObject.name + " card: " + card.Name);
					castingHero.PanelAvatar.GetComponent<PanelAvatar>().CastOn(pt.PanelAvatar.GetComponent<PanelAvatar>(), card, cost);
				}
			}
		} else {
			List<PanelTile> pts = PanelTiles.GetComponent<PanelTiles>().GetAllPanelTiles();
			foreach (PanelTile pa in pts) {
				if (CanBeCastedOn(caster, pa, card)) {
					pa.PanelInteraction.GetComponent<PanelInteraction>().CanCastHere(caster, card);
					Mode = global::Mode.CastingSpell;
					if (pa.PanelAvatar.GetComponent<PanelAvatar>().Model != null){
						//Debug.Log("Spell: " + card.Name + ", Can be casted on: " + pa.PanelAvatar.GetComponent<PanelAvatar>().Model.Card.Name);
					}
				}
			}
		}
	}

	private bool CanBeCastedOn(PanelTile caster, PanelTile castedOn, Card card) {

		AvatarModel castedOnModel = castedOn.PanelAvatar.GetComponent<PanelAvatar>().Model;
		AvatarModel castersModel = caster.PanelAvatar.GetComponent<PanelAvatar>().Model;

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
			case CardTarget.JustThrow: 
				canCast = true;
				break;
			case CardTarget.Character:
				canCast = castedOnModel != null;
				break;
			case CardTarget.OtherFriendlyMinion: 
				canCast = castersModel != castedOnModel && castedOnModel != null && castedOnModel.Card.CardPersistency != CardPersistency.Hero && castersModel.GetMyHero().IsItYourMinion(castedOnModel); 
				break;
			case CardTarget.EnemyCharacter:
				canCast = castedOnModel != null && !castersModel.GetMyHero().IsItYourMinion(castedOnModel) && castersModel.GetMyHero() != castedOnModel.GetMyHero();
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
		if (castedOnModel != null) {
			//Debug.Log("Can be casted on: " + castedOnModel.Card.Name + "? " + (canCast ? "yes" : "no"));
		}

		return canCast;
	}


	private void RevalidateEffects() {
		PanelTiles.GetComponent<PanelTiles>().UpdateAdjacentModels();

		//mark to remove all every action revalidate effects
		List<AvatarModel> allModels = PanelTiles.GetComponent<PanelTiles>().GetAllAvatarModels();
		foreach (AvatarModel am in allModels) {

			foreach (CastedCard cc in am.Effects.ToArray()) {
				if (cc.Card.CardPersistency == CardPersistency.EveryActionRevalidate) {
					cc.MarkedToRemove = true;
				}
			}

		}
		
		//add all every action revalidate effects
		foreach (PanelTile pt in PanelTiles.GetComponent<PanelTiles>().GetAllPanelTiles()) {

			AvatarModel am = pt.PanelAvatar.GetComponent<PanelAvatar>().Model;
			if (am != null) {
				AvatarModel myHero = am.GetMyHero();

				foreach (KeyValuePair<Effect, Card> e in am.Card.Effects) {
					if (e.Key == Effect.WhileAlive && e.Value.CardPersistency == CardPersistency.EveryActionRevalidate ) {
						CastSpell(pt, e.Value, pt, pt, true, 0);
					}
				}
			}
		}

		//remove all effect which are still marked to remove
		foreach (PanelTile pt in PanelTiles.GetComponent<PanelTiles>().GetAllPanelTiles()) {

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
		PanelBottom.GetComponent<PanelBottom>().HeroModel = ActualTurnModel;
	}

	private void DisableAllPanelsInteraction() {
		Debug.Log("clearing all tiles");
		foreach (GameObject tile in PanelTiles.GetComponent<ScrollableList>().ElementsToPut) {
			tile.GetComponent<PanelTile>().PanelInteraction.GetComponent<PanelInteraction>().Clear();
		}
	}

}

public enum Mode {
	Ready, MovingOrAttacking,
	CastingSpell
}
