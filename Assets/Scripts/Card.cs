using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Card  {

	public string Name;
	public Sprite Animation;
	public int Cost;
	public Dictionary<ParamType, int> Params;
	public Dictionary<Effect, Card> Effects;


	public Card(string name, int cost, Dictionary<ParamType, int> param, Dictionary<Effect, Card> effects = null) {
		Name = name;
		Cost = cost;
		Animation = Resources.Load<Sprite>("Images/Cards/" + name);
		Params = param;
		Effects = effects!=null?effects:new Dictionary<Effect, Card>();
	}

	public static readonly Card Wisp = new Card("Wisp", 0, new Dictionary<ParamType, int>() { { ParamType.Health, 1 }, { ParamType.Attack, 1 }, { ParamType.Distance, 1 }, {ParamType.Speed, 1}});
	
	public static readonly Card RockbiterWeapon = new Card("Rockbiter Weapon", 1, new Dictionary<ParamType, int>() { { ParamType.AttackThisTurn, 3 }, { ParamType.Distance, 5 } });
	public static readonly Card FlametongueTotem= new Card("Flametongue Totem", 2, new Dictionary<ParamType, int>() { { ParamType.AttackForAdjacent, 2 }, { ParamType.Distance, 1 }, {ParamType.Health, 3}, {ParamType.Speed, 1} });
	public static readonly Card Thrallmar = new Card("Thrallmar", 2, new Dictionary<ParamType, int>() { { ParamType.Health, 3 }, { ParamType.Attack, 2 }, { ParamType.Distance, 1 }, { ParamType.Speed, 2 } });
	public static readonly Card BloodfenRaptor = new Card("Bloodfen Raptor", 2, new Dictionary<ParamType, int>() { { ParamType.Health, 2 }, { ParamType.Attack, 3 }, { ParamType.Distance, 1 }, { ParamType.Speed, 1 } });
	public static readonly Card MurlocScout = new Card("Murloc Scout", 0, new Dictionary<ParamType, int>() { { ParamType.Health, 1 }, { ParamType.Attack, 1 }, { ParamType.Distance, 1 }, { ParamType.Speed, 1 } });	
	public static readonly Card MurlocTidehunter = new Card("Murloc Tidehunter", 2, new Dictionary<ParamType, int>() { { ParamType.Health, 1 }, { ParamType.Attack, 2 }, { ParamType.Distance, 1 }, { ParamType.Speed, 1 } },
		new Dictionary<Effect, Card>(){ {Effect.Battlecry, MurlocScout}} );
	public static readonly Card ShatteredSunClericBlessing = new Card("Shattered Sun Cleric Blessing", 0, new Dictionary<ParamType, int>() { { ParamType.AdditionalHealth, 1 }, { ParamType.AdditionalAttack, 1 }, { ParamType.Distance, 5 } });	
	public static readonly Card ShatteredSunCleric = new Card("Shattered Sun Cleric", 3, new Dictionary<ParamType, int>() { { ParamType.Health, 2 }, { ParamType.Attack, 3 }, { ParamType.Distance, 1 }, { ParamType.Speed, 1 } },
		new Dictionary<Effect, Card>() { { Effect.Battlecry, ShatteredSunClericBlessing } });
	public static readonly Card Hex = new Card("Hex", 0, new Dictionary<ParamType, int>() { {ParamType.ReplaceExisting, 1}, { ParamType.Health, 1 }, { ParamType.Distance, 5 }, {ParamType.Speed, 1} });		

	public static readonly Card IceBolt = new Card("Ice bolt", 2, new Dictionary<ParamType, int>() { { ParamType.Damage, 10 }, { ParamType.Speed, 2 }, { ParamType.Distance, 2 } });
	public static readonly Card Mud = new Card("Mud", 4, new Dictionary<ParamType, int>() { { ParamType.Damage, 1 }, { ParamType.Distance, 1 } });
	public static readonly Card HealingTouch = new Card("Healing touch", 4, new Dictionary<ParamType, int>() { { ParamType.Distance, 3 }, {ParamType.Heal, 10} });
	public static readonly Card Fireball = new Card("Fireball", 4, new Dictionary<ParamType, int>() { { ParamType.Damage, 6}, {ParamType.Distance, 5} });

	//heroes
	public static readonly Card Dementor = new Card("Dementor", 5, new Dictionary<ParamType, int>() { { ParamType.Health, 30 }, {ParamType.Speed, 1} });
	public static readonly Card Lasia = new Card("Lasia", 5, new Dictionary<ParamType, int>() { { ParamType.Health, 30 }, {ParamType.Speed, 1}});
}

public enum Effect {
	Battlecry
}



