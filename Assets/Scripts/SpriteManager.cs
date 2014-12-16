using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public delegate void LoadSprite(Sprite s);

public class SpriteManager : MonoBehaviour{

	public static Sprite Fight;

	public static Dictionary<AnimationType, Sprite> LasiaAnimations = new Dictionary<AnimationType, Sprite>();
	public static Dictionary<AnimationType, Sprite> DementorAnimations = new Dictionary<AnimationType, Sprite>();
	public static Dictionary<AnimationType, Sprite> OrbAnimations = new Dictionary<AnimationType, Sprite>();
	public static Dictionary<AnimationType, Sprite> IceBoltAnimations = new Dictionary<AnimationType, Sprite>();
	public static Dictionary<AnimationType, Sprite> MudAnimations = new Dictionary<AnimationType, Sprite>();
	public static Dictionary<AnimationType, Sprite> ZombieAnimations = new Dictionary<AnimationType, Sprite>();
	public static Dictionary<AnimationType, Sprite> ShoeAnimations = new Dictionary<AnimationType, Sprite>();
	public static Dictionary<AnimationType, Sprite> FireballAnimations = new Dictionary<AnimationType, Sprite>();


	void Awake(){

		Fight = Resources.Load<Sprite>("Images/fight");
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

		ZombieAnimations.Add(AnimationType.OnBoard, Resources.Load<Sprite>("Images/zombieAnimation"));
		ZombieAnimations.Add(AnimationType.Icon, Resources.Load<Sprite>("Images/zombie"));

		ShoeAnimations.Add(AnimationType.Icon, Resources.Load<Sprite>("Images/shoeIcon"));
		ShoeAnimations.Add(AnimationType.OnBoard, Resources.Load<Sprite>("Images/shoeAnimation"));

		FireballAnimations.Add(AnimationType.Icon, Resources.Load<Sprite>("Images/fireballIcon"));
		FireballAnimations.Add(AnimationType.OnBoard, Resources.Load<Sprite>("Images/fireball"));
	}
}
