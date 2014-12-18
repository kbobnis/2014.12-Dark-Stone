﻿using UnityEngine;
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

	internal void CastOn(Card c, AvatarModel am) {

		if (c == null) {
			throw new Exception("Why you cast when there is no card");
		}
		Model = am.Cast(Model, c);
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

	public static readonly int HandSize = 4;
	public static readonly int MaxCrystals = 10;
	
	private Card _Card;
	private int _ActualHealth;
	private int _ActualMana, _ActualDamage, Speed, _MovesLeft, MaxHealth, _MaxMana;

	public List<Card> Deck = new List<Card>();
	private List<Card> _Hand = new List<Card>();
	private List<AvatarModel> Minions = new List<AvatarModel>();
	private AvatarModel _Creator;
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

	public AvatarModel Creator {
		get { return _Creator; }
	}
	public AvatarModel(Card card, bool onBoard, AvatarModel creator) {
		_Creator = creator;
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
				case ParamType.Attack: _ActualDamage = kvp.Value; break;
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
		if (_MaxMana > MaxCrystals){
			_MaxMana = MaxCrystals;
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
			if (_Hand.Count < AvatarModel.HandSize) {
				_Hand.Add(c);
			} else {
				Debug.Log("Burning card " + c.Name + " from deck");
			}
		}
	}

	internal bool IsOnBoard() {
		return OnBoard;
	}

	public int ActualDamage {
		get { return _ActualDamage; }
	}

	internal AvatarModel Cast(AvatarModel actualModel, Card c) {
		AvatarModel am = actualModel;
		_ActualMana -= c.Cost;

		//if is a monster, then adding as minion
		if (c.Params.ContainsKey(ParamType.Health)) {
			if (actualModel != null) {
				//can not cast minion on other minion
				throw new Exception("Can not cast minion on top of other");
			}
			am = new AvatarModel(c, true, this);
			Minions.Add(am);
		} else {
			//if is a spell, then doing the magic
			if (c.Params.ContainsKey(ParamType.Damage)) {
				if (actualModel == null) {
					throw new Exception("Can not cast damage spell on nothing");
				}
				actualModel.ActualHealth -= c.Params[ParamType.Damage];
			}
		}
		return am;
	}
}



