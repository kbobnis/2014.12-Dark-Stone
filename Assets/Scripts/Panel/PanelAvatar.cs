using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class PanelAvatar : MonoBehaviour {

	public GameObject PanelHealth, PanelAttack, PanelTile;

	private AvatarModel _Model = null;

	public AvatarModel Model {
		get { return _Model; }
		set { _Model = value; } //because we modify contents of AvatarModel }
	}

	void Update() {
		UpdateFromModel();
	}

	public void UpdateFromModel() {

		if (Model != null && Model.IsOnBoard() &&  Model.ActualHealth <= 0) {
			//is dead
			_Model.GetMyHero().Minions.Remove(_Model);
			_Model = null;
		}

		GetComponent<Image>().enabled = _Model != null;
		if (_Model != null) {
			GetComponent<Image>().color = Color.white;
			GetComponent<Image>().sprite = _Model.Card.Animation;
			if (Model != null && (Model.MovesLeft == 0 || (!PanelMinigame.Me.ActualTurnModel.IsItYourMinion(Model) && PanelMinigame.Me.ActualTurnModel != Model) )) {
				GetComponent<Image>().color = Color.black;
			}
		}
		
		PanelHealth.GetComponent<PanelValue>().Prepare(_Model != null ? _Model.ActualHealth : 0);
		PanelAttack.GetComponent<PanelValue>().Prepare(_Model != null ? _Model.ActualAttack : 0);

	}

	internal AvatarModel GetAndRemoveModel() {
		AvatarModel pam = Model;
		Model = null;
		return pam;
	}

	internal void BattleOutWith(PanelAvatar panelAvatar) {
		AvatarModel pam = panelAvatar.Model;

		Model.ActualHealth -= pam.ActualAttack;
		pam.ActualHealth -= Model.ActualAttack;
		UpdateFromModel();
		panelAvatar.UpdateFromModel();
	}

	public void CastOn(PanelAvatar onWhat, Card c) {
		onWhat.Model = Model.Cast(onWhat.Model, c);
	}

}

