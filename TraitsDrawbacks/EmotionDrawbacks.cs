using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Items;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using RES = EldritchArcana.Properties.Resources;

namespace EldritchArcana
{
    internal class EmotionDrawbacks
    {
        public static BlueprintFeatureSelection CreateEmotionDrawbacks()
        {
            //string[]  = new string[] { };
            string[] EmotionGuids = new string[200];
            //EmotionGuids = guids;
            string baseguid = "CB54279F30DA4802833F";
            int x = 0;
            for (long i = 542922691494; i < 542922691644; i++)
            {
                EmotionGuids[x] = baseguid + i.ToString();
                x++;
            }
            //int rnd = DateTime.Now.Millisecond%4;

            var noFeature = Helpers.PrerequisiteNoFeature(null);
            var EmotionDrawbacks = Helpers.CreateFeatureSelection("EmotionDrawback", RES.EmotionDrawbackName_info,
                RES.EmotionDrawbackDescription_info,
                EmotionGuids[0], null, FeatureGroup.None, noFeature);

            noFeature.Feature = EmotionDrawbacks;

            var choices = new List<BlueprintFeature>
            {
                Helpers.CreateFeature("AnxiousDrawback", RES.AnxiousDrawbackName_info,
                RES.AnxiousDrawbackDescription_info,
                EmotionGuids[1],
                Helpers.NiceIcons(16), // great fortitude
                FeatureGroup.None,
                Helpers.CreateAddStatBonus(StatType.SkillPersuasion, -2, ModifierDescriptor.Penalty)),

                //var tieflingHeritageDemodand = library.Get<BlueprintFeature>("a53d760a364cd90429e16aa1e7048d0a");
                Helpers.CreateFeature("AttachedDrawback", RES.AttachedDrawbackName_info,
                RES.AttachedDrawbackDescription_info,
                EmotionGuids[2],
                Helpers.GetIcon("2483a523984f44944a7cf157b21bf79c"), // Elven Immunities
                FeatureGroup.None,
                Helpers.CreateAddStatBonus(StatType.SaveWill, -1, ModifierDescriptor.Penalty),
                Helpers.Create<SavingThrowBonusAgainstDescriptor>(s => { s.SpellDescriptor = SpellDescriptor.Fear; s.Value = -2; s.ModifierDescriptor = ModifierDescriptor.Penalty; })),


                Helpers.CreateFeature("BetrayedDrawback", RES.BetrayedDrawbackName_info,
                RES.BetrayedDrawbackDescription_info,
                EmotionGuids[3],
                Helpers.NiceIcons(2), // Accomplished Sneak Attacker
                FeatureGroup.None,
                Helpers.CreateAddStatBonus(StatType.CheckDiplomacy, -3, ModifierDescriptor.Penalty)),


                Helpers.CreateFeature("BitterDrawback", RES.BitterDrawbackName_info,
                RES.BitterDrawbackDescription_info,
                EmotionGuids[4],
                Helpers.NiceIcons(5), // great fortitude
                FeatureGroup.None,
                Helpers.Create<FeyFoundlingLogic>(s => { s.dieModefier = 0; s.flatModefier = -1; })),

                Helpers.CreateFeature("CondescendingDrawback", RES.CondescendingDrawbackName_info,
                RES.CondescendingDrawbackDescription_info,
                EmotionGuids[5],
                Helpers.NiceIcons(10), // enchantment
                FeatureGroup.None,
                Helpers.CreateAddStatBonus(StatType.CheckDiplomacy, -5, ModifierDescriptor.Penalty),
                Helpers.CreateAddStatBonus(StatType.CheckIntimidate, -5, ModifierDescriptor.Penalty)),

                //Effect Your base speed when frightened and fleeing increases by 5 feet, and the penalties you take from having the cowering, frightened, panicked, or shaken conditions increase by 1.If you would normally be immune to fear, you do not take these penalties but instead lose your immunity to fear(regardless of its source).
                Helpers.CreateFeature("CowardlyDrawback", RES.CowardlyDrawbackName_info,
                RES.CowardlyDrawbackDescription_info,
                EmotionGuids[6],
                Helpers.NiceIcons(6), //invisiblilty
                FeatureGroup.None,
                Helpers.CreateAddStatBonus(StatType.SaveFortitude, -2, ModifierDescriptor.Penalty),
                Helpers.CreateAddStatBonus(StatType.Speed, 5, ModifierDescriptor.FearPenalty),
                Helpers.Create<SavingThrowBonusAgainstDescriptor>(s => { s.SpellDescriptor = SpellDescriptor.Fear; s.Value = -4; s.ModifierDescriptor = ModifierDescriptor.Penalty; })),

                Helpers.CreateFeature("CrueltyDrawback", RES.CrueltyDrawbackName_info,
                RES.CrueltyDrawbackDescription_info,
                EmotionGuids[7],
                Helpers.NiceIcons(9), // breakbone
                FeatureGroup.None,
                Helpers.CreateAddStatBonus(StatType.BaseAttackBonus, -2, ModifierDescriptor.Penalty),
                DamageBonusAgainstFlankedTarget.Create(4)),

                Helpers.CreateFeature("EmptyMaskDrawback", RES.EmptyMaskDrawbackName_info,
                RES.EmptyMaskDrawbackDescription_info,
                EmotionGuids[8],
                Helpers.NiceIcons(14), // mask
                FeatureGroup.None,
                //Helpers.CreateAddStatBonus(StatType.SaveWill, -1, ModifierDescriptor.Penalty),
                Helpers.Create<SavingThrowBonusAgainstDescriptor>(s => { s.SpellDescriptor = SpellDescriptor.Compulsion; s.Value = -2; s.ModifierDescriptor = ModifierDescriptor.Penalty; }))
            };

            var debuff = Helpers.CreateBuff("EnvyDeBuff", RES.EnvyDrawbackName_info,
                RES.EnvyDebuffDescription_info,
                EmotionGuids[9],
                Helpers.NiceIcons(1), null,
                Helpers.Create<ConcentrationBonus>(a => { a.Value = -2; a.CheckFact = true; }));

            choices.Add(Helpers.CreateFeature("EnvyDrawback", RES.EnvyDrawbackName_info,
                RES.EnvyDrawbackDescription_info,
                EmotionGuids[10],
                Helpers.NiceIcons(1), //grab
                FeatureGroup.None,
                CovetousCurseLogic.Create(debuff)));//
            //
            //int rnd = DateTime.Now.Millisecond % 64;
            var Fraud = Helpers.CreateFeatureSelection("GuiltyFraudDrawback", RES.GuiltyFraudDrawbackName_info,
                RES.GuiltyFraudDrawbackDescription_info,
                EmotionGuids[11],
                Helpers.NiceIcons(999), // great fortitude
                FeatureGroup.None,
                //WeaponCategory.LightRepeatingCrossbow                
                Helpers.CreateAddStatBonus(StatType.SkillPersuasion, -2, ModifierDescriptor.Penalty));

            //var weap = WeaponCategory.Dart;
            var hoi = new List<BlueprintFeature>() { };
            x = 11;//x is just a cheat value we use to ged guids
            //foreach (WeaponCategory weap in (WeaponCategory[])Enum.GetValues(typeof(WeaponCategory)))
            var Onehandedweapons = new Dictionary<WeaponCategory, String> {
                { WeaponCategory.Club, RES.WeaponCategoryClubName_info },
                { WeaponCategory.Dagger, RES.WeaponCategoryDaggerName_info },
                { WeaponCategory.Dart,RES.WeaponCategoryDartName_info },
                { WeaponCategory.DuelingSword, RES.WeaponCategoryDuelingSwordName_info },
                { WeaponCategory.ElvenCurvedBlade, RES.WeaponCategoryElvenCurveBladeName_info },
                { WeaponCategory.Falcata, RES.WeaponCategoryFalcataName_info },
                { WeaponCategory.Flail, RES.WeaponCategoryFlailName_info },
                { WeaponCategory.Handaxe, RES.WeaponCategoryHandaxeName_info },
                { WeaponCategory.HeavyMace, RES.WeaponCategoryHeavyMaceName_info },
                { WeaponCategory.Javelin, RES.WeaponCategoryJavelinName_info},
                { WeaponCategory.LightMace, RES.WeaponCategoryLightMaceName_info },
                { WeaponCategory.Shuriken, RES.WeaponCategoryShurikenName_info },
                { WeaponCategory.Sickle, RES.WeaponCategorySickleName_info },
                { WeaponCategory.Sling, RES.WeaponCategorySlingName_info },
                { WeaponCategory.Kama, RES.WeaponCategoryKamaName_info },
                { WeaponCategory.Kukri, RES.WeaponCategoryKukriName_info },
                { WeaponCategory.Starknife, RES.WeaponCategoryStarknifeName_info },
                { WeaponCategory.ThrowingAxe, RES.WeaponCategoryThrowingAxeName_info },
                { WeaponCategory.LightPick, RES.WeaponCategoryLightPickName_info },
                { WeaponCategory.DwarvenWaraxe, RES.WeaponCategoryDwarvenWaraxeName_info },
                { WeaponCategory.Trident, RES.WeaponCategoryTridentName_info },
                { WeaponCategory.BastardSword, RES.WeaponCategoryBastardSwordName_info },
                { WeaponCategory.Battleaxe, RES.WeaponCategoryBattleaxeName_info },
                { WeaponCategory.Longsword, RES.WeaponCategoryLongswordName_info },
                { WeaponCategory.Nunchaku, RES.WeaponCategoryNunchakuName_info },
                { WeaponCategory.Rapier, RES.WeaponCategoryRapierName_info },
                { WeaponCategory.Estoc, RES.WeaponCategoryEstocName_info },
                { WeaponCategory.Sai, RES.WeaponCategorySaiName_info },
                { WeaponCategory.Scimitar, RES.WeaponCategoryScimitarName_info },
                { WeaponCategory.Shortsword, RES.WeaponCategoryShortswordName_info },
                { WeaponCategory.Warhammer, RES.WeaponCategoryWarhammerName_info },
                { WeaponCategory.LightHammer, RES.WeaponCategoryLightHammerName_info },
                { WeaponCategory.WeaponLightShield, RES.WeaponCategoryLightShieldName_info },
                { WeaponCategory.WeaponHeavyShield, RES.WeaponCategoryHeavyShieldName_info },
            };

            foreach (KeyValuePair<WeaponCategory, String> weap in Onehandedweapons)
            {

                x++;
                hoi.Add(Helpers.CreateFeature(
                $"Greedy{weap.Key}Drawback",
                string.Format(RES.GuiltyFraudDrawbackWeaponFeat_info, weap.Value),
                string.Format(RES.GuiltyFraudDrawbackWeaponDescription_info, weap.Value), 
                EmotionGuids[x],
                Helpers.NiceIcons(999), FeatureGroup.None,
                Helpers.Create<AddStartingEquipment>(a =>
                {

                    a.CategoryItems = new WeaponCategory[] { weap.Key, weap.Key };
                    a.RestrictedByClass = Array.Empty<BlueprintCharacterClass>();
                    a.BasicItems = Array.Empty<BlueprintItem>();
                })));

                //Log.Write(x.ToString());
            }
            //Log.Write(x.ToString());

            x++;
            choices.Add(Helpers.CreateFeature("HauntedDrawback", RES.HauntedDrawbackName_info,
                RES.HauntedDrawbackDescription_info,
                EmotionGuids[x],
                Helpers.NiceIcons(39), // fatigue
                FeatureGroup.None,
                //Helpers.CreateAddStatBonus(StatType.SaveWill, -1, ModifierDescriptor.Penalty),
                Helpers.Create<SavingThrowBonusAgainstDescriptor>(s => { s.SpellDescriptor = SpellDescriptor.Evil; s.Value = -2; s.ModifierDescriptor = ModifierDescriptor.Penalty; })));

            x++;
            choices.Add(Helpers.CreateFeature("HauntedRegretDrawback", RES.HauntedRegretDrawbackName_info,
                RES.HauntedRegretDrawbackDescription_info,
                EmotionGuids[x],
                Helpers.NiceIcons(7),//fatigue//
                FeatureGroup.None,
                Helpers.Create<ConcentrationBonus>(a => a.Value = -2),
                Helpers.Create<SavingThrowBonusAgainstDescriptor>(s => { s.SpellDescriptor = SpellDescriptor.MindAffecting; s.Value = -2; s.ModifierDescriptor = ModifierDescriptor.Penalty; })));

            x++;
            choices.Add(Helpers.CreateFeature("ImpatientDrawback", RES.ImpatientDrawbackName_info,
                RES.ImpatientDrawbackDescription_info,
                EmotionGuids[x],
                Helpers.NiceIcons(33), //rush
                FeatureGroup.None,
                Helpers.CreateAddStatBonus(StatType.BaseAttackBonus, -1, ModifierDescriptor.Penalty),
                Helpers.CreateAddStatBonus(StatType.Initiative, 1, ModifierDescriptor.Insight),
                Helpers.Create<SavingThrowBonusAgainstDescriptor>(s => { s.SpellDescriptor = SpellDescriptor.Evil; s.Value = -2; s.ModifierDescriptor = ModifierDescriptor.Penalty; })));

            x++;
            choices.Add(Helpers.CreateFeature("DaydreamerDrawback", RES.DaydreamerDrawbackName_info,
                RES.DaydreamerDrawbackDescription_info,
                EmotionGuids[x],
                Helpers.NiceIcons(6), //rush
                FeatureGroup.None,
                Helpers.CreateAddStatBonus(StatType.Initiative, -1, ModifierDescriptor.Penalty),
                Helpers.CreateAddStatBonus(StatType.SkillPerception, -2, ModifierDescriptor.Penalty)));

            x++;
            choices.Add(Helpers.CreateFeature("ShadowDrawback", RES.ShadowDrawbackName_info,
                 RES.ShadowDrawbackDescription_info,
                 EmotionGuids[x],
                 Helpers.NiceIcons(6), //rush
                 FeatureGroup.None,
                 Helpers.Create<ShadowSensitivity>()));
            x++;
            var debuff2 = Helpers.CreateBuff("ShadowDeBuff", RES.ShadowDrawbackName_info,
                RES.ShadowDeBuffDescription_info,
                EmotionGuids[x],
                Helpers.NiceIcons(22), null);
            var components = new List<BlueprintComponent> { };
            components.AddRange((new SpellSchool[]
            {
                SpellSchool.Abjuration,
                SpellSchool.Conjuration,
                SpellSchool.Divination,
                SpellSchool.Enchantment,
                SpellSchool.Evocation,
                SpellSchool.Illusion,
                SpellSchool.Necromancy,
                SpellSchool.Transmutation,
                SpellSchool.Universalist
            }).Select((school) => Helpers.Create<SavingThrowBonusAgainstSchool>(a =>
            {
                a.School = school;
                a.Value = -1;
                a.ModifierDescriptor = ModifierDescriptor.Penalty;
            })));
            debuff2.AddComponents(components);
            ShadowSensitivity.ShadowBuff = debuff2;

            x++;
            choices.Add(Helpers.CreateFeature("SleepyDrawback", RES.SleepyDrawbackName_info,
                 RES.SleepyDrawbackDescription_info,
                 EmotionGuids[x],
                 Helpers.NiceIcons(7),
                 FeatureGroup.None,
                 Helpers.Create<SavingThrowBonusAgainstDescriptor>(s => { s.Bonus = -2; s.SpellDescriptor = SpellDescriptor.Sleep; })));

            x++;
            choices.Add(Helpers.CreateFeature("ZealousDrawback", RES.ZealousDrawbackName_info,
                 RES.ZealousDrawbackDescription_info,
                 EmotionGuids[x],
                 Helpers.NiceIcons(48),
                 FeatureGroup.None,
                 Helpers.CreateAddStatBonus(StatType.AdditionalAttackBonus, -5, ModifierDescriptor.Sacred),
                 Helpers.CreateAddStatBonus(StatType.AdditionalDamage, 2, ModifierDescriptor.Trait)
                 ));

            Fraud.SetFeatures(hoi);
            choices.Add(Fraud);

            choices.Add(UndoSelection.Feature.Value);
            EmotionDrawbacks.SetFeatures(choices);
            return EmotionDrawbacks;
        }
    }
}