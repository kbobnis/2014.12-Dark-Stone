using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum CardPersistency {
	Minion, UntilEndTurn, WhileHolderAlive, OneMilisecond, Hero, EveryActionRevalidate
}
public enum Effect {
	Battlecry, WhileAlive
}

public class Card  {

	public string Name;
	public Sprite Animation;
	public int Cost;
	public CardPersistency CardPersistency;
	public Dictionary<ParamType, int> Params;
	public Dictionary<Effect, Card> Effects;


	public Card(string name, int cost, CardPersistency cardPersistency, Dictionary<ParamType, int> param, Dictionary<Effect, Card> effects = null) {
		Name = name;
		Cost = cost;
		CardPersistency = cardPersistency;
		Animation = Resources.Load<Sprite>("Images/Cards/" + name);
		Params = param;
		Effects = effects!=null?effects:new Dictionary<Effect, Card>();

	}

	public static readonly Card Wisp = new Card("Wisp", 0, CardPersistency.Minion, new Dictionary<ParamType, int>() { { ParamType.Health, 1 }, { ParamType.Attack, 1 }, { ParamType.Distance, 1 }, {ParamType.Speed, 1}});

	public static readonly Card RockbiterWeapon = new Card("Rockbiter Weapon", 1, CardPersistency.UntilEndTurn, new Dictionary<ParamType, int>() { { ParamType.AttackAdd, 3 }, { ParamType.Distance, 5 } });
	public static readonly Card FlametongueTotemAura = new Card("Flametongue Totem Aura", 1, CardPersistency.EveryActionRevalidate, new Dictionary<ParamType, int>() { { ParamType.AttackAddForAdjacentFriendlyCharacters, 2 } });
	public static readonly Card FlametongueTotem= new Card("Flametongue Totem", 2, CardPersistency.Minion, new Dictionary<ParamType, int>() { { ParamType.Distance, 1 }, {ParamType.Health, 3}, {ParamType.Speed, 1} },
		new Dictionary<Effect,Card>() { {Effect.WhileAlive, FlametongueTotemAura} });
	public static readonly Card Thrallmar = new Card("Thrallmar", 2, CardPersistency.Minion, new Dictionary<ParamType, int>() { { ParamType.Health, 3 }, { ParamType.Attack, 2 }, { ParamType.Distance, 1 }, { ParamType.Speed, 2 } });
	public static readonly Card BloodfenRaptor = new Card("Bloodfen Raptor", 2, CardPersistency.Minion, new Dictionary<ParamType, int>() { { ParamType.Health, 2 }, { ParamType.Attack, 3 }, { ParamType.Distance, 1 }, { ParamType.Speed, 1 } });
	public static readonly Card MurlocScout = new Card("Murloc Scout", 0, CardPersistency.Minion, new Dictionary<ParamType, int>() { { ParamType.Health, 1 }, { ParamType.Attack, 1 }, { ParamType.Distance, 1 }, { ParamType.Speed, 1 } });	
	public static readonly Card MurlocTidehunter = new Card("Murloc Tidehunter", 2, CardPersistency.Minion, new Dictionary<ParamType, int>() { { ParamType.Health, 1 }, { ParamType.Attack, 2 }, { ParamType.Distance, 1 }, { ParamType.Speed, 1 } },
		new Dictionary<Effect, Card>(){ {Effect.Battlecry, MurlocScout}} );
	public static readonly Card ShatteredSunClericBlessing = new Card("Shattered Sun Cleric Blessing", 0, CardPersistency.WhileHolderAlive, new Dictionary<ParamType, int>() { { ParamType.HealthAdd, 1 }, { ParamType.AttackAdd, 1 }, { ParamType.Distance, 5 } });	
	public static readonly Card ShatteredSunCleric = new Card("Shattered Sun Cleric", 3, CardPersistency.Minion, new Dictionary<ParamType, int>() { { ParamType.Health, 2 }, { ParamType.Attack, 3 }, { ParamType.Distance, 1 }, { ParamType.Speed, 1 } },
		new Dictionary<Effect, Card>() { { Effect.Battlecry, ShatteredSunClericBlessing } });
	public static readonly Card Hex = new Card("Hex", 3, CardPersistency.Minion, new Dictionary<ParamType, int>() { {ParamType.ReplaceExisting, 1}, { ParamType.Health, 1 }, { ParamType.Distance, 5 }, {ParamType.Speed, 1} });
	public static readonly Card RazorfensBoar = new Card("Razorfens Boar", 1, CardPersistency.Minion, new Dictionary<ParamType, int>() { { ParamType.Health, 1 }, { ParamType.Attack, 1 }, { ParamType.Distance, 1 }, { ParamType.Speed, 1 } });		
	public static readonly Card RazorfenHunter = new Card("Razorfen Hunter", 3, CardPersistency.Minion, new Dictionary<ParamType, int>() { { ParamType.Health, 3 }, {ParamType.Attack, 2}, { ParamType.Distance, 1 }, { ParamType.Speed, 1 } },
		new Dictionary<Effect, Card>() { { Effect.Battlecry, RazorfensBoar} });
	public static readonly Card GnomishInvention= new Card("Gnomish Invention", 0, CardPersistency.OneMilisecond, new Dictionary<ParamType, int>() { {ParamType.HeroDrawsCard, 1} });
	public static readonly Card GnomishInventor = new Card("Gnomish Inventor", 4, CardPersistency.Minion, new Dictionary<ParamType, int>() { { ParamType.Health, 4 }, { ParamType.Attack, 2 }, { ParamType.Distance, 1 }, { ParamType.Speed, 1 } },
		new Dictionary<Effect, Card>() { { Effect.Battlecry, GnomishInvention } });
	public static readonly Card ChillwindYeti = new Card("Chillwind Yeti", 4, CardPersistency.Minion, new Dictionary<ParamType, int>() { { ParamType.Health, 5 }, { ParamType.Attack, 4 }, { ParamType.Distance, 1 }, { ParamType.Speed, 1 } });
	public static readonly Card PhysicalProtectionForAdjacent = new Card("Physical Protection for Adjacent", 1, CardPersistency.EveryActionRevalidate, new Dictionary<ParamType, int>() { { ParamType.PhysicalProtectionForFriendyAdjacentCharactersracters, 1 } });
	public static readonly Card SenjinShieldmasta= new Card("Senjin Shieldmasta", 4, CardPersistency.Minion, new Dictionary<ParamType, int>() { { ParamType.Health, 5 }, { ParamType.Attack, 3 }, { ParamType.Distance, 1 }, { ParamType.Speed, 1 } },
		new Dictionary<Effect, Card>() { { Effect.WhileAlive, PhysicalProtectionForAdjacent } });
	public static readonly Card FrostwolfCall = new Card("Frostwolf Call", 1, CardPersistency.WhileHolderAlive, new Dictionary<ParamType, int>() { { ParamType.AttackAddOfFriendlyMinionNumber, 1 }, { ParamType.HealthAddOfOtherFriendlyMinionNumber, 1 } });
	public static readonly Card FrostwolfWarlord = new Card("Frostwolf Warlord", 5, CardPersistency.Minion, new Dictionary<ParamType, int>() { { ParamType.Health, 4 }, { ParamType.Attack, 4 }, { ParamType.Distance, 1 }, { ParamType.Speed, 1 } },
		new Dictionary<Effect, Card>() { { Effect.Battlecry, FrostwolfCall} } );
	public static readonly Card Bloodlust = new Card("Bloodlust", 5, CardPersistency.UntilEndTurn, new Dictionary<ParamType, int>() { {ParamType.AttackAdd, 3} });
	public static readonly Card FireElementalsFireball = new Card("Fire Elementals Fireball", 2, CardPersistency.OneMilisecond, new Dictionary<ParamType, int>() { {ParamType.Distance, 5}, {ParamType.DealDamage, 3} });
	public static readonly Card FireElemental = new Card("Fire Elemental", 6, CardPersistency.Minion, new Dictionary<ParamType, int>() { { ParamType.Attack, 6 }, { ParamType.Health, 5 }, { ParamType.Distance, 1 }, { ParamType.Speed, 1 } },
		new Dictionary<Effect, Card>() { { Effect.Battlecry, FireElementalsFireball } });
	public static readonly Card BoulderfishOgre = new Card("Boulderfist Ogre", 6, CardPersistency.Minion, new Dictionary<ParamType, int>() { { ParamType.Attack, 6 }, { ParamType.Health, 7 }, { ParamType.Distance, 1 }, { ParamType.Speed, 1 } });
	public static readonly Card StormwindChampionsAura = new Card("Stormwind Champions Aura", 2, CardPersistency.EveryActionRevalidate, new Dictionary<ParamType, int>() { { ParamType.AttackAddForOtherFriendlyMinions, 1 }, { ParamType.HealthAddForFriendlyMinions, 1 } });
	public static readonly Card StormwindChampion = new Card("Stormwind Champion", 7, CardPersistency.Minion, new Dictionary<ParamType, int>() { { ParamType.Attack, 6 }, { ParamType.Health, 6 }, { ParamType.Distance, 1 }, { ParamType.Speed, 1 }},
		new Dictionary<Effect, Card>() {{Effect.WhileAlive, StormwindChampionsAura} });


