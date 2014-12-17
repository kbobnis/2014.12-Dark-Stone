using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

public class PanelAvatar : MonoBehaviour {

	public GameObject PanelHealth, PanelAttack, PanelMana;

	private AvatarModel _Model = new AvatarModel();

	private bool OnBoard = false;

	public AvatarModel Model {
		set { _Model = value; UpdateFromModel(); }
		get { return _Model; }
	}

	public bool IsEmpty() {
		return _Model.Card == null;
	}

	public void Prepare(Card card) {

		if (card == null) {
			Model = new AvatarModel();
		}

		if (card != null) {
			foreach (KeyValuePair<ParamType, int> kvp in card.Params) {
				switch (kvp.Key) {
					case ParamType.Health: _Model.MaxHealth += kvp.Value;  _Model.ActualHealth += kvp.Value; break;
					case ParamType.Heal: _Model.ActualHealth += kvp.Value; break;
					case ParamType.Damage: _Model.ActualDamage += kvp.Value; break;
					case ParamType.Speed:
						if (_Model.Speed < kvp.Value) {
							_Model.Speed = kvp.Value;
							_Model.MovesLeft = kvp.Value;
						} 
						break;
					case ParamType.OneTimeSpeed: _Model.MovesLeft += kvp.Value; break;
					//this is used when the spell is casted, no need for it now
					case ParamType.Distance: break;
					default: throw new NotImplementedException("Implement case for: " + kvp.Key);
				}
			}
		}

		if (_Model.Card == null){
			_Model.Card = card;
			if (card != null && !card.Animations.ContainsKey(AnimationType.Icon)) {
				throw new Exception("There is no icon animation for card: " + card.Name);
			}
		}

		UpdateFromModel();
	}

	void Update() {
		UpdateFromModel();
	}

	internal void SetDirection(Side side) {
		_Model.Direction = side;
		UpdateFromModel();
	}

	public void UpdateFromModel() {
		if (OnBoard) {
			if (Model.ActualHealth <= 0) {
				_Model = new AvatarModel();
			}
		} 

		PanelMana.SetActive(!OnBoard);
		if (PanelMana != null && Model.Card != null && !OnBoard) {
			PanelMana.GetComponent<PanelValue>().Prepare(Model.Card.Cost);
		}

		GetComponent<Image>().enabled = _Model!=null&&(_Model.Card != null);
		if (_Model.Card != null) {
			GetComponent<Image>().sprite = _Model.Card.Animations[AnimationType.Icon];
		}
		PanelHealth.GetComponent<PanelValue>().Prepare(_Model != null ? _Model.ActualHealth : 0);
		PanelAttack.GetComponent<PanelValue>().Prepare(_Model != null ? _Model.ActualDamage : 0);

	}

	public bool HasSpell() {
		return _Model.Card != null;
	}

	internal void CastOn(Card c, PanelTile panelTile) {

		if (c == null) {
			throw new Exception("Why you cast when there is no card");
		}
		Model.ActualMana -= c.Cost;
		panelTile.PanelAvatar.GetComponent<PanelAvatar>().Prepare(c);
		//if is a monster, then adding as minion
		if (c.Params.ContainsKey(ParamType.Health)) {
			Model.Minions.Add(panelTile.GetComponent<PanelTile>().PanelAvatar.GetComponent<PanelAvatar>().Model);
		}
	}

	internal AvatarModel GetAndRemoveModel() {
		AvatarModel pam = Model;
		Model = new AvatarModel(); 
		return pam;
	}

	internal void BattleOutWith(PanelAvatar panelAvatar) {
		AvatarModel pam = panelAvatar.Model;

		Model.ActualHealth -= pam.ActualDamage;
		pam.ActualHealth -= Model.ActualDamage;
		UpdateFromModel();
		panelAvatar.UpdateFromModel();
	}

	internal void IsOnBoard(bool p) {
		OnBoard = p;
	}
}

public class AvatarModel {

	public Card Card;
	private int _ActualHealth;
	public int ActualMana, ActualDamage, Speed, MovesLeft, MaxHealth, MaxMana;
	public Side Direction = Side.None;

	public List<Card> Deck = new List<Card>();
	public List<Card> Hand = new List<Card>();
	public List<AvatarModel> Minions = new List<AvatarModel>();
	public int Draught;

	public int ActualHealth {
		get { return _ActualHealth; }
		set { _ActualHealth = value;
			if (_ActualHealth > MaxHealth) {
				_ActualHealth = MaxHealth;
			}
		}
	}

	internal void AddCrystal() {
		MaxMana++;
		if (MaxMana > 10){
			MaxMana = 10;
		}
	}

	internal void RefillCrystals() {
		ActualMana = MaxMana;
	}

	internal void RefillMovements() {
		MovesLeft = Speed;
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
			Hand.Add(c);
		}
	}
}



