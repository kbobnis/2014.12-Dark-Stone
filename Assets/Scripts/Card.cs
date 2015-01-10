﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

public enum CardTarget {
	Empty, 
	FriendlyMinion, 
	EnemyMinion,
	Self, 
	Minion, 
	JustThrow, 
	Character, 
	OtherFriendlyMinion,
	EnemyCharacter,
	EnemyHero
}
public enum IsCastOn {
	Target,
	AllFriendlyMinions,
	OtherFriendlyMinions,
	AdjacentFriendlyMinions,
	AdjacentFriendlyCharacters,
	AllEnemyCharacters,
	AllEnemyMinions,
	OtherItsCharacters,
	FriendlyHero,
	AllFriendlyCharacters,
	EnemyHero
}
public enum CardPersistency {
	Minion, UntilEndTurn, WhileHolderAlive, Instant, Hero, EveryActionRevalidate
}

public static class CardPersistencyMethods {

	public static bool IsCharacter(this CardPersistency cp) {
		return cp == CardPersistency.Minion || cp == CardPersistency.Hero;
	}
}
public enum Effect {
	Battlecry,
	WhileAlive,
	AfterTakingDamage,
	HerosSpecialPower,
	AtEndTurn
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


	public static readonly Card Moonfire = new Card("Moonfire", 0, CardPersistency.Instant, CardTarget.Character, 5, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.DealDamageSpell, 1 } });
	public static readonly Card Innervate = new Card("Innervate", 0, CardPersistency.UntilEndTurn, CardTarget.JustThrow, 0, IsCastOn.FriendlyHero, new Dictionary<ParamType, int>() { {ParamType.ManaCrystalAdd, 2} });
	public static readonly Card Wisp = new Card("Wisp", 0, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Health, 1 }, { ParamType.Attack, 1 }, {ParamType.Speed, 1}});
	public static readonly Card ElvenShot = new Card("Elven Shot", 0, CardPersistency.Instant, CardTarget.Character, 5, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.DealDamage, 1 } });	
	public static readonly Card ElvenArcher = new Card("Elven Archer", 1, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Health, 1 }, { ParamType.Attack, 1 }, { ParamType.Speed, 1 } },
		new Dictionary<Effect, Card>() { {Effect.Battlecry, ElvenShot}});
	public static readonly Card AncestralHealing = new Card("Ancestral Healing", 0, CardPersistency.WhileHolderAlive, CardTarget.Minion, 5, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Taunt, 1 }, {ParamType.HealFull, 1} });
	public static readonly Card ClawsArmor = new Card("Claws Armor", 0, CardPersistency.WhileHolderAlive, CardTarget.Self, 0, IsCastOn.Target, new Dictionary<ParamType, int>() { {ParamType.ArmorAdd, 2} });
	public static readonly Card Claw = new Card("Claw", 1, CardPersistency.UntilEndTurn, CardTarget.JustThrow, 0, IsCastOn.FriendlyHero, new Dictionary<ParamType, int>() { {ParamType.AttackAdd, 2 } }, 
		new Dictionary<Effect,Card>() { { Effect.Battlecry, ClawsArmor} });
	public static readonly Card RockbiterWeapon = new Card("Rockbiter Weapon", 1, CardPersistency.UntilEndTurn, CardTarget.FriendlyMinion, 5, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.AttackAdd, 3 } });
	public static readonly Card FlametongueTotemAura = new Card("Flametongue Totem Aura", 1, CardPersistency.EveryActionRevalidate, CardTarget.Self, 0, IsCastOn.AdjacentFriendlyMinions, new Dictionary<ParamType, int>() { { ParamType.AttackAdd, 2 } });
	public static readonly Card GoldshireFootman = new Card("Goldshire Footman", 1, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Health, 2 }, { ParamType.Attack, 1 }, { ParamType.Taunt, 1 }, {ParamType.Speed, 1 }});
	public static readonly Card StonetuskBoar= new Card("Stonetusk Boar", 1, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Health, 1 }, { ParamType.Attack, 1 }, { ParamType.Charge, 1 }, { ParamType.Speed, 1 }  });
	public static readonly Card VoodooDoctorMedicine = new Card("Voodoo Doctor Medicine", 1, CardPersistency.Instant, CardTarget.Character, 5, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Heal, 2 } });
	public static readonly Card VoodooDoctor = new Card("Voodoo Doctor", 1, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Health, 1 }, { ParamType.Attack, 2 }, { ParamType.Speed, 1 } },
		new Dictionary<Effect, Card>() { { Effect.Battlecry, VoodooDoctorMedicine } });
	public static readonly Card HealingAura = new Card("Healing Aura", 1, CardPersistency.Instant, CardTarget.Self, 1, IsCastOn.AllFriendlyMinions, new Dictionary<ParamType, int>() { { ParamType.Heal, 1 } });
	public static readonly Card HealingTotem = new Card("Healing Totem", 1, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Health, 2 }, {ParamType.Speed, 1} },
		new Dictionary<Effect,Card>() { { Effect.AtEndTurn, HealingAura } });
	public static readonly Card WildGrowth = new Card("Wild Growth", 2, CardPersistency.WhileHolderAlive, CardTarget.Self, 0, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.ManaCrystalEmptyAdd, 1 } });
	public static readonly Card MarkOfTheWild = new Card("Mark of the Wild", 2, CardPersistency.WhileHolderAlive, CardTarget.Minion, 5, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.AttackAdd, 2 }, { ParamType.HealthAdd, 2 }, { ParamType.Taunt, 1 } });
	public static readonly Card FlametongueTotem= new Card("Flametongue Totem", 2, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { {ParamType.Health, 3}, {ParamType.Speed, 1} },
		new Dictionary<Effect,Card>() { {Effect.WhileAlive, FlametongueTotemAura} });
	public static readonly Card Thrallmar = new Card("Thrallmar", 2, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Health, 3 }, { ParamType.Attack, 2 }, { ParamType.Speed, 2 } });
	public static readonly Card BloodfenRaptor = new Card("Bloodfen Raptor", 2, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Health, 2 }, { ParamType.Attack, 3 }, { ParamType.Speed, 1 } });
	public static readonly Card MurlocScout = new Card("Murloc Scout", 0, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Health, 1 }, { ParamType.Attack, 1 }, { ParamType.Speed, 1 } });	
	public static readonly Card MurlocTidehunter = new Card("Murloc Tidehunter", 2, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Health, 1 }, { ParamType.Attack, 2 }, { ParamType.Speed, 1 } },
		new Dictionary<Effect, Card>(){ {Effect.Battlecry, MurlocScout}} );
	public static readonly Card KoboldsGeomancersPower = new Card("Kobold Geomancers Power", 1, CardPersistency.WhileHolderAlive, CardTarget.Self, 0, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.SpellDamageAdd, 1 } });
	public static readonly Card KoboldGeomancer = new Card("Kobold Geomancer", 2, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Health, 2 }, { ParamType.Attack, 2 }, { ParamType.Speed, 1 } },
		new Dictionary<Effect, Card>() { { Effect.WhileAlive, KoboldsGeomancersPower } });
	public static readonly Card FrostwolfGrunt = new Card("Frostwolf Grunt", 2, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Health, 2 }, { ParamType.Attack, 2 }, { ParamType.Taunt, 1 }, { ParamType.Speed, 1 } });
	public static readonly Card GnomishInvention= new Card("Gnomish Invention", 0, CardPersistency.Instant, CardTarget.JustThrow, 0, IsCastOn.Target, new Dictionary<ParamType, int>() { {ParamType.HeroDrawsCard, 1} });
	public static readonly Card NoviceEngineer = new Card("Novice Engineer", 2, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Health, 1 }, { ParamType.Attack, 1 }, { ParamType.Speed, 1 } },
		new Dictionary<Effect, Card>() { { Effect.Battlecry, Card.GnomishInvention } });
	public static readonly Card RiverCrocolisk = new Card("River Crocolisk", 2, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Health, 3 }, { ParamType.Attack, 2 }, { ParamType.Speed, 1 } });
	public static readonly Card ShatteredSunClericBlessing = new Card("Shattered Sun Cleric Blessing", 0, CardPersistency.WhileHolderAlive, CardTarget.OtherFriendlyMinion, 5, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.HealthAdd, 1 }, { ParamType.AttackAdd, 1 } });	
	public static readonly Card ShatteredSunCleric = new Card("Shattered Sun Cleric", 3, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Health, 2 }, { ParamType.Attack, 3 }, { ParamType.Speed, 1 } },
		new Dictionary<Effect, Card>() { { Effect.Battlecry, ShatteredSunClericBlessing } });
	public static readonly Card HealingTouch= new Card("Healing Touch", 3, CardPersistency.Instant, CardTarget.Character, 5, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Heal, 8 } });
	public static readonly Card SavageRoar= new Card("Savage Roar", 3, CardPersistency.UntilEndTurn, CardTarget.JustThrow, 0, IsCastOn.AllFriendlyCharacters, new Dictionary<ParamType, int>() { { ParamType.AttackAdd, 2 } });	
	public static readonly Card Hex = new Card("Hex", 3, CardPersistency.Minion, CardTarget.Minion, 5, IsCastOn.Target, new Dictionary<ParamType, int>() { {ParamType.ReplaceExisting, 1}, { ParamType.Health, 1 }, {ParamType.Speed, 1} });
	public static readonly Card IronfurGrizzly = new Card("Ironfur Grizzly", 3, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Health, 3 }, { ParamType.Attack, 3 }, { ParamType.Speed, 1 }, {ParamType.Taunt, 1}});
	public static readonly Card RazorfensBoar = new Card("Razorfens Boar", 1, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Health, 1 }, { ParamType.Attack, 1 }, { ParamType.Speed, 1 } });		
	public static readonly Card RazorfenHunter = new Card("Razorfen Hunter", 3, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Health, 3 }, {ParamType.Attack, 2}, { ParamType.Speed, 1 } },
		new Dictionary<Effect, Card>() { { Effect.Battlecry, RazorfensBoar} });
	public static readonly Card DalaranMage = new Card("Dalaran Mage", 3, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Health, 4 }, { ParamType.Attack, 1 }, { ParamType.Speed, 1 }, { ParamType.SpellDamageAdd, 1 } });
	public static readonly Card IronforgeRifleman = new Card("Ironforge Rifleman", 3, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Health, 2}, { ParamType.Attack, 2 }, { ParamType.Speed, 1 } },
		new Dictionary<Effect,Card>() { { Effect.Battlecry, ElvenShot }} );
	public static readonly Card MagmaRager = new Card("Magma Rager", 3, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Health, 1 }, { ParamType.Attack, 5 }, { ParamType.Speed, 1 }  });
	public static readonly Card RaidLeaderAura = new Card("Raid Leader Aura", 1, CardPersistency.EveryActionRevalidate, CardTarget.Self, 0, IsCastOn.OtherFriendlyMinions, new Dictionary<ParamType, int>() { { ParamType.AttackAdd, 1 } });
	public static readonly Card RaidLeader = new Card("Raid Leader", 3, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Attack, 2 }, { ParamType.Health, 2 }, { ParamType.Speed, 1 } },
		new Dictionary<Effect,Card>() { { Effect.WhileAlive, RaidLeaderAura }} );
	public static readonly Card Wolfrider = new Card("Wolfrider", 3, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Health, 1 }, { ParamType.Attack, 3 }, { ParamType.Speed, 1 }, {ParamType.Charge, 1} });
	public static readonly Card SilverbackPatriarch = new Card("Silverback Patriarch", 3, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Health, 4 }, { ParamType.Attack, 1 }, { ParamType.Speed, 1 }, {ParamType.Taunt, 1} });
	public static readonly Card GnomishInventor = new Card("Gnomish Inventor", 4, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Health, 4 }, { ParamType.Attack, 2 }, { ParamType.Speed, 1 } },
		new Dictionary<Effect, Card>() { { Effect.Battlecry, GnomishInvention } });
	public static readonly Card SwipeRicochet = new Card("Swipe Ricochet", 1, CardPersistency.Instant, CardTarget.Self, 0, IsCastOn.OtherItsCharacters, new Dictionary<ParamType, int>() { { ParamType.DealDamageSpell, 1 } });
	public static readonly Card Swipe = new Card("Swipe", 4, CardPersistency.Instant, CardTarget.EnemyCharacter, 5, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.DealDamageSpell, 4 } },
		new Dictionary<Effect,Card>() { { Effect.Battlecry, SwipeRicochet } });
	public static readonly Card ChillwindYeti = new Card("Chillwind Yeti", 4, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Health, 5 }, { ParamType.Attack, 4 }, { ParamType.Speed, 1 } });
	public static readonly Card MechanicalDragonling = new Card("Mechanical Dragonling", 1, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Health, 1 }, { ParamType.Attack, 2 }, { ParamType.Speed, 1 } });
	public static readonly Card DragonlingMechanic = new Card("Dragonling Mechanic", 4, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Health, 4 }, { ParamType.Attack, 2 }, { ParamType.Speed, 1 } },
		new Dictionary<Effect,Card>() { { Effect.Battlecry, MechanicalDragonling } });
	public static readonly Card SenjinShieldmasta = new Card("Senjin Shieldmasta", 4, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Health, 5 }, { ParamType.Attack, 3 }, { ParamType.Speed, 1 }, { ParamType.Taunt, 1 } });
	public static readonly Card OasisSnapjaw = new Card("Oasis Snapjaw", 4, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Health, 7 }, { ParamType.Attack, 2 }, { ParamType.Speed, 1 } });
	public static readonly Card OgreMagi = new Card("Ogre-magi", 4, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Health, 4 }, { ParamType.Attack, 4 }, { ParamType.Speed, 1 }, {ParamType.SpellDamageAdd, 1} });
	public static readonly Card StormwindKnight = new Card("Stormwind Knight", 4, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Health, 5 }, { ParamType.Attack, 2 }, { ParamType.Speed, 1 }, { ParamType.Charge, 1} });	
	public static readonly Card FrostwolfCall = new Card("Frostwolf Call", 1, CardPersistency.WhileHolderAlive, CardTarget.Self, 0, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.AttackAddOfOtherFriendlyMinionNumber, 1 }, { ParamType.HealthAddOfOtherFriendlyMinionNumber, 1 } });
	public static readonly Card FrostwolfWarlord = new Card("Frostwolf Warlord", 5, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Health, 4 }, { ParamType.Attack, 4 }, { ParamType.Speed, 1 } },
		new Dictionary<Effect, Card>() { { Effect.Battlecry, FrostwolfCall} } );
	public static readonly Card Bloodlust = new Card("Bloodlust", 5, CardPersistency.UntilEndTurn, CardTarget.JustThrow, 0, IsCastOn.AllFriendlyMinions, new Dictionary<ParamType, int>() { { ParamType.AttackAdd, 3 } });
	public static readonly Card BootyBayBodyguard = new Card("Booty Bay Bodyguard", 5, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Health, 4 }, { ParamType.Attack, 5 }, { ParamType.Speed, 1 }, { ParamType.Taunt, 1 } });
	public static readonly Card DarkscaleHeal = new Card("Darkscale Heal", 2, CardPersistency.Instant, CardTarget.JustThrow, 0, IsCastOn.AllFriendlyCharacters, new Dictionary<ParamType, int>() { { ParamType.Heal, 2 } });	
	public static readonly Card DarkscaleHealer = new Card("Darkscale Healer", 5, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Health, 5 }, { ParamType.Attack, 4 }, { ParamType.Speed, 1 }}, 
		new Dictionary<Effect,Card>() { { Effect.Battlecry, DarkscaleHeal }} );
	public static readonly Card GurubashiBerserkersRage= new Card("Gurubashi Berserkers Rage", 1, CardPersistency.WhileHolderAlive, CardTarget.Self, 0, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.AttackAdd, 3 } });	
	public static readonly Card GurubashiBerserker= new Card("Gurubashi Berserker", 5, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Health, 7 }, { ParamType.Attack, 2 }, { ParamType.Speed, 1 } },
		new Dictionary<Effect,Card>() { { Effect.AfterTakingDamage, GurubashiBerserkersRage } } );
	public static readonly Card NightbladesBlade = new Card("Nightblades Blade", 1, CardPersistency.Instant, CardTarget.Self, 5, IsCastOn.EnemyHero, new Dictionary<ParamType, int>() { { ParamType.DealDamage, 3 } });
	public static readonly Card Nightblade = new Card("Nightblade", 5, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Health, 4 }, { ParamType.Attack, 4 }, { ParamType.Speed, 1 } },
			new Dictionary<Effect, Card>() { { Effect.Battlecry, NightbladesBlade } });
	public static readonly Card StormpikesShot = new Card("Stormpikes Shot", 1, CardPersistency.Instant, CardTarget.Character, 5, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.DealDamage, 2 } });	
	public static readonly Card StormpikeCommando = new Card("Stormpike Commando", 5, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Health, 2 }, { ParamType.Attack, 4 }, { ParamType.Speed, 1 } },
				new Dictionary<Effect, Card>() { { Effect.Battlecry, StormpikesShot } });
	public static readonly Card FireElementalsFireball = new Card("Fire Elementals Fireball", 2, CardPersistency.Instant, CardTarget.Character, 5, IsCastOn.Target, new Dictionary<ParamType, int>() {{ParamType.DealDamage, 3} });
	public static readonly Card FireElemental = new Card("Fire Elemental", 6, CardPersistency.Minion,CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Attack, 6 }, { ParamType.Health, 5 }, { ParamType.Speed, 1 } },
		new Dictionary<Effect, Card>() { { Effect.Battlecry, FireElementalsFireball } });
	public static readonly Card BoulderfishOgre = new Card("Boulderfist Ogre", 6, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Attack, 6 }, { ParamType.Health, 7 }, { ParamType.Speed, 1 } });
	public static readonly Card Starfire = new Card("Starfire", 6, CardPersistency.Instant, CardTarget.Character, 5, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.DealDamageSpell, 5 }, { ParamType.HeroDrawsCard, 1 } });
	public static readonly Card Archmage = new Card("Archmage", 6, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Attack, 4 }, { ParamType.Health, 7 }, { ParamType.Speed, 1 }, {ParamType.SpellDamageAdd, 1} });
	public static readonly Card LordOfTheArena = new Card("Lord of the Arena", 6, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Attack, 6 }, { ParamType.Health, 5 }, { ParamType.Speed, 1 }, {ParamType.Taunt, 1}});
	public static readonly Card RecklessRocketeer = new Card("Reckless Rocketeer", 6, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Attack, 5 }, { ParamType.Health, 2 }, { ParamType.Speed, 1 }, { ParamType.Charge, 1 } });
	public static readonly Card StormwindChampionsAura = new Card("Stormwind Champions Aura", 2, CardPersistency.EveryActionRevalidate, CardTarget.Self, 0, IsCastOn.OtherFriendlyMinions, new Dictionary<ParamType, int>() { { ParamType.AttackAdd, 1 }, { ParamType.HealthAdd, 1 } });
	public static readonly Card StormwindChampion = new Card("Stormwind Champion", 7, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Attack, 6 }, { ParamType.Health, 6 }, { ParamType.Speed, 1 }},
		new Dictionary<Effect, Card>() {{Effect.WhileAlive, StormwindChampionsAura} });
	public static readonly Card Flamestrike = new Card("Flamestrike", 7, CardPersistency.Instant, CardTarget.JustThrow, 0, IsCastOn.AllEnemyMinions, new Dictionary<ParamType, int>() { { ParamType.DealDamageSpell, 4 }});
	public static readonly Card Sprint = new Card("Sprint", 7, CardPersistency.Instant, CardTarget.Self, 0, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.HeroDrawsCard, 4 } });
	public static readonly Card CoreHound = new Card("Core Hound", 7, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Attack, 9 }, {ParamType.Health, 5}, {ParamType.Speed, 1} });
	public static readonly Card WarGolem = new Card("War Golem", 7, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Attack, 7 }, { ParamType.Health, 7 }, { ParamType.Speed, 1 } });
	public static readonly Card IronbarkProtector = new Card("Ironbark Protector", 8, CardPersistency.Minion, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Attack, 8 }, { ParamType.Health, 8 }, { ParamType.Speed, 1 }, { ParamType.Taunt, 1 } });
	public static readonly Card MindControl = new Card("Mind Control", 10, CardPersistency.Instant, CardTarget.EnemyMinion, 5, IsCastOn.Target, new Dictionary<ParamType, int>() { {ParamType.TakeControl, 1 } });

	//heroes
	public static readonly Card ShapeshiftsFur = new Card("ShapeshiftsFur", 1, CardPersistency.WhileHolderAlive, CardTarget.Self, 0, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.ArmorAdd, 1 } });
	public static readonly Card Shapeshift = new Card("Shapeshift", 2, CardPersistency.UntilEndTurn, CardTarget.Self, 0, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.AttackAdd, 1 } },
		new Dictionary<Effect, Card>() { {  Effect.Battlecry, ShapeshiftsFur }  } );
	public static readonly Card Druid = new Card("Lasia", 5, CardPersistency.Hero, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Health, 30 }, {ParamType.Speed, 1}}, 
		new Dictionary<Effect,Card>() { { Effect.HerosSpecialPower, Shapeshift} } );
	

	public static readonly Card Priest = new Card("Dementor", 5, CardPersistency.Hero, CardTarget.Empty, 1, IsCastOn.Target, new Dictionary<ParamType, int>() { { ParamType.Health, 30 }, {ParamType.Speed, 1} });

	public string Describe(AvatarModel heroOwner, bool shortV = false) {
		string text = "";
		foreach (KeyValuePair<ParamType, int> kvp in Params) {
			if ((shortV || (kvp.Key != ParamType.Attack && kvp.Key != ParamType.Health)) && (kvp.Key != ParamType.Speed || kvp.Value != 1)) {

				string value = "" + kvp.Value;
				if (kvp.Key == ParamType.DealDamageSpell && heroOwner.SpellDamageAdd() > 0) {
					value = "*" + (heroOwner.SpellDamageAdd() + kvp.Value) + "*";
				}
				text += kvp.Key + ": " + value + ", ";
			}
		}
		if (!shortV) {
			text += "\n";
		}
		if (CardTarget != CardTarget.JustThrow && CardTarget != CardTarget.Self && CardTarget != CardTarget.Empty) {
			text += "Cast on: " + CardTarget + ",\n";
		}
		if (IsCastOn != IsCastOn.Target && CardTarget != CardTarget.Empty) {
			text += "Affects: " + IsCastOn + ",\n";
		}

		foreach (KeyValuePair<Effect, Card> kvp in Effects) {
			if (kvp.Key != Effect.WhileAlive) {
				text += kvp.Key + ": ";
			}
			text += kvp.Value.Describe(heroOwner, true) + ", ";
		}
		if (!shortV) {
			text += "\n";
		}


		if (CardPersistency != CardPersistency.EveryActionRevalidate && CardPersistency != CardPersistency.WhileHolderAlive && CardPersistency != CardPersistency.Instant) {
			text += CardPersistency + ",";
		}
		return text;
	}
}


