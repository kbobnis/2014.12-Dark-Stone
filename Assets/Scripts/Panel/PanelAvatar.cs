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

		if (card != null) {
			foreach (KeyValuePair<ParamType, int> kvp in card.Params) {
				switch (kvp.Key) {
					case ParamType.Health: _Model.ActualHealth += kvp.Value; break;
					case ParamType.HisMana: _Model.ActualMana += kvp.Value; break;
					case ParamType.Damage: _Model.ActualDamage += kvp.Value; break;
					case ParamType.Speed: _Model.Speed += kvp.Value; break;
					case ParamType.IncreaseManaEachTurn: _Model.IncreaseManaEachTurn += kvp.Value; break;
					//this is used when the spell is casted, no need for it now
					case ParamType.Distance: break;
					default: throw new NotImplementedException("Implement case for: " + kvp.Key);
				}
			}
		}
		if (card != null) {
			Debug.Log("Preparing panelAvatar with card: " + card.Name + " and the speed in model is: " + _Model.Speed);
		}

		if (_Model.Card == null){
			_Model.Card = card;
			if (card != null && !card.Animations.ContainsKey(AnimationType.OnBoard)) {
				throw new Exception("There is no onBoard animation for card: " + card.Name);
			}
		}

		if (card != null && card.Spells.Count > 0) {
			foreach(Card c in card.Spells){
				_Model.CardStack.Add(c);
			}
		}

		UpdateFromModel();
	}

	void Update() {
		if (Model.CardStack.Count == 0 && Model.CardFeeders.Count > 0) {
			foreach (PanelAvatar ps in Model.CardFeeders) {
				Card c = TryToPullCard();
				if (c != null) {
					Model.CardStack.Add(c);
					break;
				}
			}
			UpdateFromModel();
		}
	}

	internal void SetDirection(Side side) {
		_Model.Direction = side;
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
			PanelDirection.GetComponent<PanelDirection>().Prepare(Side.None);
		} else {
			PanelHealth.GetComponent<PanelValue>().Prepare(_Model != null ? _Model.ActualHealth : 0);
			PanelMana.GetComponent<PanelValue>().Prepare(_Model != null ? _Model.ActualMana : 0);
			PanelAttack.GetComponent<PanelValue>().Prepare(_Model != null ? _Model.ActualDamage : 0);
			PanelDirection.GetComponent<PanelDirection>().Prepare(_Model != null ? _Model.Direction : Side.None);
		}

		PanelSpell.GetComponent<Image>().enabled = false;
		PanelSpell.GetComponent<PanelSpell>().ImageSpell.SetActive(false);
		if (Model.CardStack.Count > 0) {
			PanelSpell.GetComponent<Image>().enabled = true;
			PanelSpell.GetComponent<PanelSpell>().ImageSpell.SetActive(true);
			if (!Model.CardStack[0].Animations.ContainsKey(AnimationType.Icon)) {
				throw new Exception("Card: " + Model.CardStack[0].Name + ", has no icon animation.");
			}
			PanelSpell.GetComponent<PanelSpell>().ImageSpell.GetComponent<Image>().sprite = Model.CardStack[0].Animations[AnimationType.Icon];
		}
	}

	public bool HasSpell() {
		return _Model.Card != null;
	}

	internal void CastOn(PanelTile panelTile) {

		Card c = TryToPullCard();
		if (c == null) {
			throw new Exception("Why you cast when there is no card");
		}
		Model.ActualMana -= c.Cost;
		Model.CardFeeders.Add(panelTile.GetComponent<PanelTile>().PanelAvatar.GetComponent<PanelAvatar>());
		panelTile.PanelAvatar.GetComponent<PanelAvatar>().Prepare(c);
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
					mostHealth = pam.ActualHealth;
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
		//in case all died and there will be fight sprite left.
		UpdateFromModel();
	}

	internal void FlattenModelWhenNoBattles() {
		if (Model.Card == null && _AdditionalModels.Count == 1) {
			Model = _AdditionalModels[0];
			_AdditionalModels.Clear();
			UpdateFromModel();
		}
	}

	public Card TryToPullCard() {
		Card c = null;
		if (Model.CardStack.Count > 0) {
			c = Model.CardStack[0];
			Model.CardStack.Remove(c);
			UpdateFromModel();
		}
		return c;
	}

}

public class PanelAvatarModel {

	public Card Card;
	public int ActualHealth, ActualMana, ActualDamage, Speed, MovesLeft, IncreaseManaEachTurn;
	public Side Direction = Side.None;

	public List<Card> CardStack = new List<Card>();
	public List<PanelAvatar> CardFeeders = new List<PanelAvatar>();

	public Card TopCard() {
		return CardStack.Count > 0 ? CardStack[0] : null;
	}

	
}



