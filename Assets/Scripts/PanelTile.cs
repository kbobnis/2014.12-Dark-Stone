using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class PanelTile : MonoBehaviour {

	public GameObject PanelMinigame, PanelAvatar, PanelInteraction, PanelAvatarCardPrefab;
	public WhereAmI WhereAmI;

	public void Create(int row, int column, WhereAmI whereAmI) {
		WhereAmI = whereAmI;
		Row = row;
		Column = column;
		GameObject panelAvatarCard = Instantiate(PanelAvatarCardPrefab) as GameObject;
		PanelAvatar.GetComponent<PanelAvatar>().PanelAvatarCard = panelAvatarCard;
		panelAvatarCard.transform.parent = PanelAvatar.transform;
		RectTransform rt = panelAvatarCard.GetComponent<RectTransform>();
		rt.offsetMin = new Vector2();
		rt.offsetMax = new Vector2();
	}

	public Dictionary<Side, PanelTile> Neighbours = new Dictionary<Side, PanelTile>();
	public int Row;
	public int Column;

	public void Touched(BaseEventData bed) {
		try {
			switch (WhereAmI) {
				case global::WhereAmI.Board:
					PanelMinigame.GetComponent<PanelMinigame>().PointerDownOn(this);
					break;
				case global::WhereAmI.Hand:
					PanelMinigame.GetComponent<PanelMinigame>().CardInHandSelected(PanelAvatar.GetComponent<PanelAvatar>().PanelAvatarCard.GetComponent<PanelAvatarCard>().Card);
					break;
				default:
					throw new NotImplementedException("Not implemented");
			}
		} catch (Exception e) {
			Debug.Log("Exception: " + e);
		}
	}

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
			default:
				throw new NotImplementedException("Implement case: " + card.IsCastOn);
		}
		return canIHave;
	}


}