public enum CastedCardParamType {
	AttackAdd,
	HealthAdd, //this includes max health increase
	Taunt,

	//instants -> will be removed after dealing instants
	DealDamage,
	Heal,
	HeroDrawsCard,
	SpellDamageAdd,
	DealDamageSpell,
	ManaCrystalAdd,
	ArmorAdd,
	ManaCrystalEmptyAdd,
	HealFull,
}

public class CastedCard {

	public Dictionary<CastedCardParamType, int> Params = new Dictionary<CastedCardParamType, int>();
	public Card Card;
	public bool MarkedToRemove;

	public CastedCard(AvatarModel castingBy, AvatarModel castingOn, Card c) {
		Card = c;

		if (c.Params.ContainsKey(ParamType.DealDamage)) {
			Params.Add(CastedCardParamType.DealDamage, c.Params[ParamType.DealDamage]);
		}

		if (c.Params.ContainsKey(ParamType.Heal)) {
			Params.Add(CastedCardParamType.Heal, c.Params[ParamType.Heal]);
		}

		if (c.Params.ContainsKey(ParamType.HeroDrawsCard)) {
			Params.Add(CastedCardParamType.HeroDrawsCard, c.Params[ParamType.HeroDrawsCard]);
		}

		if (c.Params.ContainsKey(ParamType.AttackAdd)) {
			Params.Add(CastedCardParamType.AttackAdd, c.Params[ParamType.AttackAdd]);
		}
		
		if (c.Params.ContainsKey(ParamType.HealthAdd)) {
			Params.Add(CastedCardParamType.HealthAdd, c.Params[ParamType.HealthAdd]);
		}
		if (c.Params.ContainsKey(ParamType.HealthAddOfOtherFriendlyMinionNumber)) {
			Params.Add(CastedCardParamType.HealthAdd, AvatarModel.MyMinionsNumber(castingOn.GetMyHero())- 1);//because itself doesn't count and its already casted
		}
		
		if (c.Params.ContainsKey(ParamType.AttackAddOfOtherFriendlyMinionNumber)) {
			Params.Add(CastedCardParamType.AttackAdd, AvatarModel.MyMinionsNumber(castingOn.GetMyHero()) - 1); //because itself doesn't count and its already casted
		}

		if (c.Params.ContainsKey(ParamType.Taunt)) {
			Params.Add(CastedCardParamType.Taunt, c.Params[ParamType.Taunt]);
		}

		if (c.Params.ContainsKey(ParamType.SpellDamageAdd)) {
			Params.Add(CastedCardParamType.SpellDamageAdd, c.Params[ParamType.SpellDamageAdd]);
		}

		if (c.Params.ContainsKey(ParamType.DealDamageSpell)) {
			Params.Add(CastedCardParamType.DealDamageSpell, c.Params[ParamType.DealDamageSpell] + castingBy.SpellDamageAdd());
		}

		if (c.Params.ContainsKey(ParamType.HealFull)) {
			Params.Add(CastedCardParamType.HealFull, 1);
		}

		if (c.Params.ContainsKey(ParamType.ManaCrystalAdd)) {
			Params.Add(CastedCardParamType.ManaCrystalAdd, c.Params[ParamType.ManaCrystalAdd]);
		}

		if (c.Params.ContainsKey(ParamType.ManaCrystalEmptyAdd)) {
			Params.Add(CastedCardParamType.ManaCrystalEmptyAdd, c.Params[ParamType.ManaCrystalEmptyAdd]);
		}

		if (c.Params.ContainsKey(ParamType.ArmorAdd)) {
			Params.Add(CastedCardParamType.ArmorAdd, c.Params[ParamType.ArmorAdd]);
		}
		if (c.Params.ContainsKey(ParamType.TakeControl)) {
			castingOn.GetMyHero().Minions.Remove(castingOn);
			castingOn.Creator = castingBy.GetMyHero();
			castingBy.Minions.Add(castingOn);
			castingOn.MovesLeft = 0;
		}
		if (c.Params.ContainsKey(ParamType.Charge)) {
			castingOn.RefillMovements();
		}
	}

