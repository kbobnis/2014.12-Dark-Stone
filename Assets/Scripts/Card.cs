using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Card  {

	public string Name;
	public int Cost;
	public Dictionary<AnimationType, Sprite> Animations;
	public Dictionary<ParamType, int> Params;

	public Card(string name, int cost, Dictionary<AnimationType, Sprite> animations, Dictionary<ParamType, int> effects) {
		Name = name;
		Cost = cost;
		Animations = animations;
		Params = effects;
	}

	public static readonly Card Lasia = new Card("Lasia", 5, SpriteManager.LasiaAnimations, new Dictionary<ParamType, int>() { { ParamType.Health, 30 }});
	public static readonly Card Dementor = new Card("Dementor", 5, SpriteManager.DementorAnimations, new Dictionary<ParamType, int>(){ { ParamType.Health, 30}});
	public static readonly Card IceBolt = new Card("Ice bolt", 2, SpriteManager.IceBoltAnimations, new Dictionary<ParamType, int>(){ { ParamType.Health, 1}, {ParamType.Damage, 1}, {ParamType.Speed, 2} });
	public static readonly Card Mud = new Card("Mud", 4, SpriteManager.MudAnimations, new Dictionary<ParamType, int>() { { ParamType.Health, 15 }, { ParamType.Damage, 1}, {ParamType.Speed, 1} });
	public static readonly Card Orb = new Card("Orb", 10, SpriteManager.OrbAnimations, new Dictionary<ParamType, int>() { { ParamType.Health, 30 } });

}
