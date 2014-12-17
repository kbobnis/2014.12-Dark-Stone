using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Card  {

	public string Name;
	public int Cost;
	public Dictionary<AnimationType, Sprite> Animations;
	public Dictionary<ParamType, int> Params;

	public Card(string name, int cost, Dictionary<AnimationType, Sprite> animations, Dictionary<ParamType, int> effects) {
		if (!animations.ContainsKey(AnimationType.Icon)) {
			throw new Exception("All cards must have icon animation");
		}
		Name = name;
		Cost = cost;
		Animations = animations;
		Params = effects;
	}

	public static readonly Card IceBolt = new Card("Ice bolt", 2, SpriteManager.IceBoltAnimations, new Dictionary<ParamType, int>() { { ParamType.Damage, 10 }, { ParamType.Speed, 2 }, { ParamType.Distance, 2 } });
	public static readonly Card Mud = new Card("Mud", 4, SpriteManager.MudAnimations, new Dictionary<ParamType, int>() { { ParamType.Health, 15 }, { ParamType.Damage, 1 }, { ParamType.Speed, 1 }, { ParamType.Distance, 1 } });
	public static readonly Card Fireball = new Card("Fireball", 3, SpriteManager.FireballAnimations, new Dictionary<ParamType, int>() { { ParamType.Damage, 20}, { ParamType.Speed, 1 }, {ParamType.Distance, 1} });
	public static readonly Card ControlSelf = new Card("Control self", 1, SpriteManager.ShoeAnimations, new Dictionary<ParamType, int>() { {ParamType.OneTimeSpeed, 1} });
	public static readonly Card Dementor = new Card("Dementor", 5, SpriteManager.DementorAnimations, new Dictionary<ParamType, int>() { { ParamType.Health, 30 }, {ParamType.Damage, 1} });
	public static readonly Card Lasia = new Card("Lasia", 5, SpriteManager.LasiaAnimations, new Dictionary<ParamType, int>() { { ParamType.Health, 30 }, {ParamType.Damage, 1}});
	public static readonly Card Zombie = new Card("Zombie", 4, SpriteManager.ZombieAnimations, new Dictionary<ParamType, int>() { { ParamType.Health, 15 }, { ParamType.Damage, 1 }, { ParamType.Distance, 1 }, {ParamType.OneTimeSpeed, 1 }});
	public static readonly Card HealingTouch = new Card("Healing touch", 4, SpriteManager.ZombieAnimations, new Dictionary<ParamType, int>() { { ParamType.Distance, 3 }, {ParamType.Heal, 10} });
}
