using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Items.Armors;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using System.Collections.Generic;
using RES = EldritchArcana.Properties.Resources;

namespace EldritchArcana
{
    internal class CombatTraits
    {
        public static BlueprintFeatureSelection CreateCombatTraits()
        {
            var noFeature = Helpers.PrerequisiteNoFeature(null);
            var combatTraits = Helpers.CreateFeatureSelection("CombatTrait", RES.CombatTraitName_info,
                RES.CombatTraitDescription_info,
                "fab4225be98a4b3e9717883f22086c82", null, FeatureGroup.None, noFeature);
            noFeature.Feature = combatTraits;

            var choices = new List<BlueprintFeature>();
            choices.Add(Helpers.CreateFeature("AnatomistTrait", RES.AnatomistTraitName_info,
                RES.AnatomistTraitDescription_info,
                "69245ef4b4ba44ddac917fc2aa10fbad",
                Image2Sprite.Create("Mods/EldritchArcana/sprites/anatomist.png"), // Improved Critical
                FeatureGroup.None,
                Helpers.Create<CriticalConfirmationBonus>(a => { a.Bonus = 1; a.Value = 0; })));

            choices.Add(Helpers.CreateFeature("ArmorExpertTrait", RES.ArmorExpertTraitName_info,
                RES.ArmorExpertTraitDescription_info,
                "94d526372a964b6db97c64291a3cb846",
                Helpers.GetIcon("3bc6e1d2b44b5bb4d92e6ba59577cf62"), // Armor Focus (light)
                FeatureGroup.None,
                Helpers.Create<ArmorCheckPenaltyIncrease>(a => a.Bonus = -1)));

            var rageResource = Traits.library.Get<BlueprintAbilityResource>("24353fcf8096ea54684a72bf58dedbc9");
            choices.Add(Helpers.CreateFeature("BerserkerOfTheSocietyTrait", RES.BerserkerOfTheSocietyTraitName_info,
                RES.BerserkerOfTheSocietyTraitDescription_info,
                "8acfcecfed05442594eed93fe448ab3d",
                Helpers.GetIcon("1a54bbbafab728348a015cf9ffcf50a7"), // Extra Rage
                FeatureGroup.None,
                rageResource.CreateIncreaseResourceAmount(3)));

            choices.Add(Helpers.CreateFeature("BladeOfTheSocietyTrait", RES.BladeOfTheSocietyTraitName_info,
                RES.BladeOfTheSocietyTraitDescription_info,
                "ff8c90626a58436997cc41e4b121be9a",
                Helpers.GetIcon("9f0187869dc23744292c0e5bb364464e"), // Accomplished Sneak Attacker
                FeatureGroup.None,
                Helpers.Create<AdditionalDamageOnSneakAttack>(a => a.Value = 1),
                Helpers.CreateAddStatBonusOnLevel(StatType.SneakAttack, 1, ModifierDescriptor.Trait, 3)
                ));

            choices.Add(Helpers.CreateFeature("DefenderOfTheSocietyTrait", RES.DefenderOfTheSocietyTraitName_info,
                RES.DefenderOfTheSocietyTraitDescription_info,
                "545bf7e13346473caf48f179083df894",
                Helpers.GetIcon("7dc004879037638489b64d5016997d12"), // Armor Focus Medium
                FeatureGroup.None,
                Helpers.Create<ArmorFocus>(a => a.ArmorCategory = ArmorProficiencyGroup.Medium),
                Helpers.Create<ArmorFocus>(a => a.ArmorCategory = ArmorProficiencyGroup.Heavy)));

            choices.Add(Helpers.CreateFeature("DeftDodgerTrait", RES.DeftDodgerTraitName_info,
                RES.DeftDodgerTraitDescription_info,
                "7b57d86503314d32b753f77909c909bc",
                Helpers.GetIcon("15e7da6645a7f3d41bdad7c8c4b9de1e"), // Lightning Reflexes
                FeatureGroup.None,
                Helpers.CreateAddStatBonus(StatType.SaveReflex, 1, ModifierDescriptor.Trait)));

            choices.Add(Helpers.CreateFeature("DirtyFighterTrait", RES.DirtyFighterTraitName_info,
                RES.DirtyFighterTraitDescription_info,
                "ac47c14063574a0a9ea6927bf637a02a",
                Helpers.GetIcon("5662d1b793db90c4b9ba68037fd2a768"), // precise strike
                FeatureGroup.None,
                DamageBonusAgainstFlankedTarget.Create(1)));

            var kiPowerResource = Traits.library.Get<BlueprintAbilityResource>("9d9c90a9a1f52d04799294bf91c80a82");
            choices.Add(Helpers.CreateFeature("HonoredFistOfTheSocietyTrait", RES.HonoredFistOfTheSocietyTraitName_info,
                RES.HonoredFistOfTheSocietyTraitDescription_info,
                "ee9c230cbbc2484084af61ac97e47e72",
                Helpers.GetIcon("7dc004879037638489b64d5016997d12"), // Armor Focus Medium
                FeatureGroup.None,
                kiPowerResource.CreateIncreaseResourceAmount(1)));

            // TODO: Killer

            choices.Add(Helpers.CreateFeature("ReactionaryTrait", RES.ReactionaryTraitName_info,
                RES.ReactionaryTraitDescription_info,
                "fa2c636580ee431297de8806a046044a",
                Helpers.GetIcon("797f25d709f559546b29e7bcb181cc74"), // Improved Initiative
                FeatureGroup.None,
                Helpers.CreateAddStatBonus(StatType.Initiative, 2, ModifierDescriptor.Trait)));

            choices.Add(Traits.CreateAddStatBonus("RecklessTrait", RES.RecklessTraitName_info,
                RES.RecklessTraitDescription_info,
                "edb2f4d0c2c34c7baccad11f2b5bfbd4",
                StatType.SkillMobility));

            choices.Add(Helpers.CreateFeature("ResilientTrait", RES.ResilientTraitName_info,
                RES.ResilientTraitDescription_info,
                "789d02217b6542ce8b0302249c86d49d",
                Helpers.GetIcon("79042cb55f030614ea29956177977c52"), // Great Fortitude
                FeatureGroup.None,
                Helpers.CreateAddStatBonus(StatType.SaveFortitude, 1, ModifierDescriptor.Trait)));

            choices.Add(Traits.CreateAddStatBonus("WittyReparteeTrait", RES.WittyReparteeTraitName_info,
                RES.WittyReparteeTraitDescription_info,
                "c6dbc457c5de40dbb4cb9fe4d7706cd9",
                StatType.SkillPersuasion));

            choices.Add(UndoSelection.Feature.Value);
            combatTraits.SetFeatures(choices);
            return combatTraits;
        }
    }
}