using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.UI.Common;
using Kingmaker.UnitLogic.FactLogic;
using System.Collections.Generic;
using RES = EldritchArcana.Properties.Resources;

namespace EldritchArcana
{
    internal class SocialTraits
    {

        public static BlueprintFeatureSelection CreateSocialTraits(out BlueprintFeatureSelection adopted)
        {
            var noFeature = Helpers.PrerequisiteNoFeature(null);
            var socialTraits = Helpers.CreateFeatureSelection("SocialTrait", RES.SocialTraitName_info,
                RES.SocialTraitDescription_info,
                "9e41e60c929e45bc84ded046148c07ec", null, FeatureGroup.None, noFeature);
            noFeature.Feature = socialTraits;
            var choices = new List<BlueprintFeature>();

            // This trait is finished by CreateRaceTraits.
            adopted = Helpers.CreateFeatureSelection("AdoptedTrait", RES.AdoptedTraitName_info,
                RES.AdoptedTraitDescription_info,
                "b4b37968273b4782b29d31c0ca215f41",
                Helpers.GetIcon("26a668c5a8c22354bac67bcd42e09a3f"), // Adaptability
                FeatureGroup.None);

            adopted.IgnorePrerequisites = true;
            adopted.Obligatory = true;
            choices.Add(adopted);

            choices.Add(Traits.CreateAddStatBonus("ChildOfTheStreetsTrait", RES.ChildOfTheStreetsTraitName_info,
                RES.ChildOfTheStreetsTraitDescription_info,
                "a181fd2561134715a04e1b05776ab7a3",
                StatType.SkillThievery));

            choices.Add(Traits.CreateAddStatBonus("FastTalkerTrait", RES.FastTalkerTraitName_info,
                RES.FastTalkerTraitDescription_info,
                "509458a5ded54ecd9a2a4ef5388de2b7",
                StatType.SkillPersuasion));

            //var ArchaeologistCleverExplorer = Traits.library.Get<BlueprintFeature>("1322e50d2b36aba45ab5405db43c53a3");
            
            var performanceResource = Traits.library.Get<BlueprintAbilityResource>("e190ba276831b5c4fa28737e5e49e6a6");
            choices.Add(Helpers.CreateFeature("MaestroOfTheSocietyTrait", RES.MaestroOfTheSocietyTraitName_info,
                RES.MaestroOfTheSocietyTraitDescription_info,
                "847cdf262e4147cda2c670db81852c58",
                Helpers.GetIcon("0d3651b2cb0d89448b112e23214e744e"),
                FeatureGroup.None,
                Helpers.Create<IncreaseResourceAmount>(i => { i.Resource = performanceResource; i.Value = 3; })));

            var gnomeReq = Helpers.PrerequisiteFeature(Helpers.gnome);
            //var performanceResource = Traits.library.Get<BlueprintAbilityResource>("e190ba276831b5c4fa28737e5e49e6a6");
            var MutagenResource = Traits.library.Get<BlueprintAbilityResource>("3b163587f010382408142fc8a97852b6");
            choices.Add(Helpers.CreateFeature("GnomishAlchemistTrait", RES.GnomishAlchemistTraitName_info,
                RES.GnomishAlchemistTraitDescription_info,
                "125cdf262e4147cda2c670db81852c69",
                Helpers.GetIcon("0d3651b2cb0d89448b112e23214e744e"),
                FeatureGroup.None,
                Helpers.Create<IncreaseResourceAmount>(i => { i.Resource = MutagenResource; i.Value = 2; }),
                gnomeReq));


            choices.Add(Traits.CreateAddStatBonus("SuspiciousTrait", RES.SuspiciousTraitName_info,
                RES.SuspiciousTraitDescription_info,
                "2f4e86a9d42547bc85b4c829a47d054c",
                StatType.SkillPerception));

            var AvidReader = Helpers.CreateFeatureSelection("AvidReaderTrait", RES.AvidReaderTraitName_info,
                RES.AvidReaderTraitDescription_info,
                "2e4dcdce32e159cbaf0fb3c641249cbf",
                Image2Sprite.Create("Mods/EldritchArcana/sprites/opposition_research.png"), FeatureGroup.None);

            var AvidReaderOptions = new List<BlueprintFeature>(){
                Helpers.CreateFeature("AvidReaderArcana", UIUtility.GetStatText(StatType.SkillKnowledgeArcana),
                    RES.AvidReaderArcanaDescription_info,
                    $"a932f3e69db44cdd33965985e37a6d2b",
                    Image2Sprite.Create("Mods/EldritchArcana/sprites/spell_perfection.png"),
                    FeatureGroup.None,
                    Helpers.Create<Take10ForSuccessLogic>(t => t.Skill = StatType.SkillKnowledgeArcana)
                  ),Helpers.CreateFeature("AvidReaderWorld", UIUtility.GetStatText(StatType.SkillKnowledgeWorld),
                    RES.AvidReaderWorldDescription_info,
                    $"b254f3e69db44cdd33964985e37a6d1b",
                    Image2Sprite.Create("Mods/EldritchArcana/sprites/opposition_research.png"),
                    FeatureGroup.None,
                    Helpers.Create<Take10ForSuccessLogic>(t => t.Skill = StatType.SkillKnowledgeWorld)
                  ),

            };

            AvidReader.SetFeatures(AvidReaderOptions);
            choices.Add(AvidReader);

            choices.Add(UndoSelection.Feature.Value);
            socialTraits.SetFeatures(choices);
            return socialTraits;
        }
    }
}
 