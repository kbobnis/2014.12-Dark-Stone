using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public delegate void LoadSprite(Sprite s);

public class SpriteManager : MonoBehaviour{

	private static List<KeyValuePair<string, LoadSprite>> SpritesToLoad = new List<KeyValuePair<string, LoadSprite>>();
	private static Dictionary<string, int> Retries = new Dictionary<string, int>();
	private static bool DownloadingNow = false;
	private static Dictionary<string, Sprite> LoadedSprites = new Dictionary<string, Sprite>();

	public static Dictionary<AnimationType, Sprite> LasiaAnimations = new Dictionary<AnimationType, Sprite>();
	public static Dictionary<AnimationType, Sprite> DementorAnimations = new Dictionary<AnimationType, Sprite>();
	public static Dictionary<AnimationType, Sprite> OrbAnimations = new Dictionary<AnimationType, Sprite>();
	public static Dictionary<AnimationType, Sprite> IceBoltAnimations = new Dictionary<AnimationType, Sprite>();
	public static Dictionary<AnimationType, Sprite> MudAnimations = new Dictionary<AnimationType, Sprite>();
	


	static SpriteManager(){
		LasiaAnimations.Add(AnimationType.OnBoard, Resources.Load<Sprite>("Images/Lasia"));
		LasiaAnimations.Add(AnimationType.OnBoardDead, Resources.Load<Sprite>("Images/LasiaDead"));

		DementorAnimations.Add(AnimationType.OnBoard, Resources.Load<Sprite>("Images/zlyDementor"));

		OrbAnimations.Add(AnimationType.Icon, Resources.Load<Sprite>("Images/orb"));
		OrbAnimations.Add(AnimationType.OnBoard, Resources.Load<Sprite>("Images/orb"));

		IceBoltAnimations.Add(AnimationType.Icon, Resources.Load<Sprite>("Images/iceIcon"));
		IceBoltAnimations.Add(AnimationType.OnBoard, Resources.Load<Sprite>("Images/iceAnimation"));
		IceBoltAnimations.Add(AnimationType.OnBoardExplode, Resources.Load<Sprite>("Images/iceAnimationExplode"));

		MudAnimations.Add(AnimationType.Icon, Resources.Load<Sprite>("Images/mudIcon"));
		MudAnimations.Add(AnimationType.OnBoard, Resources.Load<Sprite>("Images/mudEffect"));


		Debug.Log("lasia animation: " + LasiaAnimations[AnimationType.OnBoard]);

	}

	public static void LoadAsynchronous(string path, LoadSprite ls) {
		if (LoadedSprites.ContainsKey(path)) {
			ls(LoadedSprites[path]);
		} else {
			SpritesToLoad.Add(new KeyValuePair<string, LoadSprite>(path, ls));
		}
	}

	void Update() {
		if (SpritesToLoad.Count > 0) {
			KeyValuePair<string, LoadSprite> kvp = SpritesToLoad[0];
			
			if (LoadedSprites.ContainsKey(kvp.Key)) {
				kvp.Value(LoadedSprites[kvp.Key]);
				SpritesToLoad.Remove(kvp);
			} else if (DownloadingNow == false){
				DownloadingNow = true;
				StartCoroutine(LoadFromUrl(kvp.Key, kvp.Value));
				SpritesToLoad.Remove(kvp);
			}
		}
	}

	private IEnumerator LoadFromUrl(string path, LoadSprite ls){
		string url = path;
    
        WWW www = new WWW(url);
		
        yield return www;
		if (www.error != null) {
			//will retry
			if (!Retries.ContainsKey(path)){
				Retries.Add(path, 0);
			}
			Retries[path]++;

			if (Retries[path] < 3) {
				Debug.Log("Error " + www.error + ", when downloading " + path + ", retries: " + Retries[path]);
				SpritesToLoad.Add(new KeyValuePair<string, LoadSprite>(path, ls));
			}
		} else {
			Sprite s = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0.5f, 0.5f));
			s.texture.filterMode = FilterMode.Point;
			ls(s);
		}

		DownloadingNow = false;
	}

}
