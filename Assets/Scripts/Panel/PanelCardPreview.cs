using UnityEngine;
using System.Collections;

public class PanelCardPreview : MonoBehaviour {

	public GameObject PanelAvatar;

	internal void Preview(Card Card) {
		PanelAvatar.GetComponent<PanelAvatarCard>().Prepare(Card);
	}
}
