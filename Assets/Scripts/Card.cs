using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Card  {

	public string Name;
	public Sprite Animation;
	public int Cost;
	public Dictionary<ParamType, int> Params;

	public Card(string name, int cost, Dictionary<ParamType, int> effects) {
		Name = name;
		Cost = cost;
		Animation = Resources.Load<Sprite>("Images/Cards/" + name);
		Params = effects;
	}

	public static readonly Card IceBolt = new Card("Ice bolt", 2, new Dictionary<ParamType, int>() { { ParamType.Damage, 10 }, { ParamType.Speed, 2 }, { ParamType.Distance, 2 } });
	public static readonly Card Mud = new Card("Mud", 4, new Dictionary<ParamType, int>() { { ParamType.Damage, 1 }, { ParamType.Distance, 1 } });
	public static readonly Card Fireball = new Card("Fireball", 4, new Dictionary<ParamType, int>() { { ParamType.Damage, 6}, {ParamType.Distance, 5} });
	public static readonly Card Dementor = new Card("Dementor", 5, new Dictionary<ParamType, int>() { { ParamType.Health, 30 }, {ParamType.Attack, 1}, {ParamType.Speed, 1} });
	public static readonly Card Lasia = new Card("Lasia", 5, new Dictionary<ParamType, int>() { { ParamType.Health, 30 }, {ParamType.Attack, 1}, {ParamType.Speed, 1}});
	public static readonly Card Zombie = new Card("Wisp", 0, new Dictionary<ParamType, int>() { { ParamType.Health, 1 }, { ParamType.Attack, 1 }, { ParamType.Distance, 1 }, {ParamType.Speed, 1}});
	public static readonly Card HealingTouch = new Card("Healing touch", 4, new Dictionary<ParamType, int>() { { ParamType.Distance, 3 }, {ParamType.Heal, 10} });
}
