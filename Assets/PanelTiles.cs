using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;

public class PanelTiles : MonoBehaviour {

	public GameObject PanelInformation;

	public Mode Mode = Mode.Ready;

	internal void PointerDownOn(PanelTile panelTile) {
		Debug.Log("Touched on: " + panelTile.gameObject.name);

		//check if there is card on board with spell ready to cast

		switch (Mode) {
			case Mode.Ready: {
				DownWhenReady(panelTile);
				break;
			}
			case Mode.SpellCasting: {
				if (panelTile.PanelInteraction.GetComponent<PanelInteraction>().IsInMode(Mode.SpellCasting)) {
					CastSpell(panelTile);
				} else {
					DisableCastingOnAll();
				}
				break;
			}
		}

	}

	private void DisableCastingOnAll() {
		foreach (GameObject tile in gameObject.GetComponent<ScrollableList>().ElementsToPut) {
			tile.GetComponent<PanelTile>().PanelInteraction.GetComponent<PanelInteraction>().Prepare(Mode.Ready, null, null);
		}
		Mode = global::Mode.Ready;
	}

	private void CastSpell(PanelTile panelTile) {
		PanelAvatar pa = panelTile.PanelInteraction.GetComponent<PanelInteraction>().WhoIsCasting.PanelAvatar.GetComponent<PanelAvatar>();
		pa.CastOn(panelTile);
		DisableCastingOnAll();
	}

	private void DownWhenReady(PanelTile panelTile) {
		PanelSpell sp = panelTile.PanelAvatar.GetComponent<PanelAvatar>().PanelSpell.GetComponent<PanelSpell>();
		Card c = sp.TopCard();
		if (c != null) {
			Debug.Log("card: " + c);

			//can cast?
			if (c.Cost > panelTile.PanelAvatar.GetComponent<PanelAvatar>().PanelMana.GetComponent<PanelValue>().Value) {
				PanelInformation.GetComponent<PanelInformation>().SetText("You have not enough mana to cast this spell");
			} else {

				Mode = Mode.SpellCasting;

				if (!c.Params.ContainsKey(ParamType.Speed)) {
					throw new Exception("If there is no speed, then what? Programmer, think what now");
				}
				bool anyPlaceToCast = panelTile.SetInteractionForMode(Mode.SpellCasting, c, c.Params[ParamType.Speed], Side.Up, panelTile);

				if (panelTile.SetInteractionForMode(Mode.SpellCasting, c, c.Params[ParamType.Speed], Side.Down, panelTile)) {
					anyPlaceToCast = true;
				}
				if (panelTile.SetInteractionForMode(Mode.SpellCasting, c, c.Params[ParamType.Speed], Side.Left, panelTile)) {
					anyPlaceToCast = true;
				}
				if (panelTile.SetInteractionForMode(Mode.SpellCasting, c, c.Params[ParamType.Speed], Side.Right, panelTile)) {
					anyPlaceToCast = true;
				}

				if (!anyPlaceToCast) {
					Debug.Log("There is no place where it can be cast.");
					Mode = Mode.Ready;
				}
			}
		}
	}
}

public enum Mode {
	Ready, SpellCasting
}