	//heroes
	public static readonly Card Dementor = new Card("Dementor", 5, CardPersistency.Hero, new Dictionary<ParamType, int>() { { ParamType.Health, 30 }, {ParamType.Speed, 1} });
	public static readonly Card Lasia = new Card("Lasia", 5, CardPersistency.Hero, new Dictionary<ParamType, int>() { { ParamType.Health, 30 }, {ParamType.Speed, 1}});
}


public enum CastedCardParamType {
	AttackAdd,
	HealthAdd, //this includes max health
	PhysicalProtection, //can not be targeted with physical attack
}

public class CastedCard {

	public Dictionary<CastedCardParamType, int> Params = new Dictionary<CastedCardParamType, int>();
	public Card Card;

	public CastedCard(AvatarModel castingOn, Card c) {
		Card = c;

		//------------instants

		//just deal the damage
		if (c.Params.ContainsKey(ParamType.DealDamage)) {
			if (castingOn == null) {
				throw new Exception("Can not cast damage spell on nothing.");
			}
			castingOn.ActualHealth -= c.Params[ParamType.DealDamage];
		}
		//if is a spell, then doing the magic
		if (c.Params.ContainsKey(ParamType.Heal)) {
			castingOn.ActualHealth += c.Params[ParamType.Heal];
		}

		if (c.Params.ContainsKey(ParamType.HeroDrawsCard)) {
			//this is an exception. the card draws our avatar
			PanelMinigame.Me.ActualTurnModel.PullCardFromDeck();
		}

		//------------non instants
		if (c.Params.ContainsKey(ParamType.AttackAdd)) {
			Params.Add(CastedCardParamType.AttackAdd, c.Params[ParamType.AttackAdd]);
		}
		
		if (c.Params.ContainsKey(ParamType.HealthAdd)) {
			Params.Add(CastedCardParamType.HealthAdd, c.Params[ParamType.HealthAdd]);
		}
		
		if (c.Params.ContainsKey(ParamType.AttackAddForOtherFriendlyMinions)) {
			Params.Add(CastedCardParamType.AttackAdd, PanelMinigame.Me.MyMinionNumber()- 1); //because itself doesn't count and its already casted
		}

		if (c.Params.ContainsKey(ParamType.HealthAddOfOtherFriendlyMinionNumber)) {
			Params.Add(CastedCardParamType.HealthAdd, PanelMinigame.Me.MyMinionNumber() - 1);//because itself doesn't count and its already casted
		}

		if (c.Params.ContainsKey(ParamType.AttackAddForAdjacentFriendlyCharacters)) {
			Params.Add(CastedCardParamType.AttackAdd, c.Params[ParamType.AttackAddForAdjacentFriendlyCharacters]);
		}

		if (c.Params.ContainsKey(ParamType.PhysicalProtectionForFriendyAdjacentCharactersracters)) {
			Params.Add(CastedCardParamType.PhysicalProtection, 1);
		}

	}
}






