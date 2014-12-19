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
		lasiaModel.Deck.Add(Card.GnomishInventor);
		lasiaModel.Deck.Add(Card.ChillwindYeti);
		lasiaModel.Deck.Add(Card.Hex);
		lasiaModel.Deck.Add(Card.ShatteredSunCleric);
		lasiaModel.Deck.Add(Card.MurlocTidehunter);
		lasiaModel.Deck.Add(Card.BloodfenRaptor);
		lasiaModel.Deck.Add(Card.Thrallmar);
		lasiaModel.Deck.Add(Card.Wisp);
		lasiaModel.Deck.Add(Card.Fireball);
		lasiaModel.Deck.Add(Card.IceBolt);
		lasiaModel.Deck.Add(Card.Mud);
		lasiaModel.Deck.Add(Card.IceBolt);
		lasiaModel.Deck.Add(Card.Mud);

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
		dementorModel.Deck.Add(Card.Fireball);
		dementorModel.Deck.Add(Card.IceBolt);
		dementorModel.Deck.Add(Card.Mud);
		dementorModel.Deck.Add(Card.IceBolt);
		dementorModel.Deck.Add(Card.Mud);

		MyModel = lasiaModel;
		EnemysModel = dementorModel;

		PanelBottom.GetComponent<PanelBottom>().Prepare(lasiaModel);

		ActualTurnModel = EnemysModel;
		EndTurn();
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
				int distance = card.Params.ContainsKey(ParamType.Distance) ? card.Params[ParamType.Distance] : 0;
				foreach (Side s in SideMethods.AllSides()) {
					if (SetInteractionToCastAround(ActualTurnModel, panelTile, card, s, distance)) {
						Mode = global::Mode.CastingSpell;
					}
				}
			}
		}
	}

	private bool SetInteractionToCastAround(AvatarModel caster, PanelTile panelTile, Card card, Side s, int distance) {
		bool atLeastOneTile = false;
		if (distance > 0){
			if (panelTile.Neighbours.ContainsKey(s)) {
				if (SetInteractionToCastAround(caster, panelTile.Neighbours[s], card, s, distance - 1)) {
					atLeastOneTile = true;
				}
			}
		}
		//you can not cast one minion on top of the other
		AvatarModel am = panelTile.PanelAvatar.GetComponent<PanelAvatar>().Model;
		if (card.Params.ContainsKey(ParamType.Health) && am != null && !card.Params.ContainsKey(ParamType.ReplaceExisting)) {
			return atLeastOneTile;
		}

		if (am == null) {
			//you can not cast healing touch on empty area
			if (card.Params.ContainsKey(ParamType.Heal)) {
				return atLeastOneTile;
			}

			//damage spell can not be cast on empty area 
			if (card.Params.ContainsKey(ParamType.Damage)) {
				return atLeastOneTile;
			}

			if (card.Params.ContainsKey(ParamType.AttackThisTurn)) {
				return atLeastOneTile;
			}
		}

		panelTile.PanelInteraction.GetComponent<PanelInteraction>().CanCastHere(caster, card);
		atLeastOneTile = true;
		return atLeastOneTile;
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
			
			//it there is a taunter nearby, you can not attack nor move here
			bool enemysTaunterFound = false;
			foreach (Side sTmp in SideMethods.AdjacentSides()) {
				if (panelTile.Neighbours.ContainsKey(sTmp) && panelTile.Neighbours[sTmp].PanelAvatar.GetComponent<PanelAvatar>().Model != null && panelTile.Neighbours[sTmp].PanelAvatar.GetComponent<PanelAvatar>().Model.HasTaunt && !IsYourMinionHere(panelTile.Neighbours[sTmp].PanelAvatar.GetComponent<PanelAvatar>().Model)) {
					enemysTaunterFound = true;
				}
			}

			//if there is another minion, then this will be attack instead of move
			if (!enemysTaunterFound && panelTile.PanelAvatar.GetComponent<PanelAvatar>().Model != null) {
				//can not attack your own minions
				if (!IsYourMinionHere(panelTile.PanelAvatar.GetComponent<PanelAvatar>().Model) && mover.PanelAvatar.GetComponent<PanelAvatar>().Model.ActualDamage > 0) {
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
		bool isYourMinion = IsYourMinionHere(am);
		Debug.Log("This tile has " + (isYourMinion ? "yours" : "not yours") +" " + (am!=null?am.Card.Name:"no minion"));

		switch (Mode) {
			case global::Mode.Ready: {

					if (!isYourMinion && am != null) {
						PanelInformation.GetComponent<PanelInformation>().SetText("This is your enemys minion");
					}

					if (isYourMinion) {
						//has no moves
						if (am.MovesLeft <= 0) {
							PanelInformation.GetComponent<PanelInformation>().SetText("No moves left");
						} else {
							//preparing interaction panels for moves
							Debug.Log(am.MovesLeft + " moves left, ready to move");
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
						if (IsYourMinionHere(am)) {
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
					panelTile.PanelAvatar.GetComponent<PanelAvatar>().CastOn(pi.CastersCard, pi.Caster);
				}
				DisableAllPanelsInteraction();
				Mode = global::Mode.Ready;

				if (whatMode == PanelInteractionMode.Casting && panelTile.PanelAvatar.GetComponent<PanelAvatar>().Model != null) {
					Debug.Log("There is battlecry for " + whatCard.Name);
					if (whatCard.Effects.ContainsKey(Effect.Battlecry)) {
						whatCard = whatCard.Effects[Effect.Battlecry];

						//auto casting
						if (!whatCard.Params.ContainsKey(ParamType.Distance)) {
							panelTile.PanelAvatar.GetComponent<PanelAvatar>().Model.Cast(panelTile.PanelAvatar.GetComponent<PanelAvatar>().Model, whatCard);
						} else {
							//manual casting 
							int distance = whatCard.Params.ContainsKey(ParamType.Distance) ? whatCard.Params[ParamType.Distance] : 0;
							foreach (Side s in SideMethods.AllSides()) {
								if (SetInteractionToCastAround(panelTile.PanelAvatar.GetComponent<PanelAvatar>().Model, panelTile, whatCard, s, distance)) {
									Mode = global::Mode.CastingSpell;
								}
							}
							//battlecry can not be casted on itself
							panelTile.PanelInteraction.GetComponent<PanelInteraction>().Clear();
						}
					}
				}

				break;
			}
			default: throw new NotImplementedException("Implement working with mode: " + Mode);
		}

		//after action revalidate adjacent models
		PanelTiles.GetComponent<PanelTiles>().UpdateAdjacentModels();
	}


	private void DisableAllPanelsInteraction() {
		Debug.Log("clearing all tiles");
		foreach (GameObject tile in PanelTiles.GetComponent<ScrollableList>().ElementsToPut) {
			tile.GetComponent<PanelTile>().PanelInteraction.GetComponent<PanelInteraction>().Clear();
		}
	}

	public bool IsYourMinionHere(AvatarModel am) {
		bool isYourMinion = false;
		if (am != null) {
			//Debug.Log("Is your minion here? checking " + am.Card.Name);
		}
		for (int i = 0; true; i++) {
			if (am == null) {
				//not your minion
				break;
			}
			if (am != null) {
				isYourMinion = am == ActualTurnModel;
				if (isYourMinion) {
					break;
				}
			}
			if (i > 50) {
				throw new Exception("Fuck this shit");
			}
			am = am.Creator;
			if (am != null ) {
				//Debug.Log("creator is " + am.Card.Name);
			}
		}
		return isYourMinion;
	}

}

public enum Mode {
	Ready, MovingOrAttacking,
	CastingSpell
}
