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
	private int _ActualMana, Speed, _MovesLeft, MaxHealth, _MaxMana;

	private readonly int _ActualDamage;

	public List<Card> Deck = new List<Card>();
	private List<Card> _Hand = new List<Card>();
	public List<CastedCard> Effects = new List<CastedCard>();
	private List<AvatarModel> Minions = new List<AvatarModel>();
	private AvatarModel _Creator;
	public Dictionary<Side, AvatarModel> AdjacentModels = new Dictionary<Side,AvatarModel>();
	private int Draught;
	private bool OnBoard;
	private bool _HasTaunt;

	public bool HasTaunt {
		get { return _HasTaunt; }
	}

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
		_Card = card;
		OnBoard = onBoard;

		foreach (KeyValuePair<ParamType, int> kvp in card.Params) {
			switch (kvp.Key) {
				case ParamType.Health: MaxHealth = kvp.Value; ActualHealth = kvp.Value; break;
				case ParamType.Attack: _ActualDamage = kvp.Value; break;
				case ParamType.Speed: Speed = kvp.Value; break;
				case ParamType.Taunt: _HasTaunt = true; break;
				//this is used when the spell is casted, no need for it now
				case ParamType.AttackAddForAdjacentFriendlyCharacters:
				case ParamType.ReplaceExisting:
				case ParamType.Distance:
					break;
				case ParamType.Heal:
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


	private void AddCrystal() {
		_MaxMana++;
		if (_MaxMana > MaxCrystals){
			_MaxMana = MaxCrystals;
		}
	}

	private void RefillCrystals() {
		_ActualMana = _MaxMana;
	}

	private void RefillMovements() {
		_MovesLeft = Speed;
		foreach (AvatarModel am in Minions) {
			am.RefillMovements();
		}
	}

	public void PullCardFromDeck() {
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
		get {
			int additionalDamage = 0;
			foreach (CastedCard c in Effects) {
				if (c.Params.ContainsKey(CastedCardParamType.AttackAdd)){
					additionalDamage += c.Params[CastedCardParamType.AttackAdd];
				}
			}
			return _ActualDamage + additionalDamage; 
		}
	}

	internal AvatarModel Cast(AvatarModel castingOn, Card c) {
		AvatarModel am = castingOn;
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
				throw new Exception("Can not cast minion on top of other.");
			}
			if (c.Params.ContainsKey(ParamType.ReplaceExisting)) {
				Minions.Remove(am);
			}
			am = new AvatarModel(c, true, this);
			Minions.Add(am);
		} else {

			CastedCard castedCard = new CastedCard(castingOn, c);
			//for one milisecond cards the effects where applied in constructor
			if (c.CardPersistency != CardPersistency.OneMilisecond) {
				am.Effects.Add(castedCard);
			}
		}
		return am;
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

		foreach(AvatarModel am2 in Minions){
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

		if (_Creator.Card.CardPersistency == CardPersistency.Hero) {
			return _Creator;
		}
		return _Creator.GetMyHero();
	}
}



