using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class AvatarModel {

	public static readonly int HandSize = 4;
	public static readonly int MaxCrystals = 10;

	private Card _Card;
	private int _ActualMana, _MovesLeft, _ActualHealth, _MaxMana;

	public List<Card> Deck = new List<Card>();
	private List<Card> _Hand = new List<Card>();
	public List<CastedCard> Effects = new List<CastedCard>();
	public List<AvatarModel> Minions = new List<AvatarModel>();
	private AvatarModel _Creator;
	public Dictionary<Side, AvatarModel> AdjacentModels = new Dictionary<Side, AvatarModel>();
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
		get { return _Card; }
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
		_Card = card;
		OnBoard = onBoard;

		ActualHealth = card.Params[ParamType.Health];
	}

	public int ActualHealth {
		get {
			if (_ActualHealth > MaxHealth) {
				_ActualHealth = MaxHealth;
			}
			return _ActualHealth;
		}
		set {
			_ActualHealth = value;
			if (_ActualHealth > MaxHealth) {
				_ActualHealth = MaxHealth;
			}
		}
	}

	public int MaxHealth {
		get {
			int max = Card.Params[ParamType.Health];
			foreach (CastedCard c in Effects) {
				if (c.Params.ContainsKey(CastedCardParamType.HealthAdd)) {
					max += c.Params[CastedCardParamType.HealthAdd];
				}
			}
			return max;
		}
	}

	private void AddCrystal() {
		_MaxMana++;
		if (_MaxMana > MaxCrystals) {
			_MaxMana = MaxCrystals;
		}
	}

	public int MaxMana {
		get { return _MaxMana; }
	}

	private void RefillCrystals() {
		_ActualMana = _MaxMana;
	}

	private void RefillMovements() {
		int speed = Card.Params[ParamType.Speed];

		_MovesLeft = speed;
		foreach (AvatarModel am in Minions) {
			am.RefillMovements();
		}
	}

	public void PullCardFromDeck() {
		Card c = Deck.Count > 0 ? Deck[0] : null;
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

	public int ActualAttack {
		get {
			int additionalDamage = 0;
			foreach (CastedCard c in Effects) {
				if (c.Params.ContainsKey(CastedCardParamType.AttackAdd)) {
					additionalDamage += c.Params[CastedCardParamType.AttackAdd];
				}
			}
			int baseAttack = Card.Params.ContainsKey(ParamType.Attack) ? Card.Params[ParamType.Attack] : 0;
			return baseAttack + additionalDamage;
		}
	}

	internal AvatarModel Cast(AvatarModel castingOn, Card c) {
		_ActualMana -= c.Cost;
		if (_ActualMana < 0) {
			//it can go under zero. if a minion is summoning it with battlecry
			//throw new Exception("What is this situation. Mana can not go under zero. Casting: " + c.Name + " for " + c.Cost);
		}
		Hand.Remove(c);

		//if is a monster, then adding as minion
		if (c.Params.ContainsKey(ParamType.Health)) {
			if (castingOn != null && !c.Params.ContainsKey(ParamType.ReplaceExisting)) {
				//can not cast minion on other minion
				throw new Exception("Casting card: " + c.Name + ", on " + castingOn.Card.Name + " this can not happen");
			}
			if (c.Params.ContainsKey(ParamType.ReplaceExisting)) {
				Minions.Remove(castingOn);
			}
			castingOn = new AvatarModel(c, true, this);
			Minions.Add(castingOn);
		} else {
			//check if there is already an effect like this and marked to remove. we will unmark it
			bool foundTheSame = false;
			foreach (CastedCard cc in Effects) {
				if (cc.Card == c && cc.MarkedToRemove){
					cc.MarkedToRemove = false;
					foundTheSame = true;
				}
			}

			if (!foundTheSame) {
				CastedCard castedCard = new CastedCard(castingOn, c);

				if (c.CardPersistency != CardPersistency.Instant) {
					castingOn.Effects.Add(castedCard);
				}
				//because of healing this has to be after adding castedCard to effect. (max health update then healing)
				castedCard.DealInstants(castingOn);
			}
		}
		return castingOn;
	}

	internal void StartOfATurn() {
		AddCrystal();
		RefillCrystals();
		RefillMovements();
		PullCardFromDeck();
	}

	internal void EndOfATurn() {

		foreach (CastedCard c in Effects.ToArray()) {
			if (c.Card.CardPersistency == CardPersistency.UntilEndTurn) {
				Effects.Remove(c);
			}
		}
		foreach (AvatarModel am in Minions) {
			am.EndOfATurn();
			foreach (AvatarModel am2 in am.Minions) {
				am2.EndOfATurn();
			}
		}
	}

	public bool IsItYourMinion(AvatarModel am) {

		foreach (AvatarModel am2 in Minions) {
			if (am2 == am) {
				return true;
			}
			if (am2.IsItYourMinion(am)) {
				return true;
			}
		}
		return false;
	}

	internal AvatarModel GetMyHero() {
		if (_Creator == null) {
			return this;
		}
		if (_Creator.Card.CardPersistency == CardPersistency.Hero) {
			return _Creator;
		}
		return _Creator.GetMyHero();
	}

	public static int MyMinionsNumber(AvatarModel am) {
		int count = am.Minions.Count;
		foreach (AvatarModel am2 in am.Minions) {
			count += MyMinionsNumber(am2);
		}
		return count;
	}



	
}
