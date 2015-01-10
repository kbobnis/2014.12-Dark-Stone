using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class PanelInteraction : MonoBehaviour {

	private PanelInteractionMode _Mode = PanelInteractionMode.Idle;

	public PanelInteractionMode Mode {
		set { _Mode = value; UpdateImage(); }
		get { return _Mode; }
	}

	//moving
	public PanelTile WhatMoveOrAttack;

	//casting spell
	public PanelTile Caster;
	public Card CastersCard;

	//receiving damage
	private int ReceivingDamage;

	void Awake() {
		UpdateImage();
	}

	public void PointerDownOn(BaseEventData bed) {
		try {
			if (Mode != PanelInteractionMode.AnimationAttack) {
				PanelMinigame.Me.GetComponent<PanelMinigame>().PointerDownOn(gameObject.transform.parent.gameObject.GetComponent<PanelTile>());
			}
		} catch (Exception e) {
			Debug.Log("Exception: " + e);
		}
	}


	private void UpdateImage() {
		GetComponent<Image>().enabled = true;
		Text text = GetComponentInChildren<Text>();
		text.enabled = true;
		text.text = "rzecz";

		switch (Mode) {
			case PanelInteractionMode.Casting: text.text = "Cast " + CastersCard.Name + " here"; break;
			case PanelInteractionMode.Moving: text.text = "Move " + WhatMoveOrAttack.PanelAvatar.GetComponent<PanelAvatar>().Model.Card.Name + " here"; break;
			case PanelInteractionMode.Attacking: text.text = "Attack with " + WhatMoveOrAttack.PanelAvatar.GetComponent<PanelAvatar>().Model.Card.Name; break;
			case PanelInteractionMode.ReceivingDamage: text.text = "Health -= " + ReceivingDamage; break;
			case PanelInteractionMode.AnimationAttack: text.text = "Attackin!"; break;
			case PanelInteractionMode.AnimationDefend: text.text = "Defending!"; break;
			default:
				GetComponent<Image>().enabled = false;
				text.enabled = false;
				break;
		}
	}

	internal void Clear() {
		Mode = PanelInteractionMode.Idle;
	}

	internal void CanCastHere(PanelTile caster, Card card) {
		Caster = caster;
		CastersCard = card;
		Mode = PanelInteractionMode.Casting;
	}

	internal void CanMoveHere(PanelTile panelTile) {
		WhatMoveOrAttack = panelTile;
		Mode = PanelInteractionMode.Moving;
	}

	internal void CanAttackHere(PanelTile panelTile) {
		WhatMoveOrAttack = panelTile;
		Mode = PanelInteractionMode.Attacking;
	}

	internal void TakeDamage(int p) {
		ReceivingDamage = p;
		Mode = PanelInteractionMode.ReceivingDamage;
	}

	internal void IsAttacking() {
		Mode = PanelInteractionMode.AnimationAttack;
	}

	internal void IsDefending() {
		Mode = PanelInteractionMode.AnimationDefend;
	}
}

public enum PanelInteractionMode {
	Idle,
	Casting,
	Moving,
	Attacking,
	ReceivingDamage,
	AnimationAttack,
	AnimationDefend

}