	internal void DealInstants(AvatarModel castingBy, AvatarModel castingOn) {

		foreach (KeyValuePair<CastedCardParamType, int> kvp in Params.ToArray()) {

			string on = castingOn != null ? castingOn.Card.Name : "empty";
			//Debug.Log("Dealing instant: " + kvp.Key + " on " + on);
			PanelMinigame.Me.AnimationRequests.Add(new AnimationRequestStruct(castingOn, AnimationRequest.DealInstantOn, kvp));
			switch (kvp.Key) {
					
				case CastedCardParamType.DealDamage:
					castingOn.ActualHealth -= kvp.Value;
					Params.Remove(kvp.Key);
					break;
				case CastedCardParamType.HealthAdd:
					castingOn.ActualHealth += kvp.Value;
					break;
				case CastedCardParamType.Heal:
					castingOn.ActualHealth += kvp.Value;
					Params.Remove(kvp.Key);
					break;
				case CastedCardParamType.HealFull:
					castingOn.ActualHealth = castingOn.MaxHealth;
					Params.Remove(kvp.Key);
					break;
				case CastedCardParamType.HeroDrawsCard:
					for (int i = 0; i < kvp.Value; i++) {
						castingBy.GetMyHero().PullCardFromDeck();
					}
					Params.Remove(kvp.Key);
					break;
				case CastedCardParamType.DealDamageSpell:
					castingOn.ActualHealth -= kvp.Value;
					Params.Remove(kvp.Key);
					break;
				case CastedCardParamType.ArmorAdd:
					castingOn.Armor += kvp.Value;
					Params.Remove(kvp.Key);
					break;
			}
		}
	}
}






