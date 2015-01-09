using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PanelHand : MonoBehaviour {

	public GameObject Prefab;
	public List<GameObject> Elements;

	// copy the prefab
	void Awake () {
		//clone prefab and insert inside elements
		int i = 1; 
		foreach(GameObject go in Elements){

			GameObject newItem = Instantiate(Prefab) as GameObject;

			newItem.name = "Hand item " + (i++);
			newItem.GetComponent<PanelAvatarCard>().WhereAmI = WhereAmI.Hand;
			newItem.transform.parent = go.transform;
			
			//move and size the new item
			RectTransform rectTransform = newItem.GetComponent<RectTransform>();
			
			rectTransform.offsetMin = new Vector2(0, 0);
			rectTransform.offsetMax = new Vector2(0, 0);
			rectTransform.rotation = go.GetComponent<RectTransform>().rotation;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	internal void UpdateWith(AvatarModel heroModel, System.Collections.Generic.List<Card> hand) {

		for(int i=0; i < Elements.Count; i++){
			GameObject go = Elements[i];
			Card c = hand.Count>i? hand[i]:null;
			go.SetActive(c != null);
			if (c != null) {
				go.GetComponentInChildren<PanelAvatarCard>().PreviewCardHand(heroModel, c);
			}
		}
	}
}
