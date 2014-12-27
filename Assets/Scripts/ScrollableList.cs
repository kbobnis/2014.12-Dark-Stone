using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System.Collections.Generic;

public enum WhereAmI{
	Hand, Board,
}

public class ScrollableList : MonoBehaviour
{
    public GameObject itemPrefab;
	public bool FitToSize;
	public WhereAmI WhereAmI;

	private int columnCount = 1;
	public List<GameObject> ElementsToPut = new List<GameObject>();

	private void Prepare(){

		RectTransform panelRect = gameObject.GetComponent<RectTransform>();

		int itemCount = ElementsToPut.Count;

		int rowCount = itemCount / columnCount;
		if (rowCount > 0 && itemCount % columnCount > 0) {
				rowCount++;
		}

		float prefabW = itemPrefab.GetComponent<RectTransform>().rect.width;
		float prefabH = itemPrefab.GetComponent<RectTransform>().rect.height;

		float height = rowCount * prefabH;
		float width = columnCount * prefabW;

		if (FitToSize) {
			width = panelRect.rect.width;

			float scale = prefabH / prefabW;
			
			prefabW = width / columnCount;
			prefabH = prefabW * scale;
			//will it fit?
			if (prefabH * rowCount > panelRect.rect.size.y) {
				prefabH = panelRect.rect.size.y / rowCount;
				prefabW = prefabH / scale;
				width = prefabW * columnCount;
			}

			height = prefabH * rowCount;

		} else {

			//adjust the height of the container so that it will just barely fit all its children
			panelRect.offsetMin = new Vector2(-width/2, -height/2);
			panelRect.offsetMax = new Vector2(width/2, height/2);
		}

		int j = -1;
		int i = 0;
		for (int x2 = 0; x2 < itemCount; x2++)
		{
			if (x2 % columnCount == 0){
				j++;
				i = 0;
			}

			GameObject newItem = null;
			//create a new item, name it, and set the parent
			if (ElementsToPut.Count > x2 ){
				newItem = ElementsToPut[x2];
			} else {
				newItem = Instantiate(itemPrefab) as GameObject;
			}

			newItem.name = gameObject.name + " item at (" + i + "," + j + ")";
			if (newItem.GetComponent<PanelTile>() != null) {
				newItem.GetComponent<PanelTile>().Create(i, j, WhereAmI);
			}
			newItem.transform.parent = gameObject.transform;
			
			//move and size the new item
			RectTransform rectTransform = newItem.GetComponent<RectTransform>();
			
			float x = prefabW * (i) - width/2;
			float y = - prefabH * (j) + height/2 - prefabH;

			rectTransform.offsetMin = new Vector2(x, y);
			rectTransform.offsetMax = new Vector2(x + prefabW, y + prefabH);

			//this is used instead of a double for loop because itemCount may not fit perfectly into the rows/columns
			i++;
		}
	}

    void Start(){
		itemPrefab.SetActive(false);
    }

	public void Build(List<List<TileTemplate>> Buildings) {

		foreach (GameObject go in ElementsToPut) {
			Destroy(go);
		}
		ElementsToPut.Clear();
		itemPrefab.SetActive(true);

		List<List<PanelTile>> tiles = new List<List<PanelTile>>();
		columnCount = Buildings[0].Count;

		foreach (List<TileTemplate> row in Buildings) {
			List<PanelTile> buildingsRow = new List<PanelTile>();
			int actualRow = 0;
			foreach (TileTemplate bt in row) {
				GameObject newItem = Instantiate(itemPrefab) as GameObject;

				PanelTile b = newItem.GetComponent<PanelTile>();
				//panel tiles bg doesn't have panelTile
				if (b != null) {
					float pan = (actualRow - ((columnCount-1)/2f))/((columnCount-1)/2f);
					AvatarModel am = null;
					if (bt.Card != null) {
						am = new AvatarModel(bt.Card, true, null);
					}
					b.PanelAvatar.GetComponent<PanelAvatar>().Model = am;
				}

				ElementsToPut.Add(newItem);
				buildingsRow.Add(b);
				actualRow++;
			}
			tiles.Add(buildingsRow);
		}

		Prepare();

		//create neighbours -> optimization
		int x = 0;
		foreach (List<PanelTile> cols in tiles) {
			int y = 0;
			foreach (PanelTile bi in cols) {
				//panel tiles bg doesn't have panelTile
				if (bi == null) {
					continue;
				}
				PanelTile bu = bi.GetComponent<PanelTile>();

				if (tiles.Count > x + 1) {
					bu.Neighbours.Add(Side.Down, tiles[x + 1][y]);
				}

				if (tiles[x].Count > y + 1) {
					bu.Neighbours.Add(Side.Right, tiles[x][y + 1]);
				}

				if (x - 1 >= 0) {
					bu.Neighbours.Add(Side.Up, tiles[x - 1][y]);
				}

				if (y - 1 >= 0) {
					bu.Neighbours.Add(Side.Left, tiles[x][y-1]);
				}

				if (x - 1 >= 0 && y - 1 >= 0) {
					bu.Neighbours.Add(Side.UpLeft, tiles[x - 1][y - 1]);
				}
				if (x - 1 >= 0 && tiles[x - 1].Count > y + 1) {
					bu.Neighbours.Add(Side.UpRight, tiles[x - 1][y + 1]);
				}
				if (tiles.Count > x + 1 && y - 1 >= 0) {
					bu.Neighbours.Add(Side.DownLeft, tiles[x + 1][y - 1]);
				}
				if (tiles.Count > x + 1 && tiles[x+1].Count  > y+1){
					bu.Neighbours.Add(Side.DownRight, tiles[x + 1][y + 1]);
				}

				y++;
			}
			x++;
		}

		itemPrefab.SetActive(false);
	}

}
