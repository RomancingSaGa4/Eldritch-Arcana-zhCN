
using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UI.Common;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using System;
using System.Collections.Generic;
using RES = EldritchArcana.Properties.Resources;

namespace EldritchArcana
{
    internal class FaithTraits
    {
        public static BlueprintFeatureSelection CreateFaithTraits()
        {
            var noFeature = Helpers.PrerequisiteNoFeature(null);
            var faithTraits = Helpers.CreateFeatureSelection("FaithTrait", RES.FaithTraitName_info,
                RES.FaithTraitDescription_info,
                "21d0fe2d88e44e5cbfb28becadf86110", null, FeatureGroup.None, noFeature);
            noFeature.Feature = faithTraits;

            var choices = new List<BlueprintFeature>();
            choices.Add(Helpers.CreateFeature("BirthmarkTrait", RES.BirthmarkTraitName_info,
                RES.BirthmarkTraitDescription_info,
                "ebf720b1589d43a2b6cfad26aeda34f9",
                Helpers.GetIcon("2483a523984f44944a7cf157b21bf79c"), // Elven Immunities
                FeatureGroup.None,
                Helpers.Create<SavingThrowBonusAgainstSchool>(a =>
                {
                    a.School = SpellSchool.Enchantment;
                    a.Value = 2;
                    a.ModifierDescriptor = ModifierDescriptor.Trait;
                })));

            choices.Add(Helpers.CreateFeature("DefyMadnessTrait", RES.DefyMadnessTraitName_info,
                RES.DefyMadnessTraitDescription_info,
                "fdc612c0789d43a2b6cfad26aeda34f9",
                Helpers.GetIcon("2483a523984f44944a7cf157b21bf79c"), // Elven Immunities
                FeatureGroup.None,
                Helpers.Create<SavingThrowBonusAgainstDescriptor>(a =>
                {
                    a.SpellDescriptor = SpellDescriptor.MindAffecting;
                    a.Value = 1;
                    a.ModifierDescriptor = ModifierDescriptor.Trait;
                })));

            choices.Add(Traits.CreateAddStatBonus("ChildOfTheTempleTrait", RES.ChildOfTheTempleTraitName_info,
                RES.ChildOfTheTempleTraitDescription_info,
                "cb79816f17d84a51b173ef74aa325561",
                StatType.SkillLoreReligion));

            choices.Add(Traits.CreateAddStatBonus("DevoteeOfTheGreenTrait", RES.DevoteeOfTheGreenTraitName_info,
                RES.DevoteeOfTheGreenTraitDescription_info,
                "6b8e68de9fc04139af0f1127d2a33984",
                StatType.SkillLoreNature));

            choices.Add(Traits.CreateAddStatBonus("EaseOfFaithTrait", RES.EaseOfFaithTraitName_info,
                RES.EaseOfFaithTraitDescription_info,
                "300d727a858d4992a3e01c8165a4c25f",
                StatType.SkillPersuasion));

            var channelEnergyResource = Traits.library.Get<BlueprintAbilityResource>("5e2bba3e07c37be42909a12945c27de7");
            var channelEnergy = Traits.library.Get<BlueprintAbility>("f5fc9a1a2a3c1a946a31b320d1dd31b2");
            var channelEnergyEmpyrealResource = Traits.library.Get<BlueprintAbilityResource>("f9af9354fb8a79649a6e512569387dc5");
            var channelEnergyHospitalerResource = Traits.library.Get<BlueprintAbilityResource>("b0e0c7716ab27c64fb4b131c9845c596");
            choices.Add(Helpers.CreateFeature("ExaltedOfTheSocietyTrait", RES.ExaltedOfTheSocietyTraitName_info,
                RES.ExaltedOfTheSocietyTraitDescription,
                "3bb1b077ad0845b59663c0e1b343011a",
                Helpers.GetIcon("cd9f19775bd9d3343a31a065e93f0c47"), // Extra Channel
                FeatureGroup.None,
                channelEnergyResource.CreateIncreaseResourceAmount(1),
                channelEnergyEmpyrealResource.CreateIncreaseResourceAmount(1),
                channelEnergyHospitalerResource.CreateIncreaseResourceAmount(1),
                LifeMystery.channelResource.CreateIncreaseResourceAmount(1)));

            choices.Add(Helpers.CreateFeature("SacredConduitTrait", RES.SacredConduitTraitName_info,
                RES.SacredConduitTraitDescription_info,
                "bd9c29875bd9d3343a31a065e93f0c28",
                Helpers.GetIcon("cd9f19775bd9d3343a31a065e93f0c47"), // Extra Channel
                FeatureGroup.None,

                Helpers.Create<ReplaceAbilityDC>(r => { r.Ability = channelEnergy; r.Stat = StatType.Wisdom; }),
                //Helpers.Create<ability>(r => { r.Ability = channelEnergy; r.Stat = StatType.Wisdom; }),
                Helpers.Create<IncreaseSpellDescriptorDC>(r => { r.Descriptor = SpellDescriptor.Cure; r.BonusDC = 1; })
                //Helpers.Create<IncreaseSpellDescriptorDC>(r => { r.Descriptor = SpellDescriptor.; r.BonusDC = 1; })
                ));

            choices.Add(Helpers.CreateFeature("FatesFavoredTrait", RES.FatesFavoredTraitName_info,
                RES.FatesFavoredTraitDescription_info,
                "0c5dcccc21e148cdaf0fb3c643249bfb",
                Image2Sprite.Create("Mods/EldritchArcana/sprites/fey_foundling.png"), // blessing luck & resolve
                FeatureGroup.None,
                Helpers.Create<ExtraLuckBonus>()));

            var planar = Helpers.CreateFeatureSelection("PlanarSavantTrait", RES.PlanarSavantTraitName_info,
                RES.PlanarSavantTraitDescription_info,
                "2e4dcecc32e148cbaf0fb3c643249cbf",
                Helpers.NiceIcons(19),
                FeatureGroup.None, Helpers.Create<ReplaceBaseStatForStatTypeLogic>(x =>
                {
                    x.StatTypeToReplaceBastStatFor = StatType.SkillKnowledgeArcana;
                    x.NewBaseStatType = StatType.Wisdom;
                })
                );

            var planarOptions = new List<BlueprintFeature>(){
                Helpers.CreateFeature("PlanarSavantTraitArcana", RES.PlanarSavantTraitArcanaName_info,
                    RES.PlanarSavantTraitArcanaDescription_info,
                    $"a982f3e69db44cdd33963985e37a6d2b",
                    Helpers.NiceIcons(32),
                    FeatureGroup.None,
                    Helpers.Create<ReplaceBaseStatForStatTypeLogic>(x =>
                    {
                        x.StatTypeToReplaceBastStatFor = StatType.SkillKnowledgeArcana;
                        x.NewBaseStatType = StatType.Charisma;
                    })
                  ),Helpers.CreateFeature("PlanarSavantTraitWorld", RES.PlanarSavantTraitWorldName_info,
                    RES.PlanarSavantTraitWorldDescription_info,
                    $"b234f3e69db44cdd33963985e37a6d1b",
                    Helpers.NiceIcons(32),
                    FeatureGroup.None,
                    Helpers.Create<ReplaceBaseStatForStatTypeLogic>(x =>
                    {
                        x.StatTypeToReplaceBastStatFor = StatType.SkillKnowledgeWorld;
                        x.NewBaseStatType = StatType.Charisma;
                    })
                  ),

            };

            planar.SetFeatures(planarOptions);
            choices.Add(planar);

            var WisFlesh = Helpers.CreateFeatureSelection("WisdomintheFleshTrait", RES.WisdomintheFleshTraitName_info,
                RES.WisdomintheFleshTraitDescription_info,
                "1d4dcccc21e148cdaf0fb3c643249cbf",
                Helpers.NiceIcons(43), // wisman
                FeatureGroup.None);

            var WisFleshOptions = new BlueprintFeature[6];
            var icons = new int[] { 0, 1, 24, 2, 25, 6, 22 };
            var OldStats = new StatType[] {
                StatType.Dexterity,
                StatType.Dexterity,
                StatType.Charisma,
                StatType.Charisma,
                StatType.Strength,
                //StatType.Intelligence,
                StatType.Dexterity,
                StatType.Charisma,
            };
            var Stats = new StatType[] {
                StatType.SkillMobility,
                StatType.SkillThievery,
                StatType.SkillUseMagicDevice,
                StatType.CheckIntimidate,
                StatType.SkillAthletics,
                //StatType.SkillKnowledgeWorld,
                StatType.SkillStealth,
                StatType.SkillPersuasion,
            };
            for (int i = 0; i < 6; i++)
            {
                WisFleshOptions[i] = Helpers.CreateFeature($"EmpathicDiplomatTrait{Stats[i]}", 
                    String.Format(RES.EmpathicDiplomatStatTraitName_info, UIUtility.GetStatText(Stats[i])),
                    String.Format(RES.EmpathicDiplomatStatTraitDescription_info, UIUtility.GetStatText(Stats[i]), UIUtility.GetStatText(OldStats[i])),
                    $"a98{i}f{i}e69db44cdd889{i}3985e37a6d2b",
                    Helpers.NiceIcons(i),
                    FeatureGroup.None,
                    Helpers.Create<ReplaceBaseStatForStatTypeLogic>(x =>
                    {
                        x.StatTypeToReplaceBastStatFor = Stats[i];
                        x.NewBaseStatType = StatType.Wisdom;
                    })
                    );
            }
            WisFlesh.SetFeatures(WisFleshOptions);
            choices.Add(WisFlesh);

            choices.Add(Helpers.CreateFeature("IndomitableFaithTrait", RES.IndomitableFaithTraitName_info,
                RES.IndomitableFaithTraitDescription_info,
                "e50acadad65b4028884dd4a74f14e727",
                Helpers.GetIcon("175d1577bb6c9a04baf88eec99c66334"), // Iron Will
                FeatureGroup.None,
                Helpers.CreateAddStatBonus(StatType.SaveWill, 1, ModifierDescriptor.Trait)));


            var LessonResource = Helpers.CreateAbilityResource("ChaldiraResource", "Chaldira charge",
                                             "One charge Chaldira",
                                             "2dc32000b6e056d42a8ecc9921dd43c2",
                                             Helpers.NiceIcons(3),
                                             null
                                             );
            LessonResource.SetFixedResource(0);
            //随机修正
            //int rnd = DateTime.Now.Millisecond % 17 + 3;
            Random ran = new Random();
            int rnd = ran.Next(3, 20);

            var Chaldira = Helpers.CreateFeatureSelection("ChaldiraTrait", RES.ChaldiraTraitName_info,
                RES.ChaldiraTraitDescription,
                "f51acadad65b4028884dd4a74f14e817",
                Helpers.GetIcon("175d1577bb6c9a04baf88eec99c66334"), // Iron Will
                FeatureGroup.None,
                Helpers.CreateAddAbilityResource(LessonResource));

            //
            int len = 5;
            var ChaldiraOptions = new List<BlueprintFeature>(len);

            ChaldiraOptions.Add(Helpers.CreateFeature($"ChaldiraEffectnumber", RES.ChaldiraTraitOriginalName_info,
                RES.ChaldiraTraitOriginalDescription_info,
                $"f53acadad65b4048884dd4a74f14e617",
                Helpers.GetIcon("175d1577bb6c9a04baf88eec99c66334"), // Iron Will
                FeatureGroup.None,
                Helpers.Create<NewMechanics.SavingThrowReroll>(a => { a.Descriptor = ModifierDescriptor.Sacred; a.Value = rnd; a.resource = LessonResource; a.original = true; }),
                LessonResource.CreateIncreaseResourceAmount(1)));

            for (int i = 1; i < len; i++)
            {
                ChaldiraOptions.Add(Helpers.CreateFeature($"ChaldiraEffectnumber{i}", 
                    String.Format(RES.ChaldiraTraitHBName_info, i),
                    String.Format(RES.ChaldiraTraitHBDescription_info, (int)(12 / i), i),
                    $"f5{i}acadad65b40{i}8884dd4a74f14e{i}17",
                    Helpers.GetIcon("175d1577bb6c9a04baf88eec99c66334"), // Iron Will
                    FeatureGroup.None,
                    Helpers.Create<NewMechanics.SavingThrowReroll>(a => { a.Descriptor = ModifierDescriptor.Sacred; a.Value = (int)(12 / i); a.resource = LessonResource; }),
                    LessonResource.CreateIncreaseResourceAmount(i)));
            }

            Chaldira.SetFeatures(ChaldiraOptions);
            choices.Add(Chaldira);

            choices.Add(Traits.CreateAddStatBonus("ScholarOfTheGreatBeyondTrait", RES.ScholarOfTheGreatBeyondTraitName_info,
                RES.ScholarOfTheGreatBeyondTraitDescription_info,
                "0896fea4f7ca4635aa4e5338a673610d",
                StatType.SkillKnowledgeWorld));

            // TODO: Stalwart of the Society

            faithTraits.SetFeatures(choices);
            return faithTraits;
        }
    }
}