using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;

public delegate void LoadSprite(Sprite s);

public class SpriteManager : MonoBehaviour{

	public static Sprite Fight;
	public static Sprite ManaCrystallFull, ManaCrystalEmpty;

	public static Dictionary<AnimationType, Sprite> LasiaAnimations = new Dictionary<AnimationType, Sprite>();
	public static Dictionary<AnimationType, Sprite> DementorAnimations = new Dictionary<AnimationType, Sprite>();
	public static Dictionary<AnimationType, Sprite> OrbAnimations = new Dictionary<AnimationType, Sprite>();
	public static Dictionary<AnimationType, Sprite> IceBoltAnimations = new Dictionary<AnimationType, Sprite>();
	public static Dictionary<AnimationType, Sprite> MudAnimations = new Dictionary<AnimationType, Sprite>();
	public static Dictionary<AnimationType, Sprite> ZombieAnimations = new Dictionary<AnimationType, Sprite>();
	public static Dictionary<AnimationType, Sprite> ShoeAnimations = new Dictionary<AnimationType, Sprite>();
	public static Dictionary<AnimationType, Sprite> FireballAnimations = new Dictionary<AnimationType, Sprite>();

	public static Font Font;

	void Awake(){

		Font = Resources.Load<Font>("MunroSmall");

		ManaCrystallFull = Resources.Load<Sprite>("Images/manaCrystalFull");
		ManaCrystalEmpty = Resources.Load<Sprite>("Images/manaCrystalEmpty");

		Fight = Resources.Load<Sprite>("Images/fight");
		LasiaAnimations.Add(AnimationType.Icon, Resources.Load<Sprite>("Images/Lasia"));

		DementorAnimations.Add(AnimationType.Icon, Resources.Load<Sprite>("Images/zlyDementor"));

		OrbAnimations.Add(AnimationType.Icon, Resources.Load<Sprite>("Images/orb"));

		IceBoltAnimations.Add(AnimationType.Icon, Resources.Load<Sprite>("Images/iceIcon"));

		MudAnimations.Add(AnimationType.Icon, Resources.Load<Sprite>("Images/mudIcon"));

		ZombieAnimations.Add(AnimationType.Icon, Resources.Load<Sprite>("Images/zombie"));

		ShoeAnimations.Add(AnimationType.Icon, Resources.Load<Sprite>("Images/shoeIcon"));

		FireballAnimations.Add(AnimationType.Icon, Resources.Load<Sprite>("Images/fireballIcon"));
	}
}
