using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

public class PanelMinigame : MonoBehaviour {

	public GameObject PanelTiles, PanelTilesBg, PanelInformation, PanelCardPreview, PanelBottom, ButtonEndTurn;

	private AvatarModel MyModel, EnemysModel;
	public AvatarModel ActualTurnModel;
	//to be visible in inspector
	public Mode Mode = Mode.Ready;
	public static PanelMinigame Me;

	internal void Prepare() {
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
		lasiaModel.Deck.Add(Card.RockbiterWeapon);
		lasiaModel.Deck.Add(Card.FlametongueTotem);
		lasiaModel.Deck.Add(Card.RazorfenHunter);
		lasiaModel.Deck.Add(Card.SenjinShieldmasta);
		lasiaModel.Deck.Add(Card.Bloodlust);
		lasiaModel.Deck.Add(Card.BoulderfishOgre);
		lasiaModel.Deck.Add(Card.StormwindChampion);
		lasiaModel.Deck.Add(Card.FireElemental);
		lasiaModel.Deck.Add(Card.FrostwolfWarlord);
		lasiaModel.Deck.Add(Card.GnomishInventor);
		lasiaModel.Deck.Add(Card.ChillwindYeti);
		lasiaModel.Deck.Add(Card.Hex);
		lasiaModel.Deck.Add(Card.ShatteredSunCleric);
		lasiaModel.Deck.Add(Card.MurlocTidehunter);
		lasiaModel.Deck.Add(Card.BloodfenRaptor);
		lasiaModel.Deck.Add(Card.Thrallmar);
		lasiaModel.Deck.Add(Card.Wisp);

		PanelAvatar dementorAvatar = PanelTiles.GetComponent<ScrollableList>().ElementsToPut[2].GetComponent<PanelTile>().PanelAvatar.GetComponent<PanelAvatar>();
		AvatarModel dementorModel = dementorAvatar.Model;
		dementorModel.Deck.Add(Card.RockbiterWeapon);
		dementorModel.Deck.Add(Card.FlametongueTotem);
		dementorModel.Deck.Add(Card.RazorfenHunter);
		dementorModel.Deck.Add(Card.MurlocTidehunter);
		dementorModel.Deck.Add(Card.GnomishInventor);
		dementorModel.Deck.Add(Card.Hex);
		dementorModel.Deck.Add(Card.ShatteredSunCleric);
		dementorModel.Deck.Add(Card.BloodfenRaptor);
		dementorModel.Deck.Add(Card.Thrallmar);
		dementorModel.Deck.Add(Card.Wisp);

		MyModel = lasiaModel;
		EnemysModel = dementorModel;

		PanelBottom.GetComponent<PanelBottom>().Prepare(lasiaModel);

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

		ButtonEndTurn.GetComponentInChildren<Text>().text = ActualTurnModel == MyModel?"End turn":"Enemys turn";

		yield return null;
	}


	internal void CardInHandSelected(Card card) {

		if (Mode != global::Mode.Ready) {
			PanelInformation.GetComponent<PanelInformation>().SetText("Finish actual action before casting spells");
		} else {
			if (card.Cost > ActualTurnModel.ActualMana) {
				PanelInformation.GetComponent<PanelInformation>().SetText("Not enough mana crystals.\nYou have " + ActualTurnModel.ActualMana + " mana crystals. And spell " + card.Name + " costs " + card.Cost + " mana crystals");
			} else {
				//getting your main character
				PanelTile panelTile = PanelTiles.GetComponent<PanelTiles>().FindTileForModel(ActualTurnModel);
				Debug.Log("Card in hand selected, actual hero tile: " + panelTile.PanelAvatar.GetComponent<PanelAvatar>().Model.Card.Name);
				CastSpell(panelTile, card, panelTile, false);
			}
		}
	}


