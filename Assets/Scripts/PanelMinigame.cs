using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

public class PanelMinigame : MonoBehaviour {

	public GameObject PanelTiles, PanelTilesBg, PanelInformation, PanelCardPreview, PanelBottom, ButtonEndTurn;

	private AvatarModel MyModel, EnemysModel;
	private AvatarModel ActualTurnModel;
	private Mode Mode = Mode.Ready;

	internal void Prepare() {
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
		lasiaModel.Deck.Add(Card.Zombie);
		lasiaModel.Deck.Add(Card.HealingTouch);
		lasiaModel.Deck.Add(Card.Fireball);
		lasiaModel.Deck.Add(Card.IceBolt);
		lasiaModel.Deck.Add(Card.Mud);
		lasiaModel.Deck.Add(Card.IceBolt);
		lasiaModel.Deck.Add(Card.Mud);

		PanelAvatar dementorAvatar = PanelTiles.GetComponent<ScrollableList>().ElementsToPut[2].GetComponent<PanelTile>().PanelAvatar.GetComponent<PanelAvatar>();
		AvatarModel dementorModel = dementorAvatar.Model;
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

		ActualTurnModel = ActualTurnModel == MyModel?EnemysModel:MyModel;

		ActualTurnModel.AddCrystal();
		ActualTurnModel.RefillCrystals();
		ActualTurnModel.RefillMovements();
		ActualTurnModel.PullCardFromDeck();

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
					SetInteractionToCastAround(ActualTurnModel, panelTile, card, s, distance);
				}
			}
		}
	}

	private void SetInteractionToCastAround(AvatarModel caster, PanelTile panelTile, Card card, Side s, int distance) {

		if (distance > 0){
			if (panelTile.Neighbours.ContainsKey(s)) {
				SetInteractionToCastAround(caster, panelTile.Neighbours[s], card, s, distance - 1);
			}
		} 
		panelTile.PanelInteraction.GetComponent<PanelInteraction>().CanCastHere(caster, card);
		Mode = global::Mode.CastingSpell;
	}

	internal void PointerDownOn(PanelTile panelTile) {
		AvatarModel am = panelTile.PanelAvatar.GetComponent<PanelAvatar>().Model;
		bool isYourMinion = IsYourMinionHere(am);
		Debug.Log("This minion is " + (isYourMinion ? "yours" : "not yours"));

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
							foreach (Side s in SideMethods.AllSides()) {
								if (panelTile.Neighbours.ContainsKey(s)) {
									panelTile.Neighbours[s].PanelInteraction.GetComponent<PanelInteraction>().CanMoveHere(panelTile);
								}
							}
							Mode = Mode.MovingElement;
						}
					}
					break;
				}
			case global::Mode.MovingElement: {
					PanelTile whatWantsToMoveHere = panelTile.PanelInteraction.GetComponent<PanelInteraction>().WhatWantsToMoveHere;
					if (whatWantsToMoveHere != null) {
						whatWantsToMoveHere.PanelAvatar.GetComponent<PanelAvatar>().Model.MovesLeft--;
						//is here something?
						if (am != null) {
							if (IsYourMinionHere(am)) {
								//can not move on your mininion
								PanelInformation.GetComponent<PanelInformation>().SetText("Can not attack your own minion");
							} else {
								//battle

							}
						} else {
							//move
							panelTile.PanelAvatar.GetComponent<PanelAvatar>().Model = whatWantsToMoveHere.PanelAvatar.GetComponent<PanelAvatar>().Model;
							whatWantsToMoveHere.PanelAvatar.GetComponent<PanelAvatar>().Model = null;
						}

					}
					DisableAllPanelsInteraction();
					Mode = global::Mode.Ready;

					break;
				}
			case global::Mode.CastingSpell: {
				PanelInteraction pi = panelTile.PanelInteraction.GetComponent<PanelInteraction>();
				if (pi.Caster != null) {
					panelTile.PanelAvatar.GetComponent<PanelAvatar>().CastOn(pi.CastersCard, pi.Caster);
				}
				DisableAllPanelsInteraction();
				Mode = global::Mode.Ready;
				break;
			}
			default: throw new NotImplementedException("Implement working with mode: " + Mode);
		}
	}

	private void DisableAllPanelsInteraction() {
		foreach (GameObject tile in PanelTiles.GetComponent<ScrollableList>().ElementsToPut) {
			tile.GetComponent<PanelTile>().PanelInteraction.GetComponent<PanelInteraction>().Clear();
		}
	}

	private bool IsYourMinionHere(AvatarModel am) {

		bool isYourMinion = false;
		AvatarModel parent = am;
		for (int i = 0; true; i++) {
			if (parent == null) {
				//not your minion
				break;
			}
			if (parent != null) {
				isYourMinion = parent == ActualTurnModel;
			}
			if (i > 50) {
				throw new Exception("Fuck this shit");
			}
			parent = am.Creator;
		}
		return isYourMinion;
	}

}

public enum Mode {
	Ready, MovingElement,
	CastingSpell
}
