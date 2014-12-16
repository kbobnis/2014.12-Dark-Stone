using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Card  {

	public string Name;
	public int Cost;
	public Dictionary<AnimationType, Sprite> Animations;
	public Dictionary<ParamType, int> Params;
	public List<Card> Spells;

	public Card(string name, int cost, Dictionary<AnimationType, Sprite> animations, Dictionary<ParamType, int> effects, List<Card> spells = null) {
		Name = name;
		Cost = cost;
		Animations = animations;
		Params = effects;
		Spells = spells == null ? new List<Card>() : spells;
	}

	public static readonly Card IceBolt = new Card("Ice bolt", 2, SpriteManager.IceBoltAnimations, new Dictionary<ParamType, int>() { { ParamType.Health, 1 }, { ParamType.Damage, 10 }, { ParamType.Speed, 2 }, { ParamType.Distance, 2 } });
	public static readonly Card Mud = new Card("Mud", 4, SpriteManager.MudAnimations, new Dictionary<ParamType, int>() { { ParamType.Health, 15 }, { ParamType.Damage, 1 }, { ParamType.Speed, 1 }, { ParamType.Distance, 1 } });
	public static readonly Card Fireball = new Card("Fireball", 3, SpriteManager.FireballAnimations, new Dictionary<ParamType, int>() { { ParamType.Health, 1 }, { ParamType.Damage, 20}, { ParamType.Speed, 1 }, {ParamType.Distance, 1} });
	public static readonly Card ControlSelf = new Card("Control self", 1, SpriteManager.ShoeAnimations, new Dictionary<ParamType, int>() { }, null);
	public static readonly Card Dementor = new Card("Dementor", 5, SpriteManager.DementorAnimations, new Dictionary<ParamType, int>() { { ParamType.Health, 30 }, { ParamType.HisMana, 5 }, { ParamType.IncreaseManaEachTurn, 1 }, {ParamType.Damage, 1} });
	public static readonly Card Lasia = new Card("Lasia", 5, SpriteManager.LasiaAnimations, new Dictionary<ParamType, int>() { { ParamType.Health, 30 }, {ParamType.HisMana, 50}, {ParamType.IncreaseManaEachTurn, 1}, {ParamType.Damage, 1}});
	public static readonly Card Zombie = new Card("Zombie", 4, SpriteManager.ZombieAnimations, new Dictionary<ParamType, int>() { { ParamType.Health, 15 }, { ParamType.Damage, 1 }, { ParamType.Distance, 2 }, {ParamType.HisMana, 1}, {ParamType.IncreaseManaEachTurn, 1}, {ParamType.Speed, 1 }}, new List<Card>(){ControlSelf, ControlSelf, ControlSelf});
}
