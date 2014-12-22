using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public enum CardTarget {
	Empty, FriendlyMinion, Self, Minion, JustThrow, Character, OtherFriendlyMinion
}
public enum IsCastOn {
	Target,
	AllFriendlyMinions,
	OtherFriendlyMinions,
	AdjacentFriendlyMinions,
	AdjacentFriendlyCharacters,
}
public enum CardPersistency {
	Minion, UntilEndTurn, WhileHolderAlive, Instant, Hero, EveryActionRevalidate
}
public enum Effect {
	Battlecry, WhileAlive
}

public class Card  {

	public string Name;
	public Sprite Animation;
	public int Cost;
	public CardPersistency CardPersistency;
	public CardTarget CardTarget;
	public IsCastOn IsCastOn;
	public int CastDistance;
	public Dictionary<ParamType, int> Params;
	public Dictionary<Effect, Card> Effects;


	public Card(string name, int cost, CardPersistency cardPersistency, CardTarget cardTarget, int castDistance, IsCastOn cardIsCastOn, Dictionary<ParamType, int> param, Dictionary<Effect, Card> effects = null) {
		Name = name;
		Cost = cost;
		CardPersistency = cardPersistency;
		CardTarget = cardTarget;
		Animation = Resources.Load<Sprite>("Images/Cards/" + name);
		Params = param;
		Effects = effects!=null?effects:new Dictionary<Effect, Card>();
		CastDistance = castDistance;
		IsCastOn = cardIsCastOn;
	}

	public static readonly Card Wisp = new Card("Wisp", 0, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Health, 1 }, { ParamType.Attack, 1 }, {ParamType.Speed, 1}});
	public static readonly Card RockbiterWeapon = new Card("Rockbiter Weapon", 1, CardPersistency.UntilEndTurn, CardTarget.FriendlyMinion, 5, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.AttackAdd, 3 } });
	public static readonly Card FlametongueTotemAura = new Card("Flametongue Totem Aura", 1, CardPersistency.EveryActionRevalidate, CardTarget.Self, 0, IsCastOn.AdjacentFriendlyMinions, new Dictionary<ParamType, int>() { { ParamType.AttackAdd, 2 } });
	public static readonly Card FlametongueTotem= new Card("Flametongue Totem", 2, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { {ParamType.Health, 3}, {ParamType.Speed, 1} },
		new Dictionary<Effect,Card>() { {Effect.WhileAlive, FlametongueTotemAura} });
	public static readonly Card Thrallmar = new Card("Thrallmar", 2, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Health, 3 }, { ParamType.Attack, 2 }, { ParamType.Speed, 2 } });
	public static readonly Card BloodfenRaptor = new Card("Bloodfen Raptor", 2, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Health, 2 }, { ParamType.Attack, 3 }, { ParamType.Speed, 1 } });
	public static readonly Card MurlocScout = new Card("Murloc Scout", 0, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Health, 1 }, { ParamType.Attack, 1 }, { ParamType.Speed, 1 } });	
	public static readonly Card MurlocTidehunter = new Card("Murloc Tidehunter", 2, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Health, 1 }, { ParamType.Attack, 2 }, { ParamType.Speed, 1 } },
		new Dictionary<Effect, Card>(){ {Effect.Battlecry, MurlocScout}} );
	public static readonly Card ShatteredSunClericBlessing = new Card("Shattered Sun Cleric Blessing", 0, CardPersistency.WhileHolderAlive, CardTarget.OtherFriendlyMinion, 5, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.HealthAdd, 1 }, { ParamType.AttackAdd, 1 } });	
	public static readonly Card ShatteredSunCleric = new Card("Shattered Sun Cleric", 3, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Health, 2 }, { ParamType.Attack, 3 }, { ParamType.Speed, 1 } },
		new Dictionary<Effect, Card>() { { Effect.Battlecry, ShatteredSunClericBlessing } });
	public static readonly Card Hex = new Card("Hex", 3, CardPersistency.Minion, CardTarget.Minion, 5, IsCastOn.Target, new Dictionary<ParamType, int>() { {ParamType.ReplaceExisting, 1}, { ParamType.Health, 1 }, {ParamType.Speed, 1} });
	public static readonly Card RazorfensBoar = new Card("Razorfens Boar", 1, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Health, 1 }, { ParamType.Attack, 1 }, { ParamType.Speed, 1 } });		
	public static readonly Card RazorfenHunter = new Card("Razorfen Hunter", 3, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Health, 3 }, {ParamType.Attack, 2}, { ParamType.Speed, 1 } },
		new Dictionary<Effect, Card>() { { Effect.Battlecry, RazorfensBoar} });
	public static readonly Card GnomishInvention= new Card("Gnomish Invention", 0, CardPersistency.Instant, CardTarget.JustThrow, 0, IsCastOn.Target, new Dictionary<ParamType, int>() { {ParamType.HeroDrawsCard, 1} });
	public static readonly Card GnomishInventor = new Card("Gnomish Inventor", 4, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Health, 4 }, { ParamType.Attack, 2 }, { ParamType.Speed, 1 } },
		new Dictionary<Effect, Card>() { { Effect.Battlecry, GnomishInvention } });
	public static readonly Card ChillwindYeti = new Card("Chillwind Yeti", 4, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Health, 5 }, { ParamType.Attack, 4 }, { ParamType.Speed, 1 } });
	public static readonly Card PhysicalProtectionForAdjacent = new Card("Physical Protection for Adjacent", 1, CardPersistency.EveryActionRevalidate, CardTarget.Self, 0, IsCastOn.AdjacentFriendlyCharacters, new Dictionary<ParamType, int>() { { ParamType.PhysicalProtection, 1 } });
	public static readonly Card SenjinShieldmasta= new Card("Senjin Shieldmasta", 4, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Health, 5 }, { ParamType.Attack, 3 }, { ParamType.Speed, 1 } },
		new Dictionary<Effect, Card>() { { Effect.WhileAlive, PhysicalProtectionForAdjacent } });
	public static readonly Card FrostwolfCall = new Card("Frostwolf Call", 1, CardPersistency.WhileHolderAlive, CardTarget.Self, 0, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.AttackAddOfOtherFriendlyMinionNumber, 1 }, { ParamType.HealthAddOfOtherFriendlyMinionNumber, 1 } });
	public static readonly Card FrostwolfWarlord = new Card("Frostwolf Warlord", 5, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Health, 4 }, { ParamType.Attack, 4 }, { ParamType.Speed, 1 } },
		new Dictionary<Effect, Card>() { { Effect.Battlecry, FrostwolfCall} } );
	public static readonly Card Bloodlust = new Card("Bloodlust", 5, CardPersistency.UntilEndTurn, CardTarget.JustThrow, 0, IsCastOn.AllFriendlyMinions, new Dictionary<ParamType, int>() { { ParamType.AttackAdd, 3 } });
	public static readonly Card FireElementalsFireball = new Card("Fire Elementals Fireball", 2, CardPersistency.Instant, CardTarget.Character, 5, IsCastOn.Target, new Dictionary<ParamType, int>() {{ParamType.DealDamage, 3} });
	public static readonly Card FireElemental = new Card("Fire Elemental", 6, CardPersistency.Minion,CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Attack, 6 }, { ParamType.Health, 5 }, { ParamType.Speed, 1 } },
		new Dictionary<Effect, Card>() { { Effect.Battlecry, FireElementalsFireball } });
	public static readonly Card BoulderfishOgre = new Card("Boulderfist Ogre", 6, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Attack, 6 }, { ParamType.Health, 7 }, { ParamType.Speed, 1 } });
	public static readonly Card StormwindChampionsAura = new Card("Stormwind Champions Aura", 2, CardPersistency.EveryActionRevalidate, CardTarget.Self, 0, IsCastOn.OtherFriendlyMinions, new Dictionary<ParamType, int>() { { ParamType.AttackAdd, 1 }, { ParamType.HealthAdd, 1 } });
	public static readonly Card StormwindChampion = new Card("Stormwind Champion", 7, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Attack, 6 }, { ParamType.Health, 6 }, { ParamType.Speed, 1 }},
		new Dictionary<Effect, Card>() {{Effect.WhileAlive, StormwindChampionsAura} });

	//heroes
	public static readonly Card Dementor = new Card("Dementor", 5, CardPersistency.Hero, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Health, 30 }, {ParamType.Speed, 1} });
	public static readonly Card Lasia = new Card("Lasia", 5, CardPersistency.Hero, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Health, 30 }, {ParamType.Speed, 1}});
}


