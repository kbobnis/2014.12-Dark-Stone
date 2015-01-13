using UnityEngine;
using System.Collections;

public enum ParamType {
	Attack,
	AttackAdd,
	AttackAddOfOtherFriendlyMinionNumber,
	Health,
	HealthAdd,
	HealthAddOfOtherFriendlyMinionNumber,
	DealDamage,
	DealDamageSpell,
	Speed,
	Heal,
	HeroDrawsCard,
	ReplaceExisting,
	SpellDamageAdd,
	ManaCrystalAdd,
	ManaCrystalEmptyAdd,
	ArmorAdd,
	TakeControl,
	Taunt,
	Charge,
	HealFull,
}

public static class ParamTypeMethod {

	public static string Describe(this ParamType pt) {
		switch (pt) {
			case ParamType.AttackAdd: return "Add attack";
			case ParamType.AttackAddOfOtherFriendlyMinionNumber: return "Add +1 attack for each other friendly minion";
			case ParamType.HealthAdd: return "Add health";
			case ParamType.HealthAddOfOtherFriendlyMinionNumber: return "Add +1 health for each other friendly minion";
			case ParamType.DealDamage: return "Deal damage";
			case ParamType.DealDamageSpell: return "Spell damage";
			case ParamType.HeroDrawsCard: return "Draw a card";
			case ParamType.ReplaceExisting: return "Replace minion with this";
			case ParamType.SpellDamageAdd: return "Spell damage +";
			case ParamType.ManaCrystalAdd: return "Add mana crystal";
			case ParamType.ManaCrystalEmptyAdd: return "Gain empty mana crystal";
			case ParamType.ArmorAdd: return "Gain armor";
			case ParamType.TakeControl: return "Take control of enemy minion";
			case ParamType.HealFull: return "Full health heal";
		}
		return pt.ToString();
	}
}
