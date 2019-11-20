using Kingmaker.Blueprints;
using Kingmaker.Blueprints.Classes;
using Kingmaker.Blueprints.Classes.Selection;
using Kingmaker.Blueprints.Classes.Spells;
using Kingmaker.Designers.Mechanics.Buffs;
using Kingmaker.Designers.Mechanics.Facts;
using Kingmaker.EntitySystem.Stats;
using Kingmaker.Enums;
using Kingmaker.Enums.Damage;
using Kingmaker.UnitLogic.Abilities.Blueprints;
using Kingmaker.UnitLogic.Buffs.Blueprints;
using Kingmaker.UnitLogic.FactLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using RES = EldritchArcana.Properties.Resources;

namespace EldritchArcana
{
    internal class PhysiqueDrawbacks
    {
        public static BlueprintFeatureSelection CreatePhysiqueDrawbacks()
        {
            string[] PhysiqueGuids = new string[100];
            string baseguid = "CB54279F30DA4802833F";
            int x = 0;
            for (long i = 432922691494; i < 432922691544; i++)
            {
                PhysiqueGuids[x] = baseguid + i.ToString();
                x++;
            }

            var noFeature = Helpers.PrerequisiteNoFeature(null);
            var PhysiqueDrawbacks = Helpers.CreateFeatureSelection("PhysiqueDrawback", RES.PhysiqueDrawbackName_info,
                RES.PhysiqueDrawbackDescription_info,
                PhysiqueGuids[0], null, FeatureGroup.None, noFeature);

            noFeature.Feature = PhysiqueDrawbacks;
            var components = new List<BlueprintComponent> { };
            //components.Add(Helpers.Create<ArmorClassBonusAgainstAlignment>(s => { s.alignment= AlignmentComponent.Neutral; s.Value = -2; s.Descriptor = ModifierDescriptor.FearPenalty; }));            
            components.Add(Helpers.Create<ACBonusAgainstWeaponCategory>(w => { w.Category = WeaponCategory.Bite; w.ArmorClassBonus = -2; w.Descriptor = ModifierDescriptor.Penalty; }));
            components.Add(Helpers.Create<ACBonusAgainstWeaponCategory>(w => { w.Category = WeaponCategory.Claw; w.ArmorClassBonus = -2; w.Descriptor = ModifierDescriptor.Penalty; }));
            components.AddRange((new String[] {
                // Animal companions
                "f6f1cdcc404f10c4493dc1e51208fd6f",                "afb817d80b843cc4fa7b12289e6ebe3d",
                "f9ef7717531f5914a9b6ecacfad63f46",                "f894e003d31461f48a02f5caec4e3359",
                "e992949eba096644784592dc7f51a5c7",                "aa92fea676be33d4dafd176d699d7996",
                "2ee2ba60850dd064e8b98bf5c2c946ba",                "6adc3aab7cde56b40aa189a797254271",
                "ece6bde3dfc76ba4791376428e70621a",                "126712ef923ab204983d6f107629c895",
                "67a9dc42b15d0954ca4689b13e8dedea",                // Familiars
                "1cb0b559ca2e31e4d9dc65de012fa82f",                "791d888c3f87da042a0a4d0f5c43641c",
                "1bbca102706408b4cb97281c984be5d5",                "f111037444d5b6243bbbeb7fc9056ed3",
                "7ba93e2b888a3bd4ba5795ae001049f8",                "97dff21a036e80948b07097ad3df2b30",
                "952f342f26e2a27468a7826da426f3e7",                "61aeb92c176193e48b0c9c50294ab290",
                "5551dd90b1480e84a9caf4c5fd5adf65",                "adf124729a6e01f4aaf746abbed9901d",
                "4d48365690ea9a746a74d19c31562788",                "689b16790354c4c4c9b0f671f68d85fc",
                "3c0b706c526e0654b8af90ded235a089",
            }).Select(id => Helpers.Create<AddStatBonusIfHasFact>(a =>
            {
                a.Stat = StatType.AC;
                a.Value = -2;
                a.Descriptor = ModifierDescriptor.Penalty;
                a.CheckedFact = Traits.library.Get<BlueprintFeature>(id);
            })));

            var choices = new List<BlueprintFeature>();
            choices.Add(Helpers.CreateFeature("NatureWardDrawback", RES.PhysiqueDrawbackName_info,
                RES.PhysiqueDrawbackDescription_info,
                PhysiqueGuids[1],
                Helpers.GetIcon("1670990255e4fe948a863bafd5dbda5d"), // Boon Companion
                FeatureGroup.None,
                components.ToArray()));

            var burningHands = Traits.library.Get<BlueprintAbility>("4783c3709a74a794dbe7c8e7e0b1b038");
            choices.Add(Helpers.CreateFeature("BurnedDrawback", RES.BurnedDrawbackName_info,
                RES.BurnedDrawbackDescription_info,
                PhysiqueGuids[2],
                burningHands.Icon,
                FeatureGroup.None,
                Helpers.Create<SavingThrowBonusAgainstDescriptor>(s => { s.SpellDescriptor = SpellDescriptor.Fire; s.Value = -2; s.ModifierDescriptor = ModifierDescriptor.Penalty; })));

            //var burningHands = Traits.library.Get<BlueprintAbility>("4783c3709a74a794dbe7c8e7e0b1b038");
            choices.Add(Helpers.CreateFeature("EntomophobeDrawback", RES.EntomophobeDrawbackName_info,
                RES.EntomophobeDrawbackDescription_info,
                PhysiqueGuids[3],
                Helpers.NiceIcons(8),//spider web//
                FeatureGroup.None,
                Helpers.Create<SwarmAoeVulnerability>(),
                Helpers.Create<SavingThrowBonusAgainstDescriptor>(s => { s.SpellDescriptor = SpellDescriptor.Poison; s.Value = -2; s.ModifierDescriptor = ModifierDescriptor.Penalty; })));

            //Family Ties is a little silly
            choices.Add(Helpers.CreateFeature("OrphanDrawback", RES.OrphanDrawbackName_info,
                RES.OrphanDrawbackDescription_info,
                PhysiqueGuids[4],
                Helpers.NiceIcons(7), // Accomplished Sneak Attacker
                FeatureGroup.None,
                Helpers.CreateAddStatBonus(StatType.SaveWill, -2, ModifierDescriptor.Penalty)));

            choices.Add(Helpers.CreateFeature("FeyTakenDrawback", RES.Fey_takenDrawbackName_info,
                RES.Fey_takenDrawbackDescription_info,
                PhysiqueGuids[5],
                Helpers.GetIcon("2483a523984f44944a7cf157b21bf79c"), // Elven Immunities
                FeatureGroup.None,
                Helpers.Create<SavingThrowBonusAgainstDescriptor>(s => { s.SpellDescriptor = SpellDescriptor.Poison; s.Value = -2; s.ModifierDescriptor = ModifierDescriptor.Penalty; }),
                Helpers.Create<SavingThrowBonusAgainstSchool>(a => { a.School = SpellSchool.Illusion; a.Value = -2; a.ModifierDescriptor = ModifierDescriptor.Penalty; }),
                Helpers.Create<SavingThrowBonusAgainstDescriptor>(s => { s.SpellDescriptor = SpellDescriptor.Disease; s.Value = -2; s.ModifierDescriptor = ModifierDescriptor.Penalty; })));

            //Foul Brand
            //As a child, you were whisked away by mischievous fey for a time. When you returned, you were ever after considered odd and distant. You long to return there, and find the mortal world dull and at times revolting, so you do not eat as you should and do not question strange visions.
            var FoulBrand = Helpers.CreateFeatureSelection("FoulBrandDrawback", RES.FoulBrandDrawbackName,
                RES.FoulBrandDrawbackDescription_info,
                PhysiqueGuids[6],
                burningHands.Icon,
                FeatureGroup.None);

            var BrandedFeatures = new List<BlueprintFeature>()
            {
                Helpers.CreateFeature("LegDrawback", RES.FoulBrandLegDrawbackName_info,
                RES.FoulBrandLegDrawbackDescription_info,
                PhysiqueGuids[7],
                Helpers.NiceIcons(5), // Accomplished Sneak Attacker
                FeatureGroup.None,
                Helpers.CreateAddStatBonus(StatType.Speed, -5, ModifierDescriptor.Penalty))
                ,
                Helpers.CreateFeature("FaceDrawback", RES.FoulBrandFaceDrawbackName_info,
                RES.FoulBrandFaceDrawbackDescription_info,
                PhysiqueGuids[8],
                Helpers.NiceIcons(3), // fear
                FeatureGroup.None,
                Helpers.CreateAddStatBonus(StatType.SkillPersuasion, -2, ModifierDescriptor.Penalty))
                ,
                Helpers.CreateFeature("HandsDrawback", RES.FoulBrandHandsDrawbackName_info,
                RES.FoulBrandHandsDrawbackDescription_info,
                PhysiqueGuids[9],
                Helpers.NiceIcons(13), // Accomplished Sneak Attacker
                FeatureGroup.None,
                Helpers.CreateAddStatBonus(StatType.SkillThievery, -2, ModifierDescriptor.Penalty))
            };

            FoulBrand.SetFeatures(BrandedFeatures);
            choices.Add(FoulBrand);

            var Hedonisticdebuff = Helpers.CreateBuff("HedonisticDeBuff", RES.HedonisticDebuffName_info,
                RES.HedonisticDebuffDescription_info,
                PhysiqueGuids[10],
                Helpers.NiceIcons(7), null,
                Helpers.CreateAddStatBonus(StatType.Strength, -2, ModifierDescriptor.Penalty),
                Helpers.CreateAddStatBonus(StatType.Dexterity, -2, ModifierDescriptor.Penalty));

            choices.Add(Helpers.CreateFeature("HedonisticDrawback", RES.HedonisticDrawbackName_info,
                RES.HedonisticDrawbackDescription_info,
                PhysiqueGuids[11],
                Helpers.NiceIcons(10), // needs sloth icon
                FeatureGroup.None,
                CovetousCurseLogic.Create(Hedonisticdebuff)));

            choices.Add(Helpers.CreateFeature("HelplessDrawback", RES.HelplessDrawbackName_info,
                RES.HelplessDrawbackDescription_info,
                PhysiqueGuids[12],
                Helpers.NiceIcons(3),//spider web//
                FeatureGroup.None,
                Helpers.Create<SavingThrowBonusAgainstDescriptor>(s => { s.SpellDescriptor = SpellDescriptor.Paralysis; s.Value = -2; s.ModifierDescriptor = ModifierDescriptor.Penalty; }),
                Helpers.Create<SavingThrowBonusAgainstDescriptor>(s => { s.SpellDescriptor = SpellDescriptor.Petrified; s.Value = -2; s.ModifierDescriptor = ModifierDescriptor.Penalty; })));

            var UndeadImmunities = Traits.library.Get<BlueprintFeature>("8a75eb16bfff86949a4ddcb3dd2f83ae");
            var UndeadType = Traits.library.Get<BlueprintFeature>("734a29b693e9ec346ba2951b27987e33");
            var Undeadcurse = Traits.library.CopyAndAdd<BlueprintFeature>(
                UndeadType.AssetGuid,
                "UndeadCurseDrawback",
                PhysiqueGuids[13]);

            //UndeadImmunities.SetDescription("undead immunitys");
            //Sprite frailsprite = Image2Sprite.Create("images_sprites/frail.png");
            //Urgathoa
            var UrgathoaFeature = Traits.library.Get<BlueprintFeature>("812f6c07148088e41a9ac94b56ac2fc8");
            var SpellFocus = Traits.library.Get<BlueprintFeature>("16fa59cc9a72a6043b566b49184f53fe");
            var SpellFocusNecromancy = Traits.library.Get<BlueprintFeature>("8791da25011fd1844ad61a3fea6ece54");
            //var AsmodeusFeature = Traits.library.Get<BlueprintFeature>("a3a5ccc9c670e6f4ca4a686d23b89900");

            //Zon - Kuthon.aae911217c5105244bbfddca6a58d77c
            //NorgorberFeature.805b6bdc8c96f4749afc687a003f9628
            //8791da25011fd1844ad61a3fea6ece54


            var CurseOptions = Helpers.CreateFeatureSelection("CurseOptions", RES.CurseDrawbackOptionsName_info,
                RES.CurseDrawbackOptionsDescription_info,
                PhysiqueGuids[14],
                Helpers.NiceIcons(10),
                FeatureGroup.None);
            //Undeadcurse.SetName("you were cursed to be an undead");

            var ElementalWeaknesListFeature = (new DamageEnergyType[] {
                DamageEnergyType.Fire,
                DamageEnergyType.Holy,
                DamageEnergyType.Divine,
            }).Select((element) =>
            Helpers.Create<AddEnergyVulnerability>(a => { a.Type = element; }));

            var RangedWeaponsDebuff = (new WeaponCategory[] {
                WeaponCategory.LightCrossbow,
                WeaponCategory.HeavyCrossbow,
                WeaponCategory.Javelin,
                WeaponCategory.KineticBlast,
                WeaponCategory.Longbow,
                WeaponCategory.Shortbow,
                WeaponCategory.Ray,
                WeaponCategory.Dart,
                WeaponCategory.Shuriken,
                WeaponCategory.ThrowingAxe,
            }).Select((WeapCat) =>
            Helpers.Create<WeaponCategoryAttackBonus>(a => { a.Category = WeapCat; a.AttackBonus = -2; }));

            var RangedWeaponsBuff = (new WeaponCategory[] {
                WeaponCategory.LightCrossbow,
                WeaponCategory.HeavyCrossbow,
                WeaponCategory.Javelin,
                WeaponCategory.KineticBlast,
                WeaponCategory.Longbow,
                WeaponCategory.Shortbow,
                WeaponCategory.Ray,
                WeaponCategory.Dart,
                WeaponCategory.Shuriken,
                WeaponCategory.ThrowingAxe,
            }).Select((WeapCat) =>
            Helpers.Create<WeaponCategoryAttackBonus>(a => { a.Category = WeapCat; a.AttackBonus = 1; }));

            UndeadType.SetNameDescriptionIcon(RES.UndeadCurseDrawbackName_info, RES.UndeadCurseDrawbackDescription_info, Helpers.NiceIcons(44));
            //Undeadcurse.SetNameDescriptionIcon("Undead Curse(inccorrect version)", "This version just changes con and cha the new version also changes necrotic and healing to function correctly this version still exists for save compatibility and its not a big deal.", Helpers.NiceIcons(44));
            //Undeadcurse.AddComponent(Helpers.PrerequisiteFeature(UrgathoaFeature));
            var lijstjelief = new List<BlueprintFeature> { SpellFocus, SpellFocusNecromancy, UrgathoaFeature };
            UndeadType.AddComponent(Helpers.PrerequisiteFeaturesFromList(lijstjelief, any: false));
            UndeadType.AddComponents(ElementalWeaknesListFeature);
            //Undeadcurse.SetFeatures(new List<BlueprintFeature> { UndeadImmunities});
            //Undeadcurse.SetNameDescription("","You where cursed to be an undead");
            foreach (BlueprintComponent bob in UndeadImmunities.GetComponents<BlueprintComponent>())
            {
                Undeadcurse.AddComponent(bob);
            }

            var FrogPolymorphBuff = Traits.library.Get<BlueprintBuff>("662aa00fd6242e643b60ac8336ff39e6");

            var CurseFeatures = new List<BlueprintFeature>()
            {
                //UndeadImmunities,
                //Undeadcurse,
                UndeadType,
                /*
                Helpers.CreateFeature("LycantropyDrawback", "Lycantropy",
                "lycantropy.description" +
                "\nDrawback:???",
                PhysiqueGuids[15],
                Helpers.NiceIcons(3), // fear
                FeatureGroup.None,
                Helpers.CreateAddStatBonus(StatType.SkillPersuasion, -2, ModifierDescriptor.Penalty))
                ,*/
                Helpers.CreateFeature("PolymorphDrawback",  RES.PolymorphDrawbackName_info,
                RES.PolymorphDrawbackDescription_info,
                PhysiqueGuids[16],
                Helpers.NiceIcons(45),
                FeatureGroup.None,
                Helpers.Create<BuffIfHealth>(a =>
                {
                    a.Descriptor = ModifierDescriptor.Penalty;
                    a.Value = -2;
                    a.HitPointPercent = 0.5f;
                }))

            };
            CurseOptions.SetFeatures(CurseFeatures);
            choices.Add(CurseOptions);

            choices.Add(Helpers.CreateFeature("AsthmaticDrawback", RES.AsthmaticDrawbackName_info,
                RES.AsthmaticDrawbackDescription_info,
                PhysiqueGuids[17],
                Helpers.NiceIcons(7),
                FeatureGroup.None,
                Helpers.Create<SavingThrowBonusAgainstDescriptor>(s => { s.SpellDescriptor = SpellDescriptor.Fatigue; s.Value = -2; s.ModifierDescriptor = ModifierDescriptor.Penalty; }),
                Helpers.Create<SavingThrowBonusAgainstDescriptor>(s => { s.SpellDescriptor = SpellDescriptor.Bomb; s.Value = -2; s.ModifierDescriptor = ModifierDescriptor.Penalty; })));

            var Badvision = Helpers.CreateFeatureSelection("BadvisionDrawback", RES.BadvisionDrawbackName_info,
                RES.BadvisionDrawbackDescription_info,
                PhysiqueGuids[18],
                Helpers.NiceIcons(46),
                FeatureGroup.None,
                Helpers.CreateAddStatBonus(StatType.SkillPerception, -1, ModifierDescriptor.Crippled));

            var BadvisionFeatures = new List<BlueprintFeature>()
            {

                Helpers.CreateFeature("BadvisionDrawbackNear", RES.BadvisionDrawbackNearName_info,
                RES.BadvisionDrawbackNearDescription_info,
                PhysiqueGuids[19],
                Helpers.NiceIcons(46),
                FeatureGroup.None
                )
                ,
                Helpers.CreateFeature("BadvisionDrawbackFar", RES.BadvisionDrawbackFarName_info,
                RES.BadvisionDrawbackFarDescription_info,
                PhysiqueGuids[20],
                Helpers.NiceIcons(46),
                FeatureGroup.None,
                Helpers.CreateAddStatBonus(StatType.AdditionalAttackBonus, -1, ModifierDescriptor.Penalty))
                ,
                Helpers.CreateFeature("BadvisionDrawbackBookworm", RES.BadvisionDrawbackBookwormName_info,
                RES.BadvisionDrawbackBookwormDescription_info,
                PhysiqueGuids[21],
                Helpers.NiceIcons(46),
                FeatureGroup.None,
                Helpers.CreateAddStatBonus(StatType.SkillPerception, -1, ModifierDescriptor.Penalty),
                Helpers.Create<ReplaceBaseStatForStatTypeLogic>(v =>
                {
                    v.StatTypeToReplaceBastStatFor = StatType.SkillPerception;
                    v.NewBaseStatType = StatType.Intelligence;
                }))

            };
            BadvisionFeatures[0].AddComponents(RangedWeaponsDebuff);
            BadvisionFeatures[1].AddComponents(RangedWeaponsBuff);

            Badvision.SetFeatures(BadvisionFeatures);
            choices.Add(Badvision);

            choices.Add(Helpers.CreateFeature("MisbegottenDrawback", RES.MisbegottenDrawbackName_info,
                RES.MisbegottenDrawbackDescription_info,
                PhysiqueGuids[22],
                Helpers.NiceIcons(29),
                FeatureGroup.None,
                Helpers.CreateAddStatBonus(StatType.SkillStealth, -2, ModifierDescriptor.Crippled),
                Helpers.CreateAddStatBonus(StatType.SkillMobility, -2, ModifierDescriptor.Crippled),
                Helpers.CreateAddStatBonus(StatType.SkillThievery, -2, ModifierDescriptor.Crippled)));


            //choices.Add(
            //var CultistsVillage_Cultists = Traits.library.Get<BlueprintFaction>("0dd3f77814cc7bf4e9cfb1c96f2a4b4e");
            var LamashtusCurse = Traits.library.Get<BlueprintFeature>("ef3c653365c4a0a46b0d43a44f930186");
            var Occult = Helpers.CreateFeature("OccultBargainDrawback", RES.OccultBargainDrawbackName_info,
                            RES.OccultBargainDrawbackDescription_info,
                            PhysiqueGuids[23],
                            Helpers.NiceIcons(47),
                            FeatureGroup.None,
                            Helpers.Create<ConcentrationBonus>(a => { a.Value = -1; a.CheckFact = true; }),
                            Helpers.Create<ACBonusAgainstFactOwner>(t => { t.CheckedFact = LamashtusCurse; t.Bonus = -2; }));
            //occult.pre
            choices.Add(Occult);

            var feyfeature = Traits.library.Get<BlueprintFeature>("018af8005220ac94a9a4f47b3e9c2b4e");//FeyType.
            choices.Add(Helpers.CreateFeature("SpookedDrawback", RES.SpookedDrawbackName_info,
                RES.SpookedDrawbackDescription_info,
                PhysiqueGuids[24],
                Helpers.NiceIcons(39),
                FeatureGroup.None,
                Helpers.Create<AttackBonusAgainstFactOwner>(a => { a.Bonus = -4; a.CheckedFact = feyfeature; }),
                Helpers.Create<SavingThrowBonusAgainstDescriptor>(f => { f.Bonus = -2; f.SpellDescriptor = SpellDescriptor.Fear; })));

            PhysiqueDrawbacks.SetFeatures(choices);
            return PhysiqueDrawbacks;
        }
    }
}