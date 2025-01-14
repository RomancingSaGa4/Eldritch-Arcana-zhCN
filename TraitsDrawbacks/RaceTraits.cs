﻿using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UI.Common;
using Kingmaker.UnitLogic.Abilities;
using Kingmaker.UnitLogic.FactLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using RES = EldritchArcana.Properties.Resources;

namespace EldritchArcana
{
    internal class RaceTraits
    {

        public static BlueprintFeatureSelection CreateRaceTraits(BlueprintFeatureSelection adopted)
        {
            var noFeature = Helpers.PrerequisiteNoFeature(null);
            var raceTraits = Helpers.CreateFeatureSelection("RaceTrait", RES.RaceTraitName_info,
                RES.RaceTraitDescription_info,
                "6264aa9515be40cda55892da93685764", null, FeatureGroup.None,
                Helpers.PrerequisiteNoFeature(adopted), noFeature);
            noFeature.Feature = raceTraits;

            var humanReq = Helpers.PrerequisiteFeaturesFromList(Helpers.human, Helpers.halfElf, Helpers.halfOrc,
                // Note: Aasimar/Tiefling included under the assumption they have "Scion of Humanity"/"Pass for Human"
                Helpers.aasimar, Helpers.tiefling);

            var halfElfReq = Helpers.PrerequisiteFeature(Helpers.halfElf);
            var halfOrcReq = Helpers.PrerequisiteFeature(Helpers.halfOrc);
            var elfReq = Helpers.PrerequisiteFeaturesFromList(Helpers.elf, Helpers.halfElf);
            var dwarfReq = Helpers.PrerequisiteFeature(Helpers.dwarf);
            var halflingReq = Helpers.PrerequisiteFeature(Helpers.halfling);
            var gnomeReq = Helpers.PrerequisiteFeature(Helpers.gnome);
            var aasimarReq = Helpers.PrerequisiteFeature(Helpers.aasimar);
            var tieflingReq = Helpers.PrerequisiteFeature(Helpers.tiefling);

            // TODO: how do we code prerequisites so they aren't ignored by "Adopted"?
            // (only race prereq should be ignored, not others)
            //
            // Note: half-elf, half-orc can take traits from either race.
            // Also Aasimar/Tiefling are treated as having Scion of Humanity/Pass for Human in the game.
            var choices = new List<BlueprintFeature>();

            // Human:
            // - Carefully Hidden (+1 will save, +2 vs divination)
            // - Fanatic (Arcana)
            // - Historian (World and +1 bardic knowledge if Bard)
            // - Shield Bearer (+1 dmg shield bash)
            // - Superstitious (+1 save arcane spells)
            // - World Traveler (choose: persuasion, perception, or world)

            var components = new List<BlueprintComponent> { humanReq };
            components.Add(Helpers.CreateAddStatBonus(StatType.SaveWill, 1, ModifierDescriptor.Trait));
            components.Add(Helpers.Create<SavingThrowBonusAgainstSchool>(a =>
            {
                a.School = SpellSchool.Divination;
                a.Value = 2;
                a.ModifierDescriptor = ModifierDescriptor.Trait;
            }));
            choices.Add(Helpers.CreateFeature("CarefullyHiddenTrait", RES.RaceCarefullyHiddenTraitName_info,
                RES.RaceCarefullyHiddenTraitDescription_info,
                "38b92d2ebb4c4cdb8e946e29f5b2f178",
                Helpers.GetIcon("175d1577bb6c9a04baf88eec99c66334"), // Iron Will
                FeatureGroup.None,
                components.ToArray()));

            choices.Add(Traits.CreateAddStatBonus("FanaticTrait", RES.RaceFanaticTraitName_info,
                RES.RaceFanaticTraitDescription_info,
                "6427e81ba399406c93b463c284a42055",
                StatType.SkillKnowledgeArcana,
                humanReq));

            var bardicKnowledge = Traits.library.Get<BlueprintFeature>("65cff8410a336654486c98fd3bacd8c5");
            components.Clear();
            components.Add(humanReq);
            components.AddRange((new StatType[] {
                StatType.SkillKnowledgeArcana,
                StatType.SkillKnowledgeWorld,
                StatType.SkillLoreNature,
                StatType.SkillLoreReligion,
            }).Select((skill) => Helpers.Create<AddStatBonusIfHasFact>(a =>
            {
                a.Stat = skill;
                a.Value = 1;
                a.CheckedFact = bardicKnowledge;
                a.Descriptor = ModifierDescriptor.UntypedStackable;
            })));

            var historian = Traits.CreateAddStatBonus("HistorianTrait", RES.RaceHistorianTraitName_info,
                RES.RaceHistorianTraitDescription_info,
                "4af3871899e4440bae03d4c33d4b52fd",
                StatType.SkillKnowledgeWorld,
                components.ToArray());
            choices.Add(historian);

            components.Clear();
            components.Add(humanReq);
            components.AddRange(new String[] {
                "98a0dc03586a6d04791901c41700e516", // SpikedLightShield
                "1fd965e522502fe479fdd423cca07684", // WeaponLightShield
                "a1b85d048fb5003438f34356df938a9f", // SpikedHeavyShield
                "be9b6408e6101cb4997a8996484baf19"  // WeaponHeavyShield
            }.Select(id => Helpers.Create<WeaponTypeDamageBonus>(w => { w.DamageBonus = 1; w.WeaponType = Traits.library.Get<BlueprintWeaponType>(id); })));

            choices.Add(Helpers.CreateFeature("ShieldBearerTrait", RES.RaceShieldBearerTraitName_info,
                RES.RaceShieldBearerTraitDescription_info,
                "044ebbbadfba4d58afa11bfbf38df199",
                Helpers.GetIcon("121811173a614534e8720d7550aae253"), // Shield Bash
                FeatureGroup.None,
                components.ToArray()));

            choices.Add(Helpers.CreateFeature("SuperstitiousTrait", RES.RaceSuperstitiousTraitName_info,
                RES.RaceSuperstitiousTraitDescription_info,
                "f5d79e5fbb87473ca0b13ed15b742079",
                Helpers.GetIcon("2483a523984f44944a7cf157b21bf79c"), // Elven Immunities
                FeatureGroup.None,
                humanReq,
                Helpers.Create<SavingThrowBonusAgainstSpellSource>()));

            //var travelerDescription = "Your family has taken the love of travel to an extreme, roaming the world extensively. You've seen dozens of cultures and have learned to appreciate the diversity of what the world has to offer.";
            var worldTraveler = Helpers.CreateFeatureSelection("WorldTravelerTrait", RES.RaceWorldTravelerTraitName_info,
                RES.RaceWorldTravelerTraitDescription_info + RES.RaceWorldTravelerTraitBenefit_info,
                "ecacfcbeddfe453cafc8d60fc1db7d34",
                Helpers.GetIcon("3adf9274a210b164cb68f472dc1e4544"), // Human Skilled
                FeatureGroup.None,
                humanReq);

            var travelerFeats = new StatType[] {
                StatType.SkillPersuasion,
                StatType.SkillKnowledgeWorld,
                StatType.SkillPerception
            }.Select(skill => Traits.CreateAddStatBonus(
                $"WorldTraveler{skill}Trait",
                String.Format(RES.TypeHyphenSubtype_info,
                    RES.RaceWorldTravelerTraitName_info.Substring(0, RES.RaceWorldTravelerTraitName_info.IndexOf("(") >= 0 ?
                        RES.RaceWorldTravelerTraitName_info.IndexOf("(") : RES.RaceWorldTravelerTraitName_info.IndexOf("（")),
                    UIUtility.GetStatText(skill)),
                RES.RaceWorldTravelerTraitDescription_info,
                Helpers.MergeIds(Helpers.GetSkillFocus(skill).AssetGuid, "9b03b7ff17394007a3fbec18aa42604b"),
                skill)).ToArray();
            worldTraveler.SetFeatures(travelerFeats);
            choices.Add(worldTraveler);

            // Elf:
            // - Dilettante Artist (persuasion)
            // - Forlorn (+1 fort save)
            // - Warrior of the Old (+2 init)
            // - Youthful Mischief (+1 ref)
            choices.Add(Traits.CreateAddStatBonus("DilettanteArtistTrait", RES.DilettanteArtistTraitName_info,
                RES.DilettanteArtistTraitDescription_info,
                "ac5a16e72ef74b4884c674dcbb61692c",
                StatType.SkillPersuasion, elfReq));

            choices.Add(Helpers.CreateFeature("ForlornTrait", RES.ForlornTraitName_info,
                RES.ForlornTraitDescription_info,
                "1511289c92ea4233b14c4f51072ea10f",
                Helpers.GetIcon("79042cb55f030614ea29956177977c52"), // Great Fortitude
                FeatureGroup.None,
                elfReq,
                Helpers.CreateAddStatBonus(StatType.SaveFortitude, 1, ModifierDescriptor.Trait)
                ));

            choices.Add(Helpers.CreateFeature("WarriorOfOldTrait", RES.WarriorOfOldTraitName_info,
                RES.WarriorOfOldTraitDescription_info,
                "dc36a2c52abb4e6dbff549ac65a5a171",
                Helpers.GetIcon("797f25d709f559546b29e7bcb181cc74"), // Improved Initiative
                FeatureGroup.None,
                elfReq,
                Helpers.CreateAddStatBonus(StatType.Initiative, 2, ModifierDescriptor.Trait)));

            choices.Add(Helpers.CreateFeature("YouthfulMischiefTrait", RES.YouthfulMischiefTraitName_info,
                RES.YouthfulMischiefTraitDescription_info,
                "bfcc574d1f214455ac369fa46e07200e",
                Helpers.GetIcon("15e7da6645a7f3d41bdad7c8c4b9de1e"), // Lightning Reflexes
                FeatureGroup.None,
                elfReq,
                Helpers.CreateAddStatBonus(StatType.SaveReflex, 1, ModifierDescriptor.Trait)));

            // Half-orc:
            // - Brute (persuasion)
            // - Legacy of Sand (+1 will save)
            var brute = Traits.CreateAddStatBonus("BruteTrait", RES.BruteTraitName_info,
                RES.BruteTraitDescription_info,
                "1ee0ce55ace74ccbb798e2fdc13181f6", StatType.SkillPersuasion, halfOrcReq);
            brute.SetIcon(Helpers.GetIcon("885f478dff2e39442a0f64ceea6339c9")); // Intimidating
            choices.Add(brute);

            BlueprintItemWeapon bite = Traits.library.CopyAndAdd<BlueprintItemWeapon>("35dfad6517f401145af54111be04d6cf", "Tusked",
                "44dfad6517f401145af54111be04d644");

            choices.Add(Helpers.CreateFeature("TuskedTrait", RES.TuskedTraitName_info,
                RES.TuskedTraitDescription_info,
                "1511289c92ea4233b14c4f51072ea09g",
                Image2Sprite.Create("Mods/EldritchArcana/sprites/halforc_tusked.png"), // Great Fortitude
                FeatureGroup.None,
                halfOrcReq,
                Helpers.Create<AddAdditionalLimb>(x => x.Weapon = bite)
                ));

            choices.Add(Helpers.CreateFeature("LegacyOfSandTrait", RES.LegacyOfSandTraitName_info,
                RES.LegacyOfSandTraitDescription_info,
                "e5fb1675eb6e4ef9accef7eb3a10862a",
                Image2Sprite.Create("Mods/EldritchArcana/sprites/halforc_legacy_of_sand.png"),
                FeatureGroup.None,
                halfOrcReq,
                Helpers.CreateAddStatBonus(StatType.SaveWill, 1, ModifierDescriptor.Trait)));

            // Half-elf:
            // - Elven Relexes (+2 initiative)
            // - Failed Apprentice (+1 save arcane spells)
            choices.Add(Helpers.CreateFeature("ElvenReflexsTrait", RES.ElvenReflexsTraitName_info,
                RES.ElvenReflexsTraitDescription_info,
                "9975678ce2fc420da9cd6ec4fe8c8b9b",
                Helpers.GetIcon("797f25d709f559546b29e7bcb181cc74"), // Improved Initiative
                FeatureGroup.None,
                halfElfReq,
                Helpers.CreateAddStatBonus(StatType.Initiative, 2, ModifierDescriptor.Trait)));

            choices.Add(Helpers.CreateFeature("FailedAprenticeTrait", RES.FailedAprenticeTraitName_info,
                RES.FailedAprenticeTraitDescription_info,
                "8ed66066751f43c2920055dd6358adc8",
                Helpers.GetIcon("2483a523984f44944a7cf157b21bf79c"), // Elven Immunities
                FeatureGroup.None,
                halfElfReq,
                Helpers.Create<SavingThrowBonusAgainstSpellSource>()));

            // Halfling:
            // - Freed Slave (world)
            // - Freedom Fighter (mobility)
            // - Well-Informed (persuasion)
            choices.Add(Traits.CreateAddStatBonus("FreedSlaveTrait", RES.FreedSlaveTraitName_info,
                RES.FreedSlaveTraitDescription_info,
                "d2fc5fe0c64142a79e0ebee18f14b0be", StatType.SkillKnowledgeWorld, halflingReq));
            choices.Add(Traits.CreateAddStatBonus("FreedomFighterTrait", RES.FreedomFighterTraitName_info,
                RES.FreedomFighterTraitDescription_info,
                "3a4d2cd14dc446319085c865570ccc3d", StatType.SkillMobility, halflingReq));
            choices.Add(Traits.CreateAddStatBonus("WellInformedTrait", RES.WellInformedTraitName_info,
                RES.WellInformedTraitDescription_info,
                "940ced5d41594b9aa22ee22217fbd46f", StatType.SkillPersuasion, halflingReq));

            // Dwarf:
            // - Grounded (+2 mobility, +1 reflex)
            // - Militant Merchant (perception)Owner.HPLeft
            // - Ruthless (+1 confirm crits)
            // - Zest for Battle (+1 trait dmg if has morale attack bonus)

            var GloryOfOld = Helpers.CreateFeature("GloryOfOldTrait", RES.GloryOfOldTraitName_info,
                            RES.GloryOfOldTraitDescription_info,
                            "4283a523984f44944a7cf157b21bf7c9",
                            Image2Sprite.Create("Mods/EldritchArcana/sprites/spell_perfection.png"),
                            FeatureGroup.None,
                            dwarfReq,
                            Helpers.Create<SavingThrowBonusAgainstDescriptor>(s => { s.SpellDescriptor = SpellDescriptor.Poison; s.Value = 1; s.ModifierDescriptor = ModifierDescriptor.Racial; }),
                            Helpers.Create<SavingThrowBonusAgainstDescriptor>(s => { s.SpellDescriptor = SpellDescriptor.BreathWeapon; s.Value = 1; s.ModifierDescriptor = ModifierDescriptor.Trait; }));
            components.Clear();
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
                a.Value = 1;
                a.ModifierDescriptor = ModifierDescriptor.Racial;
            })));

            GloryOfOld.AddComponents(components);

            choices.Add(GloryOfOld);

            choices.Add(Helpers.CreateFeature("GroundedTrait", RES.GroundedTraitName_info,
                RES.GroundedTraitDescription_info,
                "9b13923527a64c3bbf8de904c5a9ef8b",
                Helpers.GetIcon("3a8d34905eae4a74892aae37df3352b9"), // Skill Focus Stealth (mobility)
                FeatureGroup.None,
                dwarfReq,
                Helpers.CreateAddStatBonus(StatType.SkillMobility, 2, ModifierDescriptor.Racial),
                Helpers.CreateAddStatBonus(StatType.SaveReflex, 1, ModifierDescriptor.Racial)));

            choices.Add(Traits.CreateAddStatBonus("MilitantMerchantTrait", RES.MilitantMerchantTraitName_info,
                RES.MilitantMerchantTraitDescription_info,
                "38226f4ad9ed4211878ef95497d01857", StatType.SkillPerception, dwarfReq));

            choices.Add(Helpers.CreateFeature("RuthlessTrait", RES.RuthlessTraitName_info,
                RES.RuthlessTraitDescription_info,
                "58d18289cb7f4ad4a690d9502d397a3a",
                Helpers.GetIcon("f4201c85a991369408740c6888362e20"), // Improved Critical
                FeatureGroup.None,
                dwarfReq,
                Helpers.Create<CriticalConfirmationBonus>(a => { a.Bonus = 1; a.Value = 0; })));


            var Frostborn = Helpers.CreateFeature("FrostbornTrait", RES.FrostbornTraitName_info,
                RES.FrostbornTraitDescription_info,
                "f987f5e69db44cdd99983985e37a6c3c",
                Helpers.GetIcon("121811173a614534e8720d7550aae253"), // Weapon Specialization
                FeatureGroup.None,
                dwarfReq);
            Frostborn.AddComponent(Helpers.Create<AddDamageResistanceEnergy>(r => { r.Type = Kingmaker.Enums.Damage.DamageEnergyType.Cold; r.Value = 4; }));
            Frostborn.AddComponent(Helpers.Create<SavingThrowBonusAgainstDescriptor>(s => { s.SpellDescriptor = SpellDescriptor.Cold; s.ModifierDescriptor = ModifierDescriptor.Racial; s.Bonus = 1; }));
            choices.Add(Frostborn);

            choices.Add(Helpers.CreateFeature("ZestForBattleTrait", RES.ZestForBattleTraitName_info,
                RES.ZestForBattleTraitDescription_info,
                "a987f5e69db44cdd98983985e37a6c2a",
                Helpers.GetIcon("31470b17e8446ae4ea0dacd6c5817d86"), // Weapon Specialization
                FeatureGroup.None,
                dwarfReq,
                Helpers.Create<DamageBonusIfMoraleBonus>()));

            // Gnome:
            // - Animal Friend (+1 will save and lore nature class skill, must have familar or animal companion)
            // - Rapscallion (+1 init, +1 thievery)
            components.Clear();
            components.Add(gnomeReq);
            components.Add(Helpers.Create<AddClassSkill>(a => a.Skill = StatType.SkillLoreNature));
            // TODO: is there a cleaner way to implement this rather than a hard coded list?
            // (Ideally: it should work if a party NPC has a familiar/animal companion too.)
            // See also: PrerequisitePet.
            components.AddRange((new String[] {
                // Animal companions
                "f6f1cdcc404f10c4493dc1e51208fd6f",
                "afb817d80b843cc4fa7b12289e6ebe3d",
                "f9ef7717531f5914a9b6ecacfad63f46",
                "f894e003d31461f48a02f5caec4e3359",
                "e992949eba096644784592dc7f51a5c7",
                "aa92fea676be33d4dafd176d699d7996",
                "2ee2ba60850dd064e8b98bf5c2c946ba",
                "6adc3aab7cde56b40aa189a797254271",
                "ece6bde3dfc76ba4791376428e70621a",
                "126712ef923ab204983d6f107629c895",
                "67a9dc42b15d0954ca4689b13e8dedea",
                // Familiars
                "1cb0b559ca2e31e4d9dc65de012fa82f",
                "791d888c3f87da042a0a4d0f5c43641c",
                "1bbca102706408b4cb97281c984be5d5",
                "f111037444d5b6243bbbeb7fc9056ed3",
                "7ba93e2b888a3bd4ba5795ae001049f8",
                "97dff21a036e80948b07097ad3df2b30",
                "952f342f26e2a27468a7826da426f3e7",
                "61aeb92c176193e48b0c9c50294ab290",
                "5551dd90b1480e84a9caf4c5fd5adf65",
                "adf124729a6e01f4aaf746abbed9901d",
                "4d48365690ea9a746a74d19c31562788",
                "689b16790354c4c4c9b0f671f68d85fc",
                "3c0b706c526e0654b8af90ded235a089",
            }).Select(id => Helpers.Create<AddStatBonusIfHasFact>(a =>
            {
                a.Stat = StatType.SaveWill;
                a.Value = 1;
                a.Descriptor = ModifierDescriptor.Trait;
                a.CheckedFact = Traits.library.Get<BlueprintFeature>(id);
            })));

            choices.Add(Helpers.CreateFeature("AnimalFriendTrait", RES.AnimalFriendTraitName_info,
                RES.AnimalFriendTraitDescription_info,
                "91c612b225d54adaa4ce4c633501b58e",
                Image2Sprite.Create("Mods/EldritchArcana/sprites/gnome_animal_friend.png"),//Helpers.GetIcon("1670990255e4fe948a863bafd5dbda5d"), // Boon Companion
                FeatureGroup.None,
                components.ToArray()));

            choices.Add(Helpers.CreateFeature("RapscallionTrait", RES.RapscallionTraitName_info,
                RES.RapscallionTraitDescription_info,
                "4f95abdcc70e4bda818be5b8860585c5",
                Helpers.GetSkillFocus(StatType.SkillMobility).Icon,
                FeatureGroup.None,
                gnomeReq,
                Helpers.CreateAddStatBonus(StatType.SkillMobility, 1, ModifierDescriptor.Trait),
                Helpers.CreateAddStatBonus(StatType.Initiative, 1, ModifierDescriptor.Trait)));

            // Aasimar:
            // - Martyr's Blood (+1 attack if HP below half).
            // - Toxophilite (+2 crit confirm with bows)
            // - Wary (+1 perception/persuasion)

            // TODO: Enlightened Warrior

            choices.Add(Helpers.CreateFeature("MartyrsBloodTrait", RES.MartyrsBloodTraitName_info,
                RES.MartyrsBloodTraitDescription_info,
                "729d27ad020d485f843264844f0f2155",
                Helpers.GetIcon("3ea2215150a1c8a4a9bfed9d9023903e"), // Iron Will Improved
                FeatureGroup.None,
                aasimarReq,
                Helpers.Create<AttackBonusIfAlignmentAndHealth>(a =>
                {
                    a.TargetAlignment = AlignmentComponent.Evil;
                    a.Descriptor = ModifierDescriptor.Trait;
                    a.Value = 1;
                    a.HitPointPercent = 0.5f;
                })));

            choices.Add(Helpers.CreateFeature("ToxophiliteTrait", RES.ToxophiliteTraitName_info,
                RES.ToxophiliteTraitDescription_info,
                "6c434f07c8984971b1d842cecdf144c6",
                Helpers.GetIcon("f4201c85a991369408740c6888362e20"), // Improved Critical
                FeatureGroup.None,
                aasimarReq,
                Helpers.Create<CriticalConfirmationBonus>(a =>
                {
                    a.Bonus = 2;
                    a.Value = 0;
                    a.CheckWeaponRangeType = true;
                    a.Type = AttackTypeAttackBonus.WeaponRangeType.RangedNormal;
                })));

            choices.Add(Helpers.CreateFeature("WaryTrait", RES.WaryTraitName_info,
                RES.WaryTraitDescription_info,
                "7a72a0e956784cc38ea049e503189810",
                Helpers.GetIcon("86d93a5891d299d4983bdc6ef3987afd"), // Persuasive
                FeatureGroup.None,
                aasimarReq,
                Helpers.CreateAddStatBonus(StatType.SkillPerception, 1, ModifierDescriptor.Trait),
                Helpers.CreateAddStatBonus(StatType.SkillPerception, 1, ModifierDescriptor.Trait)));

            // Tiefling:
            // - Ever Wary (retain half dex bonus AC during surpise round)
            // - Prolong Magic (racial spell-like abilities get free extend spell)
            // - God Scorn (Demodand heritage; +1 saves vs divine spells)
            // - Shadow Stabber (+2 damage if opponent can't see you)

            choices.Add(Helpers.CreateFeature("EverWaryTrait", RES.EverWaryTraitName_info,
                RES.EverWaryTraitDescripton_info,
                "0400c9c99e704a1f81a769aa88044a03",
                Helpers.GetIcon("3c08d842e802c3e4eb19d15496145709"), // uncanny dodge
                FeatureGroup.None,
                tieflingReq,
                Helpers.Create<ACBonusDuringSurpriseRound>()));

            var tieflingHeritageDemodand = Traits.library.Get<BlueprintFeature>("a53d760a364cd90429e16aa1e7048d0a");
            choices.Add(Helpers.CreateFeature("GodScornTrait", RES.GodScornTraitName_info,
                RES.GodScornTraitDescription_info,
                "db41263f6fd3450ea0a3bc45c98330f7",
                Helpers.GetIcon("2483a523984f44944a7cf157b21bf79c"), // Elven Immunities
                FeatureGroup.None,
                Helpers.PrerequisiteFeature(tieflingHeritageDemodand),
                Helpers.Create<SavingThrowBonusAgainstSpellSource>(s => s.Source = SpellSource.Divine)));

            var tieflingHeritageSelection = Traits.library.Get<BlueprintFeatureSelection>("c862fd0e4046d2d4d9702dd60474a181");
            choices.Add(Helpers.CreateFeature("ProlongMagicTrait", RES.ProlongMagicTraitName_info,
                RES.ProlongMagicTraitDescription_info,
                "820f697f59114993a55c46044c98bf9c",
                tieflingHeritageSelection.Icon,
                FeatureGroup.None,
                tieflingReq,
                // TODO: double check that this actually works for SLAs.
                Helpers.Create<AutoMetamagic>(a => { a.Metamagic = Metamagic.Extend; a.Abilities = Traits.CollectTieflingAbilities(tieflingHeritageSelection); })));

            choices.Add(Helpers.CreateFeature("ShadowStabberTrait", RES.ShadowStabberTraitName_info,
                RES.ShadowStabberTraitDescription_info,
                "b67d04e21a9147e3b8f9bd81ba36f409",
                Helpers.GetIcon("9f0187869dc23744292c0e5bb364464e"), // accomplished sneak attacker
                FeatureGroup.None,
                tieflingReq,
                Helpers.Create<DamageBonusIfInvisibleToTarget>(d => d.Bonus = 2)));

            choices.Add(UndoSelection.Feature.Value);
            raceTraits.SetFeatures(choices);
            adopted.SetFeatures(raceTraits.Features);
            adopted.AddComponent(Helpers.PrerequisiteNoFeature(raceTraits));

            return raceTraits;
        }
    }
}