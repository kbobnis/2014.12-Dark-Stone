using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class PanelAvatar : MonoBehaviour {

	public GameObject PanelHealth, PanelMana, PanelSpell, PanelAttack, PanelDirection;

	private PanelAvatarModel _Model = new PanelAvatarModel();

	private List<PanelAvatarModel> _AdditionalModels = new List<PanelAvatarModel>();

	public PanelAvatarModel Model {
		set { _Model = value; UpdateFromModel(); }
		get { return _Model; }
	}

	public bool IsEmpty() {
		return _Model.Card == null;
	}

	public void Prepare(Card card) {
		if ( _Model.Card != null) {
			throw new NotImplementedException("There is already a card here. What to do now");
		}
		_Model.Card = card;

		_Model.ActualHealth = card != null && card.Params.ContainsKey(ParamType.Health) ? card.Params[ParamType.Health] : 0;
		_Model.ActualMana = card != null && card.Params.ContainsKey(ParamType.HisMana) ? card.Params[ParamType.HisMana] : 0;
		_Model.ActualDamage = card != null && card.Params.ContainsKey(ParamType.Damage) ? card.Params[ParamType.Damage] : 0;
		if (card != null && !card.Animations.ContainsKey(AnimationType.OnBoard)) {
			throw new Exception("There is no onBoard animation for card: " + card.Name);
		}
		UpdateFromModel();
	}

	internal void SetDirection(Side side, int p) {
		_Model.Direction = side;
		_Model.Speed = p;
		UpdateFromModel();
	}

	public void UpdateFromModel() {
		GetComponent<Image>().enabled = _Model!=null&&(_Model.Card != null);
		if (_Model.Card != null) {
			GetComponent<Image>().sprite = _Model.Card.Animations[AnimationType.OnBoard];
		}
		if ((_Model.Card != null && _AdditionalModels.Count > 0) || (_AdditionalModels.Count > 1)) {
			GetComponent<Image>().enabled = true;
			GetComponent<Image>().sprite = SpriteManager.Fight;
			PanelHealth.GetComponent<PanelValue>().Prepare(0);
			PanelMana.GetComponent<PanelValue>().Prepare(0);
			PanelAttack.GetComponent<PanelValue>().Prepare(0);
			PanelDirection.GetComponent<PanelDirection>().Prepare(Side.None, 0);
		} else {
			PanelHealth.GetComponent<PanelValue>().Prepare(_Model != null ? _Model.ActualHealth : 0);
			PanelMana.GetComponent<PanelValue>().Prepare(_Model != null ? _Model.ActualMana : 0);
			PanelAttack.GetComponent<PanelValue>().Prepare(_Model != null ? _Model.ActualDamage : 0);
			PanelDirection.GetComponent<PanelDirection>().Prepare(_Model != null ? _Model.Direction : Side.None, _Model != null ? _Model.Speed : 0);
		}
	}

	public bool HasSpell() {
		return _Model.Card != null;
	}

	internal void CastOn(PanelTile panelTile) {
		Model.ActualMana -= panelTile.PanelInteraction.GetComponent<PanelInteraction>().Card.Cost;
		PanelSpell.GetComponent<PanelSpell>().CastOn(panelTile);
	}

	internal void AddModel(PanelAvatarModel model) {
		if (Model.Card == null) {
			Model = model;
		} else {
			_AdditionalModels.Add(model);
		}

	}

	internal PanelAvatarModel GetAndRemoveModel() {
		PanelAvatarModel pam = Model;
		Model = new PanelAvatarModel(); 
		return pam;
	}

	internal void BattleOut() {

		if (_AdditionalModels.Count == 0) {
			return;
		}
		if (Model.Card == null && _AdditionalModels.Count == 1) {
			Model = _AdditionalModels[0];
			_AdditionalModels.Clear();
			return;
		}
		_AdditionalModels.Add(Model);
		Model = new PanelAvatarModel();

		foreach (PanelAvatarModel pam in _AdditionalModels) {
			foreach (PanelAvatarModel pam2 in _AdditionalModels) {
				if (pam != pam2) {
					pam2.ActualHealth -= pam.ActualDamage;
					Debug.Log(pam2.Card.Name + "("+pam2.ActualHealth+" health), receives damage from : " + pam.Card.Name + " dmg: " + pam.ActualDamage);
				}
			}
		}

		foreach(PanelAvatarModel pam in _AdditionalModels.ToArray()){
			if (pam.ActualHealth <= 0) {
				_AdditionalModels.Remove(pam);
			}
		}
		int mostHealth = 0;
		int inAequo = 1;
		PanelAvatarModel pam3 = null;
		if (_AdditionalModels.Count > 0) {
			//leave the one with the most health
			foreach (PanelAvatarModel pam in _AdditionalModels) {
				if (pam.ActualHealth == mostHealth) {
					inAequo++;
				}
				if (pam.ActualHealth > mostHealth) {
					pam3 = pam;
					inAequo = 1;
				}
				
			}
		}
		if (pam3 != null) {
			_AdditionalModels.Clear();
			Model = pam3;
		}

		if (inAequo > 1) {
			Model = new PanelAvatarModel();
		}
	}

	internal void FlattenModelWhenNoBattles() {
		if (Model.Card == null && _AdditionalModels.Count == 1) {
			Model = _AdditionalModels[0];
			_AdditionalModels.Clear();
		}
	}

	internal void ShowBattleSignIfBattle() {
		UpdateFromModel();
	}
}

public class PanelAvatarModel {

	public Card Card;
	public int ActualHealth, ActualMana, ActualDamage, Speed, MovesLeft;
	public Side Direction = Side.None;

}



