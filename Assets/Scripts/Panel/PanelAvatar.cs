using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class PanelAvatar : MonoBehaviour {

	public GameObject PanelHealth, PanelAttack;

	private AvatarModel _Model = null;

	public AvatarModel Model {
		get { return _Model; }
		set { _Model = value; UpdateFromModel(); } //because we modify contents of AvatarModel }
	}

	void Update() {
		UpdateFromModel();
	}

	public void UpdateFromModel() {

		if (Model != null && Model.IsOnBoard() &&  Model.ActualHealth <= 0) {
			_Model = null;
		}

		GetComponent<Image>().enabled = _Model != null;
		if (_Model != null) {
			GetComponent<Image>().sprite = _Model.Card.Animations[AnimationType.Icon];
		}
		
		PanelHealth.GetComponent<PanelValue>().Prepare(_Model != null ? _Model.ActualHealth : 0);
		PanelAttack.GetComponent<PanelValue>().Prepare(_Model != null ? _Model.ActualDamage : 0);

	}

	internal void CastOn(Card c, PanelTile panelTile) {

		if (c == null) {
			throw new Exception("Why you cast when there is no card");
		}
		panelTile.PanelAvatar.GetComponent<PanelAvatar>().Model = Model.Cast(c);

	}

	internal AvatarModel GetAndRemoveModel() {
		AvatarModel pam = Model;
		Model = null;
		return pam;
	}

	internal void BattleOutWith(PanelAvatar panelAvatar) {
		AvatarModel pam = panelAvatar.Model;

		Model.ActualHealth -= pam.ActualDamage;
		pam.ActualHealth -= Model.ActualDamage;
		UpdateFromModel();
		panelAvatar.UpdateFromModel();
	}

}

public class AvatarModel {

	private Card _Card;
	private int _ActualHealth;
	private int _ActualMana, _ActualDamage, Speed, _MovesLeft, MaxHealth, _MaxMana;

	public List<Card> Deck = new List<Card>();
	private List<Card> _Hand = new List<Card>();
	private List<AvatarModel> Minions = new List<AvatarModel>();
	private int Draught;
	private bool OnBoard;

	public List<Card> Hand {
		get { return _Hand; }
	}

	public int MovesLeft {
		get { return _MovesLeft; }
		set { _MovesLeft = value; }
	}

	public Card Card {
		get { return _Card;  }
	}

	public int MaxMana {
		get { return _MaxMana; }
	}

	public int ActualMana {
		get { return _ActualMana; }
	}
	public AvatarModel(Card card, bool onBoard) {

		if (card == null) {
			throw new Exception("Card can not be null");
		}
		if (!card.Animations.ContainsKey(AnimationType.Icon)) {
			throw new Exception("There is no icon animation for card: " + card.Name);
		}
		_Card = card;
		OnBoard = onBoard;

		foreach (KeyValuePair<ParamType, int> kvp in card.Params) {
			switch (kvp.Key) {
				case ParamType.Health: MaxHealth = kvp.Value; ActualHealth = kvp.Value; break;
				case ParamType.Damage: _ActualDamage = kvp.Value; break;
				case ParamType.Speed: Speed = kvp.Value; break;
				//this is used when the spell is casted, no need for it now
				case ParamType.Distance:
				case ParamType.OneTimeSpeed:
				case ParamType.Heal:
					break;
				default: throw new NotImplementedException("Implement case for: " + kvp.Key);
			}
		}
	}

	public int ActualHealth {
		get { return _ActualHealth; }
		set { _ActualHealth = value;
			if (_ActualHealth > MaxHealth) {
				_ActualHealth = MaxHealth;
			}
		}
	}


	internal void AddCrystal() {
		_MaxMana++;
		if (_MaxMana > 10){
			_MaxMana = 10;
		}
	}

	internal void RefillCrystals() {
		_ActualMana = _MaxMana;
	}

	internal void RefillMovements() {
		_MovesLeft = Speed;
		foreach (AvatarModel am in Minions) {
			am.RefillMovements();
		}
	}

	internal void PullCardFromDeck() {
		Card c = Deck.Count>0?Deck[0]:null;
		if (c == null) {
			Draught++;
			ActualHealth -= Draught;
		} else {
			Deck.Remove(c);
			_Hand.Add(c);
			Debug.Log("hand now has " + _Hand.Count + " cards");
		}
	}

	internal bool IsOnBoard() {
		return OnBoard;
	}

	public int ActualDamage {
		get { return _ActualDamage; }
	}

	internal AvatarModel Cast(Card c) {
		AvatarModel am = new AvatarModel(c, true);
		_ActualMana -= c.Cost;
		//if is a monster, then adding as minion
		if (c.Params.ContainsKey(ParamType.Health)) {
			Minions.Add(am);
		}
		return am;
	}
}



