using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class PanelTile : MonoBehaviour {

	public GameObject PanelAvatar, PanelInteraction, PanelAvatarCardPrefab;

	public void Create(int row, int column) {
		Row = row;
		Column = column;
		GameObject panelAvatarCard = Instantiate(PanelAvatarCardPrefab) as GameObject;
		panelAvatarCard.GetComponent<PanelAvatarCard>().WhereAmI = global::WhereAmI.Board;
		PanelAvatar.GetComponent<PanelAvatar>().PanelAvatarCard = panelAvatarCard;
		panelAvatarCard.transform.parent = PanelAvatar.transform;
		RectTransform rt = panelAvatarCard.GetComponent<RectTransform>();
		rt.offsetMin = new Vector2();
		rt.offsetMax = new Vector2();
	}

	public Dictionary<Side, PanelTile> Neighbours = new Dictionary<Side, PanelTile>();
	public int Row;
	public int Column;

	internal int GetDistanceTo(PanelTile to) {
		int d = Mathf.Abs(to.Row - Row) + Mathf.Abs(to.Column - Column);
		if (to.Row != Row && to.Column != Column) {
			d--;
		}
		return d;
	}

	internal bool CanHeHaveThisSpell(PanelTile target, PanelTile onWhat, Card card) {

		AvatarModel casterModel = PanelAvatar.GetComponent<PanelAvatar>().Model;
		AvatarModel targetModel = target.PanelAvatar.GetComponent<PanelAvatar>().Model;
		AvatarModel onWhatModel = onWhat.PanelAvatar.GetComponent<PanelAvatar>().Model;

		bool canIHave = false;
		switch (card.IsCastOn) {
			case IsCastOn.Target:
				canIHave = onWhat == target;
				break;
			case IsCastOn.AllFriendlyMinions:
				canIHave = onWhatModel != null && casterModel.GetMyHero().IsItYourMinion(onWhatModel);
				break;
			case IsCastOn.AdjacentFriendlyMinions:
				if (onWhatModel != null && casterModel.GetMyHero().IsItYourMinion(onWhatModel)) {
					foreach (Side s in SideMethods.AdjacentSides()) {
						//we have to compare panelTiles instead of AvatarModels, because we could compare null to null
						if (Neighbours.ContainsKey(s) && Neighbours[s] == onWhat) {
							canIHave = true;
						}
					}
				}
				break;
			case IsCastOn.AdjacentFriendlyCharacters:
				if (onWhatModel != null && (casterModel.GetMyHero().IsItYourMinion(onWhatModel) || targetModel.GetMyHero() == casterModel)) {
					foreach (Side s in SideMethods.AdjacentSides()) {
						//we have to compare panelTiles instead of AvatarModels, because we could compare null to null
						if (Neighbours.ContainsKey(s) && Neighbours[s] == onWhat) {
							canIHave = true;
						}
					}
				}
				break;
			case IsCastOn.OtherFriendlyMinions:
				canIHave = onWhatModel != null && casterModel.GetMyHero().IsItYourMinion(onWhatModel) && targetModel != onWhatModel;
				break;
			case IsCastOn.AllEnemyCharacters:
				canIHave = onWhatModel != null && onWhatModel.GetMyHero() != casterModel.GetMyHero();
				break;
			case IsCastOn.OtherItsCharacters:
				canIHave = onWhatModel!= null && onWhatModel != targetModel && targetModel.GetMyHero() == onWhatModel.GetMyHero();
				break;
			case IsCastOn.FriendlyHero:
				canIHave = casterModel.GetMyHero() == onWhatModel;
				break;
			case IsCastOn.AllEnemyMinions:
				canIHave = onWhatModel != null && onWhatModel.GetMyHero() != casterModel.GetMyHero() && onWhatModel.GetMyHero().IsItYourMinion(onWhatModel);
				break;
			case IsCastOn.AllFriendlyCharacters:
				canIHave = onWhatModel != null && casterModel.IsFriendlyCharacter(onWhatModel);
				break;
			case IsCastOn.EnemyHero:
				canIHave = onWhatModel != null && onWhatModel.Card.CardPersistency == CardPersistency.Hero && !onWhatModel.IsFriendlyCharacter(casterModel);
				break;
			default:
				throw new NotImplementedException("Implement case: " + card.IsCastOn);
		}
		return canIHave;
	}

	public bool SetInteractionToMoveAround() {
		bool canMove = false;

		//check for adjacent taunt
		//check for adjacent sticky enemy characters
		bool foundTaunt = false;
		bool foundStickyEnemy = false;
		List<Side> whereTaunts = new List<Side>();
		foreach (Side s in SideMethods.AllSides()) {
			if (Neighbours.ContainsKey(s)) {
				AvatarModel m = Neighbours[s].PanelAvatar.GetComponent<PanelAvatar>().Model;
				if (m != null && !m.IsFriendlyCharacter(PanelAvatar.GetComponent<PanelAvatar>().Model)) {
					foreach (CastedCard cc in m.Effects) {
						if (cc.Params.ContainsKey(CastedCardParamType.Taunt)) {
							foundTaunt = true;
							whereTaunts.Add(s);
						}
						if (cc.Params.ContainsKey(CastedCardParamType.Sticky)) {
							foundStickyEnemy = true;
						}
					}
				}
			}
		}

		AvatarModel am = PanelAvatar.GetComponent<PanelAvatar>().Model;
		foreach (Side s in SideMethods.AllSides()) {
			if (Neighbours.ContainsKey(s)) {
				PanelTile whereMove = Neighbours[s];
				
				if (whereMove.PanelAvatar.GetComponent<PanelAvatar>().Model == null) {

					if (!foundStickyEnemy) {
						whereMove.PanelInteraction.GetComponent<PanelInteraction>().CanMoveHere(this);
						canMove = true;
					}
				} else if (!PanelAvatar.GetComponent<PanelAvatar>().Model.IsFriendlyCharacter(whereMove.PanelAvatar.GetComponent<PanelAvatar>().Model)
						&& am.ActualAttack > 0 
						&& (!foundTaunt || whereTaunts.Contains(s))) {
					whereMove.PanelInteraction.GetComponent<PanelInteraction>().CanAttackHere(this);
					canMove = true;
				}
			}
		}

		return canMove;
	}



}