public enum CastedCardParamType {
	AttackAdd,
	HealthAdd, //this includes max health
	PhysicalProtection, //can not be targeted with physical attack

	//instants -> will be removed after dealing instants
	DealDamage,
	Heal,
	HeroDrawsCard,
}

public class CastedCard {

	public Dictionary<CastedCardParamType, int> Params = new Dictionary<CastedCardParamType, int>();
	public Card Card;
	public bool MarkedToRemove;

	public CastedCard(AvatarModel am, Card c) {
		Card = c;

		if (c.Params.ContainsKey(ParamType.DealDamage)) {
			Params.Add(CastedCardParamType.DealDamage, c.Params[ParamType.DealDamage]);
		}

		if (c.Params.ContainsKey(ParamType.Heal)) {
			Params.Add(CastedCardParamType.Heal, c.Params[ParamType.Heal]);
		}

		if (c.Params.ContainsKey(ParamType.HeroDrawsCard)) {
			Params.Add(CastedCardParamType.HeroDrawsCard, 1);
		}

		if (c.Params.ContainsKey(ParamType.AttackAdd)) {
			Params.Add(CastedCardParamType.AttackAdd, c.Params[ParamType.AttackAdd]);
		}
		
		if (c.Params.ContainsKey(ParamType.HealthAdd)) {
			Params.Add(CastedCardParamType.HealthAdd, c.Params[ParamType.HealthAdd]);
		}
		if (c.Params.ContainsKey(ParamType.HealthAddOfOtherFriendlyMinionNumber)) {
			Params.Add(CastedCardParamType.HealthAdd, AvatarModel.MyMinionsNumber(am.GetMyHero())- 1);//because itself doesn't count and its already casted
		}
		
		if (c.Params.ContainsKey(ParamType.AttackAddOfOtherFriendlyMinionNumber)) {
			Params.Add(CastedCardParamType.AttackAdd, AvatarModel.MyMinionsNumber(am.GetMyHero()) - 1); //because itself doesn't count and its already casted
		}

		if (c.Params.ContainsKey(ParamType.PhysicalProtection)) {
			Params.Add(CastedCardParamType.PhysicalProtection, 1);
		}

	}

	internal void DealInstants(AvatarModel am) {

		foreach (KeyValuePair<CastedCardParamType, int> kvp in Params.ToArray()) {
			switch (kvp.Key) {
					
				case CastedCardParamType.DealDamage:
					am.ActualHealth -= kvp.Value;
					Params.Remove(kvp.Key);
					break;
				case CastedCardParamType.HealthAdd:
					am.ActualHealth += kvp.Value;
					break;
				case CastedCardParamType.Heal:
					am.ActualHealth += kvp.Value;
					Params.Remove(kvp.Key);
					break;
				case CastedCardParamType.HeroDrawsCard:
					am.GetMyHero().PullCardFromDeck();
					Params.Remove(kvp.Key);
					break;
			}
		}
	}
}






