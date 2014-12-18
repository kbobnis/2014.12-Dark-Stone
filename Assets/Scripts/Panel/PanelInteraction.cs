using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PanelInteraction : MonoBehaviour {

	private PanelInteractionMode _Mode = PanelInteractionMode.Idle;

	public PanelInteractionMode Mode {
		set { _Mode = value; UpdateImage(); }
		get { return _Mode; }
	}

	//moving
	public PanelTile WhatMoveOrAttack;

	//casting spell
	public AvatarModel Caster;
	public Card CastersCard;


	void Awake() {
		UpdateImage();
	}


	private void UpdateImage() {
		GetComponent<Image>().enabled = false;
		Text text = GetComponentInChildren<Text>();
		text.enabled = false;

		if ( Mode != PanelInteractionMode.Idle) {
			GetComponent<Image>().enabled = true;
			text.enabled = true;
			switch (Mode) {
				case PanelInteractionMode.Casting: text.text = "Cast " + CastersCard.Name + " here"; break;
				case PanelInteractionMode.Moving: text.text = "Move " + WhatMoveOrAttack.PanelAvatar.GetComponent<PanelAvatar>().Model.Card.Name + " here"; break;
				case PanelInteractionMode.Attacking: text.text = "Attack with " + WhatMoveOrAttack.PanelAvatar.GetComponent<PanelAvatar>().Model.Card.Name; break;
			}
		}
	}

	internal void Clear() {
		Mode = PanelInteractionMode.Idle;
	}

	internal void CanCastHere(AvatarModel caster, Card card) {
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
}

public enum PanelInteractionMode {
	Idle,
	Casting,
	Moving,
	Attacking

}
