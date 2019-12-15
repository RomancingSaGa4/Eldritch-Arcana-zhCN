
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Items;
using Kingmaker.Blueprints.Items.Weapons;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.UnitLogic.FactLogic;
using System;
using System.Collections.Generic;
using RES = EldritchArcana.Properties.Resources;

namespace EldritchArcana
{
    internal class RegionalTraits
    {
        public static BlueprintFeatureSelection CreateRegionalTraits()
        {
            var noFeature = Helpers.PrerequisiteNoFeature(null);
            var regionalTraits = Helpers.CreateFeatureSelection("RegionalTrait", RES.RegionalTraitName_info,
                RES.RegionalTraitDescription_info,
                "6158dd4ad2544c27bc3a9b48c2e8a2ca", null, FeatureGroup.None, noFeature);
            noFeature.Feature = regionalTraits;

            // TODO: more regional traits.

            // Note: use the generic feat names/text to let players RP this as they choose.
            var choices = new List<BlueprintFeature>();


            var signatureSpell = Helpers.CreateFeatureSelection("SignatureSpellTrait", RES.SignatureSpellTraitName_info,
                RES.SignatureSpellTraitDescription_info,
                "7a3dfe274f45432b85361bdbb0a3009b",
                Helpers.GetIcon("fe9220cdc16e5f444a84d85d5fa8e3d5"), // Spell Specialization Progression
                FeatureGroup.None,
                Helpers.Create<IncreaseCasterLevelForSpell>());
            Traits.FillSpellSelection(signatureSpell, 1, 9, Helpers.Create<IncreaseCasterLevelForSpell>());
            choices.Add(signatureSpell);

            var metamagicApprentice = Helpers.CreateFeatureSelection("MetamagicApprenticeTrait", RES.MetamagicApprenticeTraitName_info,
                RES.MetamagicApprenticeTraitDescription_info,
                "00844f940e434033ab826e5ff5930012",
                Helpers.GetIcon("ee7dc126939e4d9438357fbd5980d459"), // Spell Penetration
                FeatureGroup.None);
            Traits.FillSpellSelection(metamagicApprentice, 1, 3, Helpers.Create<ReduceMetamagicCostForSpell>(r => { r.Reduction = 1; r.MaxSpellLevel = 3; }));
            choices.Add(metamagicApprentice);

            choices.Add(Helpers.CreateFeature("BlightedTrait", RES.BlightedTraitName_info,
                RES.BlightedTraitDescription_info,
                "c50bdfaad65b4028884dd4a74f14e792",
                Image2Sprite.Create("Mods/EldritchArcana/sprites/anatomist.png"),
                FeatureGroup.None,
                Helpers.CreateAddStatBonus(StatType.AC, 1, ModifierDescriptor.NaturalArmor),
                Helpers.Create<FeyFoundlingLogic>(s => { s.dieModefier = -1; s.flatModefier = 0; })));

            choices.Add(Helpers.CreateFeature("WanderlustTrait", RES.WanderlustTraitName_info,
                RES.WanderlustTraitDescription_info,
                "d40bdfaad65b4028884dd4a74f14e793",
                Helpers.NiceIcons(0),
                FeatureGroup.None,
                Helpers.CreateAddStatBonus(StatType.Speed, 10, ModifierDescriptor.Trait)));


            var dagger = Traits.library.Get<BlueprintWeaponType>("07cc1a7fceaee5b42b3e43da960fe76d");

            var riverrat = Traits.CreateAddStatBonus("DaggerboyTrait", RES.DaggerboyTraitName_info,
                RES.DaggerboyTraitDescription_info,
                "e16eb56b2f964321a29976226dccb39f",
                StatType.SkillAthletics // strongman

                );
            //riverrat.Icon = Helpers.NiceIcons(38);
            /*
            var riverratextra = Helpers.CreateFeature("AtleticsTrait", "Swimmer",
                "Your swimming made you athletic",
                "EB6BC4BF90B1433C80878C9D0C81AAED", Helpers.GetSkillFocus(StatType.SkillAthletics).Icon,
                FeatureGroup.None,
                Helpers.Create<AddStartingEquipment>(a =>
                {
                    a.CategoryItems = new WeaponCategory[] { WeaponCategory.Dagger, WeaponCategory.Dagger };
                    a.RestrictedByClass = Array.Empty<BlueprintCharacterClass>();
                    a.BasicItems = Array.Empty<BlueprintItem>();
                }),
                //Helpers.CreateAddStatBonus(StatType.SkillAthletics,1,ModifierDescriptor.Trait),
                //,
                Helpers.Create<WeaponTypeDamageBonus>(a => { a.WeaponType = dagger; a.DamageBonus = 1; })
                //Helpers.Create<WeaponCategoryAttackBonus>(a => { a.Category = WeaponCategory.Dagger; a.AttackBonus = 1; })
                );
                */

            riverrat.AddComponent(Helpers.Create<WeaponTypeDamageBonus>(a => { a.WeaponType = dagger; a.DamageBonus = 1; }));
            riverrat.AddComponent(Helpers.Create<AddStartingEquipment>(a =>
            {
                a.CategoryItems = new WeaponCategory[] { WeaponCategory.Dagger, WeaponCategory.Dagger };
                a.RestrictedByClass = Array.Empty<BlueprintCharacterClass>();
                a.BasicItems = Array.Empty<BlueprintItem>();
            }));
            choices.Add(riverrat);
            //WeaponCategoryAttackBonus

            choices.Add(Helpers.CreateFeature("EmpathicDiplomatTrait", RES.EmpathicDiplomatTraitName_info,
                RES.EmpathicDiplomatTraitDescription_info,
                "a987f5e69db44cdd88983985e37a6d2b",
                Helpers.NiceIcons(999), // Weapon Specialization
                FeatureGroup.None,
                //dwarfReq,
                Helpers.Create<ReplaceBaseStatForStatTypeLogic>(x =>
                {
                    x.StatTypeToReplaceBastStatFor = StatType.SkillPersuasion;
                    x.NewBaseStatType = StatType.Wisdom;
                })));

            var BruisingInt = Traits.CreateAddStatBonus("BruisingIntellectTrait", RES.BruisingIntellectTraitName_info,
                RES.BruisingIntellectTraitDescription_info,
                "b222b5e69db44cdd88983985e37a6d2f",
                StatType.SkillPersuasion
                );

            BruisingInt.AddComponent(Helpers.Create<ReplaceBaseStatForStatTypeLogic>(x =>
            {
                x.StatTypeToReplaceBastStatFor = StatType.SkillPersuasion;
                x.NewBaseStatType = StatType.Intelligence;
            }));

            choices.Add(BruisingInt);

            choices.Add(UndoSelection.Feature.Value);
            regionalTraits.SetFeatures(choices);
            return regionalTraits;
        }
    }
}