	private bool SetInteractionToMoveAround(PanelTile mover, PanelTile panelTile, Side s, int distance) {
		bool atLeastOneTile = false;
		if (distance > 0) {
			if (panelTile.Neighbours.ContainsKey(s)) {
				if (SetInteractionToMoveAround(mover, panelTile.Neighbours[s], s, distance - 1)) {
					atLeastOneTile = true;
				}
			}
		} else {
			
			AvatarModel am = panelTile.PanelAvatar.GetComponent<PanelAvatar>().Model;
			if (am != null) {

				bool hasPhysicalProtection = false;

				foreach (CastedCard cc in am.Effects) {
					if (cc.Params.ContainsKey(CastedCardParamType.PhysicalProtection)) {
						hasPhysicalProtection = true;
						break;
					}
				}

				//can not attack your own minions nor with physical protection
				if (!hasPhysicalProtection && !ActualTurnModel.IsItYourMinion(panelTile.PanelAvatar.GetComponent<PanelAvatar>().Model) && ActualTurnModel != panelTile.PanelAvatar.GetComponent<PanelAvatar>().Model && mover.PanelAvatar.GetComponent<PanelAvatar>().Model.ActualAttack > 0) {
					panelTile.PanelInteraction.GetComponent<PanelInteraction>().CanAttackHere(mover);
				}

			} else if (panelTile.PanelAvatar.GetComponent<PanelAvatar>().Model == null){
				panelTile.PanelInteraction.GetComponent<PanelInteraction>().CanMoveHere(mover);
				atLeastOneTile = true;
			}
		}
		return atLeastOneTile;
	}

