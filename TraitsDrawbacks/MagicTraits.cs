using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using System.Collections.Generic;
using RES = EldritchArcana.Properties.Resources;

namespace EldritchArcana
{
    internal class MagicTraits
    {
        public static BlueprintFeatureSelection CreateMagicTraits()
        {
            var noFeature = Helpers.PrerequisiteNoFeature(null);
            var magicTraits = Helpers.CreateFeatureSelection("MagicTrait", RES.MagicTraitName_info,
                RES.MagicTraitDescription_info,
                "d89181c607e4431084f9d97532c5c554", null, FeatureGroup.None, noFeature);
            noFeature.Feature = magicTraits;

            var choices = new List<BlueprintFeature>();
            choices.Add(Traits.CreateAddStatBonus("ClassicallySchooledTrait", RES.ClassicallySchooledTraitName_info,
                RES.ClassicallySchooledTraitDescription_info,
                "788098518aa9436782397fa318c64c69",
                StatType.SkillKnowledgeArcana));

            choices.Add(Traits.CreateAddStatBonus("DangerouslyCuriousTrait", RES.DangerouslyCuriousTraitName_info,
                RES.DangerouslyCuriousTraitDescription_info,
                "0c72c573cc404b42916dc7265ea6f59a",
                StatType.SkillUseMagicDevice));


            var WildShapeResource = Traits.library.Get<BlueprintAbilityResource>("ae6af4d58b70a754d868324d1a05eda4");


            choices.Add(Helpers.CreateFeature("BeastOfSocietyTrait", RES.BeastOfSocietyTraitName_info,
                RES.BeastOfSocietyTraitDescription_info,
                "e34889a2dd7e4e9ebfdfa76bfb8f4445",
                Image2Sprite.Create("Mods/EldritchArcana/sprites/beast_of_society.png"),
                FeatureGroup.None,
                WildShapeResource.CreateIncreaseResourceAmount(4)));

            choices.Add(Helpers.CreateFeature("FocusedMindTrait", RES.FocusedMindTraitName_info,
                RES.FocusedMindTraitDescription_info,
                "e34889a2dd7e4e9ebfdfa76bfb8f5556",
                Helpers.GetIcon("06964d468fde1dc4aa71a92ea04d930d"), // Combat Casting
                FeatureGroup.None,
                Helpers.Create<ConcentrationBonus>(a => a.Value = 2)));

            var giftedAdept = Helpers.CreateFeatureSelection("GiftedAdeptTrait", RES.GiftedAdeptTraitName_info,
                RES.GiftedAdeptTraitDescription_info,
                "5eb0b8050ed5466986846cffca0b35b6",
                Helpers.GetIcon("fe9220cdc16e5f444a84d85d5fa8e3d5"), // Spell Specialization Progression
                FeatureGroup.None);
            Traits.FillSpellSelection(giftedAdept, 1, 9, Helpers.Create<IncreaseCasterLevelForSpell>());
            choices.Add(giftedAdept);

            choices.Add(Helpers.CreateFeature("MagicalKnackTrait", RES.MagicalKnackTraitName_info,
                RES.MagicalKnackTraitDescription_info,
                "8fd15d5aa003497aa7f976530d21e430",
                Helpers.GetIcon("16fa59cc9a72a6043b566b49184f53fe"), // Spell Focus
                FeatureGroup.None,
                //Helpers.Create<IncreaseCasterLevel>(),
                Helpers.Create<IncreaseCasterLevelUpToCharacterLevel>()));

            var magicalLineage = Helpers.CreateFeatureSelection("MagicalLineageTrait", RES.MagicalLineageTraitName_info,
                RES.MagicalLineageTraitDescription_info,
                "1785787fb62a4c529104ba53d0de99af",
                Helpers.GetIcon("ee7dc126939e4d9438357fbd5980d459"), // Spell Penetration
                FeatureGroup.None);
            Traits.FillSpellSelection(magicalLineage, 1, 9, Helpers.Create<ReduceMetamagicCostForSpell>(r => r.Reduction = 1));
            choices.Add(magicalLineage);

            choices.Add(Helpers.CreateFeature("PragmaticActivatorTrait", RES.PragmaticActivatorTraitName_info,
                    RES.PragmaticActivatorTraitDescription_info,
                    "d982f3e69db44cdd34263985e37a6d4c",
                    Image2Sprite.Create("Mods/EldritchArcana/sprites/spell_perfection.png"),
                    FeatureGroup.None,
                    Helpers.Create<ReplaceBaseStatForStatTypeLogic>(x =>
                    {
                        x.StatTypeToReplaceBastStatFor = StatType.SkillUseMagicDevice;
                        x.NewBaseStatType = StatType.Intelligence;
                    })
                  ));

            choices.Add(UndoSelection.Feature.Value);
            magicTraits.SetFeatures(choices);
            return magicTraits;

        }
    }
}