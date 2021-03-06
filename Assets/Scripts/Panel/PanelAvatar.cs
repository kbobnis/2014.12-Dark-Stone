﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class PanelAvatar : MonoBehaviour {

	public GameObject PanelTile, PanelAvatarCard;

	private AvatarModel _Model = null;

	public AvatarModel Model {
		get { return _Model; }
		set { _Model = value; } //because we modify contents of AvatarModel }
	}

	void Update() {
		//UpdateFromModel();
	}

	public void UpdateFromModel() {

		//to be destroyed?
		bool toBeDestroyed = false;
		if (Model != null) {
			foreach (CastedCard cc in Model.Effects) {
				if (cc.Params.ContainsKey(CastedCardParamType.Destroy)) {
					toBeDestroyed = true;
				}
			}
		}

		if (Model != null && (Model.ActualHealth <= 0 || toBeDestroyed) ) {
			//is dead
			_Model.GetMyHero().Minions.Remove(_Model);
			_Model = null;
		}

		AvatarModel heroModel = Model != null ? Model.GetMyHero() : null;
		PanelAvatarCard.GetComponent<PanelAvatarCard>().PreviewModel(heroModel, Model, PanelMinigame.Me.ActualTurnModel.IsItYourMinion(Model) || PanelMinigame.Me.ActualTurnModel == heroModel);
	}

	internal AvatarModel GetAndRemoveModel() {
		AvatarModel pam = Model;
		Model = null;
		return pam;
	}

	internal void BattleOutWith(PanelAvatar panelAvatar) {
		PanelMinigame.Me.AnimationRequests.Add(new AnimationRequestStruct(this.Model, AnimationRequest.BattleOutWith, panelAvatar));
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