	internal void PointerDownOn(PanelTile panelTile) {
		PanelInteraction pi = panelTile.PanelInteraction.GetComponent<PanelInteraction>();
		AvatarModel am = panelTile.PanelAvatar.GetComponent<PanelAvatar>().Model;
		bool isYourCharacter = ActualTurnModel.IsItYourMinion(am) || ActualTurnModel == am;
		if (am != null) {
			string effects = "";
			foreach (CastedCard cc in am.Effects) {
				string paramT = "(";
				foreach(KeyValuePair<CastedCardParamType, int> kvp in cc.Params){
					paramT += kvp.Key + ": " + kvp.Value + ";";
				}
				paramT += ")";
				effects += cc.Card.Name + paramT + ", ";
			}

			Debug.Log(am.Card.Name + " (hero: " + am.GetMyHero().Card.Name + ") " + am.MovesLeft + " moves left, max health: " + am.MaxHealth + ", his hero: " + am.GetMyHero().Card.Name + ", effects: " + effects);
		}

		switch (Mode) {
			case global::Mode.Ready: {

					if (!isYourCharacter && am != null) {
						PanelInformation.GetComponent<PanelInformation>().SetText("This is your enemys minion");
					}
					if (isYourCharacter) {
						//has no moves
						if (am.MovesLeft <= 0) {
							PanelInformation.GetComponent<PanelInformation>().SetText("No moves left");
						} else {
							//preparing interaction panels for moves
							
							foreach (Side s in SideMethods.AllSides()) {
								if (SetInteractionToMoveAround(panelTile, panelTile, s, 1)){
									Mode = Mode.MovingOrAttacking;
								}
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
					whatWantsToMoveOrAttackHere.PanelAvatar.GetComponent<PanelAvatar>().Model.MovesLeft--;
					movesLeft = whatWantsToMoveOrAttackHere.PanelAvatar.GetComponent<PanelAvatar>().Model.MovesLeft;
					//is here something?
					if (am != null) {
						if ( isYourCharacter) {
							//can not move on your mininion
							PanelInformation.GetComponent<PanelInformation>().SetText("Can not attack your own minion");
						} else {
							//battle
							whatWantsToMoveOrAttackHere.PanelAvatar.GetComponent<PanelAvatar>().BattleOutWith(panelTile.PanelAvatar.GetComponent<PanelAvatar>());
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
					CastSpell(panelTile, pi.CastersCard, pi.Caster, true);
				}
				DisableAllPanelsInteraction();
				Mode = global::Mode.Ready;

				if (whatMode == PanelInteractionMode.Casting && panelTile.PanelAvatar.GetComponent<PanelAvatar>().Model != null) {
					if (whatCard.Effects.ContainsKey(Effect.Battlecry)) {
						Debug.Log("There is battlecry for " + whatCard.Name);
						whatCard = whatCard.Effects[Effect.Battlecry];
						CastSpell(panelTile, whatCard, panelTile, false);
					}
				}
				break;
			}
			default: throw new NotImplementedException("Implement working with mode: " + Mode);
		}

		//revalidate effects
		RevalidateEffects();
	}

	private void CastSpell(PanelTile onWhat, Card card, PanelTile caster, bool explicitlySelectedTile) {

		if (explicitlySelectedTile) {
			foreach (PanelTile pt in PanelTiles.GetComponent<PanelTiles>().GetAllPanelTiles()) {
				if (pt.CanIHaveThisSpell(caster, onWhat, card)) {
					caster.PanelAvatar.GetComponent<PanelAvatar>().CastOn(pt.PanelAvatar.GetComponent<PanelAvatar>(), card);
				}
			}
		} else {
			List<PanelTile> pts = PanelTiles.GetComponent<PanelTiles>().GetAllPanelTiles();
			foreach (PanelTile pa in pts) {
				if (CanBeCastedOn(caster, pa, card)) {
					pa.PanelInteraction.GetComponent<PanelInteraction>().CanCastHere(caster, card);
					Mode = global::Mode.CastingSpell;
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
			case CardTarget.Self: 
				canCast = castersModel == castedOnModel; 
				break;
			case CardTarget.JustThrow: 
				canCast = true;
				break;
			case CardTarget.Character:
				canCast = castedOnModel != null;
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

			//update their minions btw
			foreach (AvatarModel amTmp in am.Minions.ToArray()) {
				if (amTmp.ActualHealth <= 0) {
					am.Minions.Remove(amTmp);
				}
			}

			foreach (CastedCard cc in am.Effects.ToArray()) {
				if (cc.Card.CardPersistency == CardPersistency.EveryActionRevalidate) {
					cc.MarkedToRemove = true;
				}
			}

		}
		
		//add all every action revalidate effects
		foreach (AvatarModel am in allModels) {
			AvatarModel myHero = am.GetMyHero();

			foreach (KeyValuePair<Effect, Card> e in am.Card.Effects) {
				if (e.Key == Effect.WhileAlive && e.Value.CardPersistency == CardPersistency.EveryActionRevalidate){
						
					if (e.Value.Params.ContainsKey(ParamType.AttackAddForAdjacentFriendlyMinions)) {
						foreach (KeyValuePair<Side, AvatarModel> kvp in am.AdjacentModels) {
							if (kvp.Value != null &&  (myHero.IsItYourMinion(kvp.Value) && kvp.Value != myHero)) {
								am.Cast(kvp.Value, e.Value);
							}
						}
					} else if (e.Value.Params.ContainsKey(ParamType.PhysicalProtectionForFriendyAdjacentCharactersracters)) {

						foreach (KeyValuePair<Side, AvatarModel> kvp in am.AdjacentModels) {
							if (kvp.Value != null && (myHero.IsItYourMinion(kvp.Value) || kvp.Value == myHero)) {
								am.Cast(kvp.Value, e.Value);
							}
						}
					} else if (e.Value.Params.ContainsKey(ParamType.AttackAddForOtherFriendlyMinions)){
						foreach (AvatarModel am2 in PanelTiles.GetComponent<PanelTiles>().GetAllAvatarModels()) {
							
							if (am2 != null && am.GetMyHero().IsItYourMinion(am2) && am != am2 && am.GetMyHero() != am2) {
								am.Cast(am2, e.Value);
							}
						}
					}
				}
			}
		}

		//remove all effect which are still marked to remove
		foreach (AvatarModel am in allModels) {
			foreach (CastedCard cc in am.Effects.ToArray()) {
				if (cc.Card.CardPersistency == CardPersistency.EveryActionRevalidate) {
					if (cc.MarkedToRemove) {
						am.Effects.Remove(cc);
					}
				}
			}
		}
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
