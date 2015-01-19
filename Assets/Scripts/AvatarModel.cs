using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;


public enum AnimationRequest {
	RemoveArmor,
	ReceiveDamage,
	BurnCard,
	PullCardFromDeck,
	InsertWaitingCardIntoHand,
	BattleOutWith,
	CardInHandSelected,
	ReplaceMinionOn,
	PutMinionOnTile,
	DealInstantOn,
}

public class AvatarModel {

	public static readonly int HandSize = 5;
	public static readonly int MaxCrystals = 5;

	private Card _Card;
	public int _ActualMana;
	private int _MovesLeft, _ActualHealth, _MaxMana;

	public List<Card> Deck = new List<Card>();
	public List<Card> _Hand = new List<Card>();
	public List<CastedCard> Effects = new List<CastedCard>();
	public List<AvatarModel> Minions = new List<AvatarModel>();
	private AvatarModel _Creator;
	public Dictionary<Side, AvatarModel> AdjacentModels = new Dictionary<Side, AvatarModel>();
	private int Draught;
	public int Armor;
	public bool AlreadyUsedPower;

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

		get {
			int actual = _ActualMana;
			foreach (CastedCard c in Effects) {
				if (c.Params.ContainsKey(CastedCardParamType.ManaCrystalAdd)) {
					actual += c.Params[CastedCardParamType.ManaCrystalAdd];
				}
			}
			return actual;
		}
	}

	public int MaxMana {
		get {
			int actual = _MaxMana;
			foreach (CastedCard c in Effects) {
				if (c.Params.ContainsKey(CastedCardParamType.ManaCrystalAdd)) {
					actual += c.Params[CastedCardParamType.ManaCrystalAdd];
				}
				if (c.Params.ContainsKey(CastedCardParamType.ManaCrystalEmptyAdd)) {
					actual += c.Params[CastedCardParamType.ManaCrystalEmptyAdd];
				}
			}
			return actual;
		}
	}

	public AvatarModel Creator {
		get { return _Creator; }
		set { _Creator = value; }
	}
	public AvatarModel(Card card, bool onBoard, AvatarModel creator) {
		_Creator = creator;
		if (card == null) {
			throw new Exception("Card can not be null");
		}
		_Card = card;

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
			int delta = _ActualHealth - value;
			int deltaHealth = RemoveArmorBy(delta);

			if (deltaHealth > 0) {
				PanelMinigame.Me.AnimationRequests.Add(new AnimationRequestStruct(this, global::AnimationRequest.ReceiveDamage, deltaHealth));
				if (Card.Effects.ContainsKey(Effect.AfterTakingDamage)) {
					foreach (Card afterTakingDamage in Card.Effects[Effect.AfterTakingDamage]) {
						Cast(this, afterTakingDamage);
					}
				}
			}
			_ActualHealth -= deltaHealth;
			if (_ActualHealth > MaxHealth) {
				_ActualHealth = MaxHealth;
			}
		}
	}

	private int RemoveArmorBy(int delta) {
		int res = delta;

		if (delta > 0 && Armor > 0) {
			PanelMinigame.Me.AnimationRequests.Add(new AnimationRequestStruct(this, AnimationRequest.RemoveArmor, delta));
			Armor -= delta;
			delta = 0;
			if (Armor < 0) {
				res = -Armor;
				Armor = 0;
			}
		}

		return res;
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



	private void RefillCrystals() {
		_ActualMana = MaxMana;
	}

	public void RefillMovements() {
		int speed = Card.Params.ContainsKey(ParamType.Speed)?Card.Params[ParamType.Speed]:0;

		_MovesLeft = speed;
		foreach (AvatarModel am in Minions) {
			am.RefillMovements();
		}
	}

	public void PullCardFromDeck() {
		PanelMinigame.Me.AnimationRequests.Add(new AnimationRequestStruct(this, global::AnimationRequest.PullCardFromDeck));
		Card c = Deck.Count > 0 ? Deck[0] : null;
		if (c == null) {
			PanelMinigame.Me.AnimationRequests.Add(new AnimationRequestStruct(this, global::AnimationRequest.BurnCard));
			Draught++;
			ActualHealth -= Draught;

		} else {
			Deck.Remove(c);
			if (_Hand.Count < AvatarModel.HandSize) {
				PanelMinigame.Me.AnimationRequests.Add(new AnimationRequestStruct(this, global::AnimationRequest.InsertWaitingCardIntoHand));
				_Hand.Add(c);
			}
		}
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
		Hand.Remove(c);

		//minion
		if (c.CardPersistency == CardPersistency.Minion) {
			if (castingOn != null && !c.Params.ContainsKey(ParamType.ReplaceExisting)) {
				//can not cast minion on other minion
				throw new Exception("Casting card: " + c.Name + " on " + castingOn.Card.Name + " this can not happen");
			}
			AvatarModel owner = GetMyHero();
			if (c.Params.ContainsKey(ParamType.ReplaceExisting)) {
				owner = castingOn.GetMyHero();
				owner.Minions.Remove(castingOn);
				PanelMinigame.Me.AnimationRequests.Add(new AnimationRequestStruct(this, AnimationRequest.ReplaceMinionOn, castingOn));
			} else {
				PanelMinigame.Me.AnimationRequests.Add(new AnimationRequestStruct(this, AnimationRequest.PutMinionOnTile, castingOn));
			}
			castingOn = new AvatarModel(c, true, owner);
			//minions will be a flat structure. there is no need for deep one. 
			owner.Minions.Add(castingOn);
		}

		//is null for totemic might. where the actual spell does nothing, the battlecry throws totem
		if (castingOn != null) {
			//check if there is already an effect like this and marked to remove. we will unmark it
			bool foundTheSame = false;
			foreach (CastedCard cc in castingOn.Effects) {
				if (cc.Card == c && cc.MarkedToRemove) {
					cc.MarkedToRemove = false;
					foundTheSame = true;
					break;
				}
			}

			if (!foundTheSame) {
				CastedCard castedCard = new CastedCard(this, castingOn, c);

				if (c.CardPersistency != CardPersistency.Instant && castedCard.Params.Count > 0) {
					Debug.Log("Casting " + c.Name + " on " + castingOn.Card.Name);
					castingOn.Effects.Add(castedCard);
				}
				//because of healing this has to be after adding castedCard to effect. (max health update then healing)
				castedCard.DealInstants(this, castingOn);
			}
		}
		return castingOn;
	}

	internal void StartOfATurn() {
		AddCrystal();
		RefillCrystals();
		RefillMovements();
		PullCardFromDeck();
		AlreadyUsedPower = false;
	}

	internal void EndOfATurn() {

		foreach (CastedCard c in Effects.ToArray()) {
			if (c.Card.CardPersistency == CardPersistency.UntilEndTurn) {
				Effects.Remove(c);
			}
		}
		if (Card.Effects.ContainsKey(Effect.AtEndTurn)) {
			foreach (Card atEndTurn in Card.Effects[Effect.AtEndTurn]) {
				if (atEndTurn.CardTarget == CardTarget.Self){
					PanelTile pt = PanelMinigame.Me.PanelBoardFront.GetComponent<PanelTiles>().FindTileForModel(this);
					PanelTile heroTile = PanelMinigame.Me.PanelBoardFront.GetComponent<PanelTiles>().FindTileForModel(GetMyHero());
					PanelMinigame.Me.CastSpell(pt, atEndTurn, pt, heroTile, true, 0, true);
				} else {
					throw new NotImplementedException("Implement at end turn card target: " + atEndTurn.CardTarget);
				}
			}
		}

		foreach (AvatarModel am in Minions) {
			am.EndOfATurn();
		}
	}

	public bool IsItYourMinion(AvatarModel am) {
		if (Card.CardPersistency != CardPersistency.Hero) {
			throw new Exception("This should be called only for heroes");
		}

		foreach (AvatarModel am2 in Minions) {
			if (am2 == am) {
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

	internal int SpellDamageAdd() {
		int add = 0;
		foreach (AvatarModel am in Minions) {
			foreach (CastedCard cc in am.Effects) {
				if (cc.Params.ContainsKey(CastedCardParamType.SpellDamageAdd)) {
					add += cc.Params[CastedCardParamType.SpellDamageAdd];
				}
			}
		}
		return add;
	}

	public bool IsFriendlyCharacter(AvatarModel avatarModel) {
		return GetMyHero().IsItYourMinion(avatarModel) || GetMyHero() == avatarModel;
	}
}